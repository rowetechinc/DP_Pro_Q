using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;



namespace ADCP
{
    public class DischargeSummary
    {
        public void btnCreateResultReport(List<Report> reportList, Setup.StationInfo sInfor,BasicMessage bMessage,bool bEn2Metric)
        {
                int wsnum = (int)(reportList.Count / 6) + 1; //确定需要创建的工作页数
                try //LPJ 2012-8-6 安阳水文局在使用过程中出现Bug
                {
                    Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();//创建Excel对象

                    if (xlapp == null)
                    {
                        return;
                    }
                    Workbook wb = xlapp.Workbooks.Add(Type.Missing);//创建工作薄  
                    if (wsnum > 3)
                    {
                        wb.Worksheets.Add(Type.Missing, Type.Missing, wsnum - 3, Type.Missing);
                    }

                    FrmProgressBar frmProgressBar = new FrmProgressBar(0, wsnum); //LPJ 2013-5-31
                    frmProgressBar.Show(); //LPJ 2013-5-31

                    for (int i = 1; i <= wsnum; i++)
                    {
                        frmProgressBar.setPos(i); //LPJ 2013-5-31

                        Worksheet ws = (Worksheet)wb.Worksheets[i]; //创建工作页   
                        ws.Name = "Table" + i.ToString();
                        CreatNewReport(ws, i, reportList, sInfor, bMessage, bEn2Metric);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                        ws = null;
                    }
                    frmProgressBar.Close(); //LPJ 2013-5-31

                    xlapp.Visible = false;
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = "C:\\";    //LPJ 2013-4-12 当电脑没有E盘时，出错
                    dlg.Filter = "(*.xls)|*.xls|All Files|*.*";
                    dlg.Title = Resource1.String192;
                    if (dlg.ShowDialog() == DialogResult.OK)  //保存结果
                    {
                        xlapp.ActiveWorkbook.SaveAs(@dlg.FileName); 
                        
                        wb.Close();
                    }

                    //释放对象 
                  
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);// 释放Workbook对象
                    wb = null;

                    xlapp.Quit();  // 关闭Excel
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlapp); // 释放Excel对象
                    xlapp = null;
                    GC.Collect(); // 调用垃圾回收
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }

