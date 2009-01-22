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
using System.Drawing.Printing;
using System.IO;
using GumLib;

namespace GumPad
{
    public partial class GumPad : Form
    {
        private String fileName = "";
        private RichTextBoxStreamType fileType = RichTextBoxStreamType.RichText;
        private readonly string license;
        private const String FILETYPES = "GumPad Files|*.gpd|Plain Text Files|*.txt|Unicode Text Files|*.utx|ITRANS Files|*.itx|Rich Text Files|*.rtf|All Files|*.*";
        int lastCharPrinted;
        private static System.OperatingSystem osInfo = System.Environment.OSVersion;

        public GumPad()
        {
            InitializeComponent();
            txtRTF.WordWrap = wordWrapToolStripMenuItem.Checked = Settings.Default.WordWrap;
            license = Resources.LicenseFile;
            txtRTF.TypedTextStatusLabel = statusLblTypedText;
            statusLabelSpacer.Text = "";

            if (Settings.Default.ShowModeAtStartup)
            {
                FormMode f = new FormMode();
                f.ShowDialog();
                f.Dispose();
            }

            txtRTF.TransliterateAsEntityCode = Settings.Default.TransliterateAsEntityCode;
            txtRTF.TransliterationLanguage = Settings.Default.Language;
            setupConvertAsyouType(Settings.Default.ConvertAsYouType);
            convertAsYouTypeToolStripMenuItem.Checked = Settings.Default.ConvertAsYouType;

            statusLabelTransliterator.Text = Settings.Default.ConversionSchemeName;
            statusLabelTransliterator.ToolTipText = Settings.Default.ConversionSchemeDescription;

            convertFromExtendedLatinToolStripMenuItem.ToolTipText =
                 "Leave this unchecked if you are converting plain English "
                        + "text to an Indian Language.\n"
                        + "Leave it checked if you are converting extended English "
                        + "text to an Indian Language.";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox(license);
            a.ShowDialog();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = txtRTF.SelectionFont;
            if (fontDialog1.Font == null)
            {
                fontDialog1.Font = DefaultFont;
            }
            fontDialog1.ShowColor = true;
            fontDialog1.ShowEffects = true;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtRTF.Cursor = Cursors.WaitCursor;
                    txtRTF.SuspendUpdates();
                    txtRTF.DisplayFont = fontDialog1.Font;
                    txtRTF.SelectionColor = fontDialog1.Color;
                }
                finally
                {
                    txtRTF.ResumeUpdates();
                    txtRTF.Cursor = Cursors.Default;
                }
            }
        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.DocumentName = fileName;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Cancel == checkIfModified())
            {
                return;
            }
            openFileDialog1.InitialDirectory = System.Environment.SpecialFolder.MyDocuments.ToString();
            openFileDialog1.Filter = FILETYPES;
            openFileDialog1.FilterIndex = Settings.Default.DefaultFileFilterIndex;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowReadOnly = true;
            openFileDialog1.SupportMultiDottedExtensions = true;
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRTF.SelectionFont = DefaultFont;
                Settings.Default.DefaultFileFilterIndex = openFileDialog1.FilterIndex;
                fileName = openFileDialog1.FileName;
                if (fileName.EndsWith(".gpd", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(fileName, RichTextBoxStreamType.RichText);
                }
                else if (fileName.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(fileName, RichTextBoxStreamType.RichText);
                }
                else if (fileName.EndsWith(".utx", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(fileName, RichTextBoxStreamType.UnicodePlainText);
                }
                else if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(fileName, RichTextBoxStreamType.PlainText);
                }
                else if (fileName.EndsWith(".itx", StringComparison.OrdinalIgnoreCase))
                {
                    Cursor = Cursors.WaitCursor;
                    txtRTF.LoadFile(fileName, RichTextBoxStreamType.PlainText);
                    Settings.Default.ConvertAsYouType = convertAsYouTypeToolStripMenuItem.Checked = false;
                    setupConvertAsyouType(Settings.Default.ConvertAsYouType);
                    Cursor = Cursors.Default;
                    Refresh();
                }
                else if (fileName.EndsWith(".map", StringComparison.OrdinalIgnoreCase))
                {
                    Cursor = Cursors.WaitCursor;
                    StreamReader instream = new StreamReader(fileName, Encoding.UTF8);
                    txtRTF.SuspendUpdates();
                    txtRTF.Text = instream.ReadToEnd();
                    txtRTF.SelectAll();
                    txtRTF.SelectionFont = DefaultFont;
                    txtRTF.DeselectAll();
                    txtRTF.ResumeUpdates();
                    instream.Close();
                    Cursor = Cursors.Default;
                }
                else
                {
                    FormEncoding frmEncoding = new FormEncoding();
                    frmEncoding.ShowDialog();
                    Cursor = Cursors.WaitCursor;
                    StreamReader instream = new StreamReader(fileName, frmEncoding.SelectedEncoding);
                    txtRTF.SelectionFont = DefaultFont;
                    txtRTF.Text = instream.ReadToEnd();
                    instream.Close();
                    Cursor = Cursors.Default;
                }
                txtRTF.Modified = true;
            }
        }

        private DialogResult checkIfModified()
        {
            DialogResult res = DialogResult.None;
            if (txtRTF.Modified == true)
            {
                string temp=fileName;
                if (temp.Equals(""))
                {
                    temp = "Untitled";
                }
                res = MessageBox.Show("Do you want to save changes to "
                    + temp + "?", "GumPad", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(this, null);
                }
            }
            return res;
        }

        private void showFontMessage()
        {
            if (osInfo.Version.Major < 6)
            {
                // if version < vista
                MessageBox.Show("If you are seeing empty boxes, "
                    + "set an appropriate font for the language of the text.");
            }
        }

        private void transliterateSelection()
        {
            Cursor = Cursors.WaitCursor;
            txtRTF.Transliterate();
            Cursor = Cursors.Default;
        }

        private void teluguToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = teluguToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.TELUGU;
            txtRTF.DisplayFont = Fonts.Default.TELUGU;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
                showFontMessage();
            }
        }

        private void devanagariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = devanagariToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.DEVANAGARI;
            txtRTF.DisplayFont = Fonts.Default.DEVANAGARI;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }
        }

        private void tamilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = tamilToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.TAMIL;
            txtRTF.DisplayFont = Fonts.Default.TAMIL;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }
        }

        private void kannadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = kannadaToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.KANNADA;
            txtRTF.DisplayFont = Fonts.Default.KANNADA;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
                showFontMessage();
            }
        }

        private void malayalamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = malayalamToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.MALAYALAM;
            txtRTF.DisplayFont = Fonts.Default.MALAYALAM;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }
        }

        private void oriyaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = oriyaToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.ORIYA;
            txtRTF.DisplayFont = Fonts.Default.ORIYA;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
                showFontMessage();
            }
        }

        private void marathiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = marathiToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.MARATHI;
            txtRTF.DisplayFont = Fonts.Default.MARATHI;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }
        }

        private void gujaratiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = gujaratiToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.GUJARATI;
            txtRTF.DisplayFont = Fonts.Default.GUJARATI;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
                showFontMessage();
            }
        }

        private void bengaliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = bengaliToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.BENGALI;
            txtRTF.DisplayFont = Fonts.Default.BENGALI;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }
        }

        private void gurmukhiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = gurmukhiToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.GURMUKHI;
            txtRTF.DisplayFont = Fonts.Default.GURMUKHI;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
                showFontMessage();
            }
        }

        private void latinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = oriyaToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.LATIN;
            txtRTF.DisplayFont = Fonts.Default.LATIN;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }

        }

        private void latinExToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLangDropDown.Text = oriyaToolStripMenuItem.Text;
            txtRTF.TransliterationLanguage = GumLib.Transliterator.LATINEX;
            txtRTF.DisplayFont = Fonts.Default.LATINEX;
            if (!Settings.Default.ConvertAsYouType)
            {
                transliterateSelection();
            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Cancel == checkIfModified())
            {
                return;
            }
            txtRTF.Clear();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtRTF.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtRTF.Redo();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = txtRTF.CanUndo;
            redoToolStripMenuItem.Enabled = txtRTF.CanRedo;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtRTF.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtRTF.SelectionLength > 0)
            {
                txtRTF.Copy();
            }
        }

        private void cutToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (txtRTF.SelectionLength > 0)
            {
                txtRTF.Cut();
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;
            txtRTF.WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsData(DataFormats.Rtf)
                || Clipboard.ContainsText())
            {
                txtRTF.Paste();
            }
        }

        private void fontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings fs = new FormSettings();
            fs.ShowDialog(this);
        }

        private void conversionMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMap fm = new FormMap(statusLabelTransliterator, txtRTF.Transliterator);
            DialogResult res = fm.ShowDialog(this);
            Settings.Default.ConversionSchemeName = statusLabelTransliterator.Text;
            Settings.Default.ConversionSchemeDescription = statusLabelTransliterator.ToolTipText;
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            lastCharPrinted = 0;
        }

        private void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            txtRTF.PrintDone();
        }
        
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            lastCharPrinted = txtRTF.Print(lastCharPrinted, txtRTF.TextLength, e);
            if (lastCharPrinted < txtRTF.TextLength)
				e.HasMorePages = true;
			else
				e.HasMorePages = false;
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.UseAntiAlias = true;
            printPreviewDialog1.ShowDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = FILETYPES;
            saveFileDialog1.FileName = fileName;
            saveFileDialog1.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    fileType = RichTextBoxStreamType.PlainText;
                }
                else if (fileName.EndsWith(".utx", StringComparison.OrdinalIgnoreCase))
                {
                    fileType = RichTextBoxStreamType.UnicodePlainText;
                }
                else if (fileName.EndsWith(".itx", StringComparison.OrdinalIgnoreCase))
                {
                    fileType = RichTextBoxStreamType.PlainText;
                }
                else
                {
                    fileType=RichTextBoxStreamType.RichText;
                }
                txtRTF.SaveFile(fileName, fileType);
                txtRTF.Modified = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName.Equals("")) {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }
            txtRTF.SaveFile(fileName, fileType);
            txtRTF.Modified = false;
        }

        private String findText = "";
        private String replaceText="";
        private int lastLoc = -1;
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFindReplace ffr = new FormFindReplace();
            ffr.TextBox = txtRTF;
            ffr.LastFindLocation = lastLoc;
            ffr.FindText = findText;
            ffr.ReplaceText = replaceText;
            ffr.ReplaceDialog = false;
            DialogResult res = ffr.ShowDialog();
            lastLoc = ffr.LastFindLocation;
            findText = ffr.FindText;
            replaceText = ffr.ReplaceText;
            ffr.Dispose();
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (findText.Trim().Equals(""))
            {
                findToolStripMenuItem_Click(sender, e);
            }
            else
            {
                lastLoc = txtRTF.Find(findText, lastLoc + 1, RichTextBoxFinds.None);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFindReplace ffr = new FormFindReplace();
            ffr.TextBox = txtRTF;
            ffr.LastFindLocation = lastLoc;
            ffr.FindText = findText;
            ffr.ReplaceText = replaceText;
            ffr.ReplaceDialog = true;
            DialogResult res = ffr.ShowDialog();
            lastLoc = ffr.LastFindLocation;
            findText = ffr.FindText;
            replaceText = ffr.ReplaceText;
            ffr.Dispose();
        }

        private void insertCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInsertCharacter fic = new FormInsertCharacter();
            fic.TextBox = txtRTF;
            fic.Show();
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.Document = printDocument1;
            pageSetupDialog1.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Cancel == checkIfModified())
            {
                return;
            }
            Dispose();
            Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void GumPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Cancel == checkIfModified())
            {
                e.Cancel = true;
                return;
            }
            Dispose();
            Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void showMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWebBrowser brwsr = new FormWebBrowser();
            brwsr.HTML = txtRTF.Transliterator.printLetterMap();
            brwsr.Show();
        }


        private void setupConvertAsyouType(bool convertAsYouType)
        {
            txtRTF.ConvertAsYouType = convertAsYouType;

            if (convertAsYouType)
            {
                convertToolStripMenuItem.Visible = false;
                statusLabelSpacer.Text = "Mode: Convert as you type to ";
                statusLblTypedText.Text = "";
                statusLangDropDown.Visible = true;

                if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.BENGALI))
                {
                    txtRTF.DisplayFont = Fonts.Default.BENGALI;
                    statusLangDropDown.Text = bengaliToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.DEVANAGARI))
                {
                    txtRTF.DisplayFont = Fonts.Default.DEVANAGARI;
                    statusLangDropDown.Text = devanagariToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.GUJARATI))
                {
                    txtRTF.DisplayFont = Fonts.Default.GUJARATI;
                    statusLangDropDown.Text = gujaratiToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.GURMUKHI))
                {
                    txtRTF.DisplayFont = Fonts.Default.GURMUKHI;
                    statusLangDropDown.Text = gurmukhiToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.KANNADA))
                {
                    txtRTF.DisplayFont = Fonts.Default.KANNADA;
                    statusLangDropDown.Text = kannadaToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.MALAYALAM))
                {
                    txtRTF.DisplayFont = Fonts.Default.MALAYALAM;
                    statusLangDropDown.Text = malayalamToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.MARATHI))
                {
                    txtRTF.DisplayFont = Fonts.Default.MARATHI;
                    statusLangDropDown.Text = marathiToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.ORIYA))
                {
                    txtRTF.DisplayFont = Fonts.Default.ORIYA;
                    statusLangDropDown.Text = oriyaToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.TAMIL))
                {
                    txtRTF.DisplayFont = Fonts.Default.TAMIL;
                    statusLangDropDown.Text = tamilToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.LATIN))
                {
                    txtRTF.DisplayFont = Fonts.Default.LATIN;
                    statusLangDropDown.Text = latinToolStripMenuItem.Text;
                }
                else if (txtRTF.TransliterationLanguage.Equals(GumLib.Transliterator.LATINEX))
                {
                    txtRTF.DisplayFont = Fonts.Default.LATINEX;
                    statusLangDropDown.Text = latinExToolStripMenuItem.Text;
                }
                else
                {
                    txtRTF.DisplayFont = Fonts.Default.TELUGU;
                    statusLangDropDown.Text = teluguToolStripMenuItem.Text;
                }

                //statusLangDropDown.DropDown.Items.AddRange(convertToolStripMenuItem.DropDownItems);
                statusLangDropDown.DropDown.Items.Add(bengaliToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(devanagariToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(gujaratiToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(gurmukhiToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(kannadaToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(malayalamToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(marathiToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(oriyaToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(tamilToolStripMenuItem);
                statusLangDropDown.DropDown.Items.Add(teluguToolStripMenuItem);
                foreach (ToolStripMenuItem item in statusLangDropDown.DropDownItems)
                {
                    item.Enabled = true;
                }
            }
            else
            {
                convertToolStripMenuItem.Visible = true;
                statusLabelSpacer.Text = "Mode: Convert after you type";
                statusLblTypedText.Text = "Ready";
                statusLangDropDown.Visible=false;
                //convertToolStripMenuItem.DropDown.Items.AddRange(statusLangDropDown.DropDownItems);
                convertToolStripMenuItem.DropDown.Items.Add(bengaliToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(devanagariToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(gujaratiToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(gurmukhiToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(kannadaToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(malayalamToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(marathiToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(oriyaToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(tamilToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(teluguToolStripMenuItem);

                convertToolStripMenuItem.DropDown.Items.Add(latinToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(latinExToolStripMenuItem);
                convertToolStripMenuItem.DropDown.Items.Add(convertFromExtendedLatinToolStripMenuItem);
            }
        }

        private void convertToolStripMenuItem_ClickNOTUSED(object sender, EventArgs e)
        {
            if (txtRTF.SelectionLength > 0)
            {
                foreach (ToolStripMenuItem item in convertToolStripMenuItem.DropDownItems) {
                    item.Enabled = true;
                }
            }
            else
            {
                //statusLblTypedText.Text = "No selection to convert. Use Convert menu after selecting text you want to convert.";
                foreach (ToolStripMenuItem item in convertToolStripMenuItem.DropDownItems)
                {
                    item.Enabled = false;
                }
            }
        }

        private void quickStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMode f = new FormMode();
            f.ShowDialog();
            txtRTF.TransliterationLanguage = Settings.Default.Language;
            setupConvertAsyouType(Settings.Default.ConvertAsYouType);
            f.Dispose();
        }

        private void convertAsYouTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            convertAsYouTypeToolStripMenuItem.Checked = !convertAsYouTypeToolStripMenuItem.Checked;
            Settings.Default.ConvertAsYouType = convertAsYouTypeToolStripMenuItem.Checked;
            setupConvertAsyouType(Settings.Default.ConvertAsYouType);
        }

        private void convertFromExtendedLatinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (convertFromExtendedLatinToolStripMenuItem.Checked)
            {
                txtRTF.Transliterator.UseLatinExMapForConversion = true;
            }
            else
            {
                txtRTF.Transliterator.UseLatinExMapForConversion = false;
            }
        }
    }
}