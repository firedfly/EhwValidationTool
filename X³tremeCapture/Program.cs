using System;
using System.Windows.Forms;

namespace XtremeCapture
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ApplicationExit += (object sender, EventArgs e) => { MainForm.Instance.CloseTools(); };
            Application.Run(new MainForm());
        }
    }
}
