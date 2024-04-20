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

        private void BtnSaveApiKey_Click(object sender, EventArgs e)
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

        private async void BtnGenerate_Click(object sender, EventArgs e)
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

            var imageDataList = await GenerateImagesAsync(apiKey, imageParams, imageCount);

            GenerationStatusLabel.Visible = false; // Hide the status label after response comes back, whether success or failure

            if (imageDataList != null && imageDataList.Any())
            {
                List<string> filenames = GenerateFilenames(imageCount, model, outputFormat);
                SaveImagesToFile(imageDataList, filenames, OutputFolder);
                LogGenerationRequest(prompt, negativePrompt, filenames, imageDataList.Select(detail => (string)detail["Seed"]).ToList());
                DisplayImagesInWindow(imageDataList.Select(detail => (byte[])detail["ImageData"]).ToList());
            }
            else
            {
                MessageBox.Show("No images were generated.");
            }
        }

        private List<string> GenerateFilenames(int imageCount, string model, string outputFormat)
        {
            List<string> filenames = new List<string>();
            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
            string fileExtension = outputFormat;

            for (int i = 0; i < imageCount; i++)
            {
                string filename = $"{model}_{timestamp}_{i}.{fileExtension}";
                filenames.Add(filename);
            }

            return filenames;
        }

        private void LogGenerationRequest(string prompt, string negativePrompt, List<string> filenames, List<string> seeds)
        {
            string logFilePath = Path.Combine(OutputFolder, "Image_Generation_Log.txt");
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            StringBuilder logEntry = new StringBuilder();

            for (int i = 0; i < filenames.Count; i++)
            {
                logEntry.AppendLine($"{filenames[i]}:");
                logEntry.AppendLine($"\tPrompt:\t\t\t{prompt.Trim()}");
                logEntry.AppendLine($"\tNegative Prompt:\t{negativePrompt.Trim()}");
                logEntry.AppendLine($"\tSeed:\t\t\t{seeds[i]}");
                logEntry.AppendLine(); // Adds an extra newline for spacing between entries
            }

            File.AppendAllText(logFilePath, logEntry.ToString());
        }

        private async Task<List<Dictionary<string, object>>> GenerateImagesAsync(string apiKey, Dictionary<string, object> imageParams, int imageCount)
        {
            var imageDataList = new List<Dictionary<string, object>>();

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
                    var imageData = new Dictionary<string, object>();
                    if (response.IsSuccessStatusCode)
                    {
                        
                        var imageBytes = await response.Content.ReadAsByteArrayAsync();
                        imageData.Add("ImageData", imageBytes);

                        // Extract seed from headers
                        string seedValue = response.Headers.GetValues("seed").FirstOrDefault();
                        imageData.Add("Seed", seedValue);  // Keep it as a string

                        imageDataList.Add(imageData);
                    }
                    else
                    {
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        Console.WriteLine($"Error Details: {errorDetails}");
                    }
                }
            }

            return imageDataList;
        }

        private void SaveImagesToFile(List<Dictionary<string, object>> imageDataList, List<string> filenames, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            for (int i = 0; i < imageDataList.Count; i++)
            {
                var imageData = (byte[])imageDataList[i]["ImageData"];
                string filePath = Path.Combine(outputFolder, filenames[i]);
                File.WriteAllBytes(filePath, imageData);
            }
        }

        public void DisplayImagesInWindow(List<byte[]> imageDataList)
        {
            // Create a new form to display images. This serves as a container for our image layout.
            Form previewForm = new Form
            {
                Text = "Generated Images", // Set the window title.
                ClientSize = new Size(800, 600) // Set the size of the window to 800x600 pixels.
            };

            // Calculate the average dimensions of all images to determine the best layout.
            var (averageWidth, averageHeight) = CalculateAverageDimensions(imageDataList);
            // Determine how many rows and columns the grid should have based on image count and dimensions.
            var (rows, columns) = CalculateGridDimensions(imageDataList.Count, averageWidth, averageHeight);

            // Create a TableLayoutPanel which will organize images in a grid layout.
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, // Make the panel fill its parent container.
                RowCount = rows, // Set the number of rows in the grid.
                ColumnCount = columns // Set the number of columns in the grid.
            };

            // Configure the layout of rows and columns to equally distribute space among them.
            // This loop sets up the style for each row in the TableLayoutPanel.
            for (int i = 0; i < rows; i++)
            {
                // Add a new RowStyle for each row in the table.
                // SizeType.Percent: The height of the row is a percentage of the total available space in the TableLayoutPanel.
                // 100F / rows: This calculates the percentage each row should occupy of the total TableLayoutPanel height.
                // For example, if there are 4 rows, each row is set to take 25% (100/4) of the height.
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rows));
            }

            // This loop sets up the style for each column in the TableLayoutPanel.
            for (int j = 0; j < columns; j++)
            {
                // Add a new ColumnStyle for each column in the table.
                // SizeType.Percent: The width of the column is a percentage of the total available space in the TableLayoutPanel.
                // 100F / columns: This calculates the percentage each column should occupy of the total TableLayoutPanel width.
                // For example, if there are 3 columns, each column is set to take approximately 33.33% (100/3) of the width.
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / columns));
            }


            // Iterate over each image data, create a PictureBox for each, and add it to the grid.
            foreach (byte[] imageData in imageDataList)
            {
                using (var ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms); // Load the image from its byte array.
                    PictureBox pictureBox = new PictureBox
                    {
                        Image = image, // Assign the image to the PictureBox.
                        SizeMode = PictureBoxSizeMode.Zoom, // Set image mode to zoom.
                        Dock = DockStyle.Fill, // Make the PictureBox fill its allocated cell in the grid.
                        Margin = new Padding(5) // Set a margin around the PictureBox for spacing.
                    };
                    tableLayoutPanel.Controls.Add(pictureBox); // Add the PictureBox to the grid.
                }
            }

            // Add the TableLayoutPanel to the form and display the form as a modal dialog.
            previewForm.Controls.Add(tableLayoutPanel);
            previewForm.ShowDialog();
        }

        private (double averageWidth, double averageHeight) CalculateAverageDimensions(List<byte[]> imageDataList)
        {
            double totalWidth = 0, totalHeight = 0; // Variables to hold the total width and height of all images.
            int count = 0; // Counter for the number of images processed.

            // Iterate through each byte array representing an image.
            foreach (byte[] imageData in imageDataList)
            {
                using (var ms = new MemoryStream(imageData))
                {
                    using (Image img = Image.FromStream(ms)) // Load image from byte array.
                    {
                        totalWidth += img.Width; // Add the width of this image to the total width.
                        totalHeight += img.Height; // Add the height of this image to the total height.
                        count++; // Increment the image count.
                    }
                }
            }

            // Calculate average width and height by dividing total dimensions by the number of images.
            return (totalWidth / count, totalHeight / count);
        }


        private (int rows, int columns) CalculateGridDimensions(int numImages, double averageWidth, double averageHeight)
        {
            double aspectRatio = averageWidth / averageHeight; // Calculate the aspect ratio of the images.
            int grid_size = (int)Math.Ceiling(Math.Sqrt(numImages)); // Calculate the size of the grid based on the number of images.

            int rows, columns; // Variables to hold the number of rows and columns.

            // Determine the layout based on the number of images and grid size.
            if (numImages <= grid_size * (grid_size - 1))
            {
                rows = Math.Min(numImages, grid_size - 1);
                columns = (int)Math.Ceiling((double)numImages / rows);
            }
            else
            {
                rows = columns = grid_size; // Use a square grid if the above condition doesn't apply.
            }

            // Adjust rows and columns based on the aspect ratio to optimize the layout.
            if (aspectRatio > 1.5)
            {
                // If the aspect ratio indicates a more horizontal shape, swap rows and columns.
                (rows, columns) = (columns, rows);
            }

            return (rows, columns); // Return the number of

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


        private async void BtnUpscale_Click(object sender, EventArgs e)
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
                MessageBox.Show($"Upscale request sent. Generation ID:\n{generationId}\n\nWait a short while for it to process, then click the other button to download the result.");
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

        private async void BtnDownloadUpscaledImages_Click(object sender, EventArgs e)
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
                    else if (!success && message.Contains("still being processed"))
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