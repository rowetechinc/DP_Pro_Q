using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    public class TransferGPS2Distance
    {
        /**
  * google maps的脚本里代码
  */
        /**
        * 根据两点间经纬度坐标（double值），计算两点间距离，单位为米
        */
        private const double EARTH_RADIUS = 6378.137; //地球曲率半径,单位为km

        /// <summary>
        /// 度转为弧度
        /// </summary>
        /// <param name="d">度</param>
        /// <returns>弧度</returns>
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 根据两点将的经纬度，计算两点距离，单位为米
        /// </summary>
        /// <param name="lat1">纬度A，单位度</param>
        /// <param name="lng1">经度A，单位度</param>
        /// <param name="lat2">纬度B，单位度</param>
        /// <param name="lng2">经度B，单位度</param>
        /// <returns>距离</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS * 1000;
           
            //double radLat1 = rad(lat1);
            //double radLat2 = rad(lat2);
            //double b = rad(lng1) - rad(lng2);
            //double s = Math.Acos(Math.Sin(radLat2) * Math.Sin(radLat1) + Math.Cos(radLat2) * Math.Cos(radLat1) * Math.Cos(b));
            //s = s * EARTH_RADIUS * 1000;
           
            return s;
        }

        /// <summary>
        /// 根据经纬度计算方向，单位弧度
        /// </summary>
        /// <param name="lat_a"></param>
        /// <param name="lng_a"></param>
        /// <param name="lat_b"></param>
        /// <param name="lng_b"></param>
        /// <returns></returns>
        public static double GetDirection(double lat_a, double lng_a, double lat_b, double lng_b)
        {
            double d = 0;

            lat_a = rad(lat_a);
            lng_a = rad(lng_a);
            lat_b = rad(lat_b);
            lng_b = rad(lng_b);

            d = Math.Sin(lat_a) * Math.Sin(lat_b) + Math.Cos(lat_a) * Math.Cos(lat_b) * Math.Cos(lng_a - lng_b);
            if ((1 - d * d) < 0)
                d = 0;
            else
                d = Math.Sqrt(1 - d * d);
            if (d < 1e-10)  //LPJ 2016-12-15
            {
                //if (Math.Abs(lat_a - lat_b) < 1e-15)
                //{
                //    if (lng_a <= lng_b)
                //        d = 0;
                //    else
                //        d = 180;
                //}

                //if (Math.Abs(lng_a - lng_b) < 1e-15)
                //{
                //    if (lat_a < lat_b)
                //        d = 90;
                //    else
                //        d = 180;
                //}
                d = 0;
            }
            else
            {
                d = Math.Cos(lat_b) * Math.Sin(lng_a - lng_b) / d;
                if (Math.Abs(d) > 1)  //LPJ 2017-1-3 当纬度相同时，会出现该值大于1
                {
                    if (d > 0)
                        d = Math.Asin(1) * 180 / Math.PI;
                    else
                        d = Math.Asin(-1) * 180 / Math.PI;
                }
                else
                    d = Math.Asin(d) * 180 / Math.PI;
            }

            //判断象限,一象限不变，3、4象限用180-a，第二象限+360

            if (lat_a <= lat_b && lng_a > lng_b) //二象限， 当lat相同，lnga>lngb时，为第二象限   //LPJ 2017-5-25 
                d = 180 - d;
            else if (lat_a < lat_b && lng_a <= lng_b) //三象限，当lng相同，lata>latb时，为第三象限
                d = 180 - d;
            else if (lat_a > lat_b && lng_a < lng_b) //四象限
                d = 360 + d;


            //最后，转为弧度
            d = d * Math.PI / 180;

            return d;
        }

        public static double RotateX(double x, double y, double degree)
        {
            double rotate = 0;
            degree = rad(degree);
            rotate=  y * Math.Sin(degree) + x * Math.Cos(degree);
            return rotate;
        }

        public static double RotateY(double x, double y, double degree)
        {
            double rotate = 0;
            degree = rad(degree);
            rotate = y * Math.Cos(degree) - x * Math.Sin(degree);
            return rotate;
        }

    }
}
