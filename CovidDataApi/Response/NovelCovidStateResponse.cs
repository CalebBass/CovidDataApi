using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidData.Api.Response
{
    public class NovelCovidStateResponse
    {
        public string State { get; set; } = string.Empty;

        public decimal? Cases { get; set; }
        public decimal? TodayCases { get; set; }
        public decimal? TodayDeaths { get; set; }
        public decimal? Active { get; set; }
        public decimal? Tests { get; set; }
        public decimal? TestsPerOneMillion { get; set; }
    }
}
