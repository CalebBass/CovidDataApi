using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidData.Api.Response
{
    public class CovidTrackingStateResponse
    {
        public string CheckTimeEt { get; set; } = string.Empty;

        public string DataQualityGrade { get; set; } = string.Empty;

        public string DateModified { get; set; } = string.Empty;
        public string LastUpdateEt { get; set; } = string.Empty;

        public decimal? DeathConfirmed { get; set; }

        public decimal? DeathIncrease { get; set; }

        public decimal? DeathProbable { get; set; }

        public decimal? NegativeTestsAntibody { get; set; }
        public decimal? NegativeTestsPeopleAntibody { get; set; }
        public decimal? NegativeTestsViral { get; set; }
        public decimal? Positive { get; set; }
        public decimal? PositiveTestsAntibody { get; set; }
        public decimal? PositiveTestsAntigen { get; set; }
        public decimal? PositiveTestsPeopleAntibody { get; set; }
        public decimal? PositiveTestsPeopleAntigen { get; set; }
        public decimal? PositiveTestsViral { get; set; }
        public decimal? ProbableCases { get; set; }
        public decimal? TotalTestEncountersViral { get; set; }
        public string? TotalTestResultsSource { get; set; }
        public decimal? TotalTestsAntibody { get; set; }
        public decimal? TotalTestsAntigen { get; set; }
        public decimal? TotalTestsPeopleAntibody { get; set; }
        public decimal? TotalTestsPeopleAntigen { get; set; }
        public decimal? TotalTestsPeopleViral { get; set; }
        public decimal? TotalTestsViral { get; set; }

        public decimal? Date { get; set; }
        public string Fips { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public decimal? Negative { get; set; }

        public decimal? Pending { get; set; }

        public decimal? HospitalizedCurrently { get; set; }

        public decimal? HospitalizedCumulative { get; set; }

        public decimal? InIcuCurrently { get; set; }

        public decimal? InIcuCumulative { get; set; }

        public decimal? OnVentilatorCumulative { get; set; }

        public decimal? OnVentilatorCurrently { get; set; }

        public decimal? Recovered { get; set; }

        public string DateChecked { get; set; } = string.Empty;

        public decimal? Death { get; set; }

        public decimal? Hospitalized { get; set; }

        public decimal? TotalTestResults { get; set; }

        public string LastModified { get; set; } = string.Empty;

        public decimal? Total { get; set; }

        public decimal? PosNeg { get; set; }

        public decimal? HospitalizedIncrease { get; set; }

        public decimal? NegativeIncrease { get; set; }

        public decimal? PositiveIncrease { get; set; }

        public decimal? TotalTestResultsIncrease { get; set; }

        public string Hash { get; set; } = string.Empty;
    }
}
