namespace EvidenceAnalyzerAPI.Interface
{
    public interface IClaudePDFAnalyzer
    {
        Task<string> AnalyzePDFAsync(string filePath, string userPrompt);
    }

}