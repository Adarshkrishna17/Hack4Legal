using EvidenceAnalyzerAPI.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/analyzer")]
public class AnalyzerController : ControllerBase
{
    private readonly IClaudeImageAnalyzer _imageAnalyzer;
    private readonly IClaudeVideoAnalyzer _videoAnalyzer;
    private readonly IClaudeAudioAnalyzer _audioAnalyzer;
    private readonly IClaudePDFAnalyzer _pdfAnalyzer;
    private readonly IClaudeOfficeAnalyzer _officeAnalyzer;

    public AnalyzerController(IClaudeImageAnalyzer imageAnalyzer, 
        IClaudeVideoAnalyzer videoAnalyzer, 
        IClaudeAudioAnalyzer audioAnalyzer, 
        IClaudePDFAnalyzer pdfAnalyzer,
        IClaudeOfficeAnalyzer officeAnalyzer
        )
    {
        _imageAnalyzer = imageAnalyzer;
        _videoAnalyzer = videoAnalyzer;
        _audioAnalyzer = audioAnalyzer;
        _pdfAnalyzer = pdfAnalyzer;
        _officeAnalyzer = officeAnalyzer;

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

    [HttpPost("file")]
    public async Task<IActionResult> AnalyzeAnyFile([FromForm] IFormFile file, [FromForm] string prompt)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        string filePath = Path.Combine(Path.GetTempPath(), file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string result;

        try
        {
            if (extension == ".pdf")
            {
                result = await _pdfAnalyzer.AnalyzePDFAsync(filePath, prompt);
            }
            else if (extension == ".doc" || extension == ".docx" ||
                     extension == ".xls" || extension == ".xlsx" ||
                     extension == ".ppt" || extension == ".pptx")
            {
                result = await _officeAnalyzer.AnalyzeOfficeDocumentAsync(filePath, prompt);
            }
            else
            {
                return BadRequest("Unsupported file type. Please upload a PDF, Word, Excel, or PowerPoint file.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error analyzing file: {ex.Message}");
        }
        finally
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }

}