        private void CreatNewReport(Worksheet ws, int wsnum, List<Report> reportList,Setup.StationInfo sInfor,BasicMessage bmessage,bool bEn2Metric)
        {
            Range oRng;     //定义一个工作区域

            //设置格式
            ws.Cells[1, 1] = Resource1.String132 + wsnum.ToString() + "   " + sInfor.station + Resource1.String133; //LPJ 2012-7-26加入编号
            oRng = ws.get_Range("A1", "I1");
            oRng.MergeCells = true;
            oRng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//水平居中

            oRng.Font.Bold = true;
          
            //ws.Cells[2, 1] = "日期: " + year + " 年" + month + " 月" + day + " 日";
            ws.Cells[2, 1] = Resource1.String134 + bmessage.year + Resource1.String135 + bmessage.month + Resource1.String136 + bmessage.day + Resource1.String137; //LPJ 2012-7-12 change

            ws.Cells[2, 4] = Resource1.String138 + sInfor.weather;
            ws.Cells[2, 7] = Resource1.String139 + sInfor.wind;
            oRng = ws.get_Range("A2", "C2");
            oRng.MergeCells = true;
            oRng = ws.get_Range("D2", "F2");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G2", "I2");
            oRng.MergeCells = true;
          
            ws.Cells[3, 1] = Resource1.String140 + reportList.Count;
            ws.Cells[3, 4] = Resource1.String141 + sInfor.surveyboat;
            ws.Cells[3, 7] = Resource1.String142 + sInfor.computer;
            oRng = ws.get_Range("A3", "C3");
            oRng.MergeCells = true;
            oRng = ws.get_Range("D3", "F3");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G3", "I3");
            oRng.MergeCells = true;
          
            //ws.Cells[4, 1] = "开始时间：" + starthour + ":" + startmin + ":" + startsec;
            //ws.Cells[4, 4] = "结束时间：" + endhour + ":" + endmin + ":" + endsec;
            //long timelength = (endhour * 3600 + endmin * 60 + endsec) - (starthour * 3600 + startmin * 60 + startsec);

            //ws.Cells[4, 1] = Resource1.String143 + bmessage.starthour + ":" + bmessage.startmin + ":" + bmessage.startsec; //LPJ 2012-7-12 change
            //ws.Cells[4, 4] = Resource1.String144 + bmessage.endhour + ":" + bmessage.endmin + ":" + bmessage.endsec;

            //long timelength = (bmessage.endhour * 3600 + bmessage.endmin * 60 + bmessage.endsec) - (bmessage.starthour * 3600 + bmessage.startmin * 60 + bmessage.startsec);
            //ws.Cells[4, 7] = Resource1.String145 + timelength / 60 + "min";

            ws.Cells[4, 1] = Resource1.String143 + bmessage.starthour.ToString("00") + ":" + bmessage.startmin.ToString("00"); //LPJ 2012-7-12 change
            ws.Cells[4, 4] = Resource1.String144 + bmessage.endhour.ToString("00") + ":" + bmessage.endmin.ToString("00");

            long timelength = ((bmessage.endhour * 60 + bmessage.endmin) - (bmessage.starthour * 60 + bmessage.startmin)) / 2;   //LPJ 2016-10-10 时间格式为时：分，平均时间=（开始时间+结束时间）/2
            int iHour = (int)timelength / 60 + (int)(bmessage.starthour);
            int iMinute = (int)timelength - (int)(timelength / 60) * 60 + (int)bmessage.startmin;

            if (iMinute >= 60)
            {
                iHour += 1;
                iMinute -= 60;
            }
            if (iHour >= 24)
                iHour -= 24;

            ws.Cells[4, 7] = Resource1.String145 + iHour.ToString("00") + ":" + iMinute.ToString("00");

            oRng = ws.get_Range("A4", "C4");
            oRng.MergeCells = true;
            oRng = ws.get_Range("D4", "F4");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G4", "I4");
            oRng.MergeCells = true;

            ws.Cells[5, 1] = Resource1.String146 + sInfor.kinemometer;
            ws.Cells[5, 4] = Resource1.String147 + sInfor.hardware;
            ws.Cells[5, 7] = Resource1.String148 + sInfor.software;
            oRng = ws.get_Range("A5", "C5");
            oRng.MergeCells = true;
            oRng = ws.get_Range("D5", "F5");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G5", "I5");
            oRng.MergeCells = true;
          
            ws.Cells[6, 1] = Resource1.String151 + sInfor.GPS;
            ws.Cells[6, 4] = Resource1.String152 + sInfor.Compass;
            ws.Cells[6, 7] = Resource1.String153 + sInfor.acousticSounder;
            oRng = ws.get_Range("A6", "C6");
            oRng.MergeCells = true;
            oRng = ws.get_Range("D6", "F6");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G6", "I6");
            oRng.MergeCells = true;
          
            //ws.Cells[7, 1] = "数据文件路径：" + filepath;
            ws.Cells[7, 1] = Resource1.String149 + bmessage.filepath; //LPJ 2012-7-12 change
            ws.Cells[7, 6] = Resource1.String150;
            oRng = ws.get_Range("A7", "E7");
            oRng.MergeCells = true;
            oRng = ws.get_Range("F7", "I7");
            oRng.MergeCells = true;
         
            //ws.Cells[8, 1] = "探头入水深: " + textTransducerDepth.ToString()+"m";
            //ws.Cells[8, 3] = "设置的盲区：" + textBlankSize.ToString() + "m";
            //ws.Cells[8, 5] = "深度单元尺寸:" + textBinSize.ToString() + "m";
            //ws.Cells[8, 7] = "深度单元数：" + textBinNum.ToString();

            if (bEn2Metric)
            {
                ws.Cells[8, 1] = Resource1.String154 + bmessage.textTransducerDepth.ToString("0.00") + "m"; //LPJ 2012-7-12 change
                ws.Cells[8, 3] = Resource1.String155 + bmessage.textBlankSize.ToString("0.00") + "m";
                ws.Cells[8, 5] = Resource1.String156 + bmessage.textBinSize.ToString("0.00") + "m";
            }
            else
            {
                ws.Cells[8, 1] = Resource1.String154 + bmessage.textTransducerDepth.ToString("0.00") + "ft"; //LPJ 2012-7-12 change
                ws.Cells[8, 3] = Resource1.String155 + bmessage.textBlankSize.ToString("0.00") + "ft";
                ws.Cells[8, 5] = Resource1.String156 + bmessage.textBinSize.ToString("0.00") + "ft";
            }

            ws.Cells[8, 7] = Resource1.String157 + bmessage.textBinNum.ToString();
            oRng = ws.get_Range("A8", "B8");
            oRng.MergeCells = true;
            oRng = ws.get_Range("C8", "D8");
            oRng.MergeCells = true;
            oRng = ws.get_Range("E8", "F8");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G8", "I8");
            oRng.MergeCells = true;
          
            //ws.Cells[9, 1] = "含盐度：" + textWaterSalinity.ToString() + "%";
            //ws.Cells[9, 3] = "水跟踪脉冲数：" + textWaterXmt.ToString();

            ws.Cells[9, 1] = Resource1.String158 + bmessage.textWaterSalinity.ToString("0.00") + "%"; //LPJ 2012-7-12 change
            ws.Cells[9, 3] = Resource1.String159 + bmessage.textWaterXmt.ToString();

            ws.Cells[9, 5] = Resource1.String160;
            //ws.Cells[9, 7] = "幂指数b: " + numericUpDownE.ToString();
            ws.Cells[9, 7] = Resource1.String161 + reportList[wsnum - 1].dUpDownCoff.ToString("0.0000"); //LPJ 2012-7-9
            oRng = ws.get_Range("A9", "B9");
            oRng.MergeCells = true;
            oRng = ws.get_Range("C9", "D9");
            oRng.MergeCells = true;
            oRng = ws.get_Range("E9", "F9");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G9", "I9");
            oRng.MergeCells = true;


            ws.Cells[10, 1] = Resource1.String162;
            ws.Cells[10, 2] = Resource1.String163;

            if (bEn2Metric)
            {
                ws.Cells[10, 3] = Resource1.String164 + "(m)"; //LPJ 2013-8-22
                ws.Cells[10, 5] = Resource1.String165;
                //ws.Cells[10, 7] = "半测回流\n量(m3/s)";
                //ws.Cells[10, 8] = "测回平均\n流量(m3/s)";
                ws.Cells[10, 7] = Resource1.String166 + "(m3/s)";  //LPJ 2012-7-26 修改
                ws.Cells[10, 8] = Resource1.String167 + "(m3/s)";
            }
            else
            {
                ws.Cells[10, 3] = Resource1.String164 + "(ft)"; //LPJ 2013-8-22
                ws.Cells[10, 5] = Resource1.String165;
                ws.Cells[10, 7] = Resource1.String166 + "(ft3/s)";  //LPJ 2012-7-26 修改
                ws.Cells[10, 8] = Resource1.String167 + "(ft3/s)";
            }
            ws.Cells[10, 9] = Resource1.String168;
            oRng = ws.get_Range("A10", "A11");
            oRng.MergeCells = true;
            oRng = ws.get_Range("B10", "B11");
            oRng.MergeCells = true;
            oRng = ws.get_Range("C10", "D10");
            oRng.MergeCells = true;
            oRng = ws.get_Range("E10", "F11");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G10", "G11");
            oRng.WrapText = true;  //LPJ 2012-7-26 自动换行
            oRng.MergeCells = true;
            oRng = ws.get_Range("H10", "H11");
            oRng.WrapText = true;  //LPJ 2012-7-26 自动换行
            oRng.MergeCells = true;
            oRng = ws.get_Range("I10", "I11");
            oRng.MergeCells = true;
          

            ws.Cells[11, 3] = "L";
            ws.Cells[11, 4] = "R";

          
            int listnum = 0;
            for (int i = 12; i < 18; i++)
            {
                if (listnum + (wsnum - 1) * 6 < reportList.Count())
                {
                    if (reportList[listnum + (wsnum - 1) * 6].Number % 2 == 0)
                        ws.Cells[i, 1] = ((listnum + (wsnum - 1) * 6) / 2 + 1).ToString();
                    if (reportList[listnum + (wsnum - 1) * 6].RightToLeft)
                    {
                        ws.Cells[i, 2] = Resource1.String169; //航向

                    }
                    else
                    {
                        ws.Cells[i, 2] = Resource1.String170; //航向

                    }
                    ws.Cells[i, 3] = Math.Round(reportList[listnum + (wsnum - 1) * 6].LDis, 1); //左岸水边距离
                    ws.Cells[i, 4] = Math.Round(reportList[listnum + (wsnum - 1) * 6].RDis, 1);  //右岸水边距离
                    ws.Cells[i, 5] = reportList[listnum + (wsnum - 1) * 6].fileName;
                   {
                        string str1 = "E" + i.ToString(); //LPJ 2012-7-26
                        string str2 = "E" + (i + 1).ToString(); //LPJ 2012-7-26
                        oRng = ws.get_Range(str1, str2);
                        oRng.Font.Size = 8; //LPJ 2012-7-26 设置单元格字体大小
                        oRng.WrapText = true;  //LPJ 2012-7-26 自动换行
                    }
                   ws.Cells[i, 7] = CEffectiveNumber.EffectiveNumber(reportList[listnum + (wsnum - 1) * 6].TotalQ, 3, 2); //流量
                   listnum++;
                }

                string str = i.ToString();
                oRng = ws.get_Range("E" + str, "F" + str);
                oRng.MergeCells = true;
                oRng = ws.get_Range("H" + str, "H" + str);
                oRng.MergeCells = true;
                oRng = ws.get_Range("I" + str, "I" + str);
                oRng.MergeCells = true;
             
            }

            oRng = ws.get_Range("A12", "A13");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A14", "A15");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A16", "A17");
            oRng.MergeCells = true;


