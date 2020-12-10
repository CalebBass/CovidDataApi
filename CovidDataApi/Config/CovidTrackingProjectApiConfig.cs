using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidData.Api.Config
{
    public class CovidTrackingProjectApiConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string UsaBaseUrl { get; set; } = string.Empty;
        public string UsaHistoricalBaseUrl { get; set; } = string.Empty;
        public string UsaHistoricalDaily { get; set; } = string.Empty;
        public string UsaStatesBaseUrl { get; set; } = string.Empty;
    }
}
