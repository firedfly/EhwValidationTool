using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace EhwValidationTool
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        private readonly UserInfoForm userInfoForm = new UserInfoForm();

        List<Process> processes = new List<Process>();

        public MainForm()
        {
            InitializeComponent();
            Instance = this;

            Version version = default;
            Version.TryParse(Application.ProductVersion, out version);
            this.lblVersion.Text = version == default ? "" : $"Version: {version.Major}.{version.Minor}.{version.Build}";
        }

        public void CloseTools()
        {
            userInfoForm.Hide();

            processes.ForEach(x =>
            {
                if (!x.HasExited)
                {
                    x.Kill();
                }
            });
            processes.Clear();
        }


        private async Task TakeScreenshot()
        {
            string screenshotFilename = String.Empty;
            using (var sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.FileName = "validation.jpg";
                sfd.DefaultExt = "jpg";
                sfd.Filter = "JPG (*.jpg)|*.jpg|All Files (*.*)|*.*";
                sfd.InitialDirectory = Settings.Default.ScreenshotFolder;
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                screenshotFilename = sfd.FileName;
            }

            var originalState = this.WindowState;
            this.WindowState = FormWindowState.Minimized;

            await Task.Delay(250);
            var bounds = Screen.PrimaryScreen.Bounds;
            using(var bitmap = new Bitmap(bounds.Width, bounds.Height))
            using(var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                bitmap.Save(screenshotFilename, ImageFormat.Jpeg);
            }

            this.WindowState = originalState;
        }


        public void ShowUserInfoForm(int left, int top, int width)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { ShowUserInfoForm(left, top, width); }));
                return;
            }

            userInfoForm.Left = left;
            userInfoForm.Top = top;
            userInfoForm.Width = width;
            userInfoForm.UpdateLabels();
            userInfoForm.Show();
        }

        public int GetUserInfoFormHeight()
        {
            if(InvokeRequired)
            {
                return (int)Invoke(new Func<int>(() => { return GetUserInfoFormHeight(); }));
            }
            return userInfoForm.Height;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog();
            }
        }

        private void btn2dLeft_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var tasks = new List<Task<Process>>();
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 1, true, selectTabIndex: 0));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 2, selectTabIndex: 1));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 3, selectTabIndex: 2));

            Task.WaitAll(tasks.ToArray());
            processes.AddRange(tasks.Select(x => x.Result));
        });
        }

        private void btn3dLeft_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var tasks = new List<Task<Process>>();
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 1, true, selectTabIndex: 0));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 2, selectTabIndex: 1));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 3, selectTabIndex: 2));
                tasks.Add(ToolLauncher.Launch(ToolType.GpuZ, ToolLocation.TopLeft));

                Task.WaitAll(tasks.ToArray());
                processes.AddRange(tasks.Select(x => x.Result));
            });
        }

        private void btn2dRight_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var tasks = new List<Task<Process>>();
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 1, true, selectTabIndex: 2));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 2, selectTabIndex: 1));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 3, selectTabIndex: 0));

                Task.WaitAll(tasks.ToArray());
                processes.AddRange(tasks.Select(x => x.Result));
            });
        }

        private void btn3dRight_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var tasks = new List<Task<Process>>();
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 1, true, selectTabIndex: 2));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 2, selectTabIndex: 1));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 3, selectTabIndex: 0));
                tasks.Add(ToolLauncher.Launch(ToolType.GpuZ, ToolLocation.TopRight));

                Task.WaitAll(tasks.ToArray());
                processes.AddRange(tasks.Select(x => x.Result));
            });
        }

        private void btnCloseTools_Click(object sender, EventArgs e)
        {
            CloseTools();
        }

        private void btnTakeScreenshot_Click(object sender, EventArgs e)
        {
            _ = TakeScreenshot();
        }
    }
}
