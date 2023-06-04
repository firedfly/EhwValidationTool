using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
