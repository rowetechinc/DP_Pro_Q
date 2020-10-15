using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADCP
{
    public partial class FrmBeamCheck : Form
    {
        public FrmBeamCheck(float range,int cells,float[,] snr)
        {
            InitializeComponent();
            maxRange = range;
            Cells = cells;
            SNR = snr;
            maxSNR = OnCalMax(SNR);
        }

        private float maxRange = 0;
        private int Cells = 200;
        private float[,] SNR = new float[4, 200];
        private float maxSNR = 0.001f;

        private float OnCalMax(float[,] snr)
        {
            try
            {
                float max = snr[0, 0];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < Cells; j++)
                    {
                        if (max < snr[i, j])
                            max = snr[i, j];
                    }
                }
                if (max < 0.00000001f)
                    max = 0.001f;
                return max;
            }
            catch
            {
                return 0.001f;
            }
        }

        private void panelBeamCheck_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                //绘制SNR图
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, this.panelBeamCheck.DisplayRectangle);

                Graphics g = MainBuffer.Graphics;
                g.FillRectangle(Brushes.White, 0, 0, panelBeamCheck.Width, panelBeamCheck.Height);

                DrawSNR(e.Graphics, panelBeamCheck.DisplayRectangle, SNR, maxRange, Cells);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DrawSNR(Graphics g, Rectangle rectangle, float[,] snr, float Depth, int cells)
        {
            try
            {
                int x_rect = rectangle.X + 28;
                int y_rect = rectangle.Y + 3;
                int width_rect = rectangle.Width - 30;
                int height_rect = rectangle.Height - 25;
                g.DrawRectangle(Pens.Black, x_rect, y_rect, width_rect, height_rect); //绘制边框

                #region 图例
                int x_Legend = x_rect + width_rect - 47;
                int y_Legend = y_rect + height_rect - 87;
                g.DrawRectangle(Pens.Black, x_Legend, y_Legend, 35, 75);
                g.DrawString("Intensity", new Font("Arial Narrow", 8), Brushes.Black, new PointF(x_Legend + 0, y_Legend + 0));
                Pen redpen = new Pen(Brushes.Red, 3);
                g.DrawLine(redpen, new PointF(x_Legend + 4, y_Legend + 23), new Point(x_Legend + 16, y_Legend + 23));
                g.DrawString("1", new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_Legend + 20, y_Legend + 15));

                Pen bluepen = new Pen(Brushes.Blue, 3);
                g.DrawLine(bluepen, new PointF(x_Legend + 4, y_Legend + 38), new Point(x_Legend + 16, y_Legend + 38));
                g.DrawString("2", new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_Legend + 20, y_Legend + 30));

                Pen greenpen = new Pen(Brushes.Green, 3);
                g.DrawLine(greenpen, new PointF(x_Legend + 4, y_Legend + 53), new Point(x_Legend + 16, y_Legend + 53));
                g.DrawString("3", new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_Legend + 20, y_Legend + 45));

                Pen purplepen = new Pen(Brushes.Purple, 3);
                g.DrawLine(purplepen, new PointF(x_Legend + 4, y_Legend + 68), new Point(x_Legend + 16, y_Legend + 68));
                g.DrawString("4", new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_Legend + 20, y_Legend + 60));
                #endregion

                #region 横轴、纵轴
                //根据水深，计算绘制的横栏数

                int lines = (int)(Depth / ((int)(Depth / 5) + 1)); //划分的行数
                float fheight = (lines * ((int)(Depth / 5) + 1) / Depth) * height_rect; //整数部分占据的高度

                for (int i = 1; i < lines + 1; i++)
                {
                    string strdata = (i * ((int)(Depth / 5) + 1)).ToString();

                    g.DrawLine(Pens.LightGray, new PointF(x_rect, y_rect + fheight * i / lines), new PointF(x_rect + width_rect, y_rect + fheight * i / lines));
                    g.DrawString(strdata, new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_rect - 15, y_rect + fheight * i / lines - 10));
                }
                System.Drawing.StringFormat sf = new System.Drawing.StringFormat(); //竖着写
                sf.FormatFlags = StringFormatFlags.DirectionVertical;
                g.DrawString("Range(m)", new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_rect - 26, y_rect + height_rect / 2 - 20), sf);

                for (int i = 1; i < 5; i++)
                {
                    string strdata = (i * maxSNR / 5).ToString();
                    g.DrawLine(Pens.LightGray, new PointF(x_rect + width_rect * i / 5, y_rect), new PointF(x_rect + width_rect * i / 5, y_rect + height_rect));
                    g.DrawString(strdata, new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_rect + width_rect * i / 5 - 8, y_rect + height_rect));
                }
                g.DrawString("Intensity(dB)", new Font("Arial Narrow", 10), Brushes.Black, new PointF(x_rect + width_rect / 2 - 20, y_rect + height_rect + 7));
                #endregion

                #region 主图
                Point[] beam1 = new Point[cells];
                Point[] beam2 = new Point[cells];
                Point[] beam3 = new Point[cells];
                Point[] beam4 = new Point[cells];
                for (int i = 0; i < cells; i++)
                {
                    beam1[i].X = (int)(snr[0, i] / maxSNR * width_rect) + x_rect;
                    beam1[i].Y = (i + 1) / cells * height_rect + y_rect;

                    beam2[i].X = (int)(snr[1, i] / maxSNR * width_rect) + x_rect;
                    beam2[i].Y = (i + 1) / cells * height_rect + y_rect;

                    beam3[i].X = (int)(snr[2, i] / maxSNR * width_rect) + x_rect;
                    beam3[i].Y = (i + 1) / cells * height_rect + y_rect;

                    beam4[i].X = (int)(snr[3, i] / maxSNR * width_rect) + x_rect;
                    beam4[i].Y = (i + 1) / cells * height_rect + y_rect;
                }
                g.DrawLines(redpen, beam1);
                g.DrawLines(bluepen, beam2);
                g.DrawLines(greenpen, beam3);
                g.DrawLines(purplepen, beam4);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
