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
using System.IO;
using System.Diagnostics;
using GumLib;

namespace GumPad.CommandLine
{
    class CommandLineProcessor
    {
        static volatile StringBuilder filterOutput;

        public static int processCommandLine(string[] args)
        {
            string inFileName="";
            string outFileName="";
            string lang = Transliterator.DEVANAGARI;
            bool asEntityCode = false;
            string mapFileName = "";
            string inputFilterProgram = ""; //e.g deTex to preprocess laTex prior to translit
            Encoding encoding = Encoding.ASCII;

            for (int i = 0; i < args.Length; i++)
            {
                switch(args[i])
                {
                    case "-i":
                        {
                            if (i < args.Length)
                            {
                                inFileName = args[++i];
                                continue;
                            }
                            else
                            {
                                printUsage();
                                return -1;
                            }
                        }
                    case "-o":
                        {
                            if (i < args.Length)
                            {
                                outFileName = args[++i];
                                continue;
                            }
                            else
                            {
                                printUsage();
                                return -1;
                            }
                        }
                    case "-m":
                        {
                            if (i < args.Length)
                            {
                                mapFileName = args[++i];
                                continue;
                            }
                            else
                            {
                                printUsage();
                                return -1;
                            }
                        }
                    case "-f":
                        {
                            if (i < args.Length)
                            {
                                inputFilterProgram = args[++i];
                                continue;
                            }
                            else
                            {
                                printUsage();
                                return -1;
                            }
                        }
                    case "-l":
                        {
                            if (i < args.Length)
                            {
                                lang = args[++i];
                                switch (lang)
                                {
                                    case "te":
                                        {
                                            lang = Transliterator.TELUGU;
                                            continue;
                                        }
                                    case "hi":
                                        {
                                            lang = Transliterator.DEVANAGARI;
                                            continue;
                                        }
                                    case "ta":
                                        {
                                            lang = Transliterator.TAMIL;
                                            continue;
                                        }
                                    case "ka":
                                        {
                                            lang = Transliterator.KANNADA;
                                            continue;
                                        }
                                    case "ma":
                                        {
                                            lang = Transliterator.MALAYALAM;
                                            continue;
                                        }
                                    case "or":
                                        {
                                            lang = Transliterator.ORIYA;
                                            continue;
                                        }
                                    case "gu":
                                        {
                                            lang = Transliterator.GUJARATI;
                                            continue;
                                        }
                                    case "gr":
                                        {
                                            lang = Transliterator.GURMUKHI;
                                            continue;
                                        }
                                    case "be":
                                        {
                                            lang = Transliterator.BENGALI;
                                            continue;
                                        }
                                    case "mr":
                                        {
                                            lang = Transliterator.MARATHI;
                                            continue;
                                        }
                                    case "en":
                                        {
                                            lang = Transliterator.LATIN;
                                            continue;
                                        }
                                    case "ex":
                                        {
                                            lang = Transliterator.LATINEX;
                                            continue;
                                        }
                                    default :
                                        {
                                            printUsage();
                                            return -1;
                                        }
                                }
                            }
                            else
                            {
                                printUsage();
                                return -1;
                            }
                        }
                    case "-e":
                        {
                            if (i < args.Length)
                            {
                                string enc = args[++i];
                                switch (enc)
                                {
                                    case "ascii":
                                        {
                                            encoding = Encoding.ASCII;
                                            continue;
                                        }
                                    case "utf8":
                                        {
                                            encoding = Encoding.UTF8;
                                            continue;
                                        }
                                    case "utf16":
                                        {
                                            encoding = Encoding.Unicode;
                                            continue;
                                        }
                                    case "utf16be":
                                        {
                                            encoding = Encoding.BigEndianUnicode;
                                            continue;
                                        }
                                    case "utf32":
                                        {
                                            encoding = Encoding.UTF32;
                                            continue;
                                        }
                                    default:
                                        {
                                            printUsage();
                                            return -1;
                                        }
                                }
                            }
                            else
                            {
                                printUsage();
                                return -1;
                            }
                        }
                    default:
                        {
                            printUsage();
                            return -1;
                        }
                }
            }

            if (inFileName.Equals("") || outFileName.Equals(""))
            {
                printUsage();
                return -2;
            }

            Transliterator t = new Transliterator();
            if (!inputFilterProgram.Equals("")) //e.g detex
            {
                Process filterProcess;
                filterProcess = new Process();
                filterProcess.StartInfo.FileName = inputFilterProgram;
                filterProcess.StartInfo.Arguments = inFileName;

                // Set UseShellExecute to false for redirection.
                filterProcess.StartInfo.UseShellExecute = false;

                // Redirect the standard output of the input filter command.  
                // This stream is read asynchronously using an event handler.
                filterProcess.StartInfo.RedirectStandardOutput = true;

                filterOutput = new StringBuilder("");

                // Set our event handler to asynchronously read the filter output.
                filterProcess.OutputDataReceived += new DataReceivedEventHandler(FilterOutputHandler);

                // Start the process.
                filterProcess.Start();

                // Start the asynchronous read of the filter output stream.
                filterProcess.BeginOutputReadLine();

                // Wait for the filter process to write the processed output.
                filterProcess.WaitForExit();

                string tempFileName = Path.GetTempFileName();
                StreamWriter filterOutputWriter = new StreamWriter(tempFileName);
                filterOutputWriter.Write(filterOutput);
                filterOutputWriter.Close();
                inFileName = tempFileName;
                filterProcess.Close();
            }
            if (!mapFileName.Equals(""))
            {
                string schemeName;
                string contributorName;
                TransliterationMap.loadMap(t, new StreamReader(mapFileName, Encoding.UTF8), 
                    false, out schemeName, out contributorName);
            }
            t.Language = lang;
            t.Transliterate(new StreamReader(inFileName, encoding), new StreamWriter(outFileName, false, Encoding.UTF8), asEntityCode);
            return 0;
        }

        private static void FilterOutputHandler(object filterProcess,
            DataReceivedEventArgs output)
        {
            // Collect the filter command output.
            if (!String.IsNullOrEmpty(output.Data))
            {
                filterOutput.Append(output.Data);
            }
        }

        private static void printUsage()
        {
            Console.WriteLine("usage: gumpad -i <infile> -o <outfile>"
                + " [ -lang te|hi|ta|ka|ma|mr|or|be|gu|gr|en|ex ]"
                + " [ -m <mapfile> ]"
                + " [ -f <input-filter> ]"
                + " [ -e ascii|utf8|utf16|utf16be|utf32 ]");
        }

    }
}
