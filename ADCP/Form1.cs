/**********************************************************************************************************************/
/* 注释： 2011-7-25 起                                                                                                */
/* 2011-7-25, 增加经纬度显示                                                                                          *、
/**********************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
//using Aladdin.HASP;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Diagnostics;
using DP_Pro_Calibration;//写入文件

#pragma warning disable IDE1006
#pragma warning disable IDE0017

/************************************************************************************************************************/
/*       Form1.cs 的功能是处理 menu 的选项                                                                              */
/************************************************************************************************************************/


namespace ADCP
{
    public partial class Form1 : Form
    {
        public class NoPaintBackGroundPanel1 : Panel
        {
            private static System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Image Im = ((System.Drawing.Image)(resources.GetObject("Form1.BackgroundImage")));
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                e.Graphics.DrawImage(Im, this.ClientRectangle);
            }
        }

        public Form1()
        {
            InitializeComponent();

            string DP300LastCfgpath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dp300Data", "LastTimeCfg");
            System.IO.Directory.CreateDirectory(DP300LastCfgpath);

            //LPJ 2014-6-18 read the system language 
            try
            {
                string strLanguage = System.Globalization.CultureInfo.CurrentCulture.Name;
                if ("zh-CN" == strLanguage)
                {
                    Language_ZH = true;
                    ChineseDefault();
                }
                else
                {
                    Language_ZH = false;
                    englishDefault();
                }
            }
            catch
            {
                Language_ZH = false;
                englishDefault();
            }

            getcfg = new DefaultCfg();//LPJ 2013-5-19 获取默认配置信息
            getcfg.GetDefaultCfg();
            if ("English" == getcfg.Language)
            {
                Language_ZH = false;
                //englishDefault(); //LPJ 2013-6-7
            }
            else
            {
                Language_ZH = true;
                //ChineseDefault(); //LPJ 2013-6-28
            }

        }

        public DefaultCfg getcfg;

        private DP300_Windows DP300_wnd;
        public static bool bUnLockKeyDog = false;  //If bUnLockKeyDog is ture, unlock the keydog!  JZH

        private void panel4_Click(object sender, EventArgs e)
        {
            //string Title = "关于...";
            //string text = "ADCP 系列软件 v2.0" + "\n" +
            //              "版权(C)2010-2011 上海泛际科学仪器有限公司" + "\n" +
            
            //              "版权所有。"; 
            //MessageBox.Show(text, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //LPJ 2012-4-20
            string Text = Resource1.str_aboutText1 + "\r\n" + Resource1.str_aboutText2 + "\r\n" + "\r\n" + Resource1.str_aboutText5;
            MessageBox.Show(Text, Resource1.str_About, MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }

        private void AboutLable_Click(object sender, EventArgs e)
        {
            //string Title = "关于...";
            //string text = "ADCP 系列软件 v2.0" + "\n" +
            //              "版权(C)2010-2011 上海泛际科学仪器有限公司" + "\n" +
           
            //              "版权所有。";
            //MessageBox.Show(text, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //LPJ 2012-4-20
            string Text = Resource1.str_aboutText1 + "\r\n" + Resource1.str_aboutText2 + "\r\n" + Resource1.str_aboutText5;
            MessageBox.Show(Text, Resource1.str_About, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool Language_ZH = true;
        public static bool bSurveyMode = true;  //LPJ 2016-12-14
   
        private void englishDefault()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture
                = System.Globalization.CultureInfo.GetCultureInfo("en");
           
            Language_ZH = false;
            ApplyResource(); //LPJ 2013-5-29 更新DP300界面的语言
           
        }
        private void ChineseDefault()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture
               = System.Globalization.CultureInfo.GetCultureInfo("zh");
            Language_ZH = true;
            ApplyResource(); //LPJ 2013-5-29 更新DP300界面的语言
           
        }

     
        //////LPJ 2012-4-20 
        /// 应用资源
        /// ApplyResources 的第一个参数为要设置的控件
        ///                  第二个参数为在资源文件中的ID，默认为控件的名称
        ///     
        
        System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(Form1));        
        private void ApplyResource()
        {
            //菜单
           
            //foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            //{
            //    res.ApplyResources(item, item.Name);
            //    try
            //    {
            //        foreach (ToolStripMenuItem subItem in item.DropDownItems)
            //        {
            //            res.ApplyResources(subItem, subItem.Name);
            //            if (subItem.HasDropDownItems)
            //            {
            //                try
            //                {
            //                    foreach (ToolStripMenuItem subItem1 in subItem.DropDownItems)
            //                    {
            //                        res.ApplyResources(subItem1, subItem1.Name);
            //                    }
            //                }
            //                catch { }
            //            }
            //        }

            //        GetControl(this.Controls);
            //    }
            //    catch
            //    { }
            //}

            GetControl(this.Controls);

            foreach (ToolStripItem item in this.toolStrip1.Items) //LPJ 2013-6-8
            {
                try
                {
                    res.ApplyResources(item, item.Name);
                 
                }
                catch
                {
                }
            }

            //Caption
            res.ApplyResources(this, "$this");

            ToolStripMenuItem_Manual.Text = Resource1.String246; //LPJ 2013-7-24
            ToolStripMenuItem_RTIWeb.Text = Resource1.String247;
            ToolStripMenuItem_PanCommWeb.Text = Resource1.String248;

        }

        public void GetControl(Control.ControlCollection ctl)
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

        public void btnSurveying_Click(object sender, EventArgs e)
        {
            bSurveyMode = true;  //LPJ 2016-12-14
            //CLockKeyDog unlockKeydog = new CLockKeyDog();
            //if (unlockKeydog.UnLockKeyDog(201))
            //{
                DP300_wnd = new DP300_Windows(); //LPJ 2013-5-23

                panelPanComm.Visible = false;
                panelRTI.Visible = false;
                panel2.Visible = false;

                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                btnAbout.Visible = false;
                btnLanguage.Visible = false;
                btnSurveying.Visible = false;
                btnPlayback.Visible = false;
                btnExit.Visible = false;
                btnManual.Visible = false;

                DP300_wnd.MdiParent = this;      //LPJ 2013-5-23
                DP300_wnd.Dock = System.Windows.Forms.DockStyle.Fill;  //LPJ 2013-5-23

                if (DP300_wnd.RealDisPlay_Click(null, null))
                {
                    toolStrip1.Visible = true;
                    toolStripBtnPlayback.Visible = false;
                    toolStripBtnNext.Visible = false; //LPJ 2013-9-24

                    toolStripBtnStartEdge.Visible = false;
                    toolStripBtnMoving.Visible = false;
                    toolStripBtnEndEdge.Visible = false;
                    toolStripBtnStop.Visible = false;
                    toolStripBtnSummary.Visible = false;
                    toolStripBtnLanguage.Visible = false; //LPJ 2013-6-9 
                    toolStripBtnSurvey.Visible = false; //LPJ 2013-6-14
                    toolStripBtnStartPings.Visible = true;
                    toolStripBtnSetting.Visible = false; //LPJ 2013-6-24
                    toolStripBtnStopPings.Visible = false; //LPJ 2016-1-5
                    toolStripBtnStart.Visible = false;  //LPJ 2016-1-5

                    toolStripBtnStatusG.Visible = false; //LPJ 2013-8-6
                    toolStripBtnStatusR.Visible = false; //LPJ 2013-8-6
                    toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6

                    toolStripLabelTip.Visible = true; //LPJ 2016-12-19

                    //this.FormBorderStyle = FormBorderStyle.Sizable;

                    //DP300_wnd.MdiParent = this;      //LPJ 2013-5-23
                    //DP300_wnd.Dock = System.Windows.Forms.DockStyle.Fill;  //LPJ 2013-5-23
                    //DP300_wnd.defCfg.DefCfgInf = getcfg.DefCfgInf;
                    //DP300_wnd.Show();

                    this.FormBorderStyle = FormBorderStyle.Sizable;

                    DP300_wnd.Show();

                    if (DP300_wnd.bConnect)
                    {
                        toolStripBtnStart.Enabled = true;

                        toolStripBtnStatusR.Visible = true; //LPJ 2013-8-6 

                        //LPJ 2013-7-26 提示
                        Point pnt = new Point();
                        pnt.X = this.toolStrip1.Location.X + 10;
                        pnt.Y = this.toolStrip1.Location.Y + 10;
                        toolTip1.Show(Resource1.String258, this, pnt, 15000);
                    }
                    else
                    {
                        toolStripBtnStart.Enabled = false;

                        toolStripBtnStatusR.Visible = false; //LPJ 2013-8-6

                    }
                }
                else
                {
                    panelPanComm.Visible = true;
                    panelRTI.Visible = true;
                    panel2.Visible = true;

                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;

                    btnAbout.Visible = true;
                    btnLanguage.Visible = true;
                    btnSurveying.Visible = true;
                    btnPlayback.Visible = true;
                    btnExit.Visible = true;
                    //btnManual.Visible = true;

                    DP300_wnd.bClose = true;
                }
            //}
            //else
            //{
            //    MessageBox.Show(Resource1.String304);
            //}
        }

        private int dPageNumbers = 0;
     //   private int dPageIndex = 0;
        private void btnPlayback_Click(object sender, EventArgs e)
        {
            try
            {
                bSurveyMode = false;  //LPJ 2016-12-14
                //CLockKeyDog unlockKeydog = new CLockKeyDog();
                //if (unlockKeydog.UnLockKeyDog(202))
                {
                    DP300_Windows dp300_wnd = new DP300_Windows(); //LPJ 2013-5-23

                    panelPanComm.Visible = false;
                    panelRTI.Visible = false;
                    panel2.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;

                    btnAbout.Visible = false;
                    btnLanguage.Visible = false;
                    btnSurveying.Visible = false;
                    btnPlayback.Visible = false;
                    btnExit.Visible = false;
                    btnManual.Visible = false;

                    toolStrip1.Visible = true;
                    toolStripBtnSetting.Visible = false;
                    toolStripBtnSurvey.Visible = false;
                    toolStripBtnStart.Visible = false;
                    toolStripBtnStartEdge.Visible = false;
                    toolStripBtnMoving.Visible = false;
                    toolStripBtnEndEdge.Visible = false;
                    toolStripBtnStop.Visible = false;
                    toolStripBtnLanguage.Visible = false; //LPJ 2013-6-9 

                    toolStripBtnStopPings.Visible = false; //LPJ 2016-1-5
                    toolStripBtnStartPings.Visible = false; //LPJ 2016-1-5


                    toolStripBtnSummary.Visible = true;
                    toolStripBtnPlayback.Visible = true;
                    toolStripBtnNext.Visible = true;    //LPJ 2013-9-25

                    toolStripBtnStatusG.Visible = false; //LPJ 2013-8-6
                    toolStripBtnStatusR.Visible = false; //LPJ 2013-8-6
                    toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
                    toolStripLabelTip.Visible = false; //LPJ 2013-9-26

                    tabControl1.Dock = System.Windows.Forms.DockStyle.Fill; //LPJ 2013-6-17

                    dp300_wnd.MdiParent = this;     //LPJ 2013-5-23
                    dp300_wnd.Dock = System.Windows.Forms.DockStyle.Fill;  //LPJ 2013-5-23
                    //DP300_wnd.defCfg.DefCfgInf = getcfg.DefCfgInf;

                    dp300_wnd.TopLevel = false; //LPJ 2013-5-28 添加多个工程
                    this.tabControl1.Visible = true;
                    this.tabControl1.TabPages.Clear();

                    //LPJ 2013-6-14 在这里新建一个tabPage页
                    TabPage tabpage11 = new TabPage();
                    tabControl1.TabPages.Add(tabpage11);
                    tabpage11.Controls.Add(dp300_wnd);//LPJ 2013-5-28
                    dPageNumbers++;

                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    dp300_wnd.Show();

                    dp300_wnd.PalyBackBtn_Click(null, null);

                    #region 船速参考图标 2016-8-22
                    if (0 == dp300_wnd.iVesselSpeedRef)
                        toolStripDropDownBtnVesselRef.Image = pictureBoxBT.Image;
                    else if (1 == dp300_wnd.iVesselSpeedRef)
                        toolStripDropDownBtnVesselRef.Image = pictureBoxGPS.Image;
                    else if ( 3 == dp300_wnd.iVesselSpeedRef)
                        toolStripDropDownBtnVesselRef.Image = pictureBoxGGA.Image;
                    else
                        toolStripDropDownBtnVesselRef.Image = pictureBoxNull.Image;
                    #endregion

                    //OceanADCPPlaybackToolStripMenuItem_Click(sender, e);
                    if (!dp300_wnd.playBackMode)
                    {
                        tabControl1.TabPages.Remove(tabpage11);
                        dPageNumbers--;
                    }
                    else
                    {
                        try
                        {
                            int x = dp300_wnd.PathStr.LastIndexOf("\\");
                            //int y = dp300_wnd.PathStr.Substring(0, x).LastIndexOf("\\");
                            ////tabPage1.Text = DP300_wnd.PathStr.Substring(0, x);
                            //tabpage11.Text = dp300_wnd.PathStr.Substring(y + 1, x - y - 1);
                            tabpage11.Text = dp300_wnd.PathStr.Substring(x + 1, dp300_wnd.PathStr.Length - x - 1);

                            ListStrPath.Add(dp300_wnd.PathStr); //LPJ 2013-6-13

                            strDirectory = dp300_wnd.PathStr; //LPJ 2013-9-23 保存此次打开的文件的完整路径
                        }
                        catch
                        {
                        }
                        dp300_playback.Add(dp300_wnd);
                    }
                }
                //else
                //{
                //    MessageBox.Show(Resource1.String304);
                //}

            }
            catch //(Exception ex) //LPJ 2013-6-11
            {
                MessageBox.Show(Resource1.String20);
            }

            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripBtnSurvey_Click(object sender, EventArgs e) //测量
        {
            //OceanADCPNewToolStripMenuItem_Click(sender, e);
            DP300_wnd.Show();
            DP300_wnd.RealDisPlay_Click(null, null);
        }
         private List<DP300_Windows> dp300_playback = new List<DP300_Windows>(); //LPJ 2013-7-1
         
        private List<string> ListStrPath = new List<string>(); //LPJ 2013-6-13 用于保存文件路径名
        //private List<DischargeSummary.Report> ListReport = new List<DischargeSummary.Report>(); //LPJ 2013-7-1 该变量用于保存所有回放数据的Summary数据
        private void toolStripBtnPlayback_Click(object sender, EventArgs e) //回放
        {
            //LPJ 2013-5-28 新建回放
            DP300_Windows dp300_wnd = new DP300_Windows();
            dp300_wnd.MdiParent = this;     //LPJ 2013-5-28
            dp300_wnd.Dock = System.Windows.Forms.DockStyle.Fill;  //LPJ 2013-5-28
            this.FormBorderStyle = FormBorderStyle.Sizable;

            dp300_wnd.TopLevel = false; //LPJ 2013-5-28 添加多个工程
            this.tabControl1.Visible = true;

            dPageNumbers++;

            TabPage tabpage = new TabPage();
            this.tabControl1.TabPages.Add(tabpage);
            tabpage.Text = "tabPage" + dPageNumbers.ToString();
            tabpage.Controls.Add(dp300_wnd);//LPJ 2013-5-28

            dp300_wnd.Show();
            dp300_wnd.PalyBackBtn_Click(null, null);

            if (!dp300_wnd.playBackMode) //LPJ 2013-6-13 当没有正确打开回放数据时，不创建新page页
            {
                tabControl1.TabPages.Remove(tabpage);
                dPageNumbers--;
            }
            else
            {
                try //LPJ 2013-6-4 bug
                {
                    //int x = dp300_wnd.PathStr.LastIndexOf("\\");
                    //int y = dp300_wnd.PathStr.Substring(0, x).LastIndexOf("\\");
                    ////tabpage.Text = DP300_wnd.PathStr.Substring(0, x); //LPJ 2013-5-30
                    //tabpage.Text = dp300_wnd.PathStr.Substring(y + 1, x - y - 1);//LPJ 2013-6-7 

                    //ListStrPath.Add(dp300_wnd.PathStr.Substring(0, x)); //LPJ 2013-6-13

                    int x = dp300_wnd.PathStr.LastIndexOf("\\");
                    tabpage.Text = dp300_wnd.PathStr.Substring(x + 1, dp300_wnd.PathStr.Length - x - 1);

                    ListStrPath.Add(dp300_wnd.PathStr); //LPJ 2013-6-13

                    strDirectory = dp300_wnd.PathStr; //LPJ 2013-9-23 保存此次打开的文件的完整路径
                }
                catch
                {
                }
                dp300_wnd.report.Number = dPageNumbers;
                this.tabControl1.SelectedIndex = dPageNumbers - 1; //LPJ 2013-6-7
                dp300_playback.Add(dp300_wnd); //LPJ 2013-7-1
            }
        }

        private void toolStripBtnSetting_Click(object sender, EventArgs e)
        {
            //FrmSetProject setProject = new FrmSetProject(DP300_wnd.defCfg);
            //setProject.ShowDialog();
            //DP300_wnd.defCfg.DefCfgInf = setProject.def.DefCfgInf;
        }

        #region 测量按钮操作
        private void toolStripBtnStartPings_Click(object sender, EventArgs e)
        {
            //开始发射
            //toolStripBtnStartPings.Enabled = false; //LPJ 2013-8-21
            try
            {
                if (DP300_wnd.OnStartPinging())
                {
                    //DP300_wnd.OnStartRecording();

                    toolStripBtnHome.Enabled = false; //LPJ 2013-9-25 当开始发射后，home按钮不可用
                    toolStripBtnSetting.Enabled = false;
                    toolStripBtnSurvey.Visible = false;

                    toolStripBtnStart.Visible = true;
                    //toolStripBtnStartEdge.Visible = false;  //LPJ 2013-6-18 当在主窗口中弹出设置测量时，不需要再显示开始测量按钮
                    toolStripBtnStartEdge.Visible = false;  //LPJ 2013-7-12 
                    toolStripBtnMoving.Visible = false;
                    toolStripBtnEndEdge.Visible = false;
                    toolStripBtnStopPings.Visible = true;  //LPJ 2016-1-4
                    toolStripBtnStop.Image = pictureBoxStop.Image; //LPJ 2013-8-9
                    toolStripBtnStop.Visible = false;
                    toolStripBtnStartPings.Visible = false;

                    toolStripBtnStatusR.Visible = false; //LPJ 2013-8-6
                    toolStripBtnStatusY.Visible = true;   //LPJ 2013-8-6
                    toolStripBtnStatusG.Visible = false;
                    toolStripLabelTip.Text = Resource1.String263;

                    #region 船速参考图标 2016-8-22
                    if (0 == DP300_wnd.iVesselSpeedRef)
                        toolStripDropDownBtnVesselRef.Image = pictureBoxBT.Image;
                    else if (1 == DP300_wnd.iVesselSpeedRef)
                        toolStripDropDownBtnVesselRef.Image = pictureBoxGPS.Image;
                    else if (3 == DP300_wnd.iVesselSpeedRef)
                        toolStripDropDownBtnVesselRef.Image = pictureBoxGGA.Image;
                    else
                        toolStripDropDownBtnVesselRef.Image = pictureBoxNull.Image;
                    #endregion

                    //LPJ 2013-7-26 弹出提示框
                    Point pnt = new Point();
                    pnt.X = this.toolStrip1.Location.X + 10;
                    pnt.Y = this.toolStrip1.Location.Y + 10;
                    toolTip1.Show(Resource1.String306, this, pnt, 10000);

                }

                //toolStripBtnStart.Enabled = true; //LPJ 2013-8-21

            }
            catch
            {
                // toolStripBtnSetting.Enabled = true;
                //toolStripBtnSurvey.Visible = true;

                //toolStripBtnStartPings.Enabled = true; //LPJ 2013-8-21
                toolStripBtnStartPings.Visible = true; 

                toolStripBtnStart.Visible = false;
                toolStripBtnStartEdge.Visible = false;
                toolStripBtnMoving.Visible = false;
                toolStripBtnEndEdge.Visible = false;
                toolStripBtnStop.Visible = false;
                toolStripBtnStopPings.Visible = false;  //LPJ 2016-1-4

                toolStripBtnStatusR.Visible = true; //LPJ 2013-8-6
                toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
                toolStripBtnStatusG.Visible = false; //LPJ 2013-8-6

                MessageBox.Show(Resource1.String17);
            }
        }

        /// <summary>
        /// Start Recording and processing data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnStart_Click(object sender, EventArgs e)
        {
            toolStripBtnStartPings.Visible = false;
            toolStripBtnStopPings.Visible = false;
            toolStripBtnStart.Visible = false;
            toolStripBtnStartEdge.Visible = true;
            toolStripBtnMoving.Visible = false;
            toolStripBtnEndEdge.Visible = false;
            toolStripBtnStop.Visible = true;
            
            
            DP300_wnd.OnStartRecording();


            toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
            toolStripBtnStatusG.Visible = true; //LPJ 2013-8-6
            toolStripBtnStatusR.Visible = false;

            toolStripBtnStatusR.Visible = false; //LPJ 2013-8-6
            toolStripBtnStatusY.Visible = true;   //LPJ 2013-8-6
            toolStripBtnStatusG.Visible = false;
            toolStripLabelTip.Text = Resource1.String308;

            //LPJ 2013-7-26 弹出提示框
            Point pnt = new Point();
            pnt.X = this.toolStrip1.Location.X + 10;
            pnt.Y = this.toolStrip1.Location.Y + 10;
            toolTip1.Show(Resource1.String256, this, pnt, 10000);

        }

        /// <summary>
        /// Start Edge measurement.  System is already pinging.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnStartEdge_Click(object sender, EventArgs e)
        { //开始岸边
            toolStripBtnStart.Visible = false;
            toolStripBtnStartEdge.Visible = false;
            toolStripBtnMoving.Visible = true;
            toolStripBtnEndEdge.Visible = false;
            toolStripBtnStop.Visible = true;
            DP300_wnd.btnStartEdge();

            toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
            toolStripBtnStatusG.Visible = true; //LPJ 2013-8-6
            toolStripBtnStatusR.Visible = false;
            toolStripLabelTip.Text = Resource1.String264;

            //LPJ 2013-7-26 提示
            Point pnt = new Point();
            pnt.X = this.toolStrip1.Location.X + 10;
            pnt.Y = this.toolStrip1.Location.Y + 10;
            toolTip1.Show(Resource1.String257, this, pnt, 10000);

            MoveTimer = new System.Timers.Timer(); //LPJ 2013-8-6 设置一个计时器，用于使move按钮可闪烁
            MoveTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveTimer_Elapsed);
            MoveTimer.Interval = 500;
            iflagTimer = 0;
            MoveTimer.Start();
           
        }

        private System.Timers.Timer MoveTimer ; //LPJ 2013-8-6
        private int iflagTimer = 0; //LPJ 2013-8-6
        //delegate void MoveTimerDelegate(); //LPJ 2013-8-6
        //MoveTimerDelegate RefreshMove;  //LPJ 2013-8-6

        private void MoveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) //LPJ 2013-8-6
        {
            try
            {
                if (iflagTimer >= 10)
                {
                    if (iflagTimer % 2 == 0)  //LPJ 2013-8-6
                    {
                        toolStripBtnMoving.Image = null;
                    }
                    else
                    {
                        toolStripBtnMoving.Image = pictureBoxMove.Image;
                    }
                }
            }
            catch //(Exception ee)
            {
                //MessageBox.Show(ee.Message);
            }
            iflagTimer++;
        }

        private System.Timers.Timer MoveLightTimer;
        private void toolStripBtnMoving_Click(object sender, EventArgs e)
        {
            MoveTimer.Stop(); //LPJ 2013-8-6
            toolStripBtnMoving.Image = pictureBoxMove.Image;  //LPJ 2013-8-6

            toolStripBtnStatusG.Visible = true; //LPJ 2013-8-6
            toolStripBtnStatusY.Visible = false;
            toolStripBtnStatusR.Visible = false;
            toolStripLabelTip.Text = Resource1.String265;

            //走航
            toolStripBtnStart.Visible = false;
            toolStripBtnStartEdge.Visible = false;
            toolStripBtnMoving.Visible = false;
            toolStripBtnEndEdge.Visible = true;
            toolStripBtnStop.Visible = true;
            DP300_wnd.btnMoving();

            //LPJ 2013-7-26 提示
            Point pnt = new Point();
            pnt.X = this.toolStrip1.Location.X + 10;
            pnt.Y = this.toolStrip1.Location.Y + 10;
            toolTip1.Show(Resource1.String256, this, pnt, 15000);

            MoveLightTimer = new System.Timers.Timer(); //LPJ 2013-8-6 设置一个计时器，用于使move指示可闪烁
            MoveLightTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveLightTimer_Elapsed);
            MoveLightTimer.Interval = 500;
            iflagTimer = 0;
            MoveLightTimer.Start();
        }
        private void MoveLightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //if (iflagTimer % 2 == 0)  //LPJ 2013-8-6   //LPJ 2016-12-14 走航时，令绿色按钮闪烁，文字不动
                //{
                //    toolStripBtnStatusG.Visible = false;
                //    toolStripLabelTip.Visible = false;
                //}
                //else
                //{
                //    toolStripBtnStatusG.Visible = true;
                //    toolStripLabelTip.Visible = true;
                //}


                if (iflagTimer % 2 == 0)  //LPJ 2013-8-6
                {
                    toolStripBtnStatusG.Image = null;  //2016-12-14
                }
                else
                {
                    toolStripBtnStatusG.Image = pictureBox4.Image;
                }
            }
            catch
            {
                //toolStripBtnStatusG.Visible = true;
                //toolStripLabelTip.Visible = true;
            }
            iflagTimer++;
        }
       
        private void toolStripBtnEndEdge_Click(object sender, EventArgs e)
        {
            MoveLightTimer.Stop(); //LPJ 2013-8-6
            //toolStripBtnStatusG.Visible = true; //LPJ 2013-8-6   //LPJ 2016-12-14
            toolStripBtnStatusG.Image = pictureBox4.Image;  //LPJ 2016-12-14
            toolStripBtnStatusY.Visible = false;
            toolStripBtnStatusR.Visible = false;
            //toolStripLabelTip.Visible = true; //LPJ 2013-8-8   //LPJ 2016-12-14
            toolStripLabelTip.Text = Resource1.String266; //LPJ 2013-8-8

            //结束岸边
            toolStripBtnStart.Visible = false;
            toolStripBtnStartEdge.Visible = false;
            toolStripBtnMoving.Visible = false;
            toolStripBtnEndEdge.Visible = false;
            toolStripBtnStop.Visible = true;

            DP300_wnd.btnEndEdge();
            //LPJ 2013-7-26 提示
            Point pnt = new Point();
            pnt.X = this.toolStrip1.Location.X + 10;
            pnt.Y = this.toolStrip1.Location.Y + 10;
            toolTip1.Show(Resource1.String307, this, pnt, 10000);

            StopTimer = new System.Timers.Timer(); //LPJ 2013-8-8 设置一个计时器，用于使stop按钮可闪烁
            StopTimer.Elapsed += new System.Timers.ElapsedEventHandler(StopTimer_Elapsed);
            StopTimer.Interval = 500;
            iflagTimer = 0;
            StopTimer.Start();
        }
        private System.Timers.Timer StopTimer; //LPJ 2013-8-6
        private void StopTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) //LPJ 2013-8-6
        {
            try
            {
                if (iflagTimer >= 10)
                {
                    if (iflagTimer % 2 == 0)  //LPJ 2013-8-6
                    {
                        toolStripBtnStop.Image = null;
                    }
                    else
                    {
                        toolStripBtnStop.Image = pictureBoxStop.Image;
                    }
                }
            }
            catch //(Exception ee)
            {
                //MessageBox.Show(ee.Message);
            }
            iflagTimer++;
        }

        /// <summary>
        /// 停止数据采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (MoveLightTimer != null)
                {
                    MoveLightTimer.Stop(); //LPJ 2013-8-6
                }
            }
            catch
            {
            }

            try
            {
                if (MoveTimer != null)
                {
                    MoveTimer.Stop(); //LPJ 2013-8-6
                }
            }
            catch
            {
            }
            
            try
            {
                if (StopTimer != null)
                {
                    StopTimer.Stop(); //LPJ 2013-8-8
                }
            }
            catch
            {
            }

            #region cancel
            //  toolStripBtnSetting.Enabled = true;
          ////  toolStripBtnSurvey.Visible = true;

          //  toolStripBtnStart.Visible = true;
          //  toolStripBtnStartEdge.Visible = false;
          //  toolStripBtnMoving.Visible = false;
          //  toolStripBtnEndEdge.Visible = false;
          //  toolStripBtnStop.Visible = false;

          //  toolStripBtnStatusG.Visible = false; //LPJ 2013-8-6
          //  toolStripBtnStatusR.Visible = true;  //LPJ 2013-8-6
          //  toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
          //  toolStripLabelTip.Text = Resource1.String267;

          //  DP300_wnd.btnStop();

            //  toolStripBtnHome.Enabled = true; //LPJ 2013-9-25 当开始发射后，home按钮不可用
            #endregion

            toolStripBtnStopPings.Visible = true;
            toolStripBtnStart.Visible = true;
            toolStripBtnStartEdge.Visible = false;
            toolStripBtnMoving.Visible = false;
            toolStripBtnEndEdge.Visible = false;
            toolStripBtnStop.Visible = false;
            toolStripBtnStartPings.Visible = false;

            toolStripBtnStatusG.Visible = false; //LPJ 2013-8-6
            toolStripBtnStatusR.Visible = true;  //LPJ 2013-8-6
            toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
            toolStripLabelTip.Text = Resource1.String259;

            toolStripBtnStop.Image = pictureBoxStop.Image; //LPJ 2016-12-14 当第二次开始测量按钮点击后，停止按钮图标消失

            DP300_wnd.OnStopRecording();

        }

       /// <summary>
       /// 停止发射呯
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void toolStripBtnStopPings_Click(object sender, EventArgs e)
        {
            toolStripBtnSetting.Enabled = true;
            //  toolStripBtnSurvey.Visible = true;

            toolStripBtnStartPings.Visible = true;
            toolStripBtnStart.Visible = false;
            toolStripBtnStartEdge.Visible = false;
            toolStripBtnMoving.Visible = false;
            toolStripBtnEndEdge.Visible = false;
            toolStripBtnStop.Visible = false;
            toolStripBtnStopPings.Visible = false;

            toolStripBtnStatusG.Visible = false; //LPJ 2013-8-6
            toolStripBtnStatusR.Visible = true;  //LPJ 2013-8-6
            toolStripBtnStatusY.Visible = false; //LPJ 2013-8-6
            toolStripLabelTip.Text = Resource1.String258;

            DP300_wnd.OnStopPinging();

            toolStripBtnHome.Enabled = true; //LPJ 2013-9-25 当开始发射后，home按钮不可用

        }
        #endregion

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            if (Language_ZH) //中文
            {
                //this.WindowState = FormWindowState.Normal;  //LPJ 2012-10-20
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
                //ApplyResource();
                ApplyResources(); //LPJ 2013-7-15
                Language_ZH = false;

                btnLanguage.BackgroundImage = toolStripBtnHanzi.Image;
                toolStripBtnLanguage.Image = toolStripBtnHanzi.Image;

                //this.WindowState = FormWindowState.Maximized;  //LPJ 2012-10-20

                getcfg.Language = "English"; //LPJ 2013-8-16
               
             
            }
            else //英语
            {
                //this.WindowState = FormWindowState.Normal;  //LPJ 2012-10-20

                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh");
                //ApplyResource();
                ApplyResources(); //LPJ 2013-7-15
                Language_ZH = true;

                btnLanguage.BackgroundImage = toolStripBtnAzi.Image;
                toolStripBtnLanguage.Image = toolStripBtnAzi.Image;

                //this.WindowState = FormWindowState.Maximized;  //LPJ 2012-10-20
                getcfg.Language = "Chinese"; //LPJ 2013-8-16
               
            }

            try
            {
                getcfg.WriteToCfg(); //LPJ 2013-8-16
            }
            catch
            {
            }

        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            //string Text = Resource1.str_aboutText1 + "\r\n" + Resource1.str_aboutText2 + "\r\n"
            //    + Resource1.str_aboutText4 + "\r\n" + Resource1.str_aboutText5;
            //MessageBox.Show(Text, Resource1.str_About, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //AboutBox1 about = new AboutBox1();
            //about.Show();

            showManual();
        }

        private void toolStripBtnLanguage_Click(object sender, EventArgs e)
        {
            if (Language_ZH) //中文
            {
                //englishToolStripMenuItem_Click(this, null);
                System.Threading.Thread.CurrentThread.CurrentUICulture
               = System.Globalization.CultureInfo.GetCultureInfo("en");
               
                Language_ZH = false;
               
                try
                {
                    DP300_wnd.ChooseLanguage();
                  
                }
                catch
                {
                }

                btnLanguage.BackgroundImage = toolStripBtnHanzi.Image;
                toolStripBtnLanguage.Image = toolStripBtnHanzi.Image;
            }
            else //英语
            {
                //CHNToolStripMenuItem_Click(this, null);
                System.Threading.Thread.CurrentThread.CurrentUICulture  = System.Globalization.CultureInfo.GetCultureInfo("zh");
                
                try
                {
                    DP300_wnd.ChooseLanguage();
                  
                }
                catch
                {
                }

                Language_ZH = true;

                btnLanguage.BackgroundImage = toolStripBtnAzi.Image;
                toolStripBtnLanguage.Image = toolStripBtnAzi.Image;
            }
        }

        private void toolStripBtnAbout_Click(object sender, EventArgs e)
        {
            //btnAbout_Click(this, null);
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void toolStripBtnBack_Click(object sender, EventArgs e) //LPJ 2013-5-23
        {
            if (this.tabControl1.Visible) //LPJ 2013-5-28 当在回放模式下，点击退出
            {
                if(tabControl1.TabPages.Count==1)
                    DP300_wnd.Close(); //LPJ 2013-6-8

                //获取当前所选的page页index，并将该页删除
                this.tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);//LPJ 2013-5-28 移除当前所在的页面
              //  dPageIndex--; //LPJ 2013-6-7
                try //LPJ 2013-6-4 当无流量信息时，报错
                {
                    reportList.RemoveAt(tabControl1.SelectedIndex); //LPJ 2013-5-30 移除当前所在页面的流量汇总信息
                }
                catch
                {
                }
                if (this.tabControl1.TabPages.Count == 0)
                {
                    this.tabControl1.Visible = false;
                    dPageNumbers = 0;
                  //  dPageIndex = 0;  //LPJ 2013-6-7
                    this.toolStrip1.Visible = false;
                    panelPanComm.Visible = true;
                    panelRTI.Visible = true;
                    panel2.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;

                    btnAbout.Visible = true;
                    btnLanguage.Visible = true;
                    btnSurveying.Visible = true;
                    btnPlayback.Visible = true;
                    btnExit.Visible = true;
                    //btnManual.Visible = true; //LPJ 2013-7-29 
                  


                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            else //当在测量模式下，点击退出
            {
                DP300_wnd.Close();
                if (DP300_wnd.bClose)
                {
                    this.tabControl1.Visible = false;
                    this.toolStrip1.Visible = false;
                    panelPanComm.Visible = true;
                    panelRTI.Visible = true;
                    panel2.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    btnAbout.Visible = true;
                    btnLanguage.Visible = true;
                    btnSurveying.Visible = true;
                    btnPlayback.Visible = true;
                    btnExit.Visible = true;
                    //btnManual.Visible = true; //LPJ 2013-7-29 


                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
                
            }
            
        }

        private void panelPanComm_Click(object sender, EventArgs e)
        {
            //string strPanComm;
            //strPanComm = Resource1.str_aboutText3+"\r\n\r\n"+Resource1.str_aboutText4+"\r\n"+Resource1.str_Tel;
            //MessageBox.Show(strPanComm);
            btnPanCommWeb();
        }

        private void Form1_Load(object sender, EventArgs e) //修改父窗体的背景颜色
        {
            MdiClient ctlMDI;
            // Loop through all of the form's controls looking
            // for the control of type MdiClient.
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    // Attempt to cast the control to type MdiClient.
                    ctlMDI = (MdiClient)ctl;
                    // Set the BackColor of the MdiClient control.
                    ctlMDI.BackColor = this.BackColor;
                }
                catch //(InvalidCastException exc)
                {
                    // Catch and ignore the error if casting failed.
                }
            }

            //LPJ 2013-6-13 为tabPage添加关闭按钮---start
            imageClose = new Bitmap(pictureBox1.Image);
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Padding = new System.Drawing.Point(15, 3);
            this.tabControl1.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown); 
            this.tabControl1.MouseMove+=new MouseEventHandler(this.tabControl1_MouseMove);
            //LPJ 2013-6-13 为tabPage添加关闭按钮---end

            //LPJ 2013-6-17 在这里重新为控件设置位置
            ReSetControls();

            //LPJ 2013-7-25 在这里添加帮助文档
            try
            {
                helpProvider1.HelpNamespace = Application.StartupPath + @"\help.chm";
                helpProvider1.SetShowHelp(this, true);

                helpProvider_zh.HelpNamespace = Application.StartupPath + @"\help_ZH.chm"; //LPJ 2013-7-26
                helpProvider_zh.SetShowHelp(this, true);  //LPJ 2013-7-26
            }
            catch
            {

            }
        }
        private HelpProvider helpProvider_zh = new HelpProvider(); //LPJ 2013-7-26
        
        public List<DischargeSummary.Report> reportList = new List<DischargeSummary.Report>(); //LPJ 2013-5-29 汇总文件信息
       
        private void toolStripBtnSummary_Click(object sender, EventArgs e)
        {
            reportList.Clear();

            DischargeSummary.BasicMessage bM;// = new DischargeSummary.BasicMessage();  //LPJ 2016-10-10 汇总信息

            bM = dp300_playback[0].bMessage;  //LPJ 2016-10-10 汇总信息
            bM.endhour = dp300_playback[dp300_playback.Count - 1].bMessage.endhour;
            bM.endmin = dp300_playback[dp300_playback.Count - 1].bMessage.endmin;
            bM.endsec = dp300_playback[dp300_playback.Count - 1].bMessage.endsec;

            for (int i = 0; i < dp300_playback.Count; i++)
            {
                reportList.Add(dp300_playback[i].report); 
            }
            //当点击汇总按钮时，将当前显示的工程文件信息汇总
            if (reportList.Count > 0)
            {
                FrmShowSummary showSummary = new FrmShowSummary(reportList, bM, dp300_playback[0].bEnglish2Metric); //LPJ 2013-6-19
                showSummary.Show();
            }
         
        }

        private void toolStripBtnHome_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.Visible) //playback mode
            { 
                //DP300_wnd.Close(); //LPJ 2013-6-8
                //if (DP300_wnd.bClose) //LPJ 2013-6-14
                //{
                //    this.tabControl1.TabPages.Clear();
                //    reportList.Clear();
                //    ListStrPath.Clear();
                //    //     dPageIndex = 0;
                //    dPageNumbers = 0;
                //}

                try
                {
                    this.tabControl1.TabPages.Clear();
                    ListStrPath.Clear();
                    dPageNumbers = 0;
                    reportList.Clear();

                    for (int i = 0; i < dp300_playback.Count; i++) //LPJ 2013-9-10
                    {
                        dp300_playback[i].OnHome();
                    }

                    dp300_playback.Clear();
                    
                }
                catch
                {
                }

                this.tabControl1.Visible = false;
                this.toolStrip1.Visible = false;
                panelPanComm.Visible = true;
                panelRTI.Visible = true;
                panel2.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                btnAbout.Visible = true;
                btnLanguage.Visible = true;
                btnSurveying.Visible = true;
                btnPlayback.Visible = true;
                btnExit.Visible = true;
                //btnManual.Visible = true;  //LPJ 2013-7-29 

                this.FormBorderStyle = FormBorderStyle.None;
            }
            else //measurement mode
            {
                DP300_wnd.Close();
                if (DP300_wnd.bClose) //LPJ 2013-6-14
                {
                    try
                    {
                        MoveLightTimer.Stop(); //LPJ 2013-8-6
                    }
                    catch
                    {
                    }

                    this.tabControl1.Visible = false;
                    this.toolStrip1.Visible = false;
                    panelPanComm.Visible = true;
                    panelRTI.Visible = true;
                    panel2.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    btnAbout.Visible = true;
                    btnLanguage.Visible = true;
                    btnSurveying.Visible = true;
                    btnPlayback.Visible = true;
                    btnExit.Visible = true;
                    //btnManual.Visible = true; //LPJ 2013-7-29 

                    this.FormBorderStyle = FormBorderStyle.None;
                }
            }
           
           
        }

        #region 操作tabControl
        private Bitmap imageClose; //LPJ 2013-6-13
        const int CLOSE_SIZE = 15;  //LPJ 2013-6-13
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)  //LPJ 2013-6-13
        {
            try
            {
                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);
                //先添加TabPage属性    
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);
                //再画一个矩形框 
                using (Pen p = new Pen(Color.White))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }

                //填充矩形框 
                Color recColor = e.State == DrawItemState.Selected ? Color.Gold : Color.LightGray;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }
                //画关闭符号 
                using (Pen objpen = new Pen(Color.Black))
                {
                    ////============================================= 
                    //使用图片 
                    Bitmap bt = new Bitmap(imageClose);
                    Point p5 = new Point(myTabRect.X, myTabRect.Y);
                    e.Graphics.DrawImage(bt, p5);
                }
                e.Graphics.Dispose();
            }
            catch (Exception)
            { }
        }
        //关闭按钮功能 
        private void tabControl1_MouseDown(object sender, MouseEventArgs e)  //LPJ 2013-6-13
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X, y = e.Y;
                //计算关闭区域    
                Rectangle myTabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);

                myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                myTabRect.Width = 5;
                myTabRect.Height = 5;
                //如果鼠标在区域内就关闭选项卡    
                bool isClose = x > myTabRect.X && x < myTabRect.Right + myTabRect.Width && y > myTabRect.Y && y < myTabRect.Bottom + myTabRect.Height;
                if (isClose)
                {
                    int selectIndex = this.tabControl1.SelectedIndex;
                    if (tabControl1.TabPages.Count == 1)
                    {
                        dp300_playback.Clear();
                    }
                    //this.tabControl1.TabPages.Remove(this.tabControl1.SelectedTab);
                    this.tabControl1.TabPages.RemoveAt(selectIndex); //LPJ 2013-6-19 移除选中的tab页
                    ListStrPath.RemoveAt(selectIndex);
                    //    dPageIndex--; //LPJ 2013-6-7
                    dPageNumbers--;

                    try //LPJ 2013-6-4 当无流量信息时，报错
                    {
                        dp300_playback.RemoveAt(selectIndex);
                        //reportList.RemoveAt(selectIndex); //LPJ 2013-5-30 移除当前所在页面的流量汇总信息
                    }
                    catch
                    {
                       
                    }
                }
            }

           
        }
        private void tabControl1_MouseMove(object sender, MouseEventArgs e) //LPJ 2013-6-13
        {
            int x = e.X, y = e.Y;
            Rectangle myTabRect;
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                myTabRect = this.tabControl1.GetTabRect(i);//计算text区域 
                myTabRect.Offset(0, 0);
                myTabRect.Width = 5;
                myTabRect.Height = 5;
                //如果鼠标在区域内就关闭选项卡    
                bool isText = x > myTabRect.X && x < myTabRect.Right + myTabRect.Width && y > myTabRect.Y && y < myTabRect.Bottom + myTabRect.Height;
                if (isText == true)
                {
                    try
                    {
                        toolTip1.SetToolTip(this.tabControl1, ListStrPath[i]);
                    }
                    catch
                    {
                        toolTip1.SetToolTip(this.tabControl1, "   ");
                    }
                }
            }
        }
        #endregion

        #region 设置Form大小
        private void Form1_Resize(object sender, EventArgs e)
        {
            //LPJ 2013-6-17 在这里重新为控件设置大小及位置
            ReSetControls();
        }

        private void ReSetControls() //LPJ 2013-6-17 在这里重新为控件设置大小及位置
        {
            this.panel2.Location = new Point((this.Width - panel2.Width) / 2, this.Height / 5 - panel2.Height);
            this.btnSurveying.Width = (int)(this.Width / 8.0f);
            this.btnSurveying.Height = (int)(this.Width / 8.0f);
            this.btnSurveying.Location = new Point(this.Width / 6, this.Height  / 2);
            this.label4.Location = new Point(this.Width / 6 + this.btnSurveying.Width / 2 - this.label4.Width / 2, this.btnSurveying.Bottom);

            this.btnPlayback.Width = (int)(this.Width / 8.0f);
            this.btnPlayback.Height = (int)(this.Width / 8.0f);
            this.btnPlayback.Location = new Point(this.Width / 3, this.Height / 2);
            this.label5.Location = new Point(this.Width / 3 + this.btnPlayback.Width / 2 - this.label5.Width / 2, this.btnPlayback.Bottom);

            this.btnLanguage.Width = (int)(this.Width / 8.0f);
            this.btnLanguage.Height = (int)(this.Width / 8.0f);
            this.btnLanguage.Location = new Point(this.Width / 2, this.Height / 2);
            this.label6.Location = new Point(this.Width / 2 + this.btnLanguage.Width / 2 - this.label6.Width / 2, this.btnLanguage.Bottom);

            this.btnAbout.Width = (int)(this.Width / 8.0f);
            this.btnAbout.Height = (int)(this.Width / 8.0f);
            this.btnAbout.Location = new Point(this.Width * 2 / 3, this.Height / 2);
            this.label7.Location = new Point(this.Width * 2 / 3 + this.btnAbout.Width / 2 - this.label7.Width / 2, this.btnAbout.Bottom);

            this.panelRTI.Location = new Point(10, this.Height - this.panelRTI.Height - 10);
            this.label2.Location = new Point(this.panelRTI.Right, this.panelRTI.Top + this.panelRTI.Height / 2);
            this.panelPanComm.Location = new Point(this.Width - this.panelPanComm.Width - 10, this.Height - this.panelPanComm.Height - 10);
            this.label1.Location = new Point(this.panelPanComm.Left - this.label1.Width, this.label2.Top);

            this.tabControl1.Location = new Point(0, this.toolStrip1.Height);
            this.tabControl1.Height = this.Height - this.toolStrip1.Height; //LPJ 2013-6-14
            this.tabControl1.Width = this.Width; //LPJ 2013-6-14

            //this.toolStripBtnMoving.Width = 34; //LPJ 2013-8-6
            //this.toolStripBtnMoving.Height = 34;
            this.MinimumSize = new Size(0, 0);
        }
        #endregion

        private void showManual()
        {
            //LPJ 2013-7-25 帮助文档
            try
            {
                if (!Language_ZH)
                    Process.Start(Application.StartupPath + @"\help.chm");
                else
                    Process.Start(Application.StartupPath + @"\help_ZH.chm"); //LPJ 2013-7-26
            }
            catch
            {

            }
        }

        private void ApplyResources() //LPJ 2013-7-15 将控件名称本地化
        {
            if (Language_ZH) //设置字体
            {
                panel2.BackgroundImage = pictureBox3.BackgroundImage;
            }
            else
            {
                panel2.BackgroundImage = pictureBox2.BackgroundImage;
            }

            label1.Text = Resource1.String244;
            label2.Text = Resource1.String243;
            label4.Text = Resource1.String239;
            label5.Text = Resource1.String240;
            label6.Text = Resource1.String241;
            label7.Text = Resource1.String242;

            ToolStripMenuItem_Manual.Text = Resource1.String246;
            ToolStripMenuItem_RTIWeb.Text = Resource1.String247;
            ToolStripMenuItem_PanCommWeb.Text = Resource1.String248;
            toolStripDropDownBtnVesselRef.Text = Resource1.String314;
            bTToolStripMenuItem.Text=Resource1.String232;
            nULLToolStripMenuItem.Text=Resource1.String233;

            toolStripBtnPlayback.Text = Resource1.String240; //LPJ 2013-9-24
            toolStripBtnNext.Text = Resource1.String273;
            toolStripBtnSummary.Text = Resource1.String274;
            toolStripBtnHome.Text = Resource1.String275;
            toolStripBtnHelp.Text = Resource1.String276;
            toolStripBtnAbout.Text = Resource1.String282;

            toolStripBtnStart.Text = Resource1.String277;
            toolStripBtnStartEdge.Text = Resource1.String278;
            toolStripBtnMoving.Text = Resource1.String279;
            toolStripBtnEndEdge.Text = Resource1.String280;
            toolStripBtnStop.Text = Resource1.String281;  //LPJ 2013-9-24
            toolStripBtnStartPings.Text = Resource1.String32;
            toolStripBtnStopPings.Text = Resource1.String267;

            ReSetControls();
        }

        private void ToolStripMenuItem_Menual_Click(object sender, EventArgs e)
        {
            showManual();
        }

        private void ToolStripMenuItem_RTIWeb_Click(object sender, EventArgs e)
        {
            btnRTIWeb();
        }
        private void btnRTIWeb()
        {
            try
            {
                System.Diagnostics.Process.Start("iexplore.exe", "http://www.rowetechinc.com");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void ToolStripMenuItem_PanCommWeb_Click(object sender, EventArgs e)
        {
            btnPanCommWeb();
        }

        private void btnPanCommWeb()
        {
            try
            {
                System.Diagnostics.Process.Start("iexplore.exe", "http://www.pan-comm.com");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            showManual();
        }

        private void panelRTI_Click(object sender, EventArgs e)
        {
            btnRTIWeb();
        }

        public void Form1_KeyDown(object sender, KeyEventArgs e) //LPJ 2013-8-2 设置快捷键
        {
            if (toolStripBtnStartPings.Enabled && toolStripBtnStartPings.Visible)
            {
                if (Keys.F5 == e.KeyCode)
                {
                    toolStripBtnStartPings_Click(this, null);
                    return;
                }
            }

            if (toolStripBtnStart.Enabled && toolStripBtnStart.Visible)//测量模式下，串口已经连接上
            {
                if (Keys.F5 == e.KeyCode)
                {
                    toolStripBtnStart_Click(this, null);
                    return;
                }
            }
            //开始按钮已经点击

            if (toolStripBtnStartEdge.Enabled && toolStripBtnStartEdge.Visible)
            {
                if (Keys.F5 == e.KeyCode)//开始岸边按钮
                {
                    toolStripBtnStartEdge_Click(this, null); return;
                }
            }

            if (toolStripBtnMoving.Enabled && toolStripBtnMoving.Visible)
            {
                if (Keys.F5 == e.KeyCode)//开始走航按钮
                {
                    toolStripBtnMoving_Click(this, null); return;
                }
            }
            if (toolStripBtnEndEdge.Enabled && toolStripBtnEndEdge.Visible)
            {
                if (Keys.F5 == e.KeyCode)//结束岸边按钮
                {
                    toolStripBtnEndEdge_Click(this, null); return;
                }
            }

            if (toolStripBtnStop.Enabled && toolStripBtnStop.Visible)//停止按钮可用
            {
                if (Keys.F6 == e.KeyCode)
                {
                    toolStripBtnStop_Click(this, null); return;
                }
            }

            if (toolStripBtnStopPings.Enabled && toolStripBtnStopPings.Visible)//停止按钮可用
            {
                if (Keys.F6 == e.KeyCode)
                {
                    toolStripBtnStopPings_Click(this, null); return;
                }
            }


        }

        private string strDirectory; //LPJ 2013-9-23 添加一个变量用于存储上一次打开的文件的路径
        //该函数可用于自动打开下一个回放数据，在这里需要将当前的文件路径保存，然后搜索与该路径一致的回放文件
        private void toolStripBtnNext_Click(object sender, EventArgs e)
        {
            if (null != strDirectory) //strDirectory=C:\\...\\DP300\\test_20140730_134521
            {
                try
                {
                    int iLastLength=strDirectory.LastIndexOf("\\");
                    string strFoot = strDirectory.Substring(0, iLastLength);
                     
                    //提取工程名，工程日期
                    //搜索工程名与日期相同，但是时间在其之后的文件名
                    string strDirLast = strDirectory.Substring(iLastLength, strDirectory.Length - iLastLength);
                    string[] strSplit = strDirLast.Split('_');
                    int i=strSplit.Count();

                    //获取str2下所有文件夹的名称
                    string[] strFolders = Directory.GetFiles(strFoot, "*.bin"); //取得的是所有的该路径下的文件夹的路径
                    int min = 240000;
                    string strOpenNext = "";

                    //接下来，在str2中搜索与strNext相匹配的文件名，如果找到，则在该路径playbackData中打开回放
                    //如果未找到，则弹出提示框
                    foreach (string str in strFolders)
                    {
                        int iLastLength2 = str.LastIndexOf("\\");
                        string strDir2 = str.Substring(iLastLength2, str.Length - iLastLength2);
                        string[] strDir2Split = strDir2.Split('.');

                        string[] strSplit2 = strDir2Split[0].Split('_');
                        if ((strSplit2[0] == strSplit[0]) && (strSplit2[1] == strSplit[1]))
                        {
                            if (int.Parse(strSplit2[2]) > int.Parse(strSplit[2]))
                            {
                                if (min > int.Parse(strSplit2[2]))
                                {
                                    strOpenNext = str;
                                    min = int.Parse(strSplit2[2]);
                                }
                            }
                        }
                    }

                    if ("" == strOpenNext)
                    {
                        MessageBox.Show(Resource1.String285);
                    }
                    else //将该文件打开
                    {
                        btnOpenNext(strOpenNext);
                    }
                }
                catch
                {
                    MessageBox.Show(Resource1.String285);
                }

                #region cancel
                /*
                try
                {
                    int last1 = strDirectory.LastIndexOf("\\");         //strDirectory="E:\\ADCP\\.....\\HuangPuRiver_001_20130810_130824\\PlaybackData",last1=
                    string str1 = strDirectory.Substring(0, last1);   //str1="E:\\ADCP\\.....\\HuangPuRiver_001_20130810_130824"

                    int last2 = str1.LastIndexOf("\\");
                    string str2 = str1.Substring(0, last2 + 1);          //str2="E:\\ADCP\\.....\\"

                    int length1 = str1.Length;
                    int length2 = str2.Length;
                    string str3 = str1.Substring(length2, length1 - length2);           //str3="HuangPuRiver_001_20130810_130824"

                    string[] str4 = str3.Split('_');
                    int inumber = int.Parse(str4[str4.Count() - 3]);    //inumber=001

                    string str5 = str3.Substring(0, str3.Length - 19);   //str5="HuangPuRiver_"
                    string strNex = str5 + (inumber + 1).ToString("000"); //strNext="HuangPuRiver_002"

                    //获取str2下所有文件夹的名称
                    string[] strFolders = Directory.GetDirectories(str2); //取得的是所有的该路径下的文件夹的路径
                    string strOpenNext = "";

                    //接下来，在str2中搜索与strNext相匹配的文件名，如果找到，则在该路径playbackData中打开回放
                    //如果未找到，则弹出提示框
                    foreach (string str in strFolders)
                    {
                        if (str.Contains(strNex))
                        {
                            strOpenNext = str;
                            break;
                        }
                    }
                    if ("" == strOpenNext)
                    {
                        MessageBox.Show(Resource1.String285);
                    }
                    else //将该文件打开
                    {
                        btnOpenNext(strOpenNext + "\\PlaybackData");
                    }
                }
                catch
                {
                    MessageBox.Show(Resource1.String285);
                }
                 * */
                #endregion
            }
        
        }

        private void btnOpenNext(string strPathNext) //LPJ 2013-9-23
        {
            DP300_Windows dp300_wnd = new DP300_Windows();
            dp300_wnd.MdiParent = this;     //LPJ 2013-5-28
            dp300_wnd.Dock = System.Windows.Forms.DockStyle.Fill;  //LPJ 2013-5-28
            this.FormBorderStyle = FormBorderStyle.Sizable;

            dp300_wnd.TopLevel = false; //LPJ 2013-5-28 添加多个工程
            this.tabControl1.Visible = true;

            dPageNumbers++;

            TabPage tabpage = new TabPage();
            this.tabControl1.TabPages.Add(tabpage);
            tabpage.Text = "tabPage" + dPageNumbers.ToString();
            tabpage.Controls.Add(dp300_wnd);//LPJ 2013-5-28

            dp300_wnd.Show();
            dp300_wnd.btnOpenNext(strPathNext);

            if (!dp300_wnd.playBackMode) //LPJ 2013-6-13 当没有正确打开回放数据时，不创建新page页
            {
                tabControl1.TabPages.Remove(tabpage);
                dPageNumbers--;
            }
            else
            {
                try //LPJ 2013-6-4 bug
                {
                    //int x = dp300_wnd.PathStr.LastIndexOf("\\");
                    //int y = dp300_wnd.PathStr.Substring(0, x).LastIndexOf("\\");
                   
                    //tabpage.Text = dp300_wnd.PathStr.Substring(y + 1, x - y - 1);//LPJ 2013-6-7 
                    //ListStrPath.Add(dp300_wnd.PathStr.Substring(0, x)); //LPJ 2013-6-13

                    //strDirectory = dp300_wnd.PathStr; //LPJ 2013-9-23 保存此次打开的文件的完整路径

                    int x = dp300_wnd.PathStr.LastIndexOf("\\");
                    tabpage.Text = dp300_wnd.PathStr.Substring(x + 1, dp300_wnd.PathStr.Length - x - 1);

                    ListStrPath.Add(dp300_wnd.PathStr); //LPJ 2013-6-13

                    strDirectory = dp300_wnd.PathStr;
                }
                catch
                {
                }
                dp300_wnd.report.Number = dPageNumbers;
                this.tabControl1.SelectedIndex = dPageNumbers - 1; //LPJ 2013-6-7
                dp300_playback.Add(dp300_wnd); //LPJ 2013-7-1
            }
        }

        private bool bClose = false;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (this.tabControl1.Visible) //playback mode
                {
                    bClose = true;
                }
                else
                {
                    if (DP300_wnd != null)
                        bClose = DP300_wnd.bClose;
                    else
                        bClose = true;
                }
                if (bClose)
                {
                    base.OnFormClosing(e);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                else
                    e.Cancel = true;
            }
            catch
            {
            }

        }

        private void toolStripBtnCalibration_Click(object sender, EventArgs e)
        {
            //Process.Start("DP-Pro Calibration.exe", @"\"); 
            //string strPath = Application.StartupPath + @"\ADCP Calibration\DP-Pro Calibration.exe";
            //if (OpenPress(strPath, "DP-Pro Calibration.exe"))
            //{
               
            //}
            //else
            //{
            //    MessageBox.Show("载入失败");
            //}

            FrmCalibration frmCalibrate = new FrmCalibration(true, strDirectory);
            frmCalibrate.Show();
        }

        /*
        private bool OpenPress(string FileName, string Arguments)
        {
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            if (System.IO.File.Exists(FileName))
            {
                pro.StartInfo.FileName = FileName;
                pro.StartInfo.Arguments = Arguments;
                pro.Start();

                return true;
            }
            return false;
        }
        */
        /*
        private bool ClosePress(string strName)
        {
            Process[] p = Process.GetProcesses();
            foreach (Process p1 in p)
            {
                try
                {
                    string processName = p1.ProcessName.ToLower().Trim();
                    //判断是否包含阻碍更新的进程               
                    if (processName == strName)
                    {
                        p1.Kill();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        */
        private void toolStripDropDownBtnVesselRef_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            #region 船速参考图标 2016-8-22
            int iSelectedIndex;// = 0;
            if (toolStripDropDownBtnVesselRef.DropDownItems[0].Selected)
            {
                toolStripDropDownBtnVesselRef.Image = pictureBoxBT.Image;
                iSelectedIndex = 0;
            }
            else if (toolStripDropDownBtnVesselRef.DropDownItems[1].Selected)
            {
                toolStripDropDownBtnVesselRef.Image = pictureBoxGPS.Image;
                iSelectedIndex = 1;
            }
            else if (toolStripDropDownBtnVesselRef.DropDownItems[2].Selected)
            {
                toolStripDropDownBtnVesselRef.Image = pictureBoxNull.Image;
                iSelectedIndex = 2;
            }
            else
            {
                toolStripDropDownBtnVesselRef.Image = pictureBoxGGA.Image;
                iSelectedIndex = 3;
            }
            try
            {
                if (tabControl1.Visible)
                {
                    dp300_playback[tabControl1.SelectedIndex].OnChangedVesselRef(iSelectedIndex);
                }
                else
                {
                    DP300_wnd.OnChangedVesselRef(iSelectedIndex);
                }
            }
            catch
            {
            }
           
            #endregion
        }

        
    }
}
