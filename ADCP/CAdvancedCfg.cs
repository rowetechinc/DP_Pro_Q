using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ADCP
{
    public class CAdvancedCfg
    {
        public class AdvancedConfiguration
        {
            public bool ADCPMode;//mode,ADCP/DVL

            public int EsmbIntervalHH, EsmbIntervalMM;
            public float EsmbIntervalSShh;//时间平均步长

            public int BurstInterval_HH, BurstInterval_MM;//Burst Interval
            public float BurstInterval_SS;
            public int BurstInterval_n;

            public bool WaterPingOpen; //水跟踪开关
            public int WaterProfilerMode; //水跟踪带宽，宽带、窄带、脉冲相干等
            public float WPLagLengthV; //LagLength
            public float CWPAP1, CWPAP2, CWPAP3, CWPAP4, CWPAP5;
            public float WPST_Correlation, WPST_QVelocity, WPST_VVelocity; //Water Profile Screening Thresholds
            public float WPBlankSize;//盲区
            public float WPBinSize;//单元尺寸
            public float WPWaterXmt;//脉冲长度
            public int WPBinNum;//单元层数
            public int WPWaterAvgNum;//呯数
            //public int WaterAvgIntervalHH, WaterAvgIntervalMM; //LPJ 2013-9-27 该命令已在I版本中取消
            //public float WaterAvgIntervalSShh;//平均间隔
            public float WPTimeBtwnPings;//呯间隔
            public int CWPBP_Pings;   //LPJ 2013-10-30 water base pings(used when CWPBP=0 or 1), pings a value of 2 to 100 sets the number of pings that will be averaged together during each CWPP ping.
            public float CWPBP_Time;   //LPJ 2013-10-30 time between base pings(seconds)

            public bool BtmTrkOpen;//底跟踪开关
            public int BTMode;//底跟踪带宽
            public float BTPulseLag, BTLongRangeDepth; //pulse to pulse lag，
            public float BTST_Correlation, BTST_QV, BTST_V;  //LPJ 2012-10-19 add
            public float BTT_SNRshallow, BTT_Depthshallow2deep, BTT_SNRdeep, BTT_Depthlow2high;  //
            public float BtmTrkBlank;//盲区
            public float BtmTrkDepth;//深度
            public float BtmTrkInterval;//呯间隔

            public bool WaterTrkOpen;//水参考开关
            public int WTMode;//水参考带宽
            public float WTBlankSize;//盲区
            public float WTBinSize;//单元尺寸
            public float WTInterval;//呯间隔

            public float CWSCCwaterTemperature, CWSCCTransducerDepth, CWSCCSalinity, CWSCCSpeedOfSound;//Water Speed Of Sound Control 
            public float WaterSalinity; //盐度
            public float WaterTemperature; //水温
            public float TransducerDepth;//换能器深度
            public float SoundSpeed;//声速
            public float HeadingOffset;//艏向偏角
            public float R232; //波特率
            public float R485;

        }

        public void WriteAdvancedModeToFile(string fileName, AdvancedConfiguration Advancedstruct) //主要用于将高级模式的参数写入last配置中，在回放文件中不用显示该配置
        {
            if (Advancedstruct.ADCPMode)
                File.AppendAllText(fileName, "CPROFILE" + "\r\n");
            else
                File.AppendAllText(fileName, "CDVL" + "\r\n");
            File.AppendAllText(fileName, "CEI " + Advancedstruct.EsmbIntervalHH.ToString() + ":" + Advancedstruct.EsmbIntervalMM.ToString() + ":" + Advancedstruct.EsmbIntervalSShh.ToString() + "\r\n");
            File.AppendAllText(fileName, "CBI " + Advancedstruct.BurstInterval_HH.ToString() + ":" + Advancedstruct.BurstInterval_MM.ToString() + ":" + Advancedstruct.BurstInterval_SS.ToString() + "," + Advancedstruct.BurstInterval_n.ToString() + "\r\n");
            if (Advancedstruct.WaterPingOpen)
            {
                File.AppendAllText(fileName, "CWPON 1" + "\r\n");
            }
            else
            {
                File.AppendAllText(fileName, "CWPON 0" + "\r\n");
            }

            File.AppendAllText(fileName, "CWPBB " + Advancedstruct.WaterProfilerMode.ToString() + "," + Advancedstruct.WPLagLengthV.ToString() + "\r\n");  //LPJ 2012-10-19 add
            if (0 == Advancedstruct.WaterProfilerMode || 1 == Advancedstruct.WaterProfilerMode)
            {
                File.AppendAllText(fileName, "CWPBP " + Advancedstruct.CWPBP_Pings.ToString() + "," + Advancedstruct.CWPBP_Time.ToString() + "\r\n"); //LPJ 2013-10-30
            }

            File.AppendAllText(fileName, "CWPAP " + Advancedstruct.CWPAP1.ToString() + "," + Advancedstruct.CWPAP2.ToString() + "," + Advancedstruct.CWPAP3.ToString() + "," + Advancedstruct.CWPAP4.ToString() + "," + Advancedstruct.CWPAP5.ToString() + "\r\n"); //LPJ 2013-1-31 CWPAP
            File.AppendAllText(fileName, "CWPST " + Advancedstruct.WPST_Correlation.ToString() + "," + Advancedstruct.WPST_QVelocity.ToString() + "," + Advancedstruct.WPST_VVelocity.ToString() + "\r\n"); //LPJ 2012-10-19 add
            File.AppendAllText(fileName, "CWPBL " + Advancedstruct.WPBlankSize.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWPBS " + Advancedstruct.WPBinSize.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWPX " + Advancedstruct.WPWaterXmt.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWPBN " + Advancedstruct.WPBinNum.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWPP " + Advancedstruct.WPWaterAvgNum.ToString() + "\r\n");
            //File.AppendAllText(fileName, "CWPAI " + systemSet.Advancedstruct.WaterAvgIntervalHH.ToString() + ":" + systemSet.Advancedstruct.WaterAvgIntervalMM.ToString() + ":" + systemSet.Advancedstruct.WaterAvgIntervalSShh.ToString() + "\r\n"); //LPJ 2013-9-27 该命令已在I版本中取消
            File.AppendAllText(fileName, "CWPTBP " + Advancedstruct.WPTimeBtwnPings.ToString() + "\r\n");
            if (Advancedstruct.BtmTrkOpen)
            {
                File.AppendAllText(fileName, "CBTON 1" + "\r\n");
            }
            else
            {
                File.AppendAllText(fileName, "CBTON 0" + "\r\n");
            }

            File.AppendAllText(fileName, "CBTBB " + Advancedstruct.BTMode.ToString() + "," + Advancedstruct.BTPulseLag.ToString() + "," + Advancedstruct.BTLongRangeDepth.ToString() + "\r\n"); //LPJ 2012-10-19 add
            File.AppendAllText(fileName, "CBTST " + Advancedstruct.BTST_Correlation.ToString() + "," + Advancedstruct.BTST_QV.ToString() + "," + Advancedstruct.BTST_V.ToString() + "\r\n");  //LPJ 2012-10-19 add
            File.AppendAllText(fileName, "CBTT " + Advancedstruct.BTT_SNRshallow.ToString() + "," + Advancedstruct.BTT_Depthshallow2deep.ToString() + "," + Advancedstruct.BTT_SNRdeep.ToString() + "," + Advancedstruct.BTT_Depthlow2high.ToString() + "\r\n");  //LPJ 2012-10-19 add
            File.AppendAllText(fileName, "CBTBL " + Advancedstruct.BtmTrkBlank.ToString() + "\r\n");
            File.AppendAllText(fileName, "CBTMX " + Advancedstruct.BtmTrkDepth.ToString() + "\r\n");
            File.AppendAllText(fileName, "CBTTBP " + Advancedstruct.BtmTrkInterval.ToString() + "\r\n");
            if (Advancedstruct.WaterTrkOpen)
            {
                File.AppendAllText(fileName, "CWTON 1" + "\r\n");
            }
            else
            {
                File.AppendAllText(fileName, "CWTON 0" + "\r\n");
            }
            File.AppendAllText(fileName, "CWTBB " + Advancedstruct.WTMode.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWTBL " + Advancedstruct.WTBlankSize.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWTBS " + Advancedstruct.WTBinSize.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWTTBP " + Advancedstruct.WTInterval.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWSSC " + Advancedstruct.CWSCCwaterTemperature.ToString() + "," + Advancedstruct.CWSCCTransducerDepth.ToString() + "," + Advancedstruct.CWSCCSalinity.ToString() + "," + Advancedstruct.CWSCCSpeedOfSound.ToString() + "\r\n");//LPJ 2013-1-31
            //File.AppendAllText(fileName, "CWS " + Advancedstruct.WaterSalinity.ToString() + "\r\n"); //LPJ 2014-6-16 cancel
            File.AppendAllText(fileName, "CWT " + Advancedstruct.WaterTemperature.ToString() + "\r\n");
            //File.AppendAllText(fileName, "CTD " + Advancedstruct.TransducerDepth.ToString() + "\r\n");
            File.AppendAllText(fileName, "CWSS " + Advancedstruct.SoundSpeed.ToString() + "\r\n");
            //File.AppendAllText(fileName, "CHO " + Advancedstruct.HeadingOffset.ToString() + "\r\n"); //LPJ 2014-6-16 cancel
            File.AppendAllText(fileName, "C232B " + Advancedstruct.R232.ToString() + "\r\n");
            File.AppendAllText(fileName, "C485B " + Advancedstruct.R485.ToString() + "\r\n");
        }

        public void GetFileToAdvancedMode(string fileName, ref AdvancedConfiguration advanced) // LPJ 2013-8-2 读取配置文件
        {
            StreamReader sr = new StreamReader(fileName);
            string cmd = sr.ReadLine();
            string[] cmdPart = new string[2];
            while (cmd != null)
            {
                cmdPart = cmd.Split(' ');
                switch (cmdPart[0])
                {
                    case "CDVL":
                        {
                            advanced.ADCPMode = false;
                            break;
                        }
                    case "CPROFILE":
                        {
                            advanced.ADCPMode = true;
                            break;
                        }
                    case "CEI":
                        {
                            string[] str = cmdPart[1].Split(':');
                            advanced.EsmbIntervalHH = int.Parse(str[0]);
                            advanced.EsmbIntervalMM = int.Parse(str[1]);
                            advanced.EsmbIntervalSShh = float.Parse(str[2]);
                            break;
                        }
                    case "CBI": //LPJ 2013-1-31
                        {
                            string[] str = cmdPart[1].Split(',');
                            string[] str1 = str[0].Split(':');
                            advanced.BurstInterval_HH = int.Parse(str1[0]);
                            advanced.BurstInterval_MM = int.Parse(str1[1]);
                            advanced.BurstInterval_SS = float.Parse(str1[2]);

                            try
                            {
                                advanced.BurstInterval_n = int.Parse(str[1]);  //LPJ 2013-2-4 由于之前的版本没有该值，显示为异常，因此，这里设为0，不再提取该值。
                            }
                            catch
                            {
                                advanced.BurstInterval_n = 0;
                            }

                            break;
                        }
                    case "CWPON":
                        {
                            if ("0" == cmdPart[1])
                                advanced.WaterPingOpen = false;
                            else if ("1" == cmdPart[1])
                                advanced.WaterPingOpen = true;
                            break;
                        }
                    case "CWPBB":
                        {

                            //LPJ 2012-10-19 add --start

                            string[] str = cmdPart[1].Split(',');
                            advanced.WaterProfilerMode = int.Parse(str[0]);

                            try
                            {
                                advanced.WPLagLengthV = float.Parse(str[1]);
                            }
                            catch
                            {
                                advanced.WPLagLengthV = 0.042f;
                            }
                            break;
                            //LPJ 2012-10-19 add --end
                        }
                    case "CWPBP":    //LPJ 2013-10-30 添加CWPBP命令
                        {
                            string[] str = cmdPart[1].Split(',');
                            try
                            {
                                advanced.CWPBP_Pings = int.Parse(str[0]);
                                advanced.CWPBP_Time = float.Parse(str[1]);
                            }
                            catch
                            {
                            }
                            break;
                        }
                    case "CWPAP": //LPJ 2013-1-31
                        {
                            string[] str = cmdPart[1].Split(',');
                            advanced.CWPAP1 = float.Parse(str[0]);
                            advanced.CWPAP2 = float.Parse(str[1]);
                            advanced.CWPAP3 = float.Parse(str[2]);
                            advanced.CWPAP4 = float.Parse(str[3]);
                            advanced.CWPAP5 = float.Parse(str[4]);
                            break;
                        }
                    case "CWPST":  //LPJ 2012-10-19 add --start
                        {
                            string[] str = cmdPart[1].Split(',');
                            advanced.WPST_Correlation = float.Parse(str[0]);
                            advanced.WPST_QVelocity = float.Parse(str[1]);
                            advanced.WPST_VVelocity = float.Parse(str[2]);
                            break;
                        }         //LPJ 2012-10-19 add --end
                    case "CWPBL":
                        {
                            advanced.WPBlankSize = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWPBS":
                        {
                            advanced.WPBinSize = float.Parse(cmdPart[1]);
                            break;
                        }//////
                    case "CWPX":
                        {
                            advanced.WPWaterXmt = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWPBN":
                        {
                            advanced.WPBinNum = int.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWPP":
                        {
                            advanced.WPWaterAvgNum = int.Parse(cmdPart[1]);
                            break;
                        }
                    //case "CWPAI": //LPJ 2013-9-27 该命令已在I版本中取消
                    //    {
                    //        string[] str = cmdPart[1].Split(':');
                    //        advanced.WaterAvgIntervalHH = int.Parse(str[0]);
                    //        advanced.WaterAvgIntervalMM = int.Parse(str[1]);
                    //        advanced.WaterAvgIntervalSShh = float.Parse(str[2]);
                    //        break;
                    //    }
                    case "CWPTBP":
                        {
                            advanced.WPTimeBtwnPings = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CBTON":
                        {
                            if ("0" == cmdPart[1])
                                advanced.BtmTrkOpen = false;
                            else if ("1" == cmdPart[1])
                                advanced.BtmTrkOpen = true;
                            break;
                        }
                    case "CBTBB":
                        {
                            //LPJ 2012-10-19 add --start
                            string[] str = cmdPart[1].Split(',');
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

                            break;
                            //LPJ 2012-10-19 add --end
                        }
                    case "CBTST":   //LPJ 2012-10-19 add --start
                        {
                            string[] str = cmdPart[1].Split(',');
                            advanced.BTST_Correlation = float.Parse(str[0]);
                            advanced.BTST_QV = float.Parse(str[1]);
                            advanced.BTST_V = float.Parse(str[2]);  //LPJ 2012-10-19 add

                            break;
                        }
                    case "CBTT":
                        {
                            string[] str = cmdPart[1].Split(',');
                            advanced.BTT_SNRshallow = float.Parse(str[0]);
                            advanced.BTT_Depthshallow2deep = float.Parse(str[1]);
                            advanced.BTT_SNRdeep = float.Parse(str[2]);
                            advanced.BTT_Depthlow2high = float.Parse(str[3]);  //LPJ 2012-10-19 add
                            break;
                        }    //LPJ 2012-10-19 add --end
                    case "CBTBL":
                        {
                            advanced.BtmTrkBlank = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CBTMX":
                        {
                            advanced.BtmTrkDepth = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CBTTBP":
                        {
                            advanced.BtmTrkInterval = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWTON":
                        {
                            if ("0" == cmdPart[1])
                                advanced.WaterTrkOpen = false;
                            else if ("1" == cmdPart[1])
                                advanced.WaterTrkOpen = true;
                            break;
                        }
                    case "CWTBB":
                        {
                            advanced.WTMode = int.Parse(cmdPart[1]);
                            break;
                        }//////////////
                    case "CWTBL":
                        {
                            advanced.WTBlankSize = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWTBS":
                        {
                            advanced.WTBinSize = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWTTBP":
                        {
                            advanced.WTInterval = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "CWSSC"://LPJ 2013-1-31
                        {
                            string[] str = cmdPart[1].Split(',');

                            advanced.CWSCCwaterTemperature = float.Parse(str[0]);
                            advanced.CWSCCTransducerDepth = float.Parse(str[1]);
                            advanced.CWSCCSalinity = float.Parse(str[2]);
                            advanced.CWSCCSpeedOfSound = float.Parse(str[3]);
                            break;
                        }
                    //case "CWS":
                    //    {
                    //        advanced.WaterSalinity = float.Parse(cmdPart[1]);
                    //        break;
                    //    }
                    case "CWT":
                        {
                            advanced.WaterTemperature = float.Parse(cmdPart[1]);
                            break;
                        }
                    //case "CTD":
                    //    {
                    //        advanced.TransducerDepth = float.Parse(cmdPart[1]);
                    //        break;
                    //    }
                    case "CWSS":
                        {
                            advanced.SoundSpeed = float.Parse(cmdPart[1]);
                            break;
                        }
                    //case "CHO":
                    //    {
                    //        advanced.HeadingOffset = float.Parse(cmdPart[1]);
                    //        break;
                    //    }
                    case "C232B":
                        {
                            advanced.R232 = float.Parse(cmdPart[1]);
                            break;
                        }
                    case "C485B":
                        {
                            advanced.R485 = float.Parse(cmdPart[1]);
                            break;
                        }
                }
                cmd = sr.ReadLine();
            }
            sr.Close(); //LPJ 2013-8-5

        }

    }

  
}
