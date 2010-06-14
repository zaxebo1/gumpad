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
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.IO;
using System.Diagnostics;

namespace GumLib
{
    /// <summary>
    /// Specialized RichTextBox that 
    /// implements Print, Print Preview
    /// and Convert As you Type
    /// </summary>
	public class GumPadTextBox : RichTextBox
	{

        private string buff = "";
        private Transliterator transliterator;
        private String transliterationResult;
        private int transliteratedTextLength = 0;
        private IntPtr eventMask = IntPtr.Zero;

        /// <summary>
        /// Default constructor. Initializes internal reference
        /// to GumLib.Transliterator and sets RichTextBox language 
        /// display option
        /// </summary>
        public GumPadTextBox()
            : base()
       {
            AcceptsTab = true;
            transliterator = new Transliterator();
            LanguageOption = RichTextBoxLanguageOptions.AutoFontSizeAdjust
                        | RichTextBoxLanguageOptions.DualFont
                        | RichTextBoxLanguageOptions.UIFonts;
        }

        //Convert the unit used by the .NET framework (1/100 inch) 
		//and the unit used by Win32 API calls (twips 1/1440 inch)
		private const double anInch = 14.4;

		[StructLayout(LayoutKind.Sequential)] 
			private struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
			private struct CHARRANGE
		{
			public int cpMin;         //First character of range (0 for start of doc)
			public int cpMax;           //Last character of range (-1 for end of doc)
		}

		[StructLayout(LayoutKind.Sequential)]
			private struct FORMATRANGE
		{
			public IntPtr hdc;             //Actual DC to draw on
			public IntPtr hdcTarget;       //Target DC for determining text formatting
			public RECT rc;                //Region of the DC to draw to (in twips)
			public RECT rcPage;            //Region of the whole DC (page size) (in twips)
			public CHARRANGE chrg;         //Range of text to draw (see earlier declaration)
		}

		private const int WM_USER  = 0x0400;
        private const int WM_SETREDRAW = 0x000B;
        private const int EM_FORMATRANGE = WM_USER + 57;
        private const int EM_GETEVENTMASK = WM_USER + 59;
        private const int EM_SETEVENTMASK = WM_USER + 69;
		
        /// <summary>
        /// Render the contents of the RichTextBox for printing.
        /// Returns the last character printed + 1 
        /// (printing start from this point for next page)
        /// </summary>
        /// <param name="charFrom"></param>
        /// <param name="charTo"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public int Print( int charFrom, int charTo,PrintPageEventArgs e)
		{
			//Calculate the area to render and print
			RECT rectToPrint; 
			rectToPrint.Top = (int)(e.MarginBounds.Top * anInch);
			rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * anInch);
			rectToPrint.Left = (int)(e.MarginBounds.Left * anInch);
			rectToPrint.Right = (int)(e.MarginBounds.Right * anInch);

			//Calculate the size of the page
			RECT rectPage; 
			rectPage.Top = (int)(e.PageBounds.Top * anInch);
			rectPage.Bottom = (int)(e.PageBounds.Bottom * anInch);
			rectPage.Left = (int)(e.PageBounds.Left * anInch);
			rectPage.Right = (int)(e.PageBounds.Right * anInch);

			IntPtr hdc = e.Graphics.GetHdc();

			FORMATRANGE fmtRange;
			fmtRange.chrg.cpMax = charTo;				//Indicate character from to character to 
			fmtRange.chrg.cpMin = charFrom;
			fmtRange.hdc = hdc;                    //Use the same DC for measuring and rendering
			fmtRange.hdcTarget = hdc;              //Point at printer hDC
			fmtRange.rc = rectToPrint;             //Indicate the area on page to print
			fmtRange.rcPage = rectPage;            //Indicate size of page

			IntPtr res = IntPtr.Zero;

			IntPtr wparam = IntPtr.Zero;
			wparam = new IntPtr(1);

			//Get the pointer to the FORMATRANGE structure in memory
			IntPtr lparam= IntPtr.Zero;
			lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
			Marshal.StructureToPtr(fmtRange, lparam, false);

			//Send the rendered data for printing 
            Message m = Message.Create(Handle, EM_FORMATRANGE, wparam, lparam);
            WndProc(ref m);
            res = m.Result;

			//Free the block of memory allocated
			Marshal.FreeCoTaskMem(lparam);

			//Release the device context handle obtained by a previous call
			e.Graphics.ReleaseHdc(hdc);

			//Return last + 1 character printer
			return res.ToInt32();
		}

        /// <summary>
        /// 
        /// </summary>
        public void PrintDone()
        {
            IntPtr wparam = IntPtr.Zero;
            IntPtr lparam = (IntPtr)(null);
            Message m = Message.Create(Handle, EM_FORMATRANGE, wparam, lparam);
            WndProc(ref m);
        }

        private bool convertAsYouType=true;

