namespace EvidenceAnalyzerAPI.Interface
{
    public interface IClaudeOfficeAnalyzer
    {
        Task<string> AnalyzeOfficeDocumentAsync(string filePath, string userPrompt);
    }
}