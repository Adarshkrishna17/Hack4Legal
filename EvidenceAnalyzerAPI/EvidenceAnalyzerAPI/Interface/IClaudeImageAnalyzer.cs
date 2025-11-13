namespace EvidenceAnalyzerAPI.Interface
{
    public interface IClaudeImageAnalyzer
    {
        Task<string> AnalyzeImageWithTextAsync(string imagePath, string userPrompt);
        Task<List<string>> GenerateVideoFramesAsync(string videoPath, string outputFolder);
    }
}
