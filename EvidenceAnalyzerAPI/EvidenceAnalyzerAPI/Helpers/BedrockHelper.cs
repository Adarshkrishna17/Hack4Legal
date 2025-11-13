using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon.Runtime;
using Amazon;
using System;

namespace EvidenceAnalyzerAPI.Helpers
{
    public static class BedrockHelper
    {
        public static async Task<string> SummarizeTranscriptAsync(string transcript)
        {
            var client = new AmazonBedrockRuntimeClient(RegionEndpoint.EUWest2);
            var body = new
            {
                prompt = $"\n\nHuman:Summarize the following transcript:\n{transcript} \n\nAssistant:",
                max_tokens_to_sample = 300,
                temperature = 0.5
            };

            var request = new InvokeModelRequest
            {
                ModelId = "anthropic.claude-v2",
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(body)))
            };

            try
            {
                var response = await client.InvokeModelAsync(request);
                using var reader = new StreamReader(response.Body);
                var json = await reader.ReadToEndAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("completion").GetString();
            }
            catch(Exception  ex)
            {
                return string.Empty;
            }
           
        }
    }
}