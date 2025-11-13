using EvidenceAnalyzerAPI.Helpers;
using EvidenceAnalyzerAPI.Interface;
using EvidenceAnalyzerAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EvidenceAnalyzerAPI.Services
{
    internal class ClaudeVideoAnalyzer : IClaudeVideoAnalyzer
    {
        private readonly IClaudeImageAnalyzer _imageAnalyzer;

        public ClaudeVideoAnalyzer(IClaudeImageAnalyzer imageAnalyzer)
        {
            _imageAnalyzer = imageAnalyzer;
        }

        public async Task<VideoAnalysisResult> AnalyzeVideoAsync(string videoPath, string outputFolder)
        {
            // 1️⃣ Extract frames
            var frames = FFmpegHelper.ExtractFramesWithTimestamps(videoPath, outputFolder,intervalSeconds: 1); // implement ExtractFrames()

            var frameSummaries = new List<FrameSummary>();

            // 2️⃣ Loop through frames
            foreach (var (timestamp, framePath) in frames)
            {
                var summary = await _imageAnalyzer.AnalyzeImageWithTextAsync(framePath, "Describe what's happening in this frame");
                frameSummaries.Add(new FrameSummary
                {
                    Timestamp = timestamp.ToString(@"hh\:mm\:ss"),
                    ImageUrl = Path.GetFileName(framePath),
                    Summary = summary
                });
            }

            // 3️⃣ Generate overall summary using combined frame summaries
            var combinedText = string.Join("\n", frameSummaries.Select(f => $"{f.Timestamp}: {f.Summary}"));
            var overallPrompt = "Summarize the following scene descriptions into a concise video summary:\n" + combinedText;
            var overallSummary = await _imageAnalyzer.AnalyzeImageWithTextAsync(frames.First().FramePath, overallPrompt);

            return new VideoAnalysisResult
            {
                OverallSummary = overallSummary,
                FrameSummaries = frameSummaries
            };
        }

        public async Task<VideoAnalysisResult> AnalyzeVideoByTimestampAsync(string videoPath, string outputFolder)
        {
            // You can reuse AnalyzeVideoAsync for now
            return await AnalyzeVideoAsync(videoPath, outputFolder);
        }
    }
}
