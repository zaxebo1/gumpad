using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GumLib
{
    public partial class FormUserGuide : Form
    {
        private static bool isUGLoaded = false;

        public FormUserGuide()
        {
            InitializeComponent();
            loadUserGuide();
        }

        private void loadUserGuide()
        {
            string appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder appdir = new StringBuilder(Path.Combine(appdatadir, "GumPad"));
            if (!Directory.Exists(appdir.ToString()))
            {
                Directory.CreateDirectory(appdir.ToString());
            }
            string userguidefile = Path.Combine(appdir.ToString(), "userguide.mht");
            if (!isUGLoaded)
            {
                FileStream w = new FileStream(userguidefile, FileMode.Create);
                byte[] b = GumLib.UserGuide.ToArray<byte>();
                w.Write(b, 0, b.Length);
                w.Close();
                isUGLoaded = true;
            }
            webBrowser1.Navigate(userguidefile);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void showMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transliterator t = new Transliterator();
            String html = t.printLetterMap();
            webBrowser1.DocumentText = html;
        }

        private void showUserGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadUserGuide();
        }
    }
}