            double ave = 0;
            int num = 0;
            for (int i = 12; i < 18; i++)
            {
                if (i - 12 + (wsnum - 1) * 6 < reportList.Count())
                {
                    ave += Math.Abs(reportList[i - 12 + (wsnum - 1) * 6].TotalQ);
                    num++;
                    if ((i - 12) % 2 != 0)
                    {
                        ws.Cells[i, 8] = CEffectiveNumber.EffectiveNumber(ave / num, 3, 2); //流量
                        ave = 0;
                        num = 0;
                    }
                }
            }
            for (int i = 12; i < 18; )
            {
                int k = i + 1;
                oRng = ws.get_Range("H" + i.ToString(), "H" + k.ToString());
                oRng.MergeCells = true;
                oRng = ws.get_Range("I" + i.ToString(), "I" + k.ToString());
                oRng.MergeCells = true;
                i = i + 2;
            }

            ws.Cells[18, 1] = Resource1.String171;
            oRng = ws.get_Range("A18", "I18");
            oRng.MergeCells = true;

            ws.Cells[19, 1] = Resource1.String172;
            ws.Cells[19, 3] = Resource1.String173 + "1";
            ws.Cells[19, 5] = Resource1.String173 + "2";
            ws.Cells[19, 7] = Resource1.String173 + "3";
            ws.Cells[19, 9] = Resource1.String174;
            oRng = ws.get_Range("A19", "B20");
            oRng.MergeCells = true;
            oRng = ws.get_Range("C19", "D19");
            oRng.MergeCells = true;
            oRng = ws.get_Range("E19", "F19");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G19", "H19");
            oRng.MergeCells = true;
            oRng = ws.get_Range("I19", "I20");
            oRng.MergeCells = true;