        /// <summary>
        /// Gets or Sets Convert As You Type flag
        /// Setting this flag will cause characters
        /// to be transliterated as they are keyed
        /// into the GumPadTextBox control
        /// </summary>
        public bool ConvertAsYouType
        {
            get { return convertAsYouType; }
            set 
            {
                buff = "";
                transliteratedTextLength = 0;
                convertAsYouType = value;
            }
        }

        /// <summary>
        /// Returns reference to GumLib.Transliterator object
        /// </summary>
        public Transliterator Transliterator
        {
            get { return transliterator; }
        }
	

        private String transliterationLanguage = Transliterator.TELUGU;

        /// <summary>
        /// Gets or Sets target language for transliteration
        /// </summary>
        public String TransliterationLanguage
        {
            get { return transliterationLanguage; }
            set 
            {
                transliterationLanguage = value;
                transliterator.Language = transliterationLanguage;
            }
        }
	
        private bool transliterateAsEntityCode = false;

        /// <summary>
        /// Gets or Sets flag to indicate if transliterated
        /// output is in the form of html entity code references
        /// instead of unicode characters
        /// </summary>
        public bool TransliterateAsEntityCode
        {
            get { return transliterateAsEntityCode; }
            set { transliterateAsEntityCode = value; }
        }

        private ToolStripStatusLabel typedText;

        /// <summary>
        /// Gets or Sets informational text displayed
        /// on the tool strip
        /// </summary>
        public ToolStripStatusLabel TypedTextStatusLabel
        {
            get { return typedText; }
            set { typedText = value; }
        }

        private Font displayFont;

        /// <summary>
        /// Sets the Font of the text being transliterated
        /// </summary>
        public Font DisplayFont
        {
            set
            {
                displayFont = value;
                SelectionFont = displayFont;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectionChanged(EventArgs e)
        {
            if (convertAsYouType)
            {
                SelectionFont = displayFont;
            }
            transliteratedTextLength = 0;
            base.OnSelectionChanged(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!convertAsYouType)
            {
                return;
            }
            buff = "";
            typedText.Text = buff;
            transliteratedTextLength = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!convertAsYouType)
            {
                return;
            }
            Keys k = e.KeyCode;
            if (
                (k == Keys.Up)
                || (k == Keys.Down)
                || (k == Keys.Left)
                || (k == Keys.Right)
                || (k == Keys.Home)
                || (k == Keys.End)
                )
            {
                buff = "";
                typedText.Text = buff;
                transliteratedTextLength = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!convertAsYouType)
            {
                e.Handled = false;
                base.OnKeyPress(e);
                return;
            }

            char c = e.KeyChar;

            if (Char.IsWhiteSpace(c) || Char.IsControl(c))
            {
                e.Handled = false;
                base.OnKeyPress(e);
                buff = "";
                typedText.Text = buff;
                transliteratedTextLength = 0;
            }
            else
            {
                e.Handled = true;
                base.OnKeyPress(e);
                buff += c;
                transliterationResult = transliterator.TransliterateWord(buff, transliterateAsEntityCode);
                try
                {
                    SuspendUpdates();
                    Select(SelectionStart - transliteratedTextLength, transliteratedTextLength);
                    Cut();
                    SelectedText = transliterationResult;
                    transliteratedTextLength = transliterationResult.Length;
                    Modified = true;
                }
                catch (Exception)
                {
                    transliteratedTextLength = 0;
                }
                finally
                {
                    ResumeUpdates();
                }
                typedText.Text = buff;
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SuspendUpdates()
        {
            Message m = Message.Create(Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            WndProc(ref m);
            m = Message.Create(Handle, EM_GETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
            WndProc(ref m);
            eventMask = m.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumeUpdates()
        {
            Message m = Message.Create(Handle, EM_GETEVENTMASK, IntPtr.Zero, eventMask);
            WndProc(ref m);
            m = Message.Create(Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            WndProc(ref m);
            Refresh();
        }

        /// <summary>
        /// Transliterates selected text.
        /// Transliterates the entire text if 
        /// none is selected.
        /// </summary>
        public void Transliterate()
        {
            try
            {
                SuspendUpdates();
                if (SelectionLength == 0)
                {
                    SelectAll();
                }
                int selStart = SelectionStart;
                String text = SelectedText;
                StringBuilder result = new StringBuilder(text.Length);
                transliterator.Transliterate(new StringReader(text), new StringWriter(result), transliterateAsEntityCode);
                SelectedText = result.ToString();
                SelectionStart = selStart;
                SelectionLength = result.Length;
                SelectionFont = displayFont;
                //position cursor at end of selected text
                if (result.Length > 0)
                {
                    Select(selStart + result.Length - 1, 0);
                }
                SendKeys.SendWait("{RIGHT}");
            }
            catch (Exception e)
            {
                GumTrace.log(TraceEventType.Error, e.StackTrace);
            }
            finally
            {
                DeselectAll();
                ResumeUpdates();
            }
            Modified = true;
        }
	}
}