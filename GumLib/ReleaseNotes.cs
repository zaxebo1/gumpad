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
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

/*
 * 
<?xml version="1.0" encoding="utf-8"?>
<?xml-stylesheet type="text/xsl" href="ReleaseNotes.xsl"?>
<release-notes latest-version="2.0.0.6" latest-installer="gumpad-2.0.0.6.exe">
  <notes version="2.0.0.6">
    <fix>Fixed rendering issues with Help->Show Map.</fix>
    <feature>Added "check for updates" feature.</feature>
    <known-issue />
  </notes>
</release-notes>
 * 
 */

namespace GumLib
{
    [XmlRoot("release-notes")]
    public class ReleaseNotes
    {
        [XmlAttribute("latest-version")]
        public string m_latest_version;
        [XmlAttribute("latest-installer")]
        public string m_latest_installer;
        [XmlElement("notes")]
        public RelNote[] notes;
    }

    public class RelNote
    {
        [XmlAttribute("version")]
        public string m_version;
        [XmlElement("fix")]
        public string[] m_fixes;
        [XmlElement("feature")]
        public string[] m_features;
        [XmlElement("known-issue")]
        public string[] m_known_issues;
    }

    public class RelNoteComparer : IComparer<RelNote>
    {
        /// <summary>
        /// Comparator to reverse sort by rel notes version
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(RelNote x, RelNote y)
        {
            return string.CompareOrdinal(y.m_version, x.m_version);
        }
    }
}
