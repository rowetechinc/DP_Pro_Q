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
    public partial class FrmSystemSetting : Form
    {
        public FrmSystemSetting(SystemSetting systSet, bool bPlayback, SerialPort _sp)
        {
            InitializeComponent();

            bPlayBackMode = bPlayback; //LPJ 2014-2-19

            sp = _sp; //LPJ 2016-4-8

            if (bPlayback)  //LPJ 2013-11-20 当回放模式时，只能修改船速参考和换能器深度
            {
                //comboBoxHeadingRef.Enabled = false; //LPJ2016-8-16 回放时，艏向参考可更改
                comboBoxMeasMode.Enabled = false;
                //comboBoxStandardMode.Enabled = false;
                textHeadingOffset.Enabled = false; //LPJ 2014-6-16
                textWaterSalinity.Enabled = false; //LPJ 2014-6-16

                comboBox_RS232.Enabled = false;
            }

            comboBox_RS232.SelectedIndex = 5;

            GetReference(systSet);
        }
        private SerialPort sp; //LPJ 2016-5-19

        
        private bool bPlayBackMode = false; //标记是否为回放模式 //Whether the mark is in playback mode

        private bool bEnglish = false;
        private void GetReference(SystemSetting systSet)
        {
            bEnglish = systSet.bEnglishUnit;
            
            comboBoxHeadingRef.SelectedIndex = systSet.iHeadingRef;
            comboBoxVesselSpeedRef.SelectedIndex = systSet.iSpeedRef;
            textWaterSalinity.Text = systSet.dSalinity.ToString(); //LPJ 2014-6-16
            textHeadingOffset.Text = systSet.dHeadingOffset.ToString(); //LPJ 2014-6-16
            textBoxTransducerDepth.Text = systSet.dTransducerDepth.ToString();
            
      
            if (systSet.bEnglishUnit)
            {
                label6.Text = "(ft)";
                label16.Text = "(ft)";
            }
            else
            {
                label6.Text = "(m)";
                label16.Text = "(m)";
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            //systemSet.iFlowRef = comboBoxFlowRef.SelectedIndex;
            systemSet.iHeadingRef = comboBoxHeadingRef.SelectedIndex;
            systemSet.dHeadingOffset = double.Parse(textHeadingOffset.Text);
            systemSet.strRS232 = comboBox_RS232.Text;

            systemSet.iSpeedRef = comboBoxVesselSpeedRef.SelectedIndex;

            //Bottom Tracking
            //GPS VTG
            //No Reference
            //GPS GGA

            systemSet.dSalinity = double.Parse(textWaterSalinity.Text); //LPJ 2014-6-16
            

            systemSet.dTransducerDepth = double.Parse(textBoxTransducerDepth.Text);

            systemSet.dSpeedOfSound = double.Parse(textSoundSpeed.Text);



        }

        public static SystemSetting systemSet;
        public struct SystemSetting
        {
            //public int iFlowRef;
            public int iSpeedRef;
            
            public int iHeadingRef;
            public string strRS232;

            public double dSalinity;
            public double dWaterTemperature;
            public double dSpeedOfSound;
            public double dTransducerDepth;

            public double dMaxMeasurmentDepth;
            public double dHeadingOffset;

            public bool bEnglishUnit;
            public int iInstrumentTypes;
        }
 
        ClassValidateInPut validateInput = new ClassValidateInPut();

        private void TheTextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            //
            if (!validateInput.ValidateUserInput(textBoxTransducerDepth.Text, 10))
            {
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Show(Resource1.String76, textBoxTransducerDepth);
                btnOK.Enabled = false;
                textBoxTransducerDepth.BackColor = Color.Red;
            }
            else
            {
                textBoxTransducerDepth.BackColor = Color.White;
                systemSet.dTransducerDepth = double.Parse(textBoxTransducerDepth.Text);
            }
            //
            if (!validateInput.ValidateUserInput(textWaterSalinity.Text, 10))
            {
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Show(Resource1.String76, textWaterSalinity);
                btnOK.Enabled = false;
                textWaterSalinity.BackColor = Color.Red;
            }
            else
            {
                textWaterSalinity.BackColor = Color.White;
                systemSet.dSalinity = double.Parse(textWaterSalinity.Text);
            }
            //
            if (!validateInput.ValidateUserInput(textHeadingOffset.Text, 7))
            {
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Show(Resource1.String80, textHeadingOffset);
                btnOK.Enabled = false;
                textHeadingOffset.BackColor = Color.Red;
            }
            else
            {
                if (double.Parse(textHeadingOffset.Text) > 180 || double.Parse(textHeadingOffset.Text) < -180)
                {
                    ToolTip tooltip1 = new ToolTip();
                    tooltip1.Show(Resource1.String80, textHeadingOffset);
                    btnOK.Enabled = false;
                    textHeadingOffset.BackColor = Color.Red;
                }
                else
                {
                    textHeadingOffset.BackColor = Color.White;
                    systemSet.dHeadingOffset = double.Parse(textHeadingOffset.Text);
                }
            }
            //

        }

        private void comboBoxHeadingRef_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        }
        private void comboBoxVesselSpeedRef_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        }
        
        private void textWaterSalinity_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }
        private void textBoxTransducerDepth_TextChanged(object sender, EventArgs e)
        {
            TheTextChanged(sender, e);
        }

        private void comboBox_RS232_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}
