using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ADCP
{
    class CSection_SectionDisplay
    {
        /// <summary>
        /// 绘制垂线断面的流速图
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        /// <param name="fData">垂线流速</param>
        /// <param name="fDis"></param>
        public void DisplayVelocity(Graphics g, Rectangle rectangle, float[] fData,float[] fDis)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
          
            Pen pline = new Pen(Color.Black, 1);
            Rectangle rect = new Rectangle(55 + rectangle.X, 5 + rectangle.Y, rectangle.Width - 60, rectangle.Height - 20);
            g.DrawRectangle(pline, rect.X, rect.Y, rect.Width, rect.Height);

            #region 横轴最大整数&纵轴最大整数
            int iMaxV = 0; //取整数，纵轴分为5等分
            int iMaxD = 0; //取10的倍数，横轴分为10等分

            for (int i = 0; i < fData.Count(); i++)
            {
                if (iMaxV < fData[i])
                    iMaxV = (int)(fData[i] + 0.5);

                if (iMaxD < fDis[i])
                {
                    iMaxD = ((int)(fDis[i] / 10 + 0.5)) * 10;   //???????
                }
            }
            #endregion

            #region 绘制横线（流速）,分为5段
            Pen pgray = new Pen(Color.WhiteSmoke, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 0; i <= 5; i++)
            {
                if (i > 0 && i < 5)
                {
                    g.DrawLine(pgray, rect.X, rect.Y + rect.Height * i / 5.0f, rect.X + rect.Width, rect.Y + rect.Height * i / 5.0f);
                }
                Point pnt = new Point(17 + rectangle.X, (int)((rectangle.Height - 25.0f) * i / 5.0f - 5 + rectangle.Y));
                g.DrawString((iMaxV / 5 * (5 - i)).ToString("0.00"), font, Brushes.DarkBlue, pnt);
            }
            #endregion

            #region 绘制纵线（起点距）
            for (int i = 0; i <= 10; i++)
            {
                if (i > 0 && i < 10)
                    g.DrawLine(pgray, rect.X + rect.Width * i / 10.0f, rect.Y + rect.Height, rect.X + rect.Width * i / 10.0f, rect.Y);

                int length = ((int)(iMaxD / 10.0f * i)).ToString().Length;
                Point pnt = new Point(rect.X + rect.Width * i / 10 - length * 5, rect.Height - 15 + rect.Y);
                g.DrawString((iMaxD / 10.0f * i).ToString("0.0"), font, Brushes.DarkBlue, pnt);
            }
            #endregion

            #region 绘制横坐标说明（起点距）
            Font font1 = new System.Drawing.Font("Arial Narrow", 11); //LPJ 2013-6-13
            g.DrawString("起点距(m)", font1, Brushes.DarkBlue, new Point(rect.Width / 2 - 10 + rect.X, rect.Y + rect.Height - 10));
            #endregion

            #region 绘制纵坐标说明（流速）
            Point pnt1 = new Point(rectangle.X , rect.Height / 2 - (int)(g.MeasureString("流速(m/s)", font1).Width) / 2 + rect.Y);
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString("流速(m/s)", font1, Brushes.DarkBlue, pnt1, sf);
            #endregion

            #region 绘制主图
            for (int i = 0; i < fData.Count(); i++)
            {
                float x = rect.X + fDis[i] / iMaxD * rect.Width;
                float y = rect.Y + rect.Height - fData[i] / iMaxV * rect.Height;

                Pen p = new Pen(Color.Blue, 2);
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.ArrowAnchor;

                g.DrawLine(p, x, rect.Y + rect.Height, x, y); p.Dispose();
            }
            #endregion
        }

        /// <summary>
        /// 绘制总流量百分百图
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        /// <param name="fData">流量百分比</param>
        /// <param name="fDis"></param>
        public void DisplayTotalDischarge(Graphics g, Rectangle rectangle, float[] fData, float[] fDis)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);

            Pen pline = new Pen(Color.Black, 1);
            Rectangle rect = new Rectangle(55 + rectangle.X, 5 + rectangle.Y, rectangle.Width - 60, rectangle.Height - 20);
            g.DrawRectangle(pline, rect.X, rect.Y, rect.Width, rect.Height);

            #region 横轴最大整数&纵轴最大整数
            float fMaxQ = 0; //
            int iMaxD = 0; //取10的倍数，横轴分为10等分

            for (int i = 0; i < fData.Count(); i++)
            {
                if (fMaxQ < fData[i])
                    fMaxQ = fData[i];

                if (iMaxD < fDis[i])
                {
                    iMaxD = ((int)(fDis[i] / 10 + 0.5)) * 10;   //???????
                }
            }
            #endregion

            #region 绘制横线（流量百分比）,分为5段
            Pen pgray = new Pen(Color.WhiteSmoke, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 0; i <= 5; i++)
            {
                if (i > 0 && i < 5)
                {
                    g.DrawLine(pgray, rect.X, rect.Y + rect.Height * i / 5.0f, rect.X + rect.Width, rect.Y + rect.Height * i / 5.0f);
                }
                Point pnt = new Point(17 + rectangle.X, (int)((rectangle.Height - 25.0f) * i / 5.0f - 5 + rectangle.Y));
                g.DrawString((fMaxQ / 5 * (5 - i)).ToString("0%"), font, Brushes.DarkBlue, pnt);
            }
            #endregion

            #region 绘制纵线（起点距）
            for (int i = 0; i <= 10; i++)
            {
                if (i > 0 && i < 10)
                    g.DrawLine(pgray, rect.X + rect.Width * i / 10.0f, rect.Y + rect.Height, rect.X + rect.Width * i / 10.0f, rect.Y);

                int length = ((int)(iMaxD / 10.0f * i)).ToString().Length;
                Point pnt = new Point(rect.X + rect.Width * i / 10 - length * 5, rect.Height - 15 + rect.Y);
                g.DrawString((iMaxD / 10.0f * i).ToString("0.0"), font, Brushes.DarkBlue, pnt);
            }
            #endregion

            #region 绘制横坐标说明（起点距）
            Font font1 = new System.Drawing.Font("Arial Narrow", 11); //LPJ 2013-6-13
            g.DrawString("起点距(m)", font1, Brushes.DarkBlue, new Point(rect.Width / 2 - 10 + rect.X, rect.Y + rect.Height - 10));
            #endregion

            #region 绘制纵坐标说明（流量百分比）
            Point pnt1 = new Point(rectangle.X, rect.Height / 2 - (int)(g.MeasureString("%总流量", font1).Width) / 2 + rect.Y);
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString("%总流量", font1, Brushes.DarkBlue, pnt1, sf);
            #endregion

            #region 绘制主图
            float fLast_x = 0;
            float fLast_Y = 0;
            for (int i = 0; i < fData.Count(); i++)
            {
                float x = rect.X + fDis[i] / iMaxD * rect.Width;
                float y = rect.Y + rect.Height - fData[i] / fMaxQ * rect.Height;

                Brush brush = Brushes.Green;
                if (fData[i] < 0.05f)
                    brush = Brushes.Green;
                else if (fData[i] >= 0.05 && fData[i] < 0.1)
                    brush = Brushes.Yellow;
                else
                    brush = Brushes.Red;

                if (i > 0)
                {
                    g.FillRectangle(brush, fLast_x + rect.X, rect.Y + fLast_Y, x - fLast_x, rect.Height + rect.Y - fLast_Y);
                }

                fLast_x = x;
                fLast_Y = y;
            }
            #endregion
        }

        public void DisplayDataTable()
        {
            ADCP.DVL_Windows.NoPaintBackGroundListView listViewDataTable = new DVL_Windows.NoPaintBackGroundListView();

            listViewDataTable.Columns[0].Text = "使用";
            listViewDataTable.Columns[1].Text = "开始时间";
            listViewDataTable.Columns[2].Text = "垂线号";
            listViewDataTable.Columns[3].Text = "起点距(m)";
            listViewDataTable.Columns[4].Text = "水深(m)";
            listViewDataTable.Columns[5].Text = "间距";
            listViewDataTable.Columns[6].Text = "面积";
            listViewDataTable.Columns[7].Text = "流速";
            listViewDataTable.Columns[8].Text = "平均流速";
            listViewDataTable.Columns[9].Text = "流向";
            listViewDataTable.Columns[10].Text = "ADCP入水深度";
            listViewDataTable.Columns[11].Text = "总流量";
            listViewDataTable.Columns[12].Text = "%总流量";
            listViewDataTable.Columns[13].Text = "流向偏角";
            listViewDataTable.Columns[14].Text = "平均时间";
            listViewDataTable.Columns[15].Text = "模式";
            listViewDataTable.Columns[16].Text = "流速剖面";
            listViewDataTable.Columns[17].Text = "有效测流垂线采样数";
 
            listViewDataTable.CheckBoxes = true;

            for (int i = 0; i < 10; i++)
            {
                listViewDataTable.Items[i].SubItems[1].Text = "";
                listViewDataTable.Items[i].SubItems[2].Text = "";
                listViewDataTable.Items[i].SubItems[3].Text = "";
                listViewDataTable.Items[i].SubItems[4].Text = "";
                listViewDataTable.Items[i].SubItems[5].Text = "";
                listViewDataTable.Items[i].SubItems[6].Text = "";
                listViewDataTable.Items[i].SubItems[7].Text = "";
                listViewDataTable.Items[i].SubItems[8].Text = "";
                listViewDataTable.Items[i].SubItems[9].Text = "";
                listViewDataTable.Items[i].SubItems[10].Text = "";
                listViewDataTable.Items[i].SubItems[11].Text = "";
                listViewDataTable.Items[i].SubItems[12].Text = "";
                listViewDataTable.Items[i].SubItems[13].Text = "";
                listViewDataTable.Items[i].SubItems[14].Text = "";
                listViewDataTable.Items[i].SubItems[15].Text = "";
                listViewDataTable.Items[i].SubItems[16].Text = "";
            }

        }

    }
}
