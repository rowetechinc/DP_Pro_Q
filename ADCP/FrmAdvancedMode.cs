using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JDL.UILib;
using System.IO;
using System.IO.Ports;

namespace ADCP
{
    public partial  class FrmAdvancedMode : Form
    {
        public FrmAdvancedMode(CAdvancedCfg.AdvancedConfiguration advanced, bool bEnglish, int iSystemNumber, bool bPlaybackMode, SerialPort _sp)
        {
            InitializeComponent();
            bEnglishUnit = bEnglish;

            systemNumber = iSystemNumber; //LPJ 2013-10-9 读取仪器类型号
            sp = _sp;

            //InitParameter();
            if (!bPlaybackMode)
            {
                try
                {
                    GetConfiguration(advanced); //LPJ 2013-8-5 读取高级配置
                }
                catch
                {
                    this.btnDefault_Click(this, null);
                }
            }
            else
            {
                //groupBox3.Enabled = false;
                groupBox22.Enabled = false;
                groupBox23.Enabled = false;
                groupBox24.Enabled = false;
                groupBox26.Enabled = false;
                groupBox44.Enabled = false;
                btnDefault.Visible = false;
                btnLoad.Visible = false;
                btnExport.Visible = false;
                btnOK.Visible = false;
                btnCancel.Visible = false;

                try
                {
                    GetConfiguration(advanced);
                }
                catch
                {
                }
            }
        }

        private SerialPort sp;

