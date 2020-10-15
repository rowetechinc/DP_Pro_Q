using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;

namespace ADCP
{
    public partial class FormNewADCP_Project : Form
    {
        DP300_Windows MainForm;
        public FormNewADCP_Project(DP300_Windows form)
        {
            InitializeComponent();
            MainForm = form;
            FileName.Text = MainForm.OldProjectName;  //LPJ 2012-6-13
        }
        public string filepath;  //LPJ 2012-6-13
        private void buttonProjectName_Click(object sender, EventArgs e)
        {
            //try
            //{
                MainForm.ProjectName = FileName.Text;
                MainForm.OldProjectName = FileName.Text;  //LPJ 2012-6-13
                DateTime dt = DateTime.Now;
                //string datePatt = @"yyyyMMddHHmmss";  //LPJ 2013-4-16
                string datePatt = @"yyyyMMdd_HHmmss";  //LPJ 2013-4-16 在日期与时间之间增加一个短下划线
                //MainForm.ProjectName += dt.ToString(datePatt);
               // MainForm.ProjectName = MainForm.ProjectName+"_"+dt.ToString(datePatt); //LPJ 2012-9-24 在输入的工程名与时间之间增加一个短下划线，以方便看  //LPJ 2013-4-16 在工程名和时间之间添加一个序号

                MainForm.ProjectFullName = MainForm.ProjectName + "_" + (MainForm.iStartMeasQ / 2 + 1).ToString("000") + "_" + dt.ToString(datePatt); 
                
                MainForm.newPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dp300Data", MainForm.ProjectFullName);
                filepath = MainForm.newPath; //LPJ 2012-6-13
                System.IO.Directory.CreateDirectory(MainForm.newPath + "\\PlaybackData");
                System.IO.Directory.CreateDirectory(MainForm.newPath + "\\EnsemblesSet");
                System.IO.Directory.CreateDirectory(MainForm.newPath + "\\rawData");
                System.IO.Directory.CreateDirectory(MainForm.newPath + "\\GPS");
                System.IO.Directory.CreateDirectory(MainForm.newPath + "\\SysCfg");
                MainForm.projectHasStarted = true;
                //MessageBox.Show("Project Started");
                //MainForm.projectInfotextBox.Text = "工程名：" + FileName.Text + "\r\n";  // LPJ 2012-4-24
                //MainForm.projectInfotextBox.Text += "创建时间：" + dt.ToString() + "\r\n";
                //MainForm.projectInfotextBox.Text += "文件路径：" + MainForm.newPath + "\r\n";
              
                
            //}
            //catch (System.Exception ee)
            //{
            //    return false;
            //}
            //return true;
            string tt ;
            //tt = "提示：\r\n本次工程名："+ FileName.Text + "\r\n创建时间： "+ dt.ToString() + "\r\n文件路径：" + MainForm.newPath + "\r\n"
            //    +"\r\n下一步请选择《通讯》页，设置 ADCP串口 和 GPS串口。";// LPJ 2012-4-24
            tt = Resource1.String19+":\r\n"+Resource1.String71 + FileName.Text + "\r\n"+Resource1.String22 + dt.ToString() + "\r\n"+Resource1.String23 + MainForm.newPath + "\r\n"
                + "\r\n"+Resource1.String72;
            //MessageBox.Show(tt);  //LPJ 2013-4-15 取消新建项目后的显示对话框
        }

       
        /// LPJ 2012-4-24 
        System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(FormNewADCP_Project));

        //////LPJ 2012-4-24 
        /// 应用资源
        /// ApplyResources 的第一个参数为要设置的控件
        ///                  第二个参数为在资源文件中的ID，默认为控件的名称
        public void ApplyResource()
        {
            GetControl(this.Controls);

            //Caption
            res.ApplyResources(this, "$this");
        }

        ////LPJ 2012-4-24 遍历所有控件
        public void GetControl(Control.ControlCollection ctl)
        {
            try
            {
                foreach (Control ctl1 in ctl)
                {
                    res.ApplyResources(ctl1, ctl1.Name);
                    if (ctl1.HasChildren)
                    {
                        GetControl(ctl1.Controls);
                    }
                }
            }
            catch { }
        }

        ////LPJ 2012-4-24 判断语言类型
        public void ChooseLanguage()
        {
            if (Form1.Language_ZH == false)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                ApplyResource();
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh");
                ApplyResource();
            }
        }
    }
}
