using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;
using GumLib;
using System.Windows.Forms;


namespace GumPad4Word2007
{
    public partial class GumPadRibbon
    {
        private Transliterator transliterator;
        private Word.Document doc;
        private Word.Application app;

        private void GumPadRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            this.grpTrans.PerformDynamicLayout();
            app = Globals.ThisAddIn.Application;
            doc = Globals.ThisAddIn.Application.ActiveDocument;
            transliterator = new Transliterator();
            transliterator.UseLatinExMapForConversion = chkFromLatinEx.Checked;
            btnHelp.Label = "";
        }

        private void chkFromLatinEx_Click(object sender, RibbonControlEventArgs e)
        {
            transliterator.UseLatinExMapForConversion = chkFromLatinEx.Checked;
        }

        private void galleryLang_ButtonClick(object sender, RibbonControlEventArgs e)
        {
            String sel = app.Selection.Text;

            if (sel.Length == 0)
            {
                return;
            }

            if (sender.Equals(btnTelugu))
            {
                transliterator.Language = Transliterator.TELUGU;
            }
            else if (sender.Equals(btnDevanagari))
            {
                transliterator.Language = Transliterator.DEVANAGARI;
            }
            else if (sender.Equals(btnBengali))
            {
                transliterator.Language = Transliterator.BENGALI;
            }
            else if (sender.Equals(btnGujarati))
            {
                transliterator.Language = Transliterator.GUJARATI;
            }
            else if (sender.Equals(btnGurmukhi))
            {
                transliterator.Language = Transliterator.GURMUKHI;
            }
            else if (sender.Equals(btnKannada))
            {
                transliterator.Language = Transliterator.KANNADA;
            }
            else if (sender.Equals(btnLatin))
            {
                transliterator.Language = Transliterator.LATIN;
            }
            else if (sender.Equals(btnLatinEx))
            {
                transliterator.Language = Transliterator.LATINEX;
            }
            else if (sender.Equals(btnMalayalam))
            {
                transliterator.Language = Transliterator.MALAYALAM;
            }
            else if (sender.Equals(btnMarathi))
            {
                transliterator.Language = Transliterator.MARATHI;
            }
            else if (sender.Equals(btnOriya))
            {
                transliterator.Language = Transliterator.ORIYA;
            }
            else if (sender.Equals(btnTamil))
            {
                transliterator.Language = Transliterator.TAMIL;
            }
            else
            {
                return;
            }

            StringBuilder result = new StringBuilder(sel.Length);
            transliterator.Transliterate(new StringReader(sel), new StringWriter(result), false);
            app.Selection.Text = result.ToString();
        }

        private void btnMap_Click(object sender, RibbonControlEventArgs e)
        {
        
            FormConversionMap fm = new FormConversionMap(transliterator);
            DialogResult res = fm.ShowDialog();
            fm.Dispose();
        }

        private void btnHelp_Click(object sender, RibbonControlEventArgs e)
        {
            FormUserGuide ug = new FormUserGuide();
            ug.ShowDialog();
            ug.Dispose();
        }
    }
}
