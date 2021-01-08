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

#pragma warning disable IDE1006

namespace ADCP
{
    public partial class FrmSystemSetting : Form
    {
        SystemSetting systSet;
        string theList = "";
        private SerialPort sp;

        public FrmSystemSetting(SerialPort _sp, ref SystemSetting _systSet)
        {
            InitializeComponent();
            sp = _sp;
            systSet = _systSet;
            GetReference(ref systSet);
            _systSet = systSet;
        }

        public DialogResult ShowDialog(ref SystemSetting _systSet, ref string _theList)
        {
            Cursor.Current = Cursors.WaitCursor;

            //Assign received parameter(s) to local context
            systSet = _systSet;
            theList = _theList;

            DialogResult result = this.ShowDialog(); //Display and activate this form (Form2)

            //Return parameter(s)
            _systSet = systSet;
            _theList = theList;

            //Cursor.Current = Cursors.Default;

            return result;
        }
        private void GetReference(ref SystemSetting systSet)
        {
            DownloadCommandSettings();

            CheckText(ref systSet);

            if (systSet.bEnglishUnit)
            {
                label6.Text = "(ft)";
                label16.Text = "(ft)";
                label22.Text = "(ft)";
                label23.Text = "(ft)";
                label18.Text = "(ft/s)";

                //string str = projectUnit.FeetToMeter(systSet.dTransducerDepth, 1).ToString() + "\r\n";


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

        ClassValidateInPut validateInput = new ClassValidateInPut();
        public void CheckText(ref SystemSetting systSet)
        {
            double min, max;
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
                min = 0;
                max = 100;
                if (systSet.bEnglishUnit)
                {
                    min = projectUnit.MeterToFeet(min, 1);
                    max = projectUnit.MeterToFeet(max, 1);
                }
                if (double.Parse(textBoxTransducerDepth.Text) >= min && double.Parse(textBoxTransducerDepth.Text) < max)
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
                min = 1400;
                max = 1600;
                if (systSet.bEnglishUnit)
                {
                    min = projectUnit.MeterToFeet(min, 1);
                    max = projectUnit.MeterToFeet(max, 1);
                }
                if (double.Parse(textSoundSpeed.Text) > min && double.Parse(textSoundSpeed.Text) < max)
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
                min = 2;
                max = 100;
                if (systSet.bEnglishUnit)
                {
                    min = projectUnit.MeterToFeet(min, 1);
                    max = projectUnit.MeterToFeet(max, 1);
                }

                if (double.Parse(textBoxMaxDepth.Text) >= min && double.Parse(textBoxMaxDepth.Text) <= max)
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
                min = 2;
                max = 20;
                if (systSet.bEnglishUnit)
                {
                    min = projectUnit.MeterToFeet(min, 1);
                    max = projectUnit.MeterToFeet(max, 1);
                }
                if (double.Parse(textBoxWPswitchDepth.Text) >= min && double.Parse(textBoxWPswitchDepth.Text) <= max)
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
                min = 2;
                max = 50;
                if (systSet.bEnglishUnit)
                {
                    min = projectUnit.MeterToFeet(min, 1);
                    max = projectUnit.MeterToFeet(max, 1);
                }
                if (double.Parse(textBoxBTswitchDepth.Text) >= min && double.Parse(textBoxBTswitchDepth.Text) <= max)
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
            if (!validateInput.ValidateUserInput(textBoxBTSNR.Text, 7))
            {
                btnOK.Enabled = false;
                textBoxBTSNR.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textBoxBTSNR.Text) >= 0.0 && double.Parse(textBoxBTSNR.Text) <= 100)
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

            // Magnetic Variance
            if (!validateInput.ValidateUserInput(textMagVar.Text, 7))
            {
                //ToolTip tooltip1 = new ToolTip();
                //tooltip1.Show(Resource1.String80, textHeadingOffset);
                btnOK.Enabled = false;
                textMagVar.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textMagVar.Text) > 180 || double.Parse(textMagVar.Text) < -180)
                {
                    //ToolTip tooltip1 = new ToolTip();
                    //tooltip1.Show(Resource1.String80, textHeadingOffset);
                    btnOK.Enabled = false;
                    textMagVar.BackColor = Color.Red;
                }
                else
                {
                    textMagVar.BackColor = Color.White;
                    systSet.dMagneticVar = double.Parse(textMagVar.Text);
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
            CheckText(ref systSet);
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
        private void textMagVar_TextChanged(object sender, EventArgs e)
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
        private void textBoxBTSNR_TextChanged(object sender, EventArgs e)
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
        private System.Collections.Queue defaultQueue;
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

        private bool GetParameters(ref string strpack, string Command)
        {
            strpack = "";
            defaultQueue = new System.Collections.Queue();
            //sp.PortName = "COM18";
            //sp.BaudRate = 921600;

            try
            {
                sp.Close();
                sp.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            sp.DataReceived += new SerialDataReceivedEventHandler(SP_DataReceived);


            bool OK = false;
            if (sp.IsOpen)
            {
                //sp.Write("CRSSHOW\r");
                sp.Write(Command);
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
            return OK;
        }

        private void DownloadCommandSettings()
        {
            GetFlowCommands();
            GetBackscatterCommands();
        }
        private void GetBackscatterCommands()
        {
            string strpack = "";
            string Command = "CBSSHOW\r";

            bool OK = GetParameters(ref strpack, Command);

            if (OK)
            {
                //Int32 index = 0;
                int i;
                double dVal;
                string[] separatingStrings = { " ", "," };

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
                                case "CBSBEAMON":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count; i++)
                                        {
                                            Int32 n = Convert.ToInt32(cmdPart[i]);

                                            switch (j)
                                            {
                                                case 0:
                                                    checkBoxEnable0.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable0.Checked = true;
                                                    else
                                                        checkBoxEnable0.Checked = false;
                                                    break;
                                                case 1:
                                                    checkBoxEnable1.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable1.Checked = true;
                                                    else
                                                        checkBoxEnable1.Checked = false;
                                                    break;
                                                case 2:
                                                    checkBoxEnable2.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable2.Checked = true;
                                                    else
                                                        checkBoxEnable2.Checked = false;
                                                    break;
                                                case 3:
                                                    checkBoxEnable3.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable3.Checked = true;
                                                    else
                                                        checkBoxEnable3.Checked = false;
                                                    break;
                                                case 4:
                                                    checkBoxEnable4.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable4.Checked = true;
                                                    else
                                                        checkBoxEnable4.Checked = false;
                                                    break;
                                                case 5:
                                                    checkBoxEnable5.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable5.Checked = true;
                                                    else
                                                        checkBoxEnable5.Checked = false;
                                                    break;
                                                case 6:
                                                    checkBoxEnable6.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable6.Checked = true;
                                                    else
                                                        checkBoxEnable6.Checked = false;
                                                    break;
                                                case 7:
                                                    checkBoxEnable7.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable7.Checked = true;
                                                    else
                                                        checkBoxEnable7.Checked = false;
                                                    break;
                                                case 8:
                                                    checkBoxEnable8.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable8.Checked = true;
                                                    else
                                                        checkBoxEnable8.Checked = false;
                                                    break;
                                                case 9:
                                                    checkBoxEnable9.Enabled = true;
                                                    if (n == 1)
                                                        checkBoxEnable9.Checked = true;
                                                    else
                                                        checkBoxEnable9.Checked = false;
                                                    break;
                                            }
                                            j++;
                                        }

                                        for(;j<10;j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    checkBoxEnable0.Checked = false;
                                                    checkBoxEnable0.Enabled = false;
                                                    break;
                                                case 1:
                                                    checkBoxEnable1.Checked = false;
                                                    checkBoxEnable1.Enabled = false;
                                                    break;
                                                case 2:
                                                    checkBoxEnable2.Checked = false;
                                                    checkBoxEnable2.Enabled = false;
                                                    break;
                                                case 3:
                                                    checkBoxEnable3.Checked = false;
                                                    checkBoxEnable3.Enabled = false;
                                                    break;
                                                case 4:
                                                    checkBoxEnable4.Checked = false;
                                                    checkBoxEnable4.Enabled = false;
                                                    break;
                                                case 5:
                                                    checkBoxEnable5.Checked = false;
                                                    checkBoxEnable5.Enabled = false;
                                                    break;
                                                case 6:
                                                    checkBoxEnable6.Checked = false;
                                                    checkBoxEnable6.Enabled = false;
                                                    break;
                                                case 7:
                                                    checkBoxEnable7.Checked = false;
                                                    checkBoxEnable7.Enabled = false;                                                    
                                                    break;
                                                case 8:
                                                    checkBoxEnable8.Checked = false;
                                                    checkBoxEnable8.Enabled = false;
                                                    break;
                                                case 9:
                                                    checkBoxEnable9.Checked = false;
                                                    checkBoxEnable9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "Frequency":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count-1; i++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxFreq0.Enabled = true;
                                                    textBoxFreq0.Text = cmdPart[i];
                                                    break;
                                                case 1:
                                                    textBoxFreq1.Enabled = true;
                                                    textBoxFreq1.Text = cmdPart[i];
                                                    break;
                                                case 2:
                                                    textBoxFreq2.Enabled = true;
                                                    textBoxFreq2.Text = cmdPart[i];
                                                    break;
                                                case 3:
                                                    textBoxFreq3.Enabled = true;
                                                    textBoxFreq3.Text = cmdPart[i];
                                                    break;
                                                case 4:
                                                    textBoxFreq4.Enabled = true;
                                                    textBoxFreq4.Text = cmdPart[i];
                                                    break;
                                                case 5:
                                                    textBoxFreq5.Enabled = true;
                                                    textBoxFreq5.Text = cmdPart[i];
                                                    break;
                                                case 6:
                                                    textBoxFreq6.Enabled = true;
                                                    textBoxFreq6.Text = cmdPart[i];
                                                    break;
                                                case 7:
                                                    textBoxFreq7.Enabled = true;
                                                    textBoxFreq7.Text = cmdPart[i];
                                                    break;
                                                case 8:
                                                    textBoxFreq8.Enabled = true;
                                                    textBoxFreq8.Text = cmdPart[i];
                                                    break;
                                                case 9:
                                                    textBoxFreq9.Enabled = true;
                                                    textBoxFreq9.Text = cmdPart[i];
                                                    break;
                                            }
                                            j++;
                                        }

                                        for (; j < 10; j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxFreq0.Text = "";
                                                    textBoxFreq0.Enabled = false;
                                                    break;
                                                case 1:
                                                    textBoxFreq1.Text = "";
                                                    textBoxFreq1.Enabled = false;
                                                    break;
                                                case 2:
                                                    textBoxFreq2.Text = "";
                                                    textBoxFreq2.Enabled = false;
                                                    break;
                                                case 3:
                                                    textBoxFreq3.Text = "";
                                                    textBoxFreq3.Enabled = false;
                                                    break;
                                                case 4:
                                                    textBoxFreq4.Text = "";
                                                    textBoxFreq4.Enabled = false;
                                                    break;
                                                case 5:
                                                    textBoxFreq5.Text = "";
                                                    textBoxFreq5.Enabled = false;
                                                    break;
                                                case 6:
                                                    textBoxFreq6.Text = "";
                                                    textBoxFreq6.Enabled = false;
                                                    break;
                                                case 7:
                                                    textBoxFreq7.Text = "";
                                                    textBoxFreq7.Enabled = false;
                                                    break;
                                                case 8:
                                                    textBoxFreq8.Text = "";
                                                    textBoxFreq8.Enabled = false;
                                                    break;
                                                case 9:
                                                    textBoxFreq9.Text = "";
                                                    textBoxFreq9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "CBSBINS":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count; i++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxBins0.Enabled = true;
                                                    textBoxBins0.Text = cmdPart[i];
                                                    break;
                                                case 1:
                                                    textBoxBins1.Enabled = true;
                                                    textBoxBins1.Text = cmdPart[i];
                                                    break;
                                                case 2:
                                                    textBoxBins2.Enabled = true;
                                                    textBoxBins2.Text = cmdPart[i];
                                                    break;
                                                case 3:
                                                    textBoxBins3.Enabled = true;
                                                    textBoxBins3.Text = cmdPart[i];
                                                    break;
                                                case 4:
                                                    textBoxBins4.Enabled = true;
                                                    textBoxBins4.Text = cmdPart[i];
                                                    break;
                                                case 5:
                                                    textBoxBins5.Enabled = true;
                                                    textBoxBins5.Text = cmdPart[i];
                                                    break;
                                                case 6:
                                                    textBoxBins6.Enabled = true;
                                                    textBoxBins6.Text = cmdPart[i];
                                                    break;
                                                case 7:
                                                    textBoxBins7.Enabled = true;
                                                    textBoxBins7.Text = cmdPart[i];
                                                    break;
                                                case 8:
                                                    textBoxBins8.Enabled = true;
                                                    textBoxBins8.Text = cmdPart[i];
                                                    break;
                                                case 9:
                                                    textBoxBins9.Enabled = true;
                                                    textBoxBins9.Text = cmdPart[i];
                                                    break;
                                            }
                                            j++;
                                        }

                                        for (; j < 10; j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxBins0.Text = "";
                                                    textBoxBins0.Enabled = false;
                                                    break;
                                                case 1:
                                                    textBoxBins1.Text = "";
                                                    textBoxBins1.Enabled = false;
                                                    break;
                                                case 2:
                                                    textBoxBins2.Text = "";
                                                    textBoxBins2.Enabled = false;
                                                    break;
                                                case 3:
                                                    textBoxBins3.Text = "";
                                                    textBoxBins3.Enabled = false;
                                                    break;
                                                case 4:
                                                    textBoxBins4.Text = "";
                                                    textBoxBins4.Enabled = false;
                                                    break;
                                                case 5:
                                                    textBoxBins5.Text = "";
                                                    textBoxBins5.Enabled = false;
                                                    break;
                                                case 6:
                                                    textBoxBins6.Text = "";
                                                    textBoxBins6.Enabled = false;
                                                    break;
                                                case 7:
                                                    textBoxBins7.Text = "";
                                                    textBoxBins7.Enabled = false;
                                                    break;
                                                case 8:
                                                    textBoxBins8.Text = "";
                                                    textBoxBins8.Enabled = false;
                                                    break;
                                                case 9:
                                                    textBoxBins9.Text = "";
                                                    textBoxBins9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "CBSTBP":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count - 1; i++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxTBP0.Enabled = true;
                                                    textBoxTBP0.Text = cmdPart[i];
                                                    break;
                                                case 1:
                                                    textBoxTBP1.Enabled = true;
                                                    textBoxTBP1.Text = cmdPart[i];
                                                    break;
                                                case 2:
                                                    textBoxTBP2.Enabled = true;
                                                    textBoxTBP2.Text = cmdPart[i];
                                                    break;
                                                case 3:
                                                    textBoxTBP3.Enabled = true;
                                                    textBoxTBP3.Text = cmdPart[i];
                                                    break;
                                                case 4:
                                                    textBoxTBP4.Enabled = true;
                                                    textBoxTBP4.Text = cmdPart[i];
                                                    break;
                                                case 5:
                                                    textBoxTBP5.Enabled = true;
                                                    textBoxTBP5.Text = cmdPart[i];
                                                    break;
                                                case 6:
                                                    textBoxTBP6.Enabled = true;
                                                    textBoxTBP6.Text = cmdPart[i];
                                                    break;
                                                case 7:
                                                    textBoxTBP7.Enabled = true;
                                                    textBoxTBP7.Text = cmdPart[i];
                                                    break;
                                                case 8:
                                                    textBoxTBP8.Enabled = true;
                                                    textBoxTBP8.Text = cmdPart[i];
                                                    break;
                                                case 9:
                                                    textBoxTBP9.Enabled = true;
                                                    textBoxTBP9.Text = cmdPart[i];
                                                    break;
                                            }
                                            j++;
                                        }

                                        for (; j < 10; j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxTBP0.Text = "";
                                                    textBoxTBP0.Enabled = false;
                                                    break;
                                                case 1:
                                                    textBoxTBP1.Text = "";
                                                    textBoxTBP1.Enabled = false;
                                                    break;
                                                case 2:
                                                    textBoxTBP2.Text = "";
                                                    textBoxTBP2.Enabled = false;
                                                    break;
                                                case 3:
                                                    textBoxTBP3.Text = "";
                                                    textBoxTBP3.Enabled = false;
                                                    break;
                                                case 4:
                                                    textBoxTBP4.Text = "";
                                                    textBoxTBP4.Enabled = false;
                                                    break;
                                                case 5:
                                                    textBoxTBP5.Text = "";
                                                    textBoxTBP5.Enabled = false;
                                                    break;
                                                case 6:
                                                    textBoxTBP6.Text = "";
                                                    textBoxTBP6.Enabled = false;
                                                    break;
                                                case 7:
                                                    textBoxTBP7.Text = "";
                                                    textBoxTBP7.Enabled = false;
                                                    break;
                                                case 8:
                                                    textBoxTBP8.Text = "";
                                                    textBoxTBP8.Enabled = false;
                                                    break;
                                                case 9:
                                                    textBoxTBP9.Text = "";
                                                    textBoxTBP9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "CBSBLANK":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count - 1; i++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxBlank0.Enabled = true;
                                                    textBoxBlank0.Text = cmdPart[i];
                                                    break;
                                                case 1:
                                                    textBoxBlank1.Enabled = true;
                                                    textBoxBlank1.Text = cmdPart[i];
                                                    break;
                                                case 2:
                                                    textBoxBlank2.Enabled = true;
                                                    textBoxBlank2.Text = cmdPart[i];
                                                    break;
                                                case 3:
                                                    textBoxBlank3.Enabled = true;
                                                    textBoxBlank3.Text = cmdPart[i];
                                                    break;
                                                case 4:
                                                    textBoxBlank4.Enabled = true;
                                                    textBoxBlank4.Text = cmdPart[i];
                                                    break;
                                                case 5:
                                                    textBoxBlank5.Enabled = true;
                                                    textBoxBlank5.Text = cmdPart[i];
                                                    break;
                                                case 6:
                                                    textBoxBlank6.Enabled = true;
                                                    textBoxBlank6.Text = cmdPart[i];
                                                    break;
                                                case 7:
                                                    textBoxBlank7.Enabled = true;
                                                    textBoxBlank7.Text = cmdPart[i];
                                                    break;
                                                case 8:
                                                    textBoxBlank8.Enabled = true;
                                                    textBoxBlank8.Text = cmdPart[i];
                                                    break;
                                                case 9:
                                                    textBoxBlank9.Enabled = true;
                                                    textBoxBlank9.Text = cmdPart[i];
                                                    break;
                                            }
                                            j++;
                                        }

                                        for (; j < 10; j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxBlank0.Text = "";
                                                    textBoxBlank0.Enabled = false;
                                                    break;
                                                case 1:
                                                    textBoxBlank1.Text = "";
                                                    textBoxBlank1.Enabled = false;
                                                    break;
                                                case 2:
                                                    textBoxBlank2.Text = "";
                                                    textBoxBlank2.Enabled = false;
                                                    break;
                                                case 3:
                                                    textBoxBlank3.Text = "";
                                                    textBoxBlank3.Enabled = false;
                                                    break;
                                                case 4:
                                                    textBoxBlank4.Text = "";
                                                    textBoxBlank4.Enabled = false;
                                                    break;
                                                case 5:
                                                    textBoxBlank5.Text = "";
                                                    textBoxBlank5.Enabled = false;
                                                    break;
                                                case 6:
                                                    textBoxBlank6.Text = "";
                                                    textBoxBlank6.Enabled = false;
                                                    break;
                                                case 7:
                                                    textBoxBlank7.Text = "";
                                                    textBoxBlank7.Enabled = false;
                                                    break;
                                                case 8:
                                                    textBoxBlank8.Text = "";
                                                    textBoxBlank8.Enabled = false;
                                                    break;
                                                case 9:
                                                    textBoxBlank9.Text = "";
                                                    textBoxBlank9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "CBSBINSIZE":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count - 1; i++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxBinSize0.Enabled = true;
                                                    textBoxBinSize0.Text = cmdPart[i];
                                                    break;
                                                case 1:
                                                    textBoxBinSize1.Enabled = true;
                                                    textBoxBinSize1.Text = cmdPart[i];
                                                    break;
                                                case 2:
                                                    textBoxBinSize2.Enabled = true;
                                                    textBoxBinSize2.Text = cmdPart[i];
                                                    break;
                                                case 3:
                                                    textBoxBinSize3.Enabled = true;
                                                    textBoxBinSize3.Text = cmdPart[i];
                                                    break;
                                                case 4:
                                                    textBoxBinSize4.Enabled = true;
                                                    textBoxBinSize4.Text = cmdPart[i];
                                                    break;
                                                case 5:
                                                    textBoxBinSize5.Enabled = true;
                                                    textBoxBinSize5.Text = cmdPart[i];
                                                    break;
                                                case 6:
                                                    textBoxBinSize6.Enabled = true;
                                                    textBoxBinSize6.Text = cmdPart[i];
                                                    break;
                                                case 7:
                                                    textBoxBinSize7.Enabled = true;
                                                    textBoxBinSize7.Text = cmdPart[i];
                                                    break;
                                                case 8:
                                                    textBoxBinSize8.Enabled = true;
                                                    textBoxBinSize8.Text = cmdPart[i];
                                                    break;
                                                case 9:
                                                    textBoxBinSize9.Enabled = true;
                                                    textBoxBinSize9.Text = cmdPart[i];
                                                    break;
                                            }
                                            j++;
                                        }

                                        for (; j < 10; j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxBinSize0.Text = "";
                                                    textBoxBinSize0.Enabled = false;
                                                    break;
                                                case 1:
                                                    textBoxBinSize1.Text = "";
                                                    textBoxBinSize1.Enabled = false;
                                                    break;
                                                case 2:
                                                    textBoxBinSize2.Text = "";
                                                    textBoxBinSize2.Enabled = false;
                                                    break;
                                                case 3:
                                                    textBoxBinSize3.Text = "";
                                                    textBoxBinSize3.Enabled = false;
                                                    break;
                                                case 4:
                                                    textBoxBinSize4.Text = "";
                                                    textBoxBinSize4.Enabled = false;
                                                    break;
                                                case 5:
                                                    textBoxBinSize5.Text = "";
                                                    textBoxBinSize5.Enabled = false;
                                                    break;
                                                case 6:
                                                    textBoxBinSize6.Text = "";
                                                    textBoxBinSize6.Enabled = false;
                                                    break;
                                                case 7:
                                                    textBoxBinSize7.Text = "";
                                                    textBoxBinSize7.Enabled = false;
                                                    break;
                                                case 8:
                                                    textBoxBinSize8.Text = "";
                                                    textBoxBinSize8.Enabled = false;
                                                    break;
                                                case 9:
                                                    textBoxBinSize9.Text = "";
                                                    textBoxBinSize9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "CBSXMTLENGTH":
                                    try
                                    {
                                        int j = 0;
                                        int count = cmdPart.Count();
                                        for (i = 1; i < count - 1; i++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxXmt0.Enabled = true;
                                                    textBoxXmt0.Text = cmdPart[i];
                                                    break;
                                                case 1:
                                                    textBoxXmt1.Enabled = true;
                                                    textBoxXmt1.Text = cmdPart[i];
                                                    break;
                                                case 2:
                                                    textBoxXmt2.Enabled = true;
                                                    textBoxXmt2.Text = cmdPart[i];
                                                    break;
                                                case 3:
                                                    textBoxXmt3.Enabled = true;
                                                    textBoxXmt3.Text = cmdPart[i];
                                                    break;
                                                case 4:
                                                    textBoxXmt4.Enabled = true;
                                                    textBoxXmt4.Text = cmdPart[i];
                                                    break;
                                                case 5:
                                                    textBoxXmt5.Enabled = true;
                                                    textBoxXmt5.Text = cmdPart[i];
                                                    break;
                                                case 6:
                                                    textBoxXmt6.Enabled = true;
                                                    textBoxXmt6.Text = cmdPart[i];
                                                    break;
                                                case 7:
                                                    textBoxXmt7.Enabled = true;
                                                    textBoxXmt7.Text = cmdPart[i];
                                                    break;
                                                case 8:
                                                    textBoxXmt8.Enabled = true;
                                                    textBoxXmt8.Text = cmdPart[i];
                                                    break;
                                                case 9:
                                                    textBoxXmt9.Enabled = true;
                                                    textBoxXmt9.Text = cmdPart[i];
                                                    break;
                                            }
                                            j++;
                                        }

                                        for (; j < 10; j++)
                                        {
                                            switch (j)
                                            {
                                                case 0:
                                                    textBoxXmt0.Text = "";
                                                    textBoxXmt0.Enabled = false;
                                                    break;
                                                case 1:
                                                    textBoxXmt1.Text = "";
                                                    textBoxXmt1.Enabled = false;
                                                    break;
                                                case 2:
                                                    textBoxXmt2.Text = "";
                                                    textBoxXmt2.Enabled = false;
                                                    break;
                                                case 3:
                                                    textBoxXmt3.Text = "";
                                                    textBoxXmt3.Enabled = false;
                                                    break;
                                                case 4:
                                                    textBoxXmt4.Text = "";
                                                    textBoxXmt4.Enabled = false;
                                                    break;
                                                case 5:
                                                    textBoxXmt5.Text = "";
                                                    textBoxXmt5.Enabled = false;
                                                    break;
                                                case 6:
                                                    textBoxXmt6.Text = "";
                                                    textBoxXmt6.Enabled = false;
                                                    break;
                                                case 7:
                                                    textBoxXmt7.Text = "";
                                                    textBoxXmt7.Enabled = false;
                                                    break;
                                                case 8:
                                                    textBoxXmt8.Text = "";
                                                    textBoxXmt8.Enabled = false;
                                                    break;
                                                case 9:
                                                    textBoxXmt9.Text = "";
                                                    textBoxXmt9.Enabled = false;
                                                    break;
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                case "CBSPINGS":
                                    try
                                    {
                                        textBoxBSpings.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CBSPH":
                                    try
                                    {
                                        textBoxPH.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CWSSC":
                                    try
                                    {
                                        Int32 index = Convert.ToInt32(cmdPart[1]);
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
                                case "CBSDEPTH":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textBoxTransducerDepth.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CBSSALINITY":
                                    try
                                    {
                                        textWaterSalinity.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CBSTEMP":
                                    try
                                    {
                                        textWaterTemperature.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CWSS":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textSoundSpeed.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CHO":
                                    try
                                    {
                                        textHeadingOffset.Text = cmdPart[1];
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
        private void GetFlowCommands()
        {
            string strpack = "";
            string Command = "CRSSHOW\r";

            bool OK = GetParameters(ref strpack, Command);

            if (OK)
            {
                Int32 index = 0;
                double dVal;
                string[] separatingStrings = { " ", "," };

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
                                        textBoxNumberOfBins.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CRSWPAI":
                                    try
                                    {
                                        textBoxWPaveragingInterval.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CRSMAXDEPTH":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textBoxMaxDepth.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CRSWPSWITCH":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textBoxWPswitchDepth.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CRSBTSWITCH":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textBoxBTswitchDepth.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CRSXDCRDEPTH":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textBoxTransducerDepth.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CRSSALINITY":
                                    try
                                    {
                                        textWaterSalinity.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CRSTEMP":
                                    try
                                    {
                                        textWaterTemperature.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CRSBTSNR":
                                    try
                                    {
                                        textBoxBTSNR.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CRSBTCOR":
                                    try
                                    {
                                        BTST_Correlation_text.Text = cmdPart[1];
                                    }
                                    catch { }
                                    break;
                                case "CWSS":
                                    try
                                    {
                                        dVal = Convert.ToDouble(cmdPart[1]);
                                        if (systSet.bEnglishUnit)
                                            dVal = projectUnit.MeterToFeet(dVal, 1);
                                        textSoundSpeed.Text = dVal.ToString("0.000");
                                    }
                                    catch { }
                                    break;
                                case "CHO":
                                    try
                                    {
                                        textHeadingOffset.Text = cmdPart[1];
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
        private void buttonDownloadCommandSettings_Click(object sender, EventArgs e)
        {
            DownloadCommandSettings();

        }
        private Units projectUnit = new Units();
        /*
        private void WriteCommandSettings(SystemSetting systSet)
        {
            string CMD = "CRSMODE " + systSet.iMeasurmentMode.ToString() + "\r\n";
            CMD += "CRSVB " + systSet.iVerticalBeam.ToString() + "\r\n";
            CMD += "CRSAUTOBIN " + systSet.iAutoBinSize.ToString() + "\r\n";
            CMD += "CRSAUTOLAG " + systSet.iAutoLag.ToString() + "\r\n";
            CMD += "CRSWPBN " + systSet.iBins.ToString() + "\r\n";
            CMD += "CRSWPAI " + systSet.dAveragingInterval.ToString() + "\r\n";
            CMD += "CRSMAXDEPTH " + systSet.dMaxMeasurementDepth.ToString() + "\r\n";
            CMD += "CRSWPSWITCH " + systSet.dWpSwitchDepth.ToString() + "\r\n";
            CMD += "CRSBTSWITCH " + systSet.dBtSwitchDepth.ToString() + "\r\n";
            if (systSet.bEnglishUnit)
            {
                CMD += "CRSXDCRDEPTH " + projectUnit.FeetToMeter(systSet.dTransducerDepth, 1).ToString() + "\r\n";
            }
            else
            {
                CMD += "CRSXDCRDEPTH " + systSet.dTransducerDepth.ToString() + "\r\n";
            }
            CMD += "CWSSC " + systSet.iWaterTemperatureSource.ToString();
            CMD += "," + systSet.iTransducerDepthSource.ToString();
            CMD += "," + systSet.iSalinitySource.ToString();
            CMD += "," + systSet.iSpeedOfSoundSource.ToString();
            CMD += "\r\n";
            if (systSet.bEnglishUnit)
            {
                CMD += "CWSS " + projectUnit.FeetToMeter(systSet.dSpeedOfSound, 1).ToString() + "\r\n";
            }
            else
            {
                CMD += "CWSS " + systSet.dSpeedOfSound.ToString() + "\r\n";
            }
            CMD += "CRSSALINITY " + systSet.dSalinity.ToString() + "\r\n";
            CMD += "CRSTEMP " + systSet.dWaterTemperature.ToString() + "\r\n";
            CMD += "CRSBTSNR " + systSet.dBtSNR.ToString() + "\r\n";
            CMD += "CRSBTCOR " + systSet.dBtCorrelationThreshold.ToString() + "\r\n";
            CMD += "CHO " + systSet.dHeadingOffset.ToString() + "\r\n";
            CMD += "CHS " + systSet.iHeadingRef.ToString() + "\r\n";
            CMD += "C232B " + systSet.strRS232 + "\r\n";

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
        */
        private void buttonSetDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Close();
                sp.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //sp.DataReceived += new SerialDataReceivedEventHandler(SP_DataReceived);

            if (sp.IsOpen)
            {
                sp.Write("MODERIVER\r");
                Thread.Sleep(1000);
                sp.Close();
            }
            DownloadCommandSettings();
        }

        //private void WriteIt()
        //{
        //    WriteCommandSettings(systSet);
        //}
        private void btnOK_Click(object sender, EventArgs e)
        {
            //WriteIt();
            theList = "";
            try
            {
                int pings = int.Parse(textBoxBSpings.Text);

                theList += "CBSPINGS " + textBoxBSpings.Text + "\r\n";

                if (pings > 0)
                {
                    theList += "CBSDEPTH " + textBoxTransducerDepth.Text + "\r\n";
                    theList += "CBSSALINITY " + textWaterSalinity.Text + "\r\n";
                    theList += "CBSTEMP " + textWaterTemperature.Text + "\r\n";
                    theList += "CBSPH " + textBoxPH.Text + "\r\n";

                    theList += "CBSBEAMON ";
                    if (checkBoxEnable0.Checked)
                        theList += "1";
                    else
                        theList += "0";
                    if (checkBoxEnable1.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable2.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable3.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable4.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable5.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable6.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable7.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable8.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    if (checkBoxEnable9.Checked)
                        theList += ",1";
                    else
                        theList += ",0";
                    theList += "\r\n";

                    theList += "CBSBINS " + textBoxBins0.Text;
                    theList += "," + textBoxBins1.Text;
                    theList += "," + textBoxBins2.Text;
                    theList += "," + textBoxBins3.Text;
                    theList += "," + textBoxBins4.Text;
                    theList += "," + textBoxBins5.Text;
                    theList += "," + textBoxBins6.Text;
                    theList += "," + textBoxBins7.Text;
                    theList += "," + textBoxBins8.Text;
                    theList += "," + textBoxBins9.Text;
                    theList += "\r\n";

                    theList += "CBSTBP " + textBoxTBP0.Text;
                    theList += "," + textBoxTBP1.Text;
                    theList += "," + textBoxTBP2.Text;
                    theList += "," + textBoxTBP3.Text;
                    theList += "," + textBoxTBP4.Text;
                    theList += "," + textBoxTBP5.Text;
                    theList += "," + textBoxTBP6.Text;
                    theList += "," + textBoxTBP7.Text;
                    theList += "," + textBoxTBP8.Text;
                    theList += "," + textBoxTBP9.Text;
                    theList += "\r\n";

                    theList += "CBSBLANK " + textBoxBlank0.Text;
                    theList += "," + textBoxBlank1.Text;
                    theList += "," + textBoxBlank2.Text;
                    theList += "," + textBoxBlank3.Text;
                    theList += "," + textBoxBlank4.Text;
                    theList += "," + textBoxBlank5.Text;
                    theList += "," + textBoxBlank6.Text;
                    theList += "," + textBoxBlank7.Text;
                    theList += "," + textBoxBlank8.Text;
                    theList += "," + textBoxBlank9.Text;
                    theList += "\r\n";

                    theList += "CBSBINSIZE " + textBoxBinSize0.Text;
                    theList += "," + textBoxBinSize1.Text;
                    theList += "," + textBoxBinSize2.Text;
                    theList += "," + textBoxBinSize3.Text;
                    theList += "," + textBoxBinSize4.Text;
                    theList += "," + textBoxBinSize5.Text;
                    theList += "," + textBoxBinSize6.Text;
                    theList += "," + textBoxBinSize7.Text;
                    theList += "," + textBoxBinSize8.Text;
                    theList += "," + textBoxBinSize9.Text;
                    theList += "\r\n";

                    theList += "CBSXMTLENGTH " + textBoxXmt0.Text;
                    theList += "," + textBoxXmt1.Text;
                    theList += "," + textBoxXmt2.Text;
                    theList += "," + textBoxXmt3.Text;
                    theList += "," + textBoxXmt4.Text;
                    theList += "," + textBoxXmt5.Text;
                    theList += "," + textBoxXmt6.Text;
                    theList += "," + textBoxXmt7.Text;
                    theList += "," + textBoxXmt8.Text;
                    theList += "," + textBoxXmt9.Text;
                    theList += "\r\n";
                }
            }
            catch { }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            theList = "";
        }
        private void textBoxBSpings_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBSpings.Text, 7))
            {
                textBoxBSpings.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBSpings.Text) >= 0) && (int.Parse(textBoxBSpings.Text) < 11))
                {
                    textBoxBSpings.BackColor = Color.White;
                }
                else
                {
                    textBoxBSpings.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins0_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins0.Text, 7))
            {
                textBoxBins0.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins0.Text) >= 0) && (int.Parse(textBoxBins0.Text) < 256))
                {
                    textBoxBins0.BackColor = Color.White;
                }
                else
                {
                    textBoxBins0.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins1_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins1.Text, 7))
            {
                textBoxBins1.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins1.Text) >= 0) && (int.Parse(textBoxBins1.Text) < 256))
                {
                    textBoxBins1.BackColor = Color.White;
                }
                else
                {
                    textBoxBins1.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins2_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins2.Text, 7))
            {
                textBoxBins2.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins2.Text) >= 0) && (int.Parse(textBoxBins2.Text) < 256))
                {
                    textBoxBins2.BackColor = Color.White;
                }
                else
                {
                    textBoxBins2.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins3_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins3.Text, 7))
            {
                textBoxBins3.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins3.Text) >= 0) && (int.Parse(textBoxBins3.Text) < 256))
                {
                    textBoxBins3.BackColor = Color.White;
                }
                else
                {
                    textBoxBins3.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins4_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins4.Text, 7))
            {
                textBoxBins4.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins4.Text) >= 0) && (int.Parse(textBoxBins4.Text) < 256))
                {
                    textBoxBins4.BackColor = Color.White;
                }
                else
                {
                    textBoxBins4.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins5_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins5.Text, 7))
            {
                textBoxBins5.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins5.Text) >= 0) && (int.Parse(textBoxBins5.Text) < 256))
                {
                    textBoxBins5.BackColor = Color.White;
                }
                else
                {
                    textBoxBins5.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins6_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins6.Text, 7))
            {
                textBoxBins6.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins6.Text) >= 0) && (int.Parse(textBoxBins6.Text) < 256))
                {
                    textBoxBins6.BackColor = Color.White;
                }
                else
                {
                    textBoxBins6.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins7_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins7.Text, 7))
            {
                textBoxBins7.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins7.Text) >= 0) && (int.Parse(textBoxBins7.Text) < 256))
                {
                    textBoxBins7.BackColor = Color.White;
                }
                else
                {
                    textBoxBins7.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins8_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins8.Text, 7))
            {
                textBoxBins8.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins8.Text) >= 0) && (int.Parse(textBoxBins8.Text) < 256))
                {
                    textBoxBins8.BackColor = Color.White;
                }
                else
                {
                    textBoxBins8.BackColor = Color.Red;
                }
            }
        }

        private void textBoxBins9_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxBins9.Text, 7))
            {
                textBoxBins9.BackColor = Color.Red;
            }
            else
            {
                if ((int.Parse(textBoxBins9.Text) >= 0) && (int.Parse(textBoxBins9.Text) < 256))
                {
                    textBoxBins9.BackColor = Color.White;
                }
                else
                {
                    textBoxBins9.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP0_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP0.Text, 7))
            {
                textBoxTBP0.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP0.Text) >= 0) && (double.Parse(textBoxTBP0.Text) <= 1.0))
                {
                    textBoxTBP0.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP0.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP1_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP1.Text, 7))
            {
                textBoxTBP1.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP1.Text) >= 0) && (double.Parse(textBoxTBP1.Text) <= 1.0))
                {
                    textBoxTBP1.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP1.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP2_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP2.Text, 7))
            {
                textBoxTBP2.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP2.Text) >= 0) && (double.Parse(textBoxTBP2.Text) <= 1.0))
                {
                    textBoxTBP2.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP2.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP3_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP3.Text, 7))
            {
                textBoxTBP3.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP3.Text) >= 0) && (double.Parse(textBoxTBP3.Text) <= 1.0))
                {
                    textBoxTBP3.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP3.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP4_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP4.Text, 7))
            {
                textBoxTBP4.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP4.Text) >= 0) && (double.Parse(textBoxTBP4.Text) <= 1.0))
                {
                    textBoxTBP4.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP4.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP5_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP5.Text, 7))
            {
                textBoxTBP5.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP5.Text) >= 0) && (double.Parse(textBoxTBP5.Text) <= 1.0))
                {
                    textBoxTBP5.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP5.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP6_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP6.Text, 7))
            {
                textBoxTBP6.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP6.Text) >= 0) && (double.Parse(textBoxTBP6.Text) <= 1.0))
                {
                    textBoxTBP6.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP6.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP7_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP7.Text, 7))
            {
                textBoxTBP7.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP7.Text) >= 0) && (double.Parse(textBoxTBP7.Text) <= 1.0))
                {
                    textBoxTBP7.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP7.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP8_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP8.Text, 7))
            {
                textBoxTBP8.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP8.Text) >= 0) && (double.Parse(textBoxTBP8.Text) <= 1.0))
                {
                    textBoxTBP8.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP8.BackColor = Color.Red;
                }
            }
        }

        private void textBoxTBP9_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxTBP9.Text, 7))
            {
                textBoxTBP9.BackColor = Color.Red;
            }
            else
            {
                if ((double.Parse(textBoxTBP9.Text) >= 0) && (double.Parse(textBoxTBP9.Text) <= 1.0))
                {
                    textBoxTBP9.BackColor = Color.White;
                }
                else
                {
                    textBoxTBP9.BackColor = Color.Red;
                }
            }
        }
    }
}
