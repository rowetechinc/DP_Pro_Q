using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calcflow;
using PickAndDecode;
using System.IO;
using DP_Pro_Calibration;
using ADCP;

namespace DP_Pro_Calibration
{
    public partial class FrmCalibration : Form
    {
        public FrmCalibration(bool bStart,string strPath)
        {
            InitializeComponent();

            OnLoadRawData(strPath + ".bin");
            textBox1.Text = strPath;

            OnLoadGPS(strPath + ".gps");

            GetCalibrationResult();
        }

        private void OnLoadRawData(string strPath)
        {
            rawdata.Clear();
            OperateFile operateFile = new OperateFile();
            rawdata = operateFile.ReadFile(strPath);
        }

        private void OnLoadGPS(string strPath)
        {
            gpsdata.Clear();
            List<string> strGPSData = new List<string>();

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.Default);
            string strLine = "";
            string strGPSLine = "";
            while ((strLine = sr.ReadLine()) != null)
            {
                if (strLine.Contains("#"))
                {
                    if (strGPSLine != "")
                    {
                        strGPSData.Add(strGPSLine);
                    }
                    strGPSLine = "";
                }
                else
                {
                    strGPSLine += strLine + "\r\n";
                }

            }
            if (strGPSLine != "")
                strGPSData.Add(strGPSLine);

            ReadData read = new ReadData();

            foreach (string str in strGPSData)
            {
                GPSdata gps = new GPSdata();
                try
                {
                    read.EncodeGPS(str, ref gps.UTC, ref gps.GGA_Latitude, ref gps.GGA_Longitude, ref gps.VTG_Vx, ref gps.VTG_Vy, ref gps.HDT_heading);
                    gpsdata.Add(gps);
                }
                catch
                {
                }
            }
        }
        
        List<ArrayClass> rawdata = new List<ArrayClass>();
        List<GPSdata> gpsdata = new List<GPSdata>();
        CalibrateResult calResult = new CalibrateResult();
        List<ProfileResult> pResult = new List<ProfileResult>();

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.CurrentDirectory;
            dlg.Filter = "BIN(*.bin)|*.bin|ENS(*.ENS)|*.ENS";
            dlg.Title = "Open File";
            dlg.ShowReadOnly = true;
            dlg.Multiselect = true;  //选择多个文件    

            rawdata.Clear();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OperateFile operateFile = new OperateFile();
                rawdata = operateFile.ReadFile(dlg.FileName);

                textBox1.Text = dlg.FileName;

