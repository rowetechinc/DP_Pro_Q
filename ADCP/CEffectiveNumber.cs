using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    class CEffectiveNumber
    {
        /// <summary>
        /// 保留有效位数
        /// </summary>
        /// <param name="value">原始数据</param>
        /// <param name="precision">有效位数</param>
        /// <param name="dec">小数位</param>
        /// <returns>返回数值</returns>
        public static string EffectiveNumber(double value, int precision, int dec)
        {
            string strResult = "";
            int n = 0;

            if (value < 1)
                n = (int)Math.Log10(1.0 / Math.Abs(value));  //当数值小于1时，计算第一个有效数字前的零的个数

            if (value < 1 && n > 0)
            {
                strResult = Math.Round(value, dec).ToString(); //小数位
            }
            else
            {
                strResult = value.ToString("G" + precision.ToString()); //有效数字
               
            }
            return strResult;
        }
    }
}
