using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    public partial class CDecodeGPS
    {
        //从NMEA数据中提取strType类型的字符串，例如strType="GPGGA"||"GPVTG",则单独提取strType行的字符串
        //输入：
        //strType为提取数据的类型，例如：GPGGA,GPVTG,GPHDT,GPROT等
        //data 为NMEA格式的GPS数据
        //输出：strBuffer，与strType类型相匹配的字符串
        public void GPSNMEA_decode(string data, string strType, ref string strBuffer) //LPJ 2013-11-18
        {
            string[] split = data.Split('$');
            bool bTrue = false;
            for (int i = 0; i < split.Count(); i++)
            {
                string[] split2 = split[i].Split(',');
                string str = split2[0];

                if (strType == str)
                {
                    strBuffer = split[i];
                    bTrue = true;
                }

                if (bTrue)
                    break;
            }
        }

        ////解析地球坐标
        ////输入：
        ////earthCoor为地球坐标，格式为DDDMM.sssss，其中DDD为度，MM为分，sssss为秒
        ////返回参数：
        ////Degree为度
        ////Min为分
        ////Sec为秒
        //public void TransferEarthCoordinate(string earthCoor, ref int Degree, ref int Min, ref int Sec)
        //{
        //    string[] str_Lat = earthCoor.Split('.');
        //    int sec = int.Parse(str_Lat[1].Substring(0, 3));
        //    int degree_Lat = int.Parse(str_Lat[0]);
        //    Degree = degree_Lat / 100;
        //    Min = degree_Lat - Degree * 100;
        //    Sec = (int)(sec / 1000.0f * 60);

        //}

        /// <summary>
        /// 解析GGA中的经纬度坐标
        /// </summary>
        /// <param name="earthCoor">earthCoor为地球坐标，格式为DDDMM.mmmm，其中DDD为度，MM.mmmm为分</param>
        /// <param name="Degree">度</param>
        /// <param name="Min">分</param>
        /// <param name="Sec">秒</param>
        public void TransferEarthCoordinate(double earthCoor, ref int Degree, ref int Min, ref double Sec)
        {
            int degree_Lat = (int)earthCoor;
            Degree = degree_Lat / 100;
            Min = degree_Lat - Degree * 100;
            Sec = (earthCoor - degree_Lat) * 60;

        }

        //解析GGA数据
        //输入：
        //data为GPGGA数据
        //输出：
        //GPS_Time为采集数据时间
        //GPS_Longitude为string类型的经度，GPS_Latitude为string类型的纬度
        //GPS_NS为string类型的South或North，GPS_EW为string类型的West或East
        public void GPS_GGAdecode(string data,ref string GPS_Time,ref string GPS_Longitude,ref string GPS_Latitude,ref string GPS_NS,ref string GPS_EW, ref string GPS_Quailty)
        {
            try
            {
                string[] split1; string GGA_str1;
                if (data.Contains('$'))
                {
                    split1 = data.Split('$');
                    GGA_str1 = split1[1];
                }
                else
                {
                    GGA_str1 = data;
                }
                if (GGA_str1 != "")
                {
                    string[] gga1 = GGA_str1.Split(',');
                    GPS_Time = gga1[1];
                    GPS_Latitude = gga1[2];
                    GPS_NS = gga1[3];
                    GPS_Longitude = gga1[4];
                    GPS_EW = gga1[5];
                    GPS_Quailty = gga1[6];
                }
                else
                {
                    GPS_Latitude = "0000.000";
                    GPS_Longitude = "00000.000";
                    GPS_Quailty = "";
                }
            }
            catch
            {
                GPS_Latitude = "0000.000";
                GPS_Longitude = "00000.000";
                GPS_Quailty = "";
            }
        }

        ///// <summary>
        ///// 解析GGA数据
        ///// </summary>
        ///// <param name="data">GPGGA数据</param>
        ///// <param name="GPS_Time">UTC时间</param>
        ///// <param name="GPS_Longitude">GPS_Longitude为float类型的经度</param>
        ///// <param name="GPS_Latitude">GPS_Latitude为float类型的纬度</param>
        //public void GPS_GGAdecode(string data, ref float GPS_Time, ref float GPS_Longitude, ref float GPS_Latitude)
        //{
        //    try
        //    {
        //        string[] split1 = data.Split('$');
        //        string GGA_str1 = split1[1];
        //        string[] gga1 = GGA_str1.Split(',');
        //        GPS_Time = float.Parse(gga1[1]);

        //        GPS_Latitude = float.Parse(gga1[2]);
        //        if (gga1[3] == "S")
        //            GPS_Latitude = -1 * GPS_Latitude;

        //        GPS_Longitude = float.Parse(gga1[4]);
        //        if (gga1[4] == "W")
        //            GPS_Longitude = -1 * GPS_Longitude;

        //    }
        //    catch
        //    {
        //        GPS_Latitude = 0;
        //        GPS_Longitude = 0;
        //    }
        //}

        //解析VTG数据
        //输入：
        //data为GPVTG数据
        //输出：
        //GPS_Vx为GPS航行速度的X分量，单位为m/s
        //GPS_Vy为GPS航行速度的Y分量，单位为m/s
        public void GPS_VTGdecode(string data, ref float GPS_BoatSpeed, ref double GPS_Angle, ref float GPS_Vx, ref float GPS_Vy)
        {
            try
            {
                if (data != "")
                {
                    string[] split1; string VTG_str;
                    if (data.Contains('$'))
                    {
                        split1 = data.Split('$');
                        VTG_str = split1[1];
                    }
                    else
                    {
                        VTG_str = data;
                    }

                    //string[] split2 = data.Split('$');
                    //string VTG_str = split2[1];
                    string[] vtg1 = VTG_str.Split(',');
                    GPS_Angle = double.Parse(vtg1[1]) * Math.PI / 180.0f;                     //提取运动角度，真北方向,单位:度,转为弧度
                    GPS_BoatSpeed = float.Parse(vtg1[5]) * 1.852f * 1000.0f / 3600.0f;                 // 提取水平运动速度数据，单位：节，转为:m/s

                    GPS_Vx = (float)(GPS_BoatSpeed * Math.Sin(GPS_Angle));
                    GPS_Vy = (float)(GPS_BoatSpeed * Math.Cos(GPS_Angle));
                }
                else
                {
                    GPS_Angle = 0;
                    GPS_BoatSpeed = 0;
                    GPS_Vx = 0;
                    GPS_Vy = 0;
                }
            }
            catch
            {
                GPS_Angle = 0;                  
                GPS_BoatSpeed = 0;
                GPS_Vx = 0;
                GPS_Vy = 0;
            }
        }

        ////解析HDT数据
        ////输入：
        ////data为GPHDT数据
        ////输出：
        ////GPS_HDT为string类型的HDT航向
        //public void GPS_HDTdecode(string data,ref string GPS_HDT)
        //{
        //    try
        //    {
        //        string[] split3 = data.Split('$');   
        //        string HDT_str = split3[1];          
        //        string[] HDT1 = HDT_str.Split(',');    
        //        GPS_HDT = HDT1[1];
        //    }
        //    catch
        //    {
        //        GPS_HDT = "0.0";
        //    }
        //}

        /// <summary>
        /// 解析HDT数据
        /// </summary>
        /// <param name="data">GPHDT数据</param>
        /// <param name="GPS_HDT">为float类型的HDT航向</param>
        public void GPS_HDTdecode(string data, ref float GPS_HDT)
        {
            try
            {
                string[] split1; string HDT_str;
                if (data.Contains('$'))
                {
                    split1 = data.Split('$');
                    HDT_str = split1[1];
                }
                else
                {
                    HDT_str = data;
                }

                //string[] split3 = data.Split('$');
                //string HDT_str = split3[1];
                string[] HDT1 = HDT_str.Split(',');
                GPS_HDT = float.Parse(HDT1[1]);
            }
            catch
            {
                GPS_HDT = 0;
            }
        }

        //解析ROT数据
        //输入：
        //data为GPROT数据
        //输出：
        //GPS_ROT为float类型的ROT值  
        public void GPS_ROTdecode(string data,ref float GPS_ROT)
        {
            try
            {
                string[] split3 = data.Split('$');   
                string ROT_str = split3[1];           
                string[] ROT1 = ROT_str.Split(',');
                GPS_ROT = float.Parse(ROT1[1]);
            }
            catch
            {
            }
        }

        //解析VXVY数据
        //输入：
        //data为VXVY数据
        //输出：
        //GPS_VX为X方向的分量
        //GPS_VY为Y方向分量
        public void GPS_VXVYdecode(string data,ref float GPS_VX,ref float GPS_VY)
        {
            try
            {
                string[] split3 = data.Split('$');   
                string VX_str = split3[1];          
                string[] VX = VX_str.Split(',');    
             
                GPS_VX = (float)Convert.ToDecimal( VX[1]);
                GPS_VY = (float)Convert.ToDecimal( VX[2]);
            }
            catch
            {
                GPS_VX = 0;
                GPS_VY = 0;
            }
        }

        /// <summary>
        /// 解析GPS UTC时间，该时间格式为hhmmss.sss
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float GPS_Timedecode(string data)
        {
            float fGPStime = 0;
            if (data != "")
            {
                float time = float.Parse(data);

                int iHour = (int)(time / 10000);
                int iMinute = (int)((time - iHour * 10000) / 100);
                float fSecond = time - iHour * 10000 - iMinute * 100;
                fGPStime = iHour * 3600 + iMinute * 60 + fSecond;
            }
            return fGPStime;
        }
    }
}
