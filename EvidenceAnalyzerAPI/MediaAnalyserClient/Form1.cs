using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaAnalyserClient
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(10) 
        };


        // Define your API endpoints
        private const string ImageEndpoint = "http://localhost:5224/api/analyzer/image";
        private const string VideoEndpoint = "http://localhost:5224/api/analyzer/Video/timestamps";
        private const string AudioEndpoint = "http://localhost:5224/api/analyzer/audio";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select Media File";
                dlg.Filter = "Media Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.mp4;*.avi;*.mov;*.mkv;*.mp3;*.wav;*.aac;*.flac|All Files|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = dlg.FileName;
                    lblStatus.Text = $"Selected: {Path.GetFileName(dlg.FileName)}";
                }
            }
        }

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            txtResponse.Clear();
            lblStatus.Text = "";

            string filePath = txtFilePath.Text.Trim();
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            string mediaType = GetEndpointByExtension(extension);
            if (mediaType == null)
            {
                MessageBox.Show("Invalid media file type.", "Unsupported Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            


            

            lblStatus.Text = "Uploading and analyzing... Please wait.";

            try
            {
                string result = await UploadFileAsync(mediaType, filePath);
                txtResponse.Text = result;
                switch (mediaType)
                {
                    case "image":
                        break;

                    case "video":
                        var analysis = JsonConvert.DeserializeObject<VideoAnalysis>(result);
                        if (analysis != null)
                        {
                            txtResponse.Text = analysis.OverallSummary;
                            dgvFrames.DataSource = analysis.FrameSummaries;
                        }
                        break;

                    case "audio":
                        
                        break;

                    default:
                        throw new Exception("Unknown media type");
                }

               
                lblStatus.Text = "Analysis done";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error";
                MessageBox.Show($"Error: {ex.Message}", "Upload Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetEndpointByExtension(string ext)
        {
            string[] imageExt = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string[] videoExt = { ".mp4", ".avi", ".mov", ".mkv" };
            string[] audioExt = { ".mp3", ".wav", ".aac", ".flac" };

            if (imageExt.Contains(ext)) return "image";
            if (videoExt.Contains(ext)) return "video";
            if (audioExt.Contains(ext)) return "audio";
            return null;
        }

        private static async Task<string> UploadFileAsync(string mediaType, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            string endpoint;
            string fileFieldName;
            bool requiresPrompt;

            switch (mediaType)
            {
                case "image":
                    endpoint = "http://localhost:5224/api/analyzer/image";
                    fileFieldName = "image";
                    requiresPrompt = true;
                    break;

                case "video":
                    endpoint = "http://localhost:5224/api/analyzer/video/timestamps";
                    fileFieldName = "video";
                    requiresPrompt = true;
                    break;

                case "audio":
                    endpoint = "http://localhost:5224/api/analyzer/audio";
                    fileFieldName = "audioFile";
                    requiresPrompt = false;
                    break;

                default:
                    throw new Exception("Unknown media type");
            }

            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var fileContent = new StreamContent(fileStream);

                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        form.Add(fileContent, fileFieldName, Path.GetFileName(filePath));

                        if (requiresPrompt)
                        {
                            string defaultPrompt = "Analyze this media file";
                            if (mediaType == "image")
                                defaultPrompt = "Analyze this image and describe its content";
                            else if (mediaType == "video")
                                defaultPrompt = "Analyze this video and return timestamps for key events";

                            form.Add(new StringContent(defaultPrompt, Encoding.UTF8), "prompt");
                        }

                        _httpClient.DefaultRequestHeaders.Clear();
                        _httpClient.DefaultRequestHeaders.ExpectContinue = false;

                        var response = await _httpClient.PostAsync(endpoint, form);
                        string responseText = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                            throw new Exception($"API Error {response.StatusCode}: {responseText}");

                        return responseText;
                    }
                }
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException("The request timed out. Try increasing HttpClient timeout or check API performance.", ex);
            }
        }

        public class VideoAnalysis
        {
            [JsonProperty("overallSummary")]
            public string OverallSummary { get; set; }

            [JsonProperty("frameSummaries")]
            public List<FrameSummary> FrameSummaries { get; set; }
        }

        public class FrameSummary
        {
            [JsonProperty("timestamp")]
            public string Timestamp { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }
        }
    }
}
