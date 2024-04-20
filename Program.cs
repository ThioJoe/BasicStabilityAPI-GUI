using BasicStabilityAPI;
using System;
using System.Windows.Forms;

namespace StableDiffusionWinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); //Uncomment this line to run the program normally
            //Application.Run(new TestingImagePreviewForm());  // Uncomment this line and comment to run the image preview test form
            Application.Run(new Form1());
        }
    }
}