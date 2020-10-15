using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;//写入文件
using System.IO.Ports;//串口类命名空间
using System.Collections;//ArrayList 类命名空间
using System.Globalization;

namespace ADCP
{
    public partial class DVL_Windows : Form
    {
        public class NoPaintBackGroundPanel : Panel
        {
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                //不绘制背景,即跳过面板的背景绘制环节，这样在面板上绘图替换时两个缓冲区?图形之间就不会因为夹着一块背景图出现闪烁了
            }
            public NoPaintBackGroundPanel()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            }
        }

        public class NoPaintBackGroundTabPage : TabPage
        {
            public NoPaintBackGroundTabPage()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            }
            //protected override void OnNotifyMessage(Message m)
            //{
            //    if (m.Msg != 0x14)
            //    {
            //        base.OnNotifyMessage(m);
            //    }
            //}
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                
            }

        }

        public class NoPaintBackGroundListView : ListView
        {
            public NoPaintBackGroundListView()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            }
            protected override void OnNotifyMessage(Message m)
            {
                if (m.Msg != 0x14)
                {
                    base.OnNotifyMessage(m);
                }
            }

            protected override void OnPaintBackground(PaintEventArgs pevent)
            {
                //base.OnPaintBackground(pevent);
            }
        }

        /// LPJ 2012-8-20 
        System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(DVL_Windows));

        //////LPJ 2012-8-20 
        /// 应用资源
        /// ApplyResources 的第一个参数为要设置的控件
        ///                  第二个参数为在资源文件中的ID，默认为控件的名称
        public void ApplyResource()
        {
            GetControl(this.Controls);
            
            //Caption
            res.ApplyResources(this, "$this");
        }

        ////LPJ 2012-8-20  遍历所有控件
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
                    if (ctl1 is StatusStrip)    //LPJ 2012-8-20 遍历状态栏控件
                    {
                        foreach (ToolStripItem ctl2 in this.DVLstatusStrip.Items)
                        {
                            res.ApplyResources(ctl2, ctl2.Name);
                        }
                    }
                   
                }
            }
            catch { }
        }

        ////LPJ 2012-8-20  判断语言类型
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

        private SerialPort sp;//用默认值初始化串口;
        public DVL_Windows()
        {
            InitializeComponent();

            sp = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);//用默认值初始化串口;
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            Displayer = new DisplayDelegate(SetPosion);//委托指针指向 SetPosion 函数
            //1.设置各组合框显示当前串口的配置
            string[] ports = SerialPort.GetPortNames();//得到系统的串口号
            Array.Sort(ports);//对串口号字符串排序
            this.PortNameList.Items.AddRange(ports);//添加到组合框

            BoatBitmap = new Bitmap(Application.StartupPath + "\\SmallBoat.bmp");
            BoatBitmap.MakeTransparent(Color.White);//白色置为透明（背景）
            RotatedBoatBitmap = (Bitmap)RotateImage(BoatBitmap, 22.5F);

            //使各组合框显示当前串口的配置
            PortNameList.SelectedIndex = PortNameList.Items.IndexOf(sp.PortName);
            BaudRateList.SelectedIndex = BaudRateList.Items.IndexOf(sp.BaudRate.ToString());
            DataBitsList.SelectedIndex = DataBitsList.Items.IndexOf(sp.DataBits.ToString());
            ParityList.SelectedIndex = ParityList.Items.IndexOf(sp.Parity.ToString());
            switch (sp.StopBits.ToString())
            {
                case "None":
                    {
                        StopBitsList.SelectedIndex = StopBitsList.Items.IndexOf("0");
                        break;
                    }
                case "One":
                    {
                        StopBitsList.SelectedIndex = StopBitsList.Items.IndexOf("1");
                        break;
                    }
                case "OnePointFive":
                    {
                        StopBitsList.SelectedIndex = StopBitsList.Items.IndexOf("1.5");
                        break;
                    }
                case "Two":
                    {
                        StopBitsList.SelectedIndex = StopBitsList.Items.IndexOf("2");
                        break;
                    }
            }


        }

        private float MaxDisplayWidth;
        private float MaxDisplayHeight;
        private float CurrentDisplayWidth;
        private float CurrentDisplayHeight;
        private float CurrentDisplayUnit;
        private float CurrentDisplayTop;
        private float CurrentDisplayLeft;
        private float multiple = 1;//设置随窗口大小的改变绘图区缩放的倍数
        private float PreviousMultiple = 1;//上一次刷新后客户区所处于的倍数
        private float DragLengthX = 0;
        private float DragLengthY = 0;
        private float MouseWheelScale = 1;//滚轮缩小率的初始值
        private float Xpzn = 0, Ypzn = 0;//初始中心点经纬度
        private int LinesNumOfAllFiles;
        private Bitmap BoatBitmap, RotatedBoatBitmap;
        private PointF BoatPosition = new PointF(0, 0);

        private void DVLpanel_Paint(object sender, PaintEventArgs e)
        {
            
            Bitmap Compass = new Bitmap(Application.StartupPath + "\\Bitmap1.bmp");
            Compass.MakeTransparent(Color.Black);
            MaxDisplayWidth = 985;//DVLpanel.Width;//由于DVLpanel改变了父窗口，被放入了分栏容器中，其DVLpanel.Width已不固定，所以不能用该变量为依据，改为实际大小985
            MaxDisplayHeight = DVLpanel.Height;
            CurrentDisplayWidth = DVLpanel.ClientSize.Width;
            CurrentDisplayHeight = DVLpanel.ClientSize.Height;

            CurrentDisplayUnit = (float)CurrentDisplayWidth / 8;
            CurrentDisplayTop = DVLpanel.ClientRectangle.Top;
            CurrentDisplayLeft = DVLpanel.ClientRectangle.Left;
            multiple = (float)CurrentDisplayWidth / (float)MaxDisplayWidth;//设置随窗口大小的改变绘图区缩放的倍数

            //拖动积累量
            DragLengthX = DragLengthX * (multiple / PreviousMultiple);//改变窗口大小后，相应更新拖动积累量的值
            DragLengthY = DragLengthY * (multiple / PreviousMultiple);
            PreviousMultiple = multiple;

            /////////////////////////////         //////////////////////////////
            /////////////////////////////双缓冲绘图//////////////////////////////
            /////////////////////////////         //////////////////////////////

            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;//获取当前绘图主缓冲区上下文
            BufferedGraphics MainBuffer = currentContext.Allocate(e.Graphics, DVLpanel.DisplayRectangle);
            //设置双缓冲的图形缓冲区：使用指定的e.Graphics像素格式，创建指定大小panelTrackDisplayer.DisplayRectangle的图形缓冲区

            // Rectangle BoatRec = new Rectangle(20, 20, 32, 32);
            using (Graphics g = MainBuffer.Graphics)
            {
                //g.Clear(Color.DimGray);
                //
                //高度表
                //
                int alpha = 0;
                if (LightFlag)
                {
                    alpha = 255;
                }
                else
                {
                    alpha = 90;
                }
                Color cl = Color.FromArgb(alpha, Color.DimGray);
                SolidBrush brush;
                brush = new SolidBrush(cl);
                g.FillRectangle(brush, DVLpanel.ClientSize.Width - 60, 0, 60, DVLpanel.ClientSize.Height);
                cl = Color.FromArgb(alpha, Color.Cyan);
                Pen p;
                p = new Pen(cl);
                brush = new SolidBrush(cl);
                g.DrawLine(p, DVLpanel.ClientSize.Width - 30, 0, DVLpanel.ClientSize.Width - 30, DVLpanel.ClientSize.Height);
                for (int i = 1; i <= 5; i++)
                {
                    g.DrawLine(p, DVLpanel.ClientSize.Width - 50, DVLpanel.ClientSize.Height * i / 5, DVLpanel.ClientSize.Width - 10, DVLpanel.ClientSize.Height * i / 5);
                    g.DrawLine(p, DVLpanel.ClientSize.Width - 40, DVLpanel.ClientSize.Height * (2 * i - 1) / 10, DVLpanel.ClientSize.Width - 20, DVLpanel.ClientSize.Height * (2 * i - 1) / 10);

                    using (Font font = new Font("Arial", 8))
                    {
                        g.DrawString((MaxDepth / 5 * i).ToString(), font, brush, DVLpanel.ClientSize.Width - 60, (float)DVLpanel.ClientSize.Height * i / 5 - (float)font.Height);
                        g.DrawString((MaxDepth / 5 * i - MaxDepth/10).ToString(), font, brush, DVLpanel.ClientSize.Width - 60, (float)DVLpanel.ClientSize.Height * (2 * i - 1) / 10 - (float)font.Height);
                    }
                    for (int j = 1; j <= 4; j++)
                    {
                        g.DrawLine(p, DVLpanel.ClientSize.Width - 35, DVLpanel.ClientSize.Height * (10 * (i - 1) + j) / 50, DVLpanel.ClientSize.Width - 25, DVLpanel.ClientSize.Height * (10 * (i - 1) + j) / 50);
                        g.DrawLine(p, DVLpanel.ClientSize.Width - 35, DVLpanel.ClientSize.Height * (10 * (i - 1) + j + 5) / 50, DVLpanel.ClientSize.Width - 25, DVLpanel.ClientSize.Height * (10 * (i - 1) + j + 5) / 50);
                    }
                }
                cl = Color.FromArgb(alpha, Color.DeepPink);
                p = new Pen(cl);
                float y;
                y = (float)DVLpanel.ClientSize.Height / MaxDepth * BoatHeight;
                if (y > DVLpanel.ClientSize.Height-1)
                {
                    y = DVLpanel.ClientSize.Height-1;
                }
                g.DrawLine(p, DVLpanel.ClientSize.Width - 60, y - 1, DVLpanel.ClientSize.Width, y - 1);
                cl = Color.FromArgb(alpha, Color.Red);
                p = new Pen(cl);
                g.DrawLine(p, DVLpanel.ClientSize.Width - 60, y, DVLpanel.ClientSize.Width, y);
                cl = Color.FromArgb(alpha, Color.DarkRed);
                p = new Pen(cl);
                g.DrawLine(p, DVLpanel.ClientSize.Width - 60, y + 1, DVLpanel.ClientSize.Width, y + 1);
                //
                //
                //

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;   // 反锯齿
                g.SmoothingMode = SmoothingMode.HighQuality; //高质量
                g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量

                g.DrawImage(Compass, 10, 17);

                using (Pen GreenPen = new Pen(Color.LimeGreen, 1))
                {
                    using (Font font = new Font("Arial", 8))//画中心经纬度及距离标值所用字体
                    {
                        //鼠标在边缘或手动移动时，画面水平移动距离除以单位刻度的商,代表的是画面流过的线条个数
                        int nX = (int)(DragLengthX) / (int)CurrentDisplayUnit;
                        //鼠标在边缘或手动移动时，画面水平移动距离除以单位刻度的余数，代表的是尚未产生最新线条时已经流过的距离
                        int lX = (int)(DragLengthX) % (int)CurrentDisplayUnit;
                        //鼠标在边缘或手动移动时，画面竖直移动距离除以单位刻度的商
                        int nY = (int)(DragLengthY) / (int)CurrentDisplayUnit;
                        //鼠标在边缘或手动移动时，画面竖直移动距离除以单位刻度的余数
                        int lY = (int)(DragLengthY) % (int)CurrentDisplayUnit;


                        bool DrawOrignX = false;
                        float OrignX = 0;
                        for (int i = 0; i <= 8; i++)//画竖线
                        {
                            //由顶端到低端，依次向下画横线，如果检测到画线的位置已经超出了显示区下端，则结束本次画面绘制
                            if (lX + CurrentDisplayUnit * i > CurrentDisplayWidth)
                            {
                                break;
                            }

                            float NumX = 50 * (i - 4 - nX) * MouseWheelScale;
                            string NumXStr = NumX.ToString();
                            SizeF NumXSize = g.MeasureString(NumXStr, font);
                            float NumXLength = NumXSize.Width;//字符串长度（像素）
                            //标定横坐标
                            g.DrawString(NumXStr, font, Brushes.LightGray,
                                CurrentDisplayLeft + lX + CurrentDisplayUnit * i - NumXLength / 2,
                                CurrentDisplayTop);

                            if (i == nX + 4)//纵轴线:nX==0,即第四根线；nX==1，即第五根线，... ...
                            {
                                //标出经度值
                                DrawOrignX = true;
                                OrignX = CurrentDisplayLeft + lX + CurrentDisplayUnit * i;
                                g.DrawString(Xpzn.ToString() + 'E', font, Brushes.SkyBlue,
                                    OrignX, CurrentDisplayTop + CurrentDisplayHeight - font.Height);

                                GreenPen.DashStyle = DashStyle.Solid;//实线
                            }
                            else
                            {
                                GreenPen.DashStyle = DashStyle.Dot;//点线形式
                            }

                            //画竖线
                            g.DrawLine(GreenPen,
                                CurrentDisplayLeft + lX + CurrentDisplayUnit * i, CurrentDisplayTop,
                                CurrentDisplayLeft + lX + CurrentDisplayUnit * i, CurrentDisplayTop + CurrentDisplayHeight);
                        }

                        //绘图区中心线两侧所显示的的线条个数
                        int SideHNum = (int)(CurrentDisplayHeight / CurrentDisplayUnit) / 2;
                        //使初始绘图区中心始终为纵横两条线的交点
                        int AllHNum = 2 * SideHNum + 1;
                        //上侧最后一条横线到绘图区顶端之间的距离
                        float SmallHeight = CurrentDisplayTop + CurrentDisplayHeight / 2 - SideHNum * CurrentDisplayUnit;


                        bool DrawOrignY = false;
                        float OrignY = 0;
                        //画横线，使初始绘图区中心始终为纵横两条线的交点
                        for (int j = 0; j <= AllHNum; j++)
                        {
                            //由顶端到低端，依次向下画横线，如果检测到画线的位置已经超出了显示区下端，则结束本次画面绘制
                            if (lY + SmallHeight + CurrentDisplayUnit * j > CurrentDisplayHeight)
                            {
                                break;
                            }

                            float NumY = 50 * (j - SideHNum - nY) * MouseWheelScale;
                            string NumYStr = NumY.ToString();
                            SizeF NumYSize = g.MeasureString(NumYStr, font);
                            float NumYHeight = NumYSize.Height;
                            //标定纵坐标
                            g.DrawString(NumYStr, font, Brushes.LightGray,
                                CurrentDisplayLeft, CurrentDisplayTop + lY + SmallHeight + CurrentDisplayUnit * j - NumYHeight / 2);

                            if (j == nY + SideHNum)//横轴线
                            {
                                //画纬度值:
                                SizeF size = g.MeasureString(Ypzn.ToString() + 'N', font);
                                float length = size.Width;
                                DrawOrignY = true;
                                OrignY = CurrentDisplayTop + lY + SmallHeight + CurrentDisplayUnit * j;
                                g.DrawString(Ypzn.ToString() + 'N', font, Brushes.SkyBlue,
                                    CurrentDisplayLeft + CurrentDisplayWidth - length, OrignY);

                                GreenPen.DashStyle = DashStyle.Solid;//实线
                            }
                            else
                            {
                                GreenPen.DashStyle = DashStyle.Dot;//点线形式
                            }

                            //画横线
                            g.DrawLine(GreenPen,
                                    CurrentDisplayLeft,
                                    CurrentDisplayTop + lY + SmallHeight + CurrentDisplayUnit * j,
                                    CurrentDisplayLeft + CurrentDisplayWidth,
                                    CurrentDisplayTop + SmallHeight + lY + CurrentDisplayUnit * j);
                        }
                        if (DrawOrignX && DrawOrignY)
                        {
                            g.FillEllipse(Brushes.GreenYellow, OrignX - 3f, OrignY - 3f, 6f, 6f);
                        }

                        //由于每次都是从（CurrentDisplayTop+SmallHeight）处开始往下绘制画横线，
                        //故要补充绘制（CurrentDisplayTop+SmallHeight）以上的部分：
                        if (lY + SmallHeight >= CurrentDisplayUnit)
                        {
                            //准备画的线是标定为0的那根线向上数第(SideHNum + (nY+1))根
                            float NumY = 50 * (0 - SideHNum - nY - 1) * MouseWheelScale;
                            string NumYStr = NumY.ToString();
                            SizeF NumYSize = g.MeasureString(NumYStr, font);
                            float NumYHeight = NumYSize.Height;
                            //标定纵坐标
                            g.DrawString(NumYStr, font, Brushes.LightGray,
                                CurrentDisplayLeft,
                                CurrentDisplayTop + lY + SmallHeight - CurrentDisplayUnit - NumYHeight / 2);
                            //画横线
                            g.DrawLine(GreenPen,
                                    CurrentDisplayLeft,
                                    CurrentDisplayTop + lY + SmallHeight - CurrentDisplayUnit,
                                    CurrentDisplayLeft + CurrentDisplayWidth,
                                    CurrentDisplayTop + lY + SmallHeight - CurrentDisplayUnit);
                        }
                    }
                }

                Matrix matrix = new Matrix();//定义一个做几何变换的对象
                matrix.Translate((float)CurrentDisplayWidth / 2 + DragLengthX, (float)CurrentDisplayHeight / 2 + DragLengthY);//缩放中心设为绘图区中心
                matrix.Scale(multiple / MouseWheelScale, multiple / MouseWheelScale);
                g.Transform = matrix;

                PointF MapBoatPzn = TansToMapPoint(BoatPosition);

                PointF P = new PointF(MapBoatPzn.X - BoatBitmap.Width - 20 * (MouseWheelScale / multiple - 1), MapBoatPzn.Y - BoatBitmap.Height - 20 * (MouseWheelScale / multiple - 1));
                SizeF RecSize = new SizeF(40 / (multiple / MouseWheelScale), 40 / (multiple / MouseWheelScale));
                RectangleF rec = new RectangleF(P, RecSize);
                g.DrawImage(RotatedBoatBitmap, rec);//DVL船大小不随缩放率改变 

                this.BoatPositionStatus.Text = BoatPosition.X.ToString() + "," + BoatPosition.Y.ToString() + "," + BoatHeight.ToString();

                if (Infoclass.PointArray.Count != 0)
                {
                    //PointF PtStart = TansToMapPoint((PointF)(((BoatCoordinate)Infoclass.PointArray[0]).BoatPosition));
                    PointF PtStart = new PointF(0, 0);
                    int i = 0;
                    foreach (BoatCoordinate bc in Infoclass.PointArray)
                    {
                        if(i > LinesPosition) break;
                        //if (TansToMapPoint((PointF)(bc.BoatPosition))!= PtStart)
                        //{
                        //    g.DrawLine(Pens.DimGray, PtStart, TansToMapPoint((PointF)(bc.BoatPosition)));
                        //}
                        g.DrawLine(Pens.DimGray, PtStart, TansToMapPoint((PointF)(bc.BoatPosition)));
                        PtStart = TansToMapPoint((PointF)(bc.BoatPosition));
                        i++;
                    }
                }

                // 自定义缓冲中的图形渲染在屏幕上
                MainBuffer.Render(e.Graphics);
                MainBuffer.Dispose();
            }
        }

        private float HeightDisplayeX = 0;
        private void HeightDisplayerPanel_Paint(object sender, PaintEventArgs e)
        {
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;//获取当前绘图主缓冲区上下文
            BufferedGraphics SecondaryBuffer = currentContext.Allocate(e.Graphics, HeightDisplayerPanel.DisplayRectangle);

            using (Graphics sg = SecondaryBuffer.Graphics)
            {
                sg.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;   // 反锯齿
                sg.SmoothingMode = SmoothingMode.HighQuality; //高质量
                sg.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
                Matrix matrix = new Matrix();//定义一个做几何变换的对象
                matrix.Scale((float)HeightDisplayerPanel.ClientSize.Width / (float)HeightDisplayerPanel.Width, (float)HeightDisplayerPanel.ClientSize.Height / MaxDepth);
                if (Infoclass.HeightDisplayerPointArray.Count > 0)
                {
                    if (HeightDisplayeX > HeightDisplayerPanel.Width)
                    {
                        matrix.Translate(HeightDisplayerPanel.Width - HeightDisplayeX, 0);
                    }
                    sg.Transform = matrix;
                    PointF[] Points = new PointF[LinesPosition + 1];
                    Points[0] = new PointF(0, 0);
                    if (LinesPosition + 1 > 1)
                    {
                        int j = 0;
                        foreach (PointF pt in Infoclass.HeightDisplayerPointArray)
                        {
                            if (j > LinesPosition) break;
                            Points[j] = pt ;
                            if (Points[j].Y > MaxDepth - 1)
                            {
                                Points[j].Y = MaxDepth - 1;
                            }
                            j++;
                        }
                        sg.DrawCurve(Pens.Cyan, Points);
                    }
                    //sg.FillEllipse(Brushes.Blue, ((PointF)Infoclass.HeightDisplayerPointArray[LinesPosition]).X - 3, ((PointF)Infoclass.HeightDisplayerPointArray[LinesPosition]).Y - 3, 6, 6);
                    //sg.FillEllipse(Brushes.LawnGreen, Points[LinesPosition].X - 6, Points[LinesPosition].Y - 6, 12, 12);
                }

                SecondaryBuffer.Render(e.Graphics);
                SecondaryBuffer.Dispose();
            }
        }

        delegate void DisplayDelegate(string s); //定义一个委托
        DisplayDelegate Displayer;//声明这个委托，用以执行航行线程（时钟线程）
        private ArrayList EnsembleArray = new ArrayList();
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = sp.ReadLine();
            //if ((string.Compare(data.Substring(0, 5), "START", true) != 0) && (string.Compare(data.Substring(0, 4), "STOP",true) != 0))
            this.BeginInvoke(Displayer, data);
        }

        float getdata(string data, int num)
        {
            int iLength = data.Length;
            int start = 0;
            int end = 0;
            int i = 0;
            int j = 0;

            for ( ; i < iLength; i++)
            {
                if (data[i] == ',')
                {
                    j++;//记录遍历过程中遇到的逗号的个数
                    if (num == j)
                    {
                        start = i + 1;
                    }
                    else if (num + 1 == j)
                    {
                        end = i;
                        break;
                    }
                }
            }
            string str = data.Substring(start, end - start);
            if (str == "")
            {
                return 0;
            }
            else
            {
                return (float.Parse(str));
            }
        }

        double D_getdata(string data, int num)
        {
            int iLength = data.Length;
            int start = 0;
            int end = 0;
            int i = 0;
            int j = 0;

            for (; i < iLength; i++)
            {
                if (data[i] == ',')
                {
                    j++;//记录遍历过程中遇到的逗号的个数
                    if (num == j)
                    {
                        start = i + 1;
                    }
                    else if (num + 1 == j)
                    {
                        end = i;
                        break;
                    }
                }
            }
            string str = data.Substring(start, end - start);
            if (str == "")
            {
                return 0;
            }
            else
            {
                return (double.Parse(str));
            }
        }

        private PointF Sta = new PointF(0, 0);
        private float BoatHeight;
        private struct BoatCoordinate 
        {
            public PointF BoatPosition;
            public float BoatHeight;
        };
        private int n = 0;//记录已经写入的文件数
        private class InfoClass
        {
            public ArrayList PointArray = new ArrayList();//用以存储测线的关键点
            public ArrayList HeightDisplayerPointArray = new ArrayList();
            public ArrayList TimeArray = new ArrayList();
            public ArrayList VelArray = new ArrayList();
            
        }

        private InfoClass Infoclass = new InfoClass();
        private double pretime = 0;
        private int LinesNumPerFile = 100;
        float preWaterVx = 0f; float preWaterVy = 0f; float preWaterVz = 0f;
        float preBoatVx = 0f; float preBoatVy = 0f; float preBoatVz = 0f;
        bool firstReceive = true;
        int DataNum = 0;
        private void SetPosion(string data)
        {
            if (firstReceive)
            {
                DataDisPlayrichText.Clear();
                firstReceive = false;
            }
            if (2 == DataNum)
            {
                DataDisPlayrichText.Clear();
                DataNum = 0;
            }
            DataDisPlayrichText.AppendText(data + "\r\n");
            double time;
            float Vx;
            float Vy;
            float Vz;
            if (data.Length > 7)
                if (data.Substring(0, 7) == "$PRTI01")
                {
                    EnsembleArray.Add(data);
                    labelTime.Text = (D_getdata(data, 1)/100).ToString();
                    labelSampleNum.Text = (getdata(data, 2)).ToString();
                    labelTemperature.Text = (getdata(data, 3)/100).ToString();
                    labelDepthTrans.Text = (getdata(data, 7)).ToString();
                    labelDepthWater.Text = (getdata(data, 11)).ToString();
                    labelBVxIns.Text = (getdata(data, 4)).ToString();
                    labelBVyIns.Text = (getdata(data, 5)).ToString();
                    labelBVzIns.Text = (getdata(data, 6)).ToString();
                    labelBspeedIns.Text =
                        (Math.Sqrt(Math.Pow(getdata(data, 4), 2) +
                        Math.Pow(getdata(data, 5), 2) + Math.Pow(getdata(data, 6), 2))).ToString();
                    labelWVxIns.Text = (getdata(data, 8)).ToString();
                    labelWVyIns.Text = (getdata(data, 9)).ToString();
                    labelWVzIns.Text = (getdata(data, 10)).ToString();
                    labelWspeedIns.Text =
                        (Math.Sqrt(Math.Pow(getdata(data, 8), 2) +
                        Math.Pow(getdata(data, 9), 2) + Math.Pow(getdata(data, 10), 2))).ToString();
                }
                else if (data.Substring(0, 7) == "$PRTI02")
                {
                    DataNum = 2;
                    EnsembleArray.Add(data);
                    labelBVxEth.Text = (getdata(data, 4)).ToString();
                    labelBVyEth.Text = (getdata(data, 5)).ToString();
                    labelBVzEth.Text = (getdata(data, 6)).ToString();
                    labelBspeedEth.Text =
                        (Math.Sqrt(Math.Pow(getdata(data, 4), 2) +
                        Math.Pow(getdata(data, 5), 2) + Math.Pow(getdata(data, 6), 2))).ToString();
                    labelWVxEth.Text = (getdata(data, 8)).ToString();
                    labelWVyEth.Text = (getdata(data, 9)).ToString();
                    labelWVzEth.Text = (getdata(data, 10)).ToString();
                    labelWspeedEth.Text =
                        (Math.Sqrt(Math.Pow(getdata(data, 8), 2) +
                        Math.Pow(getdata(data, 9), 2) + Math.Pow(getdata(data, 10), 2))).ToString();
                    time = D_getdata(data, 1);
                    Vx = getdata(data, 4);
                    Vy = getdata(data, 5);
                    Vz = getdata(data, 6);
                    if (-99999 == Vx)//测船速无效时。。
                    {
                        if (-99999 == getdata(data, 8))//水跟踪无效时。。
                        {
                            Vx = preBoatVx;
                        }
                        else//水跟踪有效时。。。
                        {
                            Vx = preWaterVx + getdata(data, 8);
                        }
                    }
                    else//船速有效
                    {
                        if (-99999 != getdata(data, 8))//水跟踪有效
                        {
                            preWaterVx = Vx - getdata(data, 8);
                        }
                        preBoatVy = Vx;
                    }
                    if (-99999 == Vy)
                    {
                        if (-99999 == getdata(data, 9))//水跟踪无效时。。
                        {
                            Vy = preBoatVy;
                        }
                        else//水跟踪有效时。。。
                        {
                            Vy = preWaterVy + getdata(data, 9);
                        }
                    }
                    else
                    {
                        if (-99999 != getdata(data, 9))//水跟踪有效
                        {
                            preWaterVy = Vy - getdata(data, 9);
                        }
                        preBoatVy = Vy;
                    }
                    if (-99999 == Vz)
                    {
                        if (-99999 == getdata(data, 10))//水跟踪无效时。。
                        {
                            Vz = preBoatVz;
                        }
                        else//水跟踪有效时。。。
                        {
                            Vz = preWaterVz + getdata(data, 10);
                        }
                    }
                    else
                    {
                        if (-99999 != getdata(data, 10))//水跟踪有效
                        {
                            preWaterVz = Vz - getdata(data, 10);
                        }
                        preBoatVz = Vz;
                    }

                    //if (EnsembleArray.Count > 0)
                    //{
                    //    DataDisPlayrichText.Text = (string)EnsembleArray[EnsembleArray.Count - 1];
                    //}
                    BoatPosition.X += ((float)(time - pretime) / 100) * Vx/1000;
                    BoatPosition.Y += ((float)(time - pretime) / 100) * Vy/1000;
                    BoatHeight += ((float)(time - pretime) / 100) * Vz/1000;
                    BoatCoordinate bc;
                    bc.BoatPosition = BoatPosition;
                    bc.BoatHeight = BoatHeight;
                    Infoclass.PointArray.Add(bc);
                    Infoclass.TimeArray.Add(time);

                    HeightDisplayeX = 0;
                    if (Infoclass.PointArray.Count > 0)
                    {
                        for (int i = 1; i < Infoclass.PointArray.Count; i++)
                        {
                            HeightDisplayeX += (float)Math.Sqrt(
                                Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[i]).BoatPosition).X -
                                          TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[i - 1]).BoatPosition).X, 2) +
                                Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[i]).BoatPosition).Y -
                                         TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[i - 1]).BoatPosition).Y, 2));
                        }
                    }
                    float y = ((BoatCoordinate)Infoclass.PointArray[Infoclass.PointArray.Count - 1]).BoatHeight;
                    PointF h = new PointF(HeightDisplayeX, y);
                    Infoclass.HeightDisplayerPointArray.Add(h);

                    LinesPosition = Infoclass.PointArray.Count - 1;

                    if (AutoScalecheckBox.Checked == true)
                    {
                        if (TragCursorChangeToTrag == false)
                        {
                            Point p = new Point();
                            p.X = (int)(BoatPosition.X * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + ((float)CurrentDisplayWidth / 2 + DragLengthX));
                            p.Y = (int)(BoatPosition.Y * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + ((float)CurrentDisplayHeight / 2 + DragLengthY));
                            Rectangle r = new Rectangle(DVLpanel.Left, DVLpanel.Top, DVLpanel.Width, DVLpanel.Height);
                            if (!r.Contains(p))
                            {
                                DragLengthX = 0;
                                DragLengthY = 0;
                                p.X = (int)(BoatPosition.X * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + (float)CurrentDisplayWidth / 2);
                                p.Y = (int)(BoatPosition.Y * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + (float)CurrentDisplayHeight / 2);
                                if (!r.Contains(p))
                                {
                                    if (MouseWheelScale < 5)
                                    {
                                        MouseWheelScale++;
                                    }
                                    else
                                    {
                                        MouseWheelScale += 2;
                                    }
                                }
                            }
                        }

                    }

                    float RotationAngle;
                    PointF End = BoatPosition;
                    if (Sta != End)
                    {
                        RotationAngle = AngleOf2Points(Sta, End);
                        RotatedBoatBitmap = (Bitmap)RotateImage(BoatBitmap, (RotationAngle + 22.5F));
                    }
                    Sta = End;
                    if (EnsembleArray.Count >= LinesNumPerFile)
                    {
                        n++;
                        DateTime dt = DateTime.Now;
                        //string datePatt = @"yyyy.M.d hh-mm-ss tt";
                        //string FileName = ProjectName + "." + dt.ToString(datePatt) + "Part" + n.ToString() + ".dvl";
                        string FileName = "Part" + n.ToString() + ".dvl";
                        string Path = Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + FileName;
                        for (int i = 0; i < LinesNumPerFile; i++)
                        {
                            String str = (String)EnsembleArray[i];
                            File.AppendAllText(Path, str + "\r\n");
                        }
                        EnsembleArray.RemoveRange(0, LinesNumPerFile);
                    }

                    DVLpanel.Refresh();
                    HeightDisplayerPanel.Refresh();
                    pretime = time;
                }
        }

        private void getCurrentLine(int LinesPosition, ref string s1, ref string s2)//从0开始的索引
        {
            int fileNo = LinesPosition / (LinesNumPerFile / 2) + 1;
            int sampleNo = (LinesPosition + 1) % (LinesNumPerFile / 2);
            string SearchSting = "Part" + fileNo.ToString() + ".dvl";
            FileInfo[] Fn = Dinfo.GetFiles(SearchSting);//搜索包含指定标志的文件

            using (StreamReader streamread = new StreamReader(Fn[0].Open(FileMode.Open)))
            {
                for (int i = 1; i <= sampleNo; i++)
                {
                    s1 = streamread.ReadLine();//$PT01
                    streamread.ReadLine();
                    s2 = streamread.ReadLine();//$PT02
                    streamread.ReadLine();
                }
            }
        }

        private void PB_SetPosion()
        {
            if (LinesPosition < stringArray.Count && LinesPosition >= 0 && stringArray.Count > 0)
                DataDisPlayrichText.Text = (string)stringArray[LinesPosition];
            BoatPosition.X = ((BoatCoordinate)Infoclass.PointArray[LinesPosition]).BoatPosition.X;
            BoatPosition.Y = ((BoatCoordinate)Infoclass.PointArray[LinesPosition]).BoatPosition.Y;
            BoatHeight = ((BoatCoordinate)Infoclass.PointArray[LinesPosition]).BoatHeight;
            HeightDisplayeX = 0;
            if (Infoclass.PointArray.Count > 0)
            {
                for (int k = 1; k <= LinesPosition; k++)
                {
                    HeightDisplayeX += (float)Math.Sqrt(
                        Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).X -
                                  TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).X, 2) +
                        Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).Y -
                                 TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).Y, 2));
                }
            }
            if (AutoScalecheckBox.Checked == true)
            {
                if (TragCursorChangeToTrag == false)
                {
                    Point p = new Point();
                    p.X = (int)(BoatPosition.X * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + ((float)CurrentDisplayWidth / 2 + DragLengthX));
                    p.Y = (int)(BoatPosition.Y * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + ((float)CurrentDisplayHeight / 2 + DragLengthY));
                    Rectangle r = new Rectangle(DVLpanel.Left, DVLpanel.Top, DVLpanel.Width, DVLpanel.Height);
                    if (!r.Contains(p))//自动调整显示区域大小
                    {
                        DragLengthX = 0;
                        DragLengthY = 0;
                        p.X = (int)(BoatPosition.X * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + (float)CurrentDisplayWidth / 2);
                        p.Y = (int)(BoatPosition.Y * (((float)CurrentDisplayWidth / 8) / 50) / MouseWheelScale + (float)CurrentDisplayHeight / 2);
                        if (!r.Contains(p))
                        {
                            if (MouseWheelScale < 5)
                            {
                                MouseWheelScale++;
                            }
                            else
                            {
                                MouseWheelScale += 2;
                            }
                        }
                    }
                }
            }
            
            float vx = ((float[])Infoclass.VelArray[LinesPosition])[0];
            float vy = ((float[])Infoclass.VelArray[LinesPosition])[1];
            float vz = ((float[])Infoclass.VelArray[LinesPosition])[2];
            
            string data1 = string.Empty;
            string data2 = string.Empty;
            getCurrentLine(LinesPosition, ref data1, ref data2);
            labelTime.Text = (D_getdata(data1, 1) / 100).ToString();
            labelSampleNum.Text = (getdata(data1, 2)).ToString();
            labelTemperature.Text = (getdata(data1, 3) / 100).ToString();
            labelDepthTrans.Text = (getdata(data1, 7)).ToString();
            labelDepthWater.Text = (getdata(data1, 11)).ToString();
            labelBVxIns.Text = (getdata(data1, 4)).ToString();
            labelBVyIns.Text = (getdata(data1, 5)).ToString();
            labelBVzIns.Text = (getdata(data1, 6)).ToString();
            labelBspeedIns.Text =
                (Math.Sqrt(Math.Pow(getdata(data1, 4), 2) +
                Math.Pow(getdata(data1, 5), 2) + Math.Pow(getdata(data1, 6), 2))).ToString();
            labelWVxIns.Text = (getdata(data1, 8)).ToString();
            labelWVyIns.Text = (getdata(data1, 9)).ToString();
            labelWVzIns.Text = (getdata(data1, 10)).ToString();
            labelWspeedIns.Text =
                (Math.Sqrt(Math.Pow(getdata(data1, 8), 2) +
                Math.Pow(getdata(data1, 9), 2) + Math.Pow(getdata(data1, 10), 2))).ToString();

            labelBVxEth.Text = (getdata(data2, 4)).ToString();
            labelBVyEth.Text = (getdata(data2, 5)).ToString();
            labelBVzEth.Text = (getdata(data2, 6)).ToString();
            labelBspeedEth.Text =
                (Math.Sqrt(Math.Pow(getdata(data2, 4), 2) +
                Math.Pow(getdata(data2, 5), 2) + Math.Pow(getdata(data2, 6), 2))).ToString();
            labelWVxEth.Text = (getdata(data2, 8)).ToString();
            labelWVyEth.Text = (getdata(data2, 9)).ToString();
            labelWVzEth.Text = (getdata(data2, 10)).ToString();
            labelWspeedEth.Text =
                (Math.Sqrt(Math.Pow(getdata(data2, 8), 2) +
                Math.Pow(getdata(data2, 9), 2) + Math.Pow(getdata(data2, 10), 2))).ToString();

            float RotationAngle;
            PointF End = BoatPosition;
            if (Sta != End)
            {
                RotationAngle = AngleOf2Points(Sta, End);
                RotatedBoatBitmap = (Bitmap)RotateImage(BoatBitmap, (RotationAngle + 22.5F));
            }
            Sta = End;


            TimeLable.Text = NumToTime(timeKnot) + "/" + DisPlayTimeLenth;
            DVLpanel.Refresh();
            HeightDisplayerPanel.Refresh();
            ProcessBar.Refresh();
        }

        private PointF TansToMapPoint(PointF pt)
        {
            PointF RealStartPointToStore = new PointF();
            RealStartPointToStore.X = ( pt.X * ( (float)MaxDisplayWidth / 8 ) / 50 );
            RealStartPointToStore.Y = ( pt.Y * ( (float)MaxDisplayWidth / 8 ) / 50 );
            return (RealStartPointToStore);
        }

        //计算两点连线与向上的竖直线（y轴负向）的顺时针夹角（由于初始Boat图形一边是沿着纵向的，且旋转函数是顺时针方向的）
        private float AngleOf2Points(PointF Sta, PointF End)
        {
            float Angle = 0;
            if (End.X >= Sta.X)
            {
                Angle = 180 - (float)((180 / Math.PI) * Math.Acos((End.Y - Sta.Y) / Math.Sqrt(Math.Pow((End.X - Sta.X), 2) + Math.Pow((End.Y - Sta.Y), 2))));
            }
            else
            {
                Angle = 180 + (float)((180 / Math.PI) * Math.Acos((End.Y - Sta.Y) / Math.Sqrt(Math.Pow((End.X - Sta.X), 2) + Math.Pow((End.Y - Sta.Y), 2))));
            }

            return Angle;
        }

        public static Image RotateImage(Image img, float RotationAngle)//顺时针旋转
        {
            //创建一个空的位图图像
            Bitmap bmp = new Bitmap(2 * img.Width, 2 * img.Height);
            //转换为Graphics对象
            Graphics gfx = Graphics.FromImage(bmp);

            //Matrix matrix = new Matrix();
            //matrix.RotateAt(rotationAngle, new Point(img.Width, img.Height));//旋转
            //gfx.Transform = matrix;
            gfx.TranslateTransform(img.Width, img.Height);//gfx坐标系的原点定在gfx的中心点
            gfx.RotateTransform(RotationAngle);//gfx坐标系绕中心点旋转
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;//双三次插值法，得到高质量图像
            gfx.SmoothingMode = SmoothingMode.HighQuality; //高质量
            gfx.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
            //gfx.DrawImage(img, new Point(img.Width, img.Height));
            gfx.DrawImage(img, new Point(0, 0));//在gfx坐标系的原点画img图像

            gfx.Dispose();
            return bmp;
        }

        private PointF DragEndPoint;
        private PointF MousePosion;
        private bool LightFlag = false;
        private void DVLpanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (TragCursorChangeToTrag)
            {
                Cursor.Current = new Cursor(Application.StartupPath + "\\CursorTrag.cur");
                DragEndPoint = e.Location;
                DragLengthX += DragEndPoint.X - DragStartPoint.X;
                DragLengthY += DragEndPoint.Y - DragStartPoint.Y;
                DVLpanel.Refresh();
                DragStartPoint = e.Location;
            }
            else
            {
                Cursor.Current = new Cursor(Application.StartupPath + "\\CursorTouch.cur");
            }
            MousePosion = new PointF();
            MousePosion.X = (e.Location.X - ((float)CurrentDisplayWidth / 2 + DragLengthX)) * MouseWheelScale / (((float)CurrentDisplayWidth / 8) / 50);
            MousePosion.Y = (e.Location.Y - ((float)CurrentDisplayHeight / 2 + DragLengthY)) * MouseWheelScale / (((float)CurrentDisplayWidth / 8) / 50);
            this.MousePosionStatus.Text = MousePosion.X.ToString() + "," + MousePosion.Y.ToString();
            if (e.Location.X > DVLpanel.ClientSize.Width - 60)
            {
                LightFlag = true;
                DVLpanel.Refresh();
            }
            else
            {
                LightFlag = false;
                DVLpanel.Refresh();
            }
        }

        private bool TragCursorChangeToTrag = false;
        private PointF DragStartPoint;
        private void DVLpanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TragCursorChangeToTrag = true;
                Cursor.Current = new Cursor(Application.StartupPath + "\\CursorTrag.cur");
                DragStartPoint = e.Location;
            }
        }

        private void DVLpanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Current = new Cursor(Application.StartupPath + "\\CursorTouch.cur");
                TragCursorChangeToTrag = false;
            }
        }
        private void DVLpanel_Mousewheel(object sender, MouseEventArgs e)
        {
            PointF MousePositionToWindows = PointToClient(MousePosition);
            if (PanelEnterFlag)
            {
                if (e.Delta > 0)
                {
                    if (MouseWheelScale < 50)
                    {
                        if (MouseWheelScale < 5)
                        {
                            MouseWheelScale++;
                        }
                        else
                        {
                            MouseWheelScale += 2;//修正每次缩小倍数的增量，使看上去缩放比较均匀
                        }
                    }
                }
                else if (e.Delta < 0)
                {
                    if (MouseWheelScale > 1)
                    {
                        if (MouseWheelScale > 5)
                        {
                            MouseWheelScale -= 2;
                        }
                        else
                        {
                            MouseWheelScale--;//修正每次缩小倍数的增量，使看上去缩放比较均匀
                        }
                    }
                }
                DVLpanel.Refresh();
            }
        }

        private bool PanelEnterFlag = false;
        private void DVLpanel_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Current = new Cursor(Application.StartupPath + "\\CursorTouch.cur");
            this.DVLpanel.Focus();
            PanelEnterFlag = true;
        }

        private float MaxDepth = 500;
        String ProjectName = "";
        private bool OrgPznFlag = false;//表示初始位置是否已经设置
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!OrgPznFlag)
            {
                if (!IsNumber(textXPzn.Text) || !IsNumber(textYPzn.Text) || !IsNumber(WaterDepth.Text)
                    || String.IsNullOrEmpty(textXPzn.Text) || String.IsNullOrEmpty(textYPzn.Text) || String.IsNullOrEmpty(WaterDepth.Text))
                {
                    //MessageBox.Show("请输入有意义的数据！");
                    MessageBox.Show(Resource1.String86);
                }
                else
                {
                    Xpzn = float.Parse(textXPzn.Text);
                    Ypzn = float.Parse(textYPzn.Text);
                    MaxDepth = float.Parse(WaterDepth.Text);
                    textXPzn.ReadOnly = true;
                    textYPzn.ReadOnly = true;
                    FileName.ReadOnly = true;
                    WaterDepth.ReadOnly = true;

                    OrgPznFlag = true;

                    
                }
            }
            else
            {
                textXPzn.ReadOnly = false;
                textYPzn.ReadOnly = false;
                FileName.ReadOnly = false;
                WaterDepth.ReadOnly = false;
                OrgPznFlag = false;
            }
            //
            //
            //
            if (!sp.IsOpen)
            {
                if (OrgPznFlag)
                {
                    //2.用户配置好串口参数后，依据用户配置打开串口
                    sp.PortName = PortNameList.Text;
                    sp.BaudRate = int.Parse(BaudRateList.Text);
                    sp.DataBits = int.Parse(DataBitsList.Text);
                    sp.Parity = (Parity)Enum.Parse(typeof(Parity), ParityList.Text);
                    switch (StopBitsList.Text)
                    {
                        case "0":
                            {
                                sp.StopBits = StopBits.None;
                                break;
                            }
                        case "1":
                            {
                                sp.StopBits = StopBits.One;
                                break;
                            }
                        case "1.5":
                            {
                                sp.StopBits = StopBits.OnePointFive;
                                break;
                            }
                        case "2":
                            {
                                sp.StopBits = StopBits.Two;
                                break;
                            }
                    }
                    try
                    {
                        sp.Open();
                    }
                    catch (System.Exception ex)//如有异常，捕捉并显示
                    {
                        MessageBox.Show(ex.Message);
                        OrgPznFlag = sp.IsOpen;
                    }
                }
                
            }
            else
            {
                sp.Close();
                this.ComPortStatus.Text = "Not connected";
            }
            if (sp.IsOpen)
            {
                ProjectName = FileName.Text;
                if (ProjectName == "")
                {
                    //ProjectName = "(空名称)";
                    ProjectName = Resource1.String87;
                }
                DateTime dt = DateTime.Now;
                string datePatt = @"yyyy.M.d hh-mm-ss tt";
                ProjectName += dt.ToString(datePatt);
                string newPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "DVL NemaData", ProjectName);
                System.IO.Directory.CreateDirectory(newPath);

                string str = textXPzn.Text + "\r\n" + textYPzn.Text + "\r\n" + WaterDepth.Text + "\r\n";
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + ProjectName + ".infm", str);

                this.ComPortStatus.Text = (sp.PortName + "," + sp.BaudRate + "," + sp.DataBits.ToString() + "," + sp.Parity.ToString() + "," + sp.StopBits.ToString());

                this.Text = newPath;  //LPJ 2012-8-20 
            }
            buttonOK.Text = (sp.IsOpen && OrgPznFlag) ? "Cancel" : "OK";
            PortNameList.Enabled = !(sp.IsOpen && OrgPznFlag);
            BaudRateList.Enabled = !(sp.IsOpen && OrgPznFlag);
            DataBitsList.Enabled = !(sp.IsOpen && OrgPznFlag);
            ParityList.Enabled = !(sp.IsOpen && OrgPznFlag);
            StopBitsList.Enabled = !(sp.IsOpen && OrgPznFlag);

            DVLpanel.Refresh();
        }

        private bool IsNumber(String str)
        {
            int j = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (((str[i] < '0' || str[i] > '9' || str[i] == ' ') && str[i] != '.')
                    || str[0] == '.' || str[str.Length - 1] == '.')
                {
                    return false;
                }
                else
                {
                    if ('.' == str[i])
                    { j++; }
                }
            }
            if (j > 1)//多于两个‘.’
            {
                return false;
            }
            return true;
        }

        private void Startbutton_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
                sp.Write("START" + '\r');
        }

        private void Stopbutton_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
                sp.Write("STOP" + '\r');
        }

        private void buttonDVLmode_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
                sp.Write("CDVL" + '\r');
        }

        private void RealDisPlay_Click(object sender, EventArgs e)
        {
            if (playBackMode == true)
            {
                if (DialogResult.Yes ==
                    MessageBox.Show(Resource1.String88, "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    //if (DialogResult.Yes ==
                    //MessageBox.Show("是否停止当前回放，开始新的工程？", "Warning",
                    //MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    PlayBackTimer.Stop();
                    PlayBackTimer.Close();

                    playBackMode = false;
                    RealDisPlay.ForeColor = Color.Blue;
                    PalyBackBtn.ForeColor = Color.White;

                    InitialAllParam();
                    DVLpanel.Refresh();
                    HeightDisplayerPanel.Refresh();
                    ProcessBar.Refresh();
                }
            }
            else
            {
                if (sp.IsOpen == true)
                {
                    if (DialogResult.Yes ==
                    MessageBox.Show(Resource1.String89, "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        sp.Write("STOP" + "\r");
                        buttonOK_Click(null, null);
                        if (EnsembleArray.Count < LinesNumPerFile && EnsembleArray.Count > 0)
                        {
                            DateTime dt = DateTime.Now;
                            string FileName = "Part" + (n + 1).ToString() + ".dvl";
                            string Path = Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + FileName;
                            foreach (String str in EnsembleArray)
                            {
                                File.AppendAllText(Path, str + "\r\n");
                            }
                            this.Text = Path;  //LPJ 2012-8-20
                        }
                        string str1 = Infoclass.PointArray.Count.ToString();
                        File.AppendAllText(Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + ProjectName + ".infm", str1);
                       
                    }
                    InitialAllParam();
                }
            }
        }

        private string DisPlayTimeLenth = string.Empty;
        private bool playBackMode = false;
        private string PathStr;
        private DirectoryInfo Dinfo;
        private int FileNum = 0;
        static System.Timers.Timer PlayBackTimer;
        private ArrayList stringArray = new ArrayList();
        private void PalyBackBtn_Click(object sender, EventArgs e)
        {
            if (playBackMode == false)
            {
                bool AllowPlayBackFlag = false;
                if(sp.IsOpen == false)
                {
                    AllowPlayBackFlag = true;
                }
                else if (DialogResult.Yes ==
                    MessageBox.Show(Resource1.String90, "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    AllowPlayBackFlag = true;
                }
                if(AllowPlayBackFlag == true)
                {
                    if (sp.IsOpen == true)
                    {
                        sp.Write("STOP" + "\r");
                        buttonOK_Click(null, null);
                        if (EnsembleArray.Count < LinesNumPerFile && EnsembleArray.Count > 0)
                        {
                            DateTime dt = DateTime.Now;
                            //string datePatt = @"yyyy.M.d hh-mm-ss tt";
                            //string FileName = ProjectName + "." + dt.ToString(datePatt) + ".Part" + (n + 1).ToString() + ".dvl";
                            string FileName = "Part" + (n + 1).ToString() + ".dvl";
                            string Path = Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + FileName;
                            foreach (String str in EnsembleArray)
                            {
                                File.AppendAllText(Path, str + "\r\n");
                            }

                            this.Text = Path; //LPJ 2012-8-20
                        }
                        string str1 = Infoclass.PointArray.Count.ToString();
                        File.AppendAllText(Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + ProjectName + ".infm", str1);
                    }
                    InitialAllParam();
                    

                    folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;//创建选择文件夹窗口
                    if (PathStr != null) //LPJ 2013-1-15
                    {
                        folderBrowserDialog1.SelectedPath = PathStr;
                    }
                    else
                    {
                        folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory; //LPJ 2013-1-14
                    }

                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)//选取路径后。。。
                    {
                        //获取数据路径及信息文件名
                        PathStr = folderBrowserDialog1.SelectedPath;
                        int x = PathStr.LastIndexOf("\\");
                        string str = PathStr.Substring(x);

                        this.Text = PathStr; //LPJ 2012-8-20
                       
                        if (!File.Exists(PathStr + "\\" + str + ".infm"))//寻找回放信息文件，如果失败。。。
                        {
                            MessageBox.Show(Resource1.String91);
                        }
                        else//如果成功。。。
                        {
                            //改变显示模式（由实时转变为回放），改变按钮颜色
                            playBackMode = true;
                            PalyBackBtn.ForeColor = Color.Blue;
                            RealDisPlay.ForeColor = Color.White;

                            //读取信息文件的参数设置
                            Dinfo = new DirectoryInfo(PathStr);
                            FileNum = (Dinfo.GetFiles("*.dvl")).Length;
                            using(StreamReader sr = new StreamReader(PathStr + "\\" + str + ".infm"))
                            {
                                Xpzn = float.Parse(sr.ReadLine());
                                Ypzn = float.Parse(sr.ReadLine());
                                MaxDepth = float.Parse(sr.ReadLine());
                                LinesNumOfAllFiles = int.Parse(sr.ReadLine());
                            }
                            double pretime1 = 0;
                            bool IsEndOfStream = true;
                            StreamReader streamread = null;
                            for (int i = 1; i <= FileNum; )
                            {
                                if (IsEndOfStream == true)//如果已经读完一个文件，或者尚未开始读文件，则读下一个文件
                                {
                                    //string SearchSting = "*.Part" + i.ToString() + ".dvl";
                                    string SearchSting = "Part" + i.ToString() + ".dvl";
                                    FileInfo[] Fn = Dinfo.GetFiles(SearchSting);//搜索包含指定标志的文件
                                    if (Fn.Length == 0)//未找到下一个文件则报错
                                    {
                                        PlayBackTimer.Stop();
                                        MessageBox.Show(Resource1.String91);
                                        break;
                                    }
                                    else//如果找到下一个文件，则读取
                                    {
                                        streamread = new StreamReader(Fn[0].Open(FileMode.Open));
                                        IsEndOfStream = false;
                                    }
                                }

                                string strLine = streamread.ReadLine();
                                if (strLine == null)
                                {
                                    i++;
                                    IsEndOfStream = true;
                                }
                                else
                                {
                                    if (strLine != "")
                                    {
                                        if (strLine.Substring(0, 7) == "$PRTI02")
                                        {
                                            stringArray.Add(strLine);
                                            double time1 = D_getdata(strLine, 1);
                                            float Vx = getdata(strLine, 4);
                                            float Vy = getdata(strLine, 5);
                                            float Vz = getdata(strLine, 6);

                                            if (-99999 == Vx)//测船速无效时。。
                                            {
                                                if (-99999 == getdata(strLine, 8))//水跟踪无效时。。
                                                {
                                                    Vx = preBoatVx;
                                                }
                                                else//水跟踪有效时。。。
                                                {
                                                    Vx = preWaterVx + getdata(strLine, 8);
                                                }
                                            }
                                            else//船速有效
                                            {
                                                if (-99999 != getdata(strLine, 8))//水跟踪有效
                                                {
                                                    preWaterVx = Vx - getdata(strLine, 8);
                                                }
                                                preBoatVy = Vx;
                                            }
                                            if (-99999 == Vy)
                                            {
                                                if (-99999 == getdata(strLine, 9))//水跟踪无效时。。
                                                {
                                                    Vy = preBoatVy;
                                                }
                                                else//水跟踪有效时。。。
                                                {
                                                    Vy = preWaterVy + getdata(strLine, 9);
                                                }
                                            }
                                            else
                                            {
                                                if (-99999 != getdata(strLine, 9))//水跟踪有效
                                                {
                                                    preWaterVy = Vy - getdata(strLine, 9);
                                                }
                                                preBoatVy = Vy;
                                            }
                                            if (-99999 == Vz)
                                            {
                                                if (-99999 == getdata(strLine, 10))//水跟踪无效时。。
                                                {
                                                    Vz = preBoatVz;
                                                }
                                                else//水跟踪有效时。。。
                                                {
                                                    Vz = preWaterVz + getdata(strLine, 10);
                                                }
                                            }
                                            else
                                            {
                                                if (-99999 != getdata(strLine, 10))//水跟踪有效
                                                {
                                                    preWaterVz = Vz - getdata(strLine, 10);
                                                }
                                                preBoatVz = Vz;
                                            }

                                            BoatPosition.X += ((float)(time1 - pretime1) / 100) * Vx/1000;
                                            BoatPosition.Y += ((float)(time1 - pretime1) / 100) * Vy/1000;
                                            BoatHeight += ((float)(time1 - pretime1) / 100) * Vz/1000;

                                            BoatCoordinate bc;
                                            bc.BoatPosition = BoatPosition;
                                            bc.BoatHeight = BoatHeight;

                                            float[] Vel = { Vx, Vy, Vz };
                                            Infoclass.TimeArray.Add(time1);
                                            Infoclass.PointArray.Add(bc);
                                            Infoclass.VelArray.Add(Vel);

                                            float HeightDisplayeX = 0;
                                            if (Infoclass.PointArray.Count > 0)
                                            {
                                                for (int k = 1; k < Infoclass.PointArray.Count; k++)
                                                {
                                                    HeightDisplayeX += (float)Math.Sqrt(
                                                        Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).X -
                                                                  TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).X, 2) +
                                                        Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).Y -
                                                                 TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).Y, 2));
                                                }
                                            }
                                            float y = ((BoatCoordinate)Infoclass.PointArray[Infoclass.PointArray.Count - 1]).BoatHeight;
                                            PointF h = new PointF(HeightDisplayeX, y);
                                            Infoclass.HeightDisplayerPointArray.Add(h);

                                            pretime1 = time1;
                                        }
                                    }
                                }
                                /////////找到AllTextsNum != Infoclass.PointArray.Count的问题后可以删除
                                if (LinesNumOfAllFiles != Infoclass.PointArray.Count)
                                {
                                    LinesNumOfAllFiles = Infoclass.PointArray.Count;
                                }
                                /////////
                            }
                            streamread.Close();

                            TotleTimeToNow = (double)Infoclass.TimeArray[LinesPosition] * 10;
                            double DisPlayTime = pretime1;
                            //MessageBox.Show(XpznPlayBack.ToString() + " " + YpznPlayBack.ToString() + "" + MaxDepthPlayBack.ToString());

                            //创建并启动回放线程
                            DisPlayTimeLenth = NumToTime(DisPlayTime * 10);
 
                            PlayBackTimer = new System.Timers.Timer();
                            PlayBackTimer.Elapsed += new System.Timers.ElapsedEventHandler(PlayBackCenter);
                            PlayBackTimer.Interval = 50;
                            PlayBack = new PlayBackDelegate(PB_SetPosion);//委托指针指向 SetPosion 函数
                            ProcessBar.Refresh();
                            PlayBackTimer.Start();
                        }
                    }
                }
            }
        }

        private double TotleTimeToNow = 0;
        delegate void PlayBackDelegate(); //定义一个委托
        PlayBackDelegate PlayBack;
        double timeKnot = 0;
        int ForLinesPosition = 0;///////////解决LinesPosition > LinesNumOfAllFiles - 1的问题后可以删除:
        //原因，当ForLinesPosition = Infoclass.PointArray.Count-1时，LinesPosition++后溢出（此时等于Infoclass.PointArray.Count），
        //线程间交换时，该溢出值被用在另一个线程的其他地方（可能是这个原因）
        int LinesPosition = 0;//已经读过的数据的索引
        private void PlayBackCenter(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ForLinesPosition < Infoclass.PointArray.Count)
            {
                if (TotleTimeToNow >= timeKnot)
                {
                    LinesPosition = ForLinesPosition;
                    /////////找到LinesPosition > LinesNumOfAllFiles - 1的原因后可以删除
                    if (LinesPosition > LinesNumOfAllFiles - 1)
                    {
                        LinesPosition = LinesNumOfAllFiles - 1;
                    }
                    else if (LinesPosition < 0)
                    {
                        LinesPosition = 0;
                    }
                    /////////

                    timeKnot = (double)Infoclass.TimeArray[LinesPosition] * 10;
                    this.BeginInvoke(PlayBack);
                    ClickPzn = ProcessBar.Width * (LinesPosition+1) / LinesNumOfAllFiles;
                    ForLinesPosition++;
                }
                TotleTimeToNow += PlayBackTimer.Interval;
            }
        }

        private void InitialAllParam()
        {
            firstReceive = true;
            DataNum = 0;
            labelTime.Text = "0";
            labelSampleNum.Text = "0";
            labelTemperature.Text = "-";
            labelDepthTrans.Text = "0";
            labelDepthWater.Text = "0";
            labelBVxIns.Text = "0";
            labelBVyIns.Text = "0";
            labelBVzIns.Text = "0";
            labelBspeedIns.Text = "0";
            labelWVxIns.Text = "0";
            labelWVyIns.Text = "0";
            labelWVzIns.Text = "0";
            labelWspeedIns.Text = "0";
            labelBVxEth.Text = "0";
            labelBVyEth.Text = "0";
            labelBVzEth.Text = "0";
            labelBspeedEth.Text = "0";
            labelWVxEth.Text = "0";
            labelWVyEth.Text = "0";
            labelWVzEth.Text = "0";
            labelWspeedEth.Text = "0";
            preWaterVx = 0f; 
            preWaterVy = 0f; 
            preWaterVz = 0f;
            preBoatVx = 0f;
            preBoatVy = 0f;
            preBoatVz = 0f;
            RotatedBoatBitmap = (Bitmap)RotateImage(BoatBitmap, 22.5F);
            LinesPosition = 0;
            ForLinesPosition = 0;
            multiple = 1;//设置随窗口大小的改变绘图区缩放的倍数
            PreviousMultiple = 1;//上一次刷新后客户区所处于的倍数
            DragLengthX = 0;
            DragLengthY = 0;
            MouseWheelScale = 1;//滚轮缩小率的初始值
            Xpzn = 0;
            Ypzn = 0;//初始中心点经纬度
            Sta = new PointF(0, 0);
            BoatPosition = new PointF(0, 0);
            BoatHeight = 0;
            HeightDisplayeX = 0;
            n = 0;
            MaxDepth = 500;
            FileNum = 0;
            pretime = 0;
            TotleTimeToNow = 0;
            DisPlayTimeLenth = string.Empty;
            timeKnot = 0;
            ClickPzn = 1;
            Infoclass.PointArray.Clear();
            Infoclass.TimeArray.Clear();
            Infoclass.VelArray.Clear();
            Infoclass.HeightDisplayerPointArray.Clear();
            EnsembleArray.Clear();
            stringArray.Clear();
            DataDisPlayrichText.Text = "No data received... ...";
        }

        private bool DrawRecFlag = false;
        private void ProcessBar_Paint(object sender, PaintEventArgs e)
        {
            //if (playBackMode == true)
            //{
                using (Graphics g = e.Graphics)
                {
                    if (DrawRecFlag)
                    {
                        g.DrawRectangle(Pens.DimGray, 0, 0, ProcessBar.Width - 1, ProcessBar.Height - 1);
                        g.DrawLine(Pens.White, 0, ProcessBar.Height / 2, ProcessBar.Width - 1, ProcessBar.Height / 2);
                    }
                    else
                    {
                        g.DrawRectangle(Pens.Black, 0, 0, ProcessBar.Width - 1, ProcessBar.Height - 1);
                        g.DrawLine(Pens.DimGray, 0, ProcessBar.Height / 2, ProcessBar.Width - 1, ProcessBar.Height / 2);
                    }
                    g.DrawLine(Pens.LawnGreen, 0, ProcessBar.Height / 2, ClickPzn, ProcessBar.Height / 2);
                    g.FillRectangle(Brushes.LightSteelBlue, ClickPzn - 2, 4, 5, ProcessBar.Height - 8);
                    g.DrawLine(Pens.Red, ClickPzn, 4, ClickPzn, ProcessBar.Height - 5);
                }
            //}

        }

        private void ProcessBar_MouseEnter(object sender, EventArgs e)
        {
            if (playBackMode == true)
            {
                DrawRecFlag = true;
                ProcessBar.Refresh();
            }
        }

        private void ProcessBar_MouseLeave(object sender, EventArgs e)
        {
            if (playBackMode == true)
            {
                DrawRecFlag = false;
                ProcessBar.Refresh();
            }
        }

        private float ClickPzn = 1;
        private bool MouseDownFlag = false;
        private void ProcessBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (playBackMode == true)
            {
                MouseDownFlag = true;
                ClickPzn = e.X;
                if (ClickPzn <= 0)
                {
                    ClickPzn = 1;
                }
                else if (ClickPzn > ProcessBar.Width)
                {
                    ClickPzn = ProcessBar.Width;
                }
                LinesPosition = (int)(LinesNumOfAllFiles * ClickPzn / ProcessBar.Width) - 1;
                /////////找到LinesPosition > LinesNumOfAllFiles - 1的原因后可以删除
                if (LinesPosition > LinesNumOfAllFiles - 1)
                {
                    LinesPosition = LinesNumOfAllFiles - 1;
                }
                else if (LinesPosition < 0)
                {
                    LinesPosition = 0;
                }
                /////////
                ForLinesPosition = (int)(LinesNumOfAllFiles * ClickPzn / ProcessBar.Width) - 1;
                BoatPosition = ((BoatCoordinate)Infoclass.PointArray[LinesPosition]).BoatPosition;
                HeightDisplayeX = 0;
                PointF StartP = new PointF(0, 0);
                if (LinesPosition > 0)
                {
                    for (int k = 1; k <= LinesPosition; k++)
                    {
                        HeightDisplayeX += (float)Math.Sqrt(
                            Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).X -
                                      TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).X, 2) +
                            Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).Y -
                                     TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).Y, 2));
                    }
                    StartP = ((BoatCoordinate)Infoclass.PointArray[LinesPosition - 1]).BoatPosition;
                }
                if (StartP != BoatPosition)
                    RotatedBoatBitmap = (Bitmap)RotateImage(BoatBitmap, (AngleOf2Points(StartP, BoatPosition) + 22.5F));
                
                timeKnot = (double)Infoclass.TimeArray[LinesPosition] * 10;
                TotleTimeToNow = timeKnot;
                TimeLable.Text = NumToTime(timeKnot) + "/" + DisPlayTimeLenth;

                DVLpanel.Refresh();
                HeightDisplayerPanel.Refresh();
                ProcessBar.Refresh();

                PlayBackTimer.Stop();
            }
        }

        private void ProcessBar_MouseMove(object sender, MouseEventArgs e)
        {
            if(playBackMode == true)
            {
                if (MouseDownFlag == true)
                {
                    ClickPzn = e.X;
                    if (ClickPzn <= 0)
                    {
                        ClickPzn = 1;
                    }
                    else if (ClickPzn > ProcessBar.Width)
                    {
                        ClickPzn = ProcessBar.Width;
                    }
                    LinesPosition = (int)(LinesNumOfAllFiles * ClickPzn / ProcessBar.Width) - 1;
                    /////////找到LinesPosition > LinesNumOfAllFiles - 1的原因后可以删除
                    if (LinesPosition > LinesNumOfAllFiles - 1)
                    {
                        LinesPosition = LinesNumOfAllFiles - 1;
                    }
                    else if (LinesPosition < 0)
                    {
                        LinesPosition = 0;
                    }
                    /////////
                    ForLinesPosition = (int)(LinesNumOfAllFiles * ClickPzn / ProcessBar.Width) - 1;
                    BoatPosition = ((BoatCoordinate)Infoclass.PointArray[LinesPosition]).BoatPosition;
                    HeightDisplayeX = 0;
                    PointF StartP = new PointF(0, 0);
                    if (LinesPosition > 0)
                    {
                        for (int k = 1; k <= LinesPosition; k++)
                        {
                            HeightDisplayeX += (float)Math.Sqrt(
                                Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).X -
                                          TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).X, 2) +
                                Math.Pow(TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k]).BoatPosition).Y -
                                         TansToMapPoint(((BoatCoordinate)Infoclass.PointArray[k - 1]).BoatPosition).Y, 2));
                        }
                        StartP = ((BoatCoordinate)Infoclass.PointArray[LinesPosition - 1]).BoatPosition;
                    }

                    if (StartP != BoatPosition)
                        RotatedBoatBitmap = (Bitmap)RotateImage(BoatBitmap, (AngleOf2Points(StartP, BoatPosition) + 22.5F));
                    
                    timeKnot = (double)Infoclass.TimeArray[LinesPosition] * 10;
                    TotleTimeToNow = timeKnot;
                    TimeLable.Text = NumToTime(timeKnot) + "/" + DisPlayTimeLenth;

                    DVLpanel.Refresh();
                    HeightDisplayerPanel.Refresh();
                    ProcessBar.Refresh();
                }
            }
        }

        private void ProcessBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (playBackMode == true)
            {
                MouseDownFlag = false;
                PlayBackTimer.Start();
            }
        }

        private string NumToTime(double t)
        {
            int hour = 0;
            int minute = 0;
            int second = 0;
            if(t >= 3600000)
            {
                hour = (int)t / 3600000;
                t -= hour * 3600000;
            }
            if (t >= 60000)
            {
                minute = (int)t / 60000;
                t -= minute * 60000;
            }
            if (t >= 1000)
            {
                second = (int)t / 1000;
                t -= second * 1000;
            }
            string time = hour.ToString("00").PadLeft(2) + ":" + minute.ToString("00").PadLeft(2) + ":" + second.ToString("00").PadLeft(2);
            return time;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (MessageBox.Show(Resource1.String36, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (sp.IsOpen == true)
                {
                    sp.Close();
                    if (playBackMode == false)
                    {
                        if (EnsembleArray.Count < LinesNumPerFile && EnsembleArray.Count > 0)
                        {
                            DateTime dt = DateTime.Now;
                            string datePatt = @"yyyy.M.d hh-mm-ss tt";
                            string FileName = ProjectName + "." + dt.ToString(datePatt) + ".Part" + (n + 1).ToString() + ".dvl";
                            string Path = Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + FileName;
                            foreach (String str in EnsembleArray)
                            {
                                File.AppendAllText(Path, str + "\r\n");
                            }

                            this.Text = Path; //LPJ 2012-8-20
                        }

                        string str1 = Infoclass.PointArray.Count.ToString();
                        File.AppendAllText(Directory.GetCurrentDirectory() + "\\DVL NemaData\\" + ProjectName + "\\" + ProjectName + ".infm", str1);
                        base.OnFormClosing(e);
                    }
                }
            }
            else e.Cancel = true;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            DVLpanel.Refresh();
            HeightDisplayerPanel.Refresh();
            ProcessBar.Refresh();
        }
    }
}
