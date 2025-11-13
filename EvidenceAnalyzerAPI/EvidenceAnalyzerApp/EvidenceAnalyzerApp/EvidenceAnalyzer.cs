using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Windows.Forms;
using VideoAnalyzerApp;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace EvidenceAnalyzerApp
{
    public partial class EvidenceAnalyzer : Form
    {
        private string _videoPath;
        private string _audioPath;

        List<Detail> _objectDetails = new List<Detail>();
        string _summaryText = string.Empty;
        bool _isVideoSummaryAnalyisCompleted = false;
        bool _isVideoObjectDetectionAnalyisCompleted = false;

        string _pdfSavedPath = System.Windows.Forms.Application.StartupPath + "\\PDF";
        string _framesSavedPath = System.Windows.Forms.Application.StartupPath + "\\Frames";
        string _pdfReferenceImageSavedPath = System.Windows.Forms.Application.StartupPath + "\\Images";

        public EvidenceAnalyzer()
        {
            InitializeComponent();
            if (!Directory.Exists(_pdfSavedPath))
                Directory.CreateDirectory(_pdfSavedPath);
            if (!Directory.Exists(_framesSavedPath))
                Directory.CreateDirectory(_framesSavedPath);
            if (!Directory.Exists(_pdfReferenceImageSavedPath))
                Directory.CreateDirectory(_pdfReferenceImageSavedPath);
        }

        private void btnSelectVideo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "MP4 files (*.mp4)|*.mp4";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _videoPath = dialog.FileName;

                    pbVideoSummary.Maximum = 4;

                    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(() =>
                    {
                        GenerateVideoSummaryAsync();
                    });

                    Task.Run(() =>
                    {
                        VideoObjectDetectionAsync();
                    });
                    #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }

        private async Task GenerateVideoSummaryAsync()
        {
            _isVideoSummaryAnalyisCompleted = false;
            _audioPath = Path.ChangeExtension(_videoPath, ".wav");

            UpdateVideoSummaryStatus("Extracting audio...");
            FFmpegHelper.ExtractAudio(_videoPath, _audioPath);
            IncrementVideoSummaryProgressBar();

            UpdateVideoSummaryStatus("Uploading to S3...");
            await S3Uploader.UploadAsync(_audioPath, "mlcnew");
            IncrementVideoSummaryProgressBar();

            UpdateVideoSummaryStatus("Transcribing...");
            var transcript = await TranscribeHelper.TranscribeAudioAsync("mlcnew", Path.GetFileName(_audioPath));
            IncrementVideoSummaryProgressBar();

            UpdateVideoSummaryStatus("Calling claude...");
            _summaryText = await BedrockHelper.SummarizeTranscriptAsync(transcript);

            UpdateVideoSummary(_summaryText);
            UpdateVideoSummaryStatus("Completed the video summary analysis.");
            IncrementVideoSummaryProgressBar();
            _isVideoSummaryAnalyisCompleted = true;
            if (_isVideoObjectDetectionAnalyisCompleted)
            {
                PdfGenerator.GenerateAnalysisReport(_objectDetails, _summaryText, $"{_pdfSavedPath}\\VideoAnalysisReport.pdf", "Video Analysis Report");
            }
        }

        private async Task VideoObjectDetectionAsync()
        {
            _isVideoObjectDetectionAnalyisCompleted = false;
            ClaudeImageAnalyzer claudeImageAnalyzer = new ClaudeImageAnalyzer();
            UpdateVideoFrameObjectDetectionStatus("Generating video frames...");
            List<string> frames = await claudeImageAnalyzer.GenerateVideoFramesAsync(_videoPath, _framesSavedPath);
            await VideoFramesObjectDetectionAsync(frames);
            _isVideoObjectDetectionAnalyisCompleted = true;
            if (_isVideoSummaryAnalyisCompleted)
            {
                PdfGenerator.GenerateAnalysisReport(_objectDetails, _summaryText, $"{_pdfSavedPath}\\VideoAnalysisReport.pdf", "Video Analysis Report");
            }
        }

        private async Task VideoFramesObjectDetectionAsync(List<string> frames)
        {
            List<string> frameObjects = new List<string>();
            double frameCount = 0;
            UpdateVideoFrameObjectDetectionStatus("Analyzing video frames objects...");
            SetMaxValueOfVideoFrameObjectDetetctionProgressBar(frames.Count());

            foreach (var frame in frames)
            {
                try
                {
                    var rekognition = new FrameAnalyzer();

                    var rekognitionOutput = await rekognition.AnalyzeFrameAsync(frame);

                    ObjectDetection(frameObjects, frameCount, frame, rekognitionOutput);
                }
                catch (Exception ex)
                {
                    throw;
                }
                frameCount++;
                IncrementVideoFrameObjectDetetctionProgressBar(frameCount);
            }
            UpdateVideoFrameObjectDetectionStatus("Video frames objects analysis completed.");
        }

        private void ObjectDetection(List<string> frameObjects, double frameCount, string frame, List<string> rekognitionOutput)
        {
            string pdfReferenceImageFileSavedPath = string.Empty;
            foreach (var output in rekognitionOutput)
            {
                if (!frameObjects.Contains(output))
                {
                    frameObjects.Add(output);

                    if (string.IsNullOrEmpty(pdfReferenceImageFileSavedPath))
                    {
                        var imgNumber = output + " " + frameCount.ToString().PadLeft(8, '0');
                        var pdfReferenceImageFile = $@"{_pdfReferenceImageSavedPath}{imgNumber}.png";
                        System.IO.File.Copy(frame, pdfReferenceImageFile, true);
                        pdfReferenceImageFileSavedPath = pdfReferenceImageFile;
                    }

                    Detail detail = new Detail();
                    detail.ObjectName = output;
                    detail.ObjectFoundTime = Math.Round(frameCount, 2).ToString() + " Seconds";
                    detail.ObjectDuration = "0";
                    detail.ImagePath = pdfReferenceImageFileSavedPath;
                    _objectDetails.Add(detail);
                }
                else if (frameObjects.Contains(output))
                {
                    double objDuration = Convert.ToDouble(_objectDetails.Where(s => s.ObjectName == output).FirstOrDefault().ObjectDuration);
                    _objectDetails.Where(s => s.ObjectName == output).FirstOrDefault().ObjectDuration = (Math.Round((objDuration + 1), 2).ToString());
                }
                UpdateVideoFrameObjectDetetction(_objectDetails);
            }
        }

        private void btnAudioSelect_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Audio files (*.wav)|*.wav";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _audioPath = dialog.FileName;

                    pbAudioSummary.Maximum = 3;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(() =>
                    {
                        GenerateAudioSummaryAsync();
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }

        private async Task GenerateAudioSummaryAsync()
        {
            UpdateAudioSummaryStatus("Uploading to S3...");
            await S3Uploader.UploadAsync(_audioPath, "mlcnew");
            IncrementAudioSummaryProgressBar();

            UpdateAudioSummaryStatus("Transcribing...");
            var transcript = await TranscribeHelper.TranscribeAudioAsync("mlcnew", Path.GetFileName(_audioPath));
            IncrementAudioSummaryProgressBar();

            UpdateAudioSummaryStatus("Calling claude...");
            _summaryText = await BedrockHelper.SummarizeTranscriptAsync(transcript);

            UpdateAudioSummary(_summaryText);
            UpdateAudioSummaryStatus("Completed the audio summary analysis.");
            IncrementAudioSummaryProgressBar();

            _objectDetails.Clear();
            PdfGenerator.GenerateAnalysisReport(_objectDetails, _summaryText, $"{_pdfSavedPath}\\AudioAnalysisReport.pdf", "Audio Analysis Report");
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pbImage.Maximum = dialog.FileNames.Count();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(() =>
                    {
                        GenerateImageSummary(dialog.FileNames.ToList<string>());
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }
        private async Task GenerateImageSummary(List<string> selectedImages)
        {
            var _isImageAnalysisCompleted = false;
            List<ImageInterPretations> imageAndObjects = new List<ImageInterPretations>();
            UpdateImageSummary(imageAndObjects);
            UpdateImageSummaryStatus("");
            foreach (var frame in selectedImages)
            {
                try
                {
                    var rekognition = new FrameAnalyzer();
                    var claude = new ClaudeImageAnalyzer();

                    var rekognitionOutput = await rekognition.AnalyzeFrameAsync(frame);
                    var imageSummary = await claude.AnalyzeImageWithTextAsync(frame, "Summarize the image in detail.");
                    ImageInterPretations imageInterPretations = new ImageInterPretations();
                    imageInterPretations.ImagePath = System.IO.Path.GetFileName(frame);
                    imageInterPretations.ImageObjects = rekognitionOutput;
                    imageInterPretations.ImageSummary = imageSummary;
                    imageAndObjects.Add(imageInterPretations);
                    UpdateImageSummary(imageAndObjects);
                    UpdateImageSummaryStatus($"Analyzing {System.IO.Path.GetFileName(frame)}...");
                    IncrementImageSummaryProgressBar();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            UpdateImageSummaryStatus("Image objects analysis completed.");
            _isImageAnalysisCompleted = true;
            if (_isImageAnalysisCompleted && imageAndObjects.Count > 0)
            {
                PdfGenerator.GenerateAnalysisReport(imageAndObjects, $"{_pdfSavedPath}\\ImageAnalysisReport.pdf", "Image Analysis Report");
            }

        }

        private void UpdateImageSummaryStatus(string text)
        {
            if (lblImageSummaryStatus.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lblImageSummaryStatus.Text = text;
                });
            }
            else
            {
                lblImageSummaryStatus.Text = text;
            }
        }

        private void UpdateVideoFrameObjectDetetction(List<Detail> details)
        {
            // Automatically generate the DataGridView columns.
            dgvVideoObjectDetails.AutoGenerateColumns = true;

            //This will create a custom datasource for the DataGridView.
            var detailsDataSource = details.Select(x => new
            {
                ObjectName = x.ObjectName,
                ObjectFoundTime = x.ObjectFoundTime,
                ObjectDuration = x.ObjectDuration + " Seconds"
            }).ToList();

            if (dgvVideoObjectDetails.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { dgvVideoObjectDetails.DataSource = detailsDataSource; });
            }
            else
            {
                dgvVideoObjectDetails.DataSource = detailsDataSource;
            }
        }

        private void UpdateImageSummary(List<ImageInterPretations> keyValuePairs)
        {
            if (trvImageAndObjects.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { PopulateTree(keyValuePairs); });
            }
            else
            {
                PopulateTree(keyValuePairs);
            }
        }

        private void PopulateTree(List<ImageInterPretations> keyValuePairs)
        {
            trvImageAndObjects.BeginUpdate();
            trvImageAndObjects.Nodes.Clear();

            foreach (var kvp in keyValuePairs)
            {
                string fileNameOnly = System.IO.Path.GetFileName(kvp.ImagePath);
                TreeNode parentNode = new TreeNode(fileNameOnly);

                foreach (var child in kvp.ImageObjects)
                {
                    parentNode.Nodes.Add(new TreeNode(child));
                }

                trvImageAndObjects.Nodes.Add(parentNode);
            }

            trvImageAndObjects.ExpandAll();
            trvImageAndObjects.EndUpdate();
        }

        private void IncrementImageSummaryProgressBar()
        {
            int value = pbImage.Value;
            value++;
            if (value > pbImage.Maximum)
            {
                return;
            }
            if (pbImage.Parent.InvokeRequired)
            {
                pbImage.Parent.Invoke(new MethodInvoker(delegate { pbImage.Value = value; }));
            }
            else
            {
                pbImage.Increment(value);
            }
        }

        private void SetMaxValueOfVideoFrameObjectDetetctionProgressBar(int value)
        {
            if (pbVideoObjectDetection.Parent.InvokeRequired)
            {
                pbVideoObjectDetection.Parent.Invoke(new MethodInvoker(delegate { pbVideoObjectDetection.Maximum = value; }));
            }
            else
            {
                pbVideoObjectDetection.Maximum = value;
            }
        }

        private void IncrementVideoFrameObjectDetetctionProgressBar(double value)
        {
            if (value > pbVideoObjectDetection.Maximum)
            {
                return;
            }
            if (pbVideoObjectDetection.Parent.InvokeRequired)
            {
                pbVideoObjectDetection.Parent.Invoke(new MethodInvoker(delegate { pbVideoObjectDetection.Value = (int)value; }));
            }
            else
            {
                pbVideoObjectDetection.Increment((int)value);
            }
        }

        private void UpdateVideoSummaryStatus(string text)
        {
            if (lblVideoSummaryStatus.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { lblVideoSummaryStatus.Text = text; });
            }
            else
            {
                lblVideoSummaryStatus.Text = text;
            }
        }

        private void UpdateVideoSummary(string text)
        {
            if (tbVideoSummary.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { tbVideoSummary.Text = text; });
            }
            else
            {
                tbVideoSummary.Text = text;
            }
        }

        private void IncrementVideoSummaryProgressBar()
        {
            int value = pbVideoSummary.Value;
            value++;
            if (value > pbVideoSummary.Maximum)
            {
                return;
            }
            if (pbVideoSummary.Parent.InvokeRequired)
            {
                pbVideoSummary.Parent.Invoke(new MethodInvoker(delegate { pbVideoSummary.Value = value; }));
            }
            else
            {
                pbVideoSummary.Increment(value);
            }
        }

        private void UpdateVideoFrameObjectDetectionStatus(string text)
        {
            if (lblVideoObjectDetectionStatus.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lblVideoObjectDetectionStatus.Text = text;
                });
            }
            else
            {
                lblVideoObjectDetectionStatus.Text = text;
            }
        }

        private void UpdateAudioSummaryStatus(string text)
        {
            if (lblAudioStatus.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { lblAudioStatus.Text = text; });
            }
            else
            {
                lblAudioStatus.Text = text;
            }
        }

        private void IncrementAudioSummaryProgressBar()
        {
            int value = pbAudioSummary.Value;
            value++;
            if (value > pbAudioSummary.Maximum)
            {
                return;
            }
            if (pbAudioSummary.Parent.InvokeRequired)
            {
                pbAudioSummary.Parent.Invoke(new MethodInvoker(delegate { pbAudioSummary.Value = value; }));
            }
            else
            {
                pbAudioSummary.Increment(value);
            }
        }

        private void UpdateAudioSummary(string text)
        {
            if (tbAudioSummary.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { tbAudioSummary.Text = text; });
            }
            else
            {
                tbAudioSummary.Text = text;
            }
        }

        private void btnOpenVideoAnalysisReport_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenAudioAnalysisReport_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenImageAnalysisReport_Click(object sender, EventArgs e)
        {

        }
    }
}
