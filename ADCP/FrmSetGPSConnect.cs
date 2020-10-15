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
    public partial class FrmSetGPSConnect : Form
    {
        public FrmSetGPSConnect()
        {
            InitializeComponent();
            GetPortName();
            try
            {
                comboxGPS_Port.SelectedIndex = 0; 
            }
            catch
            {
                btnGPSconnect.Enabled = false;
            }

            comboBoxGPS_Baudrate.SelectedIndex = 2;
        }

        public SerialPort GPSsp = new SerialPort();
        private void btnGPSconnect_Click(object sender, EventArgs e)
        {
            GPSsp.PortName = comboxGPS_Port.Text;
            GPSsp.BaudRate = int.Parse(comboBoxGPS_Baudrate.Text);
           
            this.Close();
        }

        private void comboxGPS_Port_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboxGPS_Port_Click(object sender, EventArgs e)
        {
            
        }

        private void GetPortName()
        {
            comboxGPS_Port.Items.Clear();
            string[] ports = SerialPort.GetPortNames();//得到系统的串口号
           
            //LPJ 2013-10-17 添加判断串口是否被占用，没占用的则不显示 --start
            SerialPort _tempPort;
            foreach (string str in ports)
            {
                _tempPort = new SerialPort(str);
                try
                {
                    _tempPort.Open();
                }
                catch
                {
                    continue;
                }
                if (_tempPort.IsOpen)
                {
                    _tempPort.Close();
                    comboxGPS_Port.Items.Add(str);
                }

            }
            //LPJ 2013-10-17 添加判断串口是否被占用，没占用的则不显示 --end

            //Array.Sort(ports);//对串口号字符串排序
            //comboxGPS_Port.Items.AddRange(ports);//添加到组合框
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
