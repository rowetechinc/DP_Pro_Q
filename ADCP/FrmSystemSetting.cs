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
        public FrmSystemSetting(SystemSetting systSet, int iSystemNumber, bool bPlayback, SerialPort _sp)
        {
            InitializeComponent();

            bPlayBackMode = bPlayback; //LPJ 2014-2-19

            sp = _sp; //LPJ 2016-4-8

            if (bPlayback)  //LPJ 2013-11-20 当回放模式时，只能修改船速参考和换能器深度
            {
                //comboBoxHeadingRef.Enabled = false; //LPJ2016-8-16 回放时，艏向参考可更改
                comboBoxMeasMode.Enabled = false;
                comboBoxStandardMode.Enabled = false;
                textBoxHeadingOffset.Enabled = false; //LPJ 2014-6-16
                textBoxSalinity.Enabled = false; //LPJ 2014-6-16

                comboBox_RS232.Enabled = false;
                comboBox_RS485.Enabled = false;
            }

            comboBox_RS232.SelectedIndex = 5;
            comboBox_RS485.SelectedIndex = 5;

            systemNumber = iSystemNumber; //LPJ 2013-10-9

            GetReference(systSet);
        }

        private SerialPort sp; //LPJ 2016-5-19

        private int systemNumber = 0;
        private bool bPlayBackMode = false; //标记是否为回放模式

        private bool bEnglish = false;
        private void GetReference(SystemSetting systSet)
        {
            setAdvanced = systSet.Advancedstruct;
            bEnglish = systSet.bEnglishUnit;

            //comboBoxFlowRef.SelectedIndex = systSet.iFlowRef; //LPJ 2013-7-24 cancel
            comboBoxHeadingRef.SelectedIndex = systSet.iHeadingRef;
            comboBoxMeasMode.SelectedIndex = systSet.iMeasMode;
            comboBoxVesselSpeedRef.SelectedIndex = systSet.iVesselRef;
            textBoxSalinity.Text = systSet.dSalinity.ToString(); //LPJ 2014-6-16
            textBoxHeadingOffset.Text = systSet.dHeadingOffset.ToString(); //LPJ 2014-6-16
            textBoxTransducerDepth.Text = systSet.dTransducerDepth.ToString();
            comboBoxStandardMode.SelectedIndex = systSet.iStandardMode;
      
            if (systSet.bEnglishUnit)
            {
                label6.Text = "(ft)";

                if (1200 == systSet.iInstrumentTypes) //LPJ 2013-8-1
                {
                    comboBoxStandardMode.Items[0] = Resource1.String287 + "(" + Resource1.String238 + " < 4.8ft)";
                    comboBoxStandardMode.Items[1] = Resource1.String124 + "(" + Resource1.String238 + " < 16ft)";
                    comboBoxStandardMode.Items[2] = Resource1.String125 + "(" + Resource1.String238 + " < 32ft)";
                    comboBoxStandardMode.Items[3] = Resource1.String126 + "(" + Resource1.String238 + " < 98ft)";
                }
                else if (600 == systSet.iInstrumentTypes) //LPJ 2013-8-1
                {
                    comboBoxStandardMode.Items[0] = Resource1.String124 + "(" + Resource1.String238 + " < 32ft)";
                    comboBoxStandardMode.Items[1] = Resource1.String125 + "(" + Resource1.String238 + " < 98ft)";
                    comboBoxStandardMode.Items[2] = Resource1.String126 + "(" + Resource1.String238 + " < 164ft)";
                    comboBoxStandardMode.Items[3] = "";
                }
                else if (300 == systSet.iInstrumentTypes) //LPJ 2013-8-1
                {
                    comboBoxStandardMode.Items[0] = Resource1.String124 + "(" + Resource1.String238 + " < 64ft)";
                    comboBoxStandardMode.Items[1] = Resource1.String125 + "(" + Resource1.String238 + " < 164ft)";
                    comboBoxStandardMode.Items[2] = Resource1.String126 + "(" + Resource1.String238 + " < 320ft)";
                    comboBoxStandardMode.Items[3] = "";
                }
                else //针对不属于300、600、1200 的仪器
                {
                    comboBoxStandardMode.Items[0] = Resource1.String124 + "(" + Resource1.String238 + " < 164ft)";
                    comboBoxStandardMode.Items[1] = Resource1.String125 + "(" + Resource1.String238 + " < 320ft)";
                    comboBoxStandardMode.Items[2] = Resource1.String126 + "(" + Resource1.String238 + " < 640ft)";
                    comboBoxStandardMode.Items[3] = "";
                }
            }
            else
            {
                label6.Text = "(m)";

                if (1200 == systSet.iInstrumentTypes) //LPJ 2013-8-1
                {
                    comboBoxStandardMode.Items[0] = Resource1.String287 + "(" + Resource1.String238 + " < 1.5m)";
                    comboBoxStandardMode.Items[1] = Resource1.String124 + "(" + Resource1.String238 + " < 5m)";
                    comboBoxStandardMode.Items[2] = Resource1.String125 + "(" + Resource1.String238 + " < 10m)";
                    comboBoxStandardMode.Items[3] = Resource1.String126 + "(" + Resource1.String238 + " < 30m)";
                }
                else if (600 == systSet.iInstrumentTypes) //LPJ 2013-8-1
                {
                    comboBoxStandardMode.Items[0] = Resource1.String124 + "(" + Resource1.String238 + " < 10m)";
                    comboBoxStandardMode.Items[1] = Resource1.String125 + "(" + Resource1.String238 + " < 30m)";
                    comboBoxStandardMode.Items[2] = Resource1.String126 + "(" + Resource1.String238 + " < 50m)";
                    comboBoxStandardMode.Items[3] ="";
                }
                else if (300 == systSet.iInstrumentTypes) //LPJ 2013-8-1
                {
                    comboBoxStandardMode.Items[0] = Resource1.String124 + "(" + Resource1.String238 + " < 20m)";
                    comboBoxStandardMode.Items[1] = Resource1.String125 + "(" + Resource1.String238 + " < 50m)";
                    comboBoxStandardMode.Items[2] = Resource1.String126 + "(" + Resource1.String238 + " < 100m)";
                    comboBoxStandardMode.Items[3] = "";
                }
                else //针对不属于300、600、1200 的仪器
                {
                    comboBoxStandardMode.Items[0] = Resource1.String124 + "(" + Resource1.String238 + " < 50m)";
                    comboBoxStandardMode.Items[1] = Resource1.String125 + "(" + Resource1.String238 + " < 100m)";
                    comboBoxStandardMode.Items[2] = Resource1.String126 + "(" + Resource1.String238 + " < 200m)";
                    comboBoxStandardMode.Items[3] ="";
                }
            }
        }

        private CAdvancedCfg.AdvancedConfiguration setAdvanced = new CAdvancedCfg.AdvancedConfiguration();
        private void btnOK_Click(object sender, EventArgs e)
        {
            //systemSet.iFlowRef = comboBoxFlowRef.SelectedIndex;
            systemSet.iHeadingRef = comboBoxHeadingRef.SelectedIndex;
            systemSet.iMeasMode = comboBoxMeasMode.SelectedIndex;

            systemSet.strRS232 = comboBox_RS232.Text;
            systemSet.strRS485 = comboBox_RS485.Text;

            systemSet.iVesselRef = comboBoxVesselSpeedRef.SelectedIndex;

            //if (1 == systemSet.iVesselRef) //LPJ 2013-9-13
            //{
            //    if (textBoxHeadingOffset.Text != "")
            //        systemSet.dHeadingOffset = double.Parse(textBoxHeadingOffset.Text); //LPJ 2013-9-13
            //    else
            //        systemSet.dHeadingOffset = 0; //LPJ 2013-9-13
            //}
            //else
            //    systemSet.dHeadingOffset = 0; //LPJ 2013-9-13

            systemSet.dSalinity = double.Parse(textBoxSalinity.Text); //LPJ 2014-6-16
            systemSet.dHeadingOffset = double.Parse(textBoxHeadingOffset.Text); //LPJ 2014-6-16

            systemSet.dTransducerDepth = double.Parse(textBoxTransducerDepth.Text);
            systemSet.iStandardMode = comboBoxStandardMode.SelectedIndex;

            if (0 == comboBoxMeasMode.SelectedIndex)
                systemSet.Advancedstruct = setAdvanced; //LPJ 2013-8-6
           
        }

        public SystemSetting systemSet;
        public struct SystemSetting
        {
            //public int iFlowRef;
            public int iVesselRef;
            //public double dHeadingOffset; //LPJ 2013-9-13
            public int iHeadingRef;
            public double dTransducerDepth;
            public double dSalinity; //LPJ 2014-6-16
            public double dHeadingOffset; //LPJ 2014-6-16
            public int iMeasMode;
            public int iStandardMode; //用户模式
            public bool bEnglishUnit; //LPJ 2013-7-1 单位设置
            public int iInstrumentTypes; //LPJ 2013-8-1 设置不同的仪器类型
            public CAdvancedCfg.AdvancedConfiguration Advancedstruct; //专家模式

            public string strRS232; //LPJ 2013-7-30 当用户选择外接罗盘时，串口波特率
            public string strRS485;//LPJ 2013-7-30 当用户选择外接罗盘时，串口波特率
        }

        private void comboBoxMeasMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bPlayBackMode)
            {
                if (0 == comboBoxMeasMode.SelectedIndex)
                {
                    comboBoxStandardMode.Visible = true;
                }
                else
                {
                    comboBoxStandardMode.Visible = false;

                    FrmAdvancedMode frmadvancedMode = new FrmAdvancedMode(setAdvanced, bEnglish, 0,bPlayBackMode,sp);
                    if (DialogResult.OK == frmadvancedMode.ShowDialog())
                    {
                        systemSet.Advancedstruct = frmadvancedMode.advancedConf;

                    }
                    else
                    {
                        systemSet.Advancedstruct = setAdvanced; //LPJ 2013-8-5 
                        comboBoxMeasMode.SelectedIndex = 0;
                        comboBoxStandardMode.Visible = true;
                    }
                }
            }
            else
            {
                if (1 == comboBoxMeasMode.SelectedIndex)
                {
                    linkLabelCheckAdvanced.Visible = true;
                    linkLabelCheckAdvanced.Enabled = true;
                }
                else
                {
                    linkLabelCheckAdvanced.Visible = false;
                    linkLabelCheckAdvanced.Enabled = false;
                }
            }
        }
 
        ClassValidateInPut validateInput = new ClassValidateInPut();
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            if (!validateInput.ValidateUserInput(textBoxTransducerDepth.Text, 10))
            {
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Show(Resource1.String76, textBoxTransducerDepth);
                btnOK.Enabled = false;
            }
           
            if (!validateInput.ValidateUserInput(textBoxSalinity.Text, 10))
            {
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Show(Resource1.String76, textBoxSalinity);
                btnOK.Enabled = false;
            }
        }

        //private void textBoxTransducerDepth_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    e.Handled = validateInput.ValidateCharInput(e.KeyChar, ((TextBox)sender).Text);

        //}

        private void comboBoxHeadingRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 == comboBoxHeadingRef.SelectedIndex)
            {
                comboBox_RS232.Enabled = false;
                label_RS232.Enabled = false;
                label_RS485.Enabled = false;
                comboBox_RS485.Enabled = false;

                if (1 == comboBoxVesselSpeedRef.SelectedIndex || 3 == comboBoxVesselSpeedRef.SelectedIndex)
                {
                    textBoxHeadingOffset.Enabled = true;
                }
                else
                    textBoxHeadingOffset.Enabled = false;
            }
            else
            {
                if (bPlayBackMode)
                {
                    comboBox_RS232.Enabled = false;
                    label_RS232.Enabled = false;
                    label_RS485.Enabled = false;
                    comboBox_RS485.Enabled = false;
                }
                else
                {
                    comboBox_RS232.Enabled = true;
                    label_RS232.Enabled = true;
                    label_RS485.Enabled = true;
                    comboBox_RS485.Enabled = true;
                }
                textBoxHeadingOffset.Enabled = true;
            }
        }

        private void comboBoxVesselSpeedRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (1 == comboBoxVesselSpeedRef.SelectedIndex || 3 == comboBoxVesselSpeedRef.SelectedIndex)
            {
                textBoxHeadingOffset.Enabled = true;
            }
            else
            {
                if (1 == comboBoxHeadingRef.SelectedIndex)
                    textBoxHeadingOffset.Enabled = true;
                else
                    textBoxHeadingOffset.Enabled = false;
            }

        }

        private void textBoxHeadingOffset_TextChanged(object sender, EventArgs e)
        {
            if (!validateInput.ValidateUserInput(textBoxHeadingOffset.Text, 7))
            {
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Show(Resource1.String80, textBoxHeadingOffset);
                btnOK.Enabled = false;
            }
            else
            {
                if (double.Parse(textBoxHeadingOffset.Text) > 180 || double.Parse(textBoxHeadingOffset.Text) < -180)
                {
                    ToolTip tooltip1 = new ToolTip();
                    tooltip1.Show(Resource1.String80, textBoxHeadingOffset);
                    btnOK.Enabled = false;
                }
                else
                    btnOK.Enabled = true;
            }
        }

        private void textBoxHeadingOffset_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = validateInput.ValidateFloatInput(e.KeyChar, ((TextBox)sender).Text);

        }

        /// <summary>
        /// 在回放模式下，查看专家配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelCheckAdvanced_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAdvancedMode frmadvancedMode = new FrmAdvancedMode(setAdvanced, bEnglish, 0, bPlayBackMode,sp);
                frmadvancedMode.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 在回放模式下，查看专家配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelCheckAdvanced_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                FrmAdvancedMode frmadvancedMode = new FrmAdvancedMode(setAdvanced, bEnglish, 0, bPlayBackMode,sp);
                frmadvancedMode.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = validateInput.ValidateCharInput(e.KeyChar, ((TextBox)sender).Text);
        }

        private void textBoxSalinity_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
