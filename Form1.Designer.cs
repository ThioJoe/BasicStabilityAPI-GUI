namespace StableDiffusionWinForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.txtNegativePrompt = new System.Windows.Forms.TextBox();
            this.nudImageCount = new System.Windows.Forms.NumericUpDown();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.cmbAspectRatio = new System.Windows.Forms.ComboBox();
            this.nudSeed = new System.Windows.Forms.NumericUpDown();
            this.cmbOutputFormat = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.txtUpscalePrompt = new System.Windows.Forms.TextBox();
            this.nudUpscaleSeed = new System.Windows.Forms.NumericUpDown();
            this.txtUpscaleNegativePrompt = new System.Windows.Forms.TextBox();
            this.cmbUpscaleOutputFormat = new System.Windows.Forms.ComboBox();
            this.nudUpscaleCreativity = new System.Windows.Forms.NumericUpDown();
            this.btnUpscale = new System.Windows.Forms.Button();
            this.btnDownloadUpscaledImages = new System.Windows.Forms.Button();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lblNegativePrompt = new System.Windows.Forms.Label();
            this.lblImageCount = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.lblAspectRatio = new System.Windows.Forms.Label();
            this.lblSeed = new System.Windows.Forms.Label();
            this.lblOutputFormat = new System.Windows.Forms.Label();
            this.lblImagePath = new System.Windows.Forms.Label();
            this.lblUpscalePrompt = new System.Windows.Forms.Label();
            this.lblUpscaleSeed = new System.Windows.Forms.Label();
            this.lblUpscaleNegativePrompt = new System.Windows.Forms.Label();
            this.lblUpscaleOutputFormat = new System.Windows.Forms.Label();
            this.lblUpscaleCreativity = new System.Windows.Forms.Label();
            this.TextToImageTitleLabel = new System.Windows.Forms.Label();
            this.ImageUpscaleTitleLabel = new System.Windows.Forms.Label();
            this.MainTitleLabel = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.btnSaveApiKey = new System.Windows.Forms.Button();
            this.lblApiKeyStatus = new System.Windows.Forms.Label();
            this.ApiKeyLabel = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.GenerationStatusLabel = new System.Windows.Forms.Label();
            this.UpscaleStatusLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpscaleSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpscaleCreativity)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPrompt
            // 
            this.txtPrompt.Location = new System.Drawing.Point(40, 208);
            this.txtPrompt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(448, 90);
            this.txtPrompt.TabIndex = 0;
            // 
            // txtNegativePrompt
            // 
            this.txtNegativePrompt.Location = new System.Drawing.Point(40, 345);
            this.txtNegativePrompt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNegativePrompt.Multiline = true;
            this.txtNegativePrompt.Name = "txtNegativePrompt";
            this.txtNegativePrompt.Size = new System.Drawing.Size(448, 90);
            this.txtNegativePrompt.TabIndex = 1;
            // 
            // nudImageCount
            // 
            this.nudImageCount.Location = new System.Drawing.Point(40, 465);
            this.nudImageCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudImageCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudImageCount.Name = "nudImageCount";
            this.nudImageCount.Size = new System.Drawing.Size(180, 26);
            this.nudImageCount.TabIndex = 2;
            this.nudImageCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbModel
            // 
            this.cmbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Items.AddRange(new object[] {
            "SD3",
            "SD3-Turbo"});
            this.cmbModel.Location = new System.Drawing.Point(42, 522);
            this.cmbModel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(178, 28);
            this.cmbModel.TabIndex = 3;
            // 
            // cmbAspectRatio
            // 
            this.cmbAspectRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAspectRatio.FormattingEnabled = true;
            this.cmbAspectRatio.Items.AddRange(new object[] {
            "1:1",
            "16:9",
            "21:9",
            "2:3",
            "3:2",
            "4:5",
            "5:4",
            "9:16",
            "9:21"});
            this.cmbAspectRatio.Location = new System.Drawing.Point(231, 522);
            this.cmbAspectRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAspectRatio.Name = "cmbAspectRatio";
            this.cmbAspectRatio.Size = new System.Drawing.Size(178, 28);
            this.cmbAspectRatio.TabIndex = 4;
            // 
            // nudSeed
            // 
            this.nudSeed.Location = new System.Drawing.Point(40, 582);
            this.nudSeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudSeed.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudSeed.Name = "nudSeed";
            this.nudSeed.Size = new System.Drawing.Size(180, 26);
            this.nudSeed.TabIndex = 5;
            // 
            // cmbOutputFormat
            // 
            this.cmbOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutputFormat.FormattingEnabled = true;
            this.cmbOutputFormat.Items.AddRange(new object[] {
            "png",
            "jpeg"});
            this.cmbOutputFormat.Location = new System.Drawing.Point(230, 582);
            this.cmbOutputFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbOutputFormat.Name = "cmbOutputFormat";
            this.cmbOutputFormat.Size = new System.Drawing.Size(178, 28);
            this.cmbOutputFormat.TabIndex = 6;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(40, 625);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(180, 35);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(576, 208);
            this.txtImagePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(448, 26);
            this.txtImagePath.TabIndex = 8;
            // 
            // txtUpscalePrompt
            // 
            this.txtUpscalePrompt.Location = new System.Drawing.Point(576, 275);
            this.txtUpscalePrompt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUpscalePrompt.Multiline = true;
            this.txtUpscalePrompt.Name = "txtUpscalePrompt";
            this.txtUpscalePrompt.Size = new System.Drawing.Size(448, 90);
            this.txtUpscalePrompt.TabIndex = 9;
            // 
            // nudUpscaleSeed
            // 
            this.nudUpscaleSeed.Location = new System.Drawing.Point(576, 538);
            this.nudUpscaleSeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudUpscaleSeed.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudUpscaleSeed.Name = "nudUpscaleSeed";
            this.nudUpscaleSeed.Size = new System.Drawing.Size(180, 26);
            this.nudUpscaleSeed.TabIndex = 10;
            // 
            // txtUpscaleNegativePrompt
            // 
            this.txtUpscaleNegativePrompt.Location = new System.Drawing.Point(576, 400);
            this.txtUpscaleNegativePrompt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUpscaleNegativePrompt.Multiline = true;
            this.txtUpscaleNegativePrompt.Name = "txtUpscaleNegativePrompt";
            this.txtUpscaleNegativePrompt.Size = new System.Drawing.Size(448, 90);
            this.txtUpscaleNegativePrompt.TabIndex = 11;
            // 
            // cmbUpscaleOutputFormat
            // 
            this.cmbUpscaleOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpscaleOutputFormat.FormattingEnabled = true;
            this.cmbUpscaleOutputFormat.Items.AddRange(new object[] {
            "png",
            "jpeg",
            "webp"});
            this.cmbUpscaleOutputFormat.Location = new System.Drawing.Point(765, 538);
            this.cmbUpscaleOutputFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbUpscaleOutputFormat.Name = "cmbUpscaleOutputFormat";
            this.cmbUpscaleOutputFormat.Size = new System.Drawing.Size(178, 28);
            this.cmbUpscaleOutputFormat.TabIndex = 12;
            // 
            // nudUpscaleCreativity
            // 
            this.nudUpscaleCreativity.DecimalPlaces = 2;
            this.nudUpscaleCreativity.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nudUpscaleCreativity.Location = new System.Drawing.Point(576, 595);
            this.nudUpscaleCreativity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudUpscaleCreativity.Maximum = new decimal(new int[] {
            35,
            0,
            0,
            131072});
            this.nudUpscaleCreativity.Name = "nudUpscaleCreativity";
            this.nudUpscaleCreativity.Size = new System.Drawing.Size(180, 26);
            this.nudUpscaleCreativity.TabIndex = 13;
            this.nudUpscaleCreativity.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            // 
            // btnUpscale
            // 
            this.btnUpscale.Location = new System.Drawing.Point(576, 649);
            this.btnUpscale.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUpscale.Name = "btnUpscale";
            this.btnUpscale.Size = new System.Drawing.Size(180, 35);
            this.btnUpscale.TabIndex = 14;
            this.btnUpscale.Text = "Upscale";
            this.btnUpscale.UseVisualStyleBackColor = true;
            this.btnUpscale.Click += new System.EventHandler(this.BtnUpscale_Click);
            // 
            // btnDownloadUpscaledImages
            // 
            this.btnDownloadUpscaledImages.Location = new System.Drawing.Point(765, 649);
            this.btnDownloadUpscaledImages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDownloadUpscaledImages.Name = "btnDownloadUpscaledImages";
            this.btnDownloadUpscaledImages.Size = new System.Drawing.Size(261, 35);
            this.btnDownloadUpscaledImages.TabIndex = 15;
            this.btnDownloadUpscaledImages.Text = "Download Upscaled Images";
            this.btnDownloadUpscaledImages.UseVisualStyleBackColor = true;
            this.btnDownloadUpscaledImages.Click += new System.EventHandler(this.BtnDownloadUpscaledImages_Click);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(39, 182);
            this.lblPrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(64, 20);
            this.lblPrompt.TabIndex = 16;
            this.lblPrompt.Text = "Prompt:";
            // 
            // lblNegativePrompt
            // 
            this.lblNegativePrompt.AutoSize = true;
            this.lblNegativePrompt.Location = new System.Drawing.Point(39, 318);
            this.lblNegativePrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNegativePrompt.Name = "lblNegativePrompt";
            this.lblNegativePrompt.Size = new System.Drawing.Size(130, 20);
            this.lblNegativePrompt.TabIndex = 17;
            this.lblNegativePrompt.Text = "Negative Prompt:";
            // 
            // lblImageCount
            // 
            this.lblImageCount.AutoSize = true;
            this.lblImageCount.Location = new System.Drawing.Point(40, 438);
            this.lblImageCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageCount.Name = "lblImageCount";
            this.lblImageCount.Size = new System.Drawing.Size(105, 20);
            this.lblImageCount.TabIndex = 18;
            this.lblImageCount.Text = "Image Count:";
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(40, 497);
            this.lblModel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(56, 20);
            this.lblModel.TabIndex = 19;
            this.lblModel.Text = "Model:";
            // 
            // lblAspectRatio
            // 
            this.lblAspectRatio.AutoSize = true;
            this.lblAspectRatio.Location = new System.Drawing.Point(230, 497);
            this.lblAspectRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAspectRatio.Name = "lblAspectRatio";
            this.lblAspectRatio.Size = new System.Drawing.Size(105, 20);
            this.lblAspectRatio.TabIndex = 20;
            this.lblAspectRatio.Text = "Aspect Ratio:";
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(39, 555);
            this.lblSeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(51, 20);
            this.lblSeed.TabIndex = 21;
            this.lblSeed.Text = "Seed:";
            // 
            // lblOutputFormat
            // 
            this.lblOutputFormat.AutoSize = true;
            this.lblOutputFormat.Location = new System.Drawing.Point(230, 555);
            this.lblOutputFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputFormat.Name = "lblOutputFormat";
            this.lblOutputFormat.Size = new System.Drawing.Size(117, 20);
            this.lblOutputFormat.TabIndex = 22;
            this.lblOutputFormat.Text = "Output Format:";
            // 
            // lblImagePath
            // 
            this.lblImagePath.AutoSize = true;
            this.lblImagePath.Location = new System.Drawing.Point(572, 182);
            this.lblImagePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImagePath.Name = "lblImagePath";
            this.lblImagePath.Size = new System.Drawing.Size(95, 20);
            this.lblImagePath.TabIndex = 23;
            this.lblImagePath.Text = "Image Path:";
            // 
            // lblUpscalePrompt
            // 
            this.lblUpscalePrompt.AutoSize = true;
            this.lblUpscalePrompt.Location = new System.Drawing.Point(576, 251);
            this.lblUpscalePrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpscalePrompt.Name = "lblUpscalePrompt";
            this.lblUpscalePrompt.Size = new System.Drawing.Size(126, 20);
            this.lblUpscalePrompt.TabIndex = 24;
            this.lblUpscalePrompt.Text = "Upscale Prompt:";
            // 
            // lblUpscaleSeed
            // 
            this.lblUpscaleSeed.AutoSize = true;
            this.lblUpscaleSeed.Location = new System.Drawing.Point(572, 514);
            this.lblUpscaleSeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpscaleSeed.Name = "lblUpscaleSeed";
            this.lblUpscaleSeed.Size = new System.Drawing.Size(113, 20);
            this.lblUpscaleSeed.TabIndex = 25;
            this.lblUpscaleSeed.Text = "Upscale Seed:";
            // 
            // lblUpscaleNegativePrompt
            // 
            this.lblUpscaleNegativePrompt.AutoSize = true;
            this.lblUpscaleNegativePrompt.Location = new System.Drawing.Point(576, 375);
            this.lblUpscaleNegativePrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpscaleNegativePrompt.Name = "lblUpscaleNegativePrompt";
            this.lblUpscaleNegativePrompt.Size = new System.Drawing.Size(192, 20);
            this.lblUpscaleNegativePrompt.TabIndex = 26;
            this.lblUpscaleNegativePrompt.Text = "Upscale Negative Prompt:";
            // 
            // lblUpscaleOutputFormat
            // 
            this.lblUpscaleOutputFormat.AutoSize = true;
            this.lblUpscaleOutputFormat.Location = new System.Drawing.Point(760, 514);
            this.lblUpscaleOutputFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpscaleOutputFormat.Name = "lblUpscaleOutputFormat";
            this.lblUpscaleOutputFormat.Size = new System.Drawing.Size(179, 20);
            this.lblUpscaleOutputFormat.TabIndex = 27;
            this.lblUpscaleOutputFormat.Text = "Upscale Output Format:";
            // 
            // lblUpscaleCreativity
            // 
            this.lblUpscaleCreativity.AutoSize = true;
            this.lblUpscaleCreativity.Location = new System.Drawing.Point(572, 571);
            this.lblUpscaleCreativity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpscaleCreativity.Name = "lblUpscaleCreativity";
            this.lblUpscaleCreativity.Size = new System.Drawing.Size(139, 20);
            this.lblUpscaleCreativity.TabIndex = 28;
            this.lblUpscaleCreativity.Text = "Upscale Creativity:";
            // 
            // TextToImageTitleLabel
            // 
            this.TextToImageTitleLabel.AutoSize = true;
            this.TextToImageTitleLabel.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextToImageTitleLabel.Location = new System.Drawing.Point(104, 21);
            this.TextToImageTitleLabel.Name = "TextToImageTitleLabel";
            this.TextToImageTitleLabel.Size = new System.Drawing.Size(251, 39);
            this.TextToImageTitleLabel.TabIndex = 29;
            this.TextToImageTitleLabel.Text = "Text To Image Controls";
            // 
            // ImageUpscaleTitleLabel
            // 
            this.ImageUpscaleTitleLabel.AutoSize = true;
            this.ImageUpscaleTitleLabel.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImageUpscaleTitleLabel.Location = new System.Drawing.Point(119, 21);
            this.ImageUpscaleTitleLabel.Name = "ImageUpscaleTitleLabel";
            this.ImageUpscaleTitleLabel.Size = new System.Drawing.Size(264, 39);
            this.ImageUpscaleTitleLabel.TabIndex = 30;
            this.ImageUpscaleTitleLabel.Text = "Image Upscale Controls";
            // 
            // MainTitleLabel
            // 
            this.MainTitleLabel.AutoSize = true;
            this.MainTitleLabel.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainTitleLabel.Location = new System.Drawing.Point(16, 14);
            this.MainTitleLabel.Name = "MainTitleLabel";
            this.MainTitleLabel.Size = new System.Drawing.Size(438, 58);
            this.MainTitleLabel.TabIndex = 31;
            this.MainTitleLabel.Text = "Stable Diffusion 3 API Tool";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(15, 719);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(300, 26);
            this.txtApiKey.TabIndex = 29;
            // 
            // btnSaveApiKey
            // 
            this.btnSaveApiKey.Location = new System.Drawing.Point(321, 714);
            this.btnSaveApiKey.Name = "btnSaveApiKey";
            this.btnSaveApiKey.Size = new System.Drawing.Size(75, 37);
            this.btnSaveApiKey.TabIndex = 30;
            this.btnSaveApiKey.Text = "Save";
            this.btnSaveApiKey.UseVisualStyleBackColor = true;
            this.btnSaveApiKey.Click += new System.EventHandler(this.BtnSaveApiKey_Click);
            // 
            // lblApiKeyStatus
            // 
            this.lblApiKeyStatus.AutoSize = true;
            this.lblApiKeyStatus.ForeColor = System.Drawing.Color.Green;
            this.lblApiKeyStatus.Location = new System.Drawing.Point(408, 725);
            this.lblApiKeyStatus.Name = "lblApiKeyStatus";
            this.lblApiKeyStatus.Size = new System.Drawing.Size(0, 20);
            this.lblApiKeyStatus.TabIndex = 31;
            // 
            // ApiKeyLabel
            // 
            this.ApiKeyLabel.AutoSize = true;
            this.ApiKeyLabel.Location = new System.Drawing.Point(14, 693);
            this.ApiKeyLabel.Name = "ApiKeyLabel";
            this.ApiKeyLabel.Size = new System.Drawing.Size(65, 20);
            this.ApiKeyLabel.TabIndex = 32;
            this.ApiKeyLabel.Text = "API Key";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(408, 725);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(44, 20);
            this.lblError.TabIndex = 33;
            this.lblError.Text = "Error";
            this.lblError.Visible = false;
            // 
            // GenerationStatusLabel
            // 
            this.GenerationStatusLabel.AutoSize = true;
            this.GenerationStatusLabel.Location = new System.Drawing.Point(225, 632);
            this.GenerationStatusLabel.Name = "GenerationStatusLabel";
            this.GenerationStatusLabel.Size = new System.Drawing.Size(175, 20);
            this.GenerationStatusLabel.TabIndex = 34;
            this.GenerationStatusLabel.Text = "GenerationStatusLabel";
            this.GenerationStatusLabel.Visible = false;
            // 
            // UpscaleStatusLabel
            // 
            this.UpscaleStatusLabel.AutoSize = true;
            this.UpscaleStatusLabel.Location = new System.Drawing.Point(576, 702);
            this.UpscaleStatusLabel.Name = "UpscaleStatusLabel";
            this.UpscaleStatusLabel.Size = new System.Drawing.Size(153, 20);
            this.UpscaleStatusLabel.TabIndex = 35;
            this.UpscaleStatusLabel.Text = "UpscaleStatusLabel";
            this.UpscaleStatusLabel.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ImageUpscaleTitleLabel);
            this.panel1.Location = new System.Drawing.Point(555, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 648);
            this.panel1.TabIndex = 36;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.TextToImageTitleLabel);
            this.panel3.Location = new System.Drawing.Point(18, 92);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(488, 587);
            this.panel3.TabIndex = 37;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 769);
            this.Controls.Add(this.UpscaleStatusLabel);
            this.Controls.Add(this.GenerationStatusLabel);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.ApiKeyLabel);
            this.Controls.Add(this.MainTitleLabel);
            this.Controls.Add(this.lblUpscaleCreativity);
            this.Controls.Add(this.lblUpscaleOutputFormat);
            this.Controls.Add(this.lblUpscaleNegativePrompt);
            this.Controls.Add(this.lblUpscaleSeed);
            this.Controls.Add(this.lblUpscalePrompt);
            this.Controls.Add(this.lblImagePath);
            this.Controls.Add(this.lblOutputFormat);
            this.Controls.Add(this.lblSeed);
            this.Controls.Add(this.lblAspectRatio);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.lblImageCount);
            this.Controls.Add(this.lblNegativePrompt);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.btnDownloadUpscaledImages);
            this.Controls.Add(this.btnUpscale);
            this.Controls.Add(this.nudUpscaleCreativity);
            this.Controls.Add(this.cmbUpscaleOutputFormat);
            this.Controls.Add(this.txtUpscaleNegativePrompt);
            this.Controls.Add(this.nudUpscaleSeed);
            this.Controls.Add(this.txtUpscalePrompt);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cmbOutputFormat);
            this.Controls.Add(this.nudSeed);
            this.Controls.Add(this.cmbAspectRatio);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.nudImageCount);
            this.Controls.Add(this.txtNegativePrompt);
            this.Controls.Add(this.txtPrompt);
            this.Controls.Add(this.lblApiKeyStatus);
            this.Controls.Add(this.btnSaveApiKey);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Stable Diffusion 3";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudImageCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpscaleSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpscaleCreativity)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.TextBox txtNegativePrompt;
        private System.Windows.Forms.NumericUpDown nudImageCount;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.ComboBox cmbAspectRatio;
        private System.Windows.Forms.NumericUpDown nudSeed;
        private System.Windows.Forms.ComboBox cmbOutputFormat;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.TextBox txtUpscalePrompt;
        private System.Windows.Forms.NumericUpDown nudUpscaleSeed;
        private System.Windows.Forms.TextBox txtUpscaleNegativePrompt;
        private System.Windows.Forms.ComboBox cmbUpscaleOutputFormat;
        private System.Windows.Forms.NumericUpDown nudUpscaleCreativity;
        private System.Windows.Forms.Button btnUpscale;
        private System.Windows.Forms.Button btnDownloadUpscaledImages;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblNegativePrompt;
        private System.Windows.Forms.Label lblImageCount;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lblAspectRatio;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.Label lblOutputFormat;
        private System.Windows.Forms.Label lblImagePath;
        private System.Windows.Forms.Label lblUpscalePrompt;
        private System.Windows.Forms.Label lblUpscaleSeed;
        private System.Windows.Forms.Label lblUpscaleNegativePrompt;
        private System.Windows.Forms.Label lblUpscaleOutputFormat;
        private System.Windows.Forms.Label lblUpscaleCreativity;
        private System.Windows.Forms.Label TextToImageTitleLabel;
        private System.Windows.Forms.Label ImageUpscaleTitleLabel;
        private System.Windows.Forms.Label MainTitleLabel;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Button btnSaveApiKey;
        private System.Windows.Forms.Label lblApiKeyStatus;
        private System.Windows.Forms.Label ApiKeyLabel;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label GenerationStatusLabel;
        private System.Windows.Forms.Label UpscaleStatusLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
    }
}