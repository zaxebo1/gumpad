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
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;


namespace GumLib
{
    /// <summary>
    /// The Transliterator.
    /// Converts a sequence of input characters
    /// to a sequence of output characters based on
    /// customizable input output mappings defined
    /// in an XML text stream. The default built-in
    /// mappings correspond closely, although not exactly
    /// with the ITRANS transliteration scheme.
    /// </summary>
    public class Transliterator
    {

        private Dictionary<Regex, Char[]> patternMap;
        private Dictionary<Regex, Char[]> patternMapForLatinEx;
        private Dictionary<Char, Char> vowelMap;
        private Regex[] ptab;
        private Regex[] ptabForLatinEx;
        private int unicodeRangeStart = 0x0C00; // default is telugu
        private String lang = TELUGU; //default is telugu
        private String akaar = "a"; //default latin equivalent of akaar
        private int maxPatternLength = 0; // max length of transliteration pattern
        private bool useLatinExMapForConversion = false;


        private readonly Dictionary<string, ConversionTable> COMMONMAP;
        private readonly Dictionary<string, ConversionTable> DEVANAGARIMAP;
        private readonly Dictionary<string, ConversionTable> BENGALIMAP;
        private readonly Dictionary<string, ConversionTable> GUJARATIMAP;
        private readonly Dictionary<string, ConversionTable> GURMUKHIMAP;
        private readonly Dictionary<string, ConversionTable> KANNADAMAP;
        private readonly Dictionary<string, ConversionTable> ORIYAMAP;
        private readonly Dictionary<string, ConversionTable> TAMILMAP;
        private readonly Dictionary<string, ConversionTable> TELUGUMAP;
        private readonly Dictionary<string, ConversionTable> MALAYALAMMAP;
        //        private readonly Dictionary<string, TransOut> MARATHIMAP;

        /// <summary>
        /// Transliterates to Telugu
        /// </summary>
        public const String TELUGU = "Telugu"; //"te";
        /// <summary>
        /// Transliterates to Devanagari (Hindi)
        /// </summary>
        public const String DEVANAGARI = "Devanagari"; //"hi";
        /// <summary>
        /// Transliterates to Tamil
        /// </summary>
        public const String TAMIL = "Tamil"; //"ta";
        /// <summary>
        /// Transliterates to Kannada
        /// </summary>
        public const String KANNADA = "Kannada"; //"ka";
        /// <summary>
        /// Transliterates to Malayalam
        /// </summary>
        public const String MALAYALAM = "Malayalam"; //"ma";
        /// <summary>
        /// Transliterates to Oriya
        /// </summary>
        public const String ORIYA = "Oriya"; //"or";
        /// <summary>
        /// Transliterates to Marathi.
        /// Identical to Devanagari.
        /// </summary>
        public const String MARATHI = "Marathi"; //"mr";
        /// <summary>
        /// Transliterates to Gujarati
        /// </summary>
        public const String GUJARATI = "Gujarati"; //"gu";
        /// <summary>
        /// Transliterates to Bengali
        /// </summary>
        public const String BENGALI = "Bengali"; //"be";
        /// <summary>
        /// Transliterates to Gurmukhi
        /// </summary>
        public const String GURMUKHI = "Gurmukhi"; //"gr";
        /// <summary>
        /// Reverse transliterates to English 
        /// </summary>
        public const String LATIN = "Latin"; //"en";
        /// <summary>
        /// Transliterates to Extended Latin per ISO-15919
        /// </summary>
        public const String LATINEX = "LatinEx"; //"en";

        private String[,] LANGMAP = {
            { TELUGU, "0C00" },
            { DEVANAGARI, "0900" },
            { TAMIL, "0B80" },
            { KANNADA, "0C80" },
            { MALAYALAM, "0D00" },
            { ORIYA, "0B00" },
            { MARATHI, "0900" },
            { GUJARATI, "0A80" },
            { BENGALI, "0980" },
            { GURMUKHI, "0A00" },
            { LATIN, "0000" },
            { LATINEX, "0080" }
        };

        struct ConversionTable
        {
            public string[] m_input_sequences;
            public int[] m_desi_output_sequences;
            public string m_extended_latin_output;

            public ConversionTable(string[] input_sequences, int[] desi_output_sequences,
                string extended_latin_output)
            {
                m_input_sequences = input_sequences;
                m_desi_output_sequences = desi_output_sequences;
                m_extended_latin_output = extended_latin_output;
            }
        }

        private Dictionary<int, string> LATINTABLE = new Dictionary<int,string>();
        private Dictionary<int, string> LATINEXTABLE = new Dictionary<int,string>();

