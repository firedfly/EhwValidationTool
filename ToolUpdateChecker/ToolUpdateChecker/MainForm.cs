using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolUpdateChecker.Updaters;

namespace ToolUpdateChecker
{
    public partial class MainForm : Form
    {
        public static ChromiumWebBrowser WebBrowser { get; } = new ChromiumWebBrowser();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            var latestVersion = await CpuzUpdater.Instance.GetLatestAvailableVersion();
            MessageBox.Show(latestVersion.Version);
        }
    }
}
