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
    public partial class FrmUpLoad : Form
    {
        public FrmUpLoad(RTI.AdcpSerialPort adcpsp,string strFileName)
        {
            InitializeComponent();

            showDelegateEvent = new ShowDelegate(OnShown);

            sp = adcpsp;
            sp.Reconnect();
            strPath = strFileName;

            timerShown.Interval = 200;
            timerShown.Elapsed+=new System.Timers.ElapsedEventHandler(timerShown_Elapsed);

            sp.XModemUpload(strPath);

            timerShown.Start();
        }

        private void OnShown()
        {
            try
            {
                textBoxShow.Text += sp.ReceiveBufferString;

                if (sp.ReceiveBufferString.Contains("File successfully written to SD"))
                {
                    //timerShown.Stop();
                    //sp.Disconnect();
                    this.Close();
                }
            }
            catch
            {
            }
        }

        private void timerShown_Elapsed(object sender, EventArgs e)
        {
            try
            {
                this.BeginInvoke(showDelegateEvent);
            }
            catch
            {
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                timerShown.Stop();
                if (sp.IsOpen())
                    sp.Disconnect();
            }
            catch
            {
            }

            base.OnFormClosing(e);
        }

        #region 
        private RTI.AdcpSerialPort sp;
        private string strPath;
        private System.Timers.Timer timerShown = new System.Timers.Timer(); //设置一个定时器，
        private delegate void ShowDelegate();
        ShowDelegate showDelegateEvent;
        #endregion

    }
}
