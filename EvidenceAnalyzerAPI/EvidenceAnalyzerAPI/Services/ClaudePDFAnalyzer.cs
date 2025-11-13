using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Textract;
using Amazon.Textract.Model;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using EvidenceAnalyzerAPI.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EvidenceAnalyzerAPI.Services
{
    public class ClaudePDFAnalyzer : IClaudePDFAnalyzer
    {
        private readonly IAmazonTextract _textractClient;
        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonBedrockRuntime _bedrockClient;
        private readonly string _bucketName = "mlcs3bucket"; 

        public ClaudePDFAnalyzer()
        {
            _textractClient = new AmazonTextractClient(RegionEndpoint.EUWest2);
            _s3Client = new AmazonS3Client(RegionEndpoint.EUWest2);
            _bedrockClient = new AmazonBedrockRuntimeClient(RegionEndpoint.EUWest2);
        }

        public async Task<string> AnalyzePDFAsync(string filePath, string userPrompt)
        {
            if (!File.Exists(filePath))
                return "Error: PDF file not found.";

            try
            {
                // 1️⃣ Upload file to S3
                string s3Key = $"uploads/{Guid.NewGuid()}_{Path.GetFileName(filePath)}";
                await UploadFileToS3Async(filePath, s3Key);

                // 2️⃣ Start Textract async job
                var startResponse = await _textractClient.StartDocumentAnalysisAsync(new StartDocumentAnalysisRequest
                {
                    DocumentLocation = new DocumentLocation
                    {
                        S3Object = new S3Object
                        {
                            Bucket = _bucketName,
                            Name = s3Key
                        }
                    },
                    FeatureTypes = new List<string> { "TABLES", "FORMS", "LAYOUT" } // can add "SIGNATURES", "QUERIES" later
                });

                string jobId = startResponse.JobId;
                Console.WriteLine($"Textract Job started: {jobId}");

                // 3️⃣ Poll for completion
                GetDocumentAnalysisResponse result;
                do
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    result = await _textractClient.GetDocumentAnalysisAsync(new GetDocumentAnalysisRequest
                    {
                        JobId = jobId
                    });
                    Console.WriteLine($"Job status: {result.JobStatus}");
                } while (result.JobStatus == JobStatus.IN_PROGRESS);

                if (result.JobStatus != JobStatus.SUCCEEDED)
                    return $"Textract job failed with status: {result.JobStatus}";

                // 4️⃣ Retrieve all pages
                var allBlocks = new List<Block>(result.Blocks);
                string nextToken = result.NextToken;

                while (!string.IsNullOrEmpty(nextToken))
                {
                    var nextResponse = await _textractClient.GetDocumentAnalysisAsync(new GetDocumentAnalysisRequest
                    {
                        JobId = jobId,
                        NextToken = nextToken
                    });
                    allBlocks.AddRange(nextResponse.Blocks);
                    nextToken = nextResponse.NextToken;
                }

                // 5️⃣ Extract readable text + structure
                var pageTexts = allBlocks
                    .Where(b => b.BlockType == "LINE" && !string.IsNullOrWhiteSpace(b.Text))
                    .GroupBy(b => b.Page)
                    .Select(g => $"--- Page {g.Key} ---\n" + string.Join("\n", g.Select(b => b.Text)))
                    .ToList();

                string combinedText = string.Join("\n\n", pageTexts);

                // 6️⃣ Send to Claude for analysis
                string prompt = $"{userPrompt}\n\nExtracted content from PDF:\n{combinedText}";

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
                                new { type = "text", text = prompt }
                            }
                        }
                    }
                };

                var body = JsonConvert.SerializeObject(payload);
                var response = await _bedrockClient.InvokeModelAsync(new InvokeModelRequest
                {
                    ModelId = "anthropic.claude-3-sonnet-20240229-v1:0",
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = new MemoryStream(Encoding.UTF8.GetBytes(body))
                });

                using var reader = new StreamReader(response.Body);
                var responseBody = await reader.ReadToEndAsync();
                var json = JObject.Parse(responseBody);
                var completion = json["content"]?[0]?["text"]?.ToString();

                return completion ?? "No response from Claude.";
            }
            catch (Exception ex)
            {
                return $"Error analyzing PDF: {ex.Message}";
            }
        }

        private async Task UploadFileToS3Async(string filePath, string key)
        {
            var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(filePath, _bucketName, key);
        }
    }
}
