using EvidenceAnalyzerAPI.Models;

namespace EvidenceAnalyzerAPI.Interface
{
    public interface IClaudeAudioAnalyzer
    {
        Task<AudioAnalysisResult> AnalyzeAudioAsync(string audioPath);
    }
}
