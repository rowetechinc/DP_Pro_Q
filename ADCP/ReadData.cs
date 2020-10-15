using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PickAndDecode;
using Calcflow;
using DP_Pro_Calibration;

namespace DP_Pro_Calibration
{
    public class ReadData
    {
        /// <summary>
        /// 解析bin文件中的GPS数据
        /// </summary>
        /// <param name="str">NEMA数据</param>
        /// <param name="GGA_Latitude">纬度</param>
        /// <param name="GGA_Longitude">经度</param>
        /// <param name="VTG_Vx">X方向VTG速度</param>
        /// <param name="VTG_Vy">Y方向VTG速度</param>
        /// <param name="HDT_heading">HDT艏向</param>
        public void EncodeGPS(string str,ref float UTC, ref float GGA_Latitude, ref float GGA_Longitude, ref float VTG_Vx, ref float VTG_Vy, ref float HDT_heading)
        {
            string[] strSplit=new string[100];
            strSplit = str.Split('$');

            float hdt_last = -1; //LPJ 2015-8-26 记录上一个时刻的heading，当两个时刻间的heading差值大于20时，不采用
            float hdt_heading = 0;
            float vtg_vx = 0;
            float vtg_vy = 0;

            int iHeadingNum = 0;
            int iVTGNum = 0;
            int iGGANum = 0;

            foreach (string strData in strSplit)
            {
                //解析GGA数据，将第一个GGA数据作为可用数据
                if (strData.Contains("GPGGA") && iGGANum == 0)
                {
                    try
                    {
                        string[] tokens = new string[10];
                        tokens = strData.Split(',');

                        UTC = float.Parse(tokens[1]);
                        GGA_Latitude = float.Parse(tokens[2]);
                        if (tokens[3] == "S")
                            GGA_Latitude = -1 * GGA_Latitude;

                        GGA_Longitude = float.Parse(tokens[4]);
                        if (tokens[5] == "W")
                            GGA_Longitude = -1 * GGA_Longitude;
                    
                        iGGANum++;
                    }
                    catch
                    {
                    }
                }
                else if (strData.Contains("GPVTG")) //解析VTG数据，将vtg流速做平均后，作为可用数据
                {
                    try
                    {
                        string[] tokens = new string[10];
                        tokens = strData.Split(',');

                        double gpsAngle = float.Parse(tokens[1]) * Math.PI / 180.0f;
                        double gpsShipSpeed = float.Parse(tokens[5]) * 1.852f * 1000.0f / 3600.0f;

                        vtg_vx += (float)(gpsShipSpeed * Math.Sin(gpsAngle));
                        vtg_vy += (float)(gpsShipSpeed * Math.Cos(gpsAngle));

                        iVTGNum++;
                    }
                    catch
                    {
                    }
                }
                else if (strData.Contains("GPHDT")) //解析HDT数据，将艏向做平均后，作为可用数据
                {
                    try
                    {
                        string[] tokens = new string[10];
                        tokens = strData.Split(',');

                        float heading = float.Parse(tokens[1]);

                        if (hdt_last < 0)
                            hdt_last = heading;

                        if (Math.Abs(heading - hdt_last) < 20)
                        {
                            hdt_heading += heading;
                            iHeadingNum++;
                            hdt_last = heading;
                        }
                       
                    }
                    catch
                    {
                    }
                }
            }

            if (iVTGNum > 0)
            {
                VTG_Vx = vtg_vx / iVTGNum;
                VTG_Vy = vtg_vy / iVTGNum;
            }
            if (iHeadingNum > 0)
            {
                HDT_heading = hdt_heading / iHeadingNum;
            }
            else
            {
                HDT_heading = -1;
            }
        }

        public void ReadGPS_HDT(string str,ref float[] heading,ref int number)
        {
            string[] tokens = new string[100];//定义一个数组用于接收
            string Line;
            number=0;

            FileStream ifs = new FileStream(str, FileMode.Open);//打开数据文件
            StreamReader sr = new StreamReader(ifs);

            while ((Line = sr.ReadLine()) != null)
            {
                tokens = Line.Split(',');//用空格分割读取的数据,并存入tokens

                //获取GPS坐标
                if (tokens[0] == "$GPHDT")
                {
                    heading[number++] = float.Parse(tokens[1]);
                    continue;
                }
            }
        }

        public void ReadGPS_EN(string str, ref float[] East,ref float[] North,ref int number)
        {
            string[] tokens = new string[100];//定义一个数组用于接收
            string Line;
            number = 0;

            FileStream ifs = new FileStream(str, FileMode.Open);//打开数据文件
            StreamReader sr = new StreamReader(ifs);

            while ((Line = sr.ReadLine()) != null)
            {
                tokens = Line.Split(',');//用空格分割读取的数据,并存入tokens

                //获取GPS坐标
                if (tokens[0] == "$GPGGA")
                {
                    North[number] = float.Parse(tokens[2]);
                    East[number++] = float.Parse(tokens[4]);
                    continue;
                }
            }

        }

        public void ReadGPS_VTG(string str, ref double[] gps_Bx, ref double[] gps_By,ref int number)
        {
            string[] tokens = new string[100];//定义一个数组用于接收
            string Line;
            number = 0;

            double gpsAngle;
            double gpsShipSpeed;

            FileStream ifs = new FileStream(str, FileMode.Open);//打开数据文件
            StreamReader sr = new StreamReader(ifs);

            while ((Line = sr.ReadLine()) != null)
            {
                tokens = Line.Split(',');//用空格分割读取的数据,并存入tokens

               
                if (tokens[0] == "$GPVTG")
                {
                    gpsAngle = float.Parse(tokens[1])* Math.PI / 180.0f;
                    gpsShipSpeed = float.Parse(tokens[5]) * 1.852f * 1000.0f / 3600.0f;

                    gps_Bx[number] = gpsShipSpeed * Math.Sin(gpsAngle);
                    gps_By[number++] = gpsShipSpeed * Math.Cos(gpsAngle);

                    continue;
                }
            }

        }

    }

    public class GPSdata
    {
        public float UTC;
        public float GGA_Latitude;

        public float GGA_Longitude;
        public float VTG_Vx;
        public float VTG_Vy;
        public float HDT_heading;
    }

    public class calibrateResult
    {
        public int iBinNumber;
        public float Velocity_Profile;
        public float Velocity_BT;
        public float Velocity_VTG;
        public float Accuracy_P_BT;
        public float Accuracy_P_VTG;

    }
}
