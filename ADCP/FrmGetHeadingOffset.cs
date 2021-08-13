using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using Calcflow;
using System.Runtime.InteropServices;

#pragma warning disable IDE1006

namespace ADCP
{
    public partial class FrmGetHeadingOffset : Form
    {
        public FrmGetHeadingOffset(SerialPort serialPort)
        {
            InitializeComponent();
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            sp = serialPort;
            LHeadingOffset.Clear();
            DecodeBytesDataGPS = new DisplayDelegate(PickAndDecodeEnsemble_GPS);//LPJ 2013-11-14 委托指针指向 PickAndDecodeEnsemble 函数 
        }

        #region  共有成员变量
        public double fHeadingOffset = 0; //LPJ 2013-11-15 GPS安装偏差值,单位为弧度
        #endregion

        #region 计算GPS安装艏向偏差参数
     
        //LPJ 2013-11-14 增加GPS安装校正功能
        private bool btnGPSCalibration_Start()
        {
            try
            {
                if (comboBox_RS232.Text != "" || comboBox_RS232.Text != null)
                {
                    return false;
                }

                //发送“CHS 2”命令
                sp.Write("CHS 2" + '\r');
                Thread.Sleep(150);

                //sp.Write("C232B " + comboBox_RS232.Text + '\r');
                sp.Write("C485B " + comboBox_RS232.Text + '\r');
                Thread.Sleep(150);

                sp.Write("START" + '\r');
                Thread.Sleep(150);
                //采集数据
                //初始化数据接收定时器
                queue.Clear();
                BytesArray.Clear();
                bStartGPSCalibration = true;
                RTIdata.Clear();

                RealTimeProcessingTimer = new System.Timers.Timer();
                RealTimeProcessingTimer.Elapsed += new System.Timers.ElapsedEventHandler(RealTimeProcessingTimer_Elapsed);
                RealTimeProcessingTimer.Interval = iRealTimeInterval;
                RealTimeProcessingTimer.Start();
            }
            catch
            {
                return false;
            }
            return true;

        }

        //LPJ 2013-11-14 增加GPS安装校正功能
        private bool btnGPSCalibration_Stop()
        {
            try
            {
                bStartGPSCalibration = false;

                sp.Write("STOP" + '\r');
                Thread.Sleep(150);

                while (!ReceiveBufferString.Contains("STOP"))
                {
                    sp.Write("STOP" + '\r');
                    Thread.Sleep(150);
                }

                RealTimeProcessingTimer.Stop();
                RealTimeProcessingTimer.Close();
                queue.Clear();
                BytesArray.Clear();
                ReceiveBufferString = "";

                //将数据中的NMEA_Buffer进行解析
                //计算CHeadingOffset
                double dHeadingOffset = 0; //LPJ 2013-11-21
                dHeadingOffset = GetGPSHeadingOffset();
                LHeadingOffset.Add(dHeadingOffset); //LPJ 2013-11-21

                RTIdata.Clear();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private double GetGPSHeadingOffset()
        {
            double dHeadingOffset;// = 0; //该单位为弧度

            List<float> fBoatVx_GPS = new List<float>();
            List<float> fBoatVy_GPS = new List<float>();

            for (int i = 0; i < RTIdata.Count; i++)
            {
                string GPVTGbuffer = "";
                string NMEA_buffer;// = "";
                NMEA_buffer = Encoding.Default.GetString(RTIdata[i].NMEA_Buffer);
               
                CDecodeGPS decodeGPS = new CDecodeGPS();
                float ve = 0, vn = 0;
                decodeGPS.GPSNMEA_decode(NMEA_buffer, "GPVTG", ref GPVTGbuffer);
                float GPS_boatspeed = 0;
                double GPS_angle = 0;
                decodeGPS.GPS_VTGdecode(GPVTGbuffer, ref GPS_boatspeed, ref GPS_angle, ref ve, ref vn); //提取在X、Y方向分量上的GPS中的船速

                fBoatVx_GPS.Add(ve);
                fBoatVy_GPS.Add(vn);
            }

            //调用CGetHeadingOffset类，计算GPS艏向安装偏角
            CGetHeadingOffset headingOffset = new CGetHeadingOffset(RTIdata, fBoatVx_GPS, fBoatVy_GPS);
            dHeadingOffset = headingOffset.dHeadingOffset / 180.0 * Math.PI; //调用CGetHeadingOffset类所得的角度，单位为度
            //label10.Text = headingOffset.dHeadingOffset.ToString();

            return dHeadingOffset;
        }

        private void PickAndDecodeEnsemble_GPS()  //LPJ 2013-11-14 该函数用于采集GPS安装校正的数据
        {
            if (!HasCheckedPayload)
            {
                if (HeaderFlag == false)
                {
                    if (BytesArray.Count >= preNum + 16)
                    {
                        int j = 0;
                        for (int i = preNum; i < BytesArray.Count; i++)
                        {
                            if (BytesArray[i] == null)   //LPJ 2012-6-2 判断是否有空
                            {
                                continue;
                            }
                            if (0x80 == (byte)BytesArray[i])
                                HeaderFlagNum++;
                            else
                            {
                                HeaderFlagNum = 0;
                                HeaderFlag = false;
                            }

                            j++;
                            if (16 == HeaderFlagNum)
                            {
                                HeaderFlag = true;
                                break;
                            }
                        }
                        preNum += j;

                        if (HeaderFlag == true)
                        {
                            if (preNum - 16 > 0)
                            {
                                lock (locknull)    //JZH 2012-06-11
                                {
                                    BytesArray.RemoveRange(0, preNum - 16 /*- 28*/);
                                }
                            }
                            HeaderFlagNum = 0;
                            preNum = 0;
                        }
                    }
                }
                if (HeaderFlag == true)         //JZH 2012-03-21  在一次数据解析过程中完成
                {
                    if (BytesArray.Count >= 32)//第preNum + 25 至 preNum + 28位为Payload
                    {
                        //byte[] EsmN = new byte[4];
                        //byte[] _EsmN = new byte[4];
                        byte[] Lng = new byte[4];
                        byte[] _Lng = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            //EsmN[i] = (byte)BytesArray[16 + i];
                            //_EsmN[i] = (byte)BytesArray[20 + i];
                            Lng[i] = (byte)BytesArray[24 + i];
                            _Lng[i] = (byte)BytesArray[28 + i];
                        }
                        payloadLen = BitConverter.ToInt32(Lng, 0);
                        int _payloadLen = BitConverter.ToInt32(_Lng, 0);

                        if ((payloadLen <= 0) || (payloadLen + 1 + _payloadLen != 0))//payloadLen必须为正，否则样本是错的
                        {
                            HeaderFlag = false;
                            HasCheckedPayload = false;
                            lock (locknull)   //JZH 2012-06-11
                            {
                                BytesArray.RemoveRange(0, 32);
                            }
                        }
                        else
                            HasCheckedPayload = true;
                    }
                }
            }

            if ((BytesArray.Count >= 36 + payloadLen) && HasCheckedPayload)//PacketBytes = new byte[offset + EnsemblePacket.EnsembleHeader.Payload + 4],4为校验和的位数
            {
                byte[] BytesPacket = new byte[payloadLen];

                try   //LPJ 2012-6-2 
                {
                    //LPJ 2012-6-11 cancle
                    BytesArray.CopyTo(32, BytesPacket, 0, payloadLen);  //源数组中至少有一个元素未能被向下转换到目标数组类型。Bug 2012-5-31
                }
                catch
                {
                    lock (locknull)  //LPJ 2012-06-12
                    {
                        BytesArray.RemoveRange(0, payloadLen + 36);
                    }
                    return;
                }
                //计算数据包除数据头、校验部分以外的所有数据校验和
                //byte[] ChksumBytes = new byte[4];
                byte[] ChksumBytes = CRC16Chksum(BytesPacket);

                //读取数据包中的校验和
                byte[] CopyChksumBytes = new byte[4];
                for (int i = payloadLen + 32; i < payloadLen + 36; i++)
                {
                    CopyChksumBytes[i - payloadLen - 32] = (byte)BytesArray[i];
                }

                ArrayClass Arr = new ArrayClass();
                //比较两个校验和是否相等
                if (BytesEquals(ChksumBytes, CopyChksumBytes))
                {
                    bool getArrSuccessful = true;
                    try
                    {
                        //DecodeEnsemble(BytesPacket, Arr, payloadLen);
                        CDecodeEnsemble decodeEn = new CDecodeEnsemble(BytesPacket, payloadLen); //LPJ 2013-11-15
                        Arr = decodeEn.Arr;
                    }
                    catch
                    {
                        getArrSuccessful = false;
                    }
               
                    if (getArrSuccessful)
                    {
                        //将数据添加到RTIData_GPS中
                        RTIdata.Add(Arr);
                        labelEnsNumber.Text = RTIdata.Count().ToString();
                    }
                }
                lock (locknull)   //JZH 2012-06-11
                {
                    BytesArray.RemoveRange(0, payloadLen + 36);
                }
                HeaderFlag = false;
                HasCheckedPayload = false;
            }
            PickAndDecodeEnsemble_FunctionIsFree = true;
        }

        public static byte[] CRC16Chksum(byte[] DataPacket)
        {
            byte[] bytes = new byte[4];
            ushort crc = 0;//seed = 0
            for (int i = 0; i < DataPacket.Length; i++)
            {

                crc = (ushort)((byte)(crc >> 8) | (crc << 8));
                crc ^= DataPacket[i];
                crc ^= (byte)((crc & 0xff) >> 4);
                crc ^= (ushort)((crc << 8) << 4);
                crc ^= (ushort)(((crc & 0xff) << 4) << 1);
            }

            ushort csum = crc;
            BitConverter.GetBytes(csum).CopyTo(bytes, 0);
            return bytes;
        }

        private bool BytesEquals(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length) return false;
            if (b1 == null || b2 == null) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }
            return true;
        }

        object locknull = new object();  //LPJ 2012-06-07   
        //JZH 2012-03-21 实时数据处理定时器事件
        private void RealTimeProcessingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (locknull)  //LPJ 2012-06-07  
            {
                while (queue.Count >= 1)  //当队列中有数据时，解队列到BytesArray中
                {
                    try    //LPJ 2012-6-2 
                    {
                        BytesArray.AddRange((byte[])(queue.Dequeue()));   //目标数组的长度不够。请检查 destIndex 和长度以及数组的下限。Bug 2012-5-31 
                    }
                    catch
                    {
                        queue.Clear();
                        break;
                    }
                }
            }
         
            if (BytesArray.Count >= 736 && bStartGPSCalibration) //LPJ 2013-11-14 GPS安装校正时
            {
                try
                {
                    if (PickAndDecodeEnsemble_FunctionIsFree)
                    {
                        PickAndDecodeEnsemble_FunctionIsFree = false;
                        this.BeginInvoke(DecodeBytesDataGPS);//委托指针指向  函数
                    }
                }
                catch
                {
                }
            }
        }

        //
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] pack = new byte[sp.BytesToRead];
                sp.Read(pack, 0, pack.Length);

                if (bStartGPSCalibration == false)
                {
                    ReceiveBufferString += Encoding.Default.GetString(pack);
                }
                else   //当数据开始记录时再将进行数据处理
                {
                    try
                    {
                        lock (locknull)
                        {
                            queue.Enqueue(pack);  //将接收到的串口数据放到队列中,数据解析在定时器时间中完成
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region 私有成员变量
        private SerialPort sp = new SerialPort();
        private Queue queue = new Queue();
        private ArrayList BytesArray = new ArrayList();
        private List<ArrayClass> RTIdata = new List<ArrayClass>();
        private System.Timers.Timer RealTimeProcessingTimer = new System.Timers.Timer();
        private int iRealTimeInterval = 1000;
        private string ReceiveBufferString = "";

        private delegate void DisplayDelegate(); //定义一个委托
        private DisplayDelegate DecodeBytesDataGPS;//LPJ 2013-11-14 声明这个委托，用以执行GPS安装校正

        private bool HasCheckedPayload = false;
        private int HeaderFlagNum = 0;
        private bool HeaderFlag = false;//校验开始16字节是否为0x80
        private int payloadLen = 0;
        private int preNum = 0;//28位 样本名E0000000（8位）+20位
        private bool PickAndDecodeEnsemble_FunctionIsFree = true;

        private List<double> LHeadingOffset = new List<double>(); //LPJ 2013-11-21 当用户航向校正时，测流多个测回，取平均
        private bool bStartGPSCalibration = false; //LPJ 2013-11-14 判断是否开始GPS安装校正数据采集
       
        #region cancel
        /*
        private static int HDRLEN = 0;
        private static int MaxArray = 11;
        private int PacketPointer = 0;
        private TestUnion ByteArrayToNumber;

        [StructLayout(LayoutKind.Explicit)]
        private struct TestUnion
        {
            [FieldOffset(0)]
            public byte A;
            [FieldOffset(1)]
            public byte B;
            [FieldOffset(2)]
            public byte C;
            [FieldOffset(3)]
            public byte D;
            [FieldOffset(0)]
            public float Float;
            [FieldOffset(0)]
            public int Int;
        }

        private string VelocityID = "E000001\0";
        private string InstrumentID = "E000002\0";
        private string EarthID = "E000003\0";
        private string AmplitudeID = "E000004\0";
        private string CorrelationID = "E000005\0";
        private string BeamNID = "E000006\0";
        private string XfrmNID = "E000007\0";
        private string EnsembleDataID = "E000008\0";
        private string AncillaryID = "E000009\0";
        private string BottomTrackID = "E000010\0";
        private string NMEAID = "E000011\0";
        */
        #endregion

        #endregion

        #region 按钮
        private void btnStart_Click(object sender, EventArgs e)
        {
            //helpme
            MessageBox.Show("Offset function not ready A");
            if (false)
            {
                if (btnGPSCalibration_Start())
                {
                    btnStop.Enabled = true;
                }
                else
                {
                    //SM>
                    //MessageBox.Show("请检查串口连接是否正确");//Please check if the serial port connection is correct
                    MessageBox.Show("Check serial port connection");
                    //<SM
                }
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            //helpme
            MessageBox.Show("Offset function not ready B");
            if (false)
            {
                if (btnGPSCalibration_Stop())
                {
                    btnStart.Enabled = true;
                    btnOK.Enabled = true;

                    //将艏向偏差角显示出来
                    //string strHeadingOffset = (fHeadingOffset / Math.PI * 180).ToString("0.00");
                    //textBoxHeadingOffset.Text = "Result" + "\r\n\r\n" + " The Heading Offset is (Deg):" + strHeadingOffset;

                    textBoxHeadingOffset.Text = "Result" + "\r\n\r\n" + " The Heading Offset is (Deg):";
                    double dHeadingOffsetAve = 0;
                    for (int i = 0; i < LHeadingOffset.Count; i++) //LPJ 2013-11-21 取几次测回的平均值，作为最终结果
                    {
                        textBoxHeadingOffset.Text += i.ToString("") + "   " + (LHeadingOffset[i] / Math.PI * 180).ToString("0.00") + "\r\n";
                        dHeadingOffsetAve += LHeadingOffset[i];
                    }
                    fHeadingOffset = dHeadingOffsetAve / LHeadingOffset.Count;

                    textBoxHeadingOffset.Text += "Average is" + (fHeadingOffset / Math.PI * 180).ToString("0.00");
                }
                else
                {
                    MessageBox.Show("");
                }
            }
         
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //如果点击“确定”，则认为计算的艏向偏角可用
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //点击“取消”，则不用计算的艏向偏角
            this.Close();

        }
        #endregion
    }
}