        /// <summary>
        /// Loads user specific transliteration map
        /// from the user's Roaming Application Data folder
        /// - Users\&lt;username&gt;\AppData\Roaming\GumPad\.gumpad.map on Vista. 
        /// Initializes user specific transliteration map in the Application Data
        /// folder from the built-in map if user specific file
        /// does not exist.
        /// </summary>
        public Transliterator()
        {
            COMMONMAP = new Dictionary<string, ConversionTable>();
            DEVANAGARIMAP = new Dictionary<string, ConversionTable>();
            BENGALIMAP = new Dictionary<string, ConversionTable>();
            GUJARATIMAP = new Dictionary<string, ConversionTable>();
            GURMUKHIMAP = new Dictionary<string, ConversionTable>();
            KANNADAMAP = new Dictionary<string, ConversionTable>();
            ORIYAMAP = new Dictionary<string, ConversionTable>();
            TAMILMAP = new Dictionary<string, ConversionTable>();
            TELUGUMAP = new Dictionary<string, ConversionTable>();
            MALAYALAMMAP = new Dictionary<string, ConversionTable>();
            try
            {
                loadConversionMap();
            }
            catch (Exception e)
            {
                MessageBox.Show("There are duplicate patterns in your conversion map. "
                    + "Check and fix errors from the Preferences->Map menu.\n\n"
                    + e.Message);
            }
            
        }

        /// <summary>
        /// Reloads conversion map from the user's
        /// Roaming Application Data folder
        /// </summary>
        public void ReloadConversionMap()
        {
            LATINTABLE.Clear();
            LATINEXTABLE.Clear();

            try
            {
                loadConversionMap();
                loadPatternMaps();
                sortPatternMaps();
            }
            catch (Exception e)
            {
                MessageBox.Show("There are duplicate patterns in your conversion map. "
                    + "Check and fix errors from the Preferences->Map menu.\n\n"
                    + e.Message);
            }
        }

        private void loadConversionMap()
        {
            string schemeName;
            string contributorName;
            string appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder usermapdir = new StringBuilder(Path.Combine(appdatadir,"GumPad"));
            if (!Directory.Exists(usermapdir.ToString()))
            {
                Directory.CreateDirectory(usermapdir.ToString());
            }
            string usermapfile = Path.Combine(usermapdir.ToString(), ".gumpad.map");
            if (!File.Exists(usermapfile))
            {
                StreamWriter w = new StreamWriter(usermapfile);
                w.Write(GumLib.gumpad, Encoding.UTF8);
                w.Close();
            }
            StreamReader r = new StreamReader(usermapfile);
            TransliterationMap.loadMap(this, r, false, out schemeName,
                out contributorName);
            r.Close();
        }

        private void sortPatternMaps()
        {
            // bubble sort patternmap so that the longest patterns
            // bubble up to the top 
            ptab = new Regex[patternMap.Keys.Count];
            patternMap.Keys.CopyTo(ptab, 0);
            for (int i = 0; i < ptab.Length - 1; i++)
            {
                for (int j = 0; j < ptab.Length - 1 - i; j++)
                {
                    if (ptab[j + 1].ToString().Length > ptab[j].ToString().Length)
                    {
                        Regex tmp = ptab[j];
                        ptab[j] = ptab[j + 1];
                        ptab[j + 1] = tmp;
                        maxPatternLength = tmp.ToString().Length;
                    }
                }
            }

            // bubble sort latinEx patternmap so that the longest patterns
            // bubble up to the top 
            ptabForLatinEx = new Regex[patternMapForLatinEx.Keys.Count];
            patternMapForLatinEx.Keys.CopyTo(ptabForLatinEx, 0);
            for (int i = 0; i < ptabForLatinEx.Length - 1; i++)
            {
                for (int j = 0; j < ptabForLatinEx.Length - 1 - i; j++)
                {
                    if (ptabForLatinEx[j + 1].ToString().Length > ptabForLatinEx[j].ToString().Length)
                    {
                        Regex tmp = ptabForLatinEx[j];
                        ptabForLatinEx[j] = ptabForLatinEx[j + 1];
                        ptabForLatinEx[j + 1] = tmp;
                        maxPatternLength = tmp.ToString().Length;
                    }
                }
            }
        }

        private void loadPatternMaps()
        {
            patternMap = new Dictionary<Regex, Char[]>();
            patternMapForLatinEx = new Dictionary<Regex, Char[]>();

            loadPatternMaps(COMMONMAP);
            if (lang.Equals(DEVANAGARI))
            {
                loadPatternMaps(DEVANAGARIMAP);
            }
            else if (lang.Equals(TELUGU))
            {
                loadPatternMaps(TELUGUMAP);
            }
            else if (lang.Equals(TAMIL))
            {
                loadPatternMaps(TAMILMAP);
            }
            else if (lang.Equals(KANNADA))
            {
                loadPatternMaps(KANNADAMAP);
            }
            else if (lang.Equals(MALAYALAM))
            {
                loadPatternMaps(MALAYALAMMAP);
            }
            else if (lang.Equals(GUJARATI))
            {
                loadPatternMaps(GUJARATIMAP);
            }
            else if (lang.Equals(ORIYA))
            {
                loadPatternMaps(ORIYAMAP);
            }
            else if (lang.Equals(GURMUKHI))
            {
                loadPatternMaps(GURMUKHIMAP);
            }
            else if (lang.Equals(BENGALI))
            {
                loadPatternMaps(BENGALIMAP);
            }
        }

        private void loadPatternMaps(Dictionary<string, ConversionTable> letterMap)
        {

            foreach (string key in letterMap.Keys)
            {
                ConversionTable tout = letterMap[key];
                int[] c = tout.m_desi_output_sequences;
                if (c.Length != 0)
                {
                    if (!LATINTABLE.ContainsKey((int)c[0]))
                    {
                        LATINTABLE.Add(c[0], tout.m_input_sequences[0]);
                    }
                    if (!LATINEXTABLE.ContainsKey((int)c[0]))
                    {
                        if (!tout.m_extended_latin_output.Equals(""))
                        {
                            LATINEXTABLE.Add(c[0], tout.m_extended_latin_output);
                        }
                        else
                        {
                            LATINEXTABLE.Add(c[0], tout.m_input_sequences[0]);
                        }
                    }
                }
                char[] chars = new char[c.Length];
                for (int i = 0; i < c.Length; i++)
                {
                    chars[i] = (char)c[i];
                }
                foreach (string seq in tout.m_input_sequences)
                {
                    Regex pat = new Regex(Regex.Escape(seq), RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    patternMap.Add(pat, chars);
                }
                if (!tout.m_extended_latin_output.Equals(""))
                {
                    Regex pat = new Regex(Regex.Escape(tout.m_extended_latin_output), RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    patternMapForLatinEx.Add(pat, chars);
                }
            }

            LATINTABLE.TryGetValue(0x05, out akaar);

            // independent to dependent vowel map
            vowelMap = new Dictionary<Char, Char>();
            vowelMap.Add((char)(unicodeRangeStart + 0x06), (char)(unicodeRangeStart + 0x3E)); //aa
            vowelMap.Add((char)(unicodeRangeStart + 0x07), (char)(unicodeRangeStart + 0x3F)); //e
            vowelMap.Add((char)(unicodeRangeStart + 0x08), (char)(unicodeRangeStart + 0x40)); //ee
            vowelMap.Add((char)(unicodeRangeStart + 0x09), (char)(unicodeRangeStart + 0x41)); //u
            vowelMap.Add((char)(unicodeRangeStart + 0x0A), (char)(unicodeRangeStart + 0x42)); //uu
            vowelMap.Add((char)(unicodeRangeStart + 0x0B), (char)(unicodeRangeStart + 0x43)); //RR^i
            vowelMap.Add((char)(unicodeRangeStart + 0x60), (char)(unicodeRangeStart + 0x44)); //RR^I
            vowelMap.Add((char)(unicodeRangeStart + 0x0C), (char)(unicodeRangeStart + 0x62)); //LL^i
            vowelMap.Add((char)(unicodeRangeStart + 0x61), (char)(unicodeRangeStart + 0x63)); //LL^I
            //vowelMap.Add((char)(unicodeRangeStart + 0x0D), (char)(unicodeRangeStart + 0x46)); //candra^e
            vowelMap.Add((char)(unicodeRangeStart + 0x0E), (char)(unicodeRangeStart + 0x46));
            vowelMap.Add((char)(unicodeRangeStart + 0x0F), (char)(unicodeRangeStart + 0x47));
            vowelMap.Add((char)(unicodeRangeStart + 0x10), (char)(unicodeRangeStart + 0x48));
            //vowelMap.Add((char)(unicodeRangeStart + 0x11), (char)(unicodeRangeStart + 0x4A)); //candra^o
            vowelMap.Add((char)(unicodeRangeStart + 0x12), (char)(unicodeRangeStart + 0x4A));
            vowelMap.Add((char)(unicodeRangeStart + 0x13), (char)(unicodeRangeStart + 0x4B));
            vowelMap.Add((char)(unicodeRangeStart + 0x14), (char)(unicodeRangeStart + 0x4C));

        }


        /// <summary>
        /// 
        /// </summary>
        public bool UseLatinExMapForConversion
        {
            get
            {
                return useLatinExMapForConversion;
            }
            set
            {
                useLatinExMapForConversion = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the output language
        /// for transliterated output
        /// </summary>
        public String Language
        {
            set
            {
                lang = value;
                setLang(lang);
                try
                {
                    loadPatternMaps();
                    sortPatternMaps();
                }
                catch (Exception e)
                {
                    MessageBox.Show("There are duplicate patterns in your conversion map. "
                    + "Check and fix errors from the Preferences->Map menu.\n\n"
                    + e.Message);
                }
            }
        }

        /// <summary>
        /// Transliterates a sequence of input
        /// characters containing no white space.
        /// Referenced from <see cref="GumPadTextBox"/>
        /// when convert as you type flag is set.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="asEntityCodeFlag"></param>
        /// <returns></returns>
        public String TransliterateWord(String word, bool asEntityCodeFlag)
        {
            List<Char> list = new List<Char>();
            StringBuilder result = new StringBuilder();
            processWord(word, list, asEntityCodeFlag);
            printWord(list, new StringWriter(result), asEntityCodeFlag);
            return result.ToString();
        }

        /// <summary>
        /// Transliterate input sequences into output sequences
        /// according to the mappings defined in the currently
        /// loaded transliteration map. Uses currently set
        /// <see cref="Language"/> to determine output sequence.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <param name="asEntityCodeFlag"></param>
        public void Transliterate(TextReader input, TextWriter result, bool asEntityCodeFlag)
        {
            StringBuilder word = new StringBuilder();

            while (input.Peek() != -1)
            {
                char c = (char) input.Read();
                if (Char.IsSeparator(c) || Char.IsWhiteSpace(c))
                {
                    if (word.Length > 0)
                    {
                        List<Char> list = new List<Char>();
                        processWord(word.ToString(), list, asEntityCodeFlag);
                        printWord(list, result, asEntityCodeFlag);
                    }
                    word.Length = 0;
                    result.Write(c);
                    continue;
                }
                word.Append(c);
            }
            if (word.Length > 0)
            {
                List<Char> list = new List<Char>();
                processWord(word.ToString(), list, asEntityCodeFlag);
                printWord(list, result, asEntityCodeFlag);
            }
        }

        private void printWord(List<Char> list, TextWriter result,
            bool asEntityCodeFlag)
        {
            foreach (Char c in list)
            {
                if (asEntityCodeFlag)
                {
                    result.Write(getEntityCode(c));
                }
                else
                {
                    result.Write(c.ToString());
                }
            }
        }

        private string latinTrans(char c)
        {
            string result;

            int hiBytes;
            int loBytes;

            splitBytes(c, out hiBytes, out loBytes);

            if (LATINTABLE.TryGetValue((int)c, out result))
            {
                return result;
            }
            else if (LATINTABLE.TryGetValue(loBytes, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private void splitBytes(int c, out int hiBytes, out int loBytes)
        {
            if (c >= 0x0900 && c <= 0x097F)
            {
                hiBytes = 0x0900;
                loBytes = c - 0x0900;
            }
            else if (c >= 0x0980 && c <= 0x09FF)
            {
                hiBytes = 0x0980;
                loBytes = c - 0x0980;
            }
            else if (c >= 0x0A00 && c <= 0x0A7F)
            {
                hiBytes = 0x0A00;
                loBytes = c - 0x0A00;
            }
            else if (c >= 0x0A80 && c <= 0x0AFF)
            {
                hiBytes = 0x0A80;
                loBytes = c - 0x0A80;
            }
            else if (c >= 0x0B00 && c <= 0x0B7F)
            {
                hiBytes = 0x0B00;
                loBytes = c - 0x0B00;
            }
            else if (c >= 0x0B80 && c <= 0x0BFF)
            {
                hiBytes = 0x0B80;
                loBytes = c - 0x0B80;
            }
            else if (c >= 0x0C00 && c <= 0x0C7F)
            {
                hiBytes = 0x0C00;
                loBytes = c - 0x0C00;
            }
            else if (c >= 0x0C80 && c <= 0x0CFF)
            {
                hiBytes = 0x0C80;
                loBytes = c - 0x0C80;
            }
            else if (c >= 0x0D00 && c <= 0x0D7F)
            {
                hiBytes = 0x0D00;
                loBytes = c - 0x0D00;
            }
            else
            {
                throw new Exception("what language is this? how could we get here?? :-)");
            }
        }

        private string desiTrans(char c)
        {
            int hiBytes;
            int loBytes;

            splitBytes(c, out hiBytes, out loBytes);

            if (hiBytes == unicodeRangeStart)
            {
                return c.ToString();
            }

            if (isSwaraOrSpecialAkshara(c) || loBytes > 0x6F)
            {
                // not transliteratable inter-desi-lingua range
                return c.ToString();
            }

            return ((char)(unicodeRangeStart + loBytes)).ToString();
        }

        private bool convertToLatin(String word, List<Char> list, bool asEntityCodeFlag)
        {
            bool addAkaar = false;
            while (word.Length > 0)
            {
                if (word[0] > 0xFF)
                {
                    // skip characters that are not in ascii range
                    // and translate between indian languages
                    int c = (int)word[0];
                    string s;
                    int hiBytes;
                    int loBytes;
                    splitBytes(c, out hiBytes, out loBytes);
                    string res;
                    if (LATINTABLE.TryGetValue(c, out res))
                    {
                        if (addAkaar)
                        {
                            list.AddRange(akaar.ToCharArray());
                        }
                        if (isSwaraOrSpecialAkshara((char)c))
                        {
                            addAkaar = false;
                        }
                        else
                        {
                            addAkaar = true;
                        }
                        list.AddRange(res.ToCharArray());
                        word = word.Substring(1);
                        continue;
                    }
                    c = loBytes;
                    if (loBytes == 0x4D) // skip halant
                    {
                        addAkaar = false;
                        word = word.Substring(1);
                        continue;
                    }
                    if (vowelMap.ContainsValue((char)(unicodeRangeStart + loBytes)))
                    {
                        // map c to independent vowel
                        foreach (char vowel in vowelMap.Keys)
                        {
                            if ((char)(unicodeRangeStart + loBytes) == vowelMap[vowel])
                            {
                                c = ((int)vowel) - unicodeRangeStart;
                                break;
                            }
                        }
                    }
                    if ((c > 0x05 && c < 0x15) // is om
                        || (c > 0x3B && c < 0x70))
                    {
                        addAkaar = false;
                    }
                    else
                    {
                        if (addAkaar)
                        {
                            list.AddRange(akaar.ToCharArray());
                        }
                        if (c < 0x05)
                        {
                            addAkaar = false;
                        }
                        else
                        {
                            addAkaar = true;
                        }
                    }
                    s = latinTrans((char)(hiBytes + c));
                    if (s != null)
                    {
                        list.AddRange(s.ToCharArray());
                    }
                }
                else
                {
                    list.Add(word[0]);
                }
                word = word.Substring(1);
                continue;

            }
            if (addAkaar)
            {
                list.AddRange(akaar.ToCharArray());
            }
            return true;
        }

        private string latinExTrans(char c)
        {
            string result;

            int hiBytes;
            int loBytes;

            splitBytes(c, out hiBytes, out loBytes);

            if (LATINEXTABLE.TryGetValue((int)c, out result))
            {
                return result;
            }
            else if (LATINEXTABLE.TryGetValue(loBytes, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private bool convertToLatinEx(String word, List<Char> list, bool asEntityCodeFlag)
        {
            bool addAkaar = false;
            while (word.Length > 0)
            {
                if (word[0] > 0xFF)
                {
                    // skip characters that are not in ascii range
                    // and translate between indian languages
                    int c = (int)word[0];
                    string s;
                    int hiBytes;
                    int loBytes;
                    splitBytes(c, out hiBytes, out loBytes);
                    string res;
                    if (LATINEXTABLE.TryGetValue(c, out res))
                    {
                        if (addAkaar)
                        {
                            list.AddRange(akaar.ToCharArray());
                        }
                        if (isSwaraOrSpecialAkshara((char) c))
                        {
                            addAkaar = false;
                        }
                        else
                        {
                            addAkaar = true;
                        }
                        list.AddRange(res.ToCharArray());
                        word = word.Substring(1);
                        continue;
                    }
                    c = loBytes;
                    if (loBytes == 0x4D) // skip halant
                    {
                        addAkaar = false;
                        word = word.Substring(1);
                        continue;
                    }
                    if (vowelMap.ContainsValue((char)(unicodeRangeStart + loBytes)))
                    {
                        // map c to independent vowel
                        foreach (char vowel in vowelMap.Keys)
                        {
                            if ((char)(unicodeRangeStart + loBytes) == vowelMap[vowel])
                            {
                                c = ((int)vowel) - unicodeRangeStart;
                                break;
                            }
                        }
                    }
                    if ((c > 0x05 && c < 0x15) // is om
                        || (c > 0x3B && c < 0x70))
                    {
                        addAkaar = false;
                    }
                    else
                    {
                        if (addAkaar)
                        {
                            list.AddRange(akaar.ToCharArray());
                        }
                        if (c < 0x05)
                        {
                            addAkaar = false;
                        }
                        else
                        {
                            addAkaar = true;
                        }
                    }
                    s = latinExTrans((char)(hiBytes + c));
                    if (s != null)
                    {
                        list.AddRange(s.ToCharArray());
                    }
                }
                else
                {
                    list.Add(word[0]);
                }
                word = word.Substring(1);
                continue;

            }
            if (addAkaar)
            {
                list.AddRange(akaar.ToCharArray());
            }
            return true;
        }


        private bool processWord(String word, List<Char> list, bool asEntityCodeFlag)
        {
            if (lang.Equals(LATIN))
            {
                return convertToLatin(word, list, asEntityCodeFlag);
            }
            else if (lang.Equals(LATINEX))
            {
                return convertToLatinEx(word, list, asEntityCodeFlag);
            }

            bool atWordStart = true;
            char c = (char)0;
            char prevChar = (char)0;

            while (word.Length > 0)
            {
                if (word[0] > 0x8FF && word[0] < 0xD80)
                {
                    // skip characters that are not in ascii range
                    // and translate between indian languages
                    string s = desiTrans(word[0]);
                    if (s != null)
                    {
                        list.AddRange(s.ToCharArray());
                    }
                    word = word.Substring(1);
                    continue;
                }
                bool isConjunct = false;
                bool foundMatch = false;
                int sublen = 0;
                
                Regex[] regexTab;
                if (useLatinExMapForConversion)
                {
                    regexTab = ptabForLatinEx;
                }
                else
                {
                    regexTab = ptab;
                }

                for (int j = 0; j < regexTab.Length; j++)
                {
                    Regex p = regexTab[j];
                    MatchCollection matches = p.Matches(word);
                    bool isLookingAt = false;
                    
                    foreach (Match match in matches)
                    {
                        int index = match.Index;
                        sublen = match.Length;
                        if (index == 0)
                        {
                            isLookingAt = true;
                            break;
                        }
                    }
                    if (isLookingAt)
                    {
                        foundMatch = true;
                        char [] chars;
                        if (useLatinExMapForConversion)
                        {
                            chars = patternMapForLatinEx[p];
                        }
                        else
                        {
                            chars = patternMap[p];
                        }

                        c = chars[chars.Length - 1];
                        if (c < 0x80)
                        {
                            c = (char)(unicodeRangeStart + c);
                        }
			            if (chars.Length > 1)
			            {
				            // is a conjunct
                            isConjunct = true;
                            for (int i = 0; i < chars.Length; i++)
                            {
                                if (chars[i] < 0x80)
                                {
                                    chars[i] = (char) (unicodeRangeStart + chars[i]);
                                }
                                list.Add(chars[i]);
                            }
                            word = word.Substring(sublen); //p.ToString().Length);
                            break;
                        }
                        //patternMap.TryGetValue(p, out c);
                        if (vowelMap.ContainsKey(c))
                        {
                            if (!atWordStart)
                            {
                                c = vowelMap[c]; //.TryGetValue(c, out c);
                            }
                        }
                        word = word.Substring(sublen);
                        break;
                    }
                }
                if (!foundMatch)
                {
                    // not transliteratable
                    // print start char and continue
                    atWordStart = true;
                    c = word[0];
                    word = word.Substring(1);
                }
                if (isConsonant(prevChar) && isConsonant(c))
                {
                    // inject halant to produce a consonant conjunct
                    list.Add((char)(unicodeRangeStart + 0x4D));
                }
                bool skipChar = false;
                if (isConjunct)
                {
                    skipChar = true;
                }
                if (isConsonant(prevChar) && (c == unicodeRangeStart + 0x05))
                {
                    // skip print if a consonant is followed by 'a'
                    skipChar = true;
                }
                prevChar = c;
                if (!skipChar)
                {
                    list.Add(c);
                }
                atWordStart = false;
            }
            if (isConsonant(prevChar))
            {
                // add halant if the last char was a consonant
                list.Add((char)(unicodeRangeStart + 0x4D));
            }
            return isConsonant(prevChar);
        }

        private bool isConsonant(char c)
        {
            if ((c >= unicodeRangeStart + 0x15) && (c <= unicodeRangeStart + 0x39))
            {
                return true;
            }
            return false;
        }

        private bool isSwaraOrSpecialAkshara(char c)
        {
            if (c == '\u08A4' || c == '\u0950' || c == '\u0951'
                || c == '\u0952' || c == '\u0953'
                || c == '\u0964' || c == '\u0965'
                || c == '\u0320' || c == '\u030D' || c == '\u030E'
                || c == '\u0305' || c == '\u0329' || c == '\u0348')
            {
                return true;
            }
            return false;
        }

        private String getEntityCode(char c)
        {

            return ("&#x" + string.Format("{0:X}", (int)c) + ";");
        }

        /// <summary>
        /// Gets language that the transliterator
        /// is currently set to.
        /// </summary>
        /// <returns></returns>
        private String getLang()
        {
            return lang;
        }

        /// <summary>
        /// Sets language that transliterator
        /// should use to output transliterated sequences
        /// </summary>
        /// <param name="lang"></param>
        private void setLang(String lang)
        {
            this.lang = lang;
            for (int k = 0; k <= LANGMAP.GetUpperBound(0); k++)
            {
                if (LANGMAP[k, 0].Equals(lang, StringComparison.OrdinalIgnoreCase))
                {
                    unicodeRangeStart = int.Parse(LANGMAP[k, 1], System.Globalization.NumberStyles.HexNumber); // int.Parse(LANG_MAP[k, 1]);
                    break;
                }
            }
        }

        /// <summary>
        /// Sets transliterator mappings
        /// <see cref="TransliterationMap"/>
        /// </summary>
        /// <param name="aksharaMappings"></param>
        /// <returns></returns>
        public bool setAksharaMappings(AksharaMapping[] aksharaMappings)
        {
            COMMONMAP.Clear();
            DEVANAGARIMAP.Clear();
            BENGALIMAP.Clear();
            GUJARATIMAP.Clear();
            GURMUKHIMAP.Clear();
            KANNADAMAP.Clear();
            ORIYAMAP.Clear();
            TAMILMAP.Clear();
            TELUGUMAP.Clear();
            MALAYALAMMAP.Clear();

            foreach (AksharaMapping amap in aksharaMappings)
            {
                switch (amap.Language)
                {
                    case "*":
                        {
                            COMMONMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "hi":
                        {
                            DEVANAGARIMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "be":
                        {
                            BENGALIMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "gu":
                        {
                            GUJARATIMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "gr":
                        {
                            GURMUKHIMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "ka":
                        {
                            KANNADAMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "or":
                        {
                            ORIYAMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "ta":
                        {
                            TAMILMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "te":
                        {
                            TELUGUMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    case "ma":
                        {
                            MALAYALAMMAP[amap.CharacterName] = new ConversionTable(amap.InputSequences, getUnicodeSequenceAsIntArray(amap.UnicodeSequence), amap.LatinEx);
                            break;
                        }
                    default:
                        {
                            Console.Error.WriteLine("ignoring unrecognized language mapping for language code: " + amap.Language);
                            break;
                        }
                }
            }

            return true;
        }

        private static int[] getUnicodeSequenceAsIntArray(string unicodeSequence)
        {
            ArrayList charList = new ArrayList();
            MatchCollection u4 = Regex.Matches(unicodeSequence, @"\\u([0-9a-fA-F]{4})");
            MatchCollection u8 = Regex.Matches(unicodeSequence, @"\\U([0-9a-fA-F]{8})");
            for (int i = 0; i < u4.Count; i++)
            {
                charList.Add(Int32.Parse(u4[i].Groups[1].Value, System.Globalization.NumberStyles.HexNumber));
            }
            for (int i = 0; i < u8.Count; i++)
            {
                charList.Add(Int32.Parse(u8[i].Groups[1].Value, System.Globalization.NumberStyles.HexNumber));
            }
            return (int[])charList.ToArray(typeof(int));
        }

        private string getUnicodeSequence(int[] chars)
        {
            StringBuilder unicodeSequence = new StringBuilder();
            foreach (int i in chars)
            {
                if (i < 0x10000)
                {
                    unicodeSequence.Append(@"\u");
                    unicodeSequence.Append(string.Format("{0:X4}", i));
                }
                else
                {
                    unicodeSequence.Append(@"\U");
                    unicodeSequence.Append(string.Format("{0:X8}", i));
                }
            }
            return unicodeSequence.ToString();
        }

        /// <summary>
        /// Gets transliterator mappings currently being used
        /// by the transliterator
        /// <see cref="TransliterationMap"/>
        /// </summary>
        /// <returns></returns>
        public List<AksharaMapping> getAksharaMappings()
        {
            int mappingCount = COMMONMAP.Count + DEVANAGARIMAP.Count
                + BENGALIMAP.Count + GUJARATIMAP.Count
                + GURMUKHIMAP.Count + ORIYAMAP.Count
                + TELUGUMAP.Count + TAMILMAP.Count
                + KANNADAMAP.Count + MALAYALAMMAP.Count;

            List<AksharaMapping> aksharaMappings = new List<AksharaMapping>(mappingCount);

            foreach (string character_name in COMMONMAP.Keys)
            {
                ConversionTable tout = COMMONMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "*"));
            }

            foreach (string character_name in DEVANAGARIMAP.Keys)
            {
                ConversionTable tout = DEVANAGARIMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "hi"));
            }

            foreach (string character_name in BENGALIMAP.Keys)
            {
                ConversionTable tout = BENGALIMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "be"));
            }

            foreach (string character_name in GUJARATIMAP.Keys)
            {
                ConversionTable tout = GUJARATIMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "gu"));
            }

            foreach (string character_name in GURMUKHIMAP.Keys)
            {
                ConversionTable tout = GURMUKHIMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "gr"));
            }

            foreach (string character_name in KANNADAMAP.Keys)
            {
                ConversionTable tout = KANNADAMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "ka"));
            }

            foreach (string character_name in ORIYAMAP.Keys)
            {
                ConversionTable tout = ORIYAMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "or"));
            }

            foreach (string character_name in TAMILMAP.Keys)
            {
                ConversionTable tout = TAMILMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "ta"));
            }

            foreach (string character_name in TELUGUMAP.Keys)
            {
                ConversionTable tout = TELUGUMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "te"));
            }

            foreach (string character_name in MALAYALAMMAP.Keys)
            {
                ConversionTable tout = MALAYALAMMAP[character_name];
                aksharaMappings.Add(new AksharaMapping(character_name,
                    tout.m_input_sequences, tout.m_extended_latin_output,
                    getUnicodeSequence(tout.m_desi_output_sequences), "ma"));
            }

            return aksharaMappings;
        }

        /// <summary>
        /// @TODO
        /// </summary>
        /// <returns></returns>
        public string printLetterMapUsingXSLT()
        {
            string appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder usermapdir = new StringBuilder(Path.Combine(appdatadir, "GumPad"));
            string usermapfile = Path.Combine(usermapdir.ToString(), ".gumpad.map");

            XmlTextReader xmlreader = new XmlTextReader(new StreamReader(usermapfile));
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load("gumpad.xsl");

            StringWriter html = new StringWriter();
            XPathDocument xdoc = new XPathDocument(xmlreader);
            XmlTextWriter writer = new XmlTextWriter(html);
            writer.Formatting = Formatting.Indented;

            xslt.Transform(xdoc, new XsltArgumentList(), html);
            return html.ToString();
        }

        /// <summary>
        /// Prints HTML formatted map
        /// of currently loaded transliteration map
        /// </summary>
        /// <returns></returns>
        public string printLetterMap()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append("<html><head></head><body style=\"font-size:large\"><table width=\"100%\" border=\"1px\">");
            buff.Append("<tr><td align=\"center\">Sequence</td>");
            for (int k = 0; k <= LANGMAP.GetUpperBound(0); k++)
            {
                buff.Append("<td align=\"center\">");
                buff.Append(LANGMAP[k, 0]);
                buff.Append("</td>");
            }
            buff.Append("</tr>");

            printLangMap(COMMONMAP, buff);
            printLangMap(DEVANAGARIMAP, buff);
            printLangMap(BENGALIMAP, buff);
            printLangMap(GUJARATIMAP, buff);
            printLangMap(GURMUKHIMAP, buff);
            printLangMap(KANNADAMAP, buff);
            printLangMap(ORIYAMAP, buff);
            printLangMap(TAMILMAP, buff);
            printLangMap(TELUGUMAP, buff);
            printLangMap(MALAYALAMMAP, buff);

            buff.Append("</table></body></html>");
            return buff.ToString();
        }

        private string getDesiCharByName(Dictionary<string, ConversionTable> letterMap,
            string charname)
        {
            StringBuilder buff = new StringBuilder();
            if (letterMap.ContainsKey(charname))
            {
                int[] c = letterMap[charname].m_desi_output_sequences;
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] < 0x80)
                    {
                        buff.Append((char)(c[i] + unicodeRangeStart));
                    }
                    else
                    {
                        buff.Append((char)c[i]);
                    }
                }
            }
            else if (COMMONMAP.ContainsKey(charname))
            {
                int[] c = COMMONMAP[charname].m_desi_output_sequences;
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] < 0x80)
                    {
                        buff.Append((char)(c[i] + unicodeRangeStart));
                    }
                    else
                    {
                        buff.Append((char)c[i]);
                    }
                }
            }
            else
            {
                buff.Append("&nbsp;");
            }
            return buff.ToString();
        }

        private void printLangMap(Dictionary<string, ConversionTable> letterMap, StringBuilder buff)
        {
            foreach (string key in letterMap.Keys)
            {
                buff.Append("<tr><td align=\"center\">");
                int[] c = letterMap[key].m_desi_output_sequences;
                string latinex = letterMap[key].m_extended_latin_output;
                string[] latin = letterMap[key].m_input_sequences;
                StringBuilder inputSequence = new StringBuilder();
                foreach (string seq in letterMap[key].m_input_sequences)
                {
                    if (inputSequence.Length > 0)
                    {
                        inputSequence.Append(",");
                    }
                    inputSequence.Append(seq);
                }
                buff.Append(inputSequence);
                buff.Append("</td>");
                for (int k = 0; k <= LANGMAP.GetUpperBound(0); k++)
                {
                    // @TODO @FIXME conjuncts
                    int unicodeRangeStart = int.Parse(LANGMAP[k, 1],
                        System.Globalization.NumberStyles.HexNumber);
                    buff.Append("<td align=\"center\">");
                    switch (unicodeRangeStart)
                    {
                        case 0:
                            {
                                buff.Append(latin[0]);
                                break;
                            }
                        case 0x80:
                            {
                                buff.Append(latinex);
                                break;
                            }
                        case 0x900:
                            {
                                buff.Append(getDesiCharByName(DEVANAGARIMAP,key));
                                break;
                            }
                        case 0x0980:
                            {
                                buff.Append(getDesiCharByName(BENGALIMAP,key));
                                break;
                            }
                        case 0xA00:
                            {
                                buff.Append(getDesiCharByName(GURMUKHIMAP,key));
                                break;
                            }
                        case 0xA80:
                            {
                                buff.Append(getDesiCharByName(GUJARATIMAP,key));
                                break;
                            }
                        case 0xB00:
                            {
                                buff.Append(getDesiCharByName(ORIYAMAP,key));
                                break;
                            }
                        case 0xB80:
                            {
                                buff.Append(getDesiCharByName(TAMILMAP,key));
                                break;
                            }
                        case 0xC00:
                            {
                                buff.Append(getDesiCharByName(TELUGUMAP,key));
                                break;
                            }
                        case 0xC80:
                            {
                                buff.Append(getDesiCharByName(KANNADAMAP,key));
                                break;
                            }
                        case 0xD00:
                            {
                                buff.Append(getDesiCharByName(MALAYALAMMAP,key));
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    buff.Append("</td>");
                }
                buff.Append("</tr>");
            }
        }
    }
}
