namespace EhwValidationTool
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn3dLeft = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn2dLeft = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btn2dRight = new System.Windows.Forms.Button();
            this.btn3dRight = new System.Windows.Forms.Button();
            this.btnCloseTools = new System.Windows.Forms.Button();
            this.btnTakeScreenshot = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.chkSlowMode = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkShowSpdTabsSlot12 = new System.Windows.Forms.CheckBox();
            this.chkShowSpdTabsSlot24 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn3dLeft
            // 
            this.btn3dLeft.Location = new System.Drawing.Point(15, 165);
            this.btn3dLeft.Name = "btn3dLeft";
            this.btn3dLeft.Size = new System.Drawing.Size(180, 23);
            this.btn3dLeft.TabIndex = 0;
            this.btn3dLeft.Text = "3d Layout (Left side)";
            this.btn3dLeft.UseVisualStyleBackColor = true;
            this.btn3dLeft.Click += new System.EventHandler(this.btn3dLeft_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "This tool will arrange CPU-Z and GPU-Z for HWBOT submissions";
            // 
            // btn2dLeft
            // 
            this.btn2dLeft.Location = new System.Drawing.Point(15, 136);
            this.btn2dLeft.Name = "btn2dLeft";
            this.btn2dLeft.Size = new System.Drawing.Size(180, 23);
            this.btn2dLeft.TabIndex = 2;
            this.btn2dLeft.Text = "2d Layout (Left side)";
            this.btn2dLeft.UseVisualStyleBackColor = true;
            this.btn2dLeft.Click += new System.EventHandler(this.btn2dLeft_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(354, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(80, 23);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btn2dRight
            // 
            this.btn2dRight.Location = new System.Drawing.Point(246, 136);
            this.btn2dRight.Name = "btn2dRight";
            this.btn2dRight.Size = new System.Drawing.Size(180, 23);
            this.btn2dRight.TabIndex = 5;
            this.btn2dRight.Text = "2d Layout (Right side)";
            this.btn2dRight.UseVisualStyleBackColor = true;
            this.btn2dRight.Click += new System.EventHandler(this.btn2dRight_Click);
            // 
            // btn3dRight
            // 
            this.btn3dRight.Location = new System.Drawing.Point(246, 165);
            this.btn3dRight.Name = "btn3dRight";
            this.btn3dRight.Size = new System.Drawing.Size(180, 23);
            this.btn3dRight.TabIndex = 4;
            this.btn3dRight.Text = "3d Layout (Right side)";
            this.btn3dRight.UseVisualStyleBackColor = true;
            this.btn3dRight.Click += new System.EventHandler(this.btn3dRight_Click);
            // 
            // btnCloseTools
            // 
            this.btnCloseTools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseTools.Location = new System.Drawing.Point(246, 211);
            this.btnCloseTools.Name = "btnCloseTools";
            this.btnCloseTools.Size = new System.Drawing.Size(180, 23);
            this.btnCloseTools.TabIndex = 6;
            this.btnCloseTools.Text = "Close Tools";
            this.btnCloseTools.UseVisualStyleBackColor = true;
            this.btnCloseTools.Click += new System.EventHandler(this.btnCloseTools_Click);
            // 
            // btnTakeScreenshot
            // 
            this.btnTakeScreenshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTakeScreenshot.Location = new System.Drawing.Point(15, 211);
            this.btnTakeScreenshot.Name = "btnTakeScreenshot";
            this.btnTakeScreenshot.Size = new System.Drawing.Size(180, 23);
            this.btnTakeScreenshot.TabIndex = 7;
            this.btnTakeScreenshot.Text = "Take Screenshot";
            this.btnTakeScreenshot.UseVisualStyleBackColor = true;
            this.btnTakeScreenshot.Click += new System.EventHandler(this.btnTakeScreenshot_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(12, 17);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 8;
            this.lblVersion.Text = "Version:";
            // 
            // chkSlowMode
            // 
            this.chkSlowMode.AutoSize = true;
            this.chkSlowMode.Location = new System.Drawing.Point(15, 86);
            this.chkSlowMode.Name = "chkSlowMode";
            this.chkSlowMode.Size = new System.Drawing.Size(79, 17);
            this.chkSlowMode.TabIndex = 9;
            this.chkSlowMode.Text = "Slow Mode";
            this.toolTip1.SetToolTip(this.chkSlowMode, "Opens tools one at a time.  Useful for single core systems");
            this.chkSlowMode.UseVisualStyleBackColor = true;
            // 
            // chkShowSpdTabsSlot12
            // 
            this.chkShowSpdTabsSlot12.AutoSize = true;
            this.chkShowSpdTabsSlot12.Location = new System.Drawing.Point(246, 86);
            this.chkShowSpdTabsSlot12.Name = "chkShowSpdTabsSlot12";
            this.chkShowSpdTabsSlot12.Size = new System.Drawing.Size(166, 17);
            this.chkShowSpdTabsSlot12.TabIndex = 10;
            this.chkShowSpdTabsSlot12.Text = "Show CPU-Z SPD (Slots 1, 2)";
            this.toolTip1.SetToolTip(this.chkShowSpdTabsSlot12, "Opens tools one at a time.  Useful for single core systems");
            this.chkShowSpdTabsSlot12.UseVisualStyleBackColor = true;
            this.chkShowSpdTabsSlot12.CheckedChanged += new System.EventHandler(this.chkShowSpdTabs_CheckedChanged);
            // 
            // chkShowSpdTabsSlot24
            // 
            this.chkShowSpdTabsSlot24.AutoSize = true;
            this.chkShowSpdTabsSlot24.Location = new System.Drawing.Point(246, 108);
            this.chkShowSpdTabsSlot24.Name = "chkShowSpdTabsSlot24";
            this.chkShowSpdTabsSlot24.Size = new System.Drawing.Size(166, 17);
            this.chkShowSpdTabsSlot24.TabIndex = 11;
            this.chkShowSpdTabsSlot24.Text = "Show CPU-Z SPD (Slots 2, 4)";
            this.toolTip1.SetToolTip(this.chkShowSpdTabsSlot24, "Opens tools one at a time.  Useful for single core systems");
            this.chkShowSpdTabsSlot24.UseVisualStyleBackColor = true;
            this.chkShowSpdTabsSlot24.CheckedChanged += new System.EventHandler(this.chkShowSpdTabs_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 246);
            this.Controls.Add(this.chkShowSpdTabsSlot24);
            this.Controls.Add(this.chkShowSpdTabsSlot12);
            this.Controls.Add(this.chkSlowMode);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnTakeScreenshot);
            this.Controls.Add(this.btnCloseTools);
            this.Controls.Add(this.btn2dRight);
            this.Controls.Add(this.btn3dRight);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btn2dLeft);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn3dLeft);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "EHW Validation Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn3dLeft;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn2dLeft;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btn2dRight;
        private System.Windows.Forms.Button btn3dRight;
        private System.Windows.Forms.Button btnCloseTools;
        private System.Windows.Forms.Button btnTakeScreenshot;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.CheckBox chkSlowMode;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkShowSpdTabsSlot12;
        private System.Windows.Forms.CheckBox chkShowSpdTabsSlot24;
    }
}