            ws.Cells[20, 3] = Resource1.String175;
            ws.Cells[20, 4] = Resource1.String176;
            ws.Cells[20, 5] = Resource1.String175;
            ws.Cells[20, 6] = Resource1.String176;
            ws.Cells[20, 7] = Resource1.String175;
            ws.Cells[20, 8] = Resource1.String176;

            if (bEn2Metric)
            {
                ws.Cells[21, 1] = Resource1.String177 + "(m3/s)";
                ws.Cells[22, 1] = Resource1.String178 + "(m2)";
                ws.Cells[23, 1] = Resource1.String179 + "(m/s)";
                ws.Cells[24, 1] = Resource1.String180 + "(m/s)";
                ws.Cells[25, 1] = Resource1.String181 + "(m)";
                ws.Cells[26, 1] = Resource1.String182 + "(m)";
                ws.Cells[27, 1] = Resource1.String183 + "(m)";
            }
            else
            {
                ws.Cells[21, 1] = Resource1.String177 + "(ft3/s)";
                ws.Cells[22, 1] = Resource1.String178 + "(ft2)";
                ws.Cells[23, 1] = Resource1.String179 + "(ft/s)";
                ws.Cells[24, 1] = Resource1.String180 + "(ft/s)";
                ws.Cells[25, 1] = Resource1.String181 + "(ft)";
                ws.Cells[26, 1] = Resource1.String182 + "(ft)";
                ws.Cells[27, 1] = Resource1.String183 + "(ft)";
            }

