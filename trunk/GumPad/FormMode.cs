/*
 * Copyright © 2007-2009 Pradyumna Kumar Revur. All rights reserved.
 * 
 * GumPad is freeware. You may use it at your own risk for any purpose you like. 
 * You may redistribute GumPad in source or binary form with or without modification,
 * provided that redistributions reproduce the above copyright notice, this statement of conditions,
 * the following disclaimer and an acknowledgement in the documentation and/or other materials
 * provided with the distribution. Neither the name GumPad nor the name of Pradyumna Kumar Revur
 * or any contributors or content providers may be used to endorse or promote products derived
 * from this software without specific prior written permission from the respective parties and copyright holders.
 * 
 * THIS SOFTWARE IS PROVIDED BY PRADYUMNA KUMAR REVUR "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL PRADYUMNA KUMAR REVUR AND/OR ANY CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
    public partial class FormMode : Form
    {
        public FormMode()
        {
            InitializeComponent();

            txtModeDesc.BackColor = Color.White;
            txtModeDesc.Rtf = Resources.QuickStart;

            lblHelp1.Text = "Select the mode you would like to start with";
            lblHelp2.Text = "You can change this at any time using the  \"Convert As You Type\" item under the Preferences menu.";

            radioCnvAsYouType.Checked = Settings.Default.ConvertAsYouType;
            radioCnvAfterType.Checked = !Settings.Default.ConvertAsYouType;

            comboBoxLang.Items.Add(GumLib.Transliterator.BENGALI);
            comboBoxLang.Items.Add(GumLib.Transliterator.DEVANAGARI);
            comboBoxLang.Items.Add(GumLib.Transliterator.GUJARATI);
            comboBoxLang.Items.Add(GumLib.Transliterator.GURMUKHI);
            comboBoxLang.Items.Add(GumLib.Transliterator.KANNADA);
            comboBoxLang.Items.Add(GumLib.Transliterator.MALAYALAM);
            comboBoxLang.Items.Add(GumLib.Transliterator.MARATHI);
            comboBoxLang.Items.Add(GumLib.Transliterator.ORIYA);
            comboBoxLang.Items.Add(GumLib.Transliterator.TAMIL);
            comboBoxLang.Items.Add(GumLib.Transliterator.TELUGU);

            comboBoxLang.SelectedItem = Settings.Default.Language;
            chkShowAtStartup.Checked = Settings.Default.ShowModeAtStartup;
        }

        private void radioCnvAfterType_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxLang.Enabled = radioCnvAsYouType.Checked;
        }

        private void radioCnvAsYouType_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxLang.Enabled = radioCnvAsYouType.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings.Default.Language = comboBoxLang.SelectedItem.ToString();
            Settings.Default.ConvertAsYouType = radioCnvAsYouType.Checked;
            Settings.Default.ShowModeAtStartup=chkShowAtStartup.Checked;
            Settings.Default.Save();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}