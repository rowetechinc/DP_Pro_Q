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
        public List<string> Notes { get; set; }

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
            Notes = new List<string>();
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
            Notes.Add(note);
        }

        public void InitActiveConfig()
        {
            ActiveConfig = new Dictionary<string, object>
            {
                { "Fixed_Commands", new List<string> { } },
                { "Wizard_Commands", new List<string> { } },
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
                {  "Q_Shore_Pings_Avg", 0.0 },
                {  "Q_Shore_Left_Ens_Count", 0.0 },
                {  "Q_Shore_Right_Ens_Count", 0.0 },
                {  "Edge_Begin_Shore_Distance", 0.0 },
                {  "Edge_Begin_Left_Bank", 0 },
                {  "Edge_End_Shore_Distance", 0.0 },
                {  "Offsets_Transducer_Depth", 0.2 },
                {  "Offsets_Magnetic_Variation", 0.0 },
                {  "Offsets_Heading_Offset", 0.0 },
                {  "Offsets_One_Cycle_K", 0.0 },
                {  "Offsets_One_Cycle_Offset", 0.0 },
                {  "Offsets_Two_Cycle_K", 0.0 },
                {  "Offsets_Two_Cycle_Offset", 0.0 },
                {  "Proc_Speed_of_Sound_Correction", 0 },
                {  "Proc_Salinity", 0.0 },
                {  "Proc_Fixed_Speed_Of_Sound", 1500 },
                {  "Proc_Mark_Below_Bottom_Bad", 1 },
                {  "Proc_Mark_Below_Sidelobe_Bad", 1 },
                {  "Proc_Screen_Depth", 0 },
                {  "Proc_Screen_BT_Depth", 0 },
                {  "Proc_Use_Weighted_Mean", 0 },
                {  "Proc_Use_Weighted_Mean_Depth", 0 },
                {  "Proc_Backscatter_Type", 0 },
                {  "Proc_Intensity_Scale", 0.43 },
                {  "Proc_Absorption", 0.139 },
                {  "Proc_Projection_Angle", 0.0 },
                {  "Proc_River_Depth_Source", 4 },
                {  "Proc_Cross_Area_Type", 2 },
                {  "Proc_Use_3_Beam_BT", 1 },
                {  "Proc_Use_3_Beam_WT", 1 },
                {  "Proc_BT_Error_Vel_Threshold", 0.1 },
                {  "Proc_WT_Error_Velocity_Threshold", 10.0 },
                {  "Proc_BT_Up_Vel_Threshold", 10.0 },
                {  "Proc_WT_Up_Vel_Threshold", 10.0 },
                {  "Proc_Fish_Intensity_Threshold", 255 },
                {  "Proc_Near_Zone_Distance", 2.1 },
                {  "Rec_Filename_Prefix", "" },
                {  "Rec_Output_Directory", "" },
                {  "Rec_Root_Directory", null },
                {  "Rec_MeasNmb", "" },
                {  "Rec_GPS", "NO" },
                {  "Rec_DS", "NO" },
                {  "Rec_EH", "NO" },
                {  "Rec_ASCII_Output", "NO" },
                {  "Rec_Max_File_Size", 0.0 },
                {  "Rec_Next_Transect_Number", 0.0 },
                {  "Rec_Add_Date_Time", 0.0 },
                {  "Rec_Use_Delimiter", 1 },
                {  "Rec_Delimiter", "" },
                {  "Rec_Prefix", "" },
                {  "Rec_Use_MeasNmb", "YES" },
                {  "Rec_Use_TransectNmb", "YES" },
                {  "Rec_Use_SequenceNmb", "NO" },
                {  "Wiz_ADCP_Type", 0.0 },
                {  "Wiz_Firmware", 0.0 },
                {  "Wiz_Use_Ext_Heading", "NO" },
                {  "Wiz_Use_GPS", "NO" },
                {  "Wiz_Use_DS", "NO" },
                {  "Wiz_Max_Water_Depth", 0.0 },
                {  "Wiz_Max_Water_Speed", 0.0 },
                {  "Wiz_Max_Boat_Space", 0.0 },
                {  "Wiz_Material", 0.0 },
                {  "Wiz_Water_Mode", 0.0 },
                {  "Wiz_Bottom_Mode", 0.0 },
                {  "Wiz_Beam_Angle", 0.0 },
                {  "Wiz_Pressure_Sensor", 0.0 },
                {  "Wiz_Water_Mode_13", 0.0 },
                {  "Wiz_StreamPro_Default", 0.0 },
                {  "Wiz_StreamPro_Bin_Size", 0.0 },
                {  "Wiz_StreamPro_Bin_Number", 0.0 },
                {  "Wiz_Use_GPS_Internal", "NO" },
                {  "Wiz_Internal_GPS_Baud_Rate_Index", 0.0 }
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
        /// Set the system settings.
        /// </summary>
        /// <param name="sysSetting"></param>
        public void SetSystemSettings(SystemSetting sysSetting)
        {
            ActiveConfig["Wiz_ADCP_Type"] = sysSetting.iInstrumentTypes;

            ActiveConfig["Offsets_Transducer_Depth"] = sysSetting.dTransducerDepth;
            ActiveConfig["Offsets_Heading_Offset"] = sysSetting.dHeadingOffset;

            ActiveConfig["Proc_Salinity"] = sysSetting.dSalinity;
            ActiveConfig["Proc_Fixed_Speed_Of_Sound"] = sysSetting.dSpeedOfSound;


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
            trans.Add("Notes", this.Notes.ToArray<string>());

            // Add Active Config
            trans.Add("active_config", this.ActiveConfig);

            // Add Moving bed type
            trans.Add("moving_bed_type", this.MovingBedType);

            return trans;

        }
    }
}
