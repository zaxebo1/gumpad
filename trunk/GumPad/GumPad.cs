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
using System.Drawing.Printing;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Xml.Serialization;
using System.Threading;
using System.Media;
using GumLib;

namespace GumPad
{
    public partial class GumPad : Form
    {
        private String m_fileName = "";
        private RichTextBoxStreamType m_fileType = RichTextBoxStreamType.RichText;
        private const String FILETYPES = "(Text Files;ITRANS Files;GumPad Files)|*.txt;*.itx;*.gpd|Unicode Files|*.utx|Rich Text Files|*.rtf|All Files|*.*";
        int m_lastCharPrinted;
        private Color m_statusLblTypedTextBackColor;
        private static System.OperatingSystem m_osInfo = System.Environment.OSVersion;

        public GumPad(string[] args)
        {
            InitializeComponent();

            GumTrace.setTracing(Settings.Default.TraceLevel,
                Settings.Default.TraceOutput, Settings.Default.TraceFileName);

            txtRTF.WordWrap = wordWrapToolStripMenuItem.Checked = Settings.Default.WordWrap;
            txtRTF.TypedTextStatusLabel = statusLblTypedText;
            statusLabelSpacer.Text = "";

            txtRTF.TransliterateAsEntityCode = Settings.Default.TransliterateAsEntityCode;
            txtRTF.TransliterationLanguage = Settings.Default.Language;
            setupConvertAsyouType(Settings.Default.ConvertAsYouType);

            statusLabelTransliterator.Text = Settings.Default.ConversionSchemeName;
            statusLabelTransliterator.ToolTipText = Settings.Default.ConversionSchemeDescription;

            convertFromExtendedLatinToolStripMenuItem.ToolTipText =
                 "Leave this unchecked if you are converting plain English "
                        + "text to an Indian Language.\n"
                        + "Leave it checked if you are converting extended English "
                        + "text to an Indian Language.";

            // open file if filename was supplied on the command line
            if (args.Length != 0)
            {
                openDocument(args[0]);
            }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
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
            printDocument1.DocumentName = m_fileName;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void openDocument(String filename)
        {
            txtRTF.SelectionFont = DefaultFont;
            try
            {
                if (filename.EndsWith(".gpd", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(filename, RichTextBoxStreamType.RichText);
                }
                else if (filename.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(filename, RichTextBoxStreamType.RichText);
                }
                else if (filename.EndsWith(".utx", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(filename, RichTextBoxStreamType.UnicodePlainText);
                }
                else if (filename.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(filename, RichTextBoxStreamType.PlainText);
                }
                else if (filename.EndsWith(".itx", StringComparison.OrdinalIgnoreCase))
                {
                    txtRTF.LoadFile(filename, RichTextBoxStreamType.PlainText);
                }
                else if (filename.EndsWith(".map", StringComparison.OrdinalIgnoreCase))
                {
                    Cursor = Cursors.WaitCursor;
                    StreamReader instream = new StreamReader(filename, Encoding.UTF8);
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
                    StreamReader instream = new StreamReader(filename, frmEncoding.SelectedEncoding);
                    txtRTF.SelectionFont = DefaultFont;
                    txtRTF.Text = instream.ReadToEnd();
                    instream.Close();
                    Cursor = Cursors.Default;
                }
                m_fileName = filename;
                Text = m_fileName;
                txtRTF.Modified = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open file: " + filename);
            }
            Cursor = Cursors.Default;
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
                Settings.Default.DefaultFileFilterIndex = openFileDialog1.FilterIndex;
                setupConvertAsyouType(false);
                openDocument(openFileDialog1.FileName);
            }
        }

        private DialogResult checkIfModified()
        {
            DialogResult res = DialogResult.None;
            if (txtRTF.Modified == true)
            {
                string temp=m_fileName;
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
            if (m_osInfo.Version.Major < 6)
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
            Text = "";
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
            m_lastCharPrinted = 0;
        }

        private void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            txtRTF.PrintDone();
        }
        
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            m_lastCharPrinted = txtRTF.Print(m_lastCharPrinted, txtRTF.TextLength, e);
            if (m_lastCharPrinted < txtRTF.TextLength)
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

            saveFileDialog1.FileName = Path.GetFileName(m_fileName);
            if (m_fileName.Trim().Equals("") || Path.GetDirectoryName(m_fileName).Equals(""))
            {
                saveFileDialog1.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            }
            else
            {
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(m_fileName);
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_fileName = saveFileDialog1.FileName;
                Text = m_fileName;
                if (m_fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    m_fileType = RichTextBoxStreamType.PlainText;
                }
                else if (m_fileName.EndsWith(".utx", StringComparison.OrdinalIgnoreCase))
                {
                    m_fileType = RichTextBoxStreamType.UnicodePlainText;
                }
                else if (m_fileName.EndsWith(".itx", StringComparison.OrdinalIgnoreCase))
                {
                    m_fileType = RichTextBoxStreamType.PlainText;
                }
                else
                {
                    m_fileType=RichTextBoxStreamType.RichText;
                }
                txtRTF.SaveFile(m_fileName, m_fileType);
                txtRTF.Modified = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fileName.Equals("")) {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }
            txtRTF.SaveFile(m_fileName, m_fileType);
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
            convertAsYouTypeToolStripMenuItem.Checked = convertAsYouType;
            Settings.Default.ConvertAsYouType = convertAsYouType;

            if (convertAsYouType)
            {
                convertToolStripMenuItem.Visible = false;
                statusLabelSpacer.Text = "Mode: Convert as you type to ";
                statusLblTypedText.Text = "";
                m_statusLblTypedTextBackColor = statusLblTypedText.BackColor;
                statusLblTypedText.BackColor = Color.Yellow;
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
                statusLblTypedText.BackColor = m_statusLblTypedTextBackColor;
                statusLangDropDown.Visible = false;
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

        private void convertAsYouTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            convertAsYouTypeToolStripMenuItem.Checked = !convertAsYouTypeToolStripMenuItem.Checked;
            setupConvertAsyouType(convertAsYouTypeToolStripMenuItem.Checked);
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

        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUserGuide ug = new FormUserGuide();
            ug.ShowDialog();
            ug.Dispose();
        }

        private string getRelNotesText(ReleaseNotes relNotes, string currentVer)
        {
            StringBuilder messagebuff = new StringBuilder();
            //Array.Sort(relNotes.notes); // reverse sort rel notes by version
            foreach (RelNote note in relNotes.notes)
            {
                if ((note.m_version != null)
                    && (currentVer.CompareTo(note.m_version) <= 0))
                {
                    messagebuff.Append("Release Notes for version : ");
                    messagebuff.Append(note.m_version);

                    messagebuff.Append("\nNew Features :\n");
                    foreach (string feature in note.m_features)
                    {
                        messagebuff.Append(feature);
                        messagebuff.Append("\n");
                    }
                    messagebuff.Append("\nFixes :\n");
                    foreach (string fix in note.m_fixes)
                    {
                        messagebuff.Append(fix);
                        messagebuff.Append("\n");
                    }
                    messagebuff.Append("\nKnown Issues :\n");
                    foreach (string issue in note.m_known_issues)
                    {
                        messagebuff.Append(issue);
                        messagebuff.Append("\n");
                    }
                }
            }
            return messagebuff.ToString();
        }

        private bool isNewVersionAvailable(bool silentCheck)
        {
            WebClient wc = new WebClient();

            string xmlFile = Path.GetTempPath() + "gumpad-relnotes.xml";

            GumTrace.log(TraceEventType.Information, "rel notes file=" + xmlFile);

            ReleaseNotes relNotes = new ReleaseNotes();
            try
            {
                wc.DownloadFile(relNotesURL, xmlFile);
                wc.Dispose();

                StreamReader relNotesStream = new StreamReader(xmlFile);
                XmlSerializer serializer = new XmlSerializer(typeof(ReleaseNotes));
                relNotes = (ReleaseNotes)serializer.Deserialize(relNotesStream);
                relNotesStream.Close();

            }
            catch (Exception ex)
            {
                if (!silentCheck)
                {
                    MessageBox.Show("Check for updates failed - " + ex.Message);
                }
                return false;
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            GumTrace.log(TraceEventType.Information, "latest rev=" + relNotes.m_latest_version);

            Version currentVer = System.Reflection.Assembly.GetExecutingAssembly().
                     GetName().Version;

            StringBuilder messagebuff = new StringBuilder();
            FormCheckForUpdates f = new FormCheckForUpdates();
            if ((relNotes.m_latest_version != null) &&
                (currentVer.ToString().CompareTo(relNotes.m_latest_version) < 0))
            {
                messagebuff.Append("\n\nA new version of GumPad is available at http://gumpad.org/");
                messagebuff.Append("\n\n");
                messagebuff.Append("You are currently running version : ");
                messagebuff.Append(currentVer.ToString());
                messagebuff.Append("\n");
                messagebuff.Append("The latest version available at http://gumpad.org is : ");
                messagebuff.Append(relNotes.m_latest_version);
                messagebuff.Append("\n\n");

                messagebuff.Append(getRelNotesText(relNotes, currentVer.ToString()));

                //messagebuff.Append("\nClick on the Download and Install button below to update to the latest version");

                f.setMessageText(messagebuff.ToString());

                f.setInstallerURLandName(filesURL, relNotes.m_latest_installer);
                f.ShowDialog();
                f.Dispose();
                return true;
            }
            else
            {
                if (!silentCheck)
                {
                    messagebuff.Append("\n\nThe version you are currently using is : ");
                    messagebuff.Append(currentVer.ToString());
                    messagebuff.Append("\n\n");
                    messagebuff.Append("\n\nYou are already running the latest version of GumPad\n\n");
                    messagebuff.Append(getRelNotesText(relNotes, currentVer.ToString()));
                    f.setMessageText(messagebuff.ToString());
                    f.ShowDialog();
                    f.Dispose();
                }
                return false;
            }
        }

        private const string filesURL = "http://gumpad.googlecode.com/files/";
        private const string relNotesURL = "http://gumpad.googlecode.com/files/ReleaseNotes.xml";

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please go to http://gumpad.org/ to check for updates.\nYou can also subscribe "
            + "to an RSS feed at the above site to be notified of updates.", "Check for updates");
        }

        private void statusLblTypedText_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(statusLblTypedText.Text);
                SystemSounds.Asterisk.Play();
            }
            catch (Exception)
            {
                // do nothing...
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            undoToolStripMenuItem1.Enabled = txtRTF.CanUndo;
            cutToolStripMenuItem.Enabled = (txtRTF.SelectionLength != 0);
            copyToolStripMenuItem1.Enabled = (txtRTF.SelectionLength != 0);
            pasteToolStripMenuItem1.Enabled = txtRTF.CanPaste(DataFormats.GetFormat(DataFormats.UnicodeText));
            convertToolStripMenuItem1.Enabled = !(Settings.Default.ConvertAsYouType);
        }
    }
}