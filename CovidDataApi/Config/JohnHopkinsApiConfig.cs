using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidData.Api.Config
{
    public class JohnHopkinsApiConfig
    {
        public string BaseUrl { get; set; } = string.Empty;

        public string BaseUrlAll { get; set; } = string.Empty;

        public string Historical { get; set; } = string.Empty;
    }
}