                btnLoadGPS.Enabled = true;
            }
        }

        private void btnLoadGPS_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.CurrentDirectory;
            dlg.Filter = "TXT(*.txt)|*.txt|GPS(*.GPS)|*.GPS|All Document(*.*)|*.*";
            dlg.Title = "Load GPS File";
            dlg.ShowReadOnly = true;
            dlg.Multiselect = true;

            gpsdata.Clear();

            List<string> strGPSData = new List<string>();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = textBox1.Text + "\r\n" + "Load GPS Data:" + "\r\n";

                Array.Sort(dlg.FileNames);
                foreach (string strFile in dlg.FileNames)
                {
                    //read all lines
                    StreamReader sr = new StreamReader(strFile, System.Text.Encoding.Default);
                    string strLine = "";
                    string strGPSLine = "";
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (strLine.Contains("#"))
                        {
                            if (strGPSLine != "")
                            {
                                strGPSData.Add(strGPSLine);
                            }
                            strGPSLine = "";
                        }
                        else
                        {
                            strGPSLine += strLine + "\r\n";
                        }
                        
                    }
                    if (strGPSLine != "")
                        strGPSData.Add(strGPSLine);

                    textBox1.Text = textBox1.Text + strFile + "\r\n";
                }

                ReadData read = new ReadData();

                foreach (string str in strGPSData)
                {
                    GPSdata gps = new GPSdata();
                    try
                    {
                        read.EncodeGPS(str, ref gps.UTC, ref gps.GGA_Latitude, ref gps.GGA_Longitude, ref gps.VTG_Vx, ref gps.VTG_Vy, ref gps.HDT_heading);
                        gpsdata.Add(gps);
                    }
                    catch
                    {
                    }
                }

            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            GetCalibrationResult();
        }

        private double GetCalibrationResult()
        {
            double dHeadingOffset = 0; //该单位为弧度
            int iEnsNumbers = rawdata.Count;

            ProfileResult calresult = new ProfileResult();
            try
            {
                List<float> fBoatVx_GPS = new List<float>();
                List<float> fBoatVy_GPS = new List<float>();

                for (int i = 0; i < iEnsNumbers; i++)
                {
                    fBoatVx_GPS.Add(gpsdata[i].VTG_Vx);
                    fBoatVy_GPS.Add(gpsdata[i].VTG_Vy);
                }

                //调用CGetHeadingOffset类，计算GPS艏向安装偏角
                CGetHeadingOffset headingOffset = new CGetHeadingOffset(rawdata, fBoatVx_GPS, fBoatVy_GPS);
                dHeadingOffset = headingOffset.dHeadingOffset / 180.0 * Math.PI; //调用CGetHeadingOffset类所得的角度，单位为度

                listView_DMG.Items.Clear();

                ListViewItem WAitem = new ListViewItem(headingOffset.dDMG_GPS.ToString("0.000"));
                WAitem.SubItems.Add(headingOffset.dDMG_Bottom.ToString("0.000"));
                WAitem.SubItems.Add((headingOffset.dDMG_Bottom / headingOffset.dDMG_GPS).ToString("0.000"));
                WAitem.SubItems.Add(((headingOffset.dDMG_Bottom - headingOffset.dDMG_GPS) / headingOffset.dDMG_GPS * 100).ToString("0.00") + "%");
                listView_DMG.Items.Add(WAitem);

                calResult.DMG_BT = headingOffset.dDMG_Bottom;
                calResult.DMG_GPS = headingOffset.dDMG_GPS;
                calResult.BT_GPS_DMG = headingOffset.dDMG_Bottom / headingOffset.dDMG_GPS;
                calResult.Accuracy_BT_GPS_DMG = (headingOffset.dDMG_Bottom - headingOffset.dDMG_GPS) / headingOffset.dDMG_GPS * 100;

                int iCount_BT = 0;
                int iCount_P = 0;
                int iCount_VTG = 0;
                double fSum_BT = 0;
                double fSum_P = 0;
                double fSum_VTG = 0;
                for (int i = 0; i < iEnsNumbers; i++)
                {
                    if (rawdata[i].B_Earth[0] < 88 && rawdata[i].B_Earth[1] < 88)
                    {
                        fSum_BT += Math.Sqrt(Math.Pow(rawdata[i].B_Earth[0], 2) + Math.Pow(rawdata[i].B_Earth[1], 2));
                        iCount_BT++;
                    }
                   
                    fSum_VTG += Math.Sqrt(Math.Pow(gpsdata[i].VTG_Vx, 2) + Math.Pow(gpsdata[i].VTG_Vy, 2));
                    iCount_VTG++;

                }

                if (iCount_BT > 0)
                    fSum_BT = fSum_BT / iCount_BT;

                if (iCount_VTG > 0)
                    fSum_VTG = fSum_VTG / iCount_VTG;

                listView_Cell.Items.Clear();

                for (int j = 0; j < rawdata[0].E_Cells; j++)
                {
                    iCount_P = 0;
                    fSum_P = 0;
                    if (checkedListBox_Cell.GetItemChecked(j))
                    {
                        for (int i = 0; i < iEnsNumbers; i++)
                        {
                            if (rawdata[i].Earth[0, j] < 88 && rawdata[i].Earth[1, j] < 88)
                            {
                                fSum_P += Math.Sqrt(Math.Pow(rawdata[i].Earth[0, j], 2) + Math.Pow(rawdata[i].Earth[1, j], 2));
                                iCount_P++;
                            }
                        }

                        if (iCount_P > 0)
                            fSum_P = fSum_P / iCount_P;

                        ListViewItem Vitem = new ListViewItem((j + 1).ToString());
                        Vitem.SubItems.Add(fSum_P.ToString("0.000"));
                        Vitem.SubItems.Add(fSum_BT.ToString("0.000"));

                        Vitem.SubItems.Add(fSum_VTG.ToString("0.00"));
                        Vitem.SubItems.Add(((fSum_P - fSum_BT) / fSum_BT * 100).ToString("0.00") + "%");
                        Vitem.SubItems.Add(((fSum_P - fSum_VTG) / fSum_VTG * 100).ToString("0.00") + "%");
                        listView_Cell.Items.Add(Vitem);

                        calresult.iBinNumber = j + 1;
                        calresult.Velocity_BT = fSum_BT;
                        calresult.Velocity_Profile = fSum_P;
                        calresult.Velocity_VTG = fSum_VTG;
                        calresult.Accuracy_P_BT = (fSum_P - fSum_BT) / fSum_BT * 100;
                        calresult.Accuracy_P_VTG = (fSum_P - fSum_VTG) / fSum_VTG * 100;

                        //calResult.profileResult.Add(calresult);
                        pResult.Add(calresult);
                    }
                }

            }
            catch
            {
            }
            return dHeadingOffset;
        }

        private void btnAll_Cal_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Cell.Items.Count; i++)
                checkedListBox_Cell.SetItemChecked(i, true);
        }

        private void btnCancel_Cal_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Cell.Items.Count; i++)
                checkedListBox_Cell.SetItemChecked(i, false);
        }

        private void btnExport_Calibrate_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "(*.csv)|*.csv|All File|*.*";
            saveFileDlg.Title = "保存:";

            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                string str = null;

                str += "底跟踪校准结果\r\n";
                str += "DMG GPS,DMG BT,DMG BT/GPS,(BT-GPS)/GPS\r\n";
                str += calResult.DMG_GPS.ToString("0.000") + "," + calResult.DMG_BT.ToString("0.000") + "," + calResult.BT_GPS_DMG.ToString("0.00") + "," + calResult.Accuracy_BT_GPS_DMG.ToString("0.00") + "\r\n\r\n";

                str += "水剖面校准结果\r\n";
                str += "层数,平均流速（水剖面）,平均流速（底跟踪）,平均流速（VTG）,(水剖面-底跟踪)/底跟踪,(水剖面-VTG)/VTG\r\n";
                //for (int i = 0; i < calResult.profileResult.Count; i++)
                //{
                //    str += calResult.profileResult[i].iBinNumber.ToString() + "," + calResult.profileResult[i].Velocity_Profile.ToString("0.000") + "," + calResult.profileResult[i].Velocity_BT.ToString("0.000") + "," + calResult.profileResult[i].Velocity_VTG.ToString("0.000") + "," + calResult.profileResult[i].Accuracy_P_BT.ToString("0.00") + "," + calResult.profileResult[i].Accuracy_P_VTG.ToString("0.00") + "\r\n";
                //}

                for (int i = 0; i < pResult.Count; i++)
                {
                    str += pResult[i].iBinNumber.ToString() + "," + pResult[i].Velocity_Profile.ToString("0.000") + "," + pResult[i].Velocity_BT.ToString("0.000") + "," + pResult[i].Velocity_VTG.ToString("0.000") + "," + pResult[i].Accuracy_P_BT.ToString("0.00") + "," + pResult[i].Accuracy_P_VTG.ToString("0.00") + "\r\n";
                }

                FileStream fs = new FileStream(saveFileDlg.FileName, FileMode.Create);

                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("gb2312"));

                sw.Write(str);

                sw.Flush();
                sw.Close();
                fs.Close();

                MessageBox.Show("输出完成");
            }
        }

    }

    public class CalibrateResult
    {
        //public List<ProfileResult> profileResult;
        public double DMG_BT;
        public double DMG_GPS;
        public double BT_GPS_DMG;
        public double Accuracy_BT_GPS_DMG;

    }

    public struct ProfileResult
    {
        public int iBinNumber;
        public double Velocity_Profile;
        public double Velocity_BT;
        public double Velocity_VTG;
        public double Accuracy_P_BT;
        public double Accuracy_P_VTG;
    }
}
