using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 1, true));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 2));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 3));

            Task.WaitAll(tasks.ToArray());
            processes.AddRange(tasks.Select(x => x.Result));
        });
        }

        private void btn3dLeft_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var tasks = new List<Task<Process>>();
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 1, true));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 2));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 3));
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
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 1, true));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 2));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 3));

                Task.WaitAll(tasks.ToArray());
                processes.AddRange(tasks.Select(x => x.Result));
            });
        }

        private void btn3dRight_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var tasks = new List<Task<Process>>();
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 1, true));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 2));
                tasks.Add(ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 3));
                tasks.Add(ToolLauncher.Launch(ToolType.GpuZ, ToolLocation.TopRight));

                Task.WaitAll(tasks.ToArray());
                processes.AddRange(tasks.Select(x => x.Result));
            });
        }

        private void btnCloseTools_Click(object sender, EventArgs e)
        {
            CloseTools();
        }
    }
}
