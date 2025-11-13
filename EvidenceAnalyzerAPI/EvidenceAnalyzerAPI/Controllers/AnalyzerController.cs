using EvidenceAnalyzerAPI.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/analyzer")]
public class AnalyzerController : ControllerBase
{
    private readonly IClaudeImageAnalyzer _imageAnalyzer;
    private readonly IClaudeVideoAnalyzer _videoAnalyzer;
    private readonly IClaudeAudioAnalyzer _audioAnalyzer;

    public AnalyzerController(IClaudeImageAnalyzer imageAnalyzer, IClaudeVideoAnalyzer videoAnalyzer, IClaudeAudioAnalyzer audioAnalyzer)
    {
        _imageAnalyzer = imageAnalyzer;
        _videoAnalyzer = videoAnalyzer;
        _audioAnalyzer = audioAnalyzer;
    }

    [HttpPost("image")]
    public async Task<IActionResult> AnalyzeImage([FromForm] IFormFile image, [FromForm] string prompt)
    {
        var tempPath = Path.GetTempFileName();
        using (var stream = new FileStream(tempPath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var result = await _imageAnalyzer.AnalyzeImageWithTextAsync(tempPath, prompt);
        return Ok(result);
    }

    [HttpPost("video")]
    public async Task<IActionResult> AnalyzeVideo([FromForm] IFormFile video, [FromForm] string prompt)
    {
        if (video == null || video.Length == 0)
            return BadRequest("Video file is required.");

        string uploadPath = Path.Combine(Path.GetTempPath(), video.FileName);
        using (var stream = new FileStream(uploadPath, FileMode.Create))
        {
            await video.CopyToAsync(stream);
        }

        string outputFolder = Path.Combine(Path.GetTempPath(), "frames_" + Guid.NewGuid());
        Directory.CreateDirectory(outputFolder);

        var result = await _videoAnalyzer.AnalyzeVideoAsync(uploadPath, outputFolder);
        return Ok(result);
    }

    [HttpPost("video/timestamps")]
    public async Task<IActionResult> AnalyzeVideoByTimestamps([FromForm] IFormFile video, [FromForm] string prompt)
    {
        var tempPath = Path.GetTempFileName();
        using (var stream = new FileStream(tempPath, FileMode.Create))
        {
            await video.CopyToAsync(stream);
        }

        var result = await _videoAnalyzer.AnalyzeVideoByTimestampAsync(tempPath, prompt);
        return Ok(result);
    }

    [HttpPost("audio")]
    public async Task<IActionResult> AnalyzeAudio(IFormFile audioFile)
    {
        if (audioFile == null || audioFile.Length == 0)
            return BadRequest("No audio file uploaded.");

        string filePath = Path.Combine(Path.GetTempPath(), audioFile.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await audioFile.CopyToAsync(stream);
        }

        var result = await _audioAnalyzer.AnalyzeAudioAsync(filePath);
        return Ok(result);
    }
}