        private bool SetConfiguration(ref CAdvancedCfg.AdvancedConfiguration advancedConf)
        { 
            try
            {
                //将设置的信息传递给主界面，并在点击start的时候，发送到仪器
                //if (radio_ADCPMode.Checked)
                advancedConf.ADCPMode = true;
                //else
                //    advancedConf.ADCPMode = false;

                advancedConf.EsmbIntervalHH = int.Parse(textEsmbIntervalHH.Text);
                advancedConf.EsmbIntervalMM = int.Parse(this.textEsmbIntervalMM.Text);
                advancedConf.EsmbIntervalSShh = float.Parse(this.textEsmbIntervalSShh.Text);//时间平均步长

                advancedConf.BurstInterval_HH = int.Parse(this.txt_BurstInterval_HH.Text);
                advancedConf.BurstInterval_MM = int.Parse(this.txt_BurstInterval_MM.Text);//Burst Interval
                advancedConf.BurstInterval_SS = float.Parse(this.txt_BurstInterval_SS.Text);
                advancedConf.BurstInterval_n = int.Parse(this.txt_BurstInterval_n.Text);

                if (WaterPingOpen.Checked)//水跟踪开关
                    advancedConf.WaterPingOpen = true;
                else
                    advancedConf.WaterPingOpen = false;

                if (this.Broadband_Mode.Checked)
                {
                    advancedConf.WaterProfilerMode = 1; //水跟踪带宽，宽带、窄带、脉冲相干等
                    advancedConf.CWPBP_Pings = int.Parse(textBox_CWPBP_Pings.Text); //LPJ 2013-10-30
                    advancedConf.CWPBP_Time = float.Parse(textBox_CWPBP_Time.Text); //LPJ 2013-10-30
                }
                else if (this.Narrowband_Mode.Checked)
                {
                    advancedConf.WaterProfilerMode = 0;
                    advancedConf.CWPBP_Pings = int.Parse(textBox_CWPBP_Pings.Text); //LPJ 2013-10-30
                    advancedConf.CWPBP_Time = float.Parse(textBox_CWPBP_Time.Text); //LPJ 2013-10-30
                }
                else if (this.nonPulse_Mode.Checked)
                    advancedConf.WaterProfilerMode = 2;
                else if (this.Pulse_Mode.Checked)
                    advancedConf.WaterProfilerMode = 3;
                else if (this.NonCodedBroadband_Mode.Checked)
                    advancedConf.WaterProfilerMode = 4;
                else if (this.Broadband_Ambiguity_Mode.Checked)
                    advancedConf.WaterProfilerMode = 5;
                else if (this.CWPBB_Mode6.Checked)
                    advancedConf.WaterProfilerMode = 6;
                advancedConf.WPLagLengthV = float.Parse(this.LagLengthV_TextBox.Text); //LagLength
                advancedConf.CWPAP1 = float.Parse(this.txtBox_CWPAP1.Text);
                advancedConf.CWPAP2 = float.Parse(this.textBox_CWPAP2.Text);
                advancedConf.CWPAP3 = float.Parse(this.textBox_CWPAP3.Text);
                advancedConf.CWPAP4 = float.Parse(this.textBox_CWPAP4.Text);
                advancedConf.CWPAP5 = float.Parse(this.textBox_CWPAP5.Text);
                advancedConf.WPST_Correlation = float.Parse(this.WPST_Correlation_TextBox.Text);
                advancedConf.WPST_QVelocity = float.Parse(this.WPST_QVelocity_TextBox.Text);
                advancedConf.WPST_VVelocity = float.Parse(this.WPST_VVelocity_TextBox.Text); //Water Profile Screening Thresholds
                advancedConf.WPBlankSize = float.Parse(this.textBlankSize.Text);//盲区
                advancedConf.WPBinSize = float.Parse(this.textBinSize.Text);//单元尺寸
                advancedConf.WPWaterXmt = float.Parse(this.textWaterXmt.Text);//脉冲长度
                advancedConf.WPBinNum = int.Parse(this.textBinNum.Text);//单元层数
                advancedConf.WPWaterAvgNum = int.Parse(this.textWaterAvgNum.Text);//呯数
                //advancedConf.WaterAvgIntervalHH = int.Parse(this.textWaterAvgIntervalHH.Text);   //LPJ 2013-9-27 该命令已在I版本中取消
                //advancedConf.WaterAvgIntervalMM = int.Parse(this.textWaterAvgIntervalMM.Text);
                //advancedConf.WaterAvgIntervalSShh = float.Parse(this.textWaterAvgIntervalSShh.Text);//平均间隔
                advancedConf.WPTimeBtwnPings = float.Parse(this.textTimeBtwnPings.Text);//呯间隔

                if (this.radioBtmTrkOpen.Checked)//底跟踪开关
                    advancedConf.BtmTrkOpen = true;
                else
                    advancedConf.BtmTrkOpen = false;

                if (this.radioBTBroadBand.Checked)//底跟踪带宽
                    advancedConf.BTMode = 1;
                else if (this.radioBTNarrowBand.Checked)
                    advancedConf.BTMode = 0;
                else if (this.nonPulseTransmit_Mode.Checked)
                    advancedConf.BTMode = 2;
                else if (this.LongRange_Mode.Checked)
                    advancedConf.BTMode = 3;
                else if (this.nonCode_Pulse_Mode.Checked)
                    advancedConf.BTMode = 4;
                else if (this.nonCode_LongRange_Mode.Checked)
                    advancedConf.BTMode = 5;
                else if (this.nonCode_Pulse_LongRange_Mode.Checked)
                    advancedConf.BTMode = 6;
                else if (this.CBTBB7.Checked)
                    advancedConf.BTMode = 7;

                advancedConf.BTPulseLag = float.Parse(this.PulseLag_TextBox.Text);
                advancedConf.BTLongRangeDepth = float.Parse(this.LongRangeDepth_TextBox.Text); //pulse to pulse lag，
                advancedConf.BTST_Correlation = float.Parse(this.BTST_Correlation_text.Text);
                advancedConf.BTST_QV = float.Parse(this.BTST_QV_text.Text);
                advancedConf.BTST_V = float.Parse(this.BTST_V_text.Text);  //LPJ 2012-10-19 add
                advancedConf.BTT_SNRshallow = float.Parse(this.BTT_SNRshallow_text.Text);
                advancedConf.BTT_Depthshallow2deep = float.Parse(this.BTT_Depthshallow2deep_text.Text);
                advancedConf.BTT_SNRdeep = float.Parse(this.BTT_SNRdeep_text.Text);
                advancedConf.BTT_Depthlow2high = float.Parse(this.BTT_Depthlow2high_text.Text);  //
                advancedConf.BtmTrkBlank = float.Parse(this.textBtmTrkBlank.Text);//盲区
                advancedConf.BtmTrkDepth = float.Parse(this.textBtmTrkDepth.Text);//深度
                advancedConf.BtmTrkInterval = float.Parse(this.textBtmTrkInterval.Text);//呯间隔

                //if (this.radioWaterTrkOpen.Checked)//水参考开关 //LPJ 2013-11-20 cancel
                //    advancedConf.WaterTrkOpen = true;
                //else if (this.radioWaterTrkClose.Checked)
                //    advancedConf.WaterTrkOpen = false;
                //if (this.radioWTBroadBand.Checked)//水参考带宽
                //    advancedConf.WTMode = 1;
                //else
                //    advancedConf.WTMode = 0;
                //advancedConf.WTBlankSize = float.Parse(this.textWTBlankSize.Text);//盲区
                //advancedConf.WTBinSize = float.Parse(this.textWTBinSize.Text);//单元尺寸
                //advancedConf.WTInterval = float.Parse(this.textWTInterval.Text);//呯间隔

                advancedConf.CWSCCwaterTemperature = float.Parse(this.comboBox_waterTemperature.Text);
                advancedConf.CWSCCTransducerDepth = float.Parse(this.comboBox_TransducerDepth.Text);
                advancedConf.CWSCCSalinity = float.Parse(this.comboBox_Salinity.Text);
                advancedConf.CWSCCSpeedOfSound = float.Parse(this.comboBox_SpeedOfSound.Text);//Water Speed Of Sound Control 
                advancedConf.WaterSalinity = float.Parse(this.textWaterSalinity.Text); //盐度
                advancedConf.WaterTemperature = float.Parse(this.textWaterTemperature.Text); //水温
                //advancedConf.TransducerDepth = float.Parse(this.textTransducerDepth.Text);//换能器深度
                advancedConf.SoundSpeed = float.Parse(this.textSoundSpeed.Text);//声速
                advancedConf.HeadingOffset = float.Parse(this.textHeadingOffset.Text);//艏向偏角
                //advancedConf.R232 = float.Parse(comboBoxR232.Text); //波特率
                //advancedConf.R485 = float.Parse(comboBoxR485.Text);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SetConfiguration(ref advancedConf))
            {
                this.Close();
            }
            else
            {
            }
          
        }

        private void InitParameter()
        {
            //采用默认参数初始化
            //radio_ADCPMode.Checked = true;
            textEsmbIntervalHH.Text = "0";
            textEsmbIntervalMM.Text = "0";
            textEsmbIntervalSShh.Text = "1.0";//时间平均步长

            txt_BurstInterval_HH.Text = "0";
            txt_BurstInterval_MM.Text = "0";//Burst Interval
            txt_BurstInterval_SS.Text = "0.0";
            txt_BurstInterval_n.Text = "0";

            WaterPingOpen.Checked = true;
            Broadband_Mode.Checked = true;
            LagLengthV_TextBox.Text = "0.042"; //LagLength
            txtBox_CWPAP1.Text = "0";
            textBox_CWPAP2.Text = "0";
            textBox_CWPAP3.Text = "0";
            textBox_CWPAP4.Text = "0";
            textBox_CWPAP5.Text = "0";
            WPST_Correlation_TextBox.Text = "0.4";
            WPST_QVelocity_TextBox.Text = "1.0";
            WPST_VVelocity_TextBox.Text = "1.0";//Water Profile Screening Thresholds
            textBlankSize.Text = "0.25";//盲区
            textBinSize.Text = "0.5";//单元尺寸
            textWaterXmt.Text = "0.0";//脉冲长度
            textBinNum.Text = "20";//单元层数
            textWaterAvgNum.Text = "2";//呯数
            //textWaterAvgIntervalHH.Text = "0";  //LPJ  2013-9-27 该命令已在I版本中取消
            //textWaterAvgIntervalMM.Text = "0";
            //textWaterAvgIntervalSShh.Text = "0.0";//平均间隔
            textTimeBtwnPings.Text = "0.1";//呯间隔

            radioBtmTrkOpen.Checked = true;
            radioBTBroadBand.Checked = true;//底跟踪带宽
            PulseLag_TextBox.Text = "0.0";
            LongRangeDepth_TextBox.Text = "30.0"; //pulse to pulse lag，
            BTST_Correlation_text.Text = "0.9";
            BTST_QV_text.Text = "1.0";
            BTST_V_text.Text = "1.0";  //LPJ 2012-10-19 add
            BTT_SNRshallow_text.Text = "15.0";
            BTT_Depthshallow2deep_text.Text = "25.0";
            BTT_SNRdeep_text.Text = "5.0";
            BTT_Depthlow2high_text.Text = "2.0";  //
            textBtmTrkBlank.Text = "0.1";//盲区
            textBtmTrkDepth.Text = "50.0";//深度
            textBtmTrkInterval.Text = "0.1";//呯间隔

            //radioWaterTrkClose.Checked = true;//水参考开关
            //radioWTBroadBand.Checked = true;//水参考带宽
            //textWTBlankSize.Text = "0.5";//盲区
            //textWTBinSize.Text = "0.5";//单元尺寸
            //textWTInterval.Text = "1.0";//呯间隔

            comboBox_waterTemperature.Text = "1";
            comboBox_TransducerDepth.Text = "1";
            comboBox_Salinity.Text = "0";
            comboBox_SpeedOfSound.Text = "2";//Water Speed Of Sound Control 
            textWaterSalinity.Text = "0"; //盐度
            textWaterTemperature.Text = "15"; //水温
            //textTransducerDepth.Text = "0.1";//换能器深度
            textSoundSpeed.Text = "1500";//声速
            textHeadingOffset.Text = "0";//艏向偏角
            //comboBoxR232.Text = "115200"; //波特率
            //comboBoxR485.Text = "115200";

            if (bEnglishUnit)
            {
                LagLengthV_TextBox.Text = projectUnit.MeterToFeet(0.042 ,1).ToString("0.000");
                textBlankSize.Text = projectUnit.MeterToFeet(0.25, 1).ToString("0.000");//盲区
                textBinSize.Text = projectUnit.MeterToFeet(0.5, 1).ToString("0.000");//单元尺寸
                textWaterXmt.Text = projectUnit.MeterToFeet(0.0, 1).ToString("0.000");//脉冲长度
                WPST_QVelocity_TextBox.Text = projectUnit.MeterToFeet(1.0, 1).ToString("0.000");
                WPST_VVelocity_TextBox.Text = projectUnit.MeterToFeet(1.0, 1).ToString("0.000");
                PulseLag_TextBox.Text = projectUnit.MeterToFeet(0.0, 1).ToString("0.000");
                LongRangeDepth_TextBox.Text = projectUnit.MeterToFeet(30.0, 1).ToString("0.000");
                BTST_QV_text.Text = projectUnit.MeterToFeet(1.0, 1).ToString("0.000");
                BTST_V_text.Text = projectUnit.MeterToFeet(1.0, 1).ToString("0.000");
                BTT_Depthshallow2deep_text.Text = projectUnit.MeterToFeet(25.0, 1).ToString("0.000");
                BTT_Depthlow2high_text.Text = projectUnit.MeterToFeet(2.0, 1).ToString("0.000");  //
                textBtmTrkBlank.Text = projectUnit.MeterToFeet(0.1, 1).ToString("0.000");//盲区
                textBtmTrkDepth.Text = projectUnit.MeterToFeet(50.0, 1).ToString("0.000");//深度
                //textTransducerDepth.Text = projectUnit.MeterToFeet(0.1, 1).ToString("0.000");//换能器深度
                textSoundSpeed.Text = projectUnit.MeterToFeet(1500, 1).ToString("0.000");//声速
                //textWTBlankSize.Text = projectUnit.MeterToFeet(0.5, 1).ToString("0.000");//盲区
                //textWTBinSize.Text = projectUnit.MeterToFeet(0.5, 1).ToString("0.000");//单元尺寸

                label1.Text = "(ft)";
                label2.Text = "(ft)";
                label3.Text = "(ft)";
                label4.Text = "(ft)";
                label5.Text = "(ft/s)";
                label6.Text = "(ft/s)";
                label7.Text = "(ft)";
                label8.Text = "(ft)";
                label9.Text = "(ft/s)";
                label10.Text = "(ft/s)";
                label11.Text = "(ft)";
                label12.Text = "(ft)";
                label13.Text = "(ft)";
                label14.Text = "(ft)";
                //label15.Text = "(ft)";
                //label16.Text = "(ft)";
                label17.Text = "(ft)";
                label18.Text = "(ft/s)";

            }
            else
            {
                label1.Text = "(m)";
                label2.Text = "(m)";
                label3.Text = "(m)";
                label4.Text = "(m)";
                label5.Text = "(m/s)";
                label6.Text = "(m/s)";
                label7.Text = "(m)";
                label8.Text = "(m)";
                label9.Text = "(m/s)";
                label10.Text = "(m/s)";
                label11.Text = "(m)";
                label12.Text = "(m)";
                label13.Text = "(m)";
                label14.Text = "(m)";
                //label15.Text = "(m)";
                //label16.Text = "(m)";
                label17.Text = "(m)";
                label18.Text = "(m/s)";

            }

        }
        private Units projectUnit = new Units();
 
        private bool bEnglishUnit;
        private void GetConfiguration(CAdvancedCfg.AdvancedConfiguration advanced)
        {
            //if (advanced.ADCPMode)
                //radio_ADCPMode.Checked = true;
            //else
            //    radio_DVLMode.Checked = true;

            textEsmbIntervalHH.Text = advanced.EsmbIntervalHH.ToString();
            textEsmbIntervalMM.Text = advanced.EsmbIntervalMM.ToString();
            textEsmbIntervalSShh.Text = advanced.EsmbIntervalSShh.ToString();//时间平均步长

            txt_BurstInterval_HH.Text = advanced.BurstInterval_HH.ToString();
            txt_BurstInterval_MM.Text = advanced.BurstInterval_MM.ToString();//Burst Interval
            txt_BurstInterval_SS.Text = advanced.BurstInterval_SS.ToString();
            txt_BurstInterval_n.Text = advanced.BurstInterval_n.ToString();

            if (advanced.WaterPingOpen)
                WaterPingOpen.Checked = true;
            else
                WaterPingClose.Checked = true;

            if (1 == advanced.WaterProfilerMode)
            {
                Broadband_Mode.Checked = true;
                textBox_CWPBP_Pings.Text = advanced.CWPBP_Pings.ToString(); //LPJ 2013-10-30
                textBox_CWPBP_Time.Text = advanced.CWPBP_Time.ToString();   //LPJ 2013-10-30
            }
            else if (0 == advanced.WaterProfilerMode)
            {
                Narrowband_Mode.Checked = true;
                textBox_CWPBP_Pings.Text = advanced.CWPBP_Pings.ToString(); //LPJ 2013-10-30
                textBox_CWPBP_Time.Text = advanced.CWPBP_Time.ToString();   //LPJ 2013-10-30
            }
            else if (2 == advanced.WaterProfilerMode)
                nonPulse_Mode.Checked = true;
            else if (3 == advanced.WaterProfilerMode)
                Pulse_Mode.Checked = true;
            else if (4 == advanced.WaterProfilerMode)
                NonCodedBroadband_Mode.Checked = true;
            else if (5 == advanced.WaterProfilerMode)
                Broadband_Ambiguity_Mode.Checked = true;
            else
                CWPBB_Mode6.Checked = true;

            LagLengthV_TextBox.Text = advanced.WPLagLengthV.ToString() ; //LagLength
            txtBox_CWPAP1.Text = advanced.CWPAP1.ToString();
            textBox_CWPAP2.Text = advanced.CWPAP2.ToString();
            textBox_CWPAP3.Text = advanced.CWPAP3.ToString();
            textBox_CWPAP4.Text = advanced.CWPAP4.ToString();
            textBox_CWPAP5.Text = advanced.CWPAP5.ToString();
            WPST_Correlation_TextBox.Text = advanced.WPST_Correlation.ToString();
            WPST_QVelocity_TextBox.Text = advanced.WPST_QVelocity.ToString();
            WPST_VVelocity_TextBox.Text = advanced.WPST_VVelocity.ToString();//Water Profile Screening Thresholds
            textBlankSize.Text = advanced.WPBlankSize.ToString();//盲区
            textBinSize.Text = advanced.WPBinSize.ToString();//单元尺寸
            textWaterXmt.Text = advanced.WPWaterXmt.ToString();//脉冲长度
            textBinNum.Text = advanced.WPBinNum.ToString();//单元层数
            textWaterAvgNum.Text = advanced.WPWaterAvgNum.ToString();//呯数
            //textWaterAvgIntervalHH.Text = advanced.WaterAvgIntervalHH.ToString();  //LPJ  2013-9-27 该命令已在I版本中取消
            //textWaterAvgIntervalMM.Text = advanced.WaterAvgIntervalMM.ToString();
            //textWaterAvgIntervalSShh.Text = advanced.WaterAvgIntervalSShh.ToString();//平均间隔
            textTimeBtwnPings.Text = advanced.WPTimeBtwnPings.ToString();//呯间隔

           
            if (advanced.BtmTrkOpen)
                radioBtmTrkOpen.Checked = true;
            else
                radioBtmTrkClose.Checked = true;

            if (advanced.BTMode == 1)
                radioBTBroadBand.Checked = true;//底跟踪带宽
            else if (advanced.BTMode == 0)
                radioBTNarrowBand.Checked = true;
            else if (advanced.BTMode == 2)
                nonPulseTransmit_Mode.Checked = true;
            else if (advanced.BTMode == 3)
                LongRange_Mode.Checked = true;
            else if (advanced.BTMode == 4)
                nonCode_Pulse_Mode.Checked = true;
            else if (advanced.BTMode == 5)
                nonCode_LongRange_Mode.Checked = true;
            else if (advanced.BTMode == 6)
                nonCode_Pulse_LongRange_Mode.Checked = true;
            else
                CBTBB7.Checked = true;

            PulseLag_TextBox.Text = advanced.BTPulseLag.ToString();
            LongRangeDepth_TextBox.Text = advanced.BTLongRangeDepth.ToString(); //pulse to pulse lag，
            BTST_Correlation_text.Text = advanced.BTST_Correlation.ToString();
            BTST_QV_text.Text = advanced.BTST_QV.ToString();
            BTST_V_text.Text = advanced.BTST_V.ToString();  //LPJ 2012-10-19 add
            BTT_SNRshallow_text.Text = advanced.BTT_SNRshallow.ToString();
            BTT_Depthshallow2deep_text.Text = advanced.BTT_Depthshallow2deep.ToString();
            BTT_SNRdeep_text.Text = advanced.BTT_SNRdeep.ToString();
            BTT_Depthlow2high_text.Text = advanced.BTT_Depthlow2high.ToString();  //
            textBtmTrkBlank.Text = advanced.BtmTrkBlank.ToString();//盲区
            textBtmTrkDepth.Text = advanced.BtmTrkDepth.ToString();//深度
            textBtmTrkInterval.Text = advanced.BtmTrkInterval.ToString();//呯间隔

           
            //if (advanced.WaterTrkOpen)
            //    radioWaterTrkClose.Checked = true;//水参考开关
            //else
            //    radioWaterTrkClose.Checked = true;

            //if (advanced.WTMode == 1)
            //    radioWTBroadBand.Checked = true;//水参考带宽
            //else
            //    radioWTNarrowBand.Checked = true;

            //textWTBlankSize.Text = advanced.WTBlankSize.ToString();//盲区
            //textWTBinSize.Text = advanced.WTBinSize.ToString();//单元尺寸
            //textWTInterval.Text = advanced.WTInterval.ToString();//呯间隔

        
            comboBox_waterTemperature.Text = advanced.CWSCCwaterTemperature.ToString();
            comboBox_TransducerDepth.Text = advanced.CWSCCTransducerDepth.ToString();
            comboBox_Salinity.Text = advanced.CWSCCSalinity.ToString();
            comboBox_SpeedOfSound.Text =advanced.CWSCCSpeedOfSound.ToString();//Water Speed Of Sound Control 
            textWaterSalinity.Text = advanced.WaterSalinity.ToString(); //盐度
            textWaterTemperature.Text = advanced.WaterTemperature.ToString(); //水温
            //textTransducerDepth.Text = advanced.TransducerDepth.ToString();//换能器深度
            textSoundSpeed.Text = advanced.SoundSpeed.ToString();//声速
            textHeadingOffset.Text = advanced.HeadingOffset.ToString();//艏向偏角

            //if (advanced.R232 != 0 && advanced.R485 != 0)
            //{
            //    comboBoxR232.Text = advanced.R232.ToString(); //波特率
            //    comboBoxR485.Text = advanced.R485.ToString();
            //}
            //else
            //{
            //    comboBoxR232.Text = "115200";
            //    comboBoxR485.Text = "115200";
            //}

            if (bEnglishUnit)
            {
               
                LagLengthV_TextBox.Text = (advanced.WPLagLengthV / 0.3048).ToString("0.000");
                textBlankSize.Text = (advanced.WPBlankSize / 0.3048).ToString("0.000");//盲区
                textBinSize.Text = (advanced.WPBinSize / 0.3048).ToString("0.000");//单元尺寸
                textWaterXmt.Text = (advanced.WPWaterXmt / 0.3048).ToString("0.000");//脉冲长度
                WPST_QVelocity_TextBox.Text = (advanced.WPST_QVelocity / 0.3048).ToString("0.000");
                WPST_VVelocity_TextBox.Text = (advanced.WPST_VVelocity / 0.3048).ToString("0.000");
                PulseLag_TextBox.Text = (advanced.BTPulseLag / 0.3048).ToString("0.000");
                LongRangeDepth_TextBox.Text = (advanced.BTLongRangeDepth / 0.3048).ToString("0.000");
                BTST_QV_text.Text = (advanced.BTST_QV / 0.3048).ToString("0.000");
                BTST_V_text.Text = (advanced.BTST_V / 0.3048).ToString("0.000");
                BTT_Depthshallow2deep_text.Text = (advanced.BTT_Depthshallow2deep / 0.3048).ToString("0.000");
                BTT_Depthlow2high_text.Text = (advanced.BTT_Depthlow2high / 0.3048).ToString("0.000");  //
                textBtmTrkBlank.Text = (advanced.BtmTrkBlank / 0.3048).ToString("0.000");//盲区
                textBtmTrkDepth.Text = (advanced.BtmTrkDepth / 0.3048).ToString("0.000");//深度
                //textTransducerDepth.Text = (advanced.TransducerDepth / 0.3048).ToString("0.000");//换能器深度
                textSoundSpeed.Text = (advanced.SoundSpeed / 0.3048).ToString("0.000");//声速
                //textWTBlankSize.Text = (advanced.WTBlankSize / 0.3048).ToString("0.000");//盲区
                //textWTBinSize.Text = (advanced.WTBinSize / 0.3048).ToString("0.000");//单元尺寸

                label1.Text = "(ft)";
                label2.Text = "(ft)";
                label3.Text = "(ft)";
                label4.Text = "(ft)";
                label5.Text = "(ft/s)";
                label6.Text = "(ft/s)";
                label7.Text = "(ft)";
                label8.Text = "(ft)";
                label9.Text = "(ft/s)";
                label10.Text = "(ft/s)";
                label11.Text = "(ft)";
                label12.Text = "(ft)";
                label13.Text = "(ft)";
                label14.Text = "(ft)";
                //label15.Text = "(ft)";
                //label16.Text = "(ft)";
                label17.Text = "(ft)";
                label18.Text = "(ft/s)";

            }
            else
            {
                label1.Text = "(m)";
                label2.Text = "(m)";
                label3.Text = "(m)";
                label4.Text = "(m)";
                label5.Text = "(m/s)";
                label6.Text = "(m/s)";
                label7.Text = "(m)";
                label8.Text = "(m)";
                label9.Text = "(m/s)";
                label10.Text = "(m/s)";
                label11.Text = "(m)";
                label12.Text = "(m)";
                label13.Text = "(m)";
                label14.Text = "(m)";
                //label15.Text = "(m)";
                //label16.Text = "(m)";
                label17.Text = "(m)";
                label18.Text = "(m/s)";
                
            }

        }

        ClassValidateInPut validateInput = new ClassValidateInPut();

        private JDL.UILib.BalloonTip m_tip = new BalloonTip();
        private void ShowTip(string str, Control ctl)
        {
            m_tip.Visible = true;
            BalloonTipIconType icontype = (BalloonTipIconType)3;
            Point pos = ctl.PointToScreen(new Point(10, 2));
            int x = pos.X;
            int y = pos.Y;
            this.m_tip.ShowCloseButton = false;
            this.m_tip.ShowAt(x, y, "", str, icontype);
        }
        private void TxtBox_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;

            if (!textBinSize.Focused)
            {
                try
                {
                    if (float.Parse(textBinSize.Text) < 0.00000001f) //当层厚超过0-10时,不能为0
                    {
                        ShowTip(Resource1.String76, textBinSize);
                        btnOK.Enabled = false;
                       
                    }
                }
                catch
                {
                    btnOK.Enabled = false;
                }
            }

            if (!validateInput.ValidateUserInput(textBlankSize.Text, 12)) //盲区
            {
                ShowTip(Resource1.String76, textBlankSize);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBinSize.Text, 12)) //层厚
            {
                ShowTip(Resource1.String76, textBinSize);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textWaterXmt.Text, 12)) //脉冲长度
            {
                ShowTip(Resource1.String76, textWaterXmt);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBinNum.Text, 5)) //层数
            {
                ShowTip(Resource1.String77, textBinNum);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textWaterAvgNum.Text, 5)) //平均基数
            {
                ShowTip(Resource1.String77, textWaterAvgNum);
                btnOK.Enabled = false;
            }

            //if (!validateInput.ValidateUserInput(textWaterAvgIntervalHH.Text, 5)) //平均间隔—时   //LPJ  2013-9-27 该命令已在I版本中取消
            //{
            //    ShowTip(Resource1.String77, textWaterAvgIntervalHH);
            //    btnOK.Enabled = false;
            //}
            //if (!validateInput.ValidateUserInput(textWaterAvgIntervalMM.Text, 5)) //平均间隔—分
            //{
            //    ShowTip(Resource1.String77, textWaterAvgIntervalMM);
            //    btnOK.Enabled = false;
            //}
            //if (!validateInput.ValidateUserInput(textWaterAvgIntervalSShh.Text, 12))//平均间隔—秒
            //{
            //    ShowTip(Resource1.String76, textWaterAvgIntervalHH);
            //    btnOK.Enabled = false;
            //}
            if (!validateInput.ValidateUserInput(textTimeBtwnPings.Text, 12))  //pings间隔
            {
                ShowTip(Resource1.String76, textTimeBtwnPings);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(WPST_Correlation_TextBox.Text, 12))  //LPJ 2013-2-16  
            {
                ShowTip(Resource1.String76, WPST_Correlation_TextBox);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(WPST_QVelocity_TextBox.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, WPST_QVelocity_TextBox);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(WPST_VVelocity_TextBox.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, WPST_VVelocity_TextBox);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(txtBox_CWPAP1.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, txtBox_CWPAP1);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBox_CWPAP2.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, textBox_CWPAP2);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBox_CWPAP3.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, textBox_CWPAP3);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBox_CWPAP4.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, textBox_CWPAP4);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBox_CWPAP5.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, textBox_CWPAP5);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(LagLengthV_TextBox.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, LagLengthV_TextBox);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBox_CWPBP_Pings.Text, 5))  //LPJ 2013-10-30
            {
                ShowTip(Resource1.String77, textBox_CWPBP_Pings);
                btnOK.Enabled = false;
            }
          
            if (!validateInput.ValidateUserInput(textBox_CWPBP_Time.Text, 12))  //LPJ 2013-10-30
            {
                ShowTip(Resource1.String76, textBox_CWPBP_Time);
                btnOK.Enabled = false;
            }


            if (!validateInput.ValidateUserInput(textEsmbIntervalHH.Text, 5))  //样本间隔HH
            {
                ShowTip(Resource1.String77, textEsmbIntervalHH);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textEsmbIntervalMM.Text, 5))  //样本间隔MM
            {
                ShowTip(Resource1.String77, textEsmbIntervalMM);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textEsmbIntervalSShh.Text, 12))  //样本间隔SShh
            {
                ShowTip(Resource1.String76, textEsmbIntervalSShh);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(txt_BurstInterval_HH.Text, 5))  //HH  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String77, txt_BurstInterval_HH);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(txt_BurstInterval_MM.Text, 5))  //  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String77, txt_BurstInterval_MM);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(txt_BurstInterval_SS.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, txt_BurstInterval_SS);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(txt_BurstInterval_n.Text, 5))  //  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String77, txt_BurstInterval_n);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBtmTrkBlank.Text, 12))  //底跟踪盲区
            {
                ShowTip(Resource1.String76, textBtmTrkBlank);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBtmTrkDepth.Text, 12))  //底跟踪深度
            {
                ShowTip(Resource1.String76, textBtmTrkDepth);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textBtmTrkInterval.Text, 12))  //底跟踪pings间隔
            {
                ShowTip(Resource1.String76, textBtmTrkInterval);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(PulseLag_TextBox.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, PulseLag_TextBox);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(LongRangeDepth_TextBox.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, LongRangeDepth_TextBox);
                btnOK.Enabled = false;
            }

            if (!validateInput.ValidateUserInput(BTST_Correlation_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTST_Correlation_text);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(BTST_QV_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTST_QV_text);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(BTST_V_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTST_V_text);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(BTT_SNRshallow_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTT_SNRshallow_text);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(BTT_Depthshallow2deep_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTT_Depthshallow2deep_text);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(BTT_SNRdeep_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTT_SNRdeep_text);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(BTT_Depthlow2high_text.Text, 12))  //LPJ 2013-2-16
            {
                ShowTip(Resource1.String76, BTT_Depthlow2high_text);
                btnOK.Enabled = false;
            }


            //if (!validateInput.ValidateUserInput(textWTBlankSize.Text, 12))  //水跟踪盲区
            //{
            //    ShowTip(Resource1.String76, textWTBlankSize);
            //    btnOK.Enabled = false;
            //}
            //if (!validateInput.ValidateUserInput(textWTBinSize.Text, 12))  //水跟踪层厚
            //{
            //    ShowTip(Resource1.String76, textWTBinSize);
            //    btnOK.Enabled = false;
            //}
            //if (!validateInput.ValidateUserInput(textWTInterval.Text, 12))  //水跟踪pings间隔
            //{
            //    ShowTip(Resource1.String76, textWTInterval);
            //    btnOK.Enabled = false;
            //}

            if (!validateInput.ValidateUserInput(textWaterSalinity.Text, 12))  //盐度
            {
                ShowTip(Resource1.String76, textWaterSalinity);
                btnOK.Enabled = false;
            }
            if (!validateInput.ValidateUserInput(textWaterTemperature.Text, 12))  //水温
            {
                ShowTip(Resource1.String76, textWaterTemperature);
                btnOK.Enabled = false;
            }
            //if (!validateInput.ValidateUserInput(textTransducerDepth.Text, 12))  //换能器深度
            //{
            //    ShowTip(Resource1.String76, textTransducerDepth);
            //    btnOK.Enabled = false;
            //}
            if (!validateInput.ValidateUserInput(textSoundSpeed.Text, 12))  //声速
            {
                ShowTip(Resource1.String76, textSoundSpeed);
                btnOK.Enabled = false;
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (validateInput.ValidateCharInput(e.KeyChar, ((TextBox)sender).Text))
            {
                e.Handled = true;
            }
        }

        public CAdvancedCfg.AdvancedConfiguration advancedConf = new CAdvancedCfg.AdvancedConfiguration();
        /*
        public AdvancedConfiguration advancedConf;
        public struct AdvancedConfiguration
        {
            public bool ADCPMode;//mode,ADCP/DVL

            public int EsmbIntervalHH, EsmbIntervalMM;
            public float EsmbIntervalSShh;//时间平均步长

            public int BurstInterval_HH, BurstInterval_MM;//Burst Interval
            public float BurstInterval_SS;
            public int BurstInterval_n;

            public bool WaterPingOpen; //水跟踪开关
            public int WaterProfilerMode; //水跟踪带宽，宽带、窄带、脉冲相干等
            public float WPLagLengthV; //LagLength
            public float CWPAP1, CWPAP2, CWPAP3, CWPAP4, CWPAP5;
            public float WPST_Correlation, WPST_QVelocity, WPST_VVelocity; //Water Profile Screening Thresholds
            public float WPBlankSize;//盲区
            public float WPBinSize;//单元尺寸
            public float WPWaterXmt;//脉冲长度
            public int WPBinNum;//单元层数
            public int WPWaterAvgNum;//呯数
            //public int WaterAvgIntervalHH, WaterAvgIntervalMM; //LPJ 2013-9-27 该命令已在I版本中取消
            //public float WaterAvgIntervalSShh;//平均间隔
            public float WPTimeBtwnPings;//呯间隔

            public bool BtmTrkOpen;//底跟踪开关
            public int BTMode;//底跟踪带宽
            public float BTPulseLag, BTLongRangeDepth; //pulse to pulse lag，
            public float BTST_Correlation, BTST_QV, BTST_V;  //LPJ 2012-10-19 add
            public float BTT_SNRshallow, BTT_Depthshallow2deep, BTT_SNRdeep, BTT_Depthlow2high;  //
            public float BtmTrkBlank;//盲区
            public float BtmTrkDepth;//深度
            public float BtmTrkInterval;//呯间隔

            public bool WaterTrkOpen;//水参考开关
            public int WTMode;//水参考带宽
            public float WTBlankSize;//盲区
            public float WTBinSize;//单元尺寸
            public float WTInterval;//呯间隔

            public float CWSCCwaterTemperature, CWSCCTransducerDepth, CWSCCSalinity, CWSCCSpeedOfSound;//Water Speed Of Sound Control 
            public float WaterSalinity; //盐度
            public float WaterTemperature; //水温
            public float TransducerDepth;//换能器深度
            public float SoundSpeed;//声速
            public float HeadingOffset;//艏向偏角
            public float R232; //波特率
            public float R485;

        }
        */
        private CAdvancedCfg.AdvancedConfiguration Default600()
        {
            CAdvancedCfg.AdvancedConfiguration conf = new CAdvancedCfg.AdvancedConfiguration();

            conf.ADCPMode = true;

            conf.EsmbIntervalHH = 0;
            conf.EsmbIntervalMM = 0;
            conf.EsmbIntervalSShh = 1;

            conf.BurstInterval_HH = 0;
            conf.BurstInterval_MM = 0;
            conf.BurstInterval_n = 0;
            conf.BurstInterval_SS = 0.0f;

            conf.WaterPingOpen = true;
            conf.WaterProfilerMode = 1;
            conf.CWPBP_Pings = 1; //LPJ 2013-10-30
            conf.CWPBP_Time = 0.02f;  //LPJ 2013-10-30

            conf.WPBinNum = 30;
            conf.WPST_Correlation = 0.4f;
            conf.WPST_QVelocity = 1;
            conf.WPST_VVelocity = 1;
            conf.WPTimeBtwnPings = 0.25f;
            conf.WPWaterAvgNum = 1;
            conf.WPWaterXmt = 0;
            conf.CWPAP1 = 0;
            conf.CWPAP2 = 0;
            conf.CWPAP3 = 0;
            conf.CWPAP4 = 0;
            conf.CWPAP5 = 0;
         
            conf.BtmTrkOpen = true;
            conf.BTMode = 0;
            conf.BtmTrkInterval = 0.13f;
            conf.BTST_Correlation = 0.9f;
            conf.BTST_QV = 1;
            conf.BTST_V = 1;
            conf.BTT_SNRdeep = 10;
            conf.BTT_SNRshallow = 10;

            conf.WaterTrkOpen = false;
            conf.WTMode = 0;
            conf.WTInterval = 0.25f;

            conf.WaterSalinity = 0;
            conf.WaterTemperature = 15;
            conf.CWSCCSalinity = 0;
            conf.CWSCCSpeedOfSound = 2;
            conf.CWSCCwaterTemperature = 1;

            conf.HeadingOffset = 0;
            conf.R232 = 115200;
            conf.R485 = 115200;
            conf.SoundSpeed = 1500;

            if (bEnglishUnit)
            {
                conf.WPBinSize = (float)projectUnit.MeterToFeet(2, 1);
                conf.WPBlankSize = (float)projectUnit.MeterToFeet(0.2, 1);
                conf.WPLagLengthV = (float)projectUnit.MeterToFeet(0.084, 1);

                conf.BTPulseLag = (float)projectUnit.MeterToFeet(0, 1);
                conf.BTLongRangeDepth = (float)projectUnit.MeterToFeet(50, 1);
                conf.BtmTrkBlank = (float)projectUnit.MeterToFeet(0.1, 1);
                conf.BtmTrkDepth = (float)projectUnit.MeterToFeet(125, 1);

                conf.BTT_Depthlow2high = (float)projectUnit.MeterToFeet(4, 1);
                conf.BTT_Depthshallow2deep = (float)projectUnit.MeterToFeet(50, 1);

                conf.WTBinSize = (float)projectUnit.MeterToFeet(4, 1);
                conf.WTBlankSize = (float)projectUnit.MeterToFeet(4, 1);

                conf.CWSCCTransducerDepth = (float)projectUnit.MeterToFeet(0, 1);
                conf.TransducerDepth = (float)projectUnit.MeterToFeet(0.08, 1);
            }
            else
            {
                conf.WPBinSize = 2;
                conf.WPBlankSize = 0.2f;
                conf.WPLagLengthV = 0.084f;

                conf.BTPulseLag = 0;
                conf.BTLongRangeDepth = 50;
                conf.BtmTrkBlank = 0.1f;
                conf.BtmTrkDepth = 125;

                conf.BTT_Depthlow2high = 4;
                conf.BTT_Depthshallow2deep = 50;

                conf.WTBinSize = 4f;
                conf.WTBlankSize = 4f;

                conf.CWSCCTransducerDepth =0;
                conf.TransducerDepth = 0.08f;
            }

            return conf;
        }

        private CAdvancedCfg.AdvancedConfiguration Default1200()
        {
            CAdvancedCfg.AdvancedConfiguration conf = new CAdvancedCfg.AdvancedConfiguration();

            conf.ADCPMode = true;

            conf.EsmbIntervalHH = 0;
            conf.EsmbIntervalMM = 0;
            conf.EsmbIntervalSShh = 1;

            conf.BurstInterval_HH = 0;
            conf.BurstInterval_MM = 0;
            conf.BurstInterval_n = 0;
            conf.BurstInterval_SS = 0.0f;

            conf.WaterPingOpen = true;
            conf.WaterProfilerMode = 1;
            conf.CWPBP_Pings = 1; //LPJ 2013-10-30
            conf.CWPBP_Time = 0.02f;  //LPJ 2013-10-30

            conf.WPBinNum = 30;
            conf.WPST_Correlation = 0.4f;
            conf.WPST_QVelocity = 1;
            conf.WPST_VVelocity = 1;
            conf.WPTimeBtwnPings = 0.13f;
            conf.WPWaterAvgNum = 1;
            conf.WPWaterXmt = 0;
            conf.CWPAP1 = 0;
            conf.CWPAP2 = 0;
            conf.CWPAP3 = 0;
            conf.CWPAP4 = 0;
            conf.CWPAP5 = 0;

            conf.BtmTrkOpen = true;
            conf.BTMode = 0;
            conf.BtmTrkInterval = 0.05f;
            conf.BTST_Correlation = 0.9f;
            conf.BTST_QV = 1;
            conf.BTST_V = 1;
            conf.BTT_SNRdeep = 5;
            conf.BTT_SNRshallow = 15;

            conf.WaterTrkOpen = false;
            conf.WTMode = 0;
            conf.WTInterval = 0.13f;

            conf.WaterSalinity = 0;
            conf.WaterTemperature = 15;
            conf.CWSCCSalinity = 0;
            conf.CWSCCSpeedOfSound = 2;
            conf.CWSCCwaterTemperature =1;

            conf.HeadingOffset = 0;
            conf.R232 = 115200;
            conf.R485 = 115200;
            conf.SoundSpeed = 1500;

            if (bEnglishUnit)
            {
                conf.WPBinSize = (float)projectUnit.MeterToFeet(1, 1);
                conf.WPBlankSize = (float)projectUnit.MeterToFeet(0.1, 1);
                conf.WPLagLengthV = (float)projectUnit.MeterToFeet(0.042, 1);

                conf.BTPulseLag = (float)projectUnit.MeterToFeet(0, 1);
                conf.BTLongRangeDepth = (float)projectUnit.MeterToFeet(30, 1);
                conf.BtmTrkBlank = (float)projectUnit.MeterToFeet(0.05, 1);
                conf.BtmTrkDepth = (float)projectUnit.MeterToFeet(75, 1);

                conf.BTT_Depthlow2high = (float)projectUnit.MeterToFeet(2, 1);
                conf.BTT_Depthshallow2deep = (float)projectUnit.MeterToFeet(25, 1);

                conf.WTBinSize = (float)projectUnit.MeterToFeet(2, 1);
                conf.WTBlankSize = (float)projectUnit.MeterToFeet(2, 1);

                conf.CWSCCTransducerDepth = (float)projectUnit.MeterToFeet(0, 1);
                conf.TransducerDepth = (float)projectUnit.MeterToFeet(0.07, 1);
            }
            else
            {
                conf.WPBinSize = 1;
                conf.WPBlankSize = 0.1f;
                conf.WPLagLengthV = 0.042f;

                conf.BTPulseLag = 0;
                conf.BTLongRangeDepth = 30;
                conf.BtmTrkBlank = 0.05f;
                conf.BtmTrkDepth = 75;

                conf.BTT_Depthlow2high = 2;
                conf.BTT_Depthshallow2deep = 25;

                conf.WTBinSize = 2.0f;
                conf.WTBlankSize = 2.0f;

                conf.CWSCCTransducerDepth = 0;
                conf.TransducerDepth = 0.07f;
            }

            return conf;
        }

        private CAdvancedCfg.AdvancedConfiguration Default300()
        {
            CAdvancedCfg.AdvancedConfiguration conf = new CAdvancedCfg.AdvancedConfiguration();

            conf.ADCPMode = true;

            conf.EsmbIntervalHH = 0;
            conf.EsmbIntervalMM = 0;
            conf.EsmbIntervalSShh = 1;

            conf.BurstInterval_HH = 0;
            conf.BurstInterval_MM = 0;
            conf.BurstInterval_n = 0;
            conf.BurstInterval_SS = 0.0f;

            conf.WaterPingOpen = true;
            conf.WaterProfilerMode = 1;
            conf.CWPBP_Pings = 1; //LPJ 2013-10-30
            conf.CWPBP_Time = 0.08f;  //LPJ 2013-10-30
          
            conf.WPBinNum = 30;
            conf.WPST_Correlation = 0.25f;
            conf.WPST_QVelocity = 10;
            conf.WPST_VVelocity = 10;
            conf.WPTimeBtwnPings = 0.5f;
            conf.WPWaterAvgNum = 1;
            conf.WPWaterXmt = 0;
            conf.CWPAP1 = 0;
            conf.CWPAP2 = 0;
            conf.CWPAP3 = 0;
            conf.CWPAP4 = 0;
            conf.CWPAP5 = 0;

            conf.BtmTrkOpen = true;
            conf.BTMode = 0;
            conf.BtmTrkInterval = 0.25f;
            conf.BTST_Correlation = 0.9f;
            conf.BTST_QV = 10;
            conf.BTST_V = 10;
            conf.BTT_SNRshallow = 20;
            conf.BTT_SNRdeep = 20;
            
            conf.WaterTrkOpen = false;
            conf.WTMode = 0;
            conf.WTInterval = 0.4f;

            conf.WaterSalinity = 0;
            conf.WaterTemperature = 15;
            conf.CWSCCSalinity = 0;
            conf.CWSCCSpeedOfSound = 2;
            conf.CWSCCwaterTemperature = 1;
            
            conf.HeadingOffset = 0;
            conf.R232 = 115200;
            conf.R485 = 115200;
            conf.SoundSpeed = 1500;

            if (bEnglishUnit)
            {
                conf.WPBinSize = (float)projectUnit.MeterToFeet(4, 1);
                conf.WPBlankSize = (float)projectUnit.MeterToFeet(0.4, 1);
                conf.WPLagLengthV = (float)projectUnit.MeterToFeet(0.168, 1);

                conf.BTPulseLag = (float)projectUnit.MeterToFeet(0, 1);
                conf.BTLongRangeDepth = (float)projectUnit.MeterToFeet(100, 1);
                conf.BtmTrkBlank = (float)projectUnit.MeterToFeet(0.2f, 1);
                conf.BtmTrkDepth = (float)projectUnit.MeterToFeet(300, 1);
                
                conf.BTT_Depthlow2high = (float)projectUnit.MeterToFeet(8, 1);
                conf.BTT_Depthshallow2deep = (float)projectUnit.MeterToFeet(100, 1);

                conf.WTBinSize = (float)projectUnit.MeterToFeet(8, 1);
                conf.WTBlankSize = (float)projectUnit.MeterToFeet(8, 1);

                conf.CWSCCTransducerDepth = (float)projectUnit.MeterToFeet(0, 1);
                conf.TransducerDepth = (float)projectUnit.MeterToFeet(0.00, 1);
            }
            else
            {
                conf.WPBinSize = 4;
                conf.WPBlankSize = 0.4f;
                conf.WPLagLengthV = 0.168f;

                conf.BTPulseLag = 0;
                conf.BTLongRangeDepth =100;
                conf.BtmTrkBlank = 0.2f;
                conf.BtmTrkDepth = 300;
                
                conf.BTT_Depthlow2high = 8;
                conf.BTT_Depthshallow2deep = 100;

                conf.WTBinSize = 8f;
                conf.WTBlankSize = 8f;

                conf.CWSCCTransducerDepth = 0;
                conf.TransducerDepth = 0.00f;
            }

            return conf;
        }

        private int systemNumber = 0;
        private void btnDefault_Click(object sender, EventArgs e) //LPJ 2013-9-27 当用户选择该按钮，读取默认参数，根据仪器型号
        {
            #region 取消
           /*
            switch (systemNumber)
            {
                case 0:
                    {
                        CAdvancedCfg.AdvancedConfiguration advanced = Default300();
                        GetConfiguration(advanced);
                        break;
                    }
                case 1:
                    {
                        CAdvancedCfg.AdvancedConfiguration advanced = Default600();
                        GetConfiguration(advanced);
                        break;
                    }
                case 2:
                    {
                        CAdvancedCfg.AdvancedConfiguration advanced = Default1200();
                        GetConfiguration(advanced);
                        break;
                    }
                default:
                    {
                        CAdvancedCfg.AdvancedConfiguration advanced = Default300();
                        GetConfiguration(advanced);
                        break;
                    }
            }
            */
            #endregion

            //CAdvancedCfg.AdvancedConfiguration advanced = Default1200();
            //GetConfiguration(advanced);

            #region 发送default命令，从cshow中读取
          
            CDefault defaultCfg = new CDefault(sp);
            CAdvancedCfg.AdvancedConfiguration advanced = new CAdvancedCfg.AdvancedConfiguration();
            if (defaultCfg.SendCDEFULATCommand(systemNumber, ref advanced))
            {
                GetConfiguration(advanced);
            }
           
            #endregion

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //LPJ2013-10-17 将该配置文件导出
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "(*.cfg)|*.cfg";
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string str = file.FileName;
                if (str != "")
                {
                    CAdvancedCfg.AdvancedConfiguration advanced = new CAdvancedCfg.AdvancedConfiguration();
                    CAdvancedCfg cAdvcancedcfg = new CAdvancedCfg();
                    if (SetConfiguration(ref advanced))
                        cAdvcancedcfg.WriteAdvancedModeToFile(str, advanced);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //LPJ2013-10-17 外部导入配置文件
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "(*.cfg)|*.cfg";
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                CAdvancedCfg.AdvancedConfiguration advanced = new CAdvancedCfg.AdvancedConfiguration();
                CAdvancedCfg cAdvcancedcfg = new CAdvancedCfg();
                cAdvcancedcfg.GetFileToAdvancedMode(file.FileName, ref advanced);
                GetConfiguration(advanced);
            }
        }

        private void Broadband_Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (Broadband_Mode.Checked)
            {
                textBox_CWPBP_Pings.Enabled = true;
                textBox_CWPBP_Time.Enabled = true;
            }
            else
            {
                textBox_CWPBP_Pings.Enabled = false;
                textBox_CWPBP_Time.Enabled = false;
            }
        }

        private void Narrowband_Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (Narrowband_Mode.Checked)
            {
                textBox_CWPBP_Pings.Enabled = true;
                textBox_CWPBP_Time.Enabled = true;
            }
            else
            {
                textBox_CWPBP_Pings.Enabled = false;
                textBox_CWPBP_Time.Enabled = false;
            }
        }

      
    }
}
