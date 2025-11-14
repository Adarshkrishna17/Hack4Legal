namespace EvidenceAnalyzerAPI.Interface
{
    public interface IClaudeTextAnalyzer
    {
        Task<string> AnalyzeTextAsync(string prompt);
    }
}
