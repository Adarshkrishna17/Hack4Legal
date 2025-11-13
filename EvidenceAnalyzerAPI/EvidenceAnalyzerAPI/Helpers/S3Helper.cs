using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EvidenceAnalyzerAPI.Helpers
{
    public static class S3Helper
    {
        public static async Task UploadAsync(string bucketName, string objectKey, string filePath)
        {
            try
            {
                using var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest2);
                var fileTransferUtility = new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(filePath, bucketName, objectKey);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<string> DownloadStringAsync(string bucketName, string objectKey)
        {
             IAmazonS3 _s3Client = new AmazonS3Client(RegionEndpoint.EUWest2);
            var response = await _s3Client.GetObjectAsync(bucketName, objectKey);
            using var reader = new StreamReader(response.ResponseStream);
            return await reader.ReadToEndAsync();
        }
    }
}