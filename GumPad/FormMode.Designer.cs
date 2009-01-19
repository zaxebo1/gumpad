namespace GumPad
{
    partial class FormMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMode));
            this.txtModeDesc = new System.Windows.Forms.RichTextBox();
            this.radioCnvAfterType = new System.Windows.Forms.RadioButton();
            this.radioCnvAsYouType = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkShowAtStartup = new System.Windows.Forms.CheckBox();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.comboBoxLang = new System.Windows.Forms.ComboBox();
            this.lblHelp1 = new System.Windows.Forms.Label();
            this.lblHelp2 = new System.Windows.Forms.Label();
            this.groupBoxMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtModeDesc
            // 
            this.txtModeDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModeDesc.Location = new System.Drawing.Point(1, 0);
            this.txtModeDesc.Name = "txtModeDesc";
            this.txtModeDesc.ReadOnly = true;
            this.txtModeDesc.Size = new System.Drawing.Size(576, 204);
            this.txtModeDesc.TabIndex = 0;
            this.txtModeDesc.Text = "Select Mode";
            // 
            // radioCnvAfterType
            // 
            this.radioCnvAfterType.AutoSize = true;
            this.radioCnvAfterType.Location = new System.Drawing.Point(9, 12);
            this.radioCnvAfterType.Name = "radioCnvAfterType";
            this.radioCnvAfterType.Size = new System.Drawing.Size(129, 17);
            this.radioCnvAfterType.TabIndex = 1;
            this.radioCnvAfterType.TabStop = true;
            this.radioCnvAfterType.Text = "Convert after you type";
            this.radioCnvAfterType.UseVisualStyleBackColor = true;
            this.radioCnvAfterType.CheckedChanged += new System.EventHandler(this.radioCnvAfterType_CheckedChanged);
            // 
            // radioCnvAsYouType
            // 
            this.radioCnvAsYouType.AutoSize = true;
            this.radioCnvAsYouType.Location = new System.Drawing.Point(9, 42);
            this.radioCnvAsYouType.Name = "radioCnvAsYouType";
            this.radioCnvAsYouType.Size = new System.Drawing.Size(131, 17);
            this.radioCnvAsYouType.TabIndex = 2;
            this.radioCnvAsYouType.TabStop = true;
            this.radioCnvAsYouType.Text = "Convert as you type to";
            this.radioCnvAsYouType.UseVisualStyleBackColor = true;
            this.radioCnvAsYouType.CheckedChanged += new System.EventHandler(this.radioCnvAsYouType_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(397, 329);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(494, 329);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkShowAtStartup
            // 
            this.chkShowAtStartup.AutoSize = true;
            this.chkShowAtStartup.Location = new System.Drawing.Point(18, 332);
            this.chkShowAtStartup.Name = "chkShowAtStartup";
            this.chkShowAtStartup.Size = new System.Drawing.Size(134, 17);
            this.chkShowAtStartup.TabIndex = 6;
            this.chkShowAtStartup.Text = "Always show at startup";
            this.chkShowAtStartup.UseVisualStyleBackColor = true;
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Controls.Add(this.radioCnvAsYouType);
            this.groupBoxMode.Controls.Add(this.radioCnvAfterType);
            this.groupBoxMode.Location = new System.Drawing.Point(9, 253);
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.Size = new System.Drawing.Size(143, 62);
            this.groupBoxMode.TabIndex = 7;
            this.groupBoxMode.TabStop = false;
            // 
            // comboBoxLang
            // 
            this.comboBoxLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLang.FormattingEnabled = true;
            this.comboBoxLang.Location = new System.Drawing.Point(154, 293);
            this.comboBoxLang.Name = "comboBoxLang";
            this.comboBoxLang.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLang.TabIndex = 8;
            // 
            // lblHelp1
            // 
            this.lblHelp1.AutoSize = true;
            this.lblHelp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelp1.Location = new System.Drawing.Point(18, 218);
            this.lblHelp1.Name = "lblHelp1";
            this.lblHelp1.Size = new System.Drawing.Size(41, 13);
            this.lblHelp1.TabIndex = 9;
            this.lblHelp1.Text = "label1";
            // 
            // lblHelp2
            // 
            this.lblHelp2.AutoSize = true;
            this.lblHelp2.Location = new System.Drawing.Point(18, 235);
            this.lblHelp2.Name = "lblHelp2";
            this.lblHelp2.Size = new System.Drawing.Size(35, 13);
            this.lblHelp2.TabIndex = 10;
            this.lblHelp2.Text = "label1";
            // 
            // FormMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 358);
            this.Controls.Add(this.lblHelp2);
            this.Controls.Add(this.lblHelp1);
            this.Controls.Add(this.comboBoxLang);
            this.Controls.Add(this.groupBoxMode);
            this.Controls.Add(this.chkShowAtStartup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtModeDesc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMode";
            this.Text = "GumPad";
            this.TopMost = true;
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtModeDesc;
        private System.Windows.Forms.RadioButton radioCnvAfterType;
        private System.Windows.Forms.RadioButton radioCnvAsYouType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkShowAtStartup;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.ComboBox comboBoxLang;
        private System.Windows.Forms.Label lblHelp1;
        private System.Windows.Forms.Label lblHelp2;
    }
}