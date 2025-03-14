﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace XtremeCapture
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            this.txtCpuzLocation.Text = Settings.Default.CpuzLocation;
            this.txtGpuzLocation.Text = Settings.Default.GpuzLocation;
            this.txtHwbotUser.Text = Settings.Default.HwbotUserName;
            this.txtHwbotTeam.Text = Settings.Default.HwbotTeamName;

            updateLocationLabels();
        }

        private void updateLocationLabels()
        {
            if (!File.Exists(this.txtCpuzLocation.Text))
                this.lblCpuz.ForeColor = Color.Red;
            else
                this.lblCpuz.ForeColor = Color.Black;

            if (!File.Exists(this.txtGpuzLocation.Text))
                this.lblGpuz.ForeColor = Color.Red;
            else
                this.lblGpuz.ForeColor = Color.Black;
        }

        private void btnCpuzBrowse_Click(object sender, EventArgs e)
        {
            using(var openFile = new OpenFileDialog())
            {
                openFile.Filter = "CPU-Z|cpuz.exe|All Files (*.*)|*.*";
                if(openFile.ShowDialog() == DialogResult.OK)
                {
                    this.txtCpuzLocation.Text = openFile.FileName;
                }
            }
        }

        private void btnGpuzBrowse_Click(object sender, EventArgs e)
        {
            using (var openFile = new OpenFileDialog())
            {
                openFile.Filter = "GPU-Z|gpu-z*.exe|All Files (*.*)|*.*";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    this.txtGpuzLocation.Text = openFile.FileName;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.CpuzLocation = this.txtCpuzLocation.Text;
            Settings.Default.GpuzLocation = this.txtGpuzLocation.Text;
            Settings.Default.HwbotUserName = this.txtHwbotUser.Text;
            Settings.Default.HwbotTeamName = this.txtHwbotTeam.Text;
            Settings.SaveSettings();

            DialogResult = DialogResult.OK;
        }

        private void txtCpuzLocation_TextChanged(object sender, EventArgs e)
        {
            updateLocationLabels();
        }

        private void txtGpuzLocation_TextChanged(object sender, EventArgs e)
        {
            updateLocationLabels();
        }
    }
}
