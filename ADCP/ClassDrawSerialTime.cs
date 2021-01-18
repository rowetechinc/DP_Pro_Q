using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ADCP
{
    class ClassDrawSerialTime
    {
        public void OnDrawBoatSpeed(Graphics g, Rectangle rectangle, List<double> dData, string strUnit, double dScreen, Color color,float fDistance,string strDis)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X, rectangle.Y+2, rectangle.Width-15, rectangle.Height - rectangle.Height / 6);

            //Pen pline = new Pen(Color.Black, 1);
            //g.DrawRectangle(pline, 55 + rect.X, 5 + rect.Y, rect.Width - 60, rect.Height - 20);

            //LPJ 2013-5-25 设置参数控制最大值和最小值
            double min, max, unit;
            min = 0;
            max = 0;

            if (dData.Count > 0)
            {
                if (dData[0] < dScreen)
                {
                    min = dData[0];
                    max = dData[0];
                }

                for (int i = 0; i < dData.Count; i++)
                {
                    if (dData[i] < dScreen)
                    {
                        if (min > dData[i])
                            min = dData[i];
                        if (max < dData[i])
                            max = dData[i];
                    }
                }

            }
            if (max == min)
            {
                max++;
                min--;
            }
            unit = (max - min) / 5.0;

            //绘制横线
            Pen pgray = new Pen(Color.LightGray, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 0; i < 6; i++)
            {
               // Point pnt = new Point(12 + rect.X, (int)((i + 1) / 6.0f * rect.Height - 18) + rect.Y);
                Point pnt = new Point(17 + rect.X, (int)((rect.Height - 25.0f) * i / 5.0f - 5 + rect.Y));
                
                g.DrawLine(pgray, 55 + rect.X
                    , (rect.Height - 25.0f) * i / 5.0f + 5 + rect.Y
                    , rect.Width - 5 + rect.X
                    , (rect.Height - 25.0f) * i / 5.0f + 5 + rect.Y);

                g.DrawString((min + unit * (5 - i)).ToString("0.00"), font, Brushes.DarkBlue, pnt); //LPJ 2013-5-25 从最小值到最大值范围内
            }

            //LPJ 2013-9-23 绘制纵线 
            for (int i = 0; i < 6; i++)
            {
                if(i<5)
                g.DrawLine(pgray, 55 + rect.X + (rect.Width - 60) * i / 5, rect.Height - 15 + rect.Y, 55 + rect.X + (rect.Width - 60) * i / 5, 5 + rect.Y);

                int length = ((int)(fDistance / 5 * i)).ToString().Length;
                Point pnt = new Point(50 + rect.X + (rect.Width - 60) * i / 5 - length * 5, rect.Height - 15 + rect.Y);
                g.DrawString((fDistance / 5 * i).ToString("0.0"), font, Brushes.DarkBlue, pnt); 
            }

            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, 55 + rect.X, 3 + rect.Y, rect.Width - 60, rect.Height - 21);

            //横纵坐标说明
            Font font1 = new System.Drawing.Font("Arial Narrow", 11); //LPJ 2013-6-13

            //Point pnt1 = new Point(0 + rect.X, rect.Height / 3 - strUnit.Length * 2 + rect.Y); //LPJ 2013-6-13
            Point pnt1 = new Point(0 + rect.X, rect.Height / 2 - (int)(g.MeasureString(strUnit, font1).Width) / 2 + rect.Y);
            //g.DrawString(strUnit, font1, Brushes.DarkBlue, pnt1); //LPJ 2013-6-13
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString(strUnit, font1, Brushes.DarkBlue, pnt1, sf);
            g.DrawString(Resource1.String272 + strDis, font1, Brushes.DarkBlue, new Point(rect.Width / 2 - 10 + rect.X, rect.Y + rect.Height - 10));

            //折线
            Pen pblue = new Pen(color, 1);
            float startY = 0.0f, lastY = 0.0f;
            try
            {
                for (int i = 0; i < dData.Count; i++)
                {
                    double yy = 0;
                    try //LPJ 2013-8-16
                    {
                        yy = dData[i];
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    if (i == 0)
                    {
                        lastY = (float)dData[0];
                    }

                    if (yy < dScreen)
                    {
                        startY = (float)yy;
                        float xscale = (float)(rect.Width - 60) / (dData.Count);
                        int yscale = rect.Height - 25;
                        g.DrawLine(pblue, xscale * i + 55 + rect.X
                            , ((float)max - lastY) / (float)(max - min) * yscale + 5 + rect.Y
                            , xscale * (i + 1) + 55 + rect.X
                            , ((float)max - startY) / (float)(max - min) * yscale + 5 + rect.Y);

                        lastY = startY;
                    }
                    else //LPJ 2013-6-7
                    {
                        startY = lastY;
                        float xscale = (float)(rect.Width - 60) / (dData.Count);
                        int yscale = rect.Height - 25;
                        g.DrawLine(pblue, xscale * i + 55 + rect.X, ((float)max - lastY) / (float)(max - min) * yscale + 5 + rect.Y, xscale * (i + 1) + 55 + rect.X, ((float)max - startY) / (float)(max - min) * yscale + 5 + rect.Y);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void OnDrawWaterDirect(Graphics g, Rectangle rectangle, List<double> dData, Color color, string strUnit, float fDistance, string strDis)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);

            Rectangle rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width-15, rectangle.Height - rectangle.Height / 6);

            //Pen pline = new Pen(Color.Black, 1);
            //g.DrawRectangle(pline, 55+rect.X, 5+rect.Y, rect.Width - 60, rect.Height - 20);

            //绘制格网
            Pen pgray = new Pen(Color.LightGray, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体
            for (int i = 0; i < 5; i++)
            {
                //Point pnt = new Point(12 + rect.X, (int)((i + 1) / 5.0f * rect.Height - 18) + rect.Y);
                Point pnt = new Point(27 + rect.X, (int)((rect.Height - 25.0f) * i / 4.0f - 5 + rect.Y));
               
                g.DrawLine(pgray, 55 + rect.X
                    , (rect.Height - 25.0f) * i / 4.0f + 5 + rect.Y
                    , rect.Width - 5 + rect.X
                    , (rect.Height - 25.0f) * i / 4.0f + 5 + rect.Y);
               
                g.DrawString((360 - i * 90).ToString(""), font, Brushes.DarkBlue, pnt);
            }

            //LPJ 2013-9-23 绘制纵线 
            for (int i = 0; i < 6; i++)
            {
                if (i < 5)
                g.DrawLine(pgray, 55 + rect.X + (rect.Width - 60) * i / 5, rect.Height - 15 + rect.Y, 55 + rect.X + (rect.Width - 60) * i / 5, 5 + rect.Y);

                int length = ((int)(fDistance / 5 * i)).ToString().Length;
                Point pnt = new Point(50 + rect.X + (rect.Width - 60) * i / 5 - length * 5, rect.Height - 15 + rect.Y);
                g.DrawString((fDistance / 5 * i).ToString("0.0"), font, Brushes.DarkBlue, pnt);
            }

            Font font1 = new System.Drawing.Font("Arial Narrow", 11); //LPJ 2013-6-13
            Point pnt1 = new Point(0 + rect.X, rect.Height / 2 - (int)(g.MeasureString(strUnit, font1)).Width / 2 + rect.Y); //LPJ 2013-6-13
            //g.DrawString(Resource1.String216, font1, Brushes.DarkBlue, pnt1); //LPJ 2013-6-13
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString(strUnit, font1, Brushes.DarkBlue, pnt1, sf);
            g.DrawString(Resource1.String272 + strDis, font1, Brushes.DarkBlue, new Point(rect.Width / 2 - 10 + rect.X, rect.Y + rect.Height - 10));

            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, 55 + rect.X, 3 + rect.Y, rect.Width - 60, rect.Height - 21);

            Pen pblue = new Pen(color, 1);
            float startY = 0.0f, lastY = 0.0f;

            try
            {
                for (int i = 0; i < dData.Count; i++)
                {
                    startY = (float)(dData[i]);

                    float xscale = (float)(rect.Width - 60) / (dData.Count );
                    //float yscale = (rect.Height - 32) / 360.0f;
                    float yscale = (rect.Height - 25) / 360.0f;
                    try
                    {
                        g.DrawLine(pblue, xscale * i + 55 + rect.X
                            , (360 - lastY) * yscale + 5 + rect.Y
                            , xscale * (i + 1) + 55 + rect.X
                            , (360 - startY) * yscale + 5 + rect.Y);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    lastY = startY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OnDrawAttitude(Graphics g, Rectangle rectangle, List<double> dData, Color color, string strUnit, float fDistance, string strDis)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width-15, rectangle.Height - rectangle.Height / 6);

            //Pen pline = new Pen(Color.Black, 1);
            //g.DrawRectangle(pline, 55 + rect.X, 5 + rect.Y, rect.Width - 60, rect.Height - 20);

            //LPJ 2013-5-25 设置参数控制最大值和最小值
            double min, max, unit;
            min = 0;
            max = 0;
            if (dData.Count > 0)
            {
                min = dData[0];
                max = dData[0];
                for (int i = 0; i < dData.Count; i++)
                {
                    if (min > dData[i])
                        min = dData[i];
                    if (max < dData[i])
                        max = dData[i];
                }
            }
            if(max == min)
            {
                max++;
                min--;
            }
            unit = (max - min) / 5.0f;

            //绘制格网
            //Pen pgray = new Pen(Color.WhiteSmoke, 0.1f);
            Pen pgray = new Pen(Color.LightGray, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 0; i < 6; i++)
            {
                //Point pnt = new Point(12 + rect.X, (int)((i + 1) / 6.0f * rect.Height - 18) + rect.Y);
                Point pnt = new Point(17 + rect.X, (int)((rect.Height - 25.0f) * i / 5.0f - 5 + rect.Y));
               
                g.DrawLine(pgray, 55 + rect.X
                    , (rect.Height - 25.0f) * i / 5.0f + 5 + rect.Y
                    , rect.Width - 5 + rect.X
                    , (rect.Height - 25.0f) * i / 5.0f + 5 + rect.Y);
               
                g.DrawString((min + unit * (5 - i)).ToString("0.00"), font, Brushes.DarkBlue, pnt); //LPJ 2013-5-25 从最小值到最大值范围内              
            }

            //LPJ 2013-9-23 绘制纵线 
            for (int i = 0; i < 6; i++)
            {
                if (i < 5)
                g.DrawLine(pgray, 55 + rect.X + (rect.Width - 60) * i / 5, rect.Height - 15 + rect.Y, 55 + rect.X + (rect.Width - 60) * i / 5, 5 + rect.Y);

                int length = ((int)(fDistance / 5 * i)).ToString().Length;
                Point pnt = new Point(50 + rect.X + (rect.Width - 60) * i / 5 - length * 5, rect.Height - 15 + rect.Y);
                g.DrawString((fDistance / 5 * i).ToString("0.0"), font, Brushes.DarkBlue, pnt);
            }

            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, 55 + rect.X, 3 + rect.Y, rect.Width - 60, rect.Height - 21);

            Font font1 = new System.Drawing.Font("Arial Narrow", 11); //LPJ 2013-6-13
            //Point pnt1 = new Point(0 + rect.X, rect.Height / 3 - strUnit.Length * 2 + rect.Y); //LPJ 2013-6-13
            Point pnt1 = new Point(0 + rect.X, rect.Height / 2 - (int)(g.MeasureString(strUnit, font1)).Width / 2 + rect.Y);
            //g.DrawString(Resource1.String216, font1, Brushes.DarkBlue, pnt1); //LPJ 2013-6-13
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString(strUnit, font1, Brushes.DarkBlue, pnt1, sf);
            g.DrawString(Resource1.String272 + strDis, font1, Brushes.DarkBlue, new Point(rect.Width / 2 - 10 + rect.X, rect.Y + rect.Height - 10));

            Pen pblue = new Pen(color, 1);
            double startY = 0.0f, lastY = 0.0f;

            if (dData.Count > 0)
            {
                startY = dData[0];
                lastY = dData[0];
            }

            try
            {
                for (int i = 0; i < dData.Count; i++)
                {
                    startY = dData[i];

                    float xscale = (float)(rect.Width - 60) / (dData.Count );
                    float yscale = rect.Height - 25;

                    /*g.DrawLine(pblue, xscale * i + 55 + rect.X
                        , (float)((max - lastY) / (max - min) * yscale) + 5 + rect.Y
                        , xscale * (i + 1) + 55 + rect.X
                        , (float)((max - startY) / (max - min) * yscale) + 5 + rect.Y);*/
                    g.DrawLine(pblue, xscale * i + 55 + rect.X
                        , (float)((max - lastY) / (max - min) * yscale) + 5 + rect.Y
                        , xscale * (i + 1) + 55 + rect.X
                        , (float)((max - startY) / (max - min) * yscale) + 5 + rect.Y);
                    lastY = startY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OnDrawBoatWater(Graphics g, Rectangle rectangle, List<double> dData, Color color, string strUnit, float fDistance, string strDis)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width-15, rectangle.Height - rectangle.Height / 6);

            //Pen pline = new Pen(Color.Black, 1);
            //g.DrawRectangle(pline, 55 + rect.X, 5 + rect.Y, rect.Width - 60, rect.Height - 20);

            //LPJ 2013-5-25 设置参数控制最大值和最小值
            double min, max, unit;
            min = 0;
            max = 0;
            if (dData.Count > 0)
            {
                min = dData[0];
                max = dData[0];
                for (int i = 0; i < dData.Count; i++)
                {
                    //if (dData[i] < 20)
                    {
                        if (min > dData[i])
                            min = dData[i];
                        if (max < dData[i])
                            max = dData[i];
                    }
                }
            }
            if (max == min)
            {
                max++;
                min--;
            }
            unit = (max - min) / 5.0;

            //绘制格网
            Pen pgray = new Pen(Color.LightGray, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体
            for (int i = 0; i < 6; i++)
            {
                //Point pnt = new Point(12 + rect.X, (int)(i / 5.0f * rect.Height - 10) + rect.Y);
                Point pnt = new Point(17 + rect.X, (int)((rect.Height - 25.0f) * i / 5.0f - 5 + rect.Y));
                
                g.DrawLine(pgray, 55 + rect.X
                    , (rect.Height - 25.0f) * i / 5.0f +5+ rect.Y
                    , rect.Width - 5 + rect.X
                    , (rect.Height - 25.0f) * i / 5.0f +5+ rect.Y);
                //g.DrawString((10-i*2).ToString("0.0"), font, Brushes.DarkBlue, pnt);
                g.DrawString((min + unit * (5 - i)).ToString("0.0"), font, Brushes.DarkBlue, pnt); //LPJ 2013-5-25 从最小值到最大值范围内
            }

            //LPJ 2013-9-23 绘制纵线 
            for (int i = 0; i < 6; i++)
            {
                if (i < 5)
                g.DrawLine(pgray, 55 + rect.X + (rect.Width - 60) * i / 5, rect.Height - 15 + rect.Y, 55 + rect.X + (rect.Width - 60) * i / 5, 5 + rect.Y);

                int length = ((int)(fDistance / 5 * i)).ToString().Length;
                Point pnt = new Point(50 + rect.X + (rect.Width - 60) * i / 5 - length * 5, rect.Height - 15 + rect.Y);
                g.DrawString((fDistance / 5 * i).ToString("0.0"), font, Brushes.DarkBlue, pnt);
            }

            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, 55 + rect.X, 3 + rect.Y, rect.Width - 60, rect.Height - 21);

            Font font1 = new System.Drawing.Font("Arial Narrow", 11); //LPJ 2013-6-13
            //Point pnt1 = new Point(0 + rect.X, rect.Height / 3 - strUnit.Length * 2 + rect.Y); //LPJ 2013-6-13
            Point pnt1 = new Point(0 + rect.X, rect.Height / 2 - (int)(g.MeasureString(strUnit, font1)).Width / 2 + rect.Y);
            //g.DrawString(Resource1.String216, font1, Brushes.DarkBlue, pnt1); //LPJ 2013-6-13
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            g.DrawString(strUnit, font1, Brushes.DarkBlue, pnt1, sf);
            g.DrawString(Resource1.String272 + strDis, font1, Brushes.DarkBlue, new Point(rect.Width / 2 - 10 + rect.X, rect.Y + rect.Height - 10));

            Pen pblue = new Pen(color, 1);
            float startY = 0.0f, lastY = 0.0f;
            try
            {
                for (int i = 0; i < dData.Count; i++)
                {
                    startY = (float)(dData[i]);
                    if (i == 0)
                        lastY = startY;
                    float xscale = (float)(rect.Width - 60) / (dData.Count );
                    float yscale = rect.Height - 25;
                    try
                    {
                        g.DrawLine(pblue, xscale * i + 55 + rect.X
                            , ((float)max - lastY) / (float)(max - min) * yscale + 5 + rect.Y
                            , xscale * (i + 1) + 55 + rect.X
                            , ((float)max - startY) / (float)(max - min) * yscale + 5 + rect.Y);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    lastY = startY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
