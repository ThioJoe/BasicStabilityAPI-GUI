using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public partial class TestingImagePreviewForm : Form
{
    private TableLayoutPanel tableLayoutPanel;

    public TestingImagePreviewForm()
    {
        InitializeComponent();
        LoadAndDisplayImages();
    }

    private void InitializeComponent()
    {
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 600);
        this.Text = "Testing Image Preview Form";

        this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };

        this.Controls.Add(this.tableLayoutPanel);
    }

    private void LoadAndDisplayImages()
    {
        //string imagePath = @"C:\Users\Joe\source\repos\BasicStabilityAPI\bin\Debug\Image Outputs\Image_20240420-010746_1.png";   //Rectangle
        string imagePath = @"C:\Users\Joe\source\repos\BasicStabilityAPI\bin\Debug\Image Outputs\Image_20240420-010716_2.png";   //Square
        int numberOfImages = 3;  // Set how many times you want to load the image

        List<byte[]> imageDataList = new List<byte[]>();  // Use a List instead of an array
        for (int i = 0; i < numberOfImages; i++)
        {
            imageDataList.Add(File.ReadAllBytes(imagePath));  // Add image data to the list
        }

        DisplayImagesInWindow(imageDataList);  // Pass the list to the display function
    }

    public void DisplayImagesInWindow(List<byte[]> imageDataList)
    {
        // Create a new form to display images. This serves as a container for our image layout.
        Form previewForm = new Form();
        previewForm.Text = "Generated Images"; // Set the window title.
        previewForm.ClientSize = new Size(800, 600); // Set the size of the window to 800x600 pixels.

        // Calculate the average dimensions of all images to determine the best layout.
        var (averageWidth, averageHeight) = CalculateAverageDimensions(imageDataList);
        // Determine how many rows and columns the grid should have based on image count and dimensions.
        var (rows, columns) = CalculateGridDimensions(imageDataList.Count, averageWidth, averageHeight);

        // Create a TableLayoutPanel which will organize images in a grid layout.
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        tableLayoutPanel.Dock = DockStyle.Fill; // Make the panel fill its parent container.
        tableLayoutPanel.RowCount = rows; // Set the number of rows in the grid.
        tableLayoutPanel.ColumnCount = columns; // Set the number of columns in the grid.

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
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = image; // Assign the image to the PictureBox.
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Set image mode to zoom.
                pictureBox.Dock = DockStyle.Fill; // Make the PictureBox fill its allocated cell in the grid.
                pictureBox.Margin = new Padding(5); // Set a margin around the PictureBox for spacing.
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
}
