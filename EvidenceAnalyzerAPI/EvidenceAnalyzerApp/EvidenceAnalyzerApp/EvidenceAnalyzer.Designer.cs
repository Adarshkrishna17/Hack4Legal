namespace EvidenceAnalyzerApp
{
    partial class EvidenceAnalyzer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            groupBox8 = new GroupBox();
            dgvVideoObjectDetails = new DataGridView();
            gbSummary = new GroupBox();
            tbVideoSummary = new TextBox();
            gbVideoDetails = new GroupBox();
            label7 = new Label();
            lblVideoDuration = new Label();
            lblVideoFormat = new Label();
            label3 = new Label();
            gbVideo = new GroupBox();
            pbVideoObjectDetection = new ProgressBar();
            lblVideoObjectDetectionStatus = new Label();
            pbVideoSummary = new ProgressBar();
            lblVideoSummaryStatus = new Label();
            btnSelectVideo = new Button();
            groupBox2 = new GroupBox();
            gbAudioSummary = new GroupBox();
            tbAudioSummary = new TextBox();
            gbAudioDetails = new GroupBox();
            label5 = new Label();
            lblAudioDuration = new Label();
            lblAudioFormatStatus = new Label();
            label11 = new Label();
            groupBox4 = new GroupBox();
            pbAudioSummary = new ProgressBar();
            lblAudioStatus = new Label();
            btnAudioSelect = new Button();
            groupBox3 = new GroupBox();
            gbObjectDetails = new GroupBox();
            trvImageAndObjects = new TreeView();
            gbImageDetails = new GroupBox();
            label13 = new Label();
            lblImageSize = new Label();
            lblImageFormat = new Label();
            label17 = new Label();
            groupBox5 = new GroupBox();
            pbImage = new ProgressBar();
            lblImageSummaryStatus = new Label();
            btnSelectImage = new Button();
            label6 = new Label();
            btnOpenVideoAnalysisReport = new Button();
            btnOpenAudioAnalysisReport = new Button();
            btnOpenImageAnalysisReport = new Button();
            groupBox1.SuspendLayout();
            groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVideoObjectDetails).BeginInit();
            gbSummary.SuspendLayout();
            gbVideoDetails.SuspendLayout();
            gbVideo.SuspendLayout();
            groupBox2.SuspendLayout();
            gbAudioSummary.SuspendLayout();
            gbAudioDetails.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            gbObjectDetails.SuspendLayout();
            gbImageDetails.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ButtonHighlight;
            groupBox1.Controls.Add(groupBox8);
            groupBox1.Controls.Add(gbSummary);
            groupBox1.Controls.Add(gbVideoDetails);
            groupBox1.Controls.Add(gbVideo);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1188, 279);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Video Analyzer";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(dgvVideoObjectDetails);
            groupBox8.Location = new Point(616, 68);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(372, 199);
            groupBox8.TabIndex = 3;
            groupBox8.TabStop = false;
            groupBox8.Text = "Object Details";
            // 
            // dgvVideoObjectDetails
            // 
            dgvVideoObjectDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVideoObjectDetails.Location = new Point(6, 22);
            dgvVideoObjectDetails.Name = "dgvVideoObjectDetails";
            dgvVideoObjectDetails.Size = new Size(360, 169);
            dgvVideoObjectDetails.TabIndex = 0;
            // 
            // gbSummary
            // 
            gbSummary.Controls.Add(tbVideoSummary);
            gbSummary.Location = new Point(8, 68);
            gbSummary.Name = "gbSummary";
            gbSummary.Size = new Size(602, 199);
            gbSummary.TabIndex = 2;
            gbSummary.TabStop = false;
            gbSummary.Text = "Video Summary";
            // 
            // tbVideoSummary
            // 
            tbVideoSummary.Location = new Point(11, 22);
            tbVideoSummary.Multiline = true;
            tbVideoSummary.Name = "tbVideoSummary";
            tbVideoSummary.ScrollBars = ScrollBars.Vertical;
            tbVideoSummary.Size = new Size(577, 169);
            tbVideoSummary.TabIndex = 0;
            // 
            // gbVideoDetails
            // 
            gbVideoDetails.Controls.Add(btnOpenVideoAnalysisReport);
            gbVideoDetails.Controls.Add(label7);
            gbVideoDetails.Controls.Add(lblVideoDuration);
            gbVideoDetails.Controls.Add(lblVideoFormat);
            gbVideoDetails.Controls.Add(label3);
            gbVideoDetails.Location = new Point(994, 68);
            gbVideoDetails.Name = "gbVideoDetails";
            gbVideoDetails.Size = new Size(184, 199);
            gbVideoDetails.TabIndex = 1;
            gbVideoDetails.TabStop = false;
            gbVideoDetails.Text = "Video Details";
            // 
            // label7
            // 
            label7.Location = new Point(11, 87);
            label7.Name = "label7";
            label7.Size = new Size(76, 25);
            label7.TabIndex = 12;
            label7.Text = "Duration";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVideoDuration
            // 
            lblVideoDuration.BorderStyle = BorderStyle.FixedSingle;
            lblVideoDuration.Location = new Point(99, 92);
            lblVideoDuration.Name = "lblVideoDuration";
            lblVideoDuration.Size = new Size(76, 25);
            lblVideoDuration.TabIndex = 10;
            lblVideoDuration.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVideoFormat
            // 
            lblVideoFormat.BorderStyle = BorderStyle.FixedSingle;
            lblVideoFormat.Location = new Point(99, 40);
            lblVideoFormat.Name = "lblVideoFormat";
            lblVideoFormat.Size = new Size(76, 25);
            lblVideoFormat.TabIndex = 9;
            lblVideoFormat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Location = new Point(11, 41);
            label3.Name = "label3";
            label3.Size = new Size(76, 25);
            label3.TabIndex = 3;
            label3.Text = "Format";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // gbVideo
            // 
            gbVideo.Controls.Add(pbVideoObjectDetection);
            gbVideo.Controls.Add(lblVideoObjectDetectionStatus);
            gbVideo.Controls.Add(pbVideoSummary);
            gbVideo.Controls.Add(lblVideoSummaryStatus);
            gbVideo.Controls.Add(btnSelectVideo);
            gbVideo.Location = new Point(6, 19);
            gbVideo.Name = "gbVideo";
            gbVideo.Size = new Size(1172, 49);
            gbVideo.TabIndex = 0;
            gbVideo.TabStop = false;
            // 
            // pbVideoObjectDetection
            // 
            pbVideoObjectDetection.Location = new Point(886, 16);
            pbVideoObjectDetection.Name = "pbVideoObjectDetection";
            pbVideoObjectDetection.Size = new Size(274, 25);
            pbVideoObjectDetection.TabIndex = 4;
            // 
            // lblVideoObjectDetectionStatus
            // 
            lblVideoObjectDetectionStatus.BorderStyle = BorderStyle.FixedSingle;
            lblVideoObjectDetectionStatus.Location = new Point(623, 16);
            lblVideoObjectDetectionStatus.Name = "lblVideoObjectDetectionStatus";
            lblVideoObjectDetectionStatus.Size = new Size(247, 25);
            lblVideoObjectDetectionStatus.TabIndex = 3;
            lblVideoObjectDetectionStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pbVideoSummary
            // 
            pbVideoSummary.Location = new Point(368, 16);
            pbVideoSummary.Name = "pbVideoSummary";
            pbVideoSummary.Size = new Size(236, 25);
            pbVideoSummary.TabIndex = 2;
            // 
            // lblVideoSummaryStatus
            // 
            lblVideoSummaryStatus.BorderStyle = BorderStyle.FixedSingle;
            lblVideoSummaryStatus.Location = new Point(119, 16);
            lblVideoSummaryStatus.Name = "lblVideoSummaryStatus";
            lblVideoSummaryStatus.Size = new Size(233, 25);
            lblVideoSummaryStatus.TabIndex = 1;
            lblVideoSummaryStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnSelectVideo
            // 
            btnSelectVideo.BackColor = SystemColors.GradientActiveCaption;
            btnSelectVideo.Location = new Point(10, 16);
            btnSelectVideo.Name = "btnSelectVideo";
            btnSelectVideo.Size = new Size(97, 27);
            btnSelectVideo.TabIndex = 0;
            btnSelectVideo.Text = "Select Video";
            btnSelectVideo.UseVisualStyleBackColor = false;
            btnSelectVideo.Click += btnSelectVideo_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(gbAudioSummary);
            groupBox2.Controls.Add(gbAudioDetails);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Location = new Point(12, 306);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(584, 256);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Audio Analyzer";
            // 
            // gbAudioSummary
            // 
            gbAudioSummary.Controls.Add(tbAudioSummary);
            gbAudioSummary.Location = new Point(8, 73);
            gbAudioSummary.Name = "gbAudioSummary";
            gbAudioSummary.Size = new Size(379, 173);
            gbAudioSummary.TabIndex = 3;
            gbAudioSummary.TabStop = false;
            gbAudioSummary.Text = "Audio Summary";
            // 
            // tbAudioSummary
            // 
            tbAudioSummary.Location = new Point(9, 22);
            tbAudioSummary.Multiline = true;
            tbAudioSummary.Name = "tbAudioSummary";
            tbAudioSummary.ScrollBars = ScrollBars.Vertical;
            tbAudioSummary.Size = new Size(362, 145);
            tbAudioSummary.TabIndex = 1;
            // 
            // gbAudioDetails
            // 
            gbAudioDetails.Controls.Add(btnOpenAudioAnalysisReport);
            gbAudioDetails.Controls.Add(label5);
            gbAudioDetails.Controls.Add(lblAudioDuration);
            gbAudioDetails.Controls.Add(lblAudioFormatStatus);
            gbAudioDetails.Controls.Add(label11);
            gbAudioDetails.Location = new Point(394, 73);
            gbAudioDetails.Name = "gbAudioDetails";
            gbAudioDetails.Size = new Size(184, 173);
            gbAudioDetails.TabIndex = 14;
            gbAudioDetails.TabStop = false;
            gbAudioDetails.Text = "Audio Details";
            // 
            // label5
            // 
            label5.Location = new Point(16, 80);
            label5.Name = "label5";
            label5.Size = new Size(76, 25);
            label5.TabIndex = 12;
            label5.Text = "Duration";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAudioDuration
            // 
            lblAudioDuration.BorderStyle = BorderStyle.FixedSingle;
            lblAudioDuration.Location = new Point(101, 80);
            lblAudioDuration.Name = "lblAudioDuration";
            lblAudioDuration.Size = new Size(76, 25);
            lblAudioDuration.TabIndex = 10;
            lblAudioDuration.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAudioFormatStatus
            // 
            lblAudioFormatStatus.BorderStyle = BorderStyle.FixedSingle;
            lblAudioFormatStatus.Location = new Point(101, 33);
            lblAudioFormatStatus.Name = "lblAudioFormatStatus";
            lblAudioFormatStatus.Size = new Size(76, 25);
            lblAudioFormatStatus.TabIndex = 9;
            lblAudioFormatStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            label11.Location = new Point(16, 33);
            label11.Name = "label11";
            label11.Size = new Size(76, 25);
            label11.TabIndex = 3;
            label11.Text = "Format";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(pbAudioSummary);
            groupBox4.Controls.Add(lblAudioStatus);
            groupBox4.Controls.Add(btnAudioSelect);
            groupBox4.Location = new Point(6, 16);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(572, 49);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            // 
            // pbAudioSummary
            // 
            pbAudioSummary.Location = new Point(324, 16);
            pbAudioSummary.Name = "pbAudioSummary";
            pbAudioSummary.Size = new Size(242, 25);
            pbAudioSummary.TabIndex = 2;
            // 
            // lblAudioStatus
            // 
            lblAudioStatus.BorderStyle = BorderStyle.FixedSingle;
            lblAudioStatus.Location = new Point(115, 16);
            lblAudioStatus.Name = "lblAudioStatus";
            lblAudioStatus.Size = new Size(196, 25);
            lblAudioStatus.TabIndex = 1;
            lblAudioStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAudioSelect
            // 
            btnAudioSelect.BackColor = SystemColors.GradientActiveCaption;
            btnAudioSelect.Location = new Point(10, 16);
            btnAudioSelect.Name = "btnAudioSelect";
            btnAudioSelect.Size = new Size(97, 27);
            btnAudioSelect.TabIndex = 0;
            btnAudioSelect.Text = "Select Audio";
            btnAudioSelect.UseVisualStyleBackColor = false;
            btnAudioSelect.Click += btnAudioSelect_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(gbObjectDetails);
            groupBox3.Controls.Add(gbImageDetails);
            groupBox3.Controls.Add(groupBox5);
            groupBox3.Location = new Point(612, 306);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(588, 256);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Image Analyzer";
            // 
            // gbObjectDetails
            // 
            gbObjectDetails.Controls.Add(trvImageAndObjects);
            gbObjectDetails.Location = new Point(7, 72);
            gbObjectDetails.Name = "gbObjectDetails";
            gbObjectDetails.Size = new Size(379, 173);
            gbObjectDetails.TabIndex = 4;
            gbObjectDetails.TabStop = false;
            gbObjectDetails.Text = "Object Details";
            // 
            // trvImageAndObjects
            // 
            trvImageAndObjects.Dock = DockStyle.Fill;
            trvImageAndObjects.Location = new Point(3, 19);
            trvImageAndObjects.Name = "trvImageAndObjects";
            trvImageAndObjects.Size = new Size(373, 151);
            trvImageAndObjects.TabIndex = 0;
            // 
            // gbImageDetails
            // 
            gbImageDetails.Controls.Add(btnOpenImageAnalysisReport);
            gbImageDetails.Controls.Add(label13);
            gbImageDetails.Controls.Add(lblImageSize);
            gbImageDetails.Controls.Add(lblImageFormat);
            gbImageDetails.Controls.Add(label17);
            gbImageDetails.Location = new Point(394, 72);
            gbImageDetails.Name = "gbImageDetails";
            gbImageDetails.Size = new Size(184, 173);
            gbImageDetails.TabIndex = 15;
            gbImageDetails.TabStop = false;
            gbImageDetails.Text = "Image Details";
            // 
            // label13
            // 
            label13.Location = new Point(16, 80);
            label13.Name = "label13";
            label13.Size = new Size(76, 25);
            label13.TabIndex = 12;
            label13.Text = "Size";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblImageSize
            // 
            lblImageSize.BorderStyle = BorderStyle.FixedSingle;
            lblImageSize.Location = new Point(101, 80);
            lblImageSize.Name = "lblImageSize";
            lblImageSize.Size = new Size(76, 25);
            lblImageSize.TabIndex = 10;
            lblImageSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblImageFormat
            // 
            lblImageFormat.BorderStyle = BorderStyle.FixedSingle;
            lblImageFormat.Location = new Point(101, 33);
            lblImageFormat.Name = "lblImageFormat";
            lblImageFormat.Size = new Size(76, 25);
            lblImageFormat.TabIndex = 9;
            lblImageFormat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            label17.Location = new Point(16, 33);
            label17.Name = "label17";
            label17.Size = new Size(76, 25);
            label17.TabIndex = 3;
            label17.Text = "Format";
            label17.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(pbImage);
            groupBox5.Controls.Add(lblImageSummaryStatus);
            groupBox5.Controls.Add(btnSelectImage);
            groupBox5.Location = new Point(6, 16);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(572, 49);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            // 
            // pbImage
            // 
            pbImage.Location = new Point(324, 16);
            pbImage.Name = "pbImage";
            pbImage.Size = new Size(242, 25);
            pbImage.TabIndex = 2;
            // 
            // lblImageSummaryStatus
            // 
            lblImageSummaryStatus.BorderStyle = BorderStyle.FixedSingle;
            lblImageSummaryStatus.Location = new Point(115, 16);
            lblImageSummaryStatus.Name = "lblImageSummaryStatus";
            lblImageSummaryStatus.Size = new Size(196, 25);
            lblImageSummaryStatus.TabIndex = 1;
            lblImageSummaryStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnSelectImage
            // 
            btnSelectImage.BackColor = SystemColors.GradientActiveCaption;
            btnSelectImage.Location = new Point(10, 16);
            btnSelectImage.Name = "btnSelectImage";
            btnSelectImage.Size = new Size(97, 27);
            btnSelectImage.TabIndex = 0;
            btnSelectImage.Text = "Select Image";
            btnSelectImage.UseVisualStyleBackColor = false;
            btnSelectImage.Click += btnSelectImage_Click;
            // 
            // label6
            // 
            label6.BackColor = SystemColors.GradientActiveCaption;
            label6.Location = new Point(12, 299);
            label6.Name = "label6";
            label6.Size = new Size(1188, 3);
            label6.TabIndex = 3;
            label6.Text = "label6";
            // 
            // btnOpenVideoAnalysisReport
            // 
            btnOpenVideoAnalysisReport.BackColor = SystemColors.GradientActiveCaption;
            btnOpenVideoAnalysisReport.Location = new Point(41, 137);
            btnOpenVideoAnalysisReport.Name = "btnOpenVideoAnalysisReport";
            btnOpenVideoAnalysisReport.Size = new Size(106, 39);
            btnOpenVideoAnalysisReport.TabIndex = 5;
            btnOpenVideoAnalysisReport.Text = "Open Video Analyisis Report";
            btnOpenVideoAnalysisReport.UseVisualStyleBackColor = false;
            btnOpenVideoAnalysisReport.Click += btnOpenVideoAnalysisReport_Click;
            // 
            // btnOpenAudioAnalysisReport
            // 
            btnOpenAudioAnalysisReport.BackColor = SystemColors.GradientActiveCaption;
            btnOpenAudioAnalysisReport.Location = new Point(35, 120);
            btnOpenAudioAnalysisReport.Name = "btnOpenAudioAnalysisReport";
            btnOpenAudioAnalysisReport.Size = new Size(106, 39);
            btnOpenAudioAnalysisReport.TabIndex = 13;
            btnOpenAudioAnalysisReport.Text = "Open Video Analyisis Report";
            btnOpenAudioAnalysisReport.UseVisualStyleBackColor = false;
            btnOpenAudioAnalysisReport.Click += btnOpenAudioAnalysisReport_Click;
            // 
            // btnOpenImageAnalysisReport
            // 
            btnOpenImageAnalysisReport.BackColor = SystemColors.GradientActiveCaption;
            btnOpenImageAnalysisReport.Location = new Point(41, 121);
            btnOpenImageAnalysisReport.Name = "btnOpenImageAnalysisReport";
            btnOpenImageAnalysisReport.Size = new Size(106, 39);
            btnOpenImageAnalysisReport.TabIndex = 14;
            btnOpenImageAnalysisReport.Text = "Open Image Analyisis Report";
            btnOpenImageAnalysisReport.UseVisualStyleBackColor = false;
            btnOpenImageAnalysisReport.Click += btnOpenImageAnalysisReport_Click;
            // 
            // EvidenceAnalyzer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1205, 574);
            Controls.Add(label6);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "EvidenceAnalyzer";
            Text = "Evidence Analyzer";
            groupBox1.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVideoObjectDetails).EndInit();
            gbSummary.ResumeLayout(false);
            gbSummary.PerformLayout();
            gbVideoDetails.ResumeLayout(false);
            gbVideo.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            gbAudioSummary.ResumeLayout(false);
            gbAudioSummary.PerformLayout();
            gbAudioDetails.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            gbObjectDetails.ResumeLayout(false);
            gbImageDetails.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox gbVideo;
        private ProgressBar pbVideoSummary;
        private Label lblVideoSummaryStatus;
        private Button btnSelectVideo;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private ProgressBar pbAudioSummary;
        private Label lblAudioStatus;
        private Button btnAudioSelect;
        private GroupBox groupBox5;
        private ProgressBar pbImage;
        private Label lblImageSummaryStatus;
        private Button btnSelectImage;
        private GroupBox gbVideoDetails;
        private Label label7;
        private Label lblVideoDuration;
        private Label lblVideoFormat;
        private Label label3;
        private GroupBox groupBox8;
        private GroupBox gbSummary;
        private GroupBox gbAudioDetails;
        private Label label4;
        private Label label5;
        private Label lblAudioTemperedStatus;
        private Label lblAudioDuration;
        private Label lblAudioFormatStatus;
        private Label label11;
        private GroupBox gbImageDetails;
        private Label label12;
        private Label label13;
        private Label lblImageStatus;
        private Label lblImageSize;
        private Label lblImageFormat;
        private Label label17;
        private GroupBox gbAudioSummary;
        private GroupBox gbObjectDetails;
        private TextBox tbVideoSummary;
        private TextBox tbAudioSummary;
        private ProgressBar pbVideoObjectDetection;
        private Label lblVideoObjectDetectionStatus;
        private Label label6;
        private TreeView trvImageAndObjects;
        private DataGridView dgvVideoObjectDetails;
        private Button btnOpenVideoAnalysisReport;
        private Button btnOpenAudioAnalysisReport;
        private Button btnOpenImageAnalysisReport;
    }
}
