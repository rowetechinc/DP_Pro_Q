using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace ADCP
{
    public partial class FrmSetCmd : Form
    {
        public FrmSetCmd()
        {
            InitializeComponent();
            comboBoxADCP_Baudrate.SelectedIndex = 5;
        }

       
        private void comboxADCP_Port_Click(object sender, EventArgs e)
        {
            comboxADCP_Port.Items.Clear();

            string[] ports = SerialPort.GetPortNames();//得到系统的串口号
            Array.Sort(ports);//对串口号字符串排序
            comboxADCP_Port.Items.AddRange(ports);//添加到组合框

        //    comboxADCP_Port.SelectedIndex = comboxADCP_Port.Items.IndexOf(sp.PortName);
        }

        //public bool bConnect = false;
        public SerialPort ADCPsp = new SerialPort();
        public SerialPort GPSsp = new SerialPort();
        public bool bGPS = false;

        private void btnADCPconnect_Click(object sender, EventArgs e)
        {
            try
            {
                ADCPsp.PortName = comboxADCP_Port.Text;
                ADCPsp.BaudRate = int.Parse(comboBoxADCP_Baudrate.Text);
                //if (bGPS) //LPJ 2013-6-3 cancel
                //{
                //    GPSsp.PortName = comboxGPS_Port.Text;
                //    GPSsp.BaudRate = int.Parse(comboBoxGPS_Baudrate.Text);
                //}
            }
            catch
            {
                ADCPsp.BaudRate = int.Parse(comboBoxADCP_Baudrate.Text);
            }
            this.Close();
        }

        private void comboxGPS_Port_Click(object sender, EventArgs e)
        {
            //comboxGPS_Port.Items.Clear();

            //string[] ports = SerialPort.GetPortNames();//得到系统的串口号
            //Array.Sort(ports);//对串口号字符串排序
            //comboxGPS_Port.Items.AddRange(ports);//添加到组合框

        }

        private void btnGPSconnect_Click(object sender, EventArgs e)
        {
            //GPSsp.PortName = comboxGPS_Port.Text;
            //GPSsp.BaudRate = int.Parse(comboBoxGPS_Baudrate.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked)
            //{
            //    bGPS = true;
            //    comboBoxGPS_Baudrate.Enabled = true;
            //    comboxGPS_Port.Enabled = true;
            //    btnGPSconnect.Enabled = true;
            //}
            //else
            //{
            //    bGPS = false;
            //    comboBoxGPS_Baudrate.Enabled = false;
            //    comboxGPS_Port.Enabled = false;
            //    btnGPSconnect.Enabled = false;
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //bConnect = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LPJ 2013-8-1 自动检测波特率
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    bConnect = false;
        //    base.OnClosing(e);
        //}

    }
}
