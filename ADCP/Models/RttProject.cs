using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ADCP
{
    /// <summary>
    /// Rowe Technologies Transect Project.
    /// This project will contain all the project settings.
    /// This Project file will also be compatitble with QRev.
    /// </summary>
    public class RttProject
    {
        public static string DEFAULT_FILE_EXT = ".rtt";
        public static string DEFAULT_PRJ_NAME = "QRev_RTI_Project";

        /// <summary>
        /// Folder Path for the project file.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// List of all the transects associated
        /// with this project.
        /// </summary>
        public List<TransectConfig> Transects { get; set; }

        /// <summary>
        /// List of all the system test results.  This will be a string that contains Pass or Fails.  All the
        /// test results should be a single string.  QRev will count the number of Pass and fails in the string.
        /// Also add a timestamp for every test result.
        /// </summary>
        public List<string> SystemTestResults { get; private set; }

        /// <summary>
        /// Timestamp associated with every test result.
        /// </summary>
        public List<string> SystemTestResultsDateTime { get; private set; }

        /// <summary>
        /// List of all the compass calibration results.  QRev just documents the date and time.
        /// </summary>
        public List<string> CompassCalResults { get; private set; }

        /// <summary>
        /// Timestamp associated with every compass calibration.
        /// </summary>
        public List<string> CompassCalResultsDateTime { get; private set; }

        /// <summary>
        /// List of all the compass evaluation results.  QRev just documents the last entry contains "Fail".
        /// The string should contain all the results for the compass evaluation.
        /// </summary>
        public List<string> CompassEvaluationResults { get; private set; }

        /// <summary>
        /// Timestamp associated with every compass evaluation.
        /// </summary>
        public List<string> CompassEvaluationResultsDateTime { get; private set; }

        /// <summary>
        /// Set the project name and folder path for the project.
        /// </summary>
        /// <param name="folderPath">Folder path to save the project.</param>
        /// <param name="projectName">Project name.</param>
        public RttProject(string folderPath, string projectName)
        {
            // Intialize the values.
            if (folderPath != null)
            {
                this.FolderPath = folderPath;
            }
            else
            {
                this.FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RTI", "River");
            }
            this.ProjectName = projectName;

            Transects = new List<TransectConfig>();
            SystemTestResults = new List<string>();
            SystemTestResultsDateTime = new List<string>();
            CompassEvaluationResults = new List<string>();
            CompassEvaluationResultsDateTime = new List<string>();
            CompassCalResults = new List<string>();
            CompassCalResultsDateTime = new List<string>();

        }

        /// <summary>
        /// Set the project name for the project.
        /// Use the default folder path.
        /// </summary>
        /// <param name="projectName">Project name. DO NOT INCLUDE THE FILE EXTENSION.</param>
        public RttProject(string projectName)
        {
            // Intialize the values.
            this.FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RTI", "River");
            this.ProjectName = projectName;

            Transects = new List<TransectConfig>();
            SystemTestResults = new List<string>();
            SystemTestResultsDateTime = new List<string>();
            CompassEvaluationResults = new List<string>();
            CompassEvaluationResultsDateTime = new List<string>();
            CompassCalResults = new List<string>();
            CompassCalResultsDateTime = new List<string>();
        }

        /// <summary>
        /// Create a default project.
        /// Use the folder path: MyDocuments/RTI/River
        /// Use the default project name.
        /// </summary>
        public RttProject()
        {
            this.FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RTI", "River");
            this.ProjectName = RttProject.DEFAULT_PRJ_NAME;

            Transects = new List<TransectConfig>();
            SystemTestResults = new List<string>();
            SystemTestResultsDateTime = new List<string>();
            CompassEvaluationResults = new List<string>();
            CompassEvaluationResultsDateTime = new List<string>();
            CompassCalResults = new List<string>();
            CompassCalResultsDateTime = new List<string>();
        }

        /// <summary>
        /// Add the transect to this project.
        /// </summary>
        /// <param name="transect"></param>
        public void AddTransect(TransectConfig transect)
        {
            Transects.Add(transect);
        }

        /// <summary>
        /// Save the Project file.  Use the given information to populate the 
        /// project file.
        /// </summary>
        /// <param name="siteInfo">Site Information.</param>
        /// <param name="sysSetting">System Settings.</param>
        /// <param name="transects">Transect Information.</param>
        /// <param name="serialNum">Serial Number</param>
        /// <param name="systemDesc">System Description</param>
        /// 
        public void SaveProject(SiteInformation siteInfo, SystemSetting sysSetting, string serialNum, string systemDesc)
        {
            // Create a datetime to share
            var dt = DateTime.Now;
            string dtStr = dt.ToUniversalTime().ToString("yyyyMMddHHmmss");
            string dateStr = dt.ToUniversalTime().ToString("yyyy/MM/dd");

            Dictionary<string, object> prj = new Dictionary<string, object>();
            prj.Add("project", GetProjectDict(siteInfo, dtStr));                                                // Project Section
            prj.Add("site_info", GetSiteInfoDict(siteInfo, sysSetting, serialNum, systemDesc, dateStr));        // Site_Info section
            prj.Add("transects", GetTransectDict());                                                            // Transects section
            prj.Add("qaqc", GetQaQcDict());                                                                     // QAQC section
            prj.Add("project_settings", GetProjectSettings(siteInfo, sysSetting));                              // Project Settings
            
            // Create a top level RTI 
            Dictionary<string, object> rti = new Dictionary<string, object>();
            rti.Add("RTI", prj);                                                                    // Top Level

            // Create the Json string with indents
            string json = JsonConvert.SerializeObject(rti, Formatting.Indented);

            // Write the string to the project RTT file
            File.WriteAllText(this.GetProjectPath(), json);
        }

        /// <summary>
        /// Create a dictionary for the rpoject information.
        /// </summary>
        /// <param name="siteInfo">Site information</param>
        /// <param name="dtStr">Date Time String as UTC.</param>
        /// <returns>Dictionary for the project section.</returns>
        public Dictionary<string, object> GetProjectDict(SiteInformation siteInfo, string dtStr)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();



            // Create the project name
            string prjName = siteInfo.siteName + "_" + siteInfo.stationNumber + "_" + dtStr;

            dict.Add("Name", prjName);
            dict.Add("Version", "1.0");
            dict.Add("Locked", null);

            return dict;
        }

        /// <summary>
        /// Populate the site information in the dictionary.
        /// This will be converted to JSON to save in Project.
        /// 
        /// Reference - Vessel Velocity Reference
        /// QRev 1 = "BT"  = "Bottom Tracking" = DP Pro 0
        /// QRev 2 = "GGA" = "GPS GGA"         = DP Pro 3
        /// QRev 3 = "VTG" = "GPS VTG"         = DP Pro 1
        /// QRev 0 = ""    = "No Reference"    = DP Pro 2
        /// 
        /// 
        /// </summary>
        /// <param name="siteInfo">Site Information.</param>
        /// <param name="sysSetting">System Settings.</param>
        /// <param name="serialNum">Serial Number</param>
        /// <param name="systemDesc">System Description.</param>
        /// <param name="dateStr">Date String as UTC.</param>
        /// <returns>Dictonary to convert to JSON to save project.</returns>
        public Dictionary<string, object> GetSiteInfoDict(SiteInformation siteInfo, SystemSetting sysSetting, string serialNum, string systemDesc, string dateStr)
        {
            Dictionary<string, object> si = new Dictionary<string, object>();
            si.Add("Name", siteInfo.siteName);
            si.Add("Number", siteInfo.stationNumber);
            si.Add("ADCPSerialNmb", serialNum);
            si.Add("Description", systemDesc);
            si.Add("Remarks", siteInfo.comments);
            si.Add("Water_Temperature", sysSetting.dWaterTemperature);

            // Vessel Speed Reference
            switch (sysSetting.iSpeedRef)
            {
                case 0:
                    si.Add("Reference", "BT");
                    break;
                case 1:
                    si.Add("Reference", "VTG");
                    break;
                case 2:
                    si.Add("Reference", "");
                    break;
                case 3:
                    si.Add("Reference", "GGA");
                    break;
                default:
                    si.Add("Reference", "BT");
                    break;
            }


            return si;
        }
        
        /// <summary>
        /// Project settings.  These values are not used by QRev but should still be saved with the project.
        /// </summary>
        /// <param name="siteInfo">Site Information.</param>
        /// <param name="sysSetting">System Settings.</param>
        /// <returns></returns>
        public Dictionary<string, object> GetProjectSettings(SiteInformation siteInfo, SystemSetting sysSetting)
        {
            Dictionary<string, object> si = new Dictionary<string, object>();
            si.Add("Measurement_Number", siteInfo.MeasNumber);
            si.Add("Party", siteInfo.FieldParty);
            si.Add("BoatMotorUsed", siteInfo.BoatMotor);
            si.Add("ProcessedBy", siteInfo.ProcessedBy);
            si.Add("MeasLocation", siteInfo.MeasLocation);
            si.Add("Inside_Gage_Height", siteInfo.InsideGageH);
            si.Add("Outside_Gage_Height", siteInfo.OutsideGageH);
            si.Add("Gage_Height_Change", siteInfo.GageHChange);
            si.Add("Rating_Discharge", siteInfo.RatingDischarge);
            si.Add("Index_Velocity", siteInfo.IndexV);
            si.Add("Rating_Number", siteInfo.RatingNumber);
            si.Add("Rated_Area", siteInfo.RatedArea);
            si.Add("Magnetic_Var_Method", siteInfo.MagnVariationMethod);
            si.Add("Measurement_Rating", siteInfo.MeasurementRating);


            si.Add("GPS_Connected", sysSetting.bGPSConnect);
            si.Add("Avg_Interval", sysSetting.dAveragingInterval);
            si.Add("BT_Correlation_Thresh", sysSetting.dBtCorrelationThreshold);
            si.Add("BT_SNR", sysSetting.dBtSNR);
            si.Add("BT_Switch_Depth", sysSetting.dBtSwitchDepth);
            si.Add("Heading_Offset", sysSetting.dHeadingOffset);
            si.Add("Max_Measurement_Depth", sysSetting.dMaxMeasurementDepth);
            si.Add("Salinity", sysSetting.dSalinity);
            si.Add("SoS", sysSetting.dSpeedOfSound);
            si.Add("Transducer_Depth", sysSetting.dTransducerDepth);
            si.Add("Water_Temp", sysSetting.dWaterTemperature);
            si.Add("WP_Switch_Depth", sysSetting.dWpSwitchDepth);
            si.Add("Firmware", sysSetting.firmware);
            si.Add("Auto_Bin_Size", sysSetting.iAutoBinSize);
            si.Add("Auto_Lag", sysSetting.iAutoLag);
            si.Add("Bins", sysSetting.iBins);
            si.Add("Heading_Ref", sysSetting.iHeadingRef);
            si.Add("Instrument_Types", sysSetting.iInstrumentTypes);
            si.Add("Measurement_Mode", sysSetting.iMeasurmentMode);
            si.Add("Salinity_Source", sysSetting.iSalinitySource);
            si.Add("SoS_Source", sysSetting.iSpeedOfSoundSource);
            si.Add("Speed_Reference", sysSetting.iSpeedRef);
            si.Add("Transducer_Depth_Source", sysSetting.iTransducerDepthSource);
            si.Add("Vertical_Beams", sysSetting.iVerticalBeam);
            si.Add("Water_Temp_Source", sysSetting.iWaterTemperatureSource);
            si.Add("ADCP_Baud", sysSetting.sAdcpBaud);
            si.Add("ADCP_Port", sysSetting.sAdcpPort);
            si.Add("GPS_Baud", sysSetting.sGpsBaud);
            si.Add("GPS_Port", sysSetting.sGpsPort);
            si.Add("RS232", sysSetting.strRS232);

            // Measurement Units
            if (sysSetting.bEnglishUnit)
            {
                si.Add("HydrologicUnit", "Metric");
            }
            else
            {
                si.Add("HydrologicUnit", "Standard");
            }


            return si;
        }


        /// <summary>
        /// Convert all the transects to an dictionary and return the array of
        /// dictionaries containing all the transects.
        /// </summary>
        /// <param name="transects">Transects to add.</param>
        /// <returns>Array containing all the transects configurations as dictionaries.</returns>
        public Dictionary<string, object>[] GetTransectDict()
        {
            // Create a list which we will convert to an array and returned
            var transect_array = new List<Dictionary<string, object>>();

            // Go through each transect and add to list as a dictionary
            foreach ( var transect in this.Transects)
            {
                // Convert the transect to a dictonary
                transect_array.Add(transect.ToDict());
            }

            return transect_array.ToArray();

        }

        /// <summary>
        /// Quality control dictionary.
        /// </summary>
        /// <returns>Dictionary of quality control values.</returns>
        public Dictionary<string, object> GetQaQcDict()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("RG_Test", SystemTestResults.ToArray<string>());
            dict.Add("RG_Test_TimeStamp", SystemTestResultsDateTime.ToArray<string>());
            dict.Add("Compass_Evaluation", CompassEvaluationResults.ToArray<string>());
            dict.Add("Compass_Evaluation_TimeStamp", CompassEvaluationResultsDateTime.ToArray<string>());
            dict.Add("Compass_Calibration", CompassCalResults.ToArray<string>());
            dict.Add("Compass_Calibration_TimeStamp", CompassCalResultsDateTime.ToArray<string>());
            return dict;
        }

        /// <summary>
        /// Add a Test result to the list.
        /// A test result will contain a string with all the test results.
        /// QRev counts all the PASS and FAILS.  It then documents all the test
        /// run based on the count and all the failures based on the number of FAILS counted.
        /// 
        /// num_tests = re.findall('(Fail|FAIL|F A I L|Pass|PASS|NOT DETECTED|P A S S)', self.data)
        /// num_fails = re.findall('(Fail|FAIL|F A I L)', self.data)
        /// 
        /// </summary>
        /// <param name="testResult">String result of the test results.</param>
        public void AddSystemTestResults(string testResult)
        {
            SystemTestResults.Add(testResult);
            SystemTestResultsDateTime.Add(DateTime.Now.ToString());
        }

        /// <summary>
        /// Add a Compass Calibraitons to the list.
        /// QRev will use the date and time only.
        /// 
        /// </summary>
        /// <param name="compassCalResult">String of the compass evaluation results.</param>
        public void AddCompassCalibrationResults(string compassCalResult)
        {
            CompassCalResults.Add(compassCalResult);
            CompassCalResultsDateTime.Add(DateTime.Now.ToString());
        }

        /// <summary>
        /// Add a Compass Evaluations to the list.
        /// QRev will use the last entry with "Error" as the results.
        /// 
        /// splits = re.split('(Total error:|Double Cycle Errors:|Error from calibration:)', self.data)
        /// 
        /// </summary>
        /// <param name="compassEval">String of the compass evaluation results.</param>
        public void AddCompassEvaluationResults(string compassEval)
        {
            CompassEvaluationResults.Add(compassEval);
            CompassEvaluationResultsDateTime.Add(DateTime.Now.ToString());
        }

        /// <summary>
        /// Get the project file path.
        /// This will use the folder path and project name to generate a 
        /// file path.
        /// </summary>
        /// <returns>File path to the project.</returns>
        public string GetProjectPath()
        {
            // Verify the folder exist
            string folderPath = this.FolderPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var newPath = Path.Combine(folderPath, this.ProjectName + RttProject.DEFAULT_FILE_EXT);

            return newPath;
        }

        /// <summary>
        /// Get the default folder path for all projects.
        /// This will create the folder in MyDocuments/RTI/River
        /// </summary>
        /// <returns>Folder path for all projects.</returns>
        public static string GetDefaultFolderPath()
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RTI", "River");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        /// <summary>
        /// Get the default file path for the project file.
        /// If no project name is given, the default project name is used.
        /// 
        /// </summary>
        /// <param name="projectName">Project name.</param>
        /// <returns>Full file path to the project file.</returns>
        public static string GetDefaultPath(string projectName="QRev_RTI_Project", string fileExt = ".rtt")
        {
            // Get the default folder path
            string folderPath = RttProject.GetDefaultFolderPath();

            // Create the path based on the project name and folder path
            var newPath = Path.Combine(folderPath, projectName + fileExt);


            return newPath;
        }

    }
}

