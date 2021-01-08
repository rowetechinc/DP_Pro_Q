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

namespace ADCP
{
    public partial class FrmSetTime : Form
    {
        public FrmSetTime(/*string strTime,*/SerialPort sp)
        {
            InitializeComponent();
            try
            {
                _sp = new SerialPort(sp.PortName, sp.BaudRate);   //2017-3-24
                _sp.DataReceived += new SerialDataReceivedEventHandler(_sp_DataReceived);

                if (!_sp.IsOpen)
                {
                    _sp.Open();
                }

                _sp.Write("STIME" + '\r');
                Thread.Sleep(300);

                //GetInstrumentTime(strTime); //LPJ 2013-10-30
                GetInstrumentTime(ReceiveBufferString);
            }
            catch { }
        }

        private void GetInstrumentTime(string strTime) //将仪器时间YYYY/MM/DD,HH:mm:SS提取并显示
        {
            try
            {
                string[] strPack = strTime.Split('\n');
                string strTimeNow = null;
                for (int i = 0; i < strPack.Count(); i++)
                {
                    if (strPack[i].Contains("STIME"))
                    {
                        try
                        {
                            string[] strDate = strPack[i + 1].Split('\r');
                            strTimeNow = strDate[0];
                        }
                        catch
                        {
                        }

                    }
                }
                try
                {
                    if (strTimeNow != null)
                    {
                        string[] strTimePack = strTimeNow.Split(' '); //将日期和时间分开
                        //string[] strTimeDatePack = strTimePack[0].Split('/');  //将日期中的年月日分别提出
                        dateTimePicker_Date.Value = DateTime.Parse(strTimePack[0]);
                        //string[] strTimeHourPack = strTimePack[1].Split(':'); //将时间中的时分秒分别提出
                        dateTimePicker_Time.Value = DateTime.Parse(strTimePack[1]);
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }

        private SerialPort _sp;  //2017-3-24

        public bool bUsePCTime = false;
        public string strDateTime;
        private void btnSetTime_Click(object sender, EventArgs e)
        {
            btnSetTime.Enabled = false;
            btnReadTime.Enabled = false;

            #region  //2017-3-24
            Cursor.Current = Cursors.WaitCursor;
            if (radioBtnPCTime.Checked)
            {
                _sp.Write("STIME " + DateTime.Now.Year.ToString("0000") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Day.ToString("00") + "," +
                                                 DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + '\r'); //LPJ 2017-1-3 将电脑时间发送到仪器
                Thread.Sleep(2000);
            }
            else
            {
                _sp.Write("STIME " + DateTime.UtcNow.Year.ToString("0000") + "/" + DateTime.UtcNow.Month.ToString("00") + "/" + DateTime.UtcNow.Day.ToString("00") + "," +
                                               DateTime.UtcNow.Hour.ToString("00") + ":" + DateTime.UtcNow.Minute.ToString("00") + ":" + DateTime.UtcNow.Second.ToString("00") + '\r'); //LPJ 2017-3-24 将UTC时间发送到仪器
                Thread.Sleep(2000);
            }

            ReceiveBufferString = null;

            _sp.Write("STIME" + '\r');
            Thread.Sleep(300);

            GetInstrumentTime(ReceiveBufferString);
            Cursor.Current = Cursors.Default;
            btnSetTime.Enabled = true;
            btnReadTime.Enabled = true;
            #endregion
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_sp.IsOpen)
                    _sp.Close();
            }
            catch
            {
            }
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                if (_sp.IsOpen)
                    _sp.Close();
            }
            catch
            {
            }

            base.OnClosing(e);
        }
        
        #region
        string ReceiveBufferString = null;
        private void _sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] pack = new byte[_sp.BytesToRead];   
                _sp.Read(pack, 0, pack.Length);
                ReceiveBufferString += Encoding.Default.GetString(pack);
            }
            catch
            {
            }

        }
        #endregion

        private void btnReadTime_Click(object sender, EventArgs e)
        {
            if (_sp.IsOpen)
            {
                btnReadTime.Enabled = false;
                btnSetTime.Enabled = false;

                ReceiveBufferString = null;

                _sp.Write("STIME" + '\r');
                Thread.Sleep(300);


                GetInstrumentTime(ReceiveBufferString);
                btnReadTime.Enabled = true;
                btnSetTime.Enabled = true;
            }
        }

        private void FrmSetTime_Load(object sender, EventArgs e)
        {

        }
    }
}
