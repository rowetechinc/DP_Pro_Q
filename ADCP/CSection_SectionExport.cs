using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace ADCP
{
    class CSection_SectionExport
    {
        public void ExportTable()
        {
            try //LPJ 2012-8-6 安阳水文局在使用过程中出现Bug
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();//创建Excel对象

                if (xlapp == null)
                {
                    return;
                }
                Workbook wb = xlapp.Workbooks.Add(Type.Missing);//创建工作薄  

                Worksheet ws = (Worksheet)wb.Worksheets[0]; //创建工作页   
                ws.Name = "Table";
                CreatNewTable(ws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                ws = null;

                xlapp.Visible = false;
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.InitialDirectory = "C:\\";
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

        private void CreatNewTable(Worksheet ws)
        {
            Range oRng;     //定义一个工作区域

            //设置格式
            ws.Cells[1, 1] = ""; //LPJ 2012-7-26加入编号
            oRng = ws.get_Range("A1", "K1");
            oRng.MergeCells = true;
            oRng.RowHeight = 35;
            oRng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//水平居中

            oRng.Font.Bold = true;

            ws.Cells[2, 1] = "日期：";
            ws.Cells[2, 3] = "";
            ws.Cells[2, 5] = "测次：";
            ws.Cells[2, 7] = "";
            ws.Cells[2, 9] = "气温：";
            ws.Cells[2, 11] = "";

            ws.Cells[3, 1] = "开始时间：";
            ws.Cells[3, 3] = "";
            ws.Cells[3, 5] = "结束时间：";
            ws.Cells[3, 7] = "";
            ws.Cells[3, 9] = "水位(m)：";
            ws.Cells[3, 11] = "";

            ws.Cells[4, 1] = "仪器型号：";
            ws.Cells[4, 3] = "";
            ws.Cells[4, 5] = "序列号：";
            ws.Cells[4, 7] = "";
            ws.Cells[4, 9] = "固件版本：";
            ws.Cells[4, 11] = "";

            ws.Cells[5, 1] = "数据文件：";
            ws.Cells[5, 3] = "";
           
            ws.Cells[6, 1] = "入水深度(m)：";
            ws.Cells[6, 3] = "";
            ws.Cells[6, 5] = "盲区(m)：";
            ws.Cells[6, 7] = "";
            ws.Cells[6, 9] = "单元数：";
            ws.Cells[6, 11] = "";

            ws.Cells[7, 1] = "单元尺寸(m)：";
            ws.Cells[7, 3] = "";
            ws.Cells[7, 5] = "流量计算方法：";
            ws.Cells[7, 7] = "";
            ws.Cells[7, 9] = "开始岸：";
            ws.Cells[7, 11] = "";

            ws.Cells[8, 1] = "左岸系数：";
            ws.Cells[8, 3] = "";
            ws.Cells[8, 5] = "右岸系数：";
            ws.Cells[8, 7] = "";

            for (int i = 2; i < 9; i++)
            {
                if (i != 5)
                {
                    oRng = ws.get_Range("A" + i.ToString(), "B" + i.ToString());
                    oRng.MergeCells = true;
                    oRng = ws.get_Range("C" + i.ToString(), "D" + i.ToString());
                    oRng.MergeCells = true;
                    oRng = ws.get_Range("E" + i.ToString(), "F" + i.ToString());
                    oRng.MergeCells = true;
                    oRng = ws.get_Range("G" + i.ToString(), "H" + i.ToString());
                    oRng.MergeCells = true;
                    oRng = ws.get_Range("I" + i.ToString(), "J" + i.ToString());
                    oRng.MergeCells = true;
                }
                else
                {
                    oRng = ws.get_Range("A" + i.ToString(), "B" + i.ToString());
                    oRng.MergeCells = true;
                    oRng = ws.get_Range("C" + i.ToString(), "K" + i.ToString());
                    oRng.MergeCells = true;
                }
            }

            oRng = ws.get_Range("A2", "K8");
            oRng.RowHeight = 19;

            ws.Cells[9, 1] = "垂线号";
            ws.Cells[9, 2] = "时间";
            ws.Cells[9, 4] = "起点距(m)";
            ws.Cells[9, 5] = "水深(m)";

            ws.Cells[9, 6] = "历时";
            ws.Cells[9, 7] = "平均流速";
            ws.Cells[9, 8] = "流向";
            ws.Cells[9, 9] = "面积(m2)";

            ws.Cells[9, 10] = "垂线流量(m3/s)";
            ws.Cells[9, 11] = "%总流量";

            oRng = ws.get_Range("A9", "K9");
            oRng.RowHeight = 30;

            int iCount = 50;  //总垂线数
            if (iCount < 30)
                iCount = 30;
            for (int i = 0; i < iCount; i++)
            {
                ws.Cells[i + 10, 1] = (i + 1).ToString();
                oRng = ws.get_Range("B" + (i + 10).ToString(), "C" + (i + 10).ToString());
                oRng.MergeCells = true;
            }

            oRng = ws.get_Range("A10", "K" + (iCount + 9).ToString());
            oRng.RowHeight = 15;

            ws.Cells[iCount + 10, 1] = "总流量(m3/s)";
            ws.Cells[iCount + 10, 3] = "";
            ws.Cells[iCount + 10, 4] = "平均流速(m/s)";
            ws.Cells[iCount + 10, 6] = "";
            ws.Cells[iCount + 10, 7] = "平均流向";
            ws.Cells[iCount + 10, 9] = "";
            ws.Cells[iCount + 10, 10] = "水面宽(m)";
            ws.Cells[iCount + 10, 11] = "";

            ws.Cells[iCount + 11, 1] = "总面积(m2)";
            ws.Cells[iCount + 11, 3] = "";
            ws.Cells[iCount + 11, 4] = "最大流速(m/s)";
            ws.Cells[iCount + 11, 6] = "";
            ws.Cells[iCount + 11, 7] = "最大水深(m)";
            ws.Cells[iCount + 11, 9] = "";
            ws.Cells[iCount + 11, 10] = "总不确定度";
            ws.Cells[iCount + 11, 11] = "";

            oRng = ws.get_Range("A" + (iCount + 10).ToString(), "B" + (iCount + 10).ToString());
            oRng.MergeCells = true;

            oRng = ws.get_Range("D" + (iCount + 10).ToString(), "E" + (iCount + 10).ToString());
            oRng.MergeCells = true;

            oRng = ws.get_Range("G" + (iCount + 10).ToString(), "H" + (iCount + 10).ToString());
            oRng.MergeCells = true;

            oRng = ws.get_Range("A" + (iCount + 11).ToString(), "B" + (iCount + 11).ToString());
            oRng.MergeCells = true;

            oRng = ws.get_Range("D" + (iCount + 11).ToString(), "E" + (iCount + 11).ToString());
            oRng.MergeCells = true;

            oRng = ws.get_Range("G" + (iCount + 11).ToString(), "H" + (iCount + 11).ToString());
            oRng.MergeCells = true;

            oRng = ws.get_Range("A" + (iCount + 10).ToString(), "K" + (iCount + 11).ToString());
            oRng.RowHeight = 19;

            oRng = ws.get_Range("A1", "K" + (iCount + 11).ToString());
            oRng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;//细线框 
            oRng.Font.Name = "宋体";
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRng);
            oRng = null;
        }

    }
}
