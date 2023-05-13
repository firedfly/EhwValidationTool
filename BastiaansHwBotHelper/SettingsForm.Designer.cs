namespace BastiaansHwBotHelper
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCpuz = new System.Windows.Forms.Label();
            this.lblGpuz = new System.Windows.Forms.Label();
            this.txtCpuzLocation = new System.Windows.Forms.TextBox();
            this.txtGpuzLocation = new System.Windows.Forms.TextBox();
            this.btnCpuzBrowse = new System.Windows.Forms.Button();
            this.btnGpuzBrowse = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHwbotUser = new System.Windows.Forms.Label();
            this.txtHwbotUser = new System.Windows.Forms.TextBox();
            this.txtHwbotTeam = new System.Windows.Forms.TextBox();
            this.lblHwbotTeam = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCpuz
            // 
            this.lblCpuz.AutoSize = true;
            this.lblCpuz.Location = new System.Drawing.Point(21, 37);
            this.lblCpuz.Name = "lblCpuz";
            this.lblCpuz.Size = new System.Drawing.Size(83, 13);
            this.lblCpuz.TabIndex = 0;
            this.lblCpuz.Text = "CPU-Z Location";
            // 
            // lblGpuz
            // 
            this.lblGpuz.AutoSize = true;
            this.lblGpuz.Location = new System.Drawing.Point(21, 71);
            this.lblGpuz.Name = "lblGpuz";
            this.lblGpuz.Size = new System.Drawing.Size(84, 13);
            this.lblGpuz.TabIndex = 1;
            this.lblGpuz.Text = "GPU-Z Location";
            // 
            // txtCpuzLocation
            // 
            this.txtCpuzLocation.Location = new System.Drawing.Point(111, 34);
            this.txtCpuzLocation.Name = "txtCpuzLocation";
            this.txtCpuzLocation.Size = new System.Drawing.Size(342, 20);
            this.txtCpuzLocation.TabIndex = 1;
            this.txtCpuzLocation.TextChanged += new System.EventHandler(this.txtCpuzLocation_TextChanged);
            // 
            // txtGpuzLocation
            // 
            this.txtGpuzLocation.Location = new System.Drawing.Point(111, 68);
            this.txtGpuzLocation.Name = "txtGpuzLocation";
            this.txtGpuzLocation.Size = new System.Drawing.Size(342, 20);
            this.txtGpuzLocation.TabIndex = 3;
            this.txtGpuzLocation.TextChanged += new System.EventHandler(this.txtGpuzLocation_TextChanged);
            // 
            // btnCpuzBrowse
            // 
            this.btnCpuzBrowse.Location = new System.Drawing.Point(459, 32);
            this.btnCpuzBrowse.Name = "btnCpuzBrowse";
            this.btnCpuzBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCpuzBrowse.TabIndex = 2;
            this.btnCpuzBrowse.Text = "Browse";
            this.btnCpuzBrowse.UseVisualStyleBackColor = true;
            this.btnCpuzBrowse.Click += new System.EventHandler(this.btnCpuzBrowse_Click);
            // 
            // btnGpuzBrowse
            // 
            this.btnGpuzBrowse.Location = new System.Drawing.Point(459, 66);
            this.btnGpuzBrowse.Name = "btnGpuzBrowse";
            this.btnGpuzBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnGpuzBrowse.TabIndex = 4;
            this.btnGpuzBrowse.Text = "Browse";
            this.btnGpuzBrowse.UseVisualStyleBackColor = true;
            this.btnGpuzBrowse.Click += new System.EventHandler(this.btnGpuzBrowse_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(484, 195);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(403, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblHwbotUser
            // 
            this.lblHwbotUser.AutoSize = true;
            this.lblHwbotUser.Location = new System.Drawing.Point(21, 105);
            this.lblHwbotUser.Name = "lblHwbotUser";
            this.lblHwbotUser.Size = new System.Drawing.Size(73, 13);
            this.lblHwbotUser.TabIndex = 8;
            this.lblHwbotUser.Text = "HWBOT User";
            // 
            // txtHwbotUser
            // 
            this.txtHwbotUser.Location = new System.Drawing.Point(111, 102);
            this.txtHwbotUser.Name = "txtHwbotUser";
            this.txtHwbotUser.Size = new System.Drawing.Size(342, 20);
            this.txtHwbotUser.TabIndex = 5;
            // 
            // txtHwbotTeam
            // 
            this.txtHwbotTeam.Location = new System.Drawing.Point(111, 136);
            this.txtHwbotTeam.Name = "txtHwbotTeam";
            this.txtHwbotTeam.Size = new System.Drawing.Size(342, 20);
            this.txtHwbotTeam.TabIndex = 6;
            // 
            // lblHwbotTeam
            // 
            this.lblHwbotTeam.AutoSize = true;
            this.lblHwbotTeam.Location = new System.Drawing.Point(21, 139);
            this.lblHwbotTeam.Name = "lblHwbotTeam";
            this.lblHwbotTeam.Size = new System.Drawing.Size(78, 13);
            this.lblHwbotTeam.TabIndex = 10;
            this.lblHwbotTeam.Text = "HWBOT Team";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(571, 230);
            this.Controls.Add(this.txtHwbotTeam);
            this.Controls.Add(this.lblHwbotTeam);
            this.Controls.Add(this.txtHwbotUser);
            this.Controls.Add(this.lblHwbotUser);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnGpuzBrowse);
            this.Controls.Add(this.btnCpuzBrowse);
            this.Controls.Add(this.txtGpuzLocation);
            this.Controls.Add(this.txtCpuzLocation);
            this.Controls.Add(this.lblGpuz);
            this.Controls.Add(this.lblCpuz);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCpuz;
        private System.Windows.Forms.Label lblGpuz;
        private System.Windows.Forms.TextBox txtCpuzLocation;
        private System.Windows.Forms.TextBox txtGpuzLocation;
        private System.Windows.Forms.Button btnCpuzBrowse;
        private System.Windows.Forms.Button btnGpuzBrowse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHwbotUser;
        private System.Windows.Forms.TextBox txtHwbotUser;
        private System.Windows.Forms.TextBox txtHwbotTeam;
        private System.Windows.Forms.Label lblHwbotTeam;
    }
}