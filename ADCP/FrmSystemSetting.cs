using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections;

namespace ADCP
{
    public partial class FrmSystemSetting : Form
    {
        public FrmSystemSetting(SerialPort _sp, bool English)
        {
            InitializeComponent();
            sp = _sp;
            systSet.bEnglishUnit = English;
            GetReference();
        }
        private SerialPort sp;
        public static SystemSetting systSet;
        private void GetReference()
        {   
            CheckText();

            if (systSet.bEnglishUnit)
            {
                label6.Text = "(ft)";
                label16.Text = "(ft)";
                label22.Text = "(ft)";
                label23.Text = "(ft)";
                label18.Text = "(ft/s)";

                //string str = projectUnit.FeetToMeter(FrmSystemSetting.systSet.dTransducerDepth, 1).ToString() + "\r\n";

                //textBoxMaxDepth
                //textBoxWPswitchDepth
                //textBoxBTswitchDepth
                //textBoxTransducerDepth
                //textSoundSpeed
            }
            else
            {
                label6.Text = "(m)";
                label16.Text = "(m)";
                label22.Text = "(m)";
                label23.Text = "(m)";
                label18.Text = "(m/s)";
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {   
            //systemSet.iFlowRef = comboBoxFlowRef.SelectedIndex;
            /*
            systSet.iHeadingRef = comboBoxHeadingRef.SelectedIndex;
            systSet.strRS232 = comboBox_RS232.Text;
            systSet.iSpeedRef = comboBoxVesselSpeedRef.SelectedIndex;
            systSet.iMeasurmentMode = comboBoxMeasMode.SelectedIndex;
            systSet.iVerticalBeam = comboBoxVerticalBeam.SelectedIndex;
            systSet.iAutoBinSize= comboBoxAutoBinSize.SelectedIndex;
            systSet.iWaterTemperatureSource= comboBox_waterTemperature.SelectedIndex;
            systSet.iTransducerDepthSource = comboBox_TransducerDepth.SelectedIndex;
            systSet.iSalinitySource = comboBox_Salinity.SelectedIndex;
            systSet.iSpeedOfSoundSource = comboBox_SpeedOfSound.SelectedIndex;
            */
        }

        
        public struct SystemSetting
        {
            //public int iFlowRef;
            public int iSpeedRef;
            public int iHeadingRef;
            public string strRS232;

            public int iMeasurmentMode;
            public int iVerticalBeam;
            public int iAutoBinSize;
            public int iAutoLag;
            public int iWaterTemperatureSource;
            public int iTransducerDepthSource;
            public int iSalinitySource;
            public int iSpeedOfSoundSource;

            public int iBins;

            public double dAveragingInterval;
            public double dMaxMeasurementDepth;
            public double dWpSwitchDepth;
            public double dBtSwitchDepth;
            public double dTransducerDepth;
            public double dBtCorrelationThreshold;
            public double dHeadingOffset;

            public double dBtSNR;

            public double dSalinity;
            public double dWaterTemperature;
            public double dSpeedOfSound;

            public bool bEnglishUnit;
            public int iInstrumentTypes;
            
        }
 
        ClassValidateInPut validateInput = new ClassValidateInPut();

        
        public void CheckText()
        {
            btnOK.Enabled = true;

            if (!validateInput.ValidateUserInput(textWaterSalinity.Text, 7))
            {
                //ToolTip tooltip1 = new ToolTip();
                //tooltip1.Show(Resource1.String80, textHeadingOffset);
                btnOK.Enabled = false;
                textWaterSalinity.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textWaterSalinity.Text) >= 0) && (double.Parse(textWaterSalinity.Text) < 45))
                {
                    textWaterSalinity.BackColor = Color.White;
                    systSet.dSalinity = double.Parse(textWaterSalinity.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textWaterSalinity.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxTransducerDepth.Text, 10))
            {
                //ToolTip tooltip1 = new ToolTip();
                //tooltip1.Show(Resource1.String76, textBoxTransducerDepth);
                btnOK.Enabled = false;
                textBoxTransducerDepth.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxTransducerDepth.Text) >= 0 && double.Parse(textBoxTransducerDepth.Text) < 100)
                {
                    textBoxTransducerDepth.BackColor = Color.White;
                    systSet.dTransducerDepth = double.Parse(textBoxTransducerDepth.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxTransducerDepth.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textWaterTemperature.Text, 7))
            {
                btnOK.Enabled = false;
                textWaterTemperature.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textWaterTemperature.Text) > -10 && double.Parse(textWaterTemperature.Text) < 60)
                {
                    textWaterTemperature.BackColor = Color.White;
                    systSet.dWaterTemperature = double.Parse(textWaterTemperature.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textWaterTemperature.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textSoundSpeed.Text, 7))
            {
                btnOK.Enabled = false;
                textSoundSpeed.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textSoundSpeed.Text) > 1400 && double.Parse(textSoundSpeed.Text) < 1600)
                {
                    textSoundSpeed.BackColor = Color.White;
                    systSet.dSpeedOfSound = double.Parse(textSoundSpeed.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textSoundSpeed.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxMaxDepth.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxMaxDepth.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxMaxDepth.Text) >= 2 && double.Parse(textBoxMaxDepth.Text) < 100)
                {
                    textBoxMaxDepth.BackColor = Color.White;
                    systSet.dMaxMeasurementDepth = double.Parse(textBoxMaxDepth.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxMaxDepth.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxNumberOfBins.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxNumberOfBins.BackColor = Color.Red;
            }
            else
            {
                if (int.Parse(textBoxNumberOfBins.Text) > 1 && int.Parse(textBoxNumberOfBins.Text) < 257)
                {
                    textBoxNumberOfBins.BackColor = Color.White;
                    systSet.iBins = int.Parse(textBoxNumberOfBins.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxNumberOfBins.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxWPaveragingInterval.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxWPaveragingInterval.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxWPaveragingInterval.Text) > 0 && double.Parse(textBoxWPaveragingInterval.Text) <= 0.5)
                {
                    textBoxWPaveragingInterval.BackColor = Color.White;
                    systSet.dAveragingInterval = double.Parse(textBoxWPaveragingInterval.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxWPaveragingInterval.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxWPswitchDepth.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxWPswitchDepth.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxWPswitchDepth.Text) >= 2 && double.Parse(textBoxWPswitchDepth.Text) <= 20)
                {
                    textBoxWPswitchDepth.BackColor = Color.White;
                    systSet.dWpSwitchDepth = double.Parse(textBoxWPswitchDepth.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxWPswitchDepth.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxBTswitchDepth.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxBTswitchDepth.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxBTswitchDepth.Text) >= 2 && double.Parse(textBoxBTswitchDepth.Text) <= 50)
                {
                    textBoxBTswitchDepth.BackColor = Color.White;
                    systSet.dBtSwitchDepth = double.Parse(textBoxBTswitchDepth.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxBTswitchDepth.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(BTST_Correlation_text.Text, 7))
            {
                btnOK.Enabled = false;
                BTST_Correlation_text.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(BTST_Correlation_text.Text) >= 0.0 && double.Parse(BTST_Correlation_text.Text) <= 1.0)
                {
                    BTST_Correlation_text.BackColor = Color.White;
                    systSet.dBtCorrelationThreshold = double.Parse(BTST_Correlation_text.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    BTST_Correlation_text.BackColor = Color.Red;
                }
            }
            //
            if (!validateInput.ValidateUserInput(textHeadingOffset.Text, 7))
            {
                //ToolTip tooltip1 = new ToolTip();
                //tooltip1.Show(Resource1.String80, textHeadingOffset);
                btnOK.Enabled = false;
                textHeadingOffset.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textHeadingOffset.Text) > 180 || double.Parse(textHeadingOffset.Text) < -180)
                {
                    //ToolTip tooltip1 = new ToolTip();
                    //tooltip1.Show(Resource1.String80, textHeadingOffset);
                    btnOK.Enabled = false;
                    textHeadingOffset.BackColor = Color.Red;
                }
                else
                {
                    textHeadingOffset.BackColor = Color.White;
                    systSet.dHeadingOffset = double.Parse(textHeadingOffset.Text);
                }
            }
            //
            if (!validateInput.ValidateUserInput(textBoxBTSNR.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxBTSNR.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxBTSNR.Text) >= 0 && double.Parse(textBoxBTSNR.Text) < 100)
                {
                    textBoxBTSNR.BackColor = Color.White;
                    systSet.dBtSNR = double.Parse(textBoxBTSNR.Text);
                }
                else
                {
                    btnOK.Enabled = false;
                    textBoxBTSNR.BackColor = Color.Red;
                }
            }

            //if (btnOK.Enabled)
            {
                systSet.iHeadingRef = comboBoxHeadingRef.SelectedIndex;
                systSet.strRS232 = comboBox_RS232.Text;
                systSet.iSpeedRef = comboBoxVesselSpeedRef.SelectedIndex;
                systSet.iMeasurmentMode = comboBoxMeasMode.SelectedIndex;
                systSet.iVerticalBeam = comboBoxVerticalBeam.SelectedIndex;
                systSet.iAutoBinSize = comboBoxAutoBinSize.SelectedIndex;
                systSet.iAutoLag = comboBoxAutoLag.SelectedIndex;
                systSet.iWaterTemperatureSource = comboBox_waterTemperature.SelectedIndex;
                systSet.iTransducerDepthSource = comboBox_TransducerDepth.SelectedIndex;
                systSet.iSalinitySource = comboBox_Salinity.SelectedIndex;
                systSet.iSpeedOfSoundSource = comboBox_SpeedOfSound.SelectedIndex;
            }
        }
        public void TheTextChanged(object sender, EventArgs e)
        {
            CheckText();
        }
        private void comboBoxHeadingRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (0 == comboBoxHeadingRef.SelectedIndex)
            {
                comboBox_RS232.Enabled = false;
                label_RS232.Enabled = false;
                if (1 == comboBoxVesselSpeedRef.SelectedIndex || 3 == comboBoxVesselSpeedRef.SelectedIndex)
                {
                    textHeadingOffset.Enabled = true;
                }
                else
                    textHeadingOffset.Enabled = false;
            }
            else
            {
                if (bPlayBackMode)
                {
                    comboBox_RS232.Enabled = false;
                    label_RS232.Enabled = false;
                }
                else
                {
                    comboBox_RS232.Enabled = true;
                    label_RS232.Enabled = true;
                }
                textHeadingOffset.Enabled = true;
            }
            */
        }
        private void comboBoxVesselSpeedRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (1 == comboBoxVesselSpeedRef.SelectedIndex || 3 == comboBoxVesselSpeedRef.SelectedIndex)
            {
                textHeadingOffset.Enabled = true;
            }
            else
            {
                if (1 == comboBoxHeadingRef.SelectedIndex)
                    textHeadingOffset.Enabled = true;
                else
                    textHeadingOffset.Enabled = false;
            }
            */
        }
        private void comboBox_RS232_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBoxMeasMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBoxVerticalBeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBoxAutoBinSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void textWaterSalinity_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxTransducerDepth_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textWaterTemperature_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textSoundSpeed_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxMaxDepth_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textHeadingOffset_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxNumberOfBins_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxWPaveragingInterval_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxWPswitchDepth_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxBTswitchDepth_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void BTST_Correlation_text_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void comboBox_waterTemperature_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBox_TransducerDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBox_Salinity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBox_SpeedOfSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TheTextChanged(sender, e);
        }
        private void comboBoxAutoLag_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //private SerialPort sp;  
        private System.Collections.Queue defaultQueue;// = new System.Collections.Queue();

        static object lockpack = new object();
        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (lockpack)
            {
                byte[] packet = new byte[sp.BytesToRead];
                sp.Read(packet, 0, packet.Length);
                defaultQueue.Enqueue(packet);
            }
        }
        private void buttonDownloadCommandSettings_Click(object sender, EventArgs e)
        {
            defaultQueue = new System.Collections.Queue();

            //sp.PortName = "COM18";
            //sp.BaudRate = 921600;

            sp.Close();
            sp.Open();

            sp.DataReceived += new SerialDataReceivedEventHandler(SP_DataReceived);

            string strpack = "";
            bool OK = false;
            if (sp.IsOpen)
            {
                sp.Write("CRSSHOW\r");
                Thread.Sleep(1000);
                
                int thecount = 0;
                int flag = 1;
                while (flag < 2 && !OK)
                {
                    if (defaultQueue.Count > 0)
                    {                        
                        while (defaultQueue.Count > 0)
                        {
                            thecount++;
                            ArrayList byteArray = new ArrayList();
                            byteArray.AddRange((byte[])defaultQueue.Dequeue());
                            byte[] pack = new byte[byteArray.Count];

                            for (int i = 0; i < byteArray.Count; i++)
                                pack[i] = (byte)byteArray[i];

                            strpack += Encoding.Default.GetString(pack);
                        }
                        if (strpack.Contains("SN07"))
                        {
                            OK = true;
                        }
                    }
                    flag++;
                }
            }
            if (OK)
            {
                Int32 index = 0;
                double dVal;
                string[] separatingStrings = { "<<", "...", " ", "," };

                var reader = new StringReader(strpack);
                string cmd = reader.ReadLine();
                string[] cmdPart = new string[2];
                while (cmd != null)
                {   
                    try
                    {
                        if (cmd != "")
                        {
                            cmdPart = cmd.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                            switch (cmdPart[0])
                            {
                                case "CRSMODE":
                                    try
                                    {
                                        index = Convert.ToInt32(cmdPart[1]);
                                        comboBoxMeasMode.SelectedIndex = index;
                                    }
                                    catch { }
                                    break;
                                case "CRSVB":
                                    try
                                    {
                                        index = Convert.ToInt32(cmdPart[1]);
                                        comboBoxVerticalBeam.SelectedIndex = index;
                                    }
                                    catch { }
                                    break;
                                case "CRSAUTOBIN":
                                    try
                                    {
                                        index = Convert.ToInt32(cmdPart[1]);
                                        comboBoxAutoBinSize.SelectedIndex = index;
                                    }
                                    catch { }
                                    break;
                                case "CRSAUTOLAG":
                                    try
                                    {
                                        index = Convert.ToInt32(cmdPart[1]);
                                        comboBoxAutoLag.SelectedIndex = index;
                                    }
                                    catch { }
                                    break;
                                case "CWSSC":
                                    try
                                    {
                                        index = Convert.ToInt32(cmdPart[1]);
                                        comboBox_waterTemperature.SelectedIndex = index;
                                        index = Convert.ToInt32(cmdPart[2]);
                                        comboBox_TransducerDepth.SelectedIndex = index;
                                        index = Convert.ToInt32(cmdPart[3]);
                                        comboBox_Salinity.SelectedIndex = index;
                                        index = Convert.ToInt32(cmdPart[4]);
                                        comboBox_SpeedOfSound.SelectedIndex = index;
                                    }
                                    catch { }
                                    break;

                                case "CRSWPBN":
                                    try
                                    {
                                        index = Convert.ToInt32(cmdPart[1]);
                                        textBoxNumberOfBins.Text = index.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSWPAI":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textBoxWPaveragingInterval.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSMAXDEPTH":
                                    try
                                    {
                                        //Units

                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if(systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textBoxMaxDepth.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSWPSWITCH":
                                    try
                                    {
                                        //textBoxWPswitchDepth Units
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textBoxWPswitchDepth.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSBTSWITCH":
                                    try
                                    {
                                        //textBoxBTswitchDepth Units
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textBoxBTswitchDepth.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSXDCRDEPTH":
                                    try
                                    {
                                        //textBoxTransducerDepth Units
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textBoxTransducerDepth.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSSALINITY":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textWaterSalinity.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSTEMP":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textWaterTemperature.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSBTSNR":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textBoxBTSNR.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CRSBTCOR":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        BTST_Correlation_text.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CWSS":
                                    try
                                    {
                                        //textSoundSpeed Units
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textSoundSpeed.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                case "CHO":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        textHeadingOffset.Text = dVal.ToString();
                                    }
                                    catch { }
                                    break;
                                    
                            }
                        }
                        try
                        {
                            cmd = reader.ReadLine();
                        }
                        catch //(Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }
                    catch //(Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }
            sp.DataReceived -= new SerialDataReceivedEventHandler(SP_DataReceived);
            //sp.Close();
        }

        private void buttonReadCommandSettings_Click(object sender, EventArgs e)
        {

        }

        private void buttonUploadCommandSettings_Click(object sender, EventArgs e)
        {

        }
        private Units projectUnit = new Units();
        private void buttonWriteCommandSettings_Click(object sender, EventArgs e)
        {
            
            

            string CMD = "CRSMODE " + FrmSystemSetting.systSet.iMeasurmentMode.ToString() + "\r\n";
            CMD += "CRSVB " + FrmSystemSetting.systSet.iVerticalBeam.ToString() + "\r\n";
            CMD += "CRSAUTOBIN " + FrmSystemSetting.systSet.iAutoBinSize.ToString() + "\r\n";
            CMD += "CRSAUTOLAG " + FrmSystemSetting.systSet.iAutoLag.ToString() + "\r\n";
            CMD += "CRSWPBN " + FrmSystemSetting.systSet.iBins.ToString() + "\r\n";
            CMD += "CRSWPAI " + FrmSystemSetting.systSet.dAveragingInterval.ToString() + "\r\n";
            CMD += "CRSMAXDEPTH " + FrmSystemSetting.systSet.dMaxMeasurementDepth.ToString() + "\r\n";
            CMD += "CRSWPSWITCH " + FrmSystemSetting.systSet.dWpSwitchDepth.ToString() + "\r\n";
            CMD += "CRSBTSWITCH " + FrmSystemSetting.systSet.dBtSwitchDepth.ToString() + "\r\n";
            if (systSet.bEnglishUnit)
            {
                CMD += "CRSXDCRDEPTH " + projectUnit.FeetToMeter(FrmSystemSetting.systSet.dTransducerDepth, 1).ToString() + "\r\n";
            }
            else
            {
                CMD += "CRSXDCRDEPTH " + FrmSystemSetting.systSet.dTransducerDepth.ToString() + "\r\n";
            }
            CMD += "CWSSC " + FrmSystemSetting.systSet.iWaterTemperatureSource.ToString();
            CMD += "," + FrmSystemSetting.systSet.iTransducerDepthSource.ToString();
            CMD += "," + FrmSystemSetting.systSet.iSalinitySource.ToString();
            CMD += "," + FrmSystemSetting.systSet.iSpeedOfSoundSource.ToString();
            CMD += "\r\n";
            if (systSet.bEnglishUnit)
            {
                CMD += "CWSS " + projectUnit.FeetToMeter(FrmSystemSetting.systSet.dSpeedOfSound, 1).ToString() + "\r\n";
            }
            else
            {
                CMD += "CWSS " + FrmSystemSetting.systSet.dSpeedOfSound.ToString() + "\r\n";
            }
            CMD += "CRSSALINITY " + FrmSystemSetting.systSet.dSalinity.ToString() + "\r\n";
            CMD += "CRSTEMP " + FrmSystemSetting.systSet.dWaterTemperature.ToString() + "\r\n";
            CMD += "CRSBTSNR " + FrmSystemSetting.systSet.dBtSNR.ToString() + "\r\n";
            CMD += "CRSBTCOR " + FrmSystemSetting.systSet.dBtCorrelationThreshold.ToString() + "\r\n";
            CMD += "CHO " + FrmSystemSetting.systSet.dHeadingOffset.ToString() + "\r\n";
            CMD += "CHS " + FrmSystemSetting.systSet.iHeadingRef.ToString() + "\r\n";
            CMD += "C232B " + FrmSystemSetting.systSet.strRS232 + "\r\n";

            //string fileName = Directory.GetCurrentDirectory() + "\\CommandFiles" + "\\Commands.txt";
            string fileName = Directory.GetCurrentDirectory() + "\\Commands.txt";
            try
            {
                File.WriteAllText(fileName, CMD);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //File.AppendAllText(fileName, "ADCP_BaudRate " + sp.BaudRate + "\r\n");
        }
    }
}
