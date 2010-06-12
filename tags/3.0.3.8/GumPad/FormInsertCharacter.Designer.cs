namespace GumPad
{
    partial class FormInsertCharacter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsertCharacter));
            this.lblCharCode = new System.Windows.Forms.Label();
            this.txtUCodeChar = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCharCode
            // 
            this.lblCharCode.AutoSize = true;
            this.lblCharCode.Location = new System.Drawing.Point(7, 15);
            this.lblCharCode.Name = "lblCharCode";
            this.lblCharCode.Size = new System.Drawing.Size(124, 13);
            this.lblCharCode.TabIndex = 0;
            this.lblCharCode.Text = "Unicode Character Code";
            // 
            // txtUCodeChar
            // 
            this.txtUCodeChar.Location = new System.Drawing.Point(144, 11);
            this.txtUCodeChar.Name = "txtUCodeChar";
            this.txtUCodeChar.Size = new System.Drawing.Size(72, 20);
            this.txtUCodeChar.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(223, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormInsertCharacter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 38);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtUCodeChar);
            this.Controls.Add(this.lblCharCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInsertCharacter";
            this.Text = "Insert Character (Experimental)";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormInsertCharacter_KeyPress);
            this.Load += new System.EventHandler(this.FormInsertCharacter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCharCode;
        private System.Windows.Forms.TextBox txtUCodeChar;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}