using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using EvidenceAnalyzerAPI.Interface;
using Newtonsoft.Json;
using System.Text;

namespace EvidenceAnalyzerAPI.Services
{
    public class ClaudeTextAnalyzer : IClaudeTextAnalyzer
    {
        private readonly AmazonBedrockRuntimeClient _client;

        public ClaudeTextAnalyzer()
        {
            _client = new AmazonBedrockRuntimeClient(RegionEndpoint.USEast1);
        }

        public async Task<string> AnalyzeTextAsync(string prompt)
        {
            var payload = new
            {
                anthropic_version = "bedrock-2023-05-31",
                max_tokens = 4096,
                messages = new[]
                {
                new {
                    role = "user",
                    content = prompt
                }
            }
            };

            string jsonBody = JsonConvert.SerializeObject(payload);

            var request = new InvokeModelRequest
            {
                ModelId = "anthropic.claude-3-sonnet-20240229-v1:0",
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody))
            };

            var response = await _client.InvokeModelAsync(request);

            string responseJson = new StreamReader(response.Body).ReadToEnd();

            // Bedrock Claude returns: { "content": [ { "text": "...." } ] }
            dynamic doc = JsonConvert.DeserializeObject(responseJson);

            return doc?.content?[0]?.text?.ToString();
        }
    }

}
