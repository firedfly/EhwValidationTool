using System;
using System.Windows.Forms;

namespace EhwValidationTool
{
    public partial class UserInfoForm : Form
    {
        public UserInfoForm()
        {
            InitializeComponent();
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            var userinfo = String.IsNullOrWhiteSpace(Settings.Default.HwbotTeamName) 
                ? $"{Settings.Default.HwbotUserName}" 
                : $"{Settings.Default.HwbotUserName} | {Settings.Default.HwbotTeamName}";
            lblUserInfo.Text = userinfo;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var WS_EX_TOOLWINDOW = 0x00000080;
                var Params = base.CreateParams;
                Params.ExStyle |= WS_EX_TOOLWINDOW;
                return Params;
            }
        }

        private void Form_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32Interop.ReleaseCapture();
                Win32Interop.SendMessage(Handle, Win32Interop.WM_NCLBUTTONDOWN, Win32Interop.HT_CAPTION, 0);
            }
        }
    }
}
