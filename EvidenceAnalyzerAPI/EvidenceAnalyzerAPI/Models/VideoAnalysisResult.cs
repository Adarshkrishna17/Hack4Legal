namespace EvidenceAnalyzerAPI.Models
{

    public class FrameSummary
    {
        public string Timestamp { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
    }

    public class VideoAnalysisResult
    {
        public string OverallSummary { get; set; }
        public List<FrameSummary> FrameSummaries { get; set; }
    }
}
