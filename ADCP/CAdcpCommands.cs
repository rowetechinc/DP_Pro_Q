using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    class CAdcpCommands
    {
        #region heading,pitch,roll

        /// <summary>
        /// Heading, pitch and roll.
        /// </summary>
        public struct HPR
        {
            /// <summary>
            /// Heading in degrees.
            /// </summary>
            public double Heading { get; set; }

            /// <summary>
            /// Pitch in degrees.
            /// </summary>
            public double Pitch { get; set; }

            /// <summary>
            /// Roll in degrees.
            /// </summary>
            public double Roll { get; set; }
        }
        #endregion

        #region battery,temperature
        public struct samp
        {
            public double Battery { get; set; }
            public double Temperature { get; set; }
            public int Beams { get; set; }
            public string Beam0Result { get; set; }
            public string Beam1Result { get; set; }
            public string Beam2Result { get; set; }
            public string Beam3Result { get; set; }
            public string Beam4Result { get; set; }
            public string Beam5Result { get; set; }
            public string Beam6Result { get; set; }
            public string Beam7Result { get; set; }



        }
        #endregion

        #region AdcpDirListing

        /// <summary>
        /// Get the listing of all the files on the ADCP
        /// and free and used space.
        /// </summary>
        public class AdcpDirListing
        {
            /// <summary>
            /// Free memory space on the ADCP.
            /// </summary>
            public float TotalSpace { get; set; }

            /// <summary>
            /// Used memory space on the ADCP.
            /// </summary>
            public float UsedSpace { get; set; }

            /// <summary>
            /// List of all the ensemble files
            /// on the system.
            /// </summary>
            //public List<AdcpEnsFileInfo> DirListing { get; set; }

            /// <summary>
            /// Initialize the list and sizes.
            /// </summary>
            public AdcpDirListing()
            {
                //DirListing = new List<AdcpEnsFileInfo>();
                TotalSpace = 0;
                UsedSpace = 0;
            }
        }

        #endregion

        #region ENGPNI

        /// <summary>
        /// Decode the results to ENGPNI.
        /// This will give the heading, pitch and roll
        /// read from the ADCP.
        /// 
        /// Ex:
        /// H=  0.00, P=  0.00, R=  0.00
        /// </summary>
        /// <param name="result">Result string from the ENGPNI command.</param>
        public static HPR DecodeEngPniResult(string result)
        {
            // Look for the ENGPNI result
            double heading = 0.0;
            double pitch = 0.0;
            double roll = 0.0;

            // Look for the line with the results
            char[] delimiters = new char[] { '\r', '\n' };
            string[] lines = result.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < lines.Length; x++)
            {
                // Check if is the line to decode
                if (lines[x].Contains("H="))
                {
                    // Decode the line
                    delimiters = new char[] { ',' };
                    string[] elem = lines[x].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int y = 0; y < elem.Length; y++)
                    {
                        // Trim the white space
                        // Then go just past the = to get the value
                        // The first character determines which value is given
                        string value = elem[y].Trim();
                        char firstChar = value[0];
                        int elemStart = value.IndexOf('=') + 1;
                        string subElem = value.Substring(elemStart, value.Length - elemStart);

                        // Parse each value
                        switch (firstChar)
                        {
                            case 'H':
                                double.TryParse(subElem, out heading);
                                break;
                            case 'P':
                                double.TryParse(subElem, out pitch);
                                break;
                            case 'R':
                                double.TryParse(subElem, out roll);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return new HPR() { Heading = heading, Pitch = pitch, Roll = roll };
        }

        #endregion
   
        #region DSDIR

        /// <summary>
        /// Parse the buffer of all the lines containing
        /// a file.  This will be the complete line with the
        /// file name, file size and date.
        /// 
        /// Ex:
        /// 
        /// DSDIR
        /// Total Space:                       3781.500 MB
        /// BOOT.BIN     2012/07/10 06:41:00      0.023
        /// RTISYS.BIN   2012/07/24 09:56:18      0.184
        /// HELP.TXT     2012/07/10 07:10:12      0.003
        /// BBCODE.BIN   2012/07/10 06:41:00      0.017
        /// ENGHELP.TXT  2012/07/17 09:49:04      0.002
        /// SYSCONF.BIN  2012/07/24 10:56:08      0.003
        /// A0000001.ENS 2012/04/02 16:53:11      1.004
        /// A0000002.ENS 2012/04/02 17:53:11      1.004
        /// A0000003.ENS 2012/04/02 18:53:11      1.004
        /// A0000004.ENS 2012/04/02 19:53:11      1.004
        /// A0000005.ENS 2012/04/02 20:53:11      1.004
        /// Used Space:                           5.252 MB
        /// 
        /// DSDIR
        /// 
        /// </summary>
        /// <param name="buffer">Buffer containing all the file info.</param>
        /// <returns>List of all the ENS and RAW files in the file list.</returns>
        public static AdcpDirListing DecodeDSDIR(string buffer)
        {
            AdcpDirListing listing = new AdcpDirListing();

            // Parse the directory listing string for all file info
            // Each line should contain a piece of file info
            string[] lines = buffer.Split('\n');

            // Create a list of all the ENS files
            for (int x = 0; x < lines.Length; x++)
            {
                // Only add the ENS files to the list
                // Ignore the txt and bin files
                //if (lines[x].Contains("ENS") || lines[x].Contains("RAW"))
                //{
                //    listing.DirListing.Add(new AdcpEnsFileInfo(lines[x]));
                //}

                // Total Space
                if (lines[x].Contains("Total Space"))
                {
                    // Parse the string of all it elements
                    // Total Space:                       3781.500 MB
                    char[] delimiters = { ':' };
                    string[] sizeInfo = lines[x].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    // If it has 2 elements, the second element should be the size
                    if (sizeInfo.Length >= 2)
                    {
                        // Remove the MB from the end of the value
                        string sizeStr = sizeInfo[1].Trim();
                        char[] delimiters1 = { ' ' };
                        string[] sizeStrElem = sizeStr.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);

                        if (sizeStrElem.Length >= 1)
                        {
                            float size = 0.0f;
                            if (float.TryParse(sizeStrElem[0].Trim(), out size))
                            {
                                listing.TotalSpace = size;
                            }
                        }
                    }
                }

                // Used space
                if (lines[x].Contains("Used Space"))
                {
                    // Parse the string of all it elements
                    // Used Space:                           5.252 MB
                    char[] delimiters = { ':' };
                    string[] sizeInfo = lines[x].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    // If it has 2 elements, the second element should be the size
                    if (sizeInfo.Length >= 2)
                    {
                        // Remove the MB from the end of the value
                        string sizeStr = sizeInfo[1].Trim();
                        char[] delimiters1 = { ' ' };
                        string[] sizeStrElem = sizeStr.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);

                        if (sizeStrElem.Length >= 1)
                        {
                            float size = 0.0f;
                            if (float.TryParse(sizeStrElem[0].Trim(), out size))
                            {
                                listing.UsedSpace = size;
                            }
                        }
                    }
                }
            }

            return listing;
        }

        #endregion
      
        #region BREAK
        /*
        /// <summary>
        /// Decode the break statement of all its data.
        /// 
        /// Ex:
        /// Copyright (c) 2009-2012 Rowe Technologies Inc. All rights reserved.
        /// DP300 DP1200 
        /// SN: 01460000000000000000000000000000
        /// FW: 00.02.05 Apr 17 2012 05:40:11
        /// </summary>
        /// <param name="buffer">Buffer containing the break statement.</param>
        /// <returns>Return the values decoded from the buffer given.</returns>
        public static string DecodeBREAK(string buffer)
        {
            string serial = "";
            string fw = "";
            string hw = "";

            // Break up the lines
            char[] delimiters = new char[] { '\r', '\n' };
            string[] lines = buffer.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // Decode each line of data
            for (int x = 0; x < lines.Length; x++)
            {
                // Change delimiter to a space
                delimiters = new char[] { ' ' };
                string[] elem = lines[x].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                // Firmware
                if (lines[x].Contains("FW:"))
                {
                    if (elem.Length >= 2)
                    {
                        fw = elem[1].Trim();

                        // Decode the firmware version
                        char[] fwDelimiters = new char[] { '.' };
                        string[] fwElem = fw.Split(fwDelimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (fwElem.Length == 3)
                        {
                            ushort major = 0;
                            ushort minor = 0;
                            ushort revision = 0;
                            if (ushort.TryParse(fwElem[0], out major))
                            {
                                firmware.FirmwareMajor = major;
                            }
                            if (ushort.TryParse(fwElem[1], out minor))
                            {
                                firmware.FirmwareMinor = minor;
                            }
                            if (ushort.TryParse(fwElem[2], out revision))
                            {
                                firmware.FirmwareRevision = revision;
                            }
                        }
                    }
                  
                }

                // Serial number
                if (lines[x].Contains("SN:"))
                {
                    if (elem.Length >= 2)
                    {
                        serial = elem[1];
                    }
                }
            }

            // Hardware
            if (lines.Length > 2)
            {
                hw = lines[1];
            }

            // Return the break statement values
            return strFM;
        }*/

        #endregion

        #region FMSHOW

        /// <summary>
        /// Decode the break statement of all its data.
        /// 
        /// Ex:
        /// System Firmware Version: 0.0.13
        /// Copyright (c) 2009-2011 Rowe Technologies Inc. All rights reserved.
        /// </summary>
        /// <param name="buffer">Buffer containing the break statement.</param>
        /// <returns>Return the values decoded from the buffer given.</returns>
        public static string DecodeFMSHOW(string buffer)
        {
            string fw = "";
          
            // Break up the lines
            char[] delimiters = new char[] { '\r', '\n' };
            string[] lines = buffer.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // Decode each line of data
            for (int x = 0; x < lines.Length; x++)
            {
                // Firmware
                if (lines[x].Contains("System Firmware Version"))
                {
                    fw = lines[x];
                } 
            }

            // Return the break statement values
            return fw;
        }

        #endregion

        #region DIAGSAMP
        public static samp DecodeDIAGSAMP(string buffer)
        {
            double temp = 0.0;
            double battery = 0.0;
            int beams = 0;
            string bm0 = "";
            string bm1 = "";
            string bm2 = "";
            string bm3 = "";
            string bm4 = "";
            string bm5 = "";
            string bm6 = "";
            string bm7 = "";

            char[] delimiters = new char[] { '\r', '\n' };
            string[] lines = buffer.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // Decode each line of data
            for (int x = 0; x < lines.Length; x++)
            {
                // battery
                if (lines[x].Contains("VINF"))
                {
                    delimiters = new char[] { ' ' };
                    string[] elem = lines[x].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (elem.Count() > 2)
                    {
                        double.TryParse(elem[1], out battery);
                    }
                }
                //temperature
                if (lines[x].Contains("Water"))
                {
                    delimiters = new char[] { ' ' };
                    string[] elem = lines[x].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (elem.Count() > 2)
                    {
                        double.TryParse(elem[1], out temp);
                    }
                }
                if (lines[x].Contains("Beam 0"))
                {
                    beams = 1;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm0 = "PASS";
                    }
                    else
                    {
                        bm0 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 1"))
                {
                    beams = 2;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm1 = "PASS";
                    }
                    else
                    {
                        bm1 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 2"))
                {
                    beams = 3;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm2 = "PASS";
                    }
                    else
                    {
                        bm2 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 3"))
                {
                    beams = 4;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm3 = "PASS";
                    }
                    else
                    {
                        bm3 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 4"))
                {
                    beams = 5;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm4 = "PASS";
                    }
                    else
                    {
                        bm4 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 5"))
                {
                    beams = 6;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm5 = "PASS";
                    }
                    else
                    {
                        bm5 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 6"))
                {
                    beams = 7;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm6 = "PASS";
                    }
                    else
                    {
                        bm6 = "FAIL";
                    }
                }
                if (lines[x].Contains("Beam 7"))
                {
                    beams = 8;
                    if (lines[x + 1].Contains("PASS"))
                    {
                        bm7 = "PASS";
                    }
                    else
                    {
                        bm7 = "FAIL";
                    }
                }
            }
            return new samp { Battery = battery, Temperature = temp, Beams = beams, Beam0Result = bm0, Beam1Result = bm1, Beam2Result = bm2, Beam3Result = bm3, Beam4Result = bm4, Beam5Result = bm5, Beam6Result = bm6, Beam7Result = bm7 };
        }
        #endregion
    }
}
