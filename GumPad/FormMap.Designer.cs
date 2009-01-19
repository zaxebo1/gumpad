namespace GumPad
{
    partial class FormMap
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMap));
            this.btnFontApply = new System.Windows.Forms.Button();
            this.btnFontCancel = new System.Windows.Forms.Button();
            this.btnFontReset = new System.Windows.Forms.Button();
            this.btnLoadCustomMap = new System.Windows.Forms.Button();
            this.btnSaveCustomMap = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkAdvancedMapEdit = new System.Windows.Forms.CheckBox();
            this.chkSkipValidation = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFontApply
            // 
            this.btnFontApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontApply.Location = new System.Drawing.Point(464, 235);
            this.btnFontApply.Name = "btnFontApply";
            this.btnFontApply.Size = new System.Drawing.Size(55, 26);
            this.btnFontApply.TabIndex = 4;
            this.btnFontApply.Text = "OK";
            this.btnFontApply.UseVisualStyleBackColor = true;
            this.btnFontApply.Click += new System.EventHandler(this.btnMapApply_Click);
            // 
            // btnFontCancel
            // 
            this.btnFontCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontCancel.Location = new System.Drawing.Point(534, 235);
            this.btnFontCancel.Name = "btnFontCancel";
            this.btnFontCancel.Size = new System.Drawing.Size(55, 26);
            this.btnFontCancel.TabIndex = 5;
            this.btnFontCancel.Text = "Cancel";
            this.btnFontCancel.UseVisualStyleBackColor = true;
            this.btnFontCancel.Click += new System.EventHandler(this.btnMapCancel_Click);
            // 
            // btnFontReset
            // 
            this.btnFontReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontReset.Location = new System.Drawing.Point(600, 235);
            this.btnFontReset.Name = "btnFontReset";
            this.btnFontReset.Size = new System.Drawing.Size(55, 26);
            this.btnFontReset.TabIndex = 6;
            this.btnFontReset.Text = "Reset";
            this.btnFontReset.UseVisualStyleBackColor = true;
            this.btnFontReset.Click += new System.EventHandler(this.btnMapReset_Click);
            // 
            // btnLoadCustomMap
            // 
            this.btnLoadCustomMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadCustomMap.Location = new System.Drawing.Point(5, 235);
            this.btnLoadCustomMap.Name = "btnLoadCustomMap";
            this.btnLoadCustomMap.Size = new System.Drawing.Size(127, 23);
            this.btnLoadCustomMap.TabIndex = 7;
            this.btnLoadCustomMap.Text = "Load Custom Map...";
            this.btnLoadCustomMap.UseVisualStyleBackColor = true;
            this.btnLoadCustomMap.Click += new System.EventHandler(this.btnLoadCustomMap_Click);
            // 
            // btnSaveCustomMap
            // 
            this.btnSaveCustomMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveCustomMap.Location = new System.Drawing.Point(140, 235);
            this.btnSaveCustomMap.Name = "btnSaveCustomMap";
            this.btnSaveCustomMap.Size = new System.Drawing.Size(127, 23);
            this.btnSaveCustomMap.TabIndex = 8;
            this.btnSaveCustomMap.Text = "Save As Custom Map...";
            this.btnSaveCustomMap.UseVisualStyleBackColor = true;
            this.btnSaveCustomMap.Click += new System.EventHandler(this.btnSaveCustomMap_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Title = "Open Custom Map";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Title = "Save Custom Map";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(2, -2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(665, 208);
            this.dataGridView1.TabIndex = 9;
            // 
            // chkAdvancedMapEdit
            // 
            this.chkAdvancedMapEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAdvancedMapEdit.AutoSize = true;
            this.chkAdvancedMapEdit.Location = new System.Drawing.Point(532, 212);
            this.chkAdvancedMapEdit.Name = "chkAdvancedMapEdit";
            this.chkAdvancedMapEdit.Size = new System.Drawing.Size(126, 17);
            this.chkAdvancedMapEdit.TabIndex = 10;
            this.chkAdvancedMapEdit.Text = "Advanced Edit Mode";
            this.chkAdvancedMapEdit.UseVisualStyleBackColor = true;
            this.chkAdvancedMapEdit.CheckedChanged += new System.EventHandler(this.chkAdvancedMapEdit_CheckedChanged);
            // 
            // chkSkipValidation
            // 
            this.chkSkipValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSkipValidation.AutoSize = true;
            this.chkSkipValidation.Location = new System.Drawing.Point(13, 212);
            this.chkSkipValidation.Name = "chkSkipValidation";
            this.chkSkipValidation.Size = new System.Drawing.Size(96, 17);
            this.chkSkipValidation.TabIndex = 11;
            this.chkSkipValidation.Text = "Skip Validation";
            this.chkSkipValidation.UseVisualStyleBackColor = true;
            // 
            // FormMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 264);
            this.Controls.Add(this.chkSkipValidation);
            this.Controls.Add(this.chkAdvancedMapEdit);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSaveCustomMap);
            this.Controls.Add(this.btnLoadCustomMap);
            this.Controls.Add(this.btnFontApply);
            this.Controls.Add(this.btnFontCancel);
            this.Controls.Add(this.btnFontReset);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMap";
            this.Text = "Conversion Map";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFontApply;
        private System.Windows.Forms.Button btnFontCancel;
        private System.Windows.Forms.Button btnFontReset;
        private System.Windows.Forms.Button btnLoadCustomMap;
        private System.Windows.Forms.Button btnSaveCustomMap;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkAdvancedMapEdit;
        private System.Windows.Forms.CheckBox chkSkipValidation;
    }
}