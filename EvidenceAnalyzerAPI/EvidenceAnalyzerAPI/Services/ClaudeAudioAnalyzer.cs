using Amazon;
using Amazon.S3;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using EvidenceAnalyzerAPI.Helpers;
using EvidenceAnalyzerAPI.Interface;
using EvidenceAnalyzerAPI.Models;
using Newtonsoft.Json;

namespace EvidenceAnalyzerAPI.Services
{
    internal class ClaudeAudioAnalyzer : IClaudeAudioAnalyzer
    {
        private readonly ClaudeImageAnalyzer _claude;
        private readonly AmazonTranscribeServiceClient _transcribeClient;
        private readonly IAmazonS3 _s3Client;
        private readonly IClaudeTextAnalyzer _textAnalyzer;

        public ClaudeAudioAnalyzer(IClaudeTextAnalyzer textAnalyzer)
        {
            _textAnalyzer = textAnalyzer;
            var region = RegionEndpoint.EUWest2;
            _s3Client = new AmazonS3Client(region);
            _transcribeClient = new AmazonTranscribeServiceClient(region);
        }

        public async Task<AudioAnalysisResult> AnalyzeAudioAsync(string audioPath)
        {
            string bucketName = "mlcs3bucket";
            string objectKey = Path.GetFileName(audioPath);

            await S3Helper.UploadAsync(bucketName, objectKey, audioPath);

            string jobName = "transcribe-" + Guid.NewGuid();
            var startReq = new StartTranscriptionJobRequest
            {
                TranscriptionJobName = jobName,
                LanguageCode = LanguageCode.EnUS,
                Media = new Media { MediaFileUri = $"s3://{bucketName}/{objectKey}" },
                OutputBucketName = bucketName
            };

            await _transcribeClient.StartTranscriptionJobAsync(startReq);

            TranscriptionJob job;
            do
            {
                await Task.Delay(4000);
                var status = await _transcribeClient.GetTranscriptionJobAsync(
                    new GetTranscriptionJobRequest { TranscriptionJobName = jobName });
                job = status.TranscriptionJob;
            }
            while (job.TranscriptionJobStatus == TranscriptionJobStatus.IN_PROGRESS);

            if (job.TranscriptionJobStatus != TranscriptionJobStatus.COMPLETED)
                throw new Exception($"Transcription failed: {job.FailureReason}");

            string objectKeyJson = $"{jobName}.json";
            string transcriptText = await S3Helper.DownloadStringAsync(bucketName, objectKeyJson);
            string actualText = ExtractTranscript(transcriptText);

            string prompt = $@"
You are a senior AI audio-content analyst. 
You MUST return **only valid JSON**, following the schema **exactly**.

Analyze the transcription deeply for:
• Meaningful insights  
• Emotions & tone  
• Themes / context  
• Red flags / risks  
• Actionable recommendations  
• Speaker intent  
• Any patterns in behavior or communication  

     IMPORTANT:
- No prose outside JSON  
- No markdown  
- No explanations  
- Do NOT repeat the input  
- Keep it concise but insightful  

 OUTPUT JSON FORMAT:
{{
  ""summary"": ""High-level 4–6 line summary of the content."",
  ""keyInsights"": [
      ""Insight 1"",
      ""Insight 2"",
      ""Insight 3""
  ],
  ""sentiment"": {{
      ""overall"": ""Positive | Neutral | Negative | Mixed"",
      ""tones"": [ ""motivational"", ""reflective"", ""informative"" ]
  }},
  ""themes"": [
      ""theme1"",
      ""theme2"",
      ""theme3""
  ],
  ""risksOrConcerns"": [
      ""Possible risk / issue if applicable"",
      ""Leave empty array if none""
  ],
  ""actionItems"": [
      ""Actionable next steps derived from content"",
      ""Clear suggestions based on speech""
  ],
  ""recommendations"": [
      ""Improvement suggestion 1"",
      ""Improvement suggestion 2""
  ]
}}

TRANSCRIPTION:
{actualText}
";

            string claudeResponse = await _textAnalyzer.AnalyzeTextAsync(prompt);

            if (claudeResponse == null)
                throw new Exception("Claude returned NULL. Check your API model call.");

            string cleanJson = ExtractJson(claudeResponse);

            var result = JsonConvert.DeserializeObject<AudioAnalysisResult>(cleanJson);
            result.Transcription = actualText;

            return result;
        }

        string ExtractJson(string input)
        {
            int start = input.IndexOf('{');
            int end = input.LastIndexOf('}');

            if (start == -1 || end == -1 || end <= start)
                throw new Exception("Could not extract JSON from Claude response: " + input);

            return input.Substring(start, end - start + 1);
        }
        private string ExtractTranscript(string json)
        {
            dynamic root = JsonConvert.DeserializeObject(json);
            return root.results.transcripts[0].transcript;
        }
    }
}
