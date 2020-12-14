using System;
using System.Linq;
using System.Security.Cryptography;
using CovidData.Api.Config;
using CovidData.Api.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidData.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
//                .AddNewtonsoftJson(options =>
//                {
//                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
//                });
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            services.AddCors();


            services.AddApiVersioning();

//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CovidDataApi", Version = "v1" });
//            });

            // creates external API http clients
            ConfigureHttpClients(services);

            // registers services
            RegisterServices(services);

            RegisterConfigsAsConcreteTypes(services);

            ConfigureJwtValidation(services);

            ConfigureSwaggerGen(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Covid Data API v1.0");
                        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Covid Data API v1.1");
                        c.SwaggerEndpoint("/swagger/v1.2/swagger.json", "Covid Data API v1.2");
                        c.SwaggerEndpoint("/swagger/v1.3/swagger.json", "Covid Data API v1.3");
                    });
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient("CdcApi", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("CdcApi:CdcAllUsaBaseUrl"));
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            
        }

        private void RegisterConfigsAsConcreteTypes(IServiceCollection services)
        {
            services.Configure<CdcApiConfig>(Configuration.GetSection("CdcApi"));
            services.Configure<CovidTrackingProjectApiConfig>(Configuration.GetSection("CovidTrackingProject"));
            services.Configure<JohnHopkinsApiConfig>(Configuration.GetSection("JohnsHopkins"));
            services.Configure<NovelCovidApiConfig>(Configuration.GetSection("NovelCovidApi"));
        }

        private void ConfigureJwtValidation(IServiceCollection services)
        {
            services.AddSingleton<RsaSecurityKey>(provider =>
            {
                // It's required to register the RSA key with dependency injection.
                // If you don't do this, the RSA instance will be prematurely disposed.

                RSA rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(
                    source: Convert.FromBase64String(Configuration.GetValue<string>("JwtAttributes:PublicRsaKey")),
                    bytesRead: out int _
                );

                return new RsaSecurityKey(rsa);
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                SecurityKey rsa = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

                options.IncludeErrorDetails = true; // <- great for debugging

                // Configure the actual Bearer validation
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = rsa,
                    ValidAudience = "PhaseTwo",
                    ValidIssuer = "cbass",
                    RequireSignedTokens = true,
                    RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
                    ValidateLifetime = true, // <- the "exp" will be validated
                    ValidateAudience = true,
                    ValidateIssuer = true,
                };
            });
        }


        private static void ConfigureSwaggerGen(IServiceCollection services)
        {
            services.AddApiVersioning(); //needed for swagger
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1.0",
                        new OpenApiInfo
                        {
                            Title = "CovidData API",
                            Version = "v1.0"
                        });
                    c.SwaggerDoc(
                        "v1.1",
                        new OpenApiInfo
                        {
                            Title = "CovidData API",
                            Version = "v1.1"
                        });
                    c.SwaggerDoc(
                        "v1.2",
                        new OpenApiInfo
                        {
                            Title = "CovidData API",
                            Version = "v1.2"
                        });

                    c.SwaggerDoc(
                        "v1.3",
                        new OpenApiInfo
                        {
                            Title = "CovidData API",
                            Version = "v1.3"
                        });
                    // configure filters
                    c.OperationFilter<RemoveVersionParameterFilter>();
                    c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();

                    // Take API versioning attributes from ASP.NET into account
                    c.DocInclusionPredicate(
                        (version, desc) =>
                        {
                            var versionAttributes = desc.CustomAttributes().OfType<ApiVersionAttribute>()
                                .SelectMany(attr => attr.Versions);
                            var mappingAttributes = desc.CustomAttributes().OfType<MapToApiVersionAttribute>()
                                .SelectMany(attr => attr.Versions).ToArray();
                            return versionAttributes.Any(v => $"v{v}" == version)
                                   && (!mappingAttributes.Any() || mappingAttributes.Any(v => $"v{v}" == version));
                        });
                    c.EnableAnnotations();

                    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                    {
                        Name = "Bearer",
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Description = "Specify the authorization token.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                    };
                    c.AddSecurityDefinition("jwt_auth", securityDefinition);

                    // Make sure swagger UI requires a Bearer token specified
                    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "jwt_auth",
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                    {
                        {securityScheme, new string[] { }},
                    };
                    c.AddSecurityRequirement(securityRequirements);

                    c.IncludeXmlComments(
                        $"{AppDomain.CurrentDomain.BaseDirectory}/SwaggerComments.xml");
                });
        }
    }
}
