using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StableDiffusionWinForms
{
    public partial class Form1 : Form
    {
        private const string OutputFolder = "Image Outputs";
        private const string UpscaleOutputFolder = "Upscale Outputs";

        public Form1()
        {
            InitializeComponent();
            cmbOutputFormat.SelectedIndex = 0; // Set the default output format to PNG
            cmbUpscaleOutputFormat.SelectedIndex = 0; // Set the default output format to PNG
            cmbAspectRatio.SelectedIndex = 0; // Set the default aspect ratio to 1:1
            cmbModel.SelectedIndex = 0; // Set the default model to "sd3"
        }

        private const string ApiKeyFileName = "stability_key.txt";

        private string LoadApiKey()
        {
            if (!File.Exists(ApiKeyFileName))
            {
                // Create an empty file if it doesn't exist
                File.WriteAllText(ApiKeyFileName, "");
                ShowError($"Missing or Invalid API key. Add API key to {ApiKeyFileName}.");
                return "";
            }

            var lines = File.ReadAllLines(ApiKeyFileName);
            foreach (var line in lines)
            {
                // Skip empty lines and lines starting with '#'
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                // Check if the line starts with "sk-"
                if (line.StartsWith("sk-"))
                    HideError(); // Hide the error message if a valid API key is found
                    return line.Trim();
            }

            // If we get here, no valid API key was found
            ShowError($"Valid API key not found in the file: {ApiKeyFileName}.");
            return "";
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.ForeColor = Color.Red; // Set the text color to red to indicate an error
            lblError.Visible = true; // Ensure the label is visible if it was previously hidden
        }

        private void HideError()
        {
            lblError.Visible = false;  // Hide the error label
        }

        private void SaveApiKey(string apiKey)
        {
            File.WriteAllText(ApiKeyFileName, apiKey);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string apiKey = LoadApiKey();
            if (!string.IsNullOrEmpty(apiKey))
            {
                lblApiKeyStatus.Text = "API Key Loaded.";
            }
        }

        private void btnSaveApiKey_Click(object sender, EventArgs e)
        {
            string apiKey = txtApiKey.Text.Trim();
            if (!string.IsNullOrEmpty(apiKey))
            {
                SaveApiKey(apiKey);
                string savedApiKey = LoadApiKey();
                if (apiKey == savedApiKey)
                {
                    lblApiKeyStatus.Text = "Saved";
                    txtApiKey.Clear();
                }
                else
                {
                    lblApiKeyStatus.Text = "Problem while saving API key.";
                }
            }
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text;
            string negativePrompt = txtNegativePrompt.Text;
            int imageCount = (int)nudImageCount.Value;
            string model = cmbModel.SelectedItem.ToString().ToLower();
            string aspectRatio = cmbAspectRatio.SelectedItem.ToString();
            int seed = (int)nudSeed.Value;
            string mode = "text-to-image";
            string outputFormat = cmbOutputFormat.SelectedItem.ToString();

            // Validation Checks
            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Error: No prompt text entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the function if the prompt is empty
            }

            var imageParams = new Dictionary<string, object>
            {
                { "model", model },
                { "mode", mode },
                { "aspect_ratio", aspectRatio },
                { "prompt", prompt },
                { "negative_prompt", negativePrompt },
                { "seed", seed },
                { "output_format", outputFormat }
            };

            string apiKey = LoadApiKey();

            // Set Status Label
            GenerationStatusLabel.Text = "Generating...";
            GenerationStatusLabel.ForeColor = Color.Blue;
            GenerationStatusLabel.Visible = true;

            var generatedImages = await GenerateImagesAsync(apiKey, imageParams, imageCount);

            GenerationStatusLabel.Visible = false; // Hide the status label after response comes back, whether success or failure

            if (generatedImages != null && generatedImages.Any())
            {
                SaveImagesToFile(generatedImages, (string)imageParams["output_format"], OutputFolder);
                DisplayImagesInWindow(generatedImages);
            }
            else
            {
                MessageBox.Show("No images were generated.");
            }
        }

        private async Task<List<byte[]>> GenerateImagesAsync(string apiKey, Dictionary<string, object> imageParams, int imageCount)
        {
            var generatedImagesData = new List<byte[]>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                httpClient.DefaultRequestHeaders.Add("Accept", "image/*");

                var tasks = new List<Task<HttpResponseMessage>>();

                for (int i = 0; i < imageCount; i++)
                {
                    var content = new MultipartFormDataContent();

                    // Add parameters as StringContent with their respective keys
                    foreach (var param in imageParams)
                    {
                        var stringContent = new StringContent(param.Value.ToString());
                        stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                        stringContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = $"\"{param.Key}\""
                        };
                        content.Add(stringContent);
                    }

                    var task = httpClient.PostAsync("https://api.stability.ai/v2beta/stable-image/generate/sd3", content);
                    tasks.Add(task);
                }

                var responses = await Task.WhenAll(tasks);

                foreach (var response in responses)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var imageBytes = await response.Content.ReadAsByteArrayAsync();
                        generatedImagesData.Add(imageBytes);
                    }
                    else
                    {
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        Console.WriteLine($"Error Details: {errorDetails}");
                    }
                }
            }

            return generatedImagesData;
        }


        private void SaveBase64ImageToFile(string base64Image, string filePath)
        {
            // Convert base64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Write byte array directly to file
            File.WriteAllBytes(filePath, imageBytes);
        }

        private void SaveImageBytesToFile(byte[] imageBytes, string filePath)
        {
            File.WriteAllBytes(filePath, imageBytes);
        }


        private void SaveImagesToFile(List<byte[]> imageDatas, string outputFormat, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            int counter = 1;
            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");

            foreach (var imageData in imageDatas)
            {
                string fileExtension = outputFormat.Equals("jpeg", StringComparison.OrdinalIgnoreCase) ? ".jpeg" : ".png";
                string filename = $"Image_{timestamp}_{counter}{fileExtension}";
                string filePath = Path.Combine(outputFolder, filename);

                SaveImageBytesToFile(imageData, filePath);
                counter++;
            }
        }


        private void SaveImageWithFormat(Image image, string filePath, string outputFormat)
        {
            ImageFormat format = ImageFormat.Png; // Default to PNG
            if (outputFormat.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
            {
                format = ImageFormat.Jpeg;
            }

            if (format == ImageFormat.Jpeg)
            {
                // Create encoder parameters with high quality
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);  // Set the quality to 95

                ImageCodecInfo jpegCodecInfo = GetEncoderInfo("image/jpeg");
                if (jpegCodecInfo == null)
                {
                    throw new InvalidOperationException("JPEG codec not found.");
                }

                // Use the encoder info to save the image
                image.Save(filePath, jpegCodecInfo, encoderParams);
            }
            else
            {
                image.Save(filePath, format);
            }
        }

        // Helper method to get encoder info based on MIME type
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
            {
                if (codec.MimeType == mimeType)
                    return codec;
            }
            return null;
        }


        private void DisplayImagesInWindow(List<byte[]> imageDatas)
        {
            var previewForm = new Form();
            previewForm.Text = "Generated Images";
            previewForm.ClientSize = new Size(800, 600);

            var flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoScroll = true;

            foreach (var imageData in imageDatas)
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var image = Image.FromStream(ms);
                    var pictureBox = new PictureBox();
                    pictureBox.Image = image;
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Width = 200;
                    pictureBox.Height = 200;
                    pictureBox.Margin = new Padding(5);

                    flowLayoutPanel.Controls.Add(pictureBox);
                }
            }

            previewForm.Controls.Add(flowLayoutPanel);
            previewForm.ShowDialog();
        }


        private string TrimQuotes(string input)
        {
            if (input.StartsWith("\"") && input.EndsWith("\"") && input.Length > 1)
            {
                return input.Substring(1, input.Length - 2);
            }
            else if (input.StartsWith("'") && input.EndsWith("'") && input.Length > 1)
            {
                return input.Substring(1, input.Length - 2);
            }
            return input;
        }


        private async void btnUpscale_Click(object sender, EventArgs e)
        {
            string imagePath = TrimQuotes(txtImagePath.Text.Trim()); // Clean and trim the input path
            string prompt = txtUpscalePrompt.Text;
            int seed = (int)nudUpscaleSeed.Value;
            string negativePrompt = txtUpscaleNegativePrompt.Text;
            string outputFormat = cmbUpscaleOutputFormat.SelectedItem.ToString();
            double creativity = (double)nudUpscaleCreativity.Value;

            // Validation Checks for empty required boxes
            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Error: No prompt text entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the function if the prompt is empty
            }
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                MessageBox.Show("Error: No existing image path entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the function if the prompt is empty
            }
            if (!File.Exists(imagePath))
            {
                MessageBox.Show("Error: The provided image file path does not seem to exist, please check it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the function if the prompt is empty
            }

            var imageParams = new Dictionary<string, object>
            {
                { "image_path", imagePath },
                { "prompt", prompt },
                { "seed", seed },
                { "negative_prompt", negativePrompt },
                { "output_format", outputFormat },
                { "creativity", creativity }
            };

            string apiKey = LoadApiKey();

            // Set status label
            UpscaleStatusLabel.Text = "Upscaling...";
            UpscaleStatusLabel.ForeColor = Color.Blue;
            UpscaleStatusLabel.Visible = true;

            var generationId = await SendUpscaleRequestAsync(imagePath, imageParams, apiKey);
            UpscaleStatusLabel.Visible = false; // Hide the status label after response comes back, whether success or failure

            if (!string.IsNullOrEmpty(generationId))
            {
                LogUpscaleRequest(generationId, imagePath, imageParams);
                MessageBox.Show($"Upscale request sent. Generation ID: {generationId}");
            }
            else
            {
                MessageBox.Show("Failed to send upscale request.");
            }
        }

        private async Task<string> SendUpscaleRequestAsync(string imagePath, Dictionary<string, object> imageParams, string apiKey)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                httpClient.DefaultRequestHeaders.Add("Accept", "image/*");

                var content = new MultipartFormDataContent();

                // Send the image file separately
                var imageStream = File.OpenRead(imagePath);
                var imageContent = new StreamContent(imageStream);
                imageContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"image\"",
                    FileName = $"\"{Path.GetFileName(imagePath)}\""
                };
                content.Add(imageContent);

                // Add the other parameters
                foreach (var param in imageParams)
                {
                    if (param.Key != "image_path") // Exclude the image_path parameter
                    {
                        var stringContent = new StringContent(param.Value.ToString());
                        stringContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = $"\"{param.Key}\""
                        };
                        content.Add(stringContent);
                    }
                }

                var response = await httpClient.PostAsync("https://api.stability.ai/v2beta/stable-image/upscale/creative", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    return responseJson["id"];
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorDetails}");
                    // Handle the error appropriately (e.g., show an error message to the user)
                }
            }

            return null;
        }


        private void LogUpscaleRequest(string generationId, string imagePath, Dictionary<string, object> imageParams)
        {
            if (!Directory.Exists(UpscaleOutputFolder))
            {
                Directory.CreateDirectory(UpscaleOutputFolder);
            }

            var logEntry = new Dictionary<string, object>
            {
                { generationId, new Dictionary<string, object>
                    {
                        { "filename", Path.GetFileName(imagePath) },
                        { "downloaded", false },
                        { "datetime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "seed", imageParams["seed"] },
                        { "negative_prompt", imageParams["negative_prompt"] },
                        { "output_format", imageParams["output_format"] },
                        { "creativity", imageParams["creativity"] }
                    }
                }
            };

            string logFilePath = Path.Combine(UpscaleOutputFolder, "ID_Filename_Log.txt");
            File.AppendAllText(logFilePath, JsonConvert.SerializeObject(logEntry) + Environment.NewLine);
        }

        private async void btnDownloadUpscaledImages_Click(object sender, EventArgs e)
        {
            string logFilePath = Path.Combine(UpscaleOutputFolder, "ID_Filename_Log.txt");

            // Set Status Label
            UpscaleStatusLabel.Text = "Checking for undownloaded files...";
            UpscaleStatusLabel.ForeColor = Color.Blue;
            UpscaleStatusLabel.Visible = true;

            var undownloadedGenerationIds = GetUndownloadedGenerationIds(logFilePath);

            if (undownloadedGenerationIds.Any())
            {
                string apiKey = LoadApiKey();

                // Update status label with the number of images to download
                UpscaleStatusLabel.Text = $"Downloading {undownloadedGenerationIds.Count()} images...";
                UpscaleStatusLabel.Refresh(); // Optionally force the label to refresh the display

                var failedGenerationIds = new List<string>();
                var stillProcessingIds = new List<string>();
                var errorMessages = new Dictionary<string, string>(); // Dictionary to keep track of error messages

                foreach (var generationId in undownloadedGenerationIds)
                {
                    var (success, upscaledImageData, message) = await GetUpscaledImageAsync(generationId, apiKey);
                    if (success && upscaledImageData != null)
                    {
                        SaveUpscaledImage(generationId, upscaledImageData);
                        UpdateLogFile(logFilePath, generationId);
                    }
                    else if (!success && message.Contains("still being generated"))
                    {
                        stillProcessingIds.Add(generationId);
                        errorMessages[generationId] = null;
                    }
                    else
                    {
                        failedGenerationIds.Add(generationId);
                        errorMessages[generationId] = message; // Store the error message for this generation ID
                    }
                }

                UpscaleStatusLabel.Visible = false; // Hide the status label after response comes back, whether success or failure

                // Build the message box contents based on the operation results
                string resultMessage = $"Downloaded {undownloadedGenerationIds.Count - failedGenerationIds.Count - stillProcessingIds.Count} images successfully.";
                if (failedGenerationIds.Any())
                {
                    resultMessage += $"\n\nGeneration IDs that failed for other reasons:\n";
                    foreach (var id in failedGenerationIds)
                    {
                        resultMessage += $"{id}: {errorMessages[id]}\n"; // Include the specific error message for each failed ID
                    }
                }
                if (stillProcessingIds.Any())
                {
                    resultMessage += $"\n\nGeneration IDs Still Processing:\n{string.Join("\n", stillProcessingIds)}";
                }

                MessageBox.Show(resultMessage);
            }
            else
            {
                MessageBox.Show("All upscaled images had already been downloaded, so none were downloaded now.");
            }
        }




        private List<string> GetUndownloadedGenerationIds(string logFilePath)
        {
            var undownloadedGenerationIds = new List<string>();

            if (File.Exists(logFilePath))
            {
                var logLines = File.ReadAllLines(logFilePath);
                foreach (var line in logLines)
                {
                    var logEntry = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(line);
                    var generationId = logEntry.Keys.First();
                    var downloaded = (bool)logEntry[generationId]["downloaded"];

                    if (!downloaded)
                    {
                        undownloadedGenerationIds.Add(generationId);
                    }
                }
            }

            return undownloadedGenerationIds;
        }

        private async Task<(bool success, byte[] imageData, string message)> GetUpscaledImageAsync(string generationId, string apiKey)
        {
            string errorMessage = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {apiKey}");
                httpClient.DefaultRequestHeaders.Add("accept", "image/*");

                var response = await httpClient.GetAsync($"https://api.stability.ai/v2beta/stable-image/upscale/creative/result/{generationId}");

                if (response.IsSuccessStatusCode)
                {
                    if ((int)response.StatusCode == 200)
                    {
                        var imageBytes = await response.Content.ReadAsByteArrayAsync();
                        return (true, imageBytes, null);
                    }
                    else if ((int)response.StatusCode == 202)
                    {
                        // The image is still being generated, so we can skip this one
                        errorMessage = $"Image ID: {generationId} is still being processed.";
                        return (false, null, errorMessage);
                    }
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorDetails}");
                    // Optionally display the error message to the user
                    MessageBox.Show($"Failed to download upscaled image with Generation ID: {generationId}\n\nError Details: {errorDetails}");
                    errorMessage = "Failed to download upscaled image. " + errorDetails;
                }
            }

            errorMessage = errorMessage ?? "Something unexpected happened. Please try again.";
            return (false, null, errorMessage);
        }


        private void SaveUpscaledImage(string generationId, byte[] upscaledImageData)
        {
            string logFilePath = Path.Combine(UpscaleOutputFolder, "ID_Filename_Log.txt");
            var fileDetails = GetFileDetailsFromLog(logFilePath, generationId);

            if (fileDetails != null && fileDetails.ContainsKey("filename") && fileDetails.ContainsKey("output_format"))
            {
                string originalFilename = (string)fileDetails["filename"];
                string outputFormat = (string)fileDetails["output_format"];
                string fileExtension = $".{outputFormat}";
                string upscaledFilenameNoExtension = $"{Path.GetFileNameWithoutExtension(originalFilename)}_upscaled";

                int index = 2;
                string upscaledFilename = $"{upscaledFilenameNoExtension}{fileExtension}";
                string filePath = Path.Combine(UpscaleOutputFolder, upscaledFilename);

                // Check if the file exists and increment the filename index until it doesn't
                while (File.Exists(filePath))
                {
                    upscaledFilename = $"{upscaledFilenameNoExtension}_{index}{fileExtension}";
                    filePath = Path.Combine(UpscaleOutputFolder, upscaledFilename);
                    index++;
                }

                File.WriteAllBytes(filePath, upscaledImageData);  // Save the byte array directly to the file
            }
        }



        private Dictionary<string, object> GetFileDetailsFromLog(string logFilePath, string generationId)
        {
            if (File.Exists(logFilePath))
            {
                var logLines = File.ReadAllLines(logFilePath);
                foreach (var line in logLines)
                {
                    var logEntry = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(line);
                    if (logEntry.ContainsKey(generationId))
                    {
                        return logEntry[generationId]; // Return the entire dictionary for the generationId
                    }
                }
            }

            return null; // Return null if no entry is found
        }


        private void UpdateLogFile(string logFilePath, string generationId)
        {
            if (File.Exists(logFilePath))
            {
                var logLines = File.ReadAllLines(logFilePath).ToList();
                for (int i = 0; i < logLines.Count; i++)
                {
                    var logEntry = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(logLines[i]);
                    if (logEntry.ContainsKey(generationId))
                    {
                        logEntry[generationId]["downloaded"] = true;
                        logLines[i] = JsonConvert.SerializeObject(logEntry);
                        break;
                    }
                }

                File.WriteAllLines(logFilePath, logLines);
            }
        }
    }
}