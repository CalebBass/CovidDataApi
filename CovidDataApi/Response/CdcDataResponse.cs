namespace CovidData.Api.Response
{
    public class CdcDataResponse
    {
        public string? ConfirmedCases { get; set; }

        public string? ConfirmedDeaths { get; set; }

        public string? CreatedAt { get; set; }

        public string? NewCases { get; set; }

        public string? NewDeaths { get; set; }

        public string? ProbableNewCases { get; set; }

        public string? ProbableNewDeaths { get; set; }

        public string? ProbableCases { get; set; }

        public string? ProbableDeaths { get; set; }

        public string? State { get; set; }

        public string? SubmissionDate { get; set; }

        public string? TotalCases { get; set; }

        public string? TotalDeaths { get; set; }
    }
}
