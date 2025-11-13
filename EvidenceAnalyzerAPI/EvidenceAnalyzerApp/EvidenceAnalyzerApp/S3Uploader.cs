using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VideoAnalyzerApp
{
    public static class S3Uploader
    {
        public static async Task UploadAsync(string filePath, string bucketName)
        {
            var client = new AmazonS3Client(RegionEndpoint.EUWest2);
            var transferUtility = new TransferUtility(client);
            try
            {
                await transferUtility.UploadAsync(filePath, bucketName);
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}