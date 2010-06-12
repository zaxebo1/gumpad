/*
 * Copyright © 2007-2009, Pradyumna Kumar Revur.
 * All rights reserved.
 * 
 * 
 * GumPad is freeware. You may use it at your own risk for any purpose you like, subject to the following terms.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * * Neither the name of the the authors or copyright holders nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GumPad
{
    public partial class FormFindReplace : Form
    {
        private String findText;
        private String replaceText;
        private bool isReplace=false;
        private RichTextBox txtRTF;
        private int lastLoc = -1;


        public int LastFindLocation
        {
            get
            {
                return lastLoc;
            }
            set
            {
                lastLoc = value;
            }
        }

        public RichTextBox TextBox
        {
            set
            {
                txtRTF = value;
            }
        }

        public bool ReplaceDialog
        {
            set
            {
                isReplace = value;
                txtReplace.Enabled = isReplace;
                btnReplace.Enabled = isReplace;
                btnReplaceAll.Enabled = isReplace;
            }
        }

        public String FindText
        {
            get
            {
                return findText;
            }

            set
            {
                findText = value;
            }
        }

        public String ReplaceText
        {
            get
            {
                return replaceText;
            }

            set
            {
                replaceText = value;
            }
        }
        
        public FormFindReplace()
        {
            InitializeComponent();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            findText = txtFind.Text;
            lastLoc = txtRTF.Find(txtFind.Text, lastLoc+1, RichTextBoxFinds.None);
            if (lastLoc == -1)
            {
                MessageBox.Show("'" + findText + "' not found");
            }
            Close();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            findText = txtFind.Text;
            replaceText = txtReplace.Text;
            if (-1 != (lastLoc = txtRTF.Find(findText, lastLoc + 1 + replaceText.Length, RichTextBoxFinds.None)))
            {
                    txtRTF.SelectedText = txtReplace.Text;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            findText = txtFind.Text;
            replaceText = txtReplace.Text;
            while (-1 != (lastLoc = txtRTF.Find(findText, lastLoc + 1 + replaceText.Length, RichTextBoxFinds.None)))
            {
                txtRTF.SelectedText = txtReplace.Text;
            }
            Close();

        }

        private void FormFindReplace_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Escape)
            {
                e.Handled = true;
                Close();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnFindNext_Click(sender, e);
            }
        }

        private void FormFindReplace_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
        }
    }
}