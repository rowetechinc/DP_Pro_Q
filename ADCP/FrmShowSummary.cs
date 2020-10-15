using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ADCP
{
    public partial class FrmShowSummary : Form
    {
        public FrmShowSummary(List<DischargeSummary.Report> reportList,DischargeSummary.BasicMessage bMessage, bool bEnglish2Metric)
        {
            InitializeComponent();
            SetColumnWidth();

            report = reportList;     
            basicMessage = bMessage;
            bEn2Metric = bEnglish2Metric; //LPJ 2013-8-22

            RefreshListViewHeader(listView_Summary, bEnglish2Metric); //LPJ 2013-8-2
            ListView_Summary_Paint();

            if (!bEnglish2Metric) //LPJ 2013-8-9 当单位为英制
            {
                RefreshSummaryData();
            }
        }
        private bool bEn2Metric = true;  //LPJ 2013-8-22
        private DischargeSummary.BasicMessage basicMessage=new DischargeSummary.BasicMessage();
        private List<DischargeSummary.Report> report = new List<DischargeSummary.Report>();
        private void ListView_Summary_Paint()
        {
            listView_Summary.Items.Clear();

            for (int i = 0; i < report.Count; i++)
            { 
                DischargeSummary.Report rep = new DischargeSummary.Report();
                rep = report[i];

                ListViewItem SummaryItems = new ListViewItem();
                AddToListViewItem(rep,SummaryItems);
                listView_Summary.Items.Add(SummaryItems);
            }

            DataProcessing(report);
        }

        private void DataProcessing(List<DischargeSummary.Report> reportList) //根据提供的数据，计算其平均值、标准差及误差
        {
            DischargeSummary.Report sumData = new DischargeSummary.Report(); //平均值
            DischargeSummary.Report DevData = new DischargeSummary.Report(); //标准差
            DischargeSummary.Report CovData = new DischargeSummary.Report();//误差
         
         //计算平均值
            CalMeanData(reportList, ref sumData); //LPJ 2013-9-18

            #region 当误差超限标红 //LPJ 2017-1-3
            for (int i = 0; i < reportList.Count(); i++)
            {
                listView_Summary.Items[i].ForeColor = Color.Black;
                if (Math.Abs((sumData.TotalQ - reportList[i].TotalQ) / sumData.TotalQ) > 0.05)
                {
                    listView_Summary.Items[i].ForeColor = Color.Red;
                }
            }
            #endregion

            sumData.fileName = "";
            sumData.startTime = "";
            sumData.startDate = "";
            sumData.Duration = Resource1.String253;

            ListViewItem sumItem = new ListViewItem();
            AddToListViewItem(sumData,sumItem); //汇总数据
            listView_Summary.Items.Add(sumItem);

            //计算标准差
            if (reportList.Count > 0)
            {
                //CalDeviationData(report, sumData, ref DevData);
                CalDeviationData(reportList, sumData, ref DevData); //LPJ 2013-9-18
            }
            else
            {
                DevData.TotalQ = 0;
                DevData.WaterWidth = 0;
                DevData.Area = 0;
                DevData.AverageVel = 0;
                DevData.MeanCourse = 0;
                DevData.DistanceMG = 0;
                DevData.CourseMG = 0;
                DevData.Length = 0;
                DevData.LeftQ = 0;
                DevData.RightQ = 0;
                DevData.TopQ = 0;
                DevData.BottomQ = 0;
                DevData.MiddleQ = 0;
            }
            DevData.fileName = "";
            DevData.startTime = "";
            DevData.startDate = "";
            DevData.Duration = Resource1.String254;

            ListViewItem devItem = new ListViewItem();
            AddToListViewItem(DevData, devItem); //汇总数据
            listView_Summary.Items.Add(devItem);

            //计算误差
            CovData = CalCovariance(DevData, sumData);
            CovData.fileName = "";
            CovData.startTime = "";
            CovData.startDate = "";
            CovData.Duration = Resource1.String255;
            ListViewItem covItem = new ListViewItem();
            AddToListViewItem(CovData, covItem);
            listView_Summary.Items.Add(covItem);
        }

        private void CalMeanData(List<DischargeSummary.Report> reportList, ref DischargeSummary.Report sumData)
        {
            for (int i = 0; i < reportList.Count; i++)
            {
                DischargeSummary.Report rep = new DischargeSummary.Report();
                rep = reportList[i];

                sumData.TotalQ += rep.TotalQ / reportList.Count;
                sumData.WaterWidth += rep.WaterWidth / reportList.Count;
                sumData.Area += rep.Area / reportList.Count;
                sumData.AverageVel += rep.AverageVel / reportList.Count;
                sumData.MeanCourse += rep.MeanCourse / reportList.Count;
                sumData.DistanceMG += rep.DistanceMG / reportList.Count;
                sumData.CourseMG += rep.CourseMG / reportList.Count;
                sumData.Length += rep.Length / reportList.Count;
                sumData.LeftQ += rep.LeftQ / reportList.Count;
                sumData.RightQ += rep.RightQ / reportList.Count;
                sumData.TopQ += rep.TopQ / reportList.Count;
                sumData.BottomQ += rep.BottomQ / reportList.Count;
                sumData.MiddleQ += rep.MiddleQ / reportList.Count;
            }
        }

        //标准差=sqrt( sum( pow(xi-aveX,2))/n);
        private void CalDeviationData(List<DischargeSummary.Report> reportList, DischargeSummary.Report sumData, ref DischargeSummary.Report DevData)
        {
                for (int i = 0; i < reportList.Count; i++)
                {
                    DischargeSummary.Report rep = new DischargeSummary.Report();
                    rep = reportList[i];

                    DevData.TotalQ += Math.Pow(rep.TotalQ - sumData.TotalQ, 2) / (reportList.Count);
                    DevData.WaterWidth += (float)Math.Pow(rep.WaterWidth - sumData.WaterWidth, 2) / (reportList.Count);
                    DevData.Area += Math.Pow(rep.Area - sumData.Area, 2) / (reportList.Count);
                    DevData.AverageVel += Math.Pow(rep.AverageVel - sumData.AverageVel, 2) / (reportList.Count);
                    DevData.MeanCourse += Math.Pow(rep.MeanCourse - sumData.MeanCourse, 2) / (reportList.Count);
                    DevData.DistanceMG += Math.Pow(rep.DistanceMG - sumData.DistanceMG, 2) / (reportList.Count);
                    DevData.CourseMG += Math.Pow(rep.CourseMG - sumData.CourseMG, 2) / (reportList.Count);
                    DevData.Length += Math.Pow(rep.Length - sumData.Length, 2) / (reportList.Count);
                    DevData.LeftQ += Math.Pow(rep.LeftQ - sumData.LeftQ, 2) / (reportList.Count);
                    DevData.RightQ += Math.Pow(rep.RightQ - sumData.RightQ, 2) / (reportList.Count);
                    DevData.TopQ += Math.Pow(rep.TopQ - sumData.TopQ, 2) / (reportList.Count);
                    DevData.BottomQ += Math.Pow(rep.BottomQ - sumData.BottomQ, 2) / (reportList.Count);
                    DevData.MiddleQ += Math.Pow(rep.MiddleQ - sumData.MiddleQ, 2) / (reportList.Count);

                }
                DevData.TotalQ = Math.Sqrt(DevData.TotalQ);
                DevData.WaterWidth = (float)Math.Sqrt(DevData.WaterWidth);
                DevData.Area = Math.Sqrt(DevData.Area);
                DevData.AverageVel = Math.Sqrt(DevData.AverageVel);
                DevData.MeanCourse = Math.Sqrt(DevData.MeanCourse);
                DevData.DistanceMG = Math.Sqrt(DevData.DistanceMG);
                DevData.CourseMG = Math.Sqrt(DevData.CourseMG);
                DevData.Length = Math.Sqrt(DevData.Length);
                DevData.LeftQ = Math.Sqrt(DevData.LeftQ);
                DevData.RightQ = Math.Sqrt(DevData.RightQ);
                DevData.TopQ = Math.Sqrt(DevData.TopQ);
                DevData.BottomQ = Math.Sqrt(DevData.BottomQ);
                DevData.MiddleQ = Math.Sqrt(DevData.MiddleQ);
            
        }

        private DischargeSummary.Report CalCovariance(DischargeSummary.Report DevData,DischargeSummary.Report MeanData)
        {
            DischargeSummary.Report Cov = new DischargeSummary.Report();
            if (Math.Abs(MeanData.TotalQ )> 1e-10)
                Cov.TotalQ = DevData.TotalQ / MeanData.TotalQ;
            else
                Cov.TotalQ = DevData.TotalQ;

            if (Math.Abs(MeanData.WaterWidth) > 1e-10)
                Cov.WaterWidth = DevData.WaterWidth / MeanData.WaterWidth;
            else
                Cov.WaterWidth = DevData.WaterWidth;

            if (Math.Abs(MeanData.Area) > 1e-10)
                Cov.Area = DevData.Area / MeanData.Area;
            else
                Cov.Area = DevData.Area;

            if (Math.Abs(MeanData.AverageVel) > 1e-10)
                Cov.AverageVel = DevData.AverageVel / MeanData.AverageVel;
            else
                Cov.AverageVel = DevData.AverageVel;

            if (Math.Abs(MeanData.MeanCourse) > 1e-10)
                Cov.MeanCourse = DevData.MeanCourse / MeanData.MeanCourse;
            else
                Cov.MeanCourse = DevData.MeanCourse;

            if (Math.Abs(MeanData.DistanceMG) > 1e-10)
                Cov.DistanceMG = DevData.DistanceMG / MeanData.DistanceMG;
            else
                Cov.DistanceMG = DevData.DistanceMG;

            if (Math.Abs(MeanData.CourseMG) > 1e-10)
                Cov.CourseMG = DevData.CourseMG / MeanData.CourseMG;
            else
                Cov.CourseMG = DevData.CourseMG;

            if (Math.Abs(MeanData.Length) > 1e-10)
                Cov.Length = DevData.Length / MeanData.Length;
            else
                Cov.Length = DevData.Length;

            if (Math.Abs(MeanData.LeftQ) > 1e-10)
                Cov.LeftQ = DevData.LeftQ / MeanData.LeftQ;
            else
                Cov.LeftQ = DevData.LeftQ;

            if (Math.Abs(MeanData.RightQ) > 1e-10)
                Cov.RightQ = DevData.RightQ / MeanData.RightQ;
            else
                Cov.RightQ = DevData.RightQ;

            if (Math.Abs(MeanData.TopQ) > 1e-10)
                Cov.TopQ = DevData.TopQ / MeanData.TopQ;
            else
                Cov.TopQ = DevData.TopQ;

            if (Math.Abs(MeanData.BottomQ) > 1e-10)
                Cov.BottomQ = DevData.BottomQ / MeanData.BottomQ;
            else
                Cov.BottomQ = DevData.BottomQ;

            if (Math.Abs(MeanData.MiddleQ) > 1e-10)
                Cov.MiddleQ = DevData.MiddleQ / MeanData.MiddleQ;
            else
                Cov.MiddleQ = DevData.MiddleQ;

            return Cov;

        }

        private void AddToListViewItem(DischargeSummary.Report sumData, ListViewItem sumItem)
        {
            sumItem.Text = sumData.fileName;
            sumItem.SubItems.Add(sumData.startDate);
            sumItem.SubItems.Add(sumData.startTime);
            sumItem.SubItems.Add(sumData.Duration);
            //sumItem.SubItems.Add(sumData.TotalQ.ToString("0.00") + "(m3/s)"); //LPJ 2013-8-7 cancel
            //sumItem.SubItems.Add(sumData.WaterWidth .ToString("0.00") + "(m)");
            //sumItem.SubItems.Add(sumData.Area .ToString("0.00") + "(m2)");
            //sumItem.SubItems.Add(sumData.AverageVel.ToString("0.00") + "(m/s)");
            //sumItem.SubItems.Add(sumData.MeanCourse .ToString("0.00") + "(Deg)");
            //sumItem.SubItems.Add(sumData.DistanceMG .ToString("0.00") + "(m)");
            //sumItem.SubItems.Add(sumData.CourseMG .ToString("0.00") + "(Deg)");
            //sumItem.SubItems.Add(sumData.Length .ToString("0.00") + "(m)");
            //sumItem.SubItems.Add(sumData.TopQ .ToString("0.00") + "(m3/s)");
            //sumItem.SubItems.Add(sumData.BottomQ .ToString("0.00") + "(m3/s)");
            //sumItem.SubItems.Add(sumData.LeftQ .ToString("0.00") + "(m3/s)");
            //sumItem.SubItems.Add(sumData.RightQ.ToString("0.00") + "(m3/s)");
            //sumItem.SubItems.Add(sumData.MiddleQ.ToString("0.00") + "(m3/s)");

          
            sumItem.SubItems.Add(sumData.WaterWidth.ToString("0.00") );
            sumItem.SubItems.Add(sumData.Area.ToString("0.00") );
            sumItem.SubItems.Add(sumData.TotalQ.ToString("0.00"));
            sumItem.SubItems.Add(sumData.AverageVel.ToString("0.00") );
            sumItem.SubItems.Add(sumData.MeanCourse.ToString("0.00") );
            sumItem.SubItems.Add(sumData.DistanceMG.ToString("0.00") );
            sumItem.SubItems.Add(sumData.CourseMG.ToString("0.00") );
            sumItem.SubItems.Add(sumData.Length.ToString("0.00") );
            sumItem.SubItems.Add(sumData.TopQ.ToString("0.00"));
            sumItem.SubItems.Add(sumData.BottomQ.ToString("0.00") );
            sumItem.SubItems.Add(sumData.LeftQ.ToString("0.00") );
            sumItem.SubItems.Add(sumData.RightQ.ToString("0.00") );
            sumItem.SubItems.Add(sumData.MiddleQ.ToString("0.00") );
        }

        private void toolStripBtnExport_Click(object sender, EventArgs e)
        {
            if (report.Count > 0)
            {
                Setup setSummary = new Setup();
                if (DialogResult.OK == setSummary.ShowDialog())
                {
                    DischargeSummary summary = new DischargeSummary();
                    if (!bEn2Metric) //LPJ 2013-8-22  单位为英制
                    {
                        List<DischargeSummary.Report> report_en = new List<DischargeSummary.Report>();
                        Units projectUnit = new Units();

                        //LPJ 2013-8-22  将公制单位的report中的值转为英制
                        for (int i = 0; i < report.Count; i++)
                        {
                            DischargeSummary.Report reportEn = new DischargeSummary.Report();
                            reportEn = report[i];

                            reportEn.Area = projectUnit.MeterToFeet(report[i].Area, 2);
                            reportEn.AverageDepth = (float)(projectUnit.MeterToFeet(report[i].AverageDepth, 1));
                            reportEn.AverageVel = projectUnit.MeterToFeet(report[i].AverageVel, 1);
                            reportEn.BottomQ = projectUnit.MeterToFeet(report[i].BottomQ, 3);
                            reportEn.DistanceMG = projectUnit.MeterToFeet(report[i].DistanceMG, 1);
                            reportEn.LDis = projectUnit.MeterToFeet(report[i].LDis, 1);
                            reportEn.LeftQ = projectUnit.MeterToFeet(report[i].LeftQ, 1);
                            reportEn.Length = projectUnit.MeterToFeet(report[i].Length, 1);
                            reportEn.MaxDepth = (float)(projectUnit.MeterToFeet(report[i].MaxDepth, 1));
                            reportEn.MaxVel = (float)(projectUnit.MeterToFeet(report[i].MaxVel, 1));
                            reportEn.MiddleQ = projectUnit.MeterToFeet(report[i].MiddleQ, 3);
                            reportEn.RDis = projectUnit.MeterToFeet(report[i].RDis, 1);
                            reportEn.RightQ = projectUnit.MeterToFeet(report[i].RightQ, 3);
                            reportEn.TopQ = projectUnit.MeterToFeet(report[i].TopQ, 3);
                            reportEn.TotalQ = projectUnit.MeterToFeet(report[i].TotalQ, 3);
                            reportEn.WaterWidth = (float)(projectUnit.MeterToFeet(report[i].WaterWidth, 1));

                            report_en.Add(reportEn);
                        }
                        basicMessage.textBinSize = (float)projectUnit.MeterToFeet(basicMessage.textBinSize, 1);
                        basicMessage.textBlankSize = (float)projectUnit.MeterToFeet(basicMessage.textBlankSize, 1);
                        basicMessage.textTransducerDepth = (float)projectUnit.MeterToFeet(basicMessage.textTransducerDepth, 1);

                        summary.btnCreateResultReport(report_en, setSummary.stationInf, basicMessage, bEn2Metric);
                    }
                    else
                    {
                        summary.btnCreateResultReport(report, setSummary.stationInf, basicMessage, bEn2Metric);
                    }

                    MessageBox.Show(Resource1.String310);
                }
                else
                {
                    MessageBox.Show(Resource1.String311);
                }
            }
            else
            {
                MessageBox.Show(Resource1.String312);
            }

        }

        private void toolStripBtnClearAll_Click(object sender, EventArgs e)
        {
            this.listView_Summary.Items.Clear();
            report.Clear();
            
        }

        private void toolStripBtnDelete_Click(object sender, EventArgs e)
        {
            int count = listView_Summary.Items.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (listView_Summary.Items[i].Checked)
                {
                    listView_Summary.Items[i].Remove();
                    report.RemoveAt(i);
                }
            }

        }

        public void RefreshListViewHeader(ListView listviewSummary,bool bEnglish2Metric) //LPJ 2013-8-2 将单位放在listView的标题栏
        {
            if (bEnglish2Metric)
            {
                listviewSummary.Columns[0].Text = Resource1.String260;
                listviewSummary.Columns[1].Text = Resource1.String261;
                listviewSummary.Columns[2].Text = Resource1.String262;
                listviewSummary.Columns[3].Text = Resource1.String113; 

               
                listviewSummary.Columns[4].Text = Resource1.String107 + "(m)";
                listviewSummary.Columns[5].Text = Resource1.String108 + "(m2)";
                listviewSummary.Columns[6].Text = Resource1.String98 + "(m3/s)";
                listviewSummary.Columns[7].Text = Resource1.String109 + "(m/s)";

                listviewSummary.Columns[8].Text = Resource1.String110 + "(Deg)";
                listviewSummary.Columns[9].Text = Resource1.String105 + "(m)";
                listviewSummary.Columns[10].Text = Resource1.String106 + "(Deg)";
                listviewSummary.Columns[11].Text = Resource1.String104 + "(m)";

                listviewSummary.Columns[12].Text = Resource1.String93 + "(m3/s)";
                listviewSummary.Columns[13].Text = Resource1.String95 + "(m3/s)";
                listviewSummary.Columns[14].Text = Resource1.String96 + "(m3/s)";
                listviewSummary.Columns[15].Text = Resource1.String97 + "(m3/s)";
                listviewSummary.Columns[16].Text = Resource1.String94 + "(m3/s)";

            }
            else
            {
                listviewSummary.Columns[0].Text = Resource1.String260;
                listviewSummary.Columns[1].Text = Resource1.String261;
                listviewSummary.Columns[2].Text = Resource1.String262;
                listviewSummary.Columns[3].Text = Resource1.String113;

              
                listviewSummary.Columns[4].Text = Resource1.String107 + "(ft)";
                listviewSummary.Columns[5].Text = Resource1.String108 + "(ft2)"; 
                listviewSummary.Columns[6].Text = Resource1.String98 + "(ft3/s)";
                listviewSummary.Columns[7].Text = Resource1.String109 + "(ft/s)";

                listviewSummary.Columns[8].Text = Resource1.String110 + "(Deg)";
                listviewSummary.Columns[9].Text = Resource1.String105 + "(ft)";
                listviewSummary.Columns[10].Text = Resource1.String106 + "(Deg)";
                listviewSummary.Columns[11].Text = Resource1.String104 + "(ft)";

                listviewSummary.Columns[12].Text = Resource1.String93 + "(ft3/s)";
                listviewSummary.Columns[13].Text = Resource1.String95 + "(ft3/s)";
                listviewSummary.Columns[14].Text = Resource1.String96 + "(ft3/s)";
                listviewSummary.Columns[15].Text = Resource1.String97 + "(ft3/s)";
                listviewSummary.Columns[16].Text = Resource1.String94 + "(ft3/s)";
            }
        }

        private void toolStripBtnPrint_Click(object sender, EventArgs e)
        {
            //将listView的内容保存
            try
            {
                //string file = "C:\\summaryData.txt"; //LPJ 2013-9-24 当操作系统为Win7时，不允许在C盘中创建文件，因此将其改为当前文件路径
                string file = Directory.GetCurrentDirectory() + "\\summaryData.txt";
                FileInfo fi = new FileInfo(file);
                StreamWriter sw = fi.CreateText();
            
                string strColumns = "";
                for (int i = 0; i < listView_Summary.Columns.Count; i++)
                {
                    strColumns += listView_Summary.Columns[i].Text + "\t";
                }
                sw.WriteLine(strColumns);

                for (int i = 0; i < this.listView_Summary.Items.Count; i++)
                {
                    strColumns = "";
                    for (int j = 0; j < listView_Summary.Columns.Count; j++)
                    {
                        strColumns += listView_Summary.Items[i].SubItems[j].Text + "\t";
                    }
                    //sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", listView_Summary.Items[i].Text,
                    //    listView_Summary.Items[i].SubItems[1].Text, listView_Summary.Items[i].SubItems[2].Text, listView_Summary.Items[i].SubItems[3].Text));
                    sw.WriteLine(strColumns);
                }
                sw.Close();
                //System.Diagnostics.Process.Start(file); //显示该文件

                //FileStream获取该路径下的该文件
                FileStream fs = new FileStream(file, FileMode.Open);

                ClassPrint print_cs = new ClassPrint();
                print_cs.StartPrint(fs, "txt"); //streamToPrint是要打印的内容（字节流），streamType是流的类型（txt表示普通文本，image表示图像）
                fs.Close();

                File.Delete(file);
            }
            catch
            {
            }
        }

        private void RefreshSummaryData()
        {
            int iCount = listView_Summary.Items.Count;
            float[] fListViewSummary = new float[13 * iCount];
            string[] strListViewSummary = new string[4 * iCount];

            Units projectUnit = new Units(); //LPJ 2013-8-9

            //将ListView中的值提取出来
            int iData = 0;
            int iStr = 0;
            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    if (j < 4)
                    {
                        strListViewSummary[iStr] = listView_Summary.Items[i].SubItems[j].Text;
                        iStr++;
                    }
                    else
                    {
                        fListViewSummary[iData] = float.Parse(listView_Summary.Items[i].SubItems[j].Text);
                        iData++;
                    }
                }
            }

            for (int i = 0; i < iCount; i++)
            {
                listView_Summary.Items[i].SubItems.Clear();
            }

            //将全部的数值进行替换
            iData = 0;
            iStr = 0;
           
           //将公制转为英制 //新设置的单位为英制,将公制转为英制
            {
                for (int i = 0; i < iCount; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        if (j == 0)
                        {
                            listView_Summary.Items[i].Text = strListViewSummary[iStr];
                            iStr++;
                        }
                        else if (j < 4)
                        {
                            listView_Summary.Items[i].SubItems.Add(strListViewSummary[iStr]);
                            iStr++;
                        }
                        else
                        {
                            switch (j)
                            {
                                case 4:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 1).ToString("0.00"));
                                    break;
                                case 5:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 2).ToString("0.00"));
                                    break;
                                case 6:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 3).ToString("0.00"));
                                    break;
                                case 7:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 1).ToString("0.00"));
                                    break;
                                case 8:
                                    listView_Summary.Items[i].SubItems.Add(fListViewSummary[iData].ToString());
                                    break;
                                case 9:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 1).ToString("0.00"));
                                    break;
                                case 10:
                                    listView_Summary.Items[i].SubItems.Add(fListViewSummary[iData].ToString());
                                    break;
                                case 11:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 1).ToString("0.00"));
                                    break;
                                default:
                                    listView_Summary.Items[i].SubItems.Add(projectUnit.MeterToFeet(fListViewSummary[iData], 3).ToString("0.00"));
                                    break;
                            }
                            iData++;
                        }
                    }
                }
            }
        }

        private void listView_Summary_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            for (int i = 0; i < listView_Summary.Items.Count; i++)
            {
                if (listView_Summary.Items.Count == report.Count + 3)
                {
                    if (i < report.Count)
                    {
                        if (listView_Summary.Items[i].Checked)
                            listView_Summary.Items[i].BackColor = Color.LightGray;
                        else
                            listView_Summary.Items[i].BackColor = Color.White;
                    }
                    else
                    {
                        if (listView_Summary.Items[i].Checked)
                            listView_Summary.Items[i].Checked = false;
                    }
                }
                else
                {
                    if (listView_Summary.Items[i].Checked)
                        listView_Summary.Items[i].BackColor = Color.LightGray;
                    else
                        listView_Summary.Items[i].BackColor = Color.White;
                }
            }
        }

        private void toolStripBtnReCal_Click(object sender, EventArgs e)
        {
            if (listView_Summary.Items.Count > 3)
            {
                RefreshListView(); //LPJ 2013-9-22 在选择某个数据后，刷新汇总表
            }
            else
            {
                listView_Summary.Items.Clear();
            }
        }

        private void RefreshListView()
        {
            int count = listView_Summary.Items.Count - 1;
            for (int i = count; i > count - 3; i--)
            {
                listView_Summary.Items.RemoveAt(i);
            }

            //LPJ 2013-9-18 -----------------------start
            List<DischargeSummary.Report> reportList = new List<DischargeSummary.Report>(); //LPJ 2013-9-18 获取需要进行后处理的数据组
            for (int i = 0; i < report.Count; i++)
            {
                DischargeSummary.Report rep = new DischargeSummary.Report();

                if (!listView_Summary.Items[i].Checked)
                {
                    rep = report[i];
                    reportList.Add(rep);
                }
            }
            //LPJ 2013-9-18 -----------------------end

            DataProcessing(reportList);
        }


        //private struct SummaryData
        //{
        //   public double TotalQ; //总流量
        //   public double WaterWidth; //河宽
        //   public double Area; //总面积
        //   public double AverageVel; //平均流速
        //   public double MeanCourse;//平均流向
        //   public double DistanceMG; //直线距离
        //   public double CourseMG; //直线方向
        //   public double Length; //测量长度
        //   public double TopQ; //顶部流量
        //   public double BottomQ; //底部流量
        //   public double LeftQ; //左岸流量
        //   public double RightQ; //右岸流量
        //   public double MiddleQ; //实测流量
        //};

        //private void Initialization(SummaryData data)
        //{
        //    data.TotalQ = 0;
        //    data.WaterWidth = 0;
        //    data.Area = 0;
        //    data.AverageVel = 0;
        //    data.MeanCourse = 0;
        //    data.DistanceMG = 0;
        //    data.CourseMG = 0;
        //    data.Length = 0;
        //    data.LeftQ = 0;
        //    data.RightQ = 0;
        //    data.TopQ = 0;
        //    data.BottomQ = 0;
        //    data.MiddleQ = 0;
        //}

        private void SetColumnWidth()
        {
            int columnwidth = this.Width / 17;
            for (int i = 0; i < 17; i++)
            {
                listView_Summary.Columns[i].Width = columnwidth;
            }
        }

        private void FrmShowSummary_Resize(object sender, EventArgs e)
        {
            SetColumnWidth();
        }

    }
}
