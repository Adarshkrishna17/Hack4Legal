using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace VideoAnalyzerApp
{
    public static class TranscribeHelper
    {
        public static async Task<string> TranscribeAudioAsync(string bucketName, string key)
        {
            var client = new AmazonTranscribeServiceClient(RegionEndpoint.EUWest2);
            string jobName = "TranscriptionJob_" + Guid.NewGuid();

            try
            {
                await client.StartTranscriptionJobAsync(new StartTranscriptionJobRequest
                {
                    TranscriptionJobName = jobName,
                    Media = new Media { MediaFileUri = $"s3://{bucketName}/{key}" },
                    MediaFormat = "wav",
                    LanguageCode = "en-US"
                });
            }
            catch (Exception ex)
            {
            }
            

            TranscriptionJob job;
            do
            {
                await Task.Delay(5000);
                job = (await client.GetTranscriptionJobAsync(new GetTranscriptionJobRequest
                {
                    TranscriptionJobName = jobName
                })).TranscriptionJob;
            }
            while (job.TranscriptionJobStatus == TranscriptionJobStatus.IN_PROGRESS);

            using var http = new HttpClient();
            var json = await http.GetStringAsync(job.Transcript.TranscriptFileUri);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("results").GetProperty("transcripts")[0].GetProperty("transcript").GetString();
        }
    }
}