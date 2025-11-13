using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime;
using EvidenceAnalyzerAPI.Helpers;
using EvidenceAnalyzerAPI.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceAnalyzerAPI.Services
{
    internal class ClaudeImageAnalyzer : IClaudeImageAnalyzer
    {
        private readonly IAmazonBedrockRuntime _bedrockClient;

        public ClaudeImageAnalyzer(IAmazonBedrockRuntime bedrockClient)
        {
            _bedrockClient = bedrockClient;
        }

        public ClaudeImageAnalyzer()
        {
            _bedrockClient = new AmazonBedrockRuntimeClient(RegionEndpoint.EUWest2);
        }

        public async Task<string> AnalyzeImageWithTextAsync(string imagePath, string userPrompt)
        {
            if (!File.Exists(imagePath)) return ""; 
            var imageBytes = File.ReadAllBytes(imagePath);
            string base64Image = Convert.ToBase64String(imageBytes);
            
            var payload = new
            {
                anthropic_version = "bedrock-2023-05-31",
                max_tokens = 1000,
                messages = new object[]
                {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "image",
                            source = new
                            {
                                type = "base64",
                                media_type = "image/jpeg",
                                data = base64Image
                            }
                        },
                        new
                        {
                            type = "text",
                            text = userPrompt
                        }
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

                // Extract Claude's reply from JSON
                var json = JObject.Parse(responseBody);
                var completion = json["content"]?[0]?["text"]?.ToString();
                return completion ?? "No response text found.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public async Task<List<string>> GenerateVideoFramesAsync(string videoPath, string outputFolder)
        {
            FFmpegHelper.ExtractFramesWithTimestamps(videoPath, outputFolder,intervalSeconds: 1);
            var frames = Directory.GetFiles(outputFolder, "*.jpg");
            return frames.ToList();
        }


    }
}
