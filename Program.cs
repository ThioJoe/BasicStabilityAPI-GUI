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
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestingImagePreviewForm());  // Run the testing form as the main form
            //Application.Run(new Form1());
        }
    }
}