namespace GumPad4Word2007
{
    partial class GumPadRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public GumPadRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabTrans = this.Factory.CreateRibbonTab();
            this.grpTrans = this.Factory.CreateRibbonGroup();
            this.galleryLang = this.Factory.CreateRibbonGallery();
            this.btnDevanagari = this.Factory.CreateRibbonButton();
            this.btnTelugu = this.Factory.CreateRibbonButton();
            this.btnTamil = this.Factory.CreateRibbonButton();
            this.btnKannada = this.Factory.CreateRibbonButton();
            this.btnMalayalam = this.Factory.CreateRibbonButton();
            this.btnMarathi = this.Factory.CreateRibbonButton();
            this.btnOriya = this.Factory.CreateRibbonButton();
            this.btnBengali = this.Factory.CreateRibbonButton();
            this.btnGujarati = this.Factory.CreateRibbonButton();
            this.btnGurmukhi = this.Factory.CreateRibbonButton();
            this.btnLatin = this.Factory.CreateRibbonButton();
            this.btnLatinEx = this.Factory.CreateRibbonButton();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.chkFromLatinEx = this.Factory.CreateRibbonCheckBox();
            this.btnMap = this.Factory.CreateRibbonButton();
            this.btnHelp = this.Factory.CreateRibbonButton();
            this.tabTrans.SuspendLayout();
            this.grpTrans.SuspendLayout();
            // 
            // tabTrans
            // 
            this.tabTrans.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tabTrans.ControlId.OfficeId = "TabHome";
            this.tabTrans.Groups.Add(this.grpTrans);
            this.tabTrans.Label = "TabHome";
            this.tabTrans.Name = "tabTrans";
            // 
            // grpTrans
            // 
            this.grpTrans.Items.Add(this.galleryLang);
            this.grpTrans.Items.Add(this.separator1);
            this.grpTrans.Items.Add(this.chkFromLatinEx);
            this.grpTrans.Items.Add(this.btnMap);
            this.grpTrans.Items.Add(this.btnHelp);
            this.grpTrans.Label = "Transliteration";
            this.grpTrans.Name = "grpTrans";
            // 
            // galleryLang
            // 
            this.galleryLang.Buttons.Add(this.btnDevanagari);
            this.galleryLang.Buttons.Add(this.btnTelugu);
            this.galleryLang.Buttons.Add(this.btnTamil);
            this.galleryLang.Buttons.Add(this.btnKannada);
            this.galleryLang.Buttons.Add(this.btnMalayalam);
            this.galleryLang.Buttons.Add(this.btnMarathi);
            this.galleryLang.Buttons.Add(this.btnOriya);
            this.galleryLang.Buttons.Add(this.btnBengali);
            this.galleryLang.Buttons.Add(this.btnGujarati);
            this.galleryLang.Buttons.Add(this.btnGurmukhi);
            this.galleryLang.Buttons.Add(this.btnLatin);
            this.galleryLang.Buttons.Add(this.btnLatinEx);
            this.galleryLang.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.galleryLang.Label = "Convert";
            this.galleryLang.Name = "galleryLang";
            this.galleryLang.OfficeImageId = "Translate";
            this.galleryLang.ShowImage = true;
            this.galleryLang.SuperTip = "Select text you want to convert. Then click this button and select the script to " +
                "convert to.";
            this.galleryLang.ButtonClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.galleryLang_ButtonClick);
            // 
            // btnDevanagari
            // 
            this.btnDevanagari.Label = "Devanagari";
            this.btnDevanagari.Name = "btnDevanagari";
            this.btnDevanagari.OfficeImageId = "ShapeRightArrow";
            this.btnDevanagari.ShowImage = true;
            // 
            // btnTelugu
            // 
            this.btnTelugu.Label = "Telugu";
            this.btnTelugu.Name = "btnTelugu";
            this.btnTelugu.OfficeImageId = "ShapeRightArrow";
            this.btnTelugu.ShowImage = true;
            // 
            // btnTamil
            // 
            this.btnTamil.Label = "Tamil";
            this.btnTamil.Name = "btnTamil";
            this.btnTamil.OfficeImageId = "ShapeRightArrow";
            this.btnTamil.ShowImage = true;
            // 
            // btnKannada
            // 
            this.btnKannada.Label = "Kannada";
            this.btnKannada.Name = "btnKannada";
            this.btnKannada.OfficeImageId = "ShapeRightArrow";
            this.btnKannada.ShowImage = true;
            // 
            // btnMalayalam
            // 
            this.btnMalayalam.Label = "Malayalam";
            this.btnMalayalam.Name = "btnMalayalam";
            this.btnMalayalam.OfficeImageId = "ShapeRightArrow";
            this.btnMalayalam.ShowImage = true;
            // 
            // btnMarathi
            // 
            this.btnMarathi.Label = "Marathi";
            this.btnMarathi.Name = "btnMarathi";
            this.btnMarathi.OfficeImageId = "ShapeRightArrow";
            this.btnMarathi.ShowImage = true;
            // 
            // btnOriya
            // 
            this.btnOriya.Label = "Oriya";
            this.btnOriya.Name = "btnOriya";
            this.btnOriya.OfficeImageId = "ShapeRightArrow";
            this.btnOriya.ShowImage = true;
            // 
            // btnBengali
            // 
            this.btnBengali.Label = "Bengali";
            this.btnBengali.Name = "btnBengali";
            this.btnBengali.OfficeImageId = "ShapeRightArrow";
            this.btnBengali.ShowImage = true;
            // 
            // btnGujarati
            // 
            this.btnGujarati.Label = "Gujarati";
            this.btnGujarati.Name = "btnGujarati";
            this.btnGujarati.OfficeImageId = "ShapeRightArrow";
            this.btnGujarati.ShowImage = true;
            // 
            // btnGurmukhi
            // 
            this.btnGurmukhi.Label = "Gurmukhi";
            this.btnGurmukhi.Name = "btnGurmukhi";
            this.btnGurmukhi.OfficeImageId = "ShapeRightArrow";
            this.btnGurmukhi.ShowImage = true;
            // 
            // btnLatin
            // 
            this.btnLatin.Label = "Latin";
            this.btnLatin.Name = "btnLatin";
            this.btnLatin.OfficeImageId = "ShapeRightArrow";
            this.btnLatin.ShowImage = true;
            // 
            // btnLatinEx
            // 
            this.btnLatinEx.Label = "Extended Latin";
            this.btnLatinEx.Name = "btnLatinEx";
            this.btnLatinEx.OfficeImageId = "ShapeRightArrow";
            this.btnLatinEx.ShowImage = true;
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // chkFromLatinEx
            // 
            this.chkFromLatinEx.Label = "Extended Latin";
            this.chkFromLatinEx.Name = "chkFromLatinEx";
            this.chkFromLatinEx.SuperTip = "Leave this box unchecked for normal conversion from English text. Check the box i" +
                "f the selected text is in Extended Latin.";
            this.chkFromLatinEx.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.chkFromLatinEx_Click);
            // 
            // btnMap
            // 
            this.btnMap.Label = "Customize map";
            this.btnMap.Name = "btnMap";
            this.btnMap.OfficeImageId = "RelationshipsEditRelationships";
            this.btnMap.ShowImage = true;
            this.btnMap.SuperTip = "Click to customize the conversion map.";
            this.btnMap.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnMap_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Label = "Help";
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.OfficeImageId = "Help";
            this.btnHelp.ShowImage = true;
            // 
            // GumPadRibbon
            // 
            this.Name = "GumPadRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tabTrans);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.GumPadRibbon_Load);
            this.tabTrans.ResumeLayout(false);
            this.tabTrans.PerformLayout();
            this.grpTrans.ResumeLayout(false);
            this.grpTrans.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabTrans;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpTrans;
        internal Microsoft.Office.Tools.Ribbon.RibbonGallery galleryLang;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnTelugu;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnDevanagari;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnTamil;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnKannada;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnMalayalam;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnMarathi;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnOriya;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnBengali;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnGujarati;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnGurmukhi;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnLatin;
        private Microsoft.Office.Tools.Ribbon.RibbonButton btnLatinEx;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkFromLatinEx;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnMap;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnHelp;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
    }

    partial class ThisRibbonCollection
    {
        internal GumPadRibbon GumPadRibbon
        {
            get { return this.GetRibbon<GumPadRibbon>(); }
        }
    }
}
