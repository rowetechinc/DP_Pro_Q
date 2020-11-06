using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ADCP
{
    public class DefaultCfg
    {
        
        //public struct DefCFG
        //{
         
        //    public string FileName;         //工程名
        //    public string EnsembleNum;      //
        //    public string Unit;            //单位
        //    public string ShowProfiler;    //显示剖面个数
        //    public string WaterSpeedRef;   //流速参考
        //    public string BoatSpeedRef;    //船速参考
        //    public string HeadingRef;      //艏向参考
        //    public string Mode;            //模式
        //    public string DepthInWater;    //换能器入水深度
        //    public string ADCP_SerialPort;  //ADCP串口号
        //    public string ADCP_Baudrate;    //ADCP波特率
          
        //    public string GPS_SerialPort;
        //    public string GPS_Baudrate;

        //    public Configurations.Configuration ProfessorCfg; //专家模式的配置信息
        //}
   public string Language;         //语言
        //public DefCFG DefCfgInf;

        public DefaultCfg()
        {
            
        }

        public void GetDefaultCfg()//在初始化时，读取配置信息，主要用于获取语言，以及在测量模式下，获取默认的配置
        {
            string strPath = Directory.GetCurrentDirectory() + "\\Default.CFG";
            if (File.Exists(strPath))
            {
                try
                {
                    StreamReader sr = new StreamReader(strPath);
                    string cmd = sr.ReadLine();
                    string[] cmdPart = new string[2];
                    while (cmd != null)
                    {
                        cmdPart = cmd.Split(' ');
                        switch (cmdPart[0])
                        {
                            case "Language":
                                {
                                    Language = cmdPart[1];
                                    break;
                                }
                            //case "ProjectName":
                            //    {
                            //        DefCfgInf.FileName = cmdPart[1];
                            //        break;
                            //    }
                            //case "EnsembleNum":
                            //    {
                            //        DefCfgInf.EnsembleNum = cmdPart[1];
                            //        break;
                            //    }
                            //case "Unit":
                            //    {
                            //        DefCfgInf.Unit = cmdPart[1];
                            //        break;
                            //    }
                            //case "ShowProfiler":
                            //    {
                            //        DefCfgInf.ShowProfiler = cmdPart[1];
                            //        break;
                            //    }
                            //case "WaterSpeedRef":
                            //    {
                            //        DefCfgInf.WaterSpeedRef = cmdPart[1];
                            //        break;
                            //    }
                            //case "BoatSpeedRef":
                            //    {
                            //        DefCfgInf.BoatSpeedRef = cmdPart[1];
                            //        break;
                            //    }
                            //case "HeadingRef":
                            //    {
                            //        DefCfgInf.HeadingRef = cmdPart[1];
                            //        break;
                            //    }
                            //case "ADCP_PortName":
                            //    {
                            //        DefCfgInf.ADCP_SerialPort = cmdPart[1];
                            //        break;
                            //    }
                            //case "ADCP_BaudRate":
                            //    {
                            //        DefCfgInf.ADCP_Baudrate = cmdPart[1];
                            //        break;
                            //    }
                            //case "Mode":
                            //    {
                            //        DefCfgInf.Mode = cmdPart[1];
                            //        break;
                            //    }
                            //case "DepthInWater":
                            //    {
                            //        DefCfgInf.DepthInWater = cmdPart[1];
                            //        break;
                            //    }
                           
                            default:
                                break;
                        }
                        cmd = sr.ReadLine();
                    }
                }
                catch //(System.Exception e)
                {
                    //Language = "Chinese";

                    Language = "English";

                    //DefCfgInf.ADCP_SerialPort = "COM1";
                    //DefCfgInf.ADCP_Baudrate = "115200";
                    //DefCfgInf.FileName = "Project Name";
                    //DefCfgInf.EnsembleNum = "50";
                    //DefCfgInf.BoatSpeedRef = "Bottom";
                    //DefCfgInf.DepthInWater = "0.1";
                    //DefCfgInf.HeadingRef = "Internal";
                    //DefCfgInf.Mode = "User,1";
                    //DefCfgInf.ShowProfiler = "0";
                    //DefCfgInf.Unit = "Metric";
                    //DefCfgInf.WaterSpeedRef = "Software";

                }
            }
            else
            {
                Language = "Chinese";
                //DefCfgInf.ADCP_SerialPort = "COM1";
                //DefCfgInf.ADCP_Baudrate = "115200";
                //DefCfgInf.FileName = "Project Name";
                //DefCfgInf.EnsembleNum = "50";
                //DefCfgInf.BoatSpeedRef = "Bottom";
                //DefCfgInf.DepthInWater = "0.1";
                //DefCfgInf.HeadingRef = "Internal";
                //DefCfgInf.Mode = "User,1";
                //DefCfgInf.ShowProfiler = "0";
                //DefCfgInf.Unit = "Metric";
                //DefCfgInf.WaterSpeedRef = "Software";
              
            }
        }

        public void WriteToCfg()
        {
            string strPath=Directory.GetCurrentDirectory() + "\\Default.CFG";
        
            WriteToFile(strPath);
           
        }

        private void WriteToFile(string strPath)
        {
            File.WriteAllText(strPath, "Language " + Language + "\r\n");
            //File.AppendAllText(strPath, "ProjectName " + DefCfgInf.FileName + "\r\n");
            //File.AppendAllText(strPath, "EnsembleNum " + DefCfgInf.EnsembleNum + "\r\n");
            //File.AppendAllText(strPath, "Unit " + DefCfgInf.Unit + "\r\n");

            //File.AppendAllText(strPath, "ShowProfiler " + DefCfgInf.ShowProfiler + "\r\n");
            //File.AppendAllText(strPath, "WaterSpeedRef " + DefCfgInf.WaterSpeedRef + "\r\n");
            //File.AppendAllText(strPath, "BoatSpeedRef " + DefCfgInf.BoatSpeedRef + "\r\n");
            //File.AppendAllText(strPath, "HeadingRef " + DefCfgInf.HeadingRef + "\r\n");

            //File.AppendAllText(strPath, "ADCP_PortName " + DefCfgInf.ADCP_SerialPort + "\r\n");
            //File.AppendAllText(strPath, "ADCP_BaudRate " + DefCfgInf.ADCP_Baudrate + "\r\n");

            //File.AppendAllText(strPath, "Mode " + DefCfgInf.Mode + "\r\n");
            //File.AppendAllText(strPath, "DepthInWater " + DefCfgInf.DepthInWater + "\r\n");

        }
    }
}