            oRng = ws.get_Range("A21", "B21");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A22", "B22");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A23", "B23");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A24", "B24");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A25", "B25");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A26", "B26");
            oRng.MergeCells = true;
            oRng = ws.get_Range("A27", "B27");
            oRng.MergeCells = true;

            double aveTotalQ = 0;
            double aveArea = 0;
            double aveVel = 0;
            double aveMAxVel = 0;
            double aveDepth = 0;
            double aveMaxDepth = 0;
            double aveWidth = 0;
            num = 0;
            for (int listnum2 = 0; listnum2 < 6; listnum2++)
            {
                if (listnum2 + (wsnum - 1) * 6 < reportList.Count())
                {
                    ws.Cells[21, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(reportList[listnum2 + (wsnum - 1) * 6].TotalQ, 3, 2);  //流量
                    ws.Cells[22, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(reportList[listnum2 + (wsnum - 1) * 6].Area, 3, 2);  //面积

                    double dAverageVel = reportList[listnum2 + (wsnum - 1) * 6].AverageVel; //平均流速
                    if (dAverageVel >= 1)
                        ws.Cells[23, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(dAverageVel, 3, 2);
                    else
                        ws.Cells[23, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(dAverageVel, 2, 3);

                    double dMaxVel = reportList[listnum2 + (wsnum - 1) * 6].MaxVel; //最大流速
                    if (dAverageVel >= 1)
                        ws.Cells[24, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(dMaxVel, 3, 2);
                    else
                        ws.Cells[24, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(dMaxVel, 2, 3);

                    double dAverageDepth = reportList[listnum2 + (wsnum - 1) * 6].AverageDepth; //平均水深
                    if (dAverageDepth >= 100)
                        ws.Cells[25, 3 + listnum2] = (int)dAverageDepth;
                    else if (dAverageDepth >= 5)
                        ws.Cells[25, 3 + listnum2] = Math.Round(dAverageDepth, 1);
                    else
                        ws.Cells[25, 3 + listnum2] = Math.Round(dAverageDepth, 2);

                    double dMaxDepth = reportList[listnum2 + (wsnum - 1) * 6].MaxDepth;  //最大水深
                    if (dMaxDepth >= 100)
                        ws.Cells[26, 3 + listnum2] = (int)dMaxDepth;
                    else if (dMaxDepth >= 5)
                        ws.Cells[26, 3 + listnum2] = Math.Round(dMaxDepth, 1);
                    else
                        ws.Cells[26, 3 + listnum2] = Math.Round(dMaxDepth, 2);

                    double dWidth = reportList[listnum2 + (wsnum - 1) * 6].WaterWidth; //水面宽
                    if (dWidth >= 5)
                        ws.Cells[27, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(dWidth, 3, 1);
                    else
                        ws.Cells[27, 3 + listnum2] = CEffectiveNumber.EffectiveNumber(dWidth, 3, 2);

                    aveTotalQ += Math.Abs(reportList[listnum2 + (wsnum - 1) * 6].TotalQ);
                    aveArea += reportList[listnum2 + (wsnum - 1) * 6].Area;
                    aveVel += reportList[listnum2 + (wsnum - 1) * 6].AverageVel;
                    aveMAxVel += reportList[listnum2 + (wsnum - 1) * 6].MaxVel;
                    aveDepth += reportList[listnum2 + (wsnum - 1) * 6].AverageDepth;
                    aveMaxDepth += reportList[listnum2 + (wsnum - 1) * 6].MaxDepth;
                    aveWidth += reportList[listnum2 + (wsnum - 1) * 6].WaterWidth;
                    num++;
                }
            }

            if (num != 0)
            {
                aveTotalQ = aveTotalQ / num;
                aveArea = aveArea / num;
                aveVel = aveVel / num;
                aveMAxVel = aveMAxVel / num;
                aveDepth = aveDepth / num;
                aveMaxDepth = aveMaxDepth / num;
                aveWidth = aveWidth / num;
            }

            ws.Cells[21, 9] = CEffectiveNumber.EffectiveNumber(aveTotalQ, 3, 2);
            ws.Cells[22, 9] = CEffectiveNumber.EffectiveNumber(aveArea, 3, 2);

            if (aveVel >= 1)
                ws.Cells[23, 9] = CEffectiveNumber.EffectiveNumber(aveVel, 3, 2);
            else
                ws.Cells[23, 9] = CEffectiveNumber.EffectiveNumber(aveVel, 2, 3);

            if (aveMAxVel >= 1)
                ws.Cells[24, 9] = CEffectiveNumber.EffectiveNumber(aveMAxVel, 3, 2);
            else
                ws.Cells[24, 9] = CEffectiveNumber.EffectiveNumber(aveMAxVel, 2, 3);

            if (aveDepth >= 100)
                ws.Cells[25, 9] = (int)aveDepth;
            else if (aveDepth >= 5)
                ws.Cells[25, 9] = Math.Round(aveDepth, 1);
            else
                ws.Cells[25, 9] = Math.Round(aveDepth, 2);

            if (aveMaxDepth >= 5)
                ws.Cells[26, 9] = Math.Round(aveMaxDepth, 1);
            else
                ws.Cells[26, 9] = Math.Round(aveMaxDepth, 2);

            if (aveWidth >= 5)
                ws.Cells[27, 9] = CEffectiveNumber.EffectiveNumber(aveWidth, 3, 1);
            else
                ws.Cells[27, 9] = CEffectiveNumber.EffectiveNumber(aveWidth, 3, 2);

            for (int i = 21; i < 28; i++)
            {
                string str = i.ToString();
                oRng = ws.get_Range("I" + str, "I" + str);
                oRng.MergeCells = true;
               
            }

            if (bEn2Metric)
            {
                ws.Cells[28, 1] = Resource1.String184 + sInfor.startWL + "m";
                ws.Cells[28, 3] = Resource1.String185 + sInfor.endWL + "m";
                ws.Cells[28, 5] = Resource1.String186 + sInfor.MeanWL + "m";
                ws.Cells[28, 7] = Resource1.String187 + "m";
            }
            else
            {
                ws.Cells[28, 1] = Resource1.String184 + sInfor.startWL + "ft";
                ws.Cells[28, 3] = Resource1.String185 + sInfor.endWL + "ft";
                ws.Cells[28, 5] = Resource1.String186 + sInfor.MeanWL + "ft";
                ws.Cells[28, 7] = Resource1.String187 + "ft";
            }

            oRng = ws.get_Range("A28", "B28");
            oRng.MergeCells = true;
            oRng = ws.get_Range("C28", "D28");
            oRng.MergeCells = true;
            oRng = ws.get_Range("E28", "F28");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G28", "I28");
            oRng.MergeCells = true;
            

            ws.Cells[29, 1] = Resource1.String168 + sInfor.remark;
            oRng = ws.get_Range("A29", "I29");
            oRng.MergeCells = true;


            ws.Cells[31, 1] = Resource1.String188 + sInfor.operate;
            ws.Cells[31, 4] = Resource1.String189 + sInfor.check;
            ws.Cells[31, 7] = Resource1.String190 + sInfor.examine;
            oRng = ws.get_Range("A31", "C31");
            oRng.MergeCells = true;
            oRng = ws.get_Range("D31", "F31");
            oRng.MergeCells = true;
            oRng = ws.get_Range("G31", "I31");
            oRng.MergeCells = true;
            
            oRng = ws.get_Range("A10", "I27");
            oRng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//水平居中
            oRng = ws.get_Range("A2", "I29");
            oRng.Font.Name = Resource1.String191; //字体
            //oRng.WrapText = true;  //LPJ 2012-7-26 自动换行
            //oRng.EntireColumn.AutoFit(); 
            oRng.EntireRow.RowHeight = 20;   //LPJ 2012-7-26 修改行高
            // oRng.Font.Bold = true;//粗体
            oRng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;//细线框 

            oRng.NumberFormatLocal = "G/通用格式";  //LPJ 2017-1-5

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRng);
            oRng = null;
        }

        public struct BasicMessage  //LPJ 2012-7-12 add
        {
            public string filepath; //数据文件路径
            public long starthour, startmin, startsec; //开始时间
            public long endhour, endmin, endsec;  //结束时间
            public long year, month, day;  //日期：年月日

            public float textBinSize;  //深度单元尺寸
            public int textBinNum; //深度单元数

            public float textBlankSize; //盲区
            public float textWaterXmt;  //水跟踪脉冲数
            public float textWaterSalinity; //含盐度
            public float textTransducerDepth; //探头入水深
        }

        public struct Report
        {
            public int Number;        //测回测次号
            public string fileName;   //数据文件名
            public string filepath;   //存储路径 //LPJ 2012-7-9 添加原始数据的文件路径

            public bool RightToLeft; //航向
            public double TotalQ;      //断面流量
            public double Area;        //断面面积
            public double AverageVel;  //平均流速
            public float MaxVel;      //最大流速
            public float AverageDepth; //平均水深
            public float MaxDepth;     //最大水深
            public float WaterWidth;    //水面宽

            //JZH 2012-07-08 添加流量计算的配置信息
            public double LDis;        //左岸水边距离
            public double RDis;        //右岸水边距离
            public double dUpDownCoff;  //流量估算幂函数系数

            //LPJ 2013-6-19 添加listViewSummary需要的其他信息
            public string startDate;  //开始日期
            public string startTime;  //开始时间
            public string Duration;   //测量时间
            public double MeanCourse; //平均流向
            public double TopQ;       //顶部流量
            public double BottomQ;    //底部流量
            public double LeftQ;      //左岸流量
            public double RightQ;     //右岸流量
            public double MiddleQ;    //实测流量
            public double DistanceMG; //直线距离
            public double CourseMG;   //直线方向
            public double Length;     //测量长度

        }

        
    }
}
