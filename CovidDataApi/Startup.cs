using System;
using CovidData.Api.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CovidDataApi", Version = "v1" });
            });

            // creates external API http clients
            ConfigureHttpClients(services);

            // registers services
            RegisterServices(services);

            RegisterConfigsAsConcreteTypes(services);

//            services.AddAuthentication(options =>
//                {
//                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                })
//                .AddJwtBearer(options =>
//                {
//                    SecurityKey rsa = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();
//
//                    options.IncludeErrorDetails = true; // good for debugging
//
//                    // jwt bearer validation properties
//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        IssuerSigningKey = rsa,
//                        ValidAudience = "PhaseTwo",
//                        ValidIssuer = "cbass",
//                        RequireSignedTokens = true,
//                        RequireExpirationTime = true,
//                        ValidateLifetime = true,
//                        ValidateAudience = true,
//                        ValidateIssuer = true
//                    };
//                });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidDataApi v1"));
            }

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
                c.DefaultRequestHeaders.Add("asdfasd", "asdf");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
//            services.AddSingleton<RsaSecurityKey>(options =>
//            {
//                RSA rsa = RSA.Create();
//                rsa.ImportRSAPublicKey(
//                    source: Convert.FromBase64String(Configuration.GetValue<string>("JwtAttributes:PublicRsaKey")),
//                    bytesRead: out int _
//                );
//
//                return new RsaSecurityKey(rsa);
//            });
        }

        private void RegisterConfigsAsConcreteTypes(IServiceCollection services)
        {
            services.Configure<CdcApiConfig>(Configuration.GetSection("CdcApi"));
            services.Configure<CovidTrackingProjectApiConfig>(Configuration.GetSection("CovidTrackingProject"));
            services.Configure<JohnHopkinsApiConfig>(Configuration.GetSection("JohnsHopkins"));
            services.Configure<NovelCovidApiConfig>(Configuration.GetSection("NovelCovidApi"));
        }
    }
}