/**
private void WriteSmartPageToFile(string fileName)
{
    //File.WriteAllText(fileName, "Language " + "" + "\r\n"); //将语言单独写一个configuration
    File.WriteAllText(fileName, "ADCP_PortName " + sp.PortName + "\r\n");
    File.AppendAllText(fileName, "ADCP_BaudRate " + sp.BaudRate + "\r\n");

    if (bGPSConnect)
        File.AppendAllText(fileName, "GPS_Connect 1" + "\r\n");//LPJ 2013-9-25 当连接GPS为true，否则为false
    else
        File.AppendAllText(fileName, "GPS_Connect 0" + "\r\n");
    File.AppendAllText(fileName, "GPS_SerialPort " + GPS_sp.PortName + "\r\n");
    File.AppendAllText(fileName, "GPS_Baudrate " + GPS_sp.BaudRate + "\r\n");


    //if (labelUnit.Text == Resource1.String236)
    if (bEnglish2Metric)
        File.AppendAllText(fileName, "Unit 0" + "\r\n");
    else
        File.AppendAllText(fileName, "Unit 1" + "\r\n");
    //File.AppendAllText(fileName, "Unit " + labelUnit.Text + "\r\n");

    #region write site information to file
    //File.AppendAllText(fileName, "SiteName " + labelSiteName.Text + "\r\n");
    //File.AppendAllText(fileName, "StationNumber " +labelStationNumber.Text +"\r\n");
    //File.AppendAllText(fileName, "MeasurementNumber " + labelMeasNumber.Text + "\r\n");
    //File.AppendAllText(fileName, "Comments " + labelSiteComments.Text + "\r\n");

    // Write the site information to the file
    WriteSiteInformationToFile(fileName, siteInformation);
    #endregion

    //File.AppendAllText(fileName, "SerialNumber " +labelsn +"\r\n");
    File.AppendAllText(fileName, "FirmwareVersion " + labelFirmWare.Text + "\r\n");
    File.AppendAllText(fileName, "InstrumentSN " + labelInstrumentSN.Text + "\r\n");  //LPJ  2016-12-14
    File.AppendAllText(fileName, "SystemNumber " + labelSystemNumber.Text + "\r\n");

    //if (labelFlowRef.Text == Resource1.String228) //LPJ 2013-7-24 cancel
    //    File.AppendAllText(fileName, "FlowReference 0"  + "\r\n");
    //else
    //    File.AppendAllText(fileName, "FlowReference 1" + "\r\n");
    //File.AppendAllText(fileName, "FlowReference " + labelFlowRef.Text+"\r\n");
    if (labelVesselRef.Text == Resource1.String232)
        File.AppendAllText(fileName, "VesselSpeedReference 0" + "\r\n");
    else if (labelVesselRef.Text == Resource1.String233)
        File.AppendAllText(fileName, "VesselSpeedReference 2" + "\r\n");
    else if (labelVesselRef.Text == "GPS VTG")
        File.AppendAllText(fileName, "VesselSpeedReference 1" + "\r\n");
    else
        File.AppendAllText(fileName, "VesselSpeedReference 3" + "\r\n");
    //File.AppendAllText(fileName, "VesselSpeedReference " + labelVesselRef.Text+"\r\n");

    if (labelHeadingRef.Text == Resource1.String230)
        File.AppendAllText(fileName, "HeadingReference 0" + "\r\n");
    else
        File.AppendAllText(fileName, "HeadingReference 1" + "\r\n");

    File.AppendAllText(fileName, "HeadingOffset " + label_Headingoffset.Text + "\r\n"); //LPJ 2014-6-16
    File.AppendAllText(fileName, "Salinity " + labelSalinity.Text + "\r\n"); //LPJ 2014-6-16
    File.AppendAllText(fileName, "TransducerDepth " + labelTransducerDepth.Text + "\r\n");

    if (labelTopEstimate.Text == Resource1.String33)
        File.AppendAllText(fileName, "TopMode 0 " + "\r\n");
    else if (labelTopEstimate.Text == Resource1.String224)
        File.AppendAllText(fileName, "TopMode 1 " + "\r\n");
    else
        File.AppendAllText(fileName, "TopMode 2 " + "\r\n");

    if (labelBottomEstimate.Text == Resource1.String33)
        File.AppendAllText(fileName, "BottomMode 0 " + "\r\n");
    else
        File.AppendAllText(fileName, "BottomMode 1 " + "\r\n");

    File.AppendAllText(fileName, "PowerCoff " + labelPowerCurveCoeff.Text + "\r\n");

    if (labelStartEdge.Text == Resource1.String226)
        File.AppendAllText(fileName, "StartBank 0" + "\r\n");
    else
        File.AppendAllText(fileName, "StartBank 1" + "\r\n");
    //File.AppendAllText(fileName, "StartBank " + labelStartEdge.Text + "\r\n"); //在测量结束后，将左右岸信息再重新写入
    File.AppendAllText(fileName, "LeftBankDist " + labelLeftDis.Text + "\r\n");

    if (labelLeftType.Text == Resource1.String221)
        File.AppendAllText(fileName, "LeftBankStyle 0 " + "\r\n");
    else if (labelLeftType.Text == Resource1.String222)
        File.AppendAllText(fileName, "LeftBankStyle 1 " + "\r\n");
    else
        File.AppendAllText(fileName, "LeftBankStyle 2 " + "\r\n");


    File.AppendAllText(fileName, "LeftBankCoff " + labelLeftRef.Text + "\r\n");
    File.AppendAllText(fileName, "LeftBankPings " + dLeftShorePings.ToString() + "\r\n");
    File.AppendAllText(fileName, "RightBankDist " + labelRightDis.Text + "\r\n");

    if (labelRightType.Text == Resource1.String221)
        File.AppendAllText(fileName, "RightBankStyle 0 " + "\r\n");
    else if (labelRightType.Text == Resource1.String222)
        File.AppendAllText(fileName, "RightBankStyle 1 " + "\r\n");
    else
        File.AppendAllText(fileName, "RightBankStyle 2 " + "\r\n");
    //File.AppendAllText(fileName, "RightBankStyle " + labelRightType.Text + "\r\n");
    File.AppendAllText(fileName, "RightBankCoff " + labelRightRef.Text + "\r\n");
    File.AppendAllText(fileName, "RightBankPings " + dRightShorePings.ToString() + "\r\n"); //LPJ 2013-5-29 将右岸平均呯数写入配置文件中

    WriteStandardModeToFile(fileName); //lpj 2013-7-30
}

private void WriteSiteInformationToFile(string fileName, SiteInformation site)
{
    File.AppendAllText(fileName, "SiteName " + site.siteName + "\r\n");
    File.AppendAllText(fileName, "StationNumber " + site.stationNumber + "\r\n");
    File.AppendAllText(fileName, "MeasurementNumber " + site.MeasNumber + "\r\n");           // ****

    File.AppendAllText(fileName, "FieldParty " + site.FieldParty + "\r\n");
    File.AppendAllText(fileName, "ProcessedBy " + site.ProcessedBy + "\r\n");
    File.AppendAllText(fileName, "DeploymentType " + site.DeploymentType + "\r\n");
    File.AppendAllText(fileName, "BoatMotor " + site.BoatMotor + "\r\n");
    File.AppendAllText(fileName, "MeasLocation " + site.MeasLocation + "\r\n");           // ****

    File.AppendAllText(fileName, "InsideGageH " + site.InsideGageH + "\r\n");
    File.AppendAllText(fileName, "OutsideGageH " + site.OutsideGageH + "\r\n");
    File.AppendAllText(fileName, "GageHChange " + site.GageHChange + "\r\n");
    File.AppendAllText(fileName, "RatingDischarge " + site.RatingDischarge + "\r\n");
    File.AppendAllText(fileName, "IndexV " + site.IndexV + "\r\n");

    File.AppendAllText(fileName, "RatingNumber " + site.RatingNumber + "\r\n");
    File.AppendAllText(fileName, "RatedArea " + site.RatedArea + "\r\n");
    File.AppendAllText(fileName, "WaterTemp " + site.WaterTemp + "\r\n");
    File.AppendAllText(fileName, "MagnVariationMethod " + site.MagnVariationMethod + "\r\n");
    File.AppendAllText(fileName, "MeasurementRating " + site.MeasurementRating + "\r\n");           // ****

    //File.AppendAllText(fileName, "ControlCode1 " + site.ControlCode1 + "\r\n");
    //File.AppendAllText(fileName, "ControlCode2 " + site.ControlCode2 + "\r\n");
    //File.AppendAllText(fileName, "ControlCode3 " + site.ControlCode3 + "\r\n");

    File.AppendAllText(fileName, "Comments " + site.comments + "\r\n");

}
**/
