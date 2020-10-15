using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CTrailPermission;

namespace ADCP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                //LPJ 2013-7-24 软件不能同时多个运行
                bool bAppRunning = false;
                System.Threading.Mutex mutex = new System.Threading.Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out bAppRunning);
                if (!bAppRunning)
                {
                    MessageBox.Show(Resource1.String245);
                    Application.Exit();
                }
                else
                {
                    //CheckTrialPermission check = new CheckTrialPermission();
                    //check.iTRIALDAYS = 30;   //设置使用期限
                    //check.strAPPLICATION = "DP_Pro_Q";  //设置注册表文件名

                    //if (check.CheckTrialDate()) //判断试用期限
                    //{
                    //    MessageBox.Show(check.iLEFTDAYS.ToString("0") + " days left of trial.", "Welcome to " + check.strAPPLICATION, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Application.Run(new Form1());
                    //}
                    //else
                    //{
                    //    Application.Exit();
                    //    string strMessage = "The trial period expired.\r\nPlease logon our official site for more information: www.pan-comm.com, or dial 021-34060911";
                    //    MessageBox.Show(strMessage, "Welcome to " + check.strAPPLICATION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    Application.Run(new Form1());
                }
            }
            catch(Exception e)
            {
                //MessageBox.Show("程序执行错误! 请退出程序重新开始。\r\n" +
                //"工程测量：请检查各串行口情况；\r\n回放数据可能丢失，其他原始数据保留。"+e.Message);//LPJ 2012-4-24
                MessageBox.Show(Resource1.String73 + "\r\n" +
               Resource1.String74 + "\r\n" + Resource1.String75 + e.Message);
            }
         }
    }
}
