using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const string ApiKey = "YOUR_API_KEY";
        private const string OutputFolder = "Image Outputs";
        private const string UpscaleOutputFolder = "Upscale Outputs";

        public Form1()
        {
            InitializeComponent();
        }

        private const string ApiKeyFileName = "stability_key.txt";

        private string LoadApiKey()
        {
            if (File.Exists(ApiKeyFileName))
            {
                return File.ReadAllText(ApiKeyFileName);
            }
            else
            {
                File.WriteAllText(ApiKeyFileName, "");
                return "";
            }
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
                lblApiKeyStatus.Text = "Saved";
            }
        }

        private void btnSaveApiKey_Click(object sender, EventArgs e)
        {
            string apiKey = txtApiKey.Text.Trim();
            if (!string.IsNullOrEmpty(apiKey))
            {
                SaveApiKey(apiKey);
                lblApiKeyStatus.Text = "Saved";
                txtApiKey.Clear();
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
            var generatedImages = await GenerateImagesAsync(apiKey, imageParams, imageCount);

            if (generatedImages != null && generatedImages.Any())
            {
                SaveImagesToFile(generatedImages);
                DisplayImagesInWindow(generatedImages);
            }
            else
            {
                MessageBox.Show("No images were generated.");
            }
        }

        private async Task<List<Image>> GenerateImagesAsync(string apiKey, Dictionary<string, object> imageParams, int imageCount)
        {
            var generatedImages = new List<Image>();

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
                        // Explicitly set ContentType if necessary
                        stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                        // Explicitly create and set the ContentDisposition
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
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            var image = Image.FromStream(ms);
                            generatedImages.Add(image);
                        }
                    }
                    else
                    {
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        Console.WriteLine($"Error Details: {errorDetails}");
                    }
                }
            }

            return generatedImages;
        }



        private void SaveImagesToFile(List<Image> images)
        {
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            foreach (var image in images)
            {
                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
                string filename = $"SD3_{timestamp}.png";
                string filePath = Path.Combine(OutputFolder, filename);
                image.Save(filePath);
            }
        }

        private void DisplayImagesInWindow(List<Image> images)
        {
            var previewForm = new Form();
            previewForm.Text = "Generated Images";
            previewForm.ClientSize = new Size(800, 600);

            var flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoScroll = true;

            foreach (var image in images)
            {
                var pictureBox = new PictureBox();
                pictureBox.Image = image;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Width = 200;
                pictureBox.Height = 200;
                pictureBox.Margin = new Padding(5);

                flowLayoutPanel.Controls.Add(pictureBox);
            }

            previewForm.Controls.Add(flowLayoutPanel);
            previewForm.ShowDialog();
        }

        private async void btnUpscale_Click(object sender, EventArgs e)
        {
            string imagePath = txtImagePath.Text;
            string prompt = txtUpscalePrompt.Text;
            int seed = (int)nudUpscaleSeed.Value;
            string negativePrompt = txtUpscaleNegativePrompt.Text;
            string outputFormat = cmbUpscaleOutputFormat.SelectedItem.ToString();
            double creativity = (double)nudUpscaleCreativity.Value;

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
            var generationId = await SendUpscaleRequestAsync(imagePath, imageParams, apiKey);
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
            var undownloadedGenerationIds = GetUndownloadedGenerationIds(logFilePath);

            if (undownloadedGenerationIds.Any())
            {
                string apiKey = LoadApiKey();
                foreach (var generationId in undownloadedGenerationIds)
                {
                    var upscaledImage = await GetUpscaledImageAsync(generationId, apiKey);
                    if (upscaledImage != null)
                    {
                        SaveUpscaledImage(generationId, upscaledImage);
                        UpdateLogFile(logFilePath, generationId);
                    }
                }
            }
            else
            {
                MessageBox.Show("All upscaled images have already been downloaded.");
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

        private async Task<Image> GetUpscaledImageAsync(string generationId, string apiKey)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {apiKey}");
                httpClient.DefaultRequestHeaders.Add("accept", "image/*");

                var response = await httpClient.GetAsync($"https://api.stability.ai/v2beta/stable-image/upscale/creative/result/{generationId}");

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }

            return null;
        }

        private void SaveUpscaledImage(string generationId, Image upscaledImage)
        {
            string logFilePath = Path.Combine(UpscaleOutputFolder, "ID_Filename_Log.txt");
            string filename = GetFilenameFromLog(logFilePath, generationId);

            if (!string.IsNullOrEmpty(filename))
            {
                string fileExtension = Path.GetExtension(filename);
                string upscaledFilename = $"{Path.GetFileNameWithoutExtension(filename)}_upscaled{fileExtension}";
                string filePath = Path.Combine(UpscaleOutputFolder, upscaledFilename);

                upscaledImage.Save(filePath);
            }
        }

        private string GetFilenameFromLog(string logFilePath, string generationId)
        {
            if (File.Exists(logFilePath))
            {
                var logLines = File.ReadAllLines(logFilePath);
                foreach (var line in logLines)
                {
                    var logEntry = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(line);
                    if (logEntry.ContainsKey(generationId))
                    {
                        return (string)logEntry[generationId]["filename"];
                    }
                }
            }

            return null;
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