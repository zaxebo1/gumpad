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
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace GumLib
{
    /// <summary>
    /// XML Serializable instance of a
    /// transliteration map
    /// </summary>
    [XmlRoot("TransliterationMap")]
    public class TransliterationMap
    {
        private ArrayList m_conversionMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public TransliterationMap()
        {
            m_conversionMap = new ArrayList();
        }

        /// <summary>
        /// XML schema revision
        /// </summary>
        [XmlAttribute("schema-revision")]
        public string schemaRevision = "2.0";

        /// <summary>
        /// XML transliteration scheme name.
        /// e.g "GumPad Built-in" or "ITRANS"
        /// meant only for documentation purposes
        /// in the XML file. This has no functional
        /// impact on transliteration
        /// </summary>
        [XmlAttribute("transliteration-scheme-name")]
        public string schemeName;

        /// <summary>
        /// Author of the translitration map XML file
        /// meant only for documentation purposes
        /// in the XML file. This has no functional
        /// impact on transliteration
        /// </summary>
        [XmlAttribute("contributor-name")]
        public string contributorName;

        /// <summary>
        /// XML node that defines the mapping characteristics of an akshara
        /// </summary>
        [XmlElement("mapping")]
        public AksharaMapping[] Mappings
        {
            get
            {
                AksharaMapping[] mappings = new AksharaMapping[m_conversionMap.Count];
                m_conversionMap.CopyTo(mappings);
                Array.Sort(mappings, new MappingComparer());
                return mappings;
            }
            set
            {
                if (value == null) return;
                AksharaMapping[] mappings = (AksharaMapping[])value;
                m_conversionMap.Clear();
                foreach (AksharaMapping mapping in mappings)
                    m_conversionMap.Add(mapping);
            }
        }

        /// <summary>
        /// Adds a mapping element to the map
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public int AddItem(AksharaMapping mapping)
        {
            return m_conversionMap.Add(mapping);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transliterator"></param>
        /// <param name="aksharaMappings"></param>
        /// <param name="schemeName"></param>
        /// <param name="contributorName"></param>
        public static void saveUserMapFile(Transliterator  transliterator,
            AksharaMapping[] aksharaMappings, bool skipValidation, string schemeName, 
            string contributorName)
        {
            string appdatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            StringBuilder usermapdir = new StringBuilder(Path.Combine(appdatadir, "GumPad"));
            if (!Directory.Exists(usermapdir.ToString()))
            {
                Directory.CreateDirectory(usermapdir.ToString());
            }
            string usermapfile = Path.Combine(usermapdir.ToString(), ".gumpad.map");

            if (!skipValidation)
            {
                validateMappings(aksharaMappings);
            }
            transliterator.setAksharaMappings(aksharaMappings);
            saveMap(transliterator, usermapfile, schemeName, contributorName);
        }

        /// <summary>
        /// Saves current map to a file
        /// </summary>
        /// <param name="transliterator"></param>
        /// <param name="mapFile"></param>
        /// <param name="schemeName"></param>
        /// <param name="contributorName"></param>
        /// <returns></returns>
        public static bool saveMap(Transliterator transliterator,
            string mapFile,
            string schemeName, 
            string contributorName)
        {
            //Map tmap = Map.Default;
            TransliterationMap map = new TransliterationMap();
            List<AksharaMapping> aksharaMappings = transliterator.getAksharaMappings();
            foreach (AksharaMapping a in aksharaMappings)
            {
                map.AddItem(a);
            }
            map.schemeName = schemeName;
            map.contributorName = contributorName;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TransliterationMap));
                StreamWriter writer = new StreamWriter(mapFile, false, Encoding.UTF8);
                serializer.Serialize(writer, map);
                writer.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads a transliteration map from
        /// an XML file replacing map currently
        /// being used by the transliterator
        /// </summary>
        /// <param name="transliterator"></param>
        /// <param name="mapFileStream"></param>
        /// <param name="schemeName"></param>
        /// <param name="contributorName"></param>
        /// <returns></returns>
        public static bool loadMap(Transliterator transliterator,
            TextReader mapFileStream,
            bool skipValidation,
            out string schemeName, out string contributorName)
        {
            AksharaMapping[] mappings;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TransliterationMap));
                TransliterationMap map = (TransliterationMap)serializer.Deserialize(mapFileStream);
                mapFileStream.Close();
                if (map.schemaRevision.Equals("1.0"))
                {
                    // read built-in 2.0 map first and set these accordingly
                    string builtInScheme;
                    string builtInContributor;
                    TransliterationMap.loadMap(transliterator,
                        new StringReader(GumLib.gumpad), false,
                        out builtInScheme, out builtInContributor);
                    List<AksharaMapping> builtInMappings = transliterator.getAksharaMappings();
                    List<AksharaMapping> v10mappings = new List<AksharaMapping>();
                    foreach (AksharaMapping m in map.Mappings)
                    {
                        if ((m.m_character_v10 != null)
                            && (m.m_sequence_v10 != null)
                            && (m.m_character_v10.Length>6))
                        {
                            string key = m.m_character_v10.Substring(6);
                            foreach (AksharaMapping bmap in builtInMappings)
                            {
                                if (bmap.CharacterName.Equals(key))
                                {
                                    m.CharacterName = bmap.CharacterName;
                                    m.InputSequences = m.m_sequence_v10.Split(',');
                                    m.UnicodeSequence = bmap.UnicodeSequence;
                                    m.LatinEx = bmap.LatinEx;
                                    m.Language = bmap.Language;
                                    m.m_character_v10 = null;
                                    m.m_sequence_v10 = null;
                                    v10mappings.Add(m);
                                    break;
                                }
                            }
                        }
                    }
                    map.Mappings = v10mappings.ToArray();
                }
                schemeName = map.schemeName;
                contributorName = map.contributorName;
                mappings = map.Mappings;
                if (!skipValidation)
                {
                    validateMappings(mappings); // throws exception
                }
                Array.Sort(mappings, new MappingComparer());
                transliterator.setAksharaMappings(mappings);
                saveUserMapFile(transliterator, mappings, skipValidation,
                        schemeName, contributorName);
            }
            catch (Exception)
            {
                schemeName = "";
                contributorName = "";
                throw;
            }
            return true;
        }

        public static void validateMappings(AksharaMapping[] mappings)
        {
            StringWriter errors = new StringWriter();
            Dictionary<string, string> commonInputMappings = new Dictionary<string, string>();
            Dictionary<string, string> langSpecificInputMappings = new Dictionary<string, string>();
            Dictionary<string, string> latinExMappings = new Dictionary<string, string>();
            foreach (AksharaMapping m in mappings)
            {
                // check if character name is valid
                if (m.CharacterName.Equals(""))
                {
                    errors.WriteLine("Character name not specified for "
                        + m.UnicodeSequence);
                }

                // check dup and blank input sequences
                foreach (string input_sequence in m.InputSequences)
                {
                    if (input_sequence.Equals(""))
                    {
                        errors.WriteLine("Input sequence for "
                            + m.CharacterName + " cannot be blank");
                        continue;
                    }

                    if (commonInputMappings.ContainsKey(input_sequence))
                    {
                        errors.WriteLine("Input sequence " + input_sequence
                            + " for character " + m.CharacterName
                            + " is already assigned to map to character "
                            + commonInputMappings[input_sequence]);
                        continue;
                    }

                    if (langSpecificInputMappings.ContainsKey(input_sequence + m.Language))
                    {
                        errors.WriteLine("Input sequence " + input_sequence
                            + " for character " + m.CharacterName
                            + " is already assigned to map to character "
                            + langSpecificInputMappings[input_sequence + m.Language]);
                        continue;
                    }

                    if (m.Language.Equals("*"))
                    {
                        commonInputMappings.Add(input_sequence, m.CharacterName);
                    }
                    else
                    {
                        //pattern dups for same lang are not ok
                        langSpecificInputMappings.Add(input_sequence + m.Language, m.CharacterName);
                        //pattern dups in different langs are ok
                        langSpecificInputMappings[input_sequence] = m.CharacterName;
                    }
                }

                // check if latinEx sequence collides with any another input sequence
                if ((m.LatinEx != null) && (!m.LatinEx.Equals("")))
                {
                    if (latinExMappings.ContainsKey(m.LatinEx))
                    {
                        errors.WriteLine("Extended Latin sequence " + m.LatinEx
                            + " for character " + m.CharacterName
                            + " is already assigned to map to character "
                            + latinExMappings[m.LatinEx]);
                    }
                    else
                    {
                        latinExMappings.Add(m.LatinEx, m.CharacterName);
                    }
                }

                // check if latinEx entries are > 0xFF
                //foreach (char c in m.LatinEx)
                //{
                //    if (c < 0x80)
                //    {
                //        errors.WriteLine("Entry for "
                //            + m.CharacterName + " has invalid character "
                //            + c + " in Extended Latin Output sequence "
                //            + "[" + m.LatinEx + "]");
                //    }
                //}

                // check if output unicode sequence is valid
                if (m.UnicodeSequence.Equals(""))
                {
                    errors.WriteLine("Unicode output not specified for "
                        + m.CharacterName);
                }

            }

            // check no dups between common and lang specific maps
            foreach (string input_sequence in langSpecificInputMappings.Keys)
            {
                if (commonInputMappings.ContainsKey(input_sequence))
                {
                    errors.WriteLine("Input sequence " + input_sequence
                        + " for character " + langSpecificInputMappings[input_sequence]
                        + " is already assigned to map to character "
                        + commonInputMappings[input_sequence]);
                }
            }

            string errorStr = errors.ToString();
            errors.Close();

            if (!errorStr.Equals(""))
            {
                throw new Exception(errorStr);
            }

            return;
        }
    }

    /// <summary>
    /// Defines mapping between input and output sequences
    /// </summary>
    public class AksharaMapping
    {
        private string m_character_name;
        private ArrayList m_input_sequences;
        private string m_unicodeSequence;
        private string m_extLatinSequence;
        private string m_lang;

        // default constructor
        /// <summary>
        /// 
        /// </summary>
        public AksharaMapping()
        {
            m_input_sequences = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="input_sequences"></param>
        /// <param name="extLatinSequence"></param>
        /// <param name="unicodeSequence"></param>
        /// <param name="lang"></param>
        public AksharaMapping(string character, string [] input_sequences,
            string extLatinSequence, string unicodeSequence,
            string lang)
        {
            m_character_name = character;
            m_input_sequences = new ArrayList();
            foreach (string input_sequence in input_sequences)
            {
                m_input_sequences.Add(input_sequence.Trim());
            }
            m_extLatinSequence = extLatinSequence;
            m_unicodeSequence = unicodeSequence;
            m_lang = lang;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("character-name", Order = 1)]
        public String CharacterName
        {
            get
            {
                if (m_character_name == null)
                {
                    return "";
                }
                else
                {
                    return m_character_name.Trim();
                }
            }
            set { m_character_name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public string InputSequenceForFormMapDataGrid {
            get
            {
                StringBuilder input_sequence_csv = new StringBuilder();
                foreach (string s in m_input_sequences)
                {
                    if (input_sequence_csv.Length == 0)
                    {
                        input_sequence_csv.Append(s);
                    }
                    else
                    {
                        input_sequence_csv.Append(",");
                        input_sequence_csv.Append(s);
                    }
                }
                return input_sequence_csv.ToString();
            }
            set {} // do nuthin
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("input-sequence", Order = 2)]
        public string[] InputSequences
        {
            get
            {
                string[] input_sequences = new string[m_input_sequences.Count];
                m_input_sequences.CopyTo(input_sequences);
                Array.Sort(input_sequences);
                return input_sequences;
            }
            set
            {
                if (value == null) return;
                string[] input_sequences = (string[])value;
                m_input_sequences.Clear();
                foreach (string input_sequence in input_sequences)
                    m_input_sequences.Add(input_sequence.Trim());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("unicode-output-sequence", Order = 3)]
        public string UnicodeSequence
        {
            get
            {
                if (m_unicodeSequence == null)
                {
                    return "";
                }
                else
                {
                    return m_unicodeSequence.Trim();
                }
            }
            set { m_unicodeSequence = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("extended-latin-output-sequence", Order = 4)]
        public string LatinEx
        {
            get
            {
                if (m_extLatinSequence == null)
                {
                    return "";
                }
                else
                {
                    return m_extLatinSequence.Trim();
                }
            }
            set { m_extLatinSequence = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("language", Order = 5)]
        public string Language
        {
            get
            {
                if (m_lang == null)
                {
                    return "";
                }
                else
                {
                    return m_lang.Trim();
                }
            }
            set { m_lang = value; }
        }

        /// <summary>
        /// character name in v1.0 xml schema
        /// </summary>
        [XmlAttribute("character")]
        public string m_character_v10;
        /// <summary>
        /// input sequence corresponding character name in v1.0 xml schema
        /// </summary>
        [XmlAttribute("sequence")]
        public string m_sequence_v10;

    }

    /// <summary>
    /// Comparator for sorting mappings
    /// </summary>
    public class MappingComparer : IComparer<AksharaMapping>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(AksharaMapping x, AksharaMapping y)
        {
            return string.CompareOrdinal(x.UnicodeSequence, y.UnicodeSequence);
        }
    }
}
