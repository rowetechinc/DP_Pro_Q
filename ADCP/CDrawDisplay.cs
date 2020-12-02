using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ADCP
{
    class CDrawDisplay
    {
        /// <summary>
        /// Draw contour
        /// </summary>
        /// <param name="DisplayRectangle">rectangle</param>
        /// <param name="e"></param>
        /// <param name="strTitle">title</param>
        /// <param name="rawdata">raw data</param>
        /// <param name="fMinDistance">vertical min</param>
        /// <param name="fMaxDistance">vertical max</param>
        /// <param name="fMinData">data min</param>
        /// <param name="fMaxData">data max</param>
        /// <param name="strVertical">vertical title</param>
        public void OnDrawContour(Rectangle DisplayRectangle, PaintEventArgs e, string strFreq, string strTitle, bool bDirection, string strUnit, List<float[]> rawdata, float fMinDistance, float fMaxDistance, float fMinData, float fMaxData, string strVertical, DateTime[] dt, int iStartEnsemble, int iEndEnsemble, bool bShowTime, bool bDraw, bool bDrawTitle, bool bDrawDepthLine, List<float> fDepth_BT, List<int> iGoodBin, float fBinSize, int iStartCell, bool bDrawTitleTime, bool bBTDepthOn)
        {
            try
            {
                //Draw Contour
                CPaint paint = new CPaint();
                List<Color[]> colorData = new List<Color[]>();

                float fMin = fMinData;
                float fMax = fMaxData;

                /*  //LPJ 2019-7-23 深度从第一个有效单元起算
                if (fMinDistance > fMaxDistance) //LPJ 2019-6-21
                {
                    float b = fMinDistance;
                    fMinDistance = fMaxDistance;
                    fMaxDistance = b;
                }
                 * */

                SizeF sizeF;

                //transfer to color
                for (int i = 0; i < rawdata.Count; i++)
                {
                    float[] fdata = rawdata[i];
                    Color[] cdata = new Color[fdata.Length];
                    int k = 0;
                    for (int j = 0; j < rawdata[i].Length; j++)
                    {
                        try
                        {
                            //cdata[k++] = paint.NumToColor(fdata[j], fMin, fMax);
                            if (bDirection)
                                cdata[k++] = paint.NumToColor6(fdata[j], fMin, fMax);
                            else
                                //cdata[k++] = paint.NumToColor2(fdata[j], fMin, fMax);
                                //cdata[k++] = paint.NumToColor4(fdata[j], fMin, fMax);
                                cdata[k++] = paint.NumToColor5(fdata[j], fMin, fMax);
                        }
                        catch { }


                    }
                    colorData.Add(cdata);
                }

                //paint
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, DisplayRectangle);
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White, DisplayRectangle);
                //g.DrawRectangle(new Pen(Color.Black), DisplayRectangle);

                #region MainContour
                Rectangle rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 2, DisplayRectangle.Width - 110, DisplayRectangle.Height - 17);
                //if (DisplayRectangle.Height < 150)  //LPJ 2018-5-21
                //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 2 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 135 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 17 * DisplayRectangle.Height / 150);

                if (bDrawTitle && bDraw)//有标题和时间
                {
                    rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 15, DisplayRectangle.Width - 110, DisplayRectangle.Height - 30);
                    //if (DisplayRectangle.Height < 150)   //LPJ 2018-5-21
                    //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 135 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 30 * DisplayRectangle.Height / 150);
                }
                else if (bDrawTitle) //有标题
                {
                    rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 15, DisplayRectangle.Width - 110, DisplayRectangle.Height - 17);
                    //if (DisplayRectangle.Height < 150)   //LPJ 2018-5-21
                    //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 135 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 17 * DisplayRectangle.Height / 150);              
                }
                else if (!bDraw && !bDrawTitle) //无标题、无时间
                {
                    rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 2, DisplayRectangle.Width - 110, DisplayRectangle.Height - 4);
                    //if (DisplayRectangle.Height < 150)
                    //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 130 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 20 * DisplayRectangle.Height / 150);
                }

                Font font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);//Font1
                Font font2 = new System.Drawing.Font("Arial Narrow", 8, FontStyle.Bold);//Font2
                Font font3 = new System.Drawing.Font("Arial Narrow", 8);//Font3
                //if (DisplayRectangle.Height < 150)
                //    font = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Height / 150.0f);

                sizeF = TextRenderer.MeasureText(g, strFreq, font2, new Size(0, 0), TextFormatFlags.NoPadding);
                g.FillRectangle(Brushes.White, new Rectangle(rect.X - 40, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                g.DrawString(strFreq, font2, Brushes.Black, new Point(rect.X - 40, rect.Y - 15));
                if (bDrawTitle)
                {
                    sizeF = TextRenderer.MeasureText(g, strTitle, font, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width / 2 - 10, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                    g.DrawString(strTitle, font, Brushes.Black, new Point(rect.X + rect.Width / 2 - 10, rect.Y - 15));
                }

                //Rectangle rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 20, DisplayRectangle.Width - 110, DisplayRectangle.Height - 40);

                if (rect.Height < 0)
                    rect.Height = 1;
                if (rect.Width < 0)
                    rect.Width = 1;
                paint.DrawContour(colorData, g, rect);

                Pen penBlack = new Pen(Brushes.Black, 1);
                g.DrawRectangle(penBlack, rect);
                #endregion

                #region Draw Depth Line
                if (bDrawDepthLine)
                {
                    #region
                    if (bBTDepthOn)  //LPJ 2019-8-7
                    {
                        try
                        {
                            float PixelRectangleHeight = (rect.Height) / ((fMaxDistance - fMinDistance) / fBinSize);
                            float StartX = rect.X;
                            float PixelRectangleWidth = rect.Width * 1.0f / rawdata.Count;
                            for (int i = 0; i < rawdata.Count; i++)
                            {
                                float s1 = 0;
                                if (fDepth_BT.Count > i)  //LPJ 2019-8-13
                                    s1 = (fDepth_BT[i] - fMinDistance) / (fMaxDistance - fMinDistance);
                                if (s1 > 1) s1 = 1;
                                else if (s1 < 0) s1 = 0;

                                float bottomToPanel = s1 * rect.Height;

                                float StartY = rect.Y + (iGoodBin[i + iStartEnsemble] - iStartCell) * PixelRectangleHeight;
                                if (StartY < rect.Y)
                                    StartY = rect.Y;
                                float recHeight = bottomToPanel - (iGoodBin[i + iStartEnsemble] - iStartCell) * PixelRectangleHeight;

                                try
                                {
                                    if (recHeight > 0)
                                    {
                                        //g.FillRectangle(Brushes.Black, StartX, StartY, PixelRectangleWidth, recHeight); //LPJ 2013-7-5 将有效层数以下到底的部分填充黑色 //LPJ 2019-8-8 cancel
                                    }
                                }
                                catch { }

                                g.FillRectangle(Brushes.Gray, StartX, StartY + recHeight, PixelRectangleWidth, rect.Height + rect.Y - (StartY + recHeight));

                                StartX += rect.Width * 1.0f / rawdata.Count;
                            }
                        }
                        catch { }
                    }

                    #endregion

                    PointF[] pnts = new PointF[fDepth_BT.Count];
                    for (int i = 0; i < fDepth_BT.Count; i++)
                    {
                        float y = (fDepth_BT[i] - fMinDistance) / (fMaxDistance - fMinDistance) * rect.Height + rect.Y;
                        float x = rect.X + rect.Width * 1.0f / fDepth_BT.Count * i;

                        if (y < rect.Y)
                            y = rect.Y;

                        if (y > rect.Y + rect.Height)  //LPJ 2019-9-6
                            y = rect.Y + rect.Height;

                        PointF pnt = new PointF(x, y);
                        pnts[i] = pnt;
                    }
                    //g.DrawLines(new Pen(Brushes.WhiteSmoke, 2), pnts);

                    if (pnts.Count() > 0)
                        g.DrawLines(new Pen(Brushes.Black, 2), pnts);  //LPJ 2019-8-7
                }
                #endregion
                #region
                //Font fontData = new System.Drawing.Font("Arial Narrow", 10, FontStyle.Bold);//font
                //if (DisplayRectangle.Height < 150)
                //    fontData = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Height / 150.0f);

                #region

                #region Horizontal ensemble
                if (!bShowTime)
                {
                    int iMulti = 1;
                    if ((iEndEnsemble - iStartEnsemble) <= 10)
                    {
                        iMulti = (int)(((iEndEnsemble - iStartEnsemble) + 1) * 40 / rect.Width) + 1;
                        for (int i = 0; i < (iEndEnsemble - iStartEnsemble) + 1; i++)
                        {
                            float fData = iStartEnsemble + i;
                            float x = rect.X + rect.Width / (float)(iEndEnsemble - iStartEnsemble) * i;
                            g.DrawLine(penBlack, x, rect.Y + rect.Height + 0, x, rect.Y + rect.Height + 2);
                            if (bDraw)
                            {
                                if (i % iMulti == 0)
                                {
                                    sizeF = TextRenderer.MeasureText(g, fData.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                    g.FillRectangle(Brushes.White, new Rectangle((int)x, rect.Y + rect.Height + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                    g.DrawString(fData.ToString("0"), font2, Brushes.Black, new Point((int)x, rect.Y + rect.Height + 2));
                                }
                            }
                        }
                    }
                    else
                    {
                        int n = 10;
                        n = (int)(((iEndEnsemble - iStartEnsemble + 9) / 10.0f) / 10.0f + 1) * 10;
                        int iV = (int)(iEndEnsemble / n);

                        iMulti = (int)(iV * 40 / rect.Width) + 1;
                        for (int i = -1; i < iV + 1; i++)
                        {
                            int iData = (int)(i + 1) * n;
                            float x = (iData - iStartEnsemble) * (rect.Width) / (float)(iEndEnsemble - iStartEnsemble) + rect.X;

                            if (x >= rect.X && x <= rect.X + rect.Width) //in the contour display
                            {
                                g.DrawLine(penBlack, x, rect.Y + rect.Height + 0, x, rect.Y + rect.Height + 2);
                                if (bDraw)
                                {
                                    if (i % iMulti == 0)
                                    {
                                        sizeF = TextRenderer.MeasureText(g, iData.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                        g.FillRectangle(Brushes.White, new Rectangle((int)x - 2, rect.Y + rect.Height + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                        g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point((int)x - 2, rect.Y + rect.Height + 2));
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Horizontal time
                else
                {
                    if (dt.Length > 0) //LPJ 2019-2-13
                    {
                        //if (bDraw)
                        if (bDrawTitleTime)
                        {

                            sizeF = TextRenderer.MeasureText(g, dt[0].ToString("MM/dd/yyyy HH:mm:ss"), font3, new Size(0, 0), TextFormatFlags.NoPadding);
                            g.FillRectangle(Brushes.White, new Rectangle(rect.X, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            g.DrawString(dt[0].ToString("MM/dd/yyyy HH:mm:ss"), font3, Brushes.Black, new Point(rect.X, rect.Y - 15));

                            sizeF = TextRenderer.MeasureText(g, dt[dt.Length - 1].ToString("MM/dd/yyyy HH:mm:ss"), font3, new Size(0, 0), TextFormatFlags.NoPadding);
                            g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width - 100, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            g.DrawString(dt[dt.Length - 1].ToString("MM/dd/yyyy HH:mm:ss"), font3, Brushes.Black, new Point(rect.X + rect.Width - 100, rect.Y - 15));
                        }
                        TimeSpan ts = dt[dt.Length - 1] - dt[0];

                        #region cancel
                        /*
                    if (ts.TotalMinutes <= 60)//10 minute
                    {
                        DateTime dtInterval = dt[0];
                        int iMin = dtInterval.Minute / 10;
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, iMin * 10, 0);
                        dtInterval = dtInterval.AddMinutes(10);

                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH:mm"), fontData, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));

                                dtInterval = dtInterval.AddMinutes(10);
                            }
                        }
                    }
                    if (ts.TotalHours <= 6) //date <6hour.show half hour
                    {
                        DateTime dtInterval = dt[0];
                        if (dt[0].Minute < 30)
                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 30, 0);
                        else
                        {
                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 0, 0);
                            dtInterval = dtInterval.AddHours(1);
                        }

                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH:mm"), fontData, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddMinutes(30);
                            }
                        }
                    }
                    else if (ts.TotalHours <= 12)  //data <24hour, show 1hour
                    {
                        DateTime dtInterval = dt[0];
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 0, 0);
                        dtInterval = dtInterval.AddHours(1);
                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH"), fontData, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddHours(1);
                            }
                        }
                    }
                    else if (ts.TotalHours <= 24)  //data <24hour, show 3hour
                    {
                        DateTime dtInterval = dt[0];
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 0, 0);
                        dtInterval = dtInterval.AddHours(3);
                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH"), fontData, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddHours(3);
                            }
                        }
                    }
                    else  //eles show day
                    {
                        DateTime dtInterval = dt[0];
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, 0, 0, 0);
                        dtInterval = dtInterval.AddDays(1);
                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("dd"), fontData, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddDays(1);
                            }
                        }
                    }
                     * */
                        #endregion

                        #region
                        {
                            int iMulti = 1;   //倍数
                            int n = 0; //显示的个数
                            double dtotalMins = ts.TotalMinutes;  //总分钟数
                            double dTotalDays = ts.TotalDays;     //总日期
                            DateTime dtInterval = dt[0];

                            bool bAddMinte = false;
                            bool bAddHour = false;

                            int iMin = (int)((dtotalMins + 4) / 25) * 5;
                            int iDay = (int)((dTotalDays + 4) / 25) * 5;
                            int iHour = (int)((ts.TotalHours + 3) / 16 + 1) * 4;
                            if (dtotalMins < 120)
                            {
                                if (iMin < 1)
                                    iMin = 1;
                                bAddMinte = true;
                            }
                            else
                            {
                                if (iDay < 1)
                                {
                                    if (dTotalDays < 1)
                                    {
                                        bAddHour = true;
                                    }
                                    else
                                    {
                                        iDay = 1;
                                    }
                                }
                            }

                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, 0, 0, 0);
                            if (bAddMinte)
                            {
                                dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, (int)(dt[0].Minute / iMin) * iMin, 0);
                                dtInterval = dtInterval.AddMinutes(iMin);

                                n = (int)(ts.TotalMinutes / iMin);
                            }
                            else if (bAddHour)
                            {
                                dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, (int)(dt[0].Hour / iHour) * iHour, 0, 0);
                                dtInterval = dtInterval.AddHours(iHour);

                                n = (int)(ts.TotalHours / iHour);
                            }
                            else
                            {
                                dtInterval = dtInterval.AddDays(iDay);
                                n = (int)(ts.TotalDays / iDay);
                            }

                            iMulti = (int)(n * 50 / rect.Width) + 1; //计算倍数
                            if (iMulti < 0.00001) //LPJ 2019-2-15
                                iMulti = 1;

                            int t = 0;
                            for (int i = 1; i < dt.Length; i++)
                            {
                                if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                                {
                                    g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                    if (bDraw)
                                    {
                                        if (t % iMulti == 0)
                                        {
                                            if (bAddMinte)
                                            {
                                                sizeF = TextRenderer.MeasureText(g, dt[i].ToString("HH:mm"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                                g.FillRectangle(Brushes.White, new Rectangle((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                                g.DrawString(dt[i].ToString("HH:mm"), font2, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                            }
                                            else if (bAddHour)
                                            {
                                                sizeF = TextRenderer.MeasureText(g, dt[i].ToString("HH") + ":00", font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                                g.FillRectangle(Brushes.White, new Rectangle((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                                g.DrawString(dt[i].ToString("HH") + ":00", font2, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                            }
                                            else
                                            {
                                                sizeF = TextRenderer.MeasureText(g, dt[i].ToString("MM/dd"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                                g.FillRectangle(Brushes.White, new Rectangle((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                                g.DrawString(dt[i].ToString("MM/dd"), font2, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                            }
                                        }
                                        t++;
                                    }
                                    if (bAddMinte)
                                        dtInterval = dtInterval.AddMinutes(iMin);
                                    else if (bAddHour)
                                        dtInterval = dtInterval.AddHours(iHour);
                                    else
                                        dtInterval = dtInterval.AddDays(iDay);
                                }

                                //add days

                            }
                        }

                        #endregion
                    }
                }
                #endregion

                #endregion
                //Vertical
                StringFormat strF = new StringFormat(StringFormatFlags.DirectionVertical);
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);
                //g.DrawString(strVertical, fontData, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                if (DisplayRectangle.Height < 100)   //LPJ 2018-5-21
                {
                    Font font4 = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Height / 100.0f, FontStyle.Bold);

                    sizeF = TextRenderer.MeasureText(g, strVertical, font4, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVertical, font4, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                }
                else
                {
                    sizeF = TextRenderer.MeasureText(g, strVertical, font, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVertical, font, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                }
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);

                #region Depth
                //draw integate distance

                int Multi = 1;
                //if (DisplayRectangle.Height < 50)
                //    Multi = 4;
                //else if (DisplayRectangle.Height < 150)
                //    Multi = 2;
                if (DisplayRectangle.Height > 150)  //LPJ 2019-8-30
                    Multi = 2;


                int N = 10; //划分为10个间隔
                if (Math.Abs(fMaxDistance - fMinDistance) < 50 && Math.Abs(fMaxDistance - fMinDistance) > 10)
                    N = 5;
                //else if (Math.Abs(fMaxDistance - fMinDistance) <= 10 && Math.Abs(fMaxDistance - fMinDistance) > 1)  //LPJ 2019-8-30
                //    N = 1;
                //else if (Math.Abs(fMaxDistance - fMinDistance) <= 1) //LPJ 2019-8-30
                //    N = 0.1f;

                if (Math.Abs(fMaxDistance - fMinDistance) <= N) //当间隔数少于N，则显示每个的整数位置
                {
                    int iCountlines = (int)(Math.Abs(fMaxDistance - fMinDistance)) + 1;  //LPJ 2019-8-30
                    float iM = 1;
                    if (Math.Abs(fMaxDistance - fMinDistance) <= 1)
                    {
                        iM = 0.1f;
                        iCountlines = (int)(Math.Abs(fMaxDistance - fMinDistance) / 0.1) + 1;  //LPJ 2019-8-30
                    }

                    for (int i = 0; i < iCountlines; i++)
                    {
                        float iData = i * iM + fMinDistance;
                        float y = (iData - fMinDistance) * (rect.Height) / (fMaxDistance - fMinDistance) + rect.Y;
                        if (y >= rect.Y && y <= rect.Y + rect.Height)
                        {
                            g.DrawLine(penBlack, rect.X - 5, y, rect.X, y);
                            //g.DrawString(fData.ToString("0.0"), fontData, Brushes.Black, new Point(rect.X + 10, (int)y - 8));
                            if (i % Multi == 0)
                            {
                                sizeF = TextRenderer.MeasureText(g, iData.ToString("0.0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                g.FillRectangle(Brushes.White, new Rectangle(rect.X - 30, (int)y - 8, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                g.DrawString(iData.ToString("0.0"), font2, Brushes.Black, new Point(rect.X - 30, (int)y - 8));
                            }
                        }
                    }
                }
                else
                {
                    int n = 10;
                    n = (int)(((Math.Abs(fMaxDistance - fMinDistance) + N - 1) / N) / N + 1) * N; //interval value

                    int iV = (int)(fMaxDistance / n);
                    if (fMaxDistance < fMinDistance)
                        iV = (int)(fMinDistance / n);
                    for (int i = -1; i < iV + 1; i++)
                    {
                        int iData = (int)((i + 1) * n);
                        float y = (iData - fMinDistance) * (rect.Height) / (fMaxDistance - fMinDistance) + rect.Y;

                        if (y >= rect.Y && y <= rect.Y + rect.Height) //in the contour display
                        {
                            g.DrawLine(penBlack, rect.X - 5, y, rect.X, y);

                            if (i % Multi == 0)
                            {
                                sizeF = TextRenderer.MeasureText(g, iData.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                g.FillRectangle(Brushes.White, new Rectangle(rect.X - 30, (int)y - 8, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point(rect.X - 30, (int)y - 8));
                            }
                        }
                    }
                }
                #endregion

                //legend
                Rectangle rectLegend = new Rectangle(rect.Width + rect.X + 5, rect.Y, 15, rect.Height);

                //paint.DrawLegend(g, rectLegend);
                int iscale = 4;
                if (bDirection)
                {
                    iscale = 4;
                    paint.DrawLegend3(g, rectLegend);
                }
                else
                {
                    iscale = 4;
                    //paint.DrawLegend2(g, rectLegend);
                    //paint.DrawLegend4(g, rectLegend);

                    paint.DrawLegend5(g, rectLegend);
                }

                if (bDrawTitle)
                {
                    sizeF = TextRenderer.MeasureText(g, strUnit, font2, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(rect.Width + rect.X, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                    g.DrawString(strUnit, font2, Brushes.Black, new Point(rect.Width + rect.X, rect.Y - 15));
                }

                Multi = 1;
                if (DisplayRectangle.Height < 50)
                    Multi = 4;
                else if (DisplayRectangle.Height < 150)
                    Multi = 2;

                for (int i = 0; i < iscale + 1; i++)
                {
                    if (i % Multi == 0)
                    {
                        Point pnt = new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i);
                        if (i == 0)
                            pnt = new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i + 4);
                        if (i == iscale)
                            pnt = new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i - 4);
                        //float fScale = (fMax - fMin) / iscale * i + fMin;
                        float fScale = fMax - (fMax - fMin) / iscale * i;
                        if (fMax >= 10)
                        {
                            sizeF = TextRenderer.MeasureText(g, fScale.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                            //g.FillRectangle(Brushes.White, new Rectangle(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            //g.DrawString(fScale.ToString("0"), font2, Brushes.Black, new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i));
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            g.DrawString(fScale.ToString("0"), font2, Brushes.Black, pnt);
                        }
                        else
                        {
                            sizeF = TextRenderer.MeasureText(g, fScale.ToString("0.0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                            //g.FillRectangle(Brushes.White, new Rectangle(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            //g.DrawString(fScale.ToString("0.0"), font2, Brushes.Black, new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i));
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            g.DrawString(fScale.ToString("0.0"), font2, Brushes.Black, pnt);
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }


        public void OnDrawContour_BS(Rectangle DisplayRectangle, PaintEventArgs e, string strFreq, string strTitle, string strUnit, List<float[]> rawdata, float fMinDistance, float fMaxDistance, float fMinData, float fMaxData, string strVertical, int iStartEnsemble, int iEndEnsemble, bool bDraw, bool bDrawTitle)
        {
            try
            {
                //Draw Contour
                CPaint paint = new CPaint();
                List<Color[]> colorData = new List<Color[]>();

                float fMin = fMinData;
                float fMax = fMaxData;

                /*  //LPJ 2019-7-23 深度从第一个有效单元起算
                if (fMinDistance > fMaxDistance) //LPJ 2019-6-21
                {
                    float b = fMinDistance;
                    fMinDistance = fMaxDistance;
                    fMaxDistance = b;
                }
                 * */

                SizeF sizeF;

                //transfer to color
                for (int i = 0; i < rawdata.Count; i++)
                {
                    float[] fdata = rawdata[i];
                    Color[] cdata = new Color[fdata.Length];
                    int k = 0;
                    for (int j = 0; j < rawdata[i].Length; j++)
                    {
                        try
                        {
                            cdata[k++] = paint.NumToColor5(fdata[j], fMin, fMax);
                        }
                        catch { }


                    }
                    colorData.Add(cdata);
                }

                //paint
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, DisplayRectangle);
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White, DisplayRectangle);
                //g.DrawRectangle(new Pen(Color.Black), DisplayRectangle);

                #region MainContour
                Rectangle rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 2, DisplayRectangle.Width - 110, DisplayRectangle.Height - 17);
                //if (DisplayRectangle.Height < 150)  //LPJ 2018-5-21
                //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 2 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 135 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 17 * DisplayRectangle.Height / 150);

                if (bDrawTitle && bDraw)//有标题和时间
                {
                    rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 15, DisplayRectangle.Width - 110, DisplayRectangle.Height - 30);
                    //if (DisplayRectangle.Height < 150)   //LPJ 2018-5-21
                    //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 135 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 30 * DisplayRectangle.Height / 150);
                }
                else if (bDrawTitle) //有标题
                {
                    rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 15, DisplayRectangle.Width - 110, DisplayRectangle.Height - 17);
                    //if (DisplayRectangle.Height < 150)   //LPJ 2018-5-21
                    //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 135 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 17 * DisplayRectangle.Height / 150);              
                }
                else if (!bDraw && !bDrawTitle) //无标题、无时间
                {
                    rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 2, DisplayRectangle.Width - 110, DisplayRectangle.Height - 4);
                    //if (DisplayRectangle.Height < 150)
                    //    rect = new Rectangle(DisplayRectangle.X + 60 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 130 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 20 * DisplayRectangle.Height / 150);
                }

                Font font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);//Font1
                Font font2 = new System.Drawing.Font("Arial Narrow", 8, FontStyle.Bold);//Font2
                Font font3 = new System.Drawing.Font("Arial Narrow", 8);//Font3
                //if (DisplayRectangle.Height < 150)
                //    font = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Height / 150.0f);

                sizeF = TextRenderer.MeasureText(g, strFreq, font2, new Size(0, 0), TextFormatFlags.NoPadding);
                g.FillRectangle(Brushes.White, new Rectangle(rect.X - 40, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                g.DrawString(strFreq, font2, Brushes.Black, new Point(rect.X - 40, rect.Y - 15));
                if (bDrawTitle)
                {
                    sizeF = TextRenderer.MeasureText(g, strTitle, font, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width / 2 - 10, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                    g.DrawString(strTitle, font, Brushes.Black, new Point(rect.X + rect.Width / 2 - 10, rect.Y - 15));
                }

                //Rectangle rect = new Rectangle(DisplayRectangle.X + 60, DisplayRectangle.Y + 20, DisplayRectangle.Width - 110, DisplayRectangle.Height - 40);

                if (rect.Height < 0)
                    rect.Height = 1;
                if (rect.Width < 0)
                    rect.Width = 1;
                paint.DrawContour(colorData, g, rect);

                Pen penBlack = new Pen(Brushes.Black, 1);
                g.DrawRectangle(penBlack, rect);
                #endregion


                #region
                //Font fontData = new System.Drawing.Font("Arial Narrow", 10, FontStyle.Bold);//font
                //if (DisplayRectangle.Height < 150)
                //    fontData = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Height / 150.0f);

                #region

                #region Horizontal ensemble
                //if (!bShowTime)
                {
                    int iMulti = 1;
                    if ((iEndEnsemble - iStartEnsemble) <= 10)
                    {
                        iMulti = (int)(((iEndEnsemble - iStartEnsemble) + 1) * 40 / rect.Width) + 1;
                        for (int i = 0; i < (iEndEnsemble - iStartEnsemble) + 1; i++)
                        {
                            float fData = iStartEnsemble + i;
                            float x = rect.X + rect.Width / (float)(iEndEnsemble - iStartEnsemble) * i;
                            g.DrawLine(penBlack, x, rect.Y + rect.Height + 0, x, rect.Y + rect.Height + 2);
                            if (bDraw)
                            {
                                if (i % iMulti == 0)
                                {
                                    sizeF = TextRenderer.MeasureText(g, fData.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                    g.FillRectangle(Brushes.White, new Rectangle((int)x, rect.Y + rect.Height + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                    g.DrawString(fData.ToString("0"), font2, Brushes.Black, new Point((int)x, rect.Y + rect.Height + 2));
                                }
                            }
                        }
                    }
                    else
                    {
                        int n = 10;
                        n = (int)(((iEndEnsemble - iStartEnsemble + 9) / 10.0f) / 10.0f + 1) * 10;
                        int iV = (int)(iEndEnsemble / n);

                        iMulti = (int)(iV * 40 / rect.Width) + 1;
                        for (int i = -1; i < iV + 1; i++)
                        {
                            int iData = (int)(i + 1) * n;
                            float x = (iData - iStartEnsemble) * (rect.Width) / (float)(iEndEnsemble - iStartEnsemble) + rect.X;

                            if (x >= rect.X && x <= rect.X + rect.Width) //in the contour display
                            {
                                g.DrawLine(penBlack, x, rect.Y + rect.Height + 0, x, rect.Y + rect.Height + 2);
                                if (bDraw)
                                {
                                    if (i % iMulti == 0)
                                    {
                                        sizeF = TextRenderer.MeasureText(g, iData.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                        g.FillRectangle(Brushes.White, new Rectangle((int)x - 2, rect.Y + rect.Height + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                        g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point((int)x - 2, rect.Y + rect.Height + 2));
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion
                //Vertical
                StringFormat strF = new StringFormat(StringFormatFlags.DirectionVertical);
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);
                //g.DrawString(strVertical, fontData, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                if (DisplayRectangle.Height < 100)   //LPJ 2018-5-21
                {
                    Font font4 = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Height / 100.0f, FontStyle.Bold);

                    sizeF = TextRenderer.MeasureText(g, strVertical, font4, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVertical, font4, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                }
                else
                {
                    sizeF = TextRenderer.MeasureText(g, strVertical, font, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVertical, font, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                }
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);

                #region Depth
                //draw integate distance

                int Multi = 1;
                //if (DisplayRectangle.Height < 50)
                //    Multi = 4;
                //else if (DisplayRectangle.Height < 150)
                //    Multi = 2;
                if (DisplayRectangle.Height > 150)  //LPJ 2019-8-30
                    Multi = 2;


                int N = 10; //划分为10个间隔
                if (Math.Abs(fMaxDistance - fMinDistance) < 50 && Math.Abs(fMaxDistance - fMinDistance) > 10)
                    N = 5;
                //else if (Math.Abs(fMaxDistance - fMinDistance) <= 10 && Math.Abs(fMaxDistance - fMinDistance) > 1)  //LPJ 2019-8-30
                //    N = 1;
                //else if (Math.Abs(fMaxDistance - fMinDistance) <= 1) //LPJ 2019-8-30
                //    N = 0.1f;

                if (Math.Abs(fMaxDistance - fMinDistance) <= N) //当间隔数少于N，则显示每个的整数位置
                {
                    int iCountlines = (int)(Math.Abs(fMaxDistance - fMinDistance)) + 1;  //LPJ 2019-8-30
                    float iM = 1;
                    if (Math.Abs(fMaxDistance - fMinDistance) <= 1)
                    {
                        iM = 0.1f;
                        iCountlines = (int)(Math.Abs(fMaxDistance - fMinDistance) / 0.1) + 1;  //LPJ 2019-8-30
                    }

                    for (int i = 0; i < iCountlines; i++)
                    {
                        float iData = i * iM + fMinDistance;
                        float y = (iData - fMinDistance) * (rect.Height) / (fMaxDistance - fMinDistance) + rect.Y;
                        if (y >= rect.Y && y <= rect.Y + rect.Height)
                        {
                            g.DrawLine(penBlack, rect.X - 5, y, rect.X, y);
                            //g.DrawString(fData.ToString("0.0"), fontData, Brushes.Black, new Point(rect.X + 10, (int)y - 8));
                            if (i % Multi == 0)
                            {
                                sizeF = TextRenderer.MeasureText(g, iData.ToString("0.0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                g.FillRectangle(Brushes.White, new Rectangle(rect.X - 30, (int)y - 8, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                g.DrawString(iData.ToString("0.0"), font2, Brushes.Black, new Point(rect.X - 30, (int)y - 8));
                            }
                        }
                    }
                }
                else
                {
                    int n = 10;
                    n = (int)(((Math.Abs(fMaxDistance - fMinDistance) + N - 1) / N) / N + 1) * N; //interval value

                    int iV = (int)(fMaxDistance / n);
                    if (fMaxDistance < fMinDistance)
                        iV = (int)(fMinDistance / n);
                    for (int i = -1; i < iV + 1; i++)
                    {
                        int iData = (int)((i + 1) * n);
                        float y = (iData - fMinDistance) * (rect.Height) / (fMaxDistance - fMinDistance) + rect.Y;

                        if (y >= rect.Y && y <= rect.Y + rect.Height) //in the contour display
                        {
                            g.DrawLine(penBlack, rect.X - 5, y, rect.X, y);

                            if (i % Multi == 0)
                            {
                                sizeF = TextRenderer.MeasureText(g, iData.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                                g.FillRectangle(Brushes.White, new Rectangle(rect.X - 30, (int)y - 8, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                                g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point(rect.X - 30, (int)y - 8));
                            }
                        }
                    }
                }
                #endregion

                //legend
                Rectangle rectLegend = new Rectangle(rect.Width + rect.X + 5, rect.Y, 15, rect.Height);

                //paint.DrawLegend(g, rectLegend);
                int iscale = 4;

                paint.DrawLegend5(g, rectLegend);


                if (bDrawTitle)
                {
                    sizeF = TextRenderer.MeasureText(g, strUnit, font2, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(rect.Width + rect.X, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                    g.DrawString(strUnit, font2, Brushes.Black, new Point(rect.Width + rect.X, rect.Y - 15));
                }

                Multi = 1;
                if (DisplayRectangle.Height < 50)
                    Multi = 4;
                else if (DisplayRectangle.Height < 150)
                    Multi = 2;

                for (int i = 0; i < iscale + 1; i++)
                {
                    if (i % Multi == 0)
                    {
                        Point pnt = new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i);
                        if (i == 0)
                            pnt = new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i + 4);
                        if (i == iscale)
                            pnt = new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i - 4);
                        //float fScale = (fMax - fMin) / iscale * i + fMin;
                        float fScale = fMax - (fMax - fMin) / iscale * i;
                        if (fMax >= 10)
                        {
                            sizeF = TextRenderer.MeasureText(g, fScale.ToString("0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                            //g.FillRectangle(Brushes.White, new Rectangle(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            //g.DrawString(fScale.ToString("0"), font2, Brushes.Black, new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i));
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            g.DrawString(fScale.ToString("0"), font2, Brushes.Black, pnt);
                        }
                        else
                        {
                            sizeF = TextRenderer.MeasureText(g, fScale.ToString("0.0"), font2, new Size(0, 0), TextFormatFlags.NoPadding);
                            //g.FillRectangle(Brushes.White, new Rectangle(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            //g.DrawString(fScale.ToString("0.0"), font2, Brushes.Black, new Point(rect.Width + rect.X + 25, rect.Y - 8 + rect.Height / iscale * i));
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));
                            g.DrawString(fScale.ToString("0.0"), font2, Brushes.Black, pnt);
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// draw time series
        /// </summary>
        /// <param name="DisplayRectangle"></param>
        /// <param name="e"></param>
        /// <param name="dataLeft">left data</param>
        /// <param name="strTitleLeft">left title</param>
        /// <param name="fScreenLeft"></param>
        /// <param name="fMinLeft"></param>
        /// <param name="fMaxLeft"></param>
        /// <param name="strVerticalLeft"></param>
        /// <param name="dataRight"></param>
        /// <param name="strTitleRight"></param>
        /// <param name="fScreenRight"></param>
        /// <param name="fMinRight"></param>
        /// <param name="fMaxRight"></param>
        /// <param name="strVerticalRight"></param>
        /// <param name="iInterval"></param>
        /// <param name="dtstartTime">horizontal min</param>
        /// <param name="dtendTime">horizontal max</param>
        public void OnDrawTimeSeries_Dual(Rectangle DisplayRectangle, PaintEventArgs e, string strFreq, List<float[]> dataLeft, List<string> strTitleLeft, float fScreenLeft, float fMinLeft, float fMaxLeft, string strVerticalLeft,
            List<float[]> dataRight, List<string> strTitleRight, float fScreenRight, float fMinRight, float fMaxRight, string strVerticalRight, int iInterval, DateTime[] dt, int iStartEnsemble, int iEndEnsemble, bool bShowTime, bool bDraw)
        {
            try
            {
                CPaint paint = new CPaint();
                SizeF sizeF;
                #region
                int iCountLeft = strTitleLeft.Count();
                List<Color> colorLeft = new List<Color>(iCountLeft);
                Color cl = new Color();
                for (int i = 0; i < iCountLeft; i++)
                {
                    //switch (i)
                    //{
                    //case 0:
                    //    cl = Color.Blue;
                    //    break;
                    //case 1:
                    //    cl = Color.DarkRed;
                    //    break;
                    //case 2:
                    //    cl = Color.Green;
                    //    break;
                    //case 3:
                    //    cl = Color.Indigo;
                    //    break;
                    //}

                    #region
                    /*
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Purple;
                            break;
                        case 2:
                            cl = Color.Brown;
                            break;
                        case 3:
                            cl = Color.DodgerBlue;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                     * */
                    #endregion

                    #region
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Red;
                            break;
                        case 2:
                            cl = Color.Green;
                            break;
                        case 3:
                            cl = Color.Fuchsia;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                    #endregion
                    colorLeft.Add(cl);
                }

                int iCountRight = strTitleRight.Count();
                List<Color> colorRight = new List<Color>(iCountRight);
                for (int i = iCountLeft; i < iCountRight + iCountLeft; i++)
                {
                    //switch (i)
                    //{
                    //    case 0:
                    //        cl = Color.OrangeRed;
                    //        break;
                    //    case 1:
                    //        cl = Color.DodgerBlue;
                    //        break;
                    //    case 2:
                    //        cl = Color.Maroon;
                    //        break;
                    //    case 3:
                    //        cl = Color.DarkBlue;
                    //        break;
                    //}

                    #region
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Red;
                            break;
                        case 2:
                            cl = Color.Green;
                            break;
                        case 3:
                            cl = Color.Fuchsia;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                    #endregion
                    colorRight.Add(cl);
                }
                #endregion

                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, DisplayRectangle);
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White, DisplayRectangle);

                #region TimeSeries
                //Rectangle rect = new Rectangle(DisplayRectangle.X + 40, DisplayRectangle.Y + 25, DisplayRectangle.Width - 80, DisplayRectangle.Height - 45);
                Rectangle rect = new Rectangle(DisplayRectangle.X + 40, DisplayRectangle.Y + 16, DisplayRectangle.Width - 80, DisplayRectangle.Height - 30);  //LPJ 2019-9-13
                //if (DisplayRectangle.Height < 150)
                //    rect = new Rectangle(DisplayRectangle.X + 40 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 95 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 35 * DisplayRectangle.Height / 150);

                if (!bDraw)
                {
                    rect = new Rectangle(DisplayRectangle.X + 40, DisplayRectangle.Y + 16, DisplayRectangle.Width - 80, DisplayRectangle.Height - 19);
                    //if (DisplayRectangle.Height < 150)
                    //    rect = new Rectangle(DisplayRectangle.X + 40 * DisplayRectangle.Height / 150, DisplayRectangle.Y + 15 * DisplayRectangle.Height / 150, DisplayRectangle.Width - 95 * DisplayRectangle.Height / 150, DisplayRectangle.Height - 17 * DisplayRectangle.Height / 150);
                }

                if (rect.Height < 0)
                    rect.Height = 1;
                if (rect.Width < 0)
                    rect.Width = 1;

                Pen pline = new Pen(Color.Black, 1);
                g.DrawRectangle(pline, rect.X + 10, rect.Y - 15, rect.Width - 20, rect.Height + 15);  //LPJ 2019-9-13

                paint.DrawMultiLines_Horizontal_Dual(g, rect, dataLeft, fScreenLeft, colorLeft, fMinLeft, fMaxLeft, dataRight, fScreenRight, colorRight, fMinRight, fMaxRight, iInterval);

                g.FillRectangle(Brushes.White, new Rectangle(rect.X + 10, rect.Y - 15, rect.Width - 20, 15)); //LPJ 2019-7-22  //LPJ 2019-9-13
                g.FillRectangle(Brushes.White, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height));  //LPJ 2019-7-22

                #endregion

                #region Title
                //Font font = new System.Drawing.Font("Arial Narrow", 10);//
                //if (rect.Height < 150)
                //    font = new System.Drawing.Font("Arial Narrow", 10 * rect.Height / 150.0f);
                Font font1 = new System.Drawing.Font("Arial Narrow", 8, FontStyle.Bold);//Font2
                Font font2 = new System.Drawing.Font("Arial Narrow", 8);//Font3
                Font font3 = new System.Drawing.Font("Arial", 8, FontStyle.Bold);//Font2

                StringFormat strF = new StringFormat(StringFormatFlags.DirectionVertical);
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);
                if (DisplayRectangle.Height < 100)
                //g.DrawString(strVerticalLeft, font, Brushes.DarkBlue, new Point(-1 * (rect.X - 20 * rect.Height / 150), -1 * (rect.Y + 20 + (rect.Height) / 2)), strF);
                {
                    Font font4 = new System.Drawing.Font("Arial", 10 * DisplayRectangle.Height / 100.0f, FontStyle.Bold);

                    sizeF = TextRenderer.MeasureText(g, strVerticalLeft, font4, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (rect.X - 20), -1 * (rect.Y + 20 + (rect.Height) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVerticalLeft, font4, Brushes.DarkBlue, new Point(-1 * (rect.X - 20), -1 * (rect.Y + 20 + (rect.Height) / 2)), strF);
                }
                else
                //g.DrawString(strVerticalLeft, font, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 20), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 45 - strVerticalLeft.Length) / 2)), strF);
                {
                    sizeF = TextRenderer.MeasureText(g, strVerticalLeft, font3, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (rect.X - 20), -1 * (rect.Y + 20 + (rect.Height) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVerticalLeft, font3, Brushes.DarkBlue, new Point(-1 * (rect.X - 20), -1 * (rect.Y + 20 + (rect.Height) / 2)), strF);
                }
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);

                //g.DrawString(strVerticalRight, font, Brushes.DarkBlue, new Point(DisplayRectangle.X + DisplayRectangle.Width - 20, DisplayRectangle.Y + (DisplayRectangle.Height - 45 - strVerticalRight.Length) / 2), strF);
                if (dataRight.Count > 0)
                {
                    if (DisplayRectangle.Height < 100)
                    {
                        Font font4 = new System.Drawing.Font("Arial", 10 * DisplayRectangle.Height / 100.0f, FontStyle.Bold);

                        sizeF = TextRenderer.MeasureText(g, strVerticalRight, font4, new Size(0, 0), TextFormatFlags.NoPadding);
                        g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width + 22, rect.Y + rect.Height / 2 - 25, Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                        g.DrawString(strVerticalRight, font4, Brushes.DarkBlue, new Point(rect.X + rect.Width + 22, rect.Y + rect.Height / 2 - 25), strF);
                    }
                    else
                    {
                        sizeF = TextRenderer.MeasureText(g, strVerticalRight, font3, new Size(0, 0), TextFormatFlags.NoPadding);
                        g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width + 22, rect.Y + rect.Height / 2 - 25, Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                        g.DrawString(strVerticalRight, font3, Brushes.DarkBlue, new Point(rect.X + rect.Width + 22, rect.Y + rect.Height / 2 - 25), strF);
                    }
                }

                //if (DisplayRectangle.Height < 150)
                //{
                //    g.DrawString(strFreq, font, Brushes.Black, new Point(rect.X, rect.Y - 20 * rect.Height / 150));

                //    for (int i = 0; i < iCountLeft; i++)
                //    {
                //        Pen penHeading = new Pen(colorLeft[i], 2);
                //        g.DrawLine(penHeading, rect.X + 90 + 110 * i, rect.Y - 10 * rect.Height / 150, rect.X + 110 + 110 * i, rect.Y - 10 * rect.Height / 150);
                //        g.DrawString(strTitleLeft[i], font, Brushes.Black, new Point(rect.X + 115 + 110 * i, rect.Y - 20 * rect.Height / 150));
                //    }
                //    for (int i = iCountLeft; i < iCountRight + iCountLeft; i++)
                //    {
                //        Pen penHeading = new Pen(colorRight[i - iCountLeft], 2);
                //        g.DrawLine(penHeading, rect.X + 90 + 110 * i, rect.Y - 10 * rect.Height / 150, rect.X + 110 + 110 * i, rect.Y - 10 * rect.Height / 150);
                //        g.DrawString(strTitleRight[i - iCountLeft], font, Brushes.Black, new Point(rect.X + 115 + 110 * i, rect.Y - 20 * rect.Height / 150));
                //    }
                //}
                //else
                {
                    int idis = 110;  //LPJ 2019-8-20
                    if (iCountLeft + iCountRight > 0)
                    {
                        idis = (int)((rect.Width - 80) * 1.0f / (iCountLeft + iCountLeft));   //LPJ 2019-8-20
                    }
                    if (idis > 110)   //LPJ 2019-8-20
                        idis = 110;
                    else if (idis < 0)  //LPJ 2019-8-20
                        idis = 1;

                    int iFontLength = 20;
                    if (iFontLength > (idis - 20) / 2)  //LPJ 2019-8-20
                        iFontLength = (idis - 20) / 4;

                    if (iFontLength < 0)  //LPJ 2019-8-20
                        iFontLength = 1;

                    Font fnt = font1;
                    if (idis + iFontLength < 90)  //LPJ 2019-8-20
                        fnt = new System.Drawing.Font("Arial Narrow", 7, FontStyle.Regular);

                    sizeF = TextRenderer.MeasureText(g, strFreq, fnt, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(rect.X + 10, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height))); //LPJ 2019-9-13

                    g.DrawString(strFreq, font1, Brushes.Black, new Point(rect.X + 10, rect.Y - 15));  //LPJ 2019-9-13

                    for (int i = 0; i < iCountLeft; i++)
                    {
                        Pen penHeading = new Pen(colorLeft[i], 2);
                        g.DrawLine(penHeading, rect.X + 60 + iFontLength + idis * i, rect.Y - 8, rect.X + 60 + iFontLength + iFontLength + idis * i, rect.Y - 8);   //LPJ 2019-8-20

                        sizeF = TextRenderer.MeasureText(g, strTitleLeft[i], fnt, new Size(0, 0), TextFormatFlags.NoPadding);
                        g.FillRectangle(Brushes.White, new Rectangle(rect.X + 80 + iFontLength + 5 + idis * i, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));   //LPJ 2019-8-20

                        g.DrawString(strTitleLeft[i], fnt, Brushes.Black, new Point(rect.X + 60 + iFontLength + iFontLength + 5 + idis * i, rect.Y - 15));   //LPJ 2019-8-20
                    }
                    for (int i = iCountLeft; i < iCountRight + iCountLeft; i++)
                    {
                        Pen penHeading = new Pen(colorRight[i - iCountLeft], 2);
                        g.DrawLine(penHeading, rect.X + 80 + idis * i, rect.Y - 8, rect.X + 60 + iFontLength + iFontLength + idis * i, rect.Y - 8);   //LPJ 2019-8-20

                        sizeF = TextRenderer.MeasureText(g, strTitleRight[i - iCountLeft], fnt, new Size(0, 0), TextFormatFlags.NoPadding);
                        g.FillRectangle(Brushes.White, new Rectangle(rect.X + 60 + iFontLength + iFontLength + 5 + idis * i, rect.Y - 15, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));    //LPJ 2019-8-20

                        g.DrawString(strTitleRight[i - iCountLeft], fnt, Brushes.Black, new Point(rect.X + 60 + iFontLength + iFontLength + 5 + idis * i, rect.Y - 15));   //LPJ 2019-8-20
                    }
                }

                #endregion

                #region lengend
                /*
                Rectangle rectLengend = new Rectangle(rect.X + rect.Width + 40, DisplayRectangle.Y + 5, 70, DisplayRectangle.Height - 25);
                if (DisplayRectangle.Height < 150)
                    rectLengend = new Rectangle(rect.X + rect.Width + 40, DisplayRectangle.Y + 5 * DisplayRectangle.Height / 150, 70, DisplayRectangle.Height - 25 * DisplayRectangle.Height / 150);
                g.DrawRectangle(new Pen(Brushes.Black, 1), rectLengend);

                int iheight = rectLengend.Height / (iCountLeft + iCountRight + 1);
                g.DrawString(strFreq, font, Brushes.Black, new Point(rectLengend.X + 10, rectLengend.Y + 5));

                for (int i = 0; i < iCountLeft; i++)
                {
                    Pen penHeading = new Pen(colorLeft[i], 2);
                    g.DrawLine(penHeading, rectLengend.X + 5, rectLengend.Y + (int)(iheight * (i + 1.3)), rectLengend.X + 15, rectLengend.Y + (int)(iheight * (i + 1.3)));
                    g.DrawString(strTitleLeft[i], font, Brushes.Black, new Point(rectLengend.X + 20, rectLengend.Y + iheight * (i + 1)));
                }
                for (int i = iCountLeft; i < iCountRight + iCountLeft; i++)
                {
                    Pen penHeading = new Pen(colorRight[i - iCountLeft], 2);
                    g.DrawLine(penHeading, rectLengend.X + 5, rectLengend.Y + (int)(iheight * (i + 1.3)), rectLengend.X + 15, rectLengend.Y + (int)(iheight * (i + 1.3)));
                    g.DrawString(strTitleRight[i - iCountLeft], font, Brushes.Black, new Point(rectLengend.X + 20, rectLengend.Y + iheight * (i + 1)));
                }
                 * */
                #endregion

                #region Horizontal

                //string startTime = dtstartTime.ToString("yyyy-MM-dd HH:mm:ss");
                //string endTime = dtendTime.ToString("yyyy-MM-dd HH:mm:ss");

                //g.DrawString(startTime, font, Brushes.Black, new Point(DisplayRectangle.X + 20, DisplayRectangle.Height + DisplayRectangle.Y - 15));
                //g.DrawString(endTime, font, Brushes.Black, new Point(DisplayRectangle.X - 110 + DisplayRectangle.Width, DisplayRectangle.Height + DisplayRectangle.Y - 15));

                Pen penBlack = new Pen(Color.Black, 1);
                Pen pgray = new Pen(Color.DarkGray, 0.1f);
                pgray.DashStyle = DashStyle.Dash;
                pgray.DashPattern = new float[] { 5, 5 };

                #region Horizontal ensemble
                if (!bShowTime)
                {
                    int iMulti = 1;
                    if ((iEndEnsemble - iStartEnsemble) <= 10)
                    {
                        iMulti = (int)(((iEndEnsemble - iStartEnsemble) + 1) * 40 / rect.Width) + 1;
                        for (int i = 0; i < (iEndEnsemble - iStartEnsemble) + 1; i++)
                        {
                            float fData = iStartEnsemble + i;

                            if (Math.Abs(iEndEnsemble - iStartEnsemble) > 0.000001)  //LPJ 2020-9-17
                            {
                                float x = rect.X + rect.Width / (float)(iEndEnsemble - iStartEnsemble) * i;
                                g.DrawLine(penBlack, x, rect.Y + rect.Height + 0, x, rect.Y + rect.Height + 2);
                                if (bDraw)
                                {
                                    if (i % iMulti == 0)
                                    {
                                        sizeF = TextRenderer.MeasureText(g, fData.ToString("0"), font1, new Size(0, 0), TextFormatFlags.NoPadding);
                                        g.FillRectangle(Brushes.White, new Rectangle((int)x, rect.Y + rect.Height + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                        g.DrawString(fData.ToString("0"), font1, Brushes.Black, new Point((int)x, rect.Y + rect.Height + 2));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int n = 10;
                        n = (int)(((iEndEnsemble - iStartEnsemble + 9) / 10.0f) / 10.0f + 1) * 10;
                        int iV = (int)(iEndEnsemble / n);

                        iMulti = (int)(iV * 40 / rect.Width) + 1;
                        for (int i = -1; i < iV + 1; i++)
                        {
                            int iData = (int)(i + 1) * n;
                            float x = (iData - iStartEnsemble) * (rect.Width) / (float)(iEndEnsemble - iStartEnsemble) + rect.X;

                            if (x >= rect.X && x <= rect.X + rect.Width) //in the contour display
                            {
                                g.DrawLine(penBlack, x, rect.Y + rect.Height + 0, x, rect.Y + rect.Height + 2);
                                if (bDraw)
                                {
                                    if (i % iMulti == 0)
                                    {
                                        sizeF = TextRenderer.MeasureText(g, iData.ToString("0"), font1, new Size(0, 0), TextFormatFlags.NoPadding);
                                        g.FillRectangle(Brushes.White, new Rectangle((int)x - 2, rect.Y + rect.Height + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                        g.DrawString(iData.ToString("0"), font1, Brushes.Black, new Point((int)x - 2, rect.Y + rect.Height + 2));
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Horizontal
                else
                {
                    if (bDraw)
                    {
                        //g.DrawString(dt[0].ToString("yy-MM-dd HH:mm:ss"), font, Brushes.Black, new Point(rect.X-40, rect.Height + rect.Y + 2));
                        //g.DrawString(dt[dt.Length - 1].ToString("HH:mm:ss"), font, Brushes.Black, new Point(rect.X + rect.Width - 10, rect.Height + rect.Y + 2));
                    }

                    TimeSpan ts = new TimeSpan(0);
                    if (dt.Length > 0)   //LPJ 2019-2-13
                        ts = dt[dt.Length - 1] - dt[0];
                    else
                        return;
                    #region cancel
                    /*
                    if (ts.TotalMinutes <= 60)
                    {
                        DateTime dtInterval = dt[0];
                        int iMin = dtInterval.Minute / 10;
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, iMin * 10, 0);
                        dtInterval = dtInterval.AddMinutes(10);

                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(pgray, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Y);
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH:mm:ss"), font, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));

                                dtInterval = dtInterval.AddMinutes(10);
                            }
                        }
                    }
                    if (ts.TotalHours <= 6) //date <6hour.show half hour
                    {
                        DateTime dtInterval = dt[0];
                        if (dt[0].Minute < 30)
                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 30, 0);
                        else
                        {
                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 0, 0);
                            dtInterval = dtInterval.AddHours(1);
                        }

                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(pgray, rect.X + +i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Y);
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH:mm:ss"), font, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddMinutes(30);
                            }
                        }
                    }
                    else if (ts.TotalHours <= 12)  //data <24hour, show 1hour
                    {
                        DateTime dtInterval = dt[0];
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 0, 0);
                        dtInterval = dtInterval.AddHours(1);
                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(pgray, rect.X + +i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Y);
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH:mm:ss"), font, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddHours(1);
                            }
                        }
                    }
                    else if (ts.TotalHours <= 24)  //data <24hour, show 3hour
                    {
                        DateTime dtInterval = dt[0];
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, 0, 0);
                        dtInterval = dtInterval.AddHours(3);
                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(pgray, rect.X + +i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Y);
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("HH:mm:ss"), font, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddHours(3);
                            }
                        }
                    }
                    else  //eles show day
                    {
                        DateTime dtInterval = dt[0];
                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, 0, 0, 0);
                        dtInterval = dtInterval.AddDays(1);
                        for (int i = 1; i < dt.Length - 1; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(pgray, rect.X + +i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Y);
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                    g.DrawString(dt[i].ToString("MM-dd"), font, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                dtInterval = dtInterval.AddDays(1);
                            }
                        }
                    }
                     * 
                     * */
                    #endregion

                    #region
                    {
                        int iMulti = 1;   //倍数
                        int n = 0; //显示的个数

                        double dtotalMins = ts.TotalMinutes;
                        double dTotalDays = ts.TotalDays;
                        DateTime dtInterval = dt[0];

                        bool bAddMinte = false;
                        bool bAddHour = false;

                        int iMin = (int)((dtotalMins + 4) / 25) * 5;
                        int iDay = (int)((dTotalDays + 4) / 25) * 5;
                        int iHour = (int)((ts.TotalHours + 3) / 16 + 1) * 4;
                        if (dtotalMins < 120)
                        {
                            if (iMin < 1)
                                iMin = 1;
                            bAddMinte = true;
                        }
                        else
                        {
                            if (iDay < 1)
                            {
                                if (dTotalDays < 1)
                                {
                                    bAddHour = true;
                                }
                                else
                                {
                                    iDay = 1;
                                }
                            }
                        }

                        dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, 0, 0, 0);
                        if (bAddMinte)
                        {
                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, dt[0].Hour, (int)(dt[0].Minute / iMin) * iMin, 0);
                            dtInterval = dtInterval.AddMinutes(iMin);

                            n = (int)(ts.TotalMinutes / iMin);
                        }
                        else if (bAddHour)
                        {
                            dtInterval = new DateTime(dt[0].Year, dt[0].Month, dt[0].Day, (int)(dt[0].Hour / iHour) * iHour, 0, 0);
                            dtInterval = dtInterval.AddHours(iHour);

                            n = (int)(ts.TotalHours / iHour);
                        }
                        else
                        {
                            dtInterval = dtInterval.AddDays(iDay);
                            n = (int)(ts.TotalDays / iDay);
                        }

                        iMulti = (int)(n * 50 / rect.Width) + 1; //计算倍数
                        if (iMulti < 0.0001)  //LPJ 2019-2-15
                            iMulti = 1;

                        int t = 0;
                        for (int i = 1; i < dt.Length; i++)
                        {
                            if (dtInterval >= dt[i - 1] && dtInterval <= dt[i])
                            {
                                g.DrawLine(penBlack, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y, rect.X + i * 1.0f / dt.Length * rect.Width, rect.Height + rect.Y + 2);
                                if (bDraw)
                                {
                                    if (t % iMulti == 0)
                                    {
                                        if (bAddMinte)
                                        {
                                            sizeF = TextRenderer.MeasureText(g, dt[i].ToString("HH:mm"), font1, new Size(0, 0), TextFormatFlags.NoPadding);
                                            g.FillRectangle(Brushes.White, new Rectangle((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                            g.DrawString(dt[i].ToString("HH:mm"), font1, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                        }
                                        else if (bAddHour)
                                        {
                                            sizeF = TextRenderer.MeasureText(g, dt[i].ToString("HH") + ":00", font1, new Size(0, 0), TextFormatFlags.NoPadding);
                                            g.FillRectangle(Brushes.White, new Rectangle((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                            g.DrawString(dt[i].ToString("HH") + ":00", font1, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                        }
                                        else
                                        {
                                            sizeF = TextRenderer.MeasureText(g, dt[i].ToString("MM/dd"), font1, new Size(0, 0), TextFormatFlags.NoPadding);
                                            g.FillRectangle(Brushes.White, new Rectangle((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                            g.DrawString(dt[i].ToString("MM/dd"), font1, Brushes.Black, new Point((int)(rect.X + i * 1.0f / dt.Length * rect.Width) - 10, rect.Height + rect.Y + 2));
                                        }
                                    }
                                    t++;
                                }
                                if (bAddMinte)
                                    dtInterval = dtInterval.AddMinutes(iMin);
                                else if (bAddHour)
                                    dtInterval = dtInterval.AddHours(iHour);
                                else
                                    dtInterval = dtInterval.AddDays(iDay);
                            }

                            //add days

                        }
                    }

                    #endregion
                }
                #endregion


                #endregion

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// draw vertical
        /// </summary>
        /// <param name="DisplayRectangle"></param>
        /// <param name="e"></param>
        /// <param name="data"></param>
        /// <param name="strTitle"></param>
        /// <param name="iInterval"></param>
        /// <param name="fMinDepth"></param>
        /// <param name="fMaxDepth"></param>
        /// <param name="fScreen"></param>
        /// <param name="fMaxData"></param>
        /// <param name="fMinData"></param>
        /// <param name="strVertical"></param>
        public void OnDrawVertical(Rectangle DisplayRectangle, PaintEventArgs e, List<float[]> data, string strTitle, int iInterval, float fLeftMinDepth, float fLeftMaxDepth, float fRightMinDepth, float fRightMaxDepth, float fScreen, float fMaxData, float fMinData, string strVertical)
        {
            try
            {
                CPaint paint = new CPaint();
                int iCount = data.Count();
                List<Color> color = new List<Color>(iCount);
                Color cl = new Color();
                for (int i = 0; i < iCount; i++)
                {
                    //switch (i)
                    //{
                    //    case 0:
                    //        cl = Color.Blue;
                    //        break;
                    //    case 1:
                    //        cl = Color.Purple;
                    //        break;
                    //    case 2:
                    //        cl = Color.Brown;
                    //        break;
                    //    case 3:
                    //        cl = Color.DarkGreen;
                    //        break;
                    //}

                    #region
                    /*
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Purple;
                            break;
                        case 2:
                            cl = Color.Brown;
                            break;
                        case 3:
                            cl = Color.DarkGreen;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DodgerBlue;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                     * */
                    #endregion
                    #region
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Red;
                            break;
                        case 2:
                            cl = Color.Green;
                            break;
                        case 3:
                            cl = Color.Fuchsia;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                    #endregion

                    color.Add(cl);
                }

                if (fLeftMinDepth > fLeftMaxDepth) //LPJ 2019-6-21
                {
                    float b = fLeftMinDepth;
                    fLeftMinDepth = fLeftMaxDepth;
                    fLeftMaxDepth = b;
                }

                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, DisplayRectangle);
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White, DisplayRectangle);

                Rectangle rect = new Rectangle(DisplayRectangle.X + 35, DisplayRectangle.Y + 20,
                     DisplayRectangle.Width - 40, DisplayRectangle.Height - 40);
                paint.DrawMultiLines_Vertical(g, rect, data, fScreen, color, fMaxData, fMinData);

                g.FillRectangle(Brushes.White, new Rectangle(DisplayRectangle.X, rect.Y, rect.X - DisplayRectangle.X, rect.Height));
                g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width, rect.Y, DisplayRectangle.Width, rect.Height));

                //vertical
                Font font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);//font
                Font font2 = new System.Drawing.Font("Arial Narrow", 8, FontStyle.Bold);//Font3
                Font font3 = new System.Drawing.Font("Arial Narrow", 8);//Font2
                //if (DisplayRectangle.Width < 100)
                //    font = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Width / 100.0f);

                StringFormat strF = new StringFormat(StringFormatFlags.DirectionVertical);
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);
                g.DrawString(strVertical, font, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 15), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);

                //if (iCount > 1)
                //{
                //    g.DrawString(strVertical, font, Brushes.DarkBlue, new Point(DisplayRectangle.X + DisplayRectangle.Width - 15, DisplayRectangle.Y + 7 + (DisplayRectangle.Height - 60) / 2), strF);
                //}

                Pen pgray = new Pen(Color.DarkGray, 0.1f);
                pgray.DashStyle = DashStyle.Dash;
                pgray.DashPattern = new float[] { 5, 5 };

                #region 绘制Left坐标vertical
                int N = 10;
                if (Math.Abs(fLeftMaxDepth - fLeftMinDepth) < 50 && Math.Abs(fLeftMaxDepth - fLeftMinDepth) > 10)
                    N = 5;
                if (Math.Abs(fLeftMaxDepth - fLeftMinDepth) <= N)
                {
                    for (int i = 0; i < Math.Abs(fLeftMaxDepth - fLeftMinDepth) + 1; i++)
                    {
                        int iData = i + (int)fLeftMinDepth;
                        float y = (iData - fLeftMinDepth) * (rect.Height) / (fLeftMaxDepth - fLeftMinDepth) + rect.Y;
                        if (y >= rect.Y && y <= rect.Y + rect.Height)
                        {
                            g.DrawLine(pgray, rect.X, y, rect.X + rect.Width, y);
                            g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point(rect.X - 20, (int)y - 8));
                        }
                    }
                }
                else
                {
                    int n = 10;
                    n = (int)(((Math.Abs(fLeftMaxDepth - fLeftMinDepth) + N - 1) / N) / N + 1) * N; //interval value
                    int iV = (int)(fLeftMaxDepth / n);
                    if (fLeftMaxDepth < fLeftMinDepth)
                        iV = (int)(fLeftMinDepth / n);
                    for (int i = -1; i < iV + 1; i++)
                    {
                        int iData = (int)(i + 1) * n;
                        float y = (iData - fLeftMinDepth) * (rect.Height) / (fLeftMaxDepth - fLeftMinDepth) + rect.Y;

                        if (y >= rect.Y && y <= rect.Y + rect.Height) //in the contour display
                        {
                            g.DrawLine(pgray, rect.X, y, rect.X + rect.Width, y);
                            g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point(rect.X - 20, (int)y - 8));
                        }
                    }
                }
                #endregion

                #region 绘制Right坐标vertical
                /*
                if (iCount > 1)
                {
                    if ((fRightMaxDepth - fRightMinDepth) <= 5)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            float fData = (fRightMaxDepth - fRightMinDepth) / 5 * i + fRightMinDepth;
                            g.DrawString(fData.ToString("0.0"), font, Brushes.Black, new Point(rect.X + rect.Width + 5, rect.Y - 5 + (rect.Height) / 5 * i));
                            g.DrawLine(pgray, rect.X + rect.Width, rect.Height * i / 5 + rect.Y, rect.Width + rect.X + 5, rect.Height * i / 5 + rect.Y);
                        }
                    }
                    else
                    {
                        int n = 5;
                        n = (int)(((fRightMaxDepth - fRightMinDepth) / 6) / 5 + 1) * 5;

                        int icount = (int)((fRightMaxDepth - fRightMinDepth + 1) / n);
                        for (int i = 0; i < icount + 1; i++)
                        {
                            float fData = (fRightMaxDepth - fRightMinDepth) / icount * i + fRightMinDepth;
                            if (i == 0)
                            {
                                fData = fData / n + 1;
                            }
                            else
                            {
                                fData = fData / n;
                            }
                            int iData = (int)(fData) * n;
                            float y = (iData - fRightMinDepth) * (rect.Height) / (fRightMaxDepth - fRightMinDepth) + rect.Y;
                            g.DrawLine(pgray, rect.X + rect.Width, y, rect.X + rect.Width + 5, y);
                            g.DrawString(iData.ToString("0"), font, Brushes.Black, new Point(rect.X + rect.Width + 5, (int)y - 8));
                        }
                    }
                }
                 * */
                #endregion

                //绘制坐标hortical
                for (int i = 0; i < iInterval + 1; i++)
                {
                    Point pnt = new Point((int)(rect.X + rect.Width * i / iInterval - 10), rect.Y - 15);
                    if (i > 0 && i < iInterval)
                    {
                        g.DrawLine(pgray, rect.X + rect.Width * i / iInterval, rect.Height + rect.Y, rect.X + rect.Width * i / iInterval, rect.Y);
                    }
                    //if (i % 2 == 0)
                    {
                        if (Math.Abs(fMaxData - fMinData) > iInterval)
                            g.DrawString((fMinData + (fMaxData - fMinData) / iInterval * i).ToString("0"), font2, Brushes.DarkBlue, pnt);
                        else
                            g.DrawString((fMinData + (fMaxData - fMinData) / iInterval * i).ToString("0.0"), font2, Brushes.DarkBlue, pnt);
                    }
                }

                #region Title
                g.DrawString(strTitle, font, Brushes.Black, new Point((DisplayRectangle.Width - 40) / 2 + DisplayRectangle.X + 10 - strTitle.Length, DisplayRectangle.Y + DisplayRectangle.Height - 15));
                #endregion
            }
            catch { }
        }

        public void OnDrawVertical_Dual(Rectangle DisplayRectangle, PaintEventArgs e, float fLeftMinDepth, float fLeftMaxDepth, List<float[]> data, List<float[]> subdata, string strTitle, float fScreen, float fMaxData, float fMinData,
            List<float[]> Downdata, string strTitleDown, float fScreenDown, float fMaxDataDown, float fMinDataDown, string strVertical, int iInterval)
        {
            try
            {
                CPaint paint = new CPaint();

                /*   //LPJ 2019-7-23
                if (fLeftMinDepth > fLeftMaxDepth) //LPJ 2019-6-21
                {
                    float b = fLeftMinDepth;
                    fLeftMinDepth = fLeftMaxDepth;
                    fLeftMaxDepth = b;
                }
                 * */

                #region color
                int iCount = data.Count();
                List<Color> color = new List<Color>(iCount);
                Color cl = new Color();
                for (int i = 0; i < iCount; i++)
                {
                    #region
                    /*
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Purple;
                            break;
                        case 2:
                            cl = Color.Brown;
                            break;
                        case 3:
                            cl = Color.DodgerBlue;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                     * */
                    #endregion

                    #region
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Red;
                            break;
                        case 2:
                            cl = Color.Green;
                            break;
                        case 3:
                            cl = Color.Fuchsia;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                    #endregion
                    color.Add(cl);
                }

                int idownCount = Downdata.Count();
                List<Color> colorDown = new List<Color>(iCount);
                for (int i = iCount; i < iCount + idownCount; i++)
                {
                    #region
                    switch (i)
                    {
                        case 0:
                            cl = Color.Blue;
                            break;
                        case 1:
                            cl = Color.Red;
                            break;
                        case 2:
                            cl = Color.Green;
                            break;
                        case 3:
                            cl = Color.Fuchsia;
                            break;
                        case 4:
                            cl = Color.DarkOrange;
                            break;
                        case 5:
                            cl = Color.DarkRed;
                            break;
                        case 6:
                            cl = Color.BlueViolet;
                            break;
                        case 7:
                            cl = Color.DarkSlateBlue;
                            break;
                        case 8:
                            cl = Color.DarkCyan;
                            break;
                        case 9:
                            cl = Color.DarkGreen;
                            break;
                        case 10:
                            cl = Color.Indigo;
                            break;
                        case 11:
                            cl = Color.HotPink;
                            break;
                        case 12:
                            cl = Color.OrangeRed;
                            break;
                        case 13:
                            cl = Color.Chocolate;
                            break;
                        case 14:
                            cl = Color.MediumBlue;
                            break;
                        case 15:
                            cl = Color.Teal;
                            break;
                        case 16:
                            cl = Color.MediumOrchid;
                            break;
                        case 17:
                            cl = Color.Firebrick;
                            break;
                        case 18:
                            cl = Color.DarkSlateGray;
                            break;
                        case 19:
                            cl = Color.DarkGreen;
                            break;
                        case 20:
                            cl = Color.Sienna;
                            break;
                    }
                    #endregion
                    colorDown.Add(cl);
                }
                #endregion

                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, DisplayRectangle);
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White, DisplayRectangle);

                int iHeiht = 50;
                if (Downdata.Count > 0) //LPJ 2019-6-17
                    iHeiht = 65;
                Rectangle rect = new Rectangle(DisplayRectangle.X + 35, DisplayRectangle.Y + 30,
                     DisplayRectangle.Width - 40, DisplayRectangle.Height - iHeiht);
                paint.DrawMultiLines_Vertical_Dual(g, rect, data, subdata, fScreen, color, fMaxData, fMinData, Downdata, fScreenDown, colorDown, fMaxDataDown, fMinDataDown);

                g.FillRectangle(Brushes.White, new Rectangle(DisplayRectangle.X, rect.Y, rect.X - DisplayRectangle.X, rect.Height));
                g.FillRectangle(Brushes.White, new Rectangle(rect.X + rect.Width, rect.Y, DisplayRectangle.Width, rect.Height));

                SizeF sizeF;
                //vertical
                //Font font = new System.Drawing.Font("Arial Narrow", 10);//font
                //if (DisplayRectangle.Width < 100)
                //    font = new System.Drawing.Font("Arial Narrow", 10 * DisplayRectangle.Width / 100.0f);

                Font font = new System.Drawing.Font("Arial", 8, FontStyle.Bold);//font
                Font font2 = new System.Drawing.Font("Arial Narrow", 8, FontStyle.Bold);//Font3
                Font font3 = new System.Drawing.Font("Arial Narrow", 8);//Font2

                Pen pgray = new Pen(Color.DarkGray, 0.1f);
                pgray.DashStyle = DashStyle.Dash;
                pgray.DashPattern = new float[] { 5, 5 };

                int Multi_V = 1;
                if (DisplayRectangle.Height < 50)
                    Multi_V = 6;
                else if (DisplayRectangle.Height < 150)
                    Multi_V = 4;
                else if (DisplayRectangle.Height < 250)
                    Multi_V = 2;

                #region 绘制Left坐标vertical
                int N = 10;
                if (Math.Abs(fLeftMaxDepth - fLeftMinDepth) < 50 && Math.Abs(fLeftMaxDepth - fLeftMinDepth) > 10)
                    N = 5;
                if (Math.Abs(fLeftMaxDepth - fLeftMinDepth) <= N)
                {
                    for (int i = 0; i < Math.Abs(fLeftMaxDepth - fLeftMinDepth) + 1; i++)
                    {
                        int iData = i + (int)fLeftMinDepth;
                        float y = (iData - fLeftMinDepth) * (rect.Height) / (fLeftMaxDepth - fLeftMinDepth) + rect.Y;
                        if (y >= rect.Y && y <= rect.Y + rect.Height)
                        {
                            g.DrawLine(pgray, rect.X, y, rect.X + rect.Width, y);
                            if (i % Multi_V == 0)
                            {
                                g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point(rect.X - 20, (int)y - 8));
                            }
                        }
                    }
                }
                else
                {
                    int n = 10;
                    n = (int)(((Math.Abs(fLeftMaxDepth - fLeftMinDepth) + N - 1) / N) / N + 1) * N; //interval value
                    int iV = (int)(fLeftMaxDepth / n);
                    if (fLeftMaxDepth < fLeftMinDepth)
                        iV = (int)(fLeftMinDepth / n);
                    for (int i = -1; i < iV + 1; i++)
                    {
                        int iData = (int)(i + 1) * n;
                        float y = (iData - fLeftMinDepth) * (rect.Height) / (fLeftMaxDepth - fLeftMinDepth) + rect.Y;

                        if (y >= rect.Y && y <= rect.Y + rect.Height) //in the contour display
                        {
                            g.DrawLine(pgray, rect.X, y, rect.X + rect.Width, y);
                            if (i % Multi_V == 0)
                            {
                                g.DrawString(iData.ToString("0"), font2, Brushes.Black, new Point(rect.X - 20, (int)y - 8));
                            }
                        }
                    }
                }
                #endregion

                int Multi = 1;
                if (DisplayRectangle.Width < 40)
                    Multi = 6;
                else if (DisplayRectangle.Width < 60)
                    Multi = 4;
                else if (DisplayRectangle.Width < 100)
                    Multi = 2;
                //绘制坐标uphortical

                #region 
                float unit = (fMaxData - fMinData) / iInterval;
                //LPJ 2019-9-12
                float f = unit - (int)unit;
                string aa = f.ToString("G");
                int result = 0;
                if (aa.Contains("."))
                    result = aa.Length - aa.IndexOf(".") - 1;

                string strFormat = "0.";
                if (result > 0)
                {
                    if (result > 3)  //LPJ 2019-9-13
                        result = 3;
                    string[] strsplit = aa.Split('.');
                    for (int i = 0; i < result; i++)
                    {
                        strFormat += "0";
                    }
                }
                else
                    strFormat = "0";
                #endregion

                for (int i = 0; i < iInterval + 1; i++)
                {
                    Point pnt = new Point((int)(rect.X + rect.Width * i / iInterval - 10), rect.Y - 15);
                    if (i > 0 && i < iInterval)
                    {
                        g.DrawLine(pgray, rect.X + rect.Width * i / iInterval, rect.Height + rect.Y, rect.X + rect.Width * i / iInterval, rect.Y);
                    }

                    if (i % Multi == 0)
                    {
                        //if (Math.Abs(fMaxData - fMinData) > iInterval)  //LPJ 2019-9-12 cancel
                        //    g.DrawString((fMinData + (fMaxData - fMinData) / iInterval * i).ToString("0"), font2, Brushes.Blue, pnt);
                        //else
                        //    g.DrawString((fMinData + (fMaxData - fMinData) / iInterval * i).ToString("0.0"), font2, Brushes.Blue, pnt);

                        g.DrawString((fMinData + (fMaxData - fMinData) / iInterval * i).ToString(strFormat), font2, Brushes.Blue, pnt);  //LPJ 2019-9-12
                    }
                }

                if (DisplayRectangle.Width < 150)
                {
                    Font font4 = new System.Drawing.Font("Arial Narrow", 8 * DisplayRectangle.Width / 150.0f);//Font2
                    g.DrawString(strTitle, font4, Brushes.Blue, new Point((rect.Width - 40) / 2 + rect.X + 10 - strTitle.Length, rect.Y - 30));
                }
                else
                    g.DrawString(strTitle, font, Brushes.Blue, new Point((rect.Width - 40) / 2 + rect.X + 10 - strTitle.Length, rect.Y - 30));

                //绘制坐标downhortical
                if (Downdata.Count > 0)
                {
                    #region
                    unit = (fMaxDataDown - fMinDataDown) / iInterval;
                    //LPJ 2019-9-12
                    f = unit - (int)unit;
                    aa = f.ToString("G");
                    result = 0;
                    if (aa.Contains("."))
                        result = aa.Length - aa.IndexOf(".") - 1;

                    strFormat = "0.";
                    if (result > 0)
                    {
                        if (result > 3)  //LPJ 2019-9-13
                            result = 3;
                        string[] strsplit = aa.Split('.');
                        for (int i = 0; i < result; i++)
                        {
                            strFormat += "0";
                        }
                    }
                    else
                        strFormat = "0";
                    #endregion

                    for (int i = 0; i < iInterval + 1; i++)
                    {
                        Point pnt = new Point((int)(rect.X + rect.Width * i / iInterval - 10), rect.Y + rect.Height);

                        if (i % Multi == 0)
                        {
                            //if (Math.Abs(fMaxDataDown - fMinDataDown) > iInterval)
                            //    g.DrawString((fMinDataDown + (fMaxDataDown - fMinDataDown) / iInterval * i).ToString("0"), font2, Brushes.Red, pnt);  //LPJ 2019-9-12
                            //else
                            //    g.DrawString((fMinDataDown + (fMaxDataDown - fMinDataDown) / iInterval * i).ToString("0.0"), font2, Brushes.Red, pnt);  //LPJ 2019-9-12

                            g.DrawString((fMinDataDown + (fMaxDataDown - fMinDataDown) / iInterval * i).ToString(strFormat), font2, Brushes.Red, pnt);  //LPJ 2019-9-12
                        }
                    }

                    if (DisplayRectangle.Width < 150)
                    {
                        Font font4 = new System.Drawing.Font("Arial Narrow", 8 * DisplayRectangle.Width / 150.0f);//Font2
                        g.DrawString(strTitleDown, font4, Brushes.Red, new Point((rect.Width - 40) / 2 + rect.X + 10 - strTitleDown.Length, rect.Y + rect.Height + 15));
                    }
                    else
                        g.DrawString(strTitleDown, font, Brushes.Red, new Point((rect.Width - 40) / 2 + rect.X + 10 - strTitleDown.Length, rect.Y + rect.Height + 15));
                }

                #region Title


                #endregion

                #region 
                StringFormat strF = new StringFormat(StringFormatFlags.DirectionVertical);
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);

                if (DisplayRectangle.Height < 150)
                {
                    Font font4 = new System.Drawing.Font("Arial Narrow", 8 * DisplayRectangle.Height / 150.0f);//Font2

                    sizeF = TextRenderer.MeasureText(g, strVertical, font4, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (DisplayRectangle.X + 15), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVertical, font, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 15), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                }
                else
                {
                    sizeF = TextRenderer.MeasureText(g, strVertical, font, new Size(0, 0), TextFormatFlags.NoPadding);
                    g.FillRectangle(Brushes.White, new Rectangle(-1 * (DisplayRectangle.X + 15), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2), Convert.ToInt32(sizeF.Height), Convert.ToInt32(sizeF.Width)));

                    g.DrawString(strVertical, font, Brushes.DarkBlue, new Point(-1 * (DisplayRectangle.X + 15), -1 * (DisplayRectangle.Y + 60 + (DisplayRectangle.Height - 60) / 2)), strF);
                }
                g.TranslateTransform(0, 0);
                g.RotateTransform(180.0F);
                #endregion
            }
            catch { }
        }


        public void DrawGPSTrack(Graphics g1, Rectangle rect, int BinDataEnsembleNum, List<float> FirstPingSecond, List<Velocity> BoatV, List<Velocity> fVelocity, List<Velocity> BoatV_GPS, List<Velocity> fVelocity_GPS, ref TrackPanelRef trackRef)
        {
            g1.FillRectangle(Brushes.White, 0, 0, rect.Width, rect.Height);
            try
            {

                trackRef.MainGPSWidth = rect.Width;
                trackRef.MainGPSHeight = rect.Height;

                //Bitmap Bitmap = new Bitmap(pictureBox2.Image);

                trackRef.MaxDisplayHeight = trackRef.MainGPSHeight;

                trackRef.MaxDisplayWidth = trackRef.MainGPSWidth;

                trackRef.DisplayRec = new Rectangle(0, 0, rect.Width, rect.Height);

                trackRef.CurrentDisplayUnit = (float)trackRef.MainGPSWidth / 16;
                //trackRef.CurrentDisplayUnit = (float)trackRef.MainGPSHeight / 16;  

                trackRef.CurrentDisplayTop = rect.Top;
                trackRef.CurrentDisplayLeft = rect.Left;

                trackRef.scaleMultiple = (float)trackRef.MainGPSHeight / trackRef.MaxDisplayHeight;
                //trackRef.scaleMultiple = (float)trackRef.MainGPSWidth / trackRef.MaxDisplayWidth;

                //拖动积累量
                trackRef.DragLengthX = trackRef.DragLengthX * (trackRef.scaleMultiple / trackRef.PreviousMultiple);//改变窗口大小后，相应更新拖动积累量的值
                trackRef.DragLengthY = trackRef.DragLengthY * (trackRef.scaleMultiple / trackRef.PreviousMultiple);
                trackRef.PreviousMultiple = trackRef.scaleMultiple;

                float OrignX = 0;
                float OrignY = 0;

                //g1.DrawImage(Bitmap, 5, 5, 20, 20);

                Font font1 = new Font("Arial", 10);
                //if (Resource1.String237 == labelUnit.Text)
                string strTitle = "Bottom Track(m)";

                #region 图例

                //if (!bEnglish2Metric)
                //{
                //    if (bAutoSwitch) 
                //    {
                //        strTitle = Resources.Resource1.String236 + "(ft)";
                //    }
                //    else
                //    {
                //        if (iFlag == 1)
                //            strTitle = strFrequency1 + Resources.Resource1.String236 + "(ft)";
                //        else
                //            strTitle = strFrequency2 + Resources.Resource1.String236 + "(ft)";
                //    }
                //}
                //else
                //{
                //    if (bAutoSwitch)
                //    {
                //        strTitle = Resources.Resource1.String236 + "(m)";
                //    }
                //    else
                //    {
                //        if (iFlag == 1)
                //            strTitle = strFrequency1 + Resources.Resource1.String236 + "(m)";
                //        else
                //            strTitle = strFrequency2 + Resources.Resource1.String236 + "(m)";
                //    }
                //}
                //if (bGPSTrack && bBottomTrack && bGPSGGATrack)
                //{
                //    g1.DrawRectangle(Pens.Black, new Rectangle(5, rect.Height - 90, 160, 80));
                //    g1.DrawString(strTitle, font1, Brushes.Black, new PointF(25, rect.Height - 87));

                //    g1.DrawLine(new Pen(Brushes.DarkBlue, 2), new Point(15, rect.Height - 60), new Point(55, rect.Height - 60));
                //    g1.DrawString(Resources.Resource1.String130, font1, Brushes.Black, new PointF(60, rect.Height - 67));

                //    g1.DrawLine(new Pen(Brushes.DarkRed, 2), new Point(15, rect.Height - 40), new Point(55, rect.Height - 40));
                //    g1.DrawString("GPS", font1, Brushes.Black, new PointF(60, rect.Height - 47));

                //    g1.DrawLine(new Pen(Brushes.Indigo, 2), new Point(15, rect.Height - 20), new Point(55, rect.Height - 20));
                //    g1.DrawString("GPS GGA", font1, Brushes.Black, new PointF(60, rect.Height - 27));
                //}
                //else if (!bGPSTrack && !bBottomTrack && !bGPSGGATrack)
                //{
                //    g1.DrawRectangle(Pens.Black, new Rectangle(5, rect.Height - 30, 160, 20));
                //    g1.DrawString(strTitle, font1, Brushes.Black, new PointF(25, rect.Height - 27));
                //}
                //else if ((bGPSGGATrack && !bBottomTrack && !bGPSTrack) || (bGPSTrack && !bBottomTrack && !bGPSGGATrack) || (bBottomTrack && !bGPSTrack && !bGPSGGATrack))
                //{
                //    g1.DrawRectangle(Pens.Black, new Rectangle(5, rect.Height - 50, 160, 40));
                //    g1.DrawString(strTitle, font1, Brushes.Black, new PointF(25, rect.Height - 47));

                //    string strLine = Resources.Resource1.String130;
                //    Brush brush = Brushes.DarkBlue;
                //    if (bGPSTrack)
                //    {
                //        strLine = "GPS VTG";
                //        brush = Brushes.DarkRed;
                //    }
                //    else if (bGPSGGATrack)
                //    {
                //        strLine = "GPS GGA";
                //        brush = Brushes.Indigo;
                //    }
                //    g1.DrawLine(new Pen(brush, 2), new Point(15, rect.Height - 20), new Point(55, rect.Height - 20));
                //    g1.DrawString(strLine, font1, Brushes.Black, new PointF(60, rect.Height - 27));

                //}
                //else
                {
                    string strLine1 = "BT", strLine2 = "GPS";
                    Brush brush1 = Brushes.DarkBlue, brush2 = Brushes.DarkRed;
                    //if (!bGPSTrack)
                    //{
                    //    strLine2 = "GPS GGA";
                    //    brush2 = Brushes.Indigo;
                    //}
                    //else if (!bBottomTrack)
                    //{
                    //    strLine1 = "GPS GGA";
                    //    brush1 = Brushes.Indigo;
                    //}

                    g1.DrawRectangle(Pens.Black, new Rectangle(5, rect.Height - 70, 160, 60));
                    g1.DrawString(strTitle, font1, Brushes.Black, new PointF(25, rect.Height - 67));

                    g1.DrawLine(new Pen(brush1, 2), new Point(15, rect.Height - 40), new Point(55, rect.Height - 40));
                    g1.DrawString(strLine1, font1, Brushes.Black, new PointF(60, rect.Height - 47));

                    g1.DrawLine(new Pen(brush2, 2), new Point(15, rect.Height - 20), new Point(55, rect.Height - 20));
                    g1.DrawString(strLine2, font1, Brushes.Black, new PointF(60, rect.Height - 27));

                }

                #endregion

                using (Pen GreenPen = new Pen(Color.DarkGray, 1))
                {
                    using (Font font = new Font("Arial", 8))//画中心经纬度及距离标值所用字体
                    {
                        //鼠标在边缘或手动移动时，画面水平移动距离除以单位刻度的商,代表的是画面流过的线条个数
                        int nX = (int)(trackRef.DragLengthX) / (int)trackRef.CurrentDisplayUnit;
                        //鼠标在边缘或手动移动时，画面水平移动距离除以单位刻度的余数，代表的是尚未产生最新线条时已经流过的距离
                        int lX = (int)(trackRef.DragLengthX) % (int)trackRef.CurrentDisplayUnit;
                        //鼠标在边缘或手动移动时，画面竖直移动距离除以单位刻度的商
                        int nY = (int)(trackRef.DragLengthY) / (int)trackRef.CurrentDisplayUnit;
                        //鼠标在边缘或手动移动时，画面竖直移动距离除以单位刻度的余数
                        int lY = (int)(trackRef.DragLengthY) % (int)trackRef.CurrentDisplayUnit;

                        bool DrawOrignX = false;

                        //   int SideWNum = (int)(trackRef.MainGPSWidth / trackRef.CurrentDisplayUnit) / 2; 
                        for (int i = 0; i <= 16; i++)//画竖线
                        {
                            //由顶端到低端，依次向下画横线，如果检测到画线的位置已经超出了显示区下端，则结束本次画面绘制
                            if (lX + trackRef.CurrentDisplayUnit * i > trackRef.MainGPSWidth)
                            {
                                break;
                            }

                            //float NumX = 10 * (i - 8 - nX) * trackRef.MouseWheelScale;    // 改变比例尺大小 
                            float NumX = (i - 8 - nX) * trackRef.MouseWheelScale;    //改变比例尺大小
                            //if (Resource1.String237 == labelUnit.Text) 
                            //if (!bEnglish2Metric)
                            //{
                            //    NumX = (float)(projectUnit.MeterToFeet(NumX, 1));
                            //}

                            //float NumX = (10 * (i - 8 - nX) * MouseWheelScale) / scaleMultiple;  
                            string NumXStr = NumX.ToString("0.00");
                            SizeF NumXSize = g1.MeasureString(NumXStr, font);
                            float NumXLength = NumXSize.Width;//字符串长度（像素）
                            //标定横坐标
                            //if(i==9) 
                            {
                                g1.DrawString(NumXStr, font, Brushes.Gray,
                                    trackRef.CurrentDisplayLeft + lX + trackRef.CurrentDisplayUnit * i - NumXLength / 2,
                                    trackRef.CurrentDisplayTop);
                            }

                            if (i == nX + 8)  //纵轴线:nX==0,即第8根线；nX==1，即第9根线，... ...
                            {
                                //标出经度值 
                                DrawOrignX = true;
                                OrignX = trackRef.CurrentDisplayLeft + lX + trackRef.CurrentDisplayUnit * i;

                                g1.DrawString("S", font, Brushes.Blue,
                                    OrignX, trackRef.CurrentDisplayTop + trackRef.MainGPSHeight - 1.5f * font.Height);

                                //VerticalFont
                                GreenPen.DashStyle = DashStyle.Solid;  //实线
                            }
                            else
                            {
                                GreenPen.DashStyle = DashStyle.Dot;    //点线形式
                            }

                            float fX1, fY2;
                            fX1 = trackRef.CurrentDisplayLeft + lX + trackRef.CurrentDisplayUnit * i;
                            fY2 = trackRef.CurrentDisplayTop + trackRef.MainGPSHeight;
                            //画竖线
                            g1.DrawLine(GreenPen, fX1, trackRef.CurrentDisplayTop, fX1, fY2);
                        }


                        //绘图区中心线两侧所显示的的线条个数
                        int SideHNum = (int)(trackRef.MainGPSHeight / trackRef.CurrentDisplayUnit) / 2;
                        //使初始绘图区中心始终为纵横两条线的交点
                        int AllHNum = 2 * SideHNum + 1;
                        //上侧最后一条横线到绘图区顶端之间的距离
                        float SmallHeight = trackRef.MainGPSHeight / 2 - SideHNum * trackRef.CurrentDisplayUnit;

                        bool DrawOrignY = false;

                        //画横线，使初始绘图区中心始终为纵横两条线的交点
                        for (int j = 0; j <= AllHNum; j++)
                        {
                            //由顶端到低端，依次向下画横线，如果检测到画线的位置已经超出了显示区下端，则结束本次画面绘制
                            if (lY + SmallHeight + trackRef.CurrentDisplayUnit * j > trackRef.MainGPSHeight)
                            {
                                break;
                            }
                            //float NumY = 10 * (j - SideHNum - nY) * trackRef.MouseWheelScale;    // 改变比例尺大小
                            float NumY = (j - SideHNum - nY) * trackRef.MouseWheelScale;    // 改变比例尺大小
                            //float NumY = (10 * (j - SideHNum - nY) * MouseWheelScale) / scaleMultiple;  
                            //Modified , negate X asix mark. When Mr Qi start the program, he set the X for North pointing right, and Y for East pointing down
                            NumY *= -1;

                            string NumYStr = NumY.ToString("0.00");
                            SizeF NumYSize = g1.MeasureString(NumYStr, font);
                            float NumYHeight = NumYSize.Height;

                            g1.DrawString(NumYStr, font, Brushes.Gray,
                              trackRef.CurrentDisplayLeft,
                              trackRef.CurrentDisplayTop + lY + SmallHeight + trackRef.CurrentDisplayUnit * j);

                            if (j == nY + SideHNum) //横轴线
                            {
                                //画纬度值:
                                //SizeF size = g1.MeasureString(SailTrackYpzn.ToString() + 'N', font);
                                SizeF size = g1.MeasureString("E", font);
                                float length = size.Width;
                                DrawOrignY = true;
                                OrignY = trackRef.CurrentDisplayTop + lY + SmallHeight + trackRef.CurrentDisplayUnit * j;

                                g1.DrawString("E", font, Brushes.Blue,
                                    trackRef.CurrentDisplayLeft + trackRef.MainGPSWidth - length, OrignY);

                                GreenPen.DashStyle = DashStyle.Solid;  //实线
                            }
                            else
                            {
                                GreenPen.DashStyle = DashStyle.Dot;    //点线形式
                            }
                            float fY1, fX2;
                            fY1 = trackRef.CurrentDisplayTop + lY + SmallHeight + trackRef.CurrentDisplayUnit * j;
                            fX2 = trackRef.CurrentDisplayLeft + trackRef.MainGPSWidth;
                            //画横线
                            g1.DrawLine(GreenPen, trackRef.CurrentDisplayLeft, fY1, fX2, fY1);
                        }
                        if (DrawOrignX && DrawOrignY)
                        {
                            g1.FillEllipse(Brushes.GreenYellow, OrignX - 3f, OrignY - 3f, 6f, 6f);
                        }

                        //由于每次都是从（CurrentDisplayTop+SmallHeight）处开始往下绘制画横线，
                        //故要补充绘制（CurrentDisplayTop+SmallHeight）以上的部分：
                        if (lY + SmallHeight >= trackRef.CurrentDisplayUnit)
                        {
                            //准备画的线是标定为0的那根线向上数第(SideHNum + (nY+1))根
                            float NumY = (0 - SideHNum - nY - 1) * trackRef.MouseWheelScale;

                            string NumYStr = NumY.ToString("0.00");
                            SizeF NumYSize = g1.MeasureString(NumYStr, font);
                            float NumYHeight = NumYSize.Height;
                            //标定纵坐标
                            g1.DrawString(NumYStr, font, Brushes.LightGray,
                                trackRef.CurrentDisplayLeft,
                                trackRef.CurrentDisplayTop + lY + SmallHeight - trackRef.CurrentDisplayUnit - NumYHeight / 2);
                            //画横线
                            float fY1, fX2;
                            fY1 = trackRef.CurrentDisplayTop + lY + SmallHeight - trackRef.CurrentDisplayUnit;
                            fX2 = trackRef.CurrentDisplayLeft + trackRef.MainGPSWidth;
                            g1.DrawLine(GreenPen, trackRef.CurrentDisplayLeft, fY1, fX2, fY1);
                        }
                    }
                    GreenPen.Dispose();
                }

                float scaleFactor = trackRef.MouseWheelScale / trackRef.scaleMultiple;
                Matrix matrix = new Matrix();//定义一个做几何变换的对象

                matrix.Translate(trackRef.MainGPSWidth / 2 + trackRef.DragLengthX, trackRef.DragLengthY + trackRef.MainGPSHeight / 2);//缩放中心设为绘图区中心
                matrix.Scale(trackRef.scaleMultiple / trackRef.MouseWheelScale, trackRef.scaleMultiple / trackRef.MouseWheelScale);

                g1.Transform = matrix;

                PointF PtStart = new PointF(0, 0);
                PointF tempP = new PointF(0, 0);
                PointF VP = new PointF(0, 0);
                Point saveLastPoint = new Point(0, 0);

                int GPSdisplayStartPoint;
                GPSdisplayStartPoint = 0;
                //PtStart = (Point)UTMpointTrackSave[GPSdisplayStartPoint];

                OnDrawTrackBT_GPS(g1, FirstPingSecond, BoatV, 1, fVelocity, GPSdisplayStartPoint, BinDataEnsembleNum, PtStart, ref tempP, VP, trackRef); //  底跟踪航迹            
                try
                {
                    OnDrawTrackBT_GPS(g1, FirstPingSecond, BoatV_GPS, 2, fVelocity_GPS, GPSdisplayStartPoint, BinDataEnsembleNum, PtStart, ref tempP, VP, trackRef);  // GPS航迹
                }
                catch
                { }
                //else if (bGPSGGATrack)
                //    OnDrawTrackBT_GPS(g1, FirstPingSecond, BoatV_GGA, 3, fVelocity_GGA, GPSdisplayStartPoint, BinDataEnsembleNum, PtStart, ref tempP, VP, trackRef);  // GPS航迹

                AutoSizeGPS(tempP, ref trackRef, rect.Height, rect.Width);
            }
            catch (Exception ex)
            { }

        }

        /// <summary>
        /// 绘制底跟踪和GPS的航迹
        /// </summary>
        /// <param name="iFlag">频率</param>
        /// <param name="g1"></param>
        /// <param name="EnsemblesInfoToStore"></param>
        /// <param name="iBT">底跟踪或GPS，当iBT=1，为底跟踪</param>
        /// <param name="iStartEnsembleID">起始样本号</param>
        /// <param name="iTotalEnsembles">总样本数</param>
        /// <param name="PtStart">起始点</param>
        /// <param name="tempP"></param>
        /// <param name="VP">流速矢量</param>
        ///<param name="trackRef">缩放参数</param>
        private void OnDrawTrackBT_GPS(Graphics g1, List<float> FirstPingSecond, List<Velocity> BoatV, int iBT, List<Velocity> fVelocity, int iStartEnsembleID, int iTotalEnsembles, PointF PtStart, ref PointF tempP, PointF VP, TrackPanelRef trackRef) //将GPS和底跟踪绘制在一起
        {
            try
            {
                PointF[] TempPnts = new PointF[iTotalEnsembles + 1];

                int PrevGoodEnsembleNoOffset = 0;  //  底跟踪前一个有效单元的偏移                     
                bool GetFirstGoodEnsemble = true;  //  采集到第一个有效单元
                float AccEast = 0; //  底跟踪东向累积量
                float AccNorth = 0; // 底跟踪北向累积量
                int PrevGoodEnsemblePos = 0; // 前一个底跟踪有效单元位置
                float fLastSecond = 0;    // 上一个有效的Ensemble时间

                PointF tempPnt = TransToMapPoint(tempP, trackRef); ;
                int icounts = 0;
                Point UTMpoint = new Point(0, 0); // 将UTMpoint设为局部变量

                Pen VelocityPen, TrackPen;
                if (1 == iBT)
                {
                    VelocityPen = new Pen(Brushes.Blue, (int)(1.5 * trackRef.MouseWheelScale));
                    TrackPen = new Pen(Brushes.DarkBlue, 4 * trackRef.MouseWheelScale); // 将航迹图中的航迹改为红色
                }
                else if (2 == iBT)
                {
                    VelocityPen = new Pen(Brushes.Brown, (int)(1.5 * trackRef.MouseWheelScale));
                    TrackPen = new Pen(Brushes.DarkRed, 4 * trackRef.MouseWheelScale);
                }
                else
                {
                    VelocityPen = new Pen(Brushes.Purple, (int)(1.5 * trackRef.MouseWheelScale));
                    TrackPen = new Pen(Brushes.Indigo, 4 * trackRef.MouseWheelScale);
                }

                for (int i = iStartEnsembleID; i < iTotalEnsembles; i++)  // 确保subsystem的总数一致
                {
                    if (i == 0)
                    {
                        UTMpoint.X = 0;  //
                        UTMpoint.Y = 0;

                        float fBoatVx = 0, fBoatVy = 0; //LPJ 2019-7-31

                        if (BoatV.Count > i)
                        {
                            fBoatVx = BoatV[i].VX; //LPJ 2019-7-31
                            fBoatVy = BoatV[i].VY;
                        }

                        if (Math.Abs(fBoatVx) > 20 || Math.Abs(fBoatVy) > 20)
                        {
                            //FirstGoodEnsembleNoOffset++; //起始数据组不是有效底跟踪数据
                            GetFirstGoodEnsemble = false;
                        }
                        else  // 改正导航航迹计算
                        {
                            GetFirstGoodEnsemble = true;
                            fLastSecond = (float)FirstPingSecond[0];
                            PrevGoodEnsemblePos = 0;
                        }

                        TempPnts[icounts++] = TransToMapPoint(PtStart, trackRef);
                    }
                    else
                    {
                        float fBoatVx = 0, fBoatVy = 0;  //LPJ 2019-7-31
                        float fBoatVx_Prev = 0, fBoatVy_Prev = 0; //LPJ 2019-7-31

                        if (BoatV.Count > i) //LPJ 2019-7-31
                        {
                            fBoatVx = BoatV[i].VX; //LPJ 2019-7-31
                            fBoatVy = BoatV[i].VY;
                        }

                        if (BoatV.Count > PrevGoodEnsemblePos) //LPJ 2019-7-31
                        {
                            fBoatVx_Prev = BoatV[PrevGoodEnsemblePos].VX;
                            fBoatVy_Prev = BoatV[PrevGoodEnsemblePos].VY;
                        }

                        try
                        {
                            if (Math.Abs(fBoatVx) < 20 && Math.Abs(fBoatVy) < 20)
                            {
                                if (GetFirstGoodEnsemble)
                                {
                                    float LEast = (1.0f) * 0.5f * (float)(fBoatVx_Prev + fBoatVx) * ((float)FirstPingSecond[i] - fLastSecond);
                                    float LNorth = (1.0f) * 0.5f * (float)(fBoatVy_Prev + fBoatVy) * ((float)FirstPingSecond[i] - fLastSecond);

                                    AccEast = AccEast + LEast;
                                    AccNorth = AccNorth + LNorth;
                                    // 支持自动缩放
                                    float fTransAccEast = AccEast * trackRef.CurrentDisplayUnit;
                                    float fTransAccNorth = AccNorth * trackRef.CurrentDisplayUnit;

                                    UTMpoint.X = (int)fTransAccEast;
                                    UTMpoint.Y = (int)fTransAccNorth;

                                    PrevGoodEnsembleNoOffset = 0;
                                    PrevGoodEnsemblePos = i;
                                    fLastSecond = (float)FirstPingSecond[i];

                                }
                                else
                                {
                                    GetFirstGoodEnsemble = true;
                                    PrevGoodEnsemblePos = i;
                                    fLastSecond = (float)FirstPingSecond[i];
                                }
                            }
                            else
                            {
                                PrevGoodEnsembleNoOffset++;
                            }
                        }
                        catch
                        {
                            //PrevGoodEnsembleNoOffset++;
                        }

                    }
                    tempP.X = UTMpoint.X;
                    tempP.Y = UTMpoint.Y;
                    tempP.Y *= -1;

                    float AVX = 0, AVY = 0; //LPJ 2019-7-31

                    if (fVelocity.Count > i) //LPJ 2019-7-31
                    {
                        AVX = fVelocity[i].VX;
                        AVY = fVelocity[i].VY;
                    }
                    VP.Y = tempP.Y - AVY * trackRef.AverageScale * trackRef.MouseWheelScale / 2;
                    VP.X = tempP.X + AVX * trackRef.AverageScale * trackRef.MouseWheelScale / 2;

                    tempPnt = TransToMapPoint(tempP, trackRef);
                    PointF VPnt = TransToMapPoint(VP, trackRef);

                    //流速矢量线 
                    g1.DrawLine(VelocityPen, tempPnt, VPnt);

                    try
                    {
                        TempPnts[icounts++] = tempPnt;
                    }
                    catch
                    {
                    }
                    PtStart = tempP; // SailTrackTansToMapPoint((PointF)(bc.SailTrackBoatPosition));
                }

                g1.DrawLines(TrackPen, TempPnts);

                #region 画辅助线
                /*
                if (2 == iBT) //画辅助线
                {
                    Pen penLine = new Pen(Color.Gold, 2 * trackRef.MouseWheelScale);
                    float[] dashValues = { 15, 5 };
                    penLine.DashPattern = dashValues;

                    PointF pntEnd = TempPnts[TempPnts.Count() - 1];
                    PointF pntStart = TempPnts[0];

                    float datX = pntEnd.X - pntStart.X;
                    float datY = pntEnd.Y - pntStart.Y;

                    double linelen = Math.Sqrt(Math.Pow(datX, 2) + Math.Pow(datY, 2));
                    double lineangle = Math.Atan2(datY, datX);

                    float x_end = (float)(linelen * Math.Cos(lineangle + 2.0f / 180 * Math.PI));
                    float y_end = (float)(linelen * Math.Sin(lineangle + 2.0f / 180 * Math.PI));

                    float x_end2 = (float)(linelen * Math.Cos(lineangle - 2.0f / 180 * Math.PI));
                    float y_end2 = (float)(linelen * Math.Sin(lineangle - 2.0f / 180 * Math.PI));

                    //g1.DrawLine(penLine, pntStart, pntEnd);
                    g1.DrawLine(penLine, pntStart, new PointF(x_end, y_end));
                    g1.DrawLine(penLine, pntStart, new PointF(x_end2, y_end2));
                }
                 * */
                #endregion

                Pen n = new Pen(Color.Blue, trackRef.MouseWheelScale);
                Rectangle rg = new Rectangle((int)(tempPnt.X - 4 * trackRef.MouseWheelScale), (int)(tempPnt.Y - 4 * trackRef.MouseWheelScale), (int)(8 * trackRef.MouseWheelScale), (int)(8 * trackRef.MouseWheelScale));
                g1.DrawEllipse(n, rg);
                n.Dispose();
            }
            catch
            {
            }
        }

        private void AutoSizeGPS(PointF pnt, ref TrackPanelRef trackRef, int height, int width) // 航迹自动缩放
        {
            Point p = new Point();

            p.X = (int)(pnt.X / trackRef.MouseWheelScale + ((float)trackRef.MainGPSWidth / 2 + trackRef.DragLengthX));
            p.Y = (int)(pnt.Y / trackRef.MouseWheelScale + ((float)trackRef.MainGPSHeight / 2 + trackRef.DragLengthY));

            Rectangle r = new Rectangle(5, 5, width - 10, height - 10);
            while (!r.Contains(p))
            {
                p.X = (int)(pnt.X / trackRef.MouseWheelScale + (float)trackRef.MainGPSWidth / 2);
                p.Y = (int)(pnt.Y / trackRef.MouseWheelScale + (float)trackRef.MainGPSHeight / 2);

                if (trackRef.MouseWheelScale < 5)
                {
                    trackRef.MouseWheelScale++;
                }
                else
                {
                    trackRef.MouseWheelScale += 2;
                }
            }
        }

        private PointF TransToMapPoint(PointF pt, TrackPanelRef trackRef)
        {
            PointF RealStartPointToStore = new PointF();
            RealStartPointToStore.X = pt.X;
            RealStartPointToStore.Y = pt.Y;
            return (RealStartPointToStore);
        }

        //Point_XY originBL = new Point_XY();
        //private Point ComputeXYposition(float FloatLatitude, float FloatLongitude, int totalNum)
        //{
        //    //将GPS坐标格式转化为度----start
        //    int iLat, iLong;
        //    iLat = (int)FloatLatitude / 100;
        //    iLong = (int)FloatLongitude / 100;
        //    double testLat = iLat + (FloatLatitude / 100.0 - iLat) / 60.0 * 100.0;
        //    double testLong = iLong + (FloatLongitude / 100.0 - iLong) / 60.0 * 100.0;
        //    double L0 = (double)iLong;
        //    //LPJ LPJ 2012-8-30 将GPS坐标格式转化为度----end

        //    BL_2_xy tempBL = new BL_2_xy();
        //    Point_XY currentBL = tempBL.UTM_BL2xy(6378137.000, 298.257223563, testLat, testLong, L0); //UTM 投影  WGS-84坐标  B=30.5° L=120.5°L0=120°

        //    Point UTMpoint = new Point(0, 0);

        //    if (totalNum < 10)
        //    {
        //        originBL = currentBL;

        //        UTMpoint.X = 0;
        //        UTMpoint.Y = 0;
        //    }
        //    else
        //    {
        //        UTMpoint.X = (int)(currentBL.X - originBL.X);
        //        UTMpoint.Y = (int)(currentBL.Y - originBL.Y);
        //    }
        //    return UTMpoint;
        //}

    }

    //定义一个结构体用于存储航迹轨迹绘制的各个参数
    public class TrackPanelRef
    {
        public float MaxDisplayHeight;
        public float MaxDisplayWidth;
        public float CurrentDisplayUnit;
        public float CurrentDisplayTop;
        public float CurrentDisplayLeft;
        public float scaleMultiple;
        public float PreviousMultiple;//上一次刷新后客户区所处于的倍数
        public float DragLengthX;
        public float DragLengthY;
        public float MouseWheelScale;//滚轮缩小率的初始值 
        public PointF DragEndPoint;
        public Rectangle DisplayRec;
        public float MainGPSWidth;
        public float MainGPSHeight;
        public int AverageScale;

        //public ArrayList GGATracksave;
        //public ArrayList AverageVXTracksave;
        //public ArrayList AverageVYTracksave;
        //public ArrayList LatitudeTrackSave;
        //public ArrayList LongitudeTrackSave;
        //public ArrayList UTMpointTrackSave;
    }

    public struct Velocity
    {
        public float VX;
        public float VY;
        public float VZ;
    }
}
