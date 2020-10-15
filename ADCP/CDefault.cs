using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace ADCP
{
    public class CDefault
    {
        #region 发送DEFAULT命令，并读取该配置 2016-4-7

        SerialPort sp;
        string strData = null;

        public CDefault(SerialPort serialPort)
        {
            sp = new SerialPort(serialPort.PortName, serialPort.BaudRate);
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
        }
   
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] pack = new byte[sp.BytesToRead];
            sp.Read(pack, 0, pack.Length);

            strData += Encoding.Default.GetString(pack);
        }
      
        /// <summary>
        /// 发送CDEFAULT命令，并读取CSHOW中的配置参数
        /// </summary>
        /// <param name="iSystemNo">当iSystemNo=0，表示为主系统频率</param>
        /// <param name="advanced"></param>
        /// <returns></returns>
        public bool SendCDEFULATCommand(int iSystemNo, ref CAdvancedCfg.AdvancedConfiguration advanced)
        {
            try
            {
                if (!sp.IsOpen)
                {
                    sp.Open();
                }

                strData = "";
                sp.Write("CDEFAULT" + '\r');
                Thread.Sleep(300);
                sp.Write("CSHOW" + '\r');
                Thread.Sleep(300);

                GetDefaultConfiguration(strData, iSystemNo, ref advanced);

                sp.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取仪器默认配置
        /// </summary>
        /// <param name="advanced"></param>
        private void GetDefaultConfiguration(string strCfg, int iSystemNo, ref CAdvancedCfg.AdvancedConfiguration advanced)
        {
            string[] strCmd = strCfg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string strLine in strCmd)
            {
                string[] cmdPart = strLine.Split(new string[] {"[0]" }, StringSplitOptions.RemoveEmptyEntries);

                if (strLine.Contains("CEI"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = str0[1].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    advanced.EsmbIntervalHH = int.Parse(str[0]);
                    advanced.EsmbIntervalMM = int.Parse(str[1]);
                    advanced.EsmbIntervalSShh = float.Parse(str[2]);
                    continue;
                }
                if (strLine.Contains("CBI")) //LPJ 2016-4-8
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    string[] str1 = str[0].Split(':');
                    advanced.BurstInterval_HH = int.Parse(str1[0]);
                    advanced.BurstInterval_MM = int.Parse(str1[1]);
                    advanced.BurstInterval_SS = float.Parse(str1[2]);

                    advanced.BurstInterval_n = int.Parse(str[1]);  //LPJ 2013-2-4 由于之前的版本没有该值，显示为异常，因此，这里设为0，不再提取该值。
                    //if (str[1] == "System.Windows.Forms.TextBox")
                    //    txt_BurstInterval_n.Text = "0";
                    //if (!ValidateUserInput(advanced.BurstInterval_n.ToString(), 5))
                    //    advanced.BurstInterval_n = 0;

                    continue;
                }
                if (strLine.Contains("CWPON"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    if (0 == int.Parse(str[0]))
                        advanced.WaterPingOpen = false;
                    //else if ("1" == cmdPart0[iSystemNo])
                    else
                        advanced.WaterPingOpen = true;
                    continue;
                }
                if (strLine.Contains("CWPBB"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    //LPJ 2012-10-19 add --start

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.WaterProfilerMode = int.Parse(str[0]);

                    try
                    {
                        advanced.WPLagLengthV = float.Parse(str[1]);
                    }
                    catch
                    {
                        advanced.WPLagLengthV = 0.042f;
                    }
                    continue;
                    //LPJ 2012-10-19 add --end
                }
                if (strLine.Contains("CWPAP"))  //LPJ 2013-1-31
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.CWPAP1 = float.Parse(str[0]);
                    advanced.CWPAP2 = float.Parse(str[1]);
                    advanced.CWPAP3 = float.Parse(str[2]);
                    advanced.CWPAP4 = float.Parse(str[3]);
                    advanced.CWPAP5 = float.Parse(str[4]);
                    continue;
                }
                if (strLine.Contains("CWPST"))  //LPJ 2012-10-19 add --start
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.WPST_Correlation = float.Parse(str[0]);
                    advanced.WPST_QVelocity = float.Parse(str[1]);
                    advanced.WPST_VVelocity = float.Parse(str[2]);
                    continue;
                }         //LPJ 2012-10-19 add --end
                if (strLine.Contains("CWPBL"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WPBlankSize = float.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CWPBS"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WPBinSize = float.Parse(cmdPart0[iSystemNo]);
                    continue;
                }//////
                if (strLine.Contains("CWPX"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.WPWaterXmt = float.Parse(str[0]);
                    continue;
                }
                if (strLine.Contains("CWPBN"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WPBinNum = int.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CWPP"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WPWaterAvgNum = int.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CWPBP"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.CWPBP_Pings = int.Parse(str[0]);
                    advanced.CWPBP_Time = float.Parse(str[1]);
                    continue;
                }
                if (strLine.Contains("CWPTBP"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WPTimeBtwnPings = float.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CBTON"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    if (0 == int.Parse(cmdPart0[iSystemNo]))
                        advanced.BtmTrkOpen = false;
                    //else if ("1" == cmdPart0[iSystemNo])
                    else
                        advanced.BtmTrkOpen = true;
                    continue;
                }
                if (strLine.Contains("CBTBB"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    //LPJ 2012-10-19 add --start
                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.BTMode = int.Parse(str[0]);

                    try
                    {
                        advanced.BTPulseLag = float.Parse(str[1]);
                        advanced.BTLongRangeDepth = float.Parse(str[2]);
                    }
                    catch
                    {
                        advanced.BTPulseLag = 0.0f;
                        advanced.BTLongRangeDepth = 30.0f;
                    }

                    continue;
                    //LPJ 2012-10-19 add --end
                }
                if (strLine.Contains("CBTST"))    //LPJ 2012-10-19 add --start
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = cmdPart0[iSystemNo].Split(',');
                    advanced.BTST_Correlation = float.Parse(str[0]);
                    advanced.BTST_QV = float.Parse(str[1]);
                    advanced.BTST_V = float.Parse(str[2]);  //LPJ 2012-10-19 add

                    continue;
                }
                if (strLine.Contains("CBTTBP"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.BtmTrkInterval = float.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CBTT"))
                {
                    string[] strName = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if ("CBTT" == strName[0])
                    {
                        string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                        string[] str = cmdPart0[iSystemNo].Split(',');
                        advanced.BTT_SNRshallow = float.Parse(str[0]);
                        advanced.BTT_Depthshallow2deep = float.Parse(str[1]);
                        advanced.BTT_SNRdeep = float.Parse(str[2]);
                        advanced.BTT_Depthlow2high = float.Parse(str[3]);  //LPJ 2012-10-19 add
                        continue;
                    }
                }   //LPJ 2012-10-19 add --end
                if (strLine.Contains("CBTBL"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.BtmTrkBlank = float.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CBTMX"))
                {
                    string[] cmdPart0 = cmdPart[1].Split(new string[] { "[1]" }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.BtmTrkDepth = float.Parse(cmdPart0[iSystemNo]);
                    continue;
                }
                if (strLine.Contains("CWSSC")) //LPJ 2013-1-31
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = str0[1].Split(',');

                    advanced.CWSCCwaterTemperature = float.Parse(str[0]);
                    advanced.CWSCCTransducerDepth = float.Parse(str[1]);
                    advanced.CWSCCSalinity = float.Parse(str[2]);
                    advanced.CWSCCSpeedOfSound = float.Parse(str[3]);
                    continue;
                }
                if (strLine.Contains("CWSS"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.SoundSpeed = float.Parse(str0[1]);
                    continue;
                }
                if (strLine.Contains("CWS"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WaterSalinity = float.Parse(str0[1]);
                    continue;
                }
                if (strLine.Contains("CWT"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.WaterTemperature = float.Parse(str0[1]);
                    continue;
                }
                if (strLine.Contains("CTD"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.TransducerDepth = float.Parse(str0[1]);
                    continue;
                }
                if (strLine.Contains("CHO"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string[] str = str0[1].Split(',');

                    advanced.HeadingOffset = float.Parse(str[0]);
                    continue;
                }
                if (strLine.Contains("C232B"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.R232 = float.Parse(str0[1]);
                    continue;
                }
                if (strLine.Contains("C485B"))
                {
                    string[] str0 = cmdPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    advanced.R485 = float.Parse(str0[1]);
                    continue;
                }

            }
        }

       #endregion

    }
}
