using Amazon;
using Amazon.S3;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using EvidenceAnalyzerAPI.Helpers;
using EvidenceAnalyzerAPI.Interface;
using EvidenceAnalyzerAPI.Models;

namespace EvidenceAnalyzerAPI.Services
{
    internal class ClaudeAudioAnalyzer : IClaudeAudioAnalyzer
    {
        private readonly ClaudeImageAnalyzer _claude;
        private readonly AmazonTranscribeServiceClient _transcribeClient;
        private readonly IAmazonS3 _s3Client;

        public ClaudeAudioAnalyzer(ClaudeImageAnalyzer claude)
        {
            _claude = claude;
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
                await Task.Delay(5000);
                var status = await _transcribeClient.GetTranscriptionJobAsync(
                    new GetTranscriptionJobRequest { TranscriptionJobName = jobName });
                job = status.TranscriptionJob;
            } while (job.TranscriptionJobStatus == TranscriptionJobStatus.IN_PROGRESS);

            if (job.TranscriptionJobStatus != TranscriptionJobStatus.COMPLETED)
                throw new Exception($"Transcription failed: {job.FailureReason}");

            
            string objectKeyJson = $"{jobName}.json";
            string transcriptText = await S3Helper.DownloadStringAsync(bucketName, objectKeyJson);

            
            
            string prompt = "Summarize this audio transcription:\n" + transcriptText;
            string summary = await _claude.AnalyzeImageWithTextAsync(null, prompt);

            return new AudioAnalysisResult
            {
                Transcription = transcriptText,
                Summary = summary
            };
        }
    }
}
