using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidData.Api.Response
{
    public class CovidTrackingUsaCurrentResponse
    {
        public decimal? Date { get; set; }

        public decimal? States { get; set; }

        public decimal? Negative { get; set; }

        public decimal? Pending { get; set; }

        public decimal? HospitalizedCurrently { get; set; }

        public decimal? HospitalizedCumulative { get; set; }

        public decimal? InIcuCurrently { get; set; }

        public decimal? InIcuCumulative { get; set; }

        public decimal? OnVentilatorCumulative { get; set; }

        public decimal? OnVentilatorCurrently { get; set; }

        public decimal? Recovered { get; set; }

        public string? DateChecked { get; set; }

        public decimal? Death { get; set; }

        public decimal? Hospitalized { get; set; }

        public decimal? TotalTestResults { get; set; }

        public string? LastModified { get; set; }

        public decimal? Total { get; set; }

        public decimal? PosNeg { get; set; }

        public decimal? DeathIncrease { get; set; }

        public decimal? HospitalizedIncrease { get; set; }

        public decimal? NegativeIncrease { get; set; }

        public decimal? PositiveIncrease { get; set; }

        public decimal? TotalTestResultsIncrease { get; set; }

        public string? Hash { get; set; }


    }
}
