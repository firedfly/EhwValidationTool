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

namespace BastiaansHwBotHelper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 1);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 2);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 3);
        }

        private void btn3dLeft_Click(object sender, EventArgs e)
        {
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 1);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 2);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomLeft, 3);
            _ = ToolLauncher.Launch(ToolType.GpuZ, ToolLocation.TopLeft);
        }

        private void btn2dRight_Click(object sender, EventArgs e)
        {
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 1);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 2);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 3);
        }

        private void btn3dRight_Click(object sender, EventArgs e)
        {
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 1);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 2);
            _ = ToolLauncher.Launch(ToolType.CpuZ, ToolLocation.BottomRight, 3);
            _ = ToolLauncher.Launch(ToolType.GpuZ, ToolLocation.TopRight);
        }
    }
}
