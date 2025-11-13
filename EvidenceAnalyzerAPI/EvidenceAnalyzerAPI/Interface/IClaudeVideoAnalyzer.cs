using EvidenceAnalyzerAPI.Models;

namespace EvidenceAnalyzerAPI.Interface
{
    public interface IClaudeVideoAnalyzer
    {
        Task<VideoAnalysisResult> AnalyzeVideoAsync(string videoPath, string outputFolder);
        Task<VideoAnalysisResult> AnalyzeVideoByTimestampAsync(string videoPath, string outputFolder);
    }
}
