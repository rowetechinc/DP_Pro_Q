namespace ADCP
{
    partial class FrmSystemSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSystemSetting));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxVesselSpeedRef = new System.Windows.Forms.ComboBox();
            this.comboBoxHeadingRef = new System.Windows.Forms.ComboBox();
            this.comboBoxMeasMode = new System.Windows.Forms.ComboBox();
            this.textBoxTransducerDepth = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_RS232 = new System.Windows.Forms.ComboBox();
            this.label_RS232 = new System.Windows.Forms.Label();
            this.groupBox44 = new System.Windows.Forms.GroupBox();
            this.comboBox_SpeedOfSound = new System.Windows.Forms.ComboBox();
            this.comboBox_Salinity = new System.Windows.Forms.ComboBox();
            this.comboBox_TransducerDepth = new System.Windows.Forms.ComboBox();
            this.comboBox_waterTemperature = new System.Windows.Forms.ComboBox();
            this.label239 = new System.Windows.Forms.Label();
            this.label240 = new System.Windows.Forms.Label();
            this.label241 = new System.Windows.Forms.Label();
            this.label242 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.textHeadingOffset = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.textSoundSpeed = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.textWaterTemperature = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.textWaterSalinity = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxMaxDepth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox41 = new System.Windows.Forms.GroupBox();
            this.buttonSetDefaults = new System.Windows.Forms.Button();
            this.buttonDownloadCommandSettings = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxBTSNR = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxAutoLag = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxBTswitchDepth = new System.Windows.Forms.TextBox();
            this.textBoxWPswitchDepth = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxWPaveragingInterval = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxNumberOfBins = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxAutoBinSize = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxVerticalBeam = new System.Windows.Forms.ComboBox();
            this.BTST_Correlation_text = new System.Windows.Forms.TextBox();
            this.label221 = new System.Windows.Forms.Label();
            this.groupBox44.SuspendLayout();
            this.groupBox41.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // comboBoxVesselSpeedRef
            // 
            this.comboBoxVesselSpeedRef.FormattingEnabled = true;
            this.comboBoxVesselSpeedRef.Items.AddRange(new object[] {
            resources.GetString("comboBoxVesselSpeedRef.Items"),
            resources.GetString("comboBoxVesselSpeedRef.Items1"),
            resources.GetString("comboBoxVesselSpeedRef.Items2"),
            resources.GetString("comboBoxVesselSpeedRef.Items3")});
            resources.ApplyResources(this.comboBoxVesselSpeedRef, "comboBoxVesselSpeedRef");
            this.comboBoxVesselSpeedRef.Name = "comboBoxVesselSpeedRef";
            this.comboBoxVesselSpeedRef.SelectedIndexChanged += new System.EventHandler(this.comboBoxVesselSpeedRef_SelectedIndexChanged);
            // 
            // comboBoxHeadingRef
            // 
            this.comboBoxHeadingRef.FormattingEnabled = true;
            this.comboBoxHeadingRef.Items.AddRange(new object[] {
            resources.GetString("comboBoxHeadingRef.Items"),
            resources.GetString("comboBoxHeadingRef.Items1"),
            resources.GetString("comboBoxHeadingRef.Items2")});
            resources.ApplyResources(this.comboBoxHeadingRef, "comboBoxHeadingRef");
            this.comboBoxHeadingRef.Name = "comboBoxHeadingRef";
            this.comboBoxHeadingRef.SelectedIndexChanged += new System.EventHandler(this.comboBoxHeadingRef_SelectedIndexChanged);
            // 
            // comboBoxMeasMode
            // 
            this.comboBoxMeasMode.FormattingEnabled = true;
            this.comboBoxMeasMode.Items.AddRange(new object[] {
            resources.GetString("comboBoxMeasMode.Items"),
            resources.GetString("comboBoxMeasMode.Items1"),
            resources.GetString("comboBoxMeasMode.Items2"),
            resources.GetString("comboBoxMeasMode.Items3")});
            resources.ApplyResources(this.comboBoxMeasMode, "comboBoxMeasMode");
            this.comboBoxMeasMode.Name = "comboBoxMeasMode";
            this.comboBoxMeasMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMeasMode_SelectedIndexChanged);
            // 
            // textBoxTransducerDepth
            // 
            resources.ApplyResources(this.textBoxTransducerDepth, "textBoxTransducerDepth");
            this.textBoxTransducerDepth.Name = "textBoxTransducerDepth";
            this.textBoxTransducerDepth.TextChanged += new System.EventHandler(this.textBoxTransducerDepth_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // comboBox_RS232
            // 
            this.comboBox_RS232.FormattingEnabled = true;
            this.comboBox_RS232.Items.AddRange(new object[] {
            resources.GetString("comboBox_RS232.Items"),
            resources.GetString("comboBox_RS232.Items1"),
            resources.GetString("comboBox_RS232.Items2"),
            resources.GetString("comboBox_RS232.Items3"),
            resources.GetString("comboBox_RS232.Items4"),
            resources.GetString("comboBox_RS232.Items5"),
            resources.GetString("comboBox_RS232.Items6")});
            resources.ApplyResources(this.comboBox_RS232, "comboBox_RS232");
            this.comboBox_RS232.Name = "comboBox_RS232";
            this.comboBox_RS232.SelectedIndexChanged += new System.EventHandler(this.comboBox_RS232_SelectedIndexChanged);
            // 
            // label_RS232
            // 
            resources.ApplyResources(this.label_RS232, "label_RS232");
            this.label_RS232.Name = "label_RS232";
            // 
            // groupBox44
            // 
            this.groupBox44.Controls.Add(this.comboBox_SpeedOfSound);
            this.groupBox44.Controls.Add(this.comboBox_Salinity);
            this.groupBox44.Controls.Add(this.comboBox_TransducerDepth);
            this.groupBox44.Controls.Add(this.comboBox_waterTemperature);
            this.groupBox44.Controls.Add(this.label239);
            this.groupBox44.Controls.Add(this.label240);
            this.groupBox44.Controls.Add(this.label241);
            this.groupBox44.Controls.Add(this.label242);
            resources.ApplyResources(this.groupBox44, "groupBox44");
            this.groupBox44.Name = "groupBox44";
            this.groupBox44.TabStop = false;
            // 
            // comboBox_SpeedOfSound
            // 
            this.comboBox_SpeedOfSound.FormattingEnabled = true;
            this.comboBox_SpeedOfSound.Items.AddRange(new object[] {
            resources.GetString("comboBox_SpeedOfSound.Items"),
            resources.GetString("comboBox_SpeedOfSound.Items1"),
            resources.GetString("comboBox_SpeedOfSound.Items2")});
            resources.ApplyResources(this.comboBox_SpeedOfSound, "comboBox_SpeedOfSound");
            this.comboBox_SpeedOfSound.Name = "comboBox_SpeedOfSound";
            this.comboBox_SpeedOfSound.SelectedIndexChanged += new System.EventHandler(this.comboBox_SpeedOfSound_SelectedIndexChanged);
            // 
            // comboBox_Salinity
            // 
            this.comboBox_Salinity.FormattingEnabled = true;
            this.comboBox_Salinity.Items.AddRange(new object[] {
            resources.GetString("comboBox_Salinity.Items"),
            resources.GetString("comboBox_Salinity.Items1"),
            resources.GetString("comboBox_Salinity.Items2")});
            resources.ApplyResources(this.comboBox_Salinity, "comboBox_Salinity");
            this.comboBox_Salinity.Name = "comboBox_Salinity";
            this.comboBox_Salinity.SelectedIndexChanged += new System.EventHandler(this.comboBox_Salinity_SelectedIndexChanged);
            // 
            // comboBox_TransducerDepth
            // 
            this.comboBox_TransducerDepth.FormattingEnabled = true;
            this.comboBox_TransducerDepth.Items.AddRange(new object[] {
            resources.GetString("comboBox_TransducerDepth.Items"),
            resources.GetString("comboBox_TransducerDepth.Items1"),
            resources.GetString("comboBox_TransducerDepth.Items2")});
            resources.ApplyResources(this.comboBox_TransducerDepth, "comboBox_TransducerDepth");
            this.comboBox_TransducerDepth.Name = "comboBox_TransducerDepth";
            this.comboBox_TransducerDepth.SelectedIndexChanged += new System.EventHandler(this.comboBox_TransducerDepth_SelectedIndexChanged);
            // 
            // comboBox_waterTemperature
            // 
            this.comboBox_waterTemperature.FormattingEnabled = true;
            this.comboBox_waterTemperature.Items.AddRange(new object[] {
            resources.GetString("comboBox_waterTemperature.Items"),
            resources.GetString("comboBox_waterTemperature.Items1"),
            resources.GetString("comboBox_waterTemperature.Items2")});
            resources.ApplyResources(this.comboBox_waterTemperature, "comboBox_waterTemperature");
            this.comboBox_waterTemperature.Name = "comboBox_waterTemperature";
            this.comboBox_waterTemperature.SelectedIndexChanged += new System.EventHandler(this.comboBox_waterTemperature_SelectedIndexChanged);
            // 
            // label239
            // 
            resources.ApplyResources(this.label239, "label239");
            this.label239.Name = "label239";
            // 
            // label240
            // 
            resources.ApplyResources(this.label240, "label240");
            this.label240.Name = "label240";
            // 
            // label241
            // 
            resources.ApplyResources(this.label241, "label241");
            this.label241.Name = "label241";
            // 
            // label242
            // 
            resources.ApplyResources(this.label242, "label242");
            this.label242.Name = "label242";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // textHeadingOffset
            // 
            resources.ApplyResources(this.textHeadingOffset, "textHeadingOffset");
            this.textHeadingOffset.Name = "textHeadingOffset";
            this.textHeadingOffset.TextChanged += new System.EventHandler(this.textHeadingOffset_TextChanged);
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.Name = "label46";
            // 
            // textSoundSpeed
            // 
            resources.ApplyResources(this.textSoundSpeed, "textSoundSpeed");
            this.textSoundSpeed.Name = "textSoundSpeed";
            this.textSoundSpeed.TextChanged += new System.EventHandler(this.textSoundSpeed_TextChanged);
            // 
            // label49
            // 
            resources.ApplyResources(this.label49, "label49");
            this.label49.Name = "label49";
            // 
            // textWaterTemperature
            // 
            resources.ApplyResources(this.textWaterTemperature, "textWaterTemperature");
            this.textWaterTemperature.Name = "textWaterTemperature";
            this.textWaterTemperature.TextChanged += new System.EventHandler(this.textWaterTemperature_TextChanged);
            // 
            // label52
            // 
            resources.ApplyResources(this.label52, "label52");
            this.label52.Name = "label52";
            // 
            // textWaterSalinity
            // 
            resources.ApplyResources(this.textWaterSalinity, "textWaterSalinity");
            this.textWaterSalinity.Name = "textWaterSalinity";
            this.textWaterSalinity.TextChanged += new System.EventHandler(this.textWaterSalinity_TextChanged);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // textBoxMaxDepth
            // 
            resources.ApplyResources(this.textBoxMaxDepth, "textBoxMaxDepth");
            this.textBoxMaxDepth.Name = "textBoxMaxDepth";
            this.textBoxMaxDepth.TextChanged += new System.EventHandler(this.textBoxMaxDepth_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox41
            // 
            this.groupBox41.Controls.Add(this.buttonSetDefaults);
            this.groupBox41.Controls.Add(this.buttonDownloadCommandSettings);
            this.groupBox41.Controls.Add(this.label14);
            this.groupBox41.Controls.Add(this.label13);
            this.groupBox41.Controls.Add(this.textBoxBTSNR);
            this.groupBox41.Controls.Add(this.label12);
            this.groupBox41.Controls.Add(this.comboBoxAutoLag);
            this.groupBox41.Controls.Add(this.groupBox1);
            this.groupBox41.Controls.Add(this.label52);
            this.groupBox41.Controls.Add(this.btnOK);
            this.groupBox41.Controls.Add(this.btnCancel);
            this.groupBox41.Controls.Add(this.comboBox_RS232);
            this.groupBox41.Controls.Add(this.label_RS232);
            this.groupBox41.Controls.Add(this.groupBox44);
            this.groupBox41.Controls.Add(this.label9);
            this.groupBox41.Controls.Add(this.label23);
            this.groupBox41.Controls.Add(this.textHeadingOffset);
            this.groupBox41.Controls.Add(this.comboBoxHeadingRef);
            this.groupBox41.Controls.Add(this.comboBoxVesselSpeedRef);
            this.groupBox41.Controls.Add(this.label3);
            this.groupBox41.Controls.Add(this.label2);
            this.groupBox41.Controls.Add(this.label22);
            this.groupBox41.Controls.Add(this.label21);
            this.groupBox41.Controls.Add(this.label20);
            this.groupBox41.Controls.Add(this.textBoxBTswitchDepth);
            this.groupBox41.Controls.Add(this.textBoxWPswitchDepth);
            this.groupBox41.Controls.Add(this.label19);
            this.groupBox41.Controls.Add(this.textBoxWPaveragingInterval);
            this.groupBox41.Controls.Add(this.label1);
            this.groupBox41.Controls.Add(this.label16);
            this.groupBox41.Controls.Add(this.label17);
            this.groupBox41.Controls.Add(this.textBoxMaxDepth);
            this.groupBox41.Controls.Add(this.label4);
            this.groupBox41.Controls.Add(this.textBoxTransducerDepth);
            this.groupBox41.Controls.Add(this.label6);
            this.groupBox41.Controls.Add(this.textBoxNumberOfBins);
            this.groupBox41.Controls.Add(this.label11);
            this.groupBox41.Controls.Add(this.comboBoxAutoBinSize);
            this.groupBox41.Controls.Add(this.label10);
            this.groupBox41.Controls.Add(this.comboBoxVerticalBeam);
            this.groupBox41.Controls.Add(this.BTST_Correlation_text);
            this.groupBox41.Controls.Add(this.label221);
            this.groupBox41.Controls.Add(this.comboBoxMeasMode);
            this.groupBox41.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox41, "groupBox41");
            this.groupBox41.Name = "groupBox41";
            this.groupBox41.TabStop = false;
            // 
            // buttonSetDefaults
            // 
            resources.ApplyResources(this.buttonSetDefaults, "buttonSetDefaults");
            this.buttonSetDefaults.Name = "buttonSetDefaults";
            this.buttonSetDefaults.UseVisualStyleBackColor = true;
            this.buttonSetDefaults.Click += new System.EventHandler(this.buttonSetDefaults_Click);
            // 
            // buttonDownloadCommandSettings
            // 
            resources.ApplyResources(this.buttonDownloadCommandSettings, "buttonDownloadCommandSettings");
            this.buttonDownloadCommandSettings.Name = "buttonDownloadCommandSettings";
            this.buttonDownloadCommandSettings.UseVisualStyleBackColor = true;
            this.buttonDownloadCommandSettings.Click += new System.EventHandler(this.buttonDownloadCommandSettings_Click);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // textBoxBTSNR
            // 
            resources.ApplyResources(this.textBoxBTSNR, "textBoxBTSNR");
            this.textBoxBTSNR.Name = "textBoxBTSNR";
            this.textBoxBTSNR.TextChanged += new System.EventHandler(this.textBoxBTSNR_TextChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // comboBoxAutoLag
            // 
            this.comboBoxAutoLag.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("comboBoxAutoLag.AutoCompleteCustomSource"),
            resources.GetString("comboBoxAutoLag.AutoCompleteCustomSource1")});
            this.comboBoxAutoLag.FormattingEnabled = true;
            this.comboBoxAutoLag.Items.AddRange(new object[] {
            resources.GetString("comboBoxAutoLag.Items"),
            resources.GetString("comboBoxAutoLag.Items1")});
            resources.ApplyResources(this.comboBoxAutoLag, "comboBoxAutoLag");
            this.comboBoxAutoLag.Name = "comboBoxAutoLag";
            this.comboBoxAutoLag.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoLag_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.textWaterSalinity);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textWaterTemperature);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label46);
            this.groupBox1.Controls.Add(this.textSoundSpeed);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label49);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // textBoxBTswitchDepth
            // 
            resources.ApplyResources(this.textBoxBTswitchDepth, "textBoxBTswitchDepth");
            this.textBoxBTswitchDepth.Name = "textBoxBTswitchDepth";
            this.textBoxBTswitchDepth.TextChanged += new System.EventHandler(this.textBoxBTswitchDepth_TextChanged);
            // 
            // textBoxWPswitchDepth
            // 
            resources.ApplyResources(this.textBoxWPswitchDepth, "textBoxWPswitchDepth");
            this.textBoxWPswitchDepth.Name = "textBoxWPswitchDepth";
            this.textBoxWPswitchDepth.TextChanged += new System.EventHandler(this.textBoxWPswitchDepth_TextChanged);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // textBoxWPaveragingInterval
            // 
            resources.ApplyResources(this.textBoxWPaveragingInterval, "textBoxWPaveragingInterval");
            this.textBoxWPaveragingInterval.Name = "textBoxWPaveragingInterval";
            this.textBoxWPaveragingInterval.TextChanged += new System.EventHandler(this.textBoxWPaveragingInterval_TextChanged);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // textBoxNumberOfBins
            // 
            resources.ApplyResources(this.textBoxNumberOfBins, "textBoxNumberOfBins");
            this.textBoxNumberOfBins.Name = "textBoxNumberOfBins";
            this.textBoxNumberOfBins.TextChanged += new System.EventHandler(this.textBoxNumberOfBins_TextChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // comboBoxAutoBinSize
            // 
            this.comboBoxAutoBinSize.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("comboBoxAutoBinSize.AutoCompleteCustomSource"),
            resources.GetString("comboBoxAutoBinSize.AutoCompleteCustomSource1")});
            this.comboBoxAutoBinSize.FormattingEnabled = true;
            this.comboBoxAutoBinSize.Items.AddRange(new object[] {
            resources.GetString("comboBoxAutoBinSize.Items"),
            resources.GetString("comboBoxAutoBinSize.Items1")});
            resources.ApplyResources(this.comboBoxAutoBinSize, "comboBoxAutoBinSize");
            this.comboBoxAutoBinSize.Name = "comboBoxAutoBinSize";
            this.comboBoxAutoBinSize.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoBinSize_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // comboBoxVerticalBeam
            // 
            this.comboBoxVerticalBeam.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("comboBoxVerticalBeam.AutoCompleteCustomSource"),
            resources.GetString("comboBoxVerticalBeam.AutoCompleteCustomSource1")});
            this.comboBoxVerticalBeam.FormattingEnabled = true;
            this.comboBoxVerticalBeam.Items.AddRange(new object[] {
            resources.GetString("comboBoxVerticalBeam.Items"),
            resources.GetString("comboBoxVerticalBeam.Items1")});
            resources.ApplyResources(this.comboBoxVerticalBeam, "comboBoxVerticalBeam");
            this.comboBoxVerticalBeam.Name = "comboBoxVerticalBeam";
            this.comboBoxVerticalBeam.SelectedIndexChanged += new System.EventHandler(this.comboBoxVerticalBeam_SelectedIndexChanged);
            // 
            // BTST_Correlation_text
            // 
            resources.ApplyResources(this.BTST_Correlation_text, "BTST_Correlation_text");
            this.BTST_Correlation_text.Name = "BTST_Correlation_text";
            this.BTST_Correlation_text.TextChanged += new System.EventHandler(this.BTST_Correlation_text_TextChanged);
            // 
            // label221
            // 
            resources.ApplyResources(this.label221, "label221");
            this.label221.Name = "label221";
            // 
            // FrmSystemSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox41);
            this.Controls.Add(this.label7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSystemSetting";
            this.ShowIcon = false;
            this.groupBox44.ResumeLayout(false);
            this.groupBox44.PerformLayout();
            this.groupBox41.ResumeLayout(false);
            this.groupBox41.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxVesselSpeedRef;
        private System.Windows.Forms.ComboBox comboBoxHeadingRef;
        private System.Windows.Forms.ComboBox comboBoxMeasMode;
        private System.Windows.Forms.TextBox textBoxTransducerDepth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_RS232;
        private System.Windows.Forms.Label label_RS232;
        private System.Windows.Forms.GroupBox groupBox44;
        private System.Windows.Forms.ComboBox comboBox_SpeedOfSound;
        private System.Windows.Forms.ComboBox comboBox_Salinity;
        private System.Windows.Forms.ComboBox comboBox_TransducerDepth;
        private System.Windows.Forms.ComboBox comboBox_waterTemperature;
        private System.Windows.Forms.Label label239;
        private System.Windows.Forms.Label label240;
        private System.Windows.Forms.Label label241;
        private System.Windows.Forms.Label label242;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textHeadingOffset;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textWaterTemperature;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox textWaterSalinity;
        private System.Windows.Forms.GroupBox groupBox41;
        private System.Windows.Forms.TextBox BTST_Correlation_text;
        private System.Windows.Forms.Label label221;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxMaxDepth;
        public System.Windows.Forms.TextBox textSoundSpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxWPaveragingInterval;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxNumberOfBins;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxAutoBinSize;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxVerticalBeam;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxBTswitchDepth;
        private System.Windows.Forms.TextBox textBoxWPswitchDepth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxAutoLag;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxBTSNR;
        private System.Windows.Forms.Button buttonDownloadCommandSettings;
        private System.Windows.Forms.Button buttonSetDefaults;
    }
}