using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public MainForm()
        {
            InitializeComponent();
            Instance = this;

            Version version = default;
            Version.TryParse(Application.ProductVersion, out version);
            this.lblVersion.Text = version == default ? "" : $"Version: {version.Major}.{version.Minor}.{version.Build}";

            this.chkSlowMode.Checked = Settings.Default.EnableSlowMode;
            this.chkSlowMode.CheckedChanged += chkSlowMode_CheckedChanged;

            this.chkShowSpdTabsSlot12.Checked = Settings.Default.EnableSpdTabsSlot1Slot2;
            this.chkShowSpdTabsSlot24.Checked = Settings.Default.EnableSpdTabsSlot2Slot4;
            this.chkShowSpdTabsSlot12.CheckedChanged += chkShowSpdTabs_CheckedChanged;
            this.chkShowSpdTabsSlot24.CheckedChanged += chkShowSpdTabs_CheckedChanged;

            updateSavedLayoutControls();
        }

        public void CloseTools()
        {
            userInfoForm.Hide();
            ToolLauncher.CloseTools();
            updateSavedLayoutControls();
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

            await TaskEx.Delay(250);
            var bounds = Screen.PrimaryScreen.Bounds;
            using(var bitmap = new Bitmap(bounds.Width, bounds.Height))
            using(var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                bitmap.Save(screenshotFilename, ImageFormat.Jpeg);
            }

            this.WindowState = originalState;
        }

        private bool ensureToolExists(ToolType toolType)
        {
            var exists = File.Exists(ToolLauncher.GetToolPath(toolType));
            if (!exists)
                MessageBox.Show($"{toolType} not found.  Update the settings with the correct path.");
            return exists;
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

        private List<ToolLaunchInfo> getSpdToToolLaunchInfo(ToolLocation toolLocation)
        {
            var tools = new List<ToolLaunchInfo>();

            if (chkShowSpdTabsSlot12.Checked || chkShowSpdTabsSlot24.Checked)
            {
                int firstSlot = chkShowSpdTabsSlot12.Checked ? 1 : 2;
                int secondSlot = chkShowSpdTabsSlot12.Checked ? 2 : 4;

                if(toolLocation == ToolLocation.TopRight || toolLocation == ToolLocation.BottomRight)
                {
                    var tmp = firstSlot;
                    firstSlot = secondSlot;
                    secondSlot = tmp;
                }

                tools.Add(new CpuzLaunchInfo { ToolLocation = toolLocation, InstanceNumber = 1, TabType = CpuzLaunchInfo.CpuzTabType.SPD, SpdSlot = firstSlot });
                tools.Add(new CpuzLaunchInfo { ToolLocation = toolLocation, InstanceNumber = 2, TabType = CpuzLaunchInfo.CpuzTabType.SPD, SpdSlot = secondSlot });
            }

            return tools;
        }

        private void updateSavedLayoutControls()
        {
            if (Settings.Default.SavedLayout != null && Settings.Default.SavedLayout.Count > 0)
            {
                btnLaunchSavedLayout.Enabled = true;
            }
            else
            {
                btnLaunchSavedLayout.Enabled = false;
            }

            btnSaveLayout.Enabled = ToolLauncher.HasOpenTools;
        }

        private async Task launchSavedLayout()
        {
            if (Settings.Default.SavedLayout == null || Settings.Default.SavedLayout.Count == 0)
                return;

            var openTools = await ToolLauncher.LaunchToolsFromSavedLayout(Settings.Default.SavedLayout, chkSlowMode.Checked);

            if(Settings.Default.UserInfoSavedLayout != null)
            {
                userInfoForm.Location = Settings.Default.UserInfoSavedLayout.Location;
                userInfoForm.Size = Settings.Default.UserInfoSavedLayout.Size;
                userInfoForm.Show();
            }

            bool userInfoHandled = false;
            foreach(var tool in Settings.Default.SavedLayout.OrderByDescending(x=>x.ZOrder))
            {
                if(!userInfoHandled && tool.ZOrder < Settings.Default.UserInfoZOrder)
                {
                    Win32Interop.BringWindowToFront(userInfoForm.Handle);
                    userInfoHandled = true;
                }

                var openTool = getOpenToolFromSavedLayout(tool, openTools);
                Win32Interop.BringWindowToFront(ToolLauncher.GetMainWindowFromProcessId(openTool, openTool.Process.Id));
            }

            if (!userInfoHandled)
            {
                Win32Interop.BringWindowToFront(userInfoForm.Handle);
            }

            updateSavedLayoutControls();
        }

        private ToolLaunchInfo getOpenToolFromSavedLayout(ToolSaveInfo toolSaveInfo, List<ToolLaunchInfo> openTools)
        {
            if(toolSaveInfo.ToolType == ToolType.CpuZ)
            {
                var cpuzSaveInfo = (CpuzSaveInfo)toolSaveInfo;
                return openTools.Where(x => x is CpuzLaunchInfo).Select(x => (CpuzLaunchInfo)x).FirstOrDefault(x => x.TabType == cpuzSaveInfo.TabType && x.SpdSlot == cpuzSaveInfo.SpdSlot);
            }
            else
            {
                return openTools.FirstOrDefault(x=>x.ToolType == toolSaveInfo.ToolType);
            }
        }

        private async void btn2dLeft_Click(object sender, EventArgs e)
        {
            if (!ensureToolExists(ToolType.CpuZ))
                return;

            var tools = new List<ToolLaunchInfo>
            {
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomLeft, InstanceNumber = 1, TabType = CpuzLaunchInfo.CpuzTabType.CPU },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomLeft, InstanceNumber = 2, TabType = CpuzLaunchInfo.CpuzTabType.Mainboard },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomLeft, InstanceNumber = 3, DisplayUserInfoAboveWindow = true, TabType = CpuzLaunchInfo.CpuzTabType.Memory }
            };
            tools.AddRange(getSpdToToolLaunchInfo(ToolLocation.TopLeft));

            await ToolLauncher.LaunchTools(tools, chkSlowMode.Checked);
            updateSavedLayoutControls();
        }

        private async void btn3dLeft_Click(object sender, EventArgs e)
        {
            if (!ensureToolExists(ToolType.CpuZ))
                return;
            if (!ensureToolExists(ToolType.GpuZ))
                return;

            var tools = new List<ToolLaunchInfo>
            {
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomLeft, InstanceNumber = 1, DisplayUserInfoAboveWindow = true, TabType = CpuzLaunchInfo.CpuzTabType.CPU },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomLeft, InstanceNumber = 2, TabType = CpuzLaunchInfo.CpuzTabType.Mainboard },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomLeft, InstanceNumber = 3, TabType = CpuzLaunchInfo.CpuzTabType.Memory },
                new ToolLaunchInfo { ToolType = ToolType.GpuZ, ToolLocation = ToolLocation.TopLeft}
            };

            await ToolLauncher.LaunchTools(tools, chkSlowMode.Checked);
            updateSavedLayoutControls();
        }

        private async void btn2dRight_Click(object sender, EventArgs e)
        {
            if (!ensureToolExists(ToolType.CpuZ))
                return;

            var tools = new List<ToolLaunchInfo>
            {
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomRight, InstanceNumber = 1, DisplayUserInfoAboveWindow = true, TabType = CpuzLaunchInfo.CpuzTabType.Memory },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomRight, InstanceNumber = 2, TabType = CpuzLaunchInfo.CpuzTabType.Mainboard },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomRight, InstanceNumber = 3, TabType = CpuzLaunchInfo.CpuzTabType.CPU }
            };
            tools.AddRange(getSpdToToolLaunchInfo(ToolLocation.TopRight));

            await ToolLauncher.LaunchTools(tools, chkSlowMode.Checked);
            updateSavedLayoutControls();
        }

        private async void btn3dRight_Click(object sender, EventArgs e)
        {
            if (!ensureToolExists(ToolType.CpuZ))
                return;
            if (!ensureToolExists(ToolType.GpuZ))
                return;

            var tools = new List<ToolLaunchInfo>
            {
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomRight, InstanceNumber = 1, DisplayUserInfoAboveWindow = true, TabType = CpuzLaunchInfo.CpuzTabType.Memory },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomRight, InstanceNumber = 2, TabType = CpuzLaunchInfo.CpuzTabType.Mainboard },
                new CpuzLaunchInfo { ToolLocation = ToolLocation.BottomRight, InstanceNumber = 3, TabType = CpuzLaunchInfo.CpuzTabType.CPU },
                new ToolLaunchInfo { ToolType = ToolType.GpuZ, ToolLocation = ToolLocation.TopRight }
            };

            await ToolLauncher.LaunchTools(tools, chkSlowMode.Checked);
            updateSavedLayoutControls();
        }

        private void btnCloseTools_Click(object sender, EventArgs e)
        {
            CloseTools();
        }

        private void btnTakeScreenshot_Click(object sender, EventArgs e)
        {
            _ = TakeScreenshot();
        }


        private void chkSlowMode_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.EnableSlowMode = chkSlowMode.Checked;
            Settings.SaveSettings();
        }
        private void chkShowSpdTabs_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.EnableSpdTabsSlot1Slot2 = chkShowSpdTabsSlot12.Checked;
            Settings.Default.EnableSpdTabsSlot2Slot4 = chkShowSpdTabsSlot24.Checked;
            Settings.SaveSettings();
        }

        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            if (!ToolLauncher.HasOpenTools)
                return;


            Settings.Default.SavedLayout = ToolLauncher.GetSaveInfoForTools();
            Settings.Default.UserInfoSavedLayout = new Rectangle(userInfoForm.Location, userInfoForm.Size);
            Settings.Default.UserInfoZOrder = Win32Interop.GetWindowZOrder(userInfoForm.Handle);
            Settings.SaveSettings();
            updateSavedLayoutControls();
        }

        private void btnLaunchSavedLayout_Click(object sender, EventArgs e)
        {
            _ = launchSavedLayout();
        }
    }
}
