namespace GumPad
{
    partial class FormCheckForUpdates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCheckForUpdates));
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.chkCheckForUpdatesAutomatically = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.saveDownloadedFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.BackColor = System.Drawing.SystemColors.Info;
            this.txtMessage.Location = new System.Drawing.Point(0, 0);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(341, 122);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "";
            // 
            // chkCheckForUpdatesAutomatically
            // 
            this.chkCheckForUpdatesAutomatically.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCheckForUpdatesAutomatically.AutoSize = true;
            this.chkCheckForUpdatesAutomatically.Location = new System.Drawing.Point(56, 126);
            this.chkCheckForUpdatesAutomatically.Name = "chkCheckForUpdatesAutomatically";
            this.chkCheckForUpdatesAutomatically.Size = new System.Drawing.Size(278, 17);
            this.chkCheckForUpdatesAutomatically.TabIndex = 1;
            this.chkCheckForUpdatesAutomatically.Text = " Check for updates automatically when gumpad starts";
            this.chkCheckForUpdatesAutomatically.UseVisualStyleBackColor = true;
            this.chkCheckForUpdatesAutomatically.CheckedChanged += new System.EventHandler(this.chkCheckForUpdatesAutomatically_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(160, 149);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 27);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(227, 149);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(103, 27);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download Installer";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // FormCheckForUpdates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 186);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkCheckForUpdatesAutomatically);
            this.Controls.Add(this.txtMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCheckForUpdates";
            this.Text = "Check for updates";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormCheckForUpdates_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.CheckBox chkCheckForUpdatesAutomatically;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.SaveFileDialog saveDownloadedFileDialog;
    }
}