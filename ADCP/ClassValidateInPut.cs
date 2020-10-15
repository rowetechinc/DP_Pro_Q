using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ADCP
{
    class ClassValidateInPut
    {
        public bool ValidateUserInput(String value, int kind)
        {
            string RegularExpressions = null;
            switch (kind)
            {
                case 1: //数字
                    RegularExpressions = "^[0-9]*$"; break;
                case 2: //整数
                    RegularExpressions = "^-?\\d+$"; break;
                case 3: //正整数
                    RegularExpressions = "^[0-9]*[1-9][0-9]*$"; break;
                case 4: //负整数
                    RegularExpressions = "^-[0-9]*[1-9][0-9]*$"; break;
                case 5: //正整数，0
                    RegularExpressions = "^\\d+$"; break;
                case 6: //负整数，0
                    RegularExpressions = "^((-\\d+)|(0+))$"; break;
                case 7: //浮点数
                    RegularExpressions = "^(-?\\d+)(\\.\\d+)?$"; break;
                case 8: //正浮点数
                    RegularExpressions = "^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$"; break;
                case 9: //负浮点数
                    RegularExpressions = "^(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))$"; break;
                case 10: //正浮点数，0
                    RegularExpressions = "^\\d+(\\.\\d+)?$"; break;
                case 11: //负浮点数，0
                    RegularExpressions = "^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$"; break;
                case 12:
                    RegularExpressions = @"^([0-9]+|[0-9]+\.{0,1}[0-9]*)$"; break;
                default: break;
            }
            Match m = Regex.Match(value, RegularExpressions);
            if (m.Success) 
                return true; 
            else
                return false;
        }

        public bool ValidateCharInput(char KeyChar,string strText)
        {
            //IsNumber：指定字符串中位于指定位置的字符是否属于数字类别 
            //IsPunctuation：指定字符串中位于指定位置的字符是否属于标点符号类别 
            //IsControl：指定字符串中位于指定位置的字符是否属于控制字符类别 
            bool bHandled = false;
            if (!Char.IsNumber(KeyChar) && !Char.IsPunctuation(KeyChar) && !Char.IsControl(KeyChar) && (KeyChar != 45))
            {
                bHandled = true;
            }
            else if (Char.IsPunctuation(KeyChar))
            {
                if (KeyChar == '.')
                {
                    if (strText.LastIndexOf('.') != -1)
                    {
                        bHandled = true;
                    }
                }
                else if (KeyChar == '-')
                {
                    if (strText.Length != 0)
                    {
                        bHandled = true;
                    }
                }
                else
                {
                    bHandled = true;
                }
            }
            return bHandled;
        }

        public bool ValidateFloatInput(char KeyChar, string strText)
        {
            //IsNumber：指定字符串中位于指定位置的字符是否属于数字类别 
            //IsPunctuation：指定字符串中位于指定位置的字符是否属于标点符号类别 
            //IsControl：指定字符串中位于指定位置的字符是否属于控制字符类别 
            bool bHandled = false;
            if (!Char.IsNumber(KeyChar) && !Char.IsPunctuation(KeyChar) && !Char.IsControl(KeyChar) && (KeyChar != 45))
            {
                bHandled = true;
            }
            else if (Char.IsPunctuation(KeyChar))
            {
                if (KeyChar == '.')
                {
                    if (strText.LastIndexOf('.') != -1)
                    {
                        bHandled = true;
                    }
                }
                else if (KeyChar != 45)
                    bHandled = true;
                //else
                //{
                //    bHandled = true;
                //}
            }
            return bHandled;
        }

    }
}
