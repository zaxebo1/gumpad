namespace GumPad
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.btnFontReset = new System.Windows.Forms.Button();
            this.btnFontCancel = new System.Windows.Forms.Button();
            this.btnFontApply = new System.Windows.Forms.Button();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.chkAdvanced = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(-1, 1);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(338, 201);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // btnFontReset
            // 
            this.btnFontReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontReset.Location = new System.Drawing.Point(269, 307);
            this.btnFontReset.Name = "btnFontReset";
            this.btnFontReset.Size = new System.Drawing.Size(55, 26);
            this.btnFontReset.TabIndex = 3;
            this.btnFontReset.Text = "Reset";
            this.btnFontReset.UseVisualStyleBackColor = true;
            this.btnFontReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // btnFontCancel
            // 
            this.btnFontCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFontCancel.Location = new System.Drawing.Point(169, 307);
            this.btnFontCancel.Name = "btnFontCancel";
            this.btnFontCancel.Size = new System.Drawing.Size(55, 26);
            this.btnFontCancel.TabIndex = 2;
            this.btnFontCancel.Text = "Cancel";
            this.btnFontCancel.UseVisualStyleBackColor = true;
            this.btnFontCancel.Click += new System.EventHandler(this.btnFontCancel_Click);
            // 
            // btnFontApply
            // 
            this.btnFontApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFontApply.Location = new System.Drawing.Point(97, 307);
            this.btnFontApply.Name = "btnFontApply";
            this.btnFontApply.Size = new System.Drawing.Size(55, 26);
            this.btnFontApply.TabIndex = 1;
            this.btnFontApply.Text = "OK";
            this.btnFontApply.UseVisualStyleBackColor = true;
            this.btnFontApply.Click += new System.EventHandler(this.btnFontApply_Click);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid2.HelpVisible = false;
            this.propertyGrid2.Location = new System.Drawing.Point(-1, 236);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(338, 65);
            this.propertyGrid2.TabIndex = 4;
            this.propertyGrid2.ToolbarVisible = false;
            // 
            // chkAdvanced
            // 
            this.chkAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAdvanced.AutoSize = true;
            this.chkAdvanced.Location = new System.Drawing.Point(192, 210);
            this.chkAdvanced.Name = "chkAdvanced";
            this.chkAdvanced.Size = new System.Drawing.Size(144, 17);
            this.chkAdvanced.TabIndex = 5;
            this.chkAdvanced.Text = "Advanced (Experimental)";
            this.chkAdvanced.UseVisualStyleBackColor = true;
            this.chkAdvanced.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 336);
            this.Controls.Add(this.chkAdvanced);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.btnFontApply);
            this.Controls.Add(this.btnFontCancel);
            this.Controls.Add(this.btnFontReset);
            this.Controls.Add(this.propertyGrid1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSettings";
            this.Text = "Fonts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button btnFontReset;
        private System.Windows.Forms.Button btnFontCancel;
        private System.Windows.Forms.Button btnFontApply;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.CheckBox chkAdvanced;
    }
}