using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    /// <summary>
    /// Transect configuration.  This will hold the file names and
    /// configuration for a transect.  The transect will then be loaded
    /// into QRev for reprocessing.
    /// </summary>
    public class TransectConfig
    {

        public class TransectNote
        {
            /// <summary>
            /// Date and time of the note.
            /// </summary>
            public string DT { get; set; }

            /// <summary>
            /// File number for the note.
            /// </summary>
            public int FileNum { get; set; }

            /// <summary>
            /// Test for the note.
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Initialize the value.
            /// </summary>
            /// <param name="text">Text to add to the note.</param>
            /// <param name="fileNum">File number.</param>
            public TransectNote(string text, int fileNum = 0)
            {
                this.DT = DateTime.Now.ToString();
                this.FileNum = fileNum;
                this.Text = text;
            }

            /// <summary>
            /// Convert this object to a dictionary so it can be used as a JSON object.
            /// </summary>
            /// <returns>Dictonary of this object.</returns>
            public Dictionary<string, object> toDict()
            {
                var json_dict = new Dictionary<string, object>()
                {
                    { "NoteFileNo", this.FileNum.ToString() },
                    { "NoteDate", this.DT },
                    { "NoteText", this.Text }
                };

                return json_dict;
            }


        }

        /// <summary>
        /// List of file names for the Transect.  It is assumed the
        /// files will be located in the same folder as the project.
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// Active configuration for the transect.  This is all the 
        /// settings for the transect.
        /// </summary>
        public Dictionary<string, object> ActiveConfig { get; set; }

        /// <summary>
        /// Enable or disable this transect.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Notes.
        /// </summary>
        public List<TransectNote> Notes { get; set; }

        /// <summary>
        /// Moving bed type.
        /// </summary>
        public string MovingBedType { get; set; }


        /// <summary>
        /// Initialize the object.
        /// </summary>
        public TransectConfig()
        {
            Files = new List<string>();
            InitActiveConfig();
            Checked = true;
            Notes = new List<TransectNote>();
            MovingBedType = null;
        }

        /// <summary>
        /// Add a file to the list of files
        /// for the transect.
        /// </summary>
        /// <param name="fileName">File name.  Do not include the path.</param>
        public void AddFile(string fileName)
        {
            Files.Add(fileName);
        }

        /// <summary>
        /// Add a note.
        /// </summary>
        /// <param name="note">Note to add.</param>
        public void AddNote(string note)
        {
            Notes.Add(new TransectNote(note, Notes.Count));
        }

        public void InitActiveConfig()
        {
            ActiveConfig = new Dictionary<string, object>
            {
                {  "Fixed_Commands", new List<string> { } },
                {  "Wizard_Commands", new List<string> { } },
                {  "User_Commands", new List<string> { } },
                {  "Fixed_Commands_RiverQ", new List<string> { } },
                {  "DS_Use_Process", 0 },
                {  "DS_Transducer_Depth", 0.0 },
                {  "DS_Transducer_Offset", 0.0 },
                {  "DS_Cor_Spd_Sound", 0 },
                {  "DS_Scale_Factor", 0.0 },
                {  "Ext_Heading_Offset", 0.0 },
                {  "Ext_Heading_Use", false },
                {  "GPS_Time_Delay", "" },
                {  "Q_Top_Method", 0.0 },
                {  "Q_Bottom_Method", 0.0 },
                {  "Q_Power_Curve_Coeff", 0.0 },
                {  "Q_Cut_Top_Bins", 0.0 },
                {  "Q_Bins_Above_Sidelobe", 0.0 },
                {  "Q_Left_Edge_Type", 1.0 },
                {  "Q_Left_Edge_Coeff", 0.0 },
                {  "Q_Right_Edge_Type", 1.0 },
                {  "Q_Right_Edge_Coeff", 0.0 },
                {  "Q_Shore_Left_Ens_Count", 0.0 },
                {  "Q_Shore_Right_Ens_Count", 0.0 },
                {  "Edge_Begin_Shore_Distance", 0.0 },
                {  "Edge_Begin_Left_Bank", 0 },
                {  "Edge_End_Shore_Distance", 0.0 },
                {  "Offsets_Transducer_Depth", 0.2 },
                {  "Offsets_Magnetic_Variation", 0.0 },
                {  "Offsets_Heading_Offset", 0.0 },
                {  "Proc_Speed_of_Sound_Correction", 0 },
                {  "Proc_Salinity", 0.0 },
                {  "Proc_Fixed_Speed_Of_Sound", 1500 },
                {  "Proc_Mark_Below_Bottom_Bad", 1 },
                {  "Proc_Screen_Depth", 0 },
                {  "Proc_Use_Weighted_Mean", 0 },
                {  "Proc_Use_Weighted_Mean_Depth", 0 },
                {  "Proc_Absorption", 0.139 },
                {  "Proc_River_Depth_Source", 4 },
                {  "Proc_Use_3_Beam_BT", 1 },
                {  "Proc_Use_3_Beam_WT", 1 },
                {  "Proc_BT_Error_Vel_Threshold", 0.1 },
                {  "Proc_WT_Error_Velocity_Threshold", 10.0 },
                {  "Proc_BT_Up_Vel_Threshold", 10.0 },
                {  "Proc_WT_Up_Vel_Threshold", 10.0 },
            };
        }

        /// <summary>
        /// Set the edge settings for QRev.
        /// 
        /// 
        /// Edge DP Pro Values
        /// 0 = Slope
        /// 1 = Vertical
        /// 2 = Estimate
        /// QRev Values
        /// Q_Left_Edge_Type - 0 = Triangular, 1 = Rectangular, 2 = Custom
        /// Q_Right_Edge_Type - 0 = Triangular, 1 = Rectangular, 2 = Custom
        /// 
        /// 
        /// Top DP Pro Values
        /// 0 = Power Curve
        /// 1 = Constant
        /// 2 = 3-Pt.Slope
        /// QRev Values
        /// Q_Top_Method - 1 = Constant, 2 = 3-Point
        /// 
        /// 
        /// Bottom DP Pro Values
        /// 0 = Power Curve
        /// 1 = Constant
        /// QRev Values
        /// Q_Bottom_Method - 2 = No Slip
        /// </summary>
        /// <param name="edgeSetting">Edge settings to populate the populate settings.</param>
        public void SetEdgeSettings(EdgeSetting edgeSetting)
        {

            ActiveConfig["Q_Top_Method"] = edgeSetting.iTopEstimate;                        // Top Method
            ActiveConfig["Q_Bottom_Method"] = edgeSetting.iBottomEstimate;                  // Bottom Method
            ActiveConfig["Q_Power_Curve_Coeff"] = edgeSetting.dPowerCurveCoeff;             // Power Curve Coeff

            ActiveConfig["Q_Left_Edge_Type"] = edgeSetting.iLeftType;                       // Left Type
            ActiveConfig["Q_Left_Edge_Coeff"] = edgeSetting.dLeftRef;                       // Left Coeff
            ActiveConfig["Q_Right_Edge_Type"] = edgeSetting.iRightType;                     // Right Type
            ActiveConfig["Q_Right_Edge_Coeff"] = edgeSetting.dRightRef;                     // Right Coeff

            if (edgeSetting.bStartLeft)
            {
                // Left Start
                ActiveConfig["Edge_Begin_Left_Bank"] = 1;                                   // Left Bank Start
                ActiveConfig["Edge_Begin_Shore_Distance"] = edgeSetting.dLeftDis;           // Left Distance (Start)
                ActiveConfig["Edge_End_Shore_Distance"] = edgeSetting.dRightDis;            // Right Distance (End)
            }
            else
            {
                // Right Start
                ActiveConfig["Edge_Begin_Left_Bank"] = 0;                                   // Right Bank Start
                ActiveConfig["Edge_Begin_Shore_Distance"] = edgeSetting.dRightDis;          // Right Distance (Start)
                ActiveConfig["Edge_End_Shore_Distance"] = edgeSetting.dLeftDis;             // Left Distance (End)
            }
        }

        /// <summary>
        /// Set the start edge settings.  
        /// Also set the top, bottom and power curve.
        /// </summary>
        /// <param name="isStartLeft">Flag if the transect starts on the left or right.</param>
        /// <param name="startDist">Start distance.</param>
        /// <param name="startType">Start contour Type.  Rectangluar or Angled.</param>
        /// <param name="startCoeff">Start coeffienct.</param>
        /// <param name="topMethod">Top method used to calculate Q.</param>
        /// <param name="bottomMethod">Bottom Method to calcualte Q.</param>
        /// <param name="powerCurveCoeff">Power Curve Coefficent for Top method.</param>
        public void SetStartEdgeSettings(bool isStartLeft, double startDist, int startType, decimal startCoeff, int topMethod, int bottomMethod, double powerCurveCoeff)
        {
            ActiveConfig["Q_Top_Method"] = topMethod;                                       // Top Method
            ActiveConfig["Q_Bottom_Method"] = bottomMethod;                                 // Bottom Method
            ActiveConfig["Q_Power_Curve_Coeff"] = powerCurveCoeff;                          // Power Curve Coeff For top and bottom

            if (isStartLeft)
            {
                ActiveConfig["Edge_Begin_Left_Bank"] = 1;                                   // Left Bank Start
                ActiveConfig["Edge_Begin_Shore_Distance"] = startDist;                      // Left Distance (Start)
                ActiveConfig["Q_Left_Edge_Type"] = startType;                               // Left Type
                ActiveConfig["Q_Left_Edge_Coeff"] = startCoeff;                             // Left Coeff
            }
            else
            {
                ActiveConfig["Edge_Begin_Left_Bank"] = 0;                                   // Right Bank Start
                ActiveConfig["Edge_Begin_Shore_Distance"] = startDist;                      // Right Distance (Start)
                ActiveConfig["Q_Right_Edge_Type"] = startType;                              // Right Type
                ActiveConfig["Q_Right_Edge_Coeff"] = startCoeff;                            // Right Coeff
            }
        }

        /// <summary>
        /// Set the End Edge settings.  This will set the values to the
        /// opposite of the start edge.
        /// </summary>
        /// <param name="isStartLeft">Check which edge was start.</param>
        /// <param name="endDist">End distance.</param>
        /// <param name="endType">End Edge Type.</param>
        /// <param name="endCoeff">End Edge Coeffienct.</param>
        public void SetEndEdgeSettings(bool isStartLeft, double endDist, int endType, decimal endCoeff)
        {
            if (isStartLeft)
            {
                // Start was Left, so set the Right edge values for the end
                ActiveConfig["Edge_End_Shore_Distance"] = endDist;                          // Right Distance (Start)
                ActiveConfig["Q_Right_Edge_Type"] = endType;                                // Right Type
                ActiveConfig["Q_Right_Edge_Coeff"] = endCoeff;                              // Right Coeff
            }
            else
            {
                // Start was Right, so set the Left edge values for the end
                ActiveConfig["Edge_End_Shore_Distance"] = endDist;                          // Left Distance (Start)
                ActiveConfig["Q_Left_Edge_Type"] = endType;                                 // Left Type
                ActiveConfig["Q_Left_Edge_Coeff"] = endCoeff;                               // Right Coeff
            }
        }

        /// <summary>
        /// Set the Start Edge Ensemble count.
        /// </summary>
        /// <param name="isStartLeft">Determine if left or right is the start edge.</param>
        /// <param name="ensCount">Number of ensemble in the start edge.</param>
        public void SetStartEdgeEnsCount(bool isStartLeft, int ensCount)
        {
            if (isStartLeft)
            {
                ActiveConfig["Q_Shore_Left_Ens_Count"] = ensCount;
            }
            else
            {
                ActiveConfig["Q_Shore_Right_Ens_Count"] = ensCount;
            }
        }

        /// <summary>
        /// Set the Start Edge Ensemble count.
        /// If it started on the left, then set the Right edge count.
        /// </summary>
        /// <param name="isStartLeft">Determine if left or right is the start edge.</param>
        /// <param name="ensCount">Number of ensemble in the start edge.</param>
        public void SetEndEdgeEnsCount(bool isStartLeft, int ensCount)
        {
            if (isStartLeft)
            {
                // Started on the left, so set the right value
                ActiveConfig["Q_Shore_Right_Ens_Count"] = ensCount;
            }
            else
            {
                // Started on the right, so se the left value
                ActiveConfig["Q_Shore_Left_Ens_Count"] = ensCount;
            }
        }

        /// <summary>
        /// Set the system settings.
        /// </summary>
        /// <param name="sysSetting"></param>
        public void SetSystemSettings(SystemSetting sysSetting)
        {
            ActiveConfig["Wiz_ADCP_Type"] = sysSetting.iInstrumentTypes;

            ActiveConfig["Offsets_Transducer_Depth"] = sysSetting.dTransducerDepth;
            ActiveConfig["DS_Transducer_Depth"] = sysSetting.dTransducerDepth;
            ActiveConfig["Offsets_Heading_Offset"] = sysSetting.dHeadingOffset;

            ActiveConfig["Proc_Salinity"] = sysSetting.dSalinity;
            ActiveConfig["Proc_Fixed_Speed_Of_Sound"] = sysSetting.dSpeedOfSound;

            Notes.Add(new TransectNote("ADCP Serial Port: " + sysSetting.sAdcpPort, Notes.Count));
            Notes.Add(new TransectNote("ADCP Baud Rate: " + sysSetting.sAdcpBaud.ToString(), Notes.Count));
            if (sysSetting.bGPSConnect)
            {
                Notes.Add(new TransectNote("GPS Connected", Notes.Count));
                Notes.Add(new TransectNote("GPS Serial Port: " + sysSetting.sGpsPort, Notes.Count));
                Notes.Add(new TransectNote("GPS Baud Rate: " + sysSetting.sGpsBaud.ToString(), Notes.Count));
            }
            else
            {
                Notes.Add(new TransectNote("No GPS Connected", Notes.Count));
            }

            ActiveConfig["Wiz_Firmware"] = sysSetting.firmware;


            /**
             *  public bool bEnglishUnit;
                public int iInstrumentTypes;

                //public int iFlowRef;
                public int iSpeedRef;
                public int iHeadingRef;
                public string strRS232;

                public int iMeasurmentMode;
                public int iVerticalBeam;
                public int iAutoBinSize;
                public int iAutoLag;
                public int iWaterTemperatureSource;
                public int iTransducerDepthSource;
                public int iSalinitySource;
                public int iSpeedOfSoundSource;

                public int iBins;

                public double dAveragingInterval;
                public double dMaxMeasurementDepth;
                public double dWpSwitchDepth;
                public double dBtSwitchDepth;
                public double dTransducerDepth;
                public double dBtCorrelationThreshold;
                public double dHeadingOffset;

                public double dBtSNR;

                public double dSalinity;
                public double dWaterTemperature;
                public double dSpeedOfSound;
             */
        }

        /// <summary>
        /// Set the list of commands used by DP Pro Q.
        /// The string will be broken up into each command by newline.
        /// </summary>
        /// <param name="cmdList">String of all the commands used to configure the ADCP.</param>
        public void SetCommandList(string cmdsStr)
        {
            string[] cmds = cmdsStr.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            ActiveConfig["Wizard_Commands"] = cmds;
        }

        /// <summary>
        /// Convert the note objects to a dictionary and add it to an array.
        /// This way they can be converted to a JSON object.
        /// </summary>
        /// <returns>Array of dictionaries containing the Note data.</returns>
        private Dictionary<string, Object>[] NotesToArray()
        {
            var array = new Dictionary<string, Object>[Notes.Count];
            for(int x = 0; x < Notes.Count; x++)
            {
                array[x] = Notes[x].toDict();
            }

            return array;
        }

        /// <summary>
        /// Create a dictionary to convert to JSON later.
        /// This will include all the values for this transect.
        /// </summary>
        /// <returns>Dictionary to add to Transects section.</returns>
        public Dictionary<string, object> ToDict()
        {
            var trans = new Dictionary<string, object>();

            // Set if checked
            if (this.Checked)
            {
                trans.Add("Checked", 1);
            }
            else
            {
                trans.Add("Checked", 0);
            }

            // Add the Files list
            trans.Add("Files", this.Files.ToArray<string>());

            // Add Notes
            trans.Add("Notes", this.NotesToArray());

            // Add Active Config
            trans.Add("active_config", this.ActiveConfig);

            // Add Moving bed type
            trans.Add("moving_bed_type", this.MovingBedType);

            return trans;

        }
    }
}
