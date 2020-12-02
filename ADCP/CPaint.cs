using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ADCP
{
    class CPaint
    {
        #region private parameter 
        #endregion

        #region public parameter
        /// <summary>
        /// 将数值转换为色度图
        /// </summary>
        /// <param name="Num">输入的数值</param>
        /// <param name="type">颜色类型，1为灰度图，2为256阶，3为</param>
        /// <param name="DirectionFlag">方向，一般设置为true</param>
        /// <param name="LimiteVelocity">最大值</param>
        /// <returns>颜色</returns>
        public Color NumToColor(float Num, int type, bool DirectionFlag, float LimiteVelocity)
        {
            int c;
            Color cl = new Color();
            switch (type)
            {
                case 1:
                    {
                        if (Num <= LimiteVelocity)//灰度
                        {
                            c = (int)(255 * (Num / LimiteVelocity));
                        }
                        else
                        {
                            c = 255;
                        }
                        cl = Color.FromArgb(c, c, c);
                        break;
                    }
                case 2:
                    {
                        if (Num <= LimiteVelocity)
                        {
                            c = (int)(1275 * (Num / LimiteVelocity));
                        }
                        else
                        {
                            c = 1275;
                        }
                        if (c <= 255)
                            cl = Color.FromArgb(0, c, 255);//(0,0,255)-(0,255,255)
                        else if (c > 255 && c <= 510)
                            cl = Color.FromArgb(0, 255, 255 - (c - 255));//(0,255,254)-(0,255,0)
                        else if (c > 510 && c <= 765)
                            cl = Color.FromArgb(c - 510, 255, 0);//(1,255,0)-(255,255,0)
                        else if (c > 765 && c <= 1020)
                            cl = Color.FromArgb(255, 255 - (c - 765), 0);//(255,254,0)-(255,0,0)
                        else //if (c > 1020)
                            cl = Color.FromArgb(255, 0, c - 1020);//(255,0,1)-(255,0,255)
                        break;
                    }
                case 3:
                    {
                        if (!DirectionFlag)//向外：蓝->绿/黑-蓝-青（511阶颜色）    VX<0
                        {
                            if (Num <= LimiteVelocity)
                            {
                                c = (int)(510 * (Num / LimiteVelocity));
                            }
                            else
                            {
                                c = 510;
                            }
                            if (c <= 255)
                            {
                                cl = Color.FromArgb(0, c, 255);//(0,0,255)-(0,255,255)
                            }
                            else //if ((c > 255) && (c <= 255 * 2))
                            {
                                cl = Color.FromArgb(0, 255, 255 - (c - 255));//(0,255,254)-(0,255,0)
                            }
                        }
                        else //向内：蓝->红/黑-红-黄（510阶颜色）
                        {
                            if (Num <= LimiteVelocity)
                            {
                                c = (int)(510 * (Num / LimiteVelocity));
                            }
                            else
                            {
                                c = 510;
                            }
                            if (c <= 255)
                            {
                                cl = Color.FromArgb(c, 0, 255);//(1,0,255)-(255,0,255)
                            }
                            else //if ((c > 255) && (c <= 255 * 2))
                            {
                                cl = Color.FromArgb(255, 0, 510 - c);//(255,0,254)-(255,0,0) 
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        if (!DirectionFlag)//向外：蓝->绿/黑-蓝-青（511阶颜色）    VX<0
                        {
                            if (Num <= LimiteVelocity)
                            {
                                c = (int)(510 * (Num / LimiteVelocity));
                            }
                            else
                            {
                                c = 510;
                            }
                            if (c <= 255)
                            {
                                cl = Color.FromArgb(0, 0, c);//(0,0,0)-(0,0,255)
                            }
                            else //if ((c > 255) && (c <= 255 * 2))
                            {
                                cl = Color.FromArgb(0, c - 255, 255);//(0,1,255)-(0,255,255)
                            }
                        }
                        else //向内：蓝->红/黑-红-黄（510阶颜色）
                        {
                            if (Num <= LimiteVelocity)
                            {
                                c = (int)(510 * (Num / LimiteVelocity));
                            }
                            else
                            {
                                c = 510;
                            }
                            if (c <= 255)
                            {
                                cl = Color.FromArgb(c, 0, 0);//(0,0,0)-(255,0,0)
                            }
                            else //if ((c > 255) && (c <= 255 * 2))
                            {
                                cl = Color.FromArgb(255, c - 255, 0);//(255,1,0)-(255,255,0)}
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            return cl;
        }

        public Color NumToColor(float Num, float min, float max)
        {
            int c;
            Color cl = new Color();

            try
            {
                if (Num <= max && Num >= min)
                {
                    c = (int)(1275 * ((Num - min) / (max - min)));
                }
                else if (Num > max)
                {
                    c = 1275;
                }
                else
                {
                    c = 0;
                }

                if (c <= 255)
                    cl = Color.FromArgb(255 - c, 0, 255);//(255,0,255)-(0,0,255)紫色->蓝色
                else if (c <= 510)
                    cl = Color.FromArgb(0, c - 255, 255);//(0,0,255)-(0,255,255)蓝色->青色
                else if (c <= 765)
                    cl = Color.FromArgb(0, 255, 765 - c);//(0,255,255)-(0,255,0)青色->绿色
                else if (c <= 1020)
                    cl = Color.FromArgb(c - 765, 255, 0);//(0,255,0)-(255,255,0)绿色->黄色
                else
                    cl = Color.FromArgb(255, 1275 - c, 0);//(255,255,0)-(255,0,0)黄色->红色
            }
            catch (Exception ex)
            {
                //int aa = 0;
            }
            return cl;
        }

        public Color NumToColor2(float Num, float min, float max)
        {
            int c;
            Color cl = new Color();

            try
            {
                if (Num <= max && Num >= min)
                {
                    c = (int)(1275 * ((Num - min) / (max - min)));
                }
                else if (Num > max)
                {
                    c = 1275;
                }
                else
                {
                    c = 0;
                }

                if (c <= 255)
                    cl = Color.FromArgb(0, c, 255);//(0,0,255)-(0,255,255)蓝色->青色
                else if (c <= 510)
                    cl = Color.FromArgb(0, 255, 510 - c);//(0,255,255)-(0,255,0)青色->绿色
                else if (c <= 765)
                    cl = Color.FromArgb(c - 510, 255, 0);//(0,255,0)-(255,255,0)绿色->黄色
                else if (c <= 1020)
                    cl = Color.FromArgb(255, 1020 - c, 0);//(255,255,0)-(255,0,0)黄色->红色
                else
                    cl = Color.FromArgb(255, 0, c - 1020);//(255,0,255)-(255,0,255)红色->紫色

            }
            catch (Exception ex)
            {
                //int aa = 0;
            }
            return cl;
        }

        /// <summary>
        /// 红色->紫->蓝->绿->红
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Color NumToColor3(float Num, float min, float max)
        {
            int c;
            Color cl = new Color();

            try
            {
                if (Num <= max && Num >= min)
                {
                    c = (int)(1020 * ((Num - min) / (max - min)));
                }
                else if (Num > max)
                {
                    c = 1021;
                }
                else
                {
                    c = 0;
                }

                if (c <= 255)
                    cl = Color.FromArgb(255, 0, c);//(255,0,0)-(255,0,255)红色->紫色
                else if (c <= 510)
                    cl = Color.FromArgb(510 - c, 0, 255);//(255,0,255)-(0,0,255)紫色->蓝色

                else if (c <= 637)
                    cl = Color.FromArgb(c - 510, c - 510, 255);//(0,0,255)-(127,127,255)蓝色->浅蓝
                else if (c <= 765)
                    cl = Color.FromArgb(765 - c, c - 510, 2 * (765 - c));//(127,127,255)-(0,255,0)浅蓝->绿色

                else if (c <= 892)
                    cl = Color.FromArgb(2 * (c - 765), 1020 - c, 0);//(0,255,0)-(255,127,0)绿色->橙色
                else if (c <= 1020)
                    cl = Color.FromArgb(255, 1020 - c, 0);//(255,127,0)-(255,0,0)橙色->红色

                else
                    //cl = Color.FromArgb(0, 0, 0);//black
                    cl = Color.FromArgb(248, 248, 255);//Gainsboro

            }
            catch (Exception ex)
            {
                //int aa = 0;
            }
            return cl;
        }

        /// <summary>
        /// blue-red
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Color NumToColor4(float Num, float min, float max)
        {
            int c;
            Color cl = new Color();

            try
            {
                if (Num <= max && Num >= min)
                {
                    c = (int)(1020 * ((Num - min) / (max - min)));
                }
                else if (Num > max)
                {
                    c = 1021;
                }
                else
                {
                    c = 0;
                }

                if (c <= 255)
                    cl = Color.FromArgb(0, c, 255);//(0,0,255)-(0,255,255)蓝色->青色
                else if (c <= 510)
                    cl = Color.FromArgb(0, 255, 510 - c);//(0,255,255)-(0,255,0)青色->绿色
                else if (c <= 765)
                    cl = Color.FromArgb(c - 510, 255, 0);//(0,255,0)-(255,255,0)绿色->黄色
                else if (c <= 1020)
                    cl = Color.FromArgb(255, 1020 - c, 0);//(255,255,0)-(255,0,0)黄色->红色
                //else
                //    cl = Color.FromArgb(255, 0, c - 1020);//(255,0,255)-(255,0,255)红色->紫色
                else
                    //cl = Color.FromArgb(245, 245, 245);//white snow
                    //cl = Color.FromArgb(211, 211, 211);//light gray
                    cl = Color.FromArgb(248, 248, 255);//Gainsboro

            }
            catch (Exception ex)
            {
                //int aa = 0;
            }
            return cl;
        }

        public Color NumToColor5(float Num, float min, float max)
        {
            int c;
            Color cl = new Color();

            try
            {
                if (Num <= max && Num >= min)
                {
                    c = (int)(1275 * ((Num - min) / (max - min)));
                }
                else if (Num > max)
                {
                    c = 1276;
                }
                else
                {
                    c = 0;
                }
                if (c <= 127)
                    cl = Color.FromArgb(0, 0, 127 + c);//(0,0,127)-(0,0,255)深蓝色->蓝色
                else if (c <= 382)
                    cl = Color.FromArgb(0, c - 127, 255);//(0,0,255)-(0,255,255)蓝色->青色
                else if (c <= 637)
                    cl = Color.FromArgb(0, 255, 637 - c);//(0,255,255)-(0,255,0)青色->绿色
                else if (c <= 892)
                    cl = Color.FromArgb(c - 637, 255, 0);//(0,255,0)-(255,255,0)绿色->黄色
                else if (c <= 1147)
                    cl = Color.FromArgb(255, 1147 - c, 0);//(255,255,0)-(255,0,0)黄色->红色
                //else
                //    cl = Color.FromArgb(255, 0, c - 1020);//(255,0,255)-(255,0,255)红色->紫色
                else if (c <= 1275)
                    cl = Color.FromArgb(1403 - c, 0, 0);//(255,0,0)-(127,0,0)红色->深红色
                else
                    //cl = Color.FromArgb(245, 245, 245);//white snow
                    //cl = Color.FromArgb(211, 211, 211);//light gray
                    cl = Color.FromArgb(248, 248, 255);//Gainsboro

                //cl = Color.FromArgb(128,128,128);//gray

            }
            catch (Exception ex)
            {
                //int aa = 0;
            }
            return cl;
        }

        /// <summary>
        /// 红色->绿->蓝->紫->红
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Color NumToColor6(float Num, float min, float max)
        {
            int c;
            Color cl = new Color();

            try
            {
                if (Num <= max && Num >= min)
                {
                    c = (int)(1020 * ((Num - min) / (max - min)));
                }
                else if (Num > max)
                {
                    c = 1021;
                }
                else
                {
                    c = 0;
                }

                if (c <= 127)
                    cl = Color.FromArgb(255, c, 0);//(255,0,0)-(255,127,0)红色->橙色
                else if (c <= 255)
                    cl = Color.FromArgb(2 * (255 - c), c, 0);//(255,127,0)-(0,255,0)橙色->绿色

                else if (c <= 382)
                    cl = Color.FromArgb(c - 255, 510 - c, 2 * (c - 255));//(0,255,0)-(127,127,255)绿色->浅蓝
                else if (c <= 510)
                    cl = Color.FromArgb(510 - c, 510 - c, 255);//(127,127,255)-(0,0,255)浅蓝->蓝色

                else if (c <= 765)
                    cl = Color.FromArgb(c - 510, 0, 255);//(0,0,255)-(255,0,255)蓝色->紫色
                else if (c <= 1020)
                    cl = Color.FromArgb(255, 0, 1020 - c);//(255,0,255)-(255,0,0)紫色->红色

                else
                    cl = Color.FromArgb(248, 248, 255);//Gainsboro

            }
            catch (Exception ex)
            {
                //int aa = 0;
            }
            return cl;
        }
        /// <summary>
        /// 绘制等值图
        /// </summary>
        /// <param name="param"></param>
        /// <param name="g"></param>
        /// <param name="rectangle">绘图范围</param>
        public void DrawContour(List<Color[]> data, Graphics g, Rectangle rectangle)
        {
            //g.FillRectangle(Brushes.Black, rectangle);//填充背景色
            Pen blackPen = new Pen(Brushes.Black, 1);
            //g.DrawRectangle(blackPen, rectangle);
            try
            {
                //计算每个像素的宽度和高度
                float pixelWidth = rectangle.Width * 1.0f / data.Count;
                float pixelHeight = rectangle.Height;   //LPJ 2019-9-19
                if (data.Count > 0)
                    pixelHeight = rectangle.Height * 1.0f / ((Color[])data[0]).Count();  //LPJ 2019-9-19

                //定义画笔
                SolidBrush colorBrush = new SolidBrush(Color.White);
                Color[] color;

                //定义填充颜色位置

                for (int i = 0; i < data.Count; i++)
                {
                    color = (Color[])data[i];

                    float startX = rectangle.X + pixelWidth * i;
                    for (int j = 0; j < color.Count(); j++)
                    {
                        colorBrush.Color = color[j];
                        float startY = rectangle.Y + pixelHeight * j;
                        g.FillRectangle(colorBrush, startX, startY, pixelWidth, pixelHeight);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 绘制水平单曲线图，即纵坐标为数据，横坐标为时间序列
        /// </summary>
        /// <param name="param"></param>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void DrawLine_Horizontal(float[] data, float fScreen, string strTitle, Graphics g, Rectangle rectangle, Color color)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X + 50, rectangle.Y + 2, rectangle.Width - 60, rectangle.Height - 15);

            //画边框
            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, rect.X, rect.Y, rect.Width, rect.Height);

            //设置参数控制最大值和最小值
            float min, max, unit;
            min = 0;
            max = 0;
            if (data.Count() > 0)
            {
                CalMax_Min(data, fScreen, ref max, ref min);
                unit = (max - min) / 5.0f;
            }
            else
            {
                return;
            }


            //绘制横线
            Pen pgray = new Pen(Color.WhiteSmoke, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 1; i < 6; i++)
            {
                Point pnt = new Point(rect.X - 20, (int)(rect.Height * i / 5.0f + rect.Y));
                g.DrawLine(pgray, rect.X, rect.Height * i / 5.0f + 5 + rect.Y, rect.Width + rect.X, rect.Height * i / 5.0f + 5 + rect.Y);
                g.DrawString((min + unit * (5 - i)).ToString("0.00"), font, Brushes.DarkBlue, pnt);
            }

            //绘制纵线
            for (int i = 1; i < 5; i++)
            {
                g.DrawLine(pgray, rect.X + rect.Width * i / 5, rect.Height + rect.Y, rect.X + rect.Width * i / 5, rect.Y);
            }

            //绘制折线
            Pen pblue = new Pen(color, 1);
            float startY = 0.0f, lastY = 0.0f;
            for (int i = 0; i < data.Count(); i++)
            {
                try
                {
                    double yy = data[i];

                    if (i == 0)
                    {
                        lastY = (float)data[0];
                    }

                    startY = (float)yy;
                    float xscale = (float)rect.Width / (data.Count());
                    int yscale = rect.Height;
                    g.DrawLine(pblue, xscale * i + rect.X, ((float)max - lastY) / (float)(max - min) * yscale + rect.Y, xscale * (i + 1) + rect.X, ((float)max - startY) / (float)(max - min) * yscale + rect.Y);

                    if (yy < fScreen)
                    {
                        lastY = startY;
                    }
                }
                catch
                {
                }
            }

        }

        /// <summary>
        /// 绘制竖直单曲线图，即纵坐标为时间序列，横坐标为数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="fScreen"></param>
        /// <param name="strTitle"></param>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void DrawLine_Vertical(float[] data, float fScreen, string strTitle, Graphics g, Rectangle rectangle, Color color)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X + 15, rectangle.Y + 20, rectangle.Width - 20, rectangle.Height - 30);

            //画边框
            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, rect.X, rect.Y, rect.Width, rect.Height);

            //设置参数控制最大值和最小值
            float min, max, unit;
            min = 0;
            max = 0;
            if (data.Count() > 0)
            {
                CalMax_Min(data, fScreen, ref max, ref min);
                unit = (max - min) / 5.0f;
            }
            else
            {
                return;
            }

            //绘制横线
            Pen pgray = new Pen(Color.WhiteSmoke, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 1; i < 5; i++)
            {
                g.DrawLine(pgray, rect.X, rect.Height * i / 5.0f + 5 + rect.Y, rect.Width + rect.X, rect.Height * i / 5.0f + 5 + rect.Y);
            }

            //绘制纵线
            for (int i = 1; i < 6; i++)
            {
                Point pnt = new Point((int)(rect.X + rect.Width * i / 5.0f), rect.Y - 15);
                g.DrawLine(pgray, rect.X + rect.Width * i / 5.0f, rect.Height + rect.Y, rect.X + rect.Width * i / 5.0f, rect.Y);
                g.DrawString((min + unit * (5 - i)).ToString("0.00"), font, Brushes.DarkBlue, pnt);
            }

            //绘制折线
            Pen pblue = new Pen(color, 1);
            float startX = 0.0f, lastX = 0.0f;
            for (int i = 0; i < data.Count(); i++)
            {
                try
                {
                    double xx = data[i];
                    if (i == 0)
                    {
                        lastX = (float)data[0];
                    }

                    startX = (float)xx;
                    float xscale = rect.Width;
                    int yscale = rect.Height / data.Count();
                    g.DrawLine(pblue, (float)(max - lastX) / (float)(max - min) * xscale + rect.X, yscale * i + rect.Y, (float)(max - lastX) / (float)(max - min) * xscale + rect.X, yscale * (i + 1) + rect.Y);

                    if (xx < fScreen)
                    {
                        lastX = startX;
                    }
                }
                catch
                {
                }
            }

            //横纵坐标说明
        }

        /// <summary>
        /// 绘制水平多曲线图，即纵坐标为数据，横坐标为时间序列
        /// </summary>
        /// <param name="param"></param>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void DrawMultiLines_Horizontal(List<float[]> data, float fScreen, Graphics g, Rectangle rectangle, List<Color> color, float fMin, float fMax, int intervals)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            //g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, rect.X, rect.Y, rect.Width, rect.Height);

            //计算最大值和最小值
            float min = fMin, max = fMax, unit;
            unit = (max - min) / intervals;

            //绘制横坐标
            Pen pgray = new Pen(Color.WhiteSmoke, 0.1f);
            Font font = new System.Drawing.Font("Arial Narrow", 10);//字体

            for (int i = 0; i < intervals + 1; i++)
            {
                Point pnt = new Point(rect.X - 20, (int)(rect.Height * i / intervals + rect.Y - 5.0f));
                if (i > 0 && i < intervals)
                {
                    g.DrawLine(pgray, rect.X, rect.Height * i / intervals + rect.Y, rect.Width + rect.X, rect.Height * i / intervals + rect.Y);
                }
                g.DrawString((min + unit * (intervals - i)).ToString("0"), font, Brushes.DarkBlue, pnt);
            }

            //绘制纵坐标
            for (int i = 1; i < intervals; i++)
            {
                g.DrawLine(pgray, rect.X + rect.Width * i / intervals, rect.Height + rect.Y, rect.X + rect.Width * i / intervals, rect.Y);
            }
            //绘制曲线
            for (int i = 0; i < data.Count; i++)
            {
                Pen pblue = new Pen(color[i], 1);
                float startY = 0.0f, lastY = 0.0f;
                float[] fdata = data[i];
                for (int j = 0; j < fdata.Count(); j++)
                {
                    try
                    {
                        double yy = fdata[j];
                        if (j == 0)
                        {
                            lastY = (float)fdata[0];
                        }

                        startY = (float)yy;
                        float xscale = (float)rect.Width / (fdata.Count());
                        int yscale = rect.Height;
                        try
                        {
                            g.DrawLine(pblue, xscale * j + rect.X, ((float)max - lastY) / (float)(max - min) * yscale + rect.Y, xscale * (j + 1) + rect.X, ((float)max - startY) / (float)(max - min) * yscale + rect.Y);
                        }
                        catch
                        {
                        }
                        if (yy < fScreen)
                        {
                            lastY = startY;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }

        public void DrawMultiLines_Horizontal_Dual(Graphics g, Rectangle rectangle, List<float[]> dataLeft, float fScreenLeft, List<Color> colorLeft, float fMinLeft, float fMaxLeft,
            List<float[]> dataRight, float fScreenRight, List<Color> colorRight, float fMinRight, float fMaxRight, int intervals)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                Rectangle rect = new Rectangle(rectangle.X + 10, rectangle.Y, rectangle.Width - 20, rectangle.Height);  //LPJ 2019-9-13

                Pen pline = new Pen(Color.Black, 1);
                //g.DrawRectangle(pline, rect.X, rect.Y, rect.Width, rect.Height);

                Pen pgray = new Pen(Color.DarkGray, 0.1f);
                pgray.DashStyle = DashStyle.Dash;
                pgray.DashPattern = new float[] { 5, 5 };
                //Font font = new System.Drawing.Font("Arial Narrow", 10);//字体
                Font font = new System.Drawing.Font("Arial Narrow", 8, FontStyle.Bold);//Font2
                SizeF sizeF;
                //if (rectangle.Height < 105)
                //    font = new System.Drawing.Font("Arial Narrow", 10 * rectangle.Height / 105.0f);

                //绘制hortical
                //for (int i = 1; i < intervals; i++)
                //{
                //    g.DrawLine(pgray, rect.X + rect.Width * i / intervals, rect.Height + rect.Y, rect.X + rect.Width * i / intervals, rect.Y);
                //}
                int Multi = 1;

                if (rectangle.Height < 15) //LPJ 2019-8-19
                    Multi = 6;
                if (rectangle.Height < 25)  //LPJ 2019-8-19
                    Multi = 4;
                else if (rectangle.Height < 45) //LPJ 2019-8-19
                    Multi = 2;

                #region Left
                //计算最大值和最小值
                float min = fMinLeft, max = fMaxLeft, unit;
                unit = (max - min) / intervals;

                #region 
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

                //绘制纵坐标 
                for (int i = 0; i < intervals + 1; i++)
                {
                    Point pnt = new Point(rect.X - 30, (int)(rect.Height * i / intervals + rect.Y - 10.0f));  //LPJ 2019-9-13
                    //if (rectangle.Height < 105)
                    //    pnt = new Point(rect.X - 20 * rectangle.Height / 105, (int)(rect.Height * i / intervals + rect.Y - 5.0f));

                    if (i >= 0 && i < intervals)
                    {
                        g.DrawLine(pgray, rect.X, rect.Height * i / intervals + rect.Y, rect.Width + rect.X, rect.Height * i / intervals + rect.Y);
                    }

                    if (i % Multi == 0)
                    {
                        //LPJ 2019-9-12 cancel
                        /*
                        if (Math.Abs(max - min) > intervals)
                        {
                            sizeF = TextRenderer.MeasureText(g, (min + unit * (intervals - i)).ToString("0"), font, new Size(0, 0), TextFormatFlags.NoPadding);
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                            g.DrawString((min + unit * (intervals - i)).ToString("0"), font, Brushes.DarkBlue, pnt);
                            //string strData = (min + unit * (intervals - i)).ToString("0");
                            //g.DrawString(strData, font, Brushes.DarkBlue, new Point(rect.X - 10 * strData.Length, (int)(rect.Height * i / intervals + rect.Y - 5.0f)));
                        }
                        else
                        {
                            sizeF = TextRenderer.MeasureText(g, (min + unit * (intervals - i)).ToString("0.0"), font, new Size(0, 0), TextFormatFlags.NoPadding);
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                            g.DrawString((min + unit * (intervals - i)).ToString("0.0"), font, Brushes.DarkBlue, pnt);
                        }*/

                        sizeF = TextRenderer.MeasureText(g, (min + unit * (intervals - i)).ToString("0.0"), font, new Size(0, 0), TextFormatFlags.NoPadding);
                        g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                        g.DrawString((min + unit * (intervals - i)).ToString(strFormat), font, Brushes.DarkBlue, pnt);  //LPJ 2019-9-12
                    }
                }

                //绘制曲线
                float fmax = fMaxLeft > fMinLeft ? fMaxLeft : fMinLeft;
                float fmin = fMaxLeft < fMinLeft ? fMaxLeft : fMinLeft;
                for (int i = 0; i < dataLeft.Count; i++)
                {
                    Pen pblue = new Pen(colorLeft[i], 1.6f);
                    float startY = 0.0f, lastY = 0.0f;
                    float[] fdata = dataLeft[i];
                    List<PointF> pnt = new List<PointF>(); //LPJ 2019-7-22
                    for (int j = 0; j < fdata.Count(); j++)
                    {
                        #region LPJ 2019-7-22
                        if (Math.Abs(fdata[j]) > fScreenLeft)
                            continue;
                        float data = fdata[j];   //LPJ 2019-7-24
                        if (data > fmax)       //LPJ 2019-7-24
                            data = fmax;
                        if (data < fmin)        //LPJ 2019-7-24
                            data = fmin;

                        float xscale = (float)rect.Width / (fdata.Count() - 1);  // LPJ 2019-7-23
                        if (fdata.Count() <= 1)  //LPJ 2019-8-16
                            xscale = rect.Width;

                        int yscale = rect.Height;
                        PointF p = new PointF(xscale * j + rect.X, (max - data) / (float)(max - min) * yscale + rect.Y);
                        pnt.Add(p);
                        lastY = fdata[j];
                        #endregion

                        #region cancel
                        /*
                    try
                    {
                        double yy = fdata[j];

                        if (yy > fmax)
                        {   //LPJ2019-7-19
                            yy = fmax * 0.99f;  //LPJ 2019-6-6
                            //continue; //LPJ 2019-7-22 
                            
                        }
                        if (yy < fmin)
                        {   //LPJ2019-7-19
                            yy = fmin * 0.99f;  //LPJ 2019-6-6
                            //continue;  //LPJ 2019-7-22 
                        }

                        if (j == 0)
                        {
                            lastY = (float)yy;
                        }
                        if (yy < fScreenLeft)
                        {
                            startY = (float)yy;
                        }
                        else
                        {
                            startY = lastY;
                        }
                        float xscale = (float)rect.Width / (fdata.Count());
                        int yscale = rect.Height;
                        try
                        {
                            g.DrawLine(pblue, xscale * j + rect.X, (max - lastY) / (float)(max - min) * yscale + rect.Y, xscale * (j + 1) + rect.X, (max - startY) / (float)(max - min) * yscale + rect.Y);
                        }
                        catch
                        {
                        }
                        if (yy < fScreenLeft)
                        {
                            lastY = startY;
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }
                     * */
                        #endregion
                    }

                    if (pnt.Count > 1) //LPJ 2019-7-22
                    {
                        //PointF p = new PointF(rect.Width+ rect.X, (max - lastY) / (float)(max - min) * rect.Height + rect.Y);
                        //pnt.Add(p);

                        PointF[] pntLines = new PointF[pnt.Count];
                        for (int k = 0; k < pnt.Count; k++)
                        {
                            pntLines[k] = pnt[k];
                        }
                        g.DrawLines(pblue, pntLines);
                    }
                    else if (pnt.Count == 1)  //LPJ 2019-8-16
                    {
                        PointF pntline = new PointF(rect.X + rect.Width, pnt[0].Y);  //LPJ 2019-8-16
                        g.DrawLine(pblue, pntline, pnt[0]);  //LPJ 2019-8-16
                    }

                }

                #endregion

                #region  Right
                //计算最大值和最小值
                min = fMinRight;
                max = fMaxRight;

                unit = (max - min) / intervals;

                #region
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

                //绘制纵坐标 
                if (dataRight.Count > 0)
                {
                    for (int i = 0; i < intervals + 1; i++)
                    {
                        Point pnt = new Point(rect.X + rect.Width + 5, (int)(rect.Height * i / intervals + rect.Y - 10.0f));
                        //g.DrawLine(pgray, rect.X, rect.Height * i / intervals + rect.Y, rect.Width + rect.X, rect.Height * i / intervals + rect.Y);

                        if (i % Multi == 0)
                        {
                            /*
                            if (Math.Abs(max - min) > intervals)
                            {
                                sizeF = TextRenderer.MeasureText(g, (min + unit * (intervals - i)).ToString("0"), font, new Size(0, 0), TextFormatFlags.NoPadding);
                                g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                g.DrawString((min + unit * (intervals - i)).ToString("0"), font, Brushes.DarkBlue, pnt);
                            }
                            else
                            {
                                sizeF = TextRenderer.MeasureText(g, (min + unit * (intervals - i)).ToString("0.0"), font, new Size(0, 0), TextFormatFlags.NoPadding);
                                g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                                g.DrawString((min + unit * (intervals - i)).ToString("0.0"), font, Brushes.DarkBlue, pnt);
                            }*/

                            sizeF = TextRenderer.MeasureText(g, (min + unit * (intervals - i)).ToString("0.0"), font, new Size(0, 0), TextFormatFlags.NoPadding);
                            g.FillRectangle(Brushes.White, new Rectangle(pnt.X, pnt.Y, Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height)));

                            g.DrawString((min + unit * (intervals - i)).ToString(strFormat), font, Brushes.DarkBlue, pnt);
                        }
                    }
                }

                //绘制曲线
                try
                {
                    fmax = fMaxRight > fMinRight ? fMaxRight : fMinRight;
                    fmin = fMaxRight < fMinRight ? fMaxRight : fMinRight;
                    for (int i = 0; i < dataRight.Count; i++)
                    {
                        Pen pblue = new Pen(colorRight[i], 1.6f);
                        float startY = 0.0f, lastY = 0.0f;
                        float[] fdata = dataRight[i];
                        List<PointF> pnt = new List<PointF>(); //LPJ 2019-7-22
                        for (int j = 0; j < fdata.Count(); j++)
                        {
                            #region LPJ 2019-7-22
                            if (Math.Abs(fdata[j]) > fScreenRight)
                                continue;

                            float data = fdata[j];   //LPJ 2019-7-24
                            if (data > fmax)       //LPJ 2019-7-24
                                data = fmax;
                            if (data < fmin)        //LPJ 2019-7-24
                                data = fmin;

                            float xscale = (float)rect.Width / (fdata.Count() - 1);
                            if (fdata.Count() <= 1)  //LPJ 2019-8-16
                                xscale = rect.Width;

                            int yscale = rect.Height;
                            PointF p = new PointF(xscale * j + rect.X, (max - data) / (float)(max - min) * yscale + rect.Y);
                            pnt.Add(p);
                            lastY = fdata[j];
                            #endregion

                            #region cancel
                            /*
                        try
                        {
                            double yy = fdata[j];
                      
                            if (yy > fmax)
                                yy = fmax * 0.99f;
                            if (yy < fmin)
                                yy = fmin * 0.99f;
                            if (j == 0)
                            {
                                lastY = (float)yy;
                            }
                            if (yy < fScreenRight)
                            {
                                startY = (float)yy;
                            }
                            else
                            {
                                startY = lastY;
                            }
                            float xscale = (float)rect.Width / (fdata.Count());
                            int yscale = rect.Height;
                            try
                            {
                                g.DrawLine(pblue, xscale * j + rect.X, ((float)max - lastY) / (float)(max - min) * yscale + rect.Y, xscale * (j + 1) + rect.X, ((float)max - startY) / (float)(max - min) * yscale + rect.Y);
                            }
                            catch
                            {
                            }
                            if (yy < fScreenRight)
                            {
                                lastY = startY;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                         * */
                            #endregion
                        }

                        if (pnt.Count > 1) //LPJ 2019-7-22
                        {

                            //PointF p = new PointF(rect.Width + rect.X, (max - lastY) / (float)(max - min) * rect.Height + rect.Y);
                            //pnt.Add(p);

                            PointF[] pntLines = new PointF[pnt.Count];
                            for (int k = 0; k < pnt.Count; k++)
                            {
                                pntLines[k] = pnt[k];
                            }
                            g.DrawLines(pblue, pntLines);
                        }
                        else if (pnt.Count == 1)  //LPJ 2019-8-16
                        {
                            PointF pntline = new PointF(rect.X + rect.Width, pnt[0].Y);  //LPJ 2019-8-16
                            g.DrawLine(pblue, pntline, pnt[0]);  //LPJ 2019-8-16
                        }
                    }
                }
                catch { }

                #endregion
            }
            catch
            { }
        }

        /// <summary>
        /// 绘制竖直多曲线图，即纵坐标为时间序列，横坐标为数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void DrawMultiLines_Vertical(Graphics g, Rectangle rectangle, List<float[]> data, float fScreen, List<Color> color, float fMax, float fMin)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            g.FillRectangle(Brushes.White, rectangle);
            Rectangle rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            Pen pline = new Pen(Color.Black, 1);
            g.DrawRectangle(pline, rect);

            //计算最大值和最小值
            float min = fMin, max = fMax;

            //绘制曲线
            for (int i = 0; i < data.Count; i++)
            {
                float[] fdata = data[i];
                Pen pblue = new Pen(color[i], 1.5f);
                float startX = 0.0f, lastX = 0.0f;

                //for (int j = 0; j < fdata.Length; j++)
                //{
                //    try
                //    {
                //        if (j == 0)
                //            lastX = (float)fdata[0];

                //        //if (lastX > fScreen)
                //        //    lastX = fScreen;
                //        //if (lastX < min)
                //        //    lastX = min;

                //        if (Math.Abs(fdata[j]) < fScreen)
                //        //if (fdata[j] <= max && fdata[j] >= min)
                //        {
                //            startX = fdata[j];
                //            float xscale = rect.Width;
                //            float yscale = rect.Height / (float)fdata.Length;
                //            g.DrawLine(pblue, (float)(lastX - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y, (float)(startX - min) / (float)(max - min) * xscale + rect.X, yscale * (j + 1) + rect.Y);

                //            lastX = startX;
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}

                for (int j = 0; j < fdata.Length; j++)
                {
                    try
                    {
                        if (j == 0)
                        {
                            lastX = (float)fdata[0];
                            continue;
                        }

                        //if (lastX > fScreen)
                        //    lastX = fScreen;
                        //if (lastX < min)
                        //    lastX = min;

                        if (Math.Abs(fdata[j]) < fScreen)
                        //if (fdata[j] <= max && fdata[j] >= min)
                        {
                            startX = fdata[j];
                            float xscale = rect.Width;
                            float yscale = rect.Height / (float)fdata.Length;
                            g.DrawLine(pblue, (float)(lastX - min) / (float)(max - min) * xscale + rect.X, yscale * (j - 1) + rect.Y, (float)(startX - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                            lastX = startX;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void DrawMultiLines_Vertical_Dual(Graphics g, Rectangle rectangle, List<float[]> data, List<float[]> subdata, float fScreen, List<Color> color, float fMax, float fMin,
            List<float[]> dataDown, float fScreenDown, List<Color> colorDown, float fMaxDown, float fMinDown)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                g.FillRectangle(Brushes.White, rectangle);
                Rectangle rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

                Pen pline = new Pen(Color.Black, 1);
                g.DrawRectangle(pline, rect);

                //计算最大值和最小值
                float min = fMin, max = fMax;

                //绘制曲线
                for (int i = 0; i < data.Count; i++)
                {
                    float[] fdata = data[i];
                    Pen pblue = new Pen(color[i], 1.6f);
                    float startX = 0.0f, lastX = 0.0f;
                    List<PointF> pnt = new List<PointF>(); //LPJ 2019-7-22
                    float flastdata = 0; //LPJ 2020-8-6
                    for (int j = 0; j < fdata.Length; j++)
                    {
                        try
                        {
                            #region   //LPJ 2019-7-22
                            if (Math.Abs(fdata[j]) > fScreen)  //LPJ 2019-6-17
                                continue;

                            float _data = fdata[j];   //LPJ 2019-7-24
                                                      //if (_data > max)       //LPJ 2019-7-24   //LPJ 2020-8-5
                                                      //    _data = max;
                                                      //if (_data < min)        //LPJ 2019-7-24  //LPJ 2020-8-5
                                                      //    _data = min;

                            if (j == 0)  //LPJ 2020-8-6
                                flastdata = _data;

                            float yscale = rect.Height / (float)(fdata.Length - 1);
                            if (_data > max && flastdata < max)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (max - flastdata) + yscale * (j - 1) * (_data - max)) / (_data - flastdata);
                                PointF p = new PointF(rect.Width + rect.X, yy + rect.Y);
                                pnt.Add(p);
                            }
                            else if (_data <= max && flastdata > max)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (flastdata - max) + yscale * (j - 1) * (max - _data)) / (flastdata - _data);
                                PointF p = new PointF(rect.Width + rect.X, yy + rect.Y);
                                pnt.Add(p);

                                if (_data >= min)
                                {
                                    float xscale = rect.Width;
                                    PointF p1 = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);
                                    pnt.Add(p1);
                                }
                            }
                            else if (_data < min && flastdata > min)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (flastdata - min) + yscale * (j - 1) * (min - _data)) / (flastdata - _data);
                                PointF p = new PointF(rect.X, yy + rect.Y);
                                pnt.Add(p);
                            }
                            else if (_data >= min && flastdata < min)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (min - flastdata) + yscale * (j - 1) * (_data - min)) / (_data - flastdata);
                                PointF p = new PointF(rect.X, yy + rect.Y);
                                pnt.Add(p);

                                if (_data <= max)
                                {
                                    float xscale = rect.Width;
                                    PointF p1 = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);
                                    pnt.Add(p1);
                                }
                            }
                            else if (_data <= max && _data >= min)  //LPJ 2020-8-6
                            {
                                float xscale = rect.Width;
                                //float yscale = rect.Height / (float)(fdata.Length - 1);
                                PointF p = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                                pnt.Add(p);
                            }

                            flastdata = _data;  //LPJ 2020-8-6

                            //float xscale = rect.Width;
                            //float yscale = rect.Height / (float)(fdata.Length - 1);
                            //PointF p = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                            //pnt.Add(p);
                            #endregion

                            /*
                         * 
                          if (j == 0)
                        {
                            lastX = (float)fdata[0];
                            continue;
                        }
                        if (Math.Abs(fdata[j]) > fScreen)  //LPJ 2019-6-17
                            fdata[j] = fScreen * 0.99f;
                        if (fdata[j] < fMin)  //LPJ 2019-6-17
                            fdata[j] = fMin * 0.99f;

                        //if (Math.Abs(lastX) > fScreen || lastX < fMin) //LPJ 2019-2-14  //LPJ 2019-3-28  //LPJ 2019-6-17
                        //    lastX = fdata[j];

                        //if (Math.Abs(fdata[j]) < fScreen && fdata[j] >= fMin) //LPJ 2019-3-28 //LPJ 2019-6-17
                        //if (fdata[j] <= max && fdata[j] >= min)
                        {
                            startX = fdata[j];
                            float xscale = rect.Width;
                            float yscale = rect.Height / (float)(fdata.Length - 1);
                            //g.DrawLine(pblue, (float)(lastX - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y, (float)(startX - min) / (float)(max - min) * xscale + rect.X, yscale * (j + 1) + rect.Y);
                            g.DrawLine(pblue, (float)(lastX - min) / (float)(max - min) * xscale + rect.X, yscale * (j - 1) + rect.Y, (float)(startX - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                            lastX = startX;
                        }
                         * */

                        }
                        catch
                        {
                        }
                    }

                    if (pnt.Count > 1) //LPJ 2019-7-24
                    {
                        PointF[] pntLines = new PointF[pnt.Count];
                        for (int k = 0; k < pnt.Count; k++)
                        {
                            pntLines[k] = pnt[k];
                        }
                        g.DrawLines(pblue, pntLines);
                    }
                }

                #region  //LPJ 2019-8-22
                for (int i = 0; i < subdata.Count; i++)
                {

                    float[] fdata = data[i];
                    float[] fsubdata = subdata[i];

                    Pen pblue = new Pen(color[i], 0.5f);

                    pblue.DashStyle = DashStyle.Custom;
                    pblue.DashPattern = new float[] { 0.5f, 0.5f };

                    List<PointF> pnt1 = new List<PointF>();
                    List<PointF> pnt2 = new List<PointF>();

                    float d = 1;  //LPJ 2019-8-23
                    if (i == 3)
                        d = (float)Math.Sqrt(2.0);  //LPJ 2019-8-23

                    for (int j = 0; j < fdata.Length; j++)
                    {
                        try
                        {
                            #region  
                            if (Math.Abs(fdata[j] + fsubdata[j] * d) > fScreen) //LPJ 2019-8-23
                                continue;
                            if (Math.Abs(fdata[j] - fsubdata[j] * d) > fScreen) //LPJ 2019-8-23
                                continue;

                            float _data1 = fdata[j] + fsubdata[j] * d;  //LPJ 2019-8-23
                            float _data2 = fdata[j] - fsubdata[j] * d;  //LPJ 2019-8-23
                            if (_data1 > max)
                                _data1 = max;
                            if (_data1 < min)
                                _data1 = min;

                            if (_data2 > max)
                                _data2 = max;
                            if (_data2 < min)
                                _data2 = min;

                            float xscale = rect.Width;
                            float yscale = rect.Height / (float)(fdata.Length - 1);
                            PointF p1 = new PointF((float)(_data1 - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);
                            PointF p2 = new PointF((float)(_data2 - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);
                            pnt1.Add(p1);
                            pnt2.Add(p2);
                            #endregion
                        }
                        catch
                        {
                        }
                    }

                    if (pnt1.Count > 1) //LPJ 2019-7-24
                    {
                        PointF[] pntLines = new PointF[pnt1.Count];
                        for (int k = 0; k < pnt1.Count; k++)
                        {
                            pntLines[k] = pnt1[k];
                        }
                        g.DrawLines(pblue, pntLines);
                    }

                    if (pnt2.Count > 1) //LPJ 2019-7-24
                    {
                        PointF[] pntLines = new PointF[pnt2.Count];
                        for (int k = 0; k < pnt2.Count; k++)
                        {
                            pntLines[k] = pnt2[k];
                        }
                        g.DrawLines(pblue, pntLines);
                    }
                }
                #endregion

                //绘制曲线down
                min = fMinDown;
                max = fMaxDown;
                for (int i = 0; i < dataDown.Count; i++)
                {
                    float[] fdata = dataDown[i];
                    Pen pblue = new Pen(colorDown[i], 1.6f);
                    float startX = 0.0f, lastX = 0.0f;
                    List<PointF> pnt = new List<PointF>(); //LPJ 2019-7-22

                    float flastdata = 0; //LPJ 2020-8-6
                    for (int j = 0; j < fdata.Length; j++)
                    {
                        try
                        {
                            #region  //LPJ 2019-7-22
                            if (Math.Abs(fdata[j]) > fScreenDown)  //LPJ 2019-6-17
                                continue;

                            float _data = fdata[j];   //LPJ 2019-7-24
                            //if (_data > max)       //LPJ 2019-7-24   //LPJ 2020-8-5
                            //    _data = max;
                            //if (_data < min)        //LPJ 2019-7-24  //LPJ 2020-8-5
                            //    _data = min;

                            if (j == 0)  //LPJ 2020-8-6
                                flastdata = _data;

                            float yscale = rect.Height / (float)(fdata.Length - 1);
                            if (_data > max && flastdata < max)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (max - flastdata) + yscale * (j - 1) * (_data - max)) / (_data - flastdata);
                                PointF p = new PointF(rect.Width + rect.X, yy + rect.Y);
                                pnt.Add(p);
                            }
                            else if (_data <= max && flastdata > max)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (flastdata - max) + yscale * (j - 1) * (max - _data)) / (flastdata - _data);
                                PointF p = new PointF(rect.Width + rect.X, yy + rect.Y);
                                pnt.Add(p);

                                if (_data >= min)
                                {
                                    float xscale = rect.Width;
                                    PointF p1 = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);
                                    pnt.Add(p1);
                                }
                            }
                            else if (_data < min && flastdata > min)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (flastdata - min) + yscale * (j - 1) * (min - _data)) / (flastdata - _data);
                                PointF p = new PointF(rect.X, yy + rect.Y);
                                pnt.Add(p);
                            }
                            else if (_data >= min && flastdata < min)  //LPJ 2020-8-6
                            {
                                float yy = (yscale * j * (min - flastdata) + yscale * (j - 1) * (_data - min)) / (_data - flastdata);
                                PointF p = new PointF(rect.X, yy + rect.Y);
                                pnt.Add(p);

                                if (_data <= max)
                                {
                                    float xscale = rect.Width;
                                    PointF p1 = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);
                                    pnt.Add(p1);
                                }
                            }
                            else if (_data <= max && _data >= min)  //LPJ 2020-8-6
                            {
                                float xscale = rect.Width;
                                //float yscale = rect.Height / (float)(fdata.Length - 1);
                                PointF p = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                                pnt.Add(p);
                            }

                            flastdata = _data;  //LPJ 2020-8-6

                            //float xscale = rect.Width;
                            //float yscale = rect.Height / (float)(fdata.Length - 1);
                            //PointF p = new PointF((float)(_data - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                            //pnt.Add(p);
                            #endregion

                            /*
                        if (j == 0)
                        {
                            lastX = (float)fdata[0];
                            continue;
                        }

                        //if (lastX > fScreenDown)
                        //    lastX = fScreenDown;
                        if (fdata[j] > max) //LPJ 2019-6-17
                            fdata[j] = max * 0.99f;
                        if (fdata[j] < min)  //LPJ 2019-6-17
                            fdata[j] = min * 0.99f;

                        //if (fdata[j] < fScreenDown)
                        //if (fdata[j] <= max && fdata[j] >= min)
                        {
                            startX = fdata[j];
                            float xscale = rect.Width;
                            float yscale = rect.Height / (float)(fdata.Length - 1);
                            //g.DrawLine(pblue, (float)(lastX - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y, (float)(startX - min) / (float)(max - min) * xscale + rect.X, yscale * (j + 1) + rect.Y);
                            g.DrawLine(pblue, (float)(lastX - min) / (float)(max - min) * xscale + rect.X, yscale * (j - 1) + rect.Y, (float)(startX - min) / (float)(max - min) * xscale + rect.X, yscale * j + rect.Y);

                            lastX = startX;
                        }
                         * */
                        }
                        catch { }
                    }

                    if (pnt.Count > 1) //LPJ 2019-7-22
                    {
                        PointF[] pntLines = new PointF[pnt.Count];
                        for (int k = 0; k < pnt.Count; k++)
                        {
                            pntLines[k] = pnt[k];
                        }
                        g.DrawLines(pblue, pntLines);
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 绘制图例， 最小值显示为紫色
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        public void DrawLegend(Graphics g, Rectangle rect)
        {
            //绘制图例
            for (int num = 0; num <= 1275; num++)
            {
                float x = rect.Height * ((float)num / 1275); //LPJ 2013-6-22
                Color c;

                //c = NumToColor(num, 2, true, 1275);
                c = NumToColor(num, 0, 1275);

                using (Pen p = new Pen(c))
                {
                    p.Width = rect.Width / 1275; //LPJ 2013-6-22
                    g.DrawLine(p, rect.X, rect.Y + rect.Height - x, rect.X + rect.Width, rect.Y + rect.Height - x);

                    p.Dispose();
                }
            }
        }

        /// <summary>
        /// 最小值显示为蓝色
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        public void DrawLegend2(Graphics g, Rectangle rect)
        {
            //绘制图例
            for (int num = 0; num <= 1275; num++)
            {
                float x = rect.Height * ((float)num / 1275); //LPJ 2013-6-22
                Color c;

                //c = NumToColor(num, 2, true, 1275);
                c = NumToColor2(num, 0, 1275);

                using (Pen p = new Pen(c))
                {
                    p.Width = rect.Width / 1275; //LPJ 2013-6-22
                    g.DrawLine(p, rect.X, rect.Y + rect.Height - x, rect.X + rect.Width, rect.Y + rect.Height - x);

                    p.Dispose();
                }
            }
        }

        /// <summary>
        /// 红-红
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        public void DrawLegend3(Graphics g, Rectangle rect)
        {
            //绘制图例
            for (int num = 0; num <= 1020; num++)
            {
                float x = rect.Height * ((float)num / 1020); //LPJ 2013-6-22
                Color c;

                //c = NumToColor(num, 2, true, 1275);
                c = NumToColor6(num, 0, 1020);

                using (Pen p = new Pen(c))
                {
                    p.Width = rect.Width / 1020; //LPJ 2013-6-22
                    //g.DrawLine(p, rect.X, x + rect.Y, rect.X + rect.Width, x + rect.Y);
                    g.DrawLine(p, rect.X, rect.Y + rect.Height - x, rect.X + rect.Width, rect.Y + rect.Height - x);

                    p.Dispose();
                }
            }
        }

        /// <summary>
        /// 最小值显示为蓝色-red ,black
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        public void DrawLegend4(Graphics g, Rectangle rect)
        {
            //绘制图例
            for (int num = 0; num <= 1020; num++)
            {
                float x = rect.Height * ((float)num / 1020); //LPJ 2013-6-22
                Color c;

                //c = NumToColor(num, 2, true, 1275);
                c = NumToColor4(num, 0, 1020);


                using (Pen p = new Pen(c))
                {
                    p.Width = rect.Width / 1020; //LPJ 2013-6-22
                    //g.DrawLine(p, rect.X, x + rect.Y, rect.X + rect.Width, x + rect.Y);
                    g.DrawLine(p, rect.X, rect.Y + rect.Height - x, rect.X + rect.Width, rect.Y + rect.Height - x);

                    p.Dispose();
                }
            }
        }

        public void DrawLegend5(Graphics g, Rectangle rect)
        {
            //绘制图例
            for (int num = 0; num <= 1275; num++)
            {
                float x = rect.Height * ((float)num / 1275); //LPJ 2013-6-22
                Color c;

                //c = NumToColor(num, 2, true, 1275);
                //c = NumToColor4(num, 0, 1020);
                c = NumToColor5(num, 0, 1275);

                using (Pen p = new Pen(c))
                {
                    p.Width = rect.Width / 1275; //LPJ 2013-6-22
                    //g.DrawLine(p, rect.X, x + rect.Y, rect.X + rect.Width, x + rect.Y);
                    g.DrawLine(p, rect.X, rect.Y + rect.Height - x, rect.X + rect.Width, rect.Y + rect.Height - x);

                    p.Dispose();
                }
            }
        }
        /// <summary>
        /// 计算最大值和最小值
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="screen">门限值，该值范围内为有效数据</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        private void CalMax_Min(float[] data, float fScreen, ref float max, ref float min)
        {
            if (data[0] < fScreen)
            {
                min = data[0];
                max = data[0];
            }
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i] < fScreen)
                {
                    if (min > data[i])
                        min = data[i];
                    if (max < data[i])
                        max = data[i];
                }
            }
        }
        #endregion
    }
}
