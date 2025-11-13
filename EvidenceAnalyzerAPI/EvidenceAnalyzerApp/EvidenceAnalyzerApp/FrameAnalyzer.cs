using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceAnalyzerApp
{
    public class FrameAnalyzer
    {
        private readonly AmazonRekognitionClient _client;

        public FrameAnalyzer()
        {
            _client = new AmazonRekognitionClient(RegionEndpoint.EUWest2);
        }

        public async Task<List<string>> AnalyzeFrameAsync(string imagePath)
        {
            // Initialize IdentifiedObjects for returning list of objects
            var identifiedObjects = new List<string>();
            try
            {
                // Read the original image from the file
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

                // Resize and compress the image to fit under 5 MB
                byte[] resizedImageBytes = ResizeAndCompressImage(imageBytes);

                // Create a memory stream from the resized image
                using var memoryStream = new MemoryStream(resizedImageBytes);

                // Create the image for Rekognition
                var image = new Amazon.Rekognition.Model.Image { Bytes = memoryStream };

                // Set up the DetectLabels request for Rekognition
                var request = new DetectLabelsRequest
                {
                    Image = image,
                    MaxLabels = 10,          // Adjust the number of labels to return
                    MinConfidence = 75F      // Minimum confidence for labels
                };

                // Send the request to Rekognition
                var response = await _client.DetectLabelsAsync(request);
                foreach (var label in response.Labels)
                {
                    if (label.Confidence > 99.98)
                    {
                        identifiedObjects.Add(label.Name);
                    }                    
                }

                return identifiedObjects;
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the process
                return identifiedObjects;
            }
        }

        // Resize and compress the image to fit under 5 MB
        private byte[] ResizeAndCompressImage(byte[] imageBytes, int maxWidth = 1024, int maxHeight = 1024)
        {
            using (var inputStream = new MemoryStream(imageBytes))
            {
                using (var image = System.Drawing.Image.FromStream(inputStream))
                {
                    // Calculate new dimensions while maintaining aspect ratio
                    int newWidth = image.Width;
                    int newHeight = image.Height;

                    float aspectRatio = (float)image.Width / image.Height;
                    if (image.Width > maxWidth || image.Height > maxHeight)
                    {
                        if (aspectRatio > 1)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)(maxWidth / aspectRatio);
                        }
                        else
                        {
                            newHeight = maxHeight;
                            newWidth = (int)(maxHeight * aspectRatio);
                        }
                    }

                    // Create a new Bitmap with the resized dimensions using the correct constructor
                    using (var resizedImage = new Bitmap(image, new Size(newWidth, newHeight)))
                    {
                        using (var outputStream = new MemoryStream())
                        {
                            // Save the resized image to the MemoryStream as PNG (lossless)
                            resizedImage.Save(outputStream, System.Drawing.Imaging.ImageFormat.Png);

                            // Check if the image is under 5 MB, otherwise keep resizing until it's under the limit
                            byte[] resizedImageBytes = outputStream.ToArray();
                            while (resizedImageBytes.Length > 5242880)  // 5 MB limit
                            {
                                // If the image is still too large, reduce the dimensions further
                                newWidth = (int)(newWidth * 0.9); // Reduce by 10%
                                newHeight = (int)(newHeight * 0.9); // Reduce by 10%

                                using (var resizedAgain = new Bitmap(image, new Size(newWidth, newHeight)))
                                {
                                    outputStream.SetLength(0);  // Clear the output stream
                                    resizedAgain.Save(outputStream, System.Drawing.Imaging.ImageFormat.Png);
                                }

                                resizedImageBytes = outputStream.ToArray();
                            }

                            return resizedImageBytes;
                        }
                    }
                }
            }
        }
    }
}
