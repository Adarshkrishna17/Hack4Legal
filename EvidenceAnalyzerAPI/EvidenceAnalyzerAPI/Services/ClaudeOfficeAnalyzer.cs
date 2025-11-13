using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using EvidenceAnalyzerAPI.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Text;

namespace EvidenceAnalyzerAPI.Services
{
    internal class ClaudeOfficeAnalyzer : IClaudeOfficeAnalyzer
    {
        private readonly IAmazonBedrockRuntime _bedrockClient;

        public ClaudeOfficeAnalyzer()
        {
            _bedrockClient = new AmazonBedrockRuntimeClient(RegionEndpoint.EUWest2);
        }

        public async Task<string> AnalyzeOfficeDocumentAsync(string filePath, string userPrompt)
        {
            if (!File.Exists(filePath))
                return "Error: File not found.";

            string extractedText = ExtractTextFromOffice(filePath);
            if (string.IsNullOrWhiteSpace(extractedText))
                return "No readable text found in document.";

            var payload = new
            {
                anthropic_version = "bedrock-2023-05-31",
                max_tokens = 1500,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = $"{userPrompt}\n\nDocument Content:\n{extractedText}" }
                        }
                    }
                }
            };

            var body = JsonConvert.SerializeObject(payload);
            var request = new InvokeModelRequest
            {
                ModelId = "anthropic.claude-3-sonnet-20240229-v1:0",
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(Encoding.UTF8.GetBytes(body))
            };

            try
            {
                var response = await _bedrockClient.InvokeModelAsync(request);
                using var reader = new StreamReader(response.Body);
                var responseBody = await reader.ReadToEndAsync();
                var json = JObject.Parse(responseBody);
                return json["content"]?[0]?["text"]?.ToString() ?? "No output from Claude.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string ExtractTextFromOffice(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            StringBuilder sb = new();

            try
            {
                switch (ext)
                {
                    case ".docx":
                        using (var doc = WordprocessingDocument.Open(filePath, false))
                            sb.Append(string.Join(" ", doc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().Select(t => t.Text)));
                        break;

                    case ".pptx":
                        using (var ppt = PresentationDocument.Open(filePath, false))
                        {
                            foreach (var slide in ppt.PresentationPart.SlideParts)
                                sb.Append(string.Join(" ", slide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>().Select(t => t.Text)));
                        }
                        break;

                    case ".xlsx":
                        using (var xl = SpreadsheetDocument.Open(filePath, false))
                        {
                            foreach (var sheet in xl.WorkbookPart.WorksheetParts)
                                sb.Append(string.Join(" ", sheet.Worksheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>()
                                    .Select(c => c.CellValue?.Text)));
                        }
                        break;

                    default:
                        return "Unsupported Office format.";
                }
            }
            catch (Exception ex)
            {
                return $"Error extracting text: {ex.Message}";
            }

            return sb.ToString();
        }
    }
}
