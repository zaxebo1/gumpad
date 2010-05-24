/*
 * Copyright © 2007-2010, Pradyumna Revur.
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using GumLib;

namespace GumPad4Word2007
{
    public partial class FormConversionMap : Form
    {
        private Transliterator m_transliterator;
        private ToolStripStatusLabel m_statusLabelTransliterator;
        private static String MAPFILEFILTER = "Conversion Map Files|*.map|All Files|*.*";
        private AksharaMapping[] m_AksharaMappings;

        public FormConversionMap(Transliterator transliterator)
        {
            InitializeComponent();
            m_transliterator = transliterator;
            m_statusLabelTransliterator = new ToolStripStatusLabel();
            m_AksharaMappings = m_transliterator.getAksharaMappings().ToArray();
            dataGridView1.DataSource = m_AksharaMappings;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            enableAdvancedEditMode();

            m_transliterator = transliterator;
        }

        private void btnMapReset_Click(object sender, EventArgs e)
        {
            string appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder usermapdir = new StringBuilder(Path.Combine(appdatadir, "GumPad"));
            string usermapfile = Path.Combine(usermapdir.ToString(), ".gumpad.map");
            if (File.Exists(usermapfile))
            {
                File.Delete(usermapfile);
            }
            m_transliterator.ReloadConversionMap();
            m_AksharaMappings = m_transliterator.getAksharaMappings().ToArray();
            dataGridView1.DataSource = m_AksharaMappings;
        }

        private void btnMapCancel_Click(object sender, EventArgs e)
        {
            FormConversionMap.ActiveForm.Close();
        }

        private void btnMapApply_Click(object sender, EventArgs e)
        {
            try
            {
                TransliterationMap.saveUserMapFile(m_transliterator, m_AksharaMappings,
            false, "Locally modified version", "Modified by " + Environment.UserName);
                m_transliterator.ReloadConversionMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FormConversionMap.ActiveForm.Close();
        }

        private void btnLoadCustomMap_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            openFileDialog1.Filter = MAPFILEFILTER;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowReadOnly = true;
            openFileDialog1.SupportMultiDottedExtensions = true;
            openFileDialog1.Multiselect = false;
            DialogResult res = openFileDialog1.ShowDialog();
            string schemeName = "";
            string contributorName = "";
            if (res == DialogResult.OK)
            {
                try
                {
                    if (!TransliterationMap.loadMap(m_transliterator,
                        new StreamReader(openFileDialog1.FileName, Encoding.UTF8),
                        false,
                        out schemeName,
                        out contributorName))
                    {
                        MessageBox.Show("Load failed.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Load failed.\n\n"
                        + ex.Message);
                }
                m_AksharaMappings = m_transliterator.getAksharaMappings().ToArray();
                dataGridView1.DataSource = m_AksharaMappings;
                try
                {
                    TransliterationMap.validateMappings(m_AksharaMappings);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fix the following errors before using this map!\n\n"
                        + ex.Message);
                }
                m_transliterator.ReloadConversionMap();
            }
        }

        private void btnSaveCustomMap_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = MAPFILEFILTER;
            saveFileDialog1.DefaultExt = ".map";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.InitialDirectory = System.Environment.SpecialFolder.MyDocuments.ToString();
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (!TransliterationMap.saveMap(m_transliterator,
                    saveFileDialog1.FileName,
                    m_statusLabelTransliterator.Text,
                    m_statusLabelTransliterator.ToolTipText))
                {
                    MessageBox.Show("Save failed.");
                }
            }
        }

        private void enableAdvancedEditMode()
        {
            m_AksharaMappings = m_transliterator.getAksharaMappings().ToArray();
            dataGridView1.DataSource = m_AksharaMappings;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.ReadOnly = false;
                col.Visible = true;
            }

            dataGridView1.Columns[0].ReadOnly = true;

            dataGridView1.Columns[0].HeaderText = "Name";
            dataGridView1.Columns[1].HeaderText = "Input";
            dataGridView1.Columns[2].HeaderText = "Output";
            dataGridView1.Columns[3].HeaderText = "Extended Latin";
            dataGridView1.Columns[4].HeaderText = "Language";
            dataGridView1.Refresh();
        }
    }
}
