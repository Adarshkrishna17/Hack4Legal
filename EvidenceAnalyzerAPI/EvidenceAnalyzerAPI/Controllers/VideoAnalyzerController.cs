using EvidenceAnalyzerAPI.Helpers;
using EvidenceAnalyzerAPI.Interface;
using EvidenceAnalyzerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EvidenceAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoAnalyzerController : ControllerBase
    {
        private readonly ILogger<VideoAnalyzerController> _logger;
        private readonly IClaudeImageAnalyzer _analyzer;


        public VideoAnalyzerController(IClaudeImageAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeVideo([FromForm] IFormFile video, [FromForm] string prompt)
        {
            if (video == null || video.Length == 0)
                return BadRequest("No video uploaded.");

            var tempPath = Path.GetTempFileName() + Path.GetExtension(video.FileName);
            using (var stream = new FileStream(tempPath, FileMode.Create))
                await video.CopyToAsync(stream);

            var framesFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(framesFolder);

            FFmpegHelper.ExtractFramesWithFFmpeg(tempPath, framesFolder, fps: 0.5);

            var frameFiles = Directory.GetFiles(framesFolder, "*.jpg").Take(10);
            var summaries = new List<string>();

            foreach (var frame in frameFiles)
            {
                var summary = await _analyzer.AnalyzeImageWithTextAsync(frame, prompt);
                summaries.Add(summary);
            }

            var finalSummary = string.Join(" ", summaries);

            return Ok(new { summary = finalSummary });
        }

        [HttpPost("analyze-structured")]
        public async Task<IActionResult> AnalyzeVideoStructured([FromForm] IFormFile video, [FromForm] string prompt)
        {
            if (video == null || video.Length == 0)
                return BadRequest("No video uploaded.");

            // Save video temporarily
            var tempVideo = Path.GetTempFileName() + Path.GetExtension(video.FileName);
            using (var stream = new FileStream(tempVideo, FileMode.Create))
                await video.CopyToAsync(stream);

            // Extract frames & timestamps
            var outputFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var frames = FFmpegHelper.ExtractFramesWithTimestamps(tempVideo, outputFolder, intervalSeconds: 1);

            var eventList = new List<object>();

            foreach (var (framePath, timestamp) in frames.Take(10)) // limit for Claude
            {
                var framePrompt = $"{prompt}. Describe what is happening around time {timestamp} in this CCTV footage.";
                var description = await _analyzer.AnalyzeImageWithTextAsync(framePath, framePrompt);
                eventList.Add(new { timestamp, description });
            }

            // Summarize entire video
            var joinedSummary = string.Join(" ", eventList.Select(e => ((dynamic)e).description));
            var overallPrompt = $"Based on these scene descriptions, summarize the entire CCTV footage in 3-5 sentences.";
            var overallSummary = await _analyzer.AnalyzeImageWithTextAsync(frames.First().FilePath, overallPrompt);

            return Ok(new
            {
                video_summary = overallSummary,
                events = eventList
            });
        }

    }
}
