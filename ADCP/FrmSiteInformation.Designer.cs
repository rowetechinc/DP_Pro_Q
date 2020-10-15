namespace ADCP
{
    partial class FrmSiteInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSiteInformation));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textBoxSiteName = new System.Windows.Forms.TextBox();
            this.textBoxStationNumber = new System.Windows.Forms.TextBox();
            this.textBoxMeasNumber = new System.Windows.Forms.TextBox();
            this.textBoxComments = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxDeploymentType = new System.Windows.Forms.ComboBox();
            this.textBoxMeasLocation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxBoatMotor = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxProcessedBy = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxFieldParty = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.comboBoxMeasurementRating = new System.Windows.Forms.ComboBox();
            this.comboBoxControlCode2 = new System.Windows.Forms.ComboBox();
            this.comboBoxControlCode3 = new System.Windows.Forms.ComboBox();
            this.comboBoxControlCode1 = new System.Windows.Forms.ComboBox();
            this.comboBoxMagnVariationMethod = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxWaterTemp = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxRatingNumber = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxRatedArea = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxIndexV = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxRatingDischarge = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxGageHChange = new System.Windows.Forms.TextBox();
            this.textBoxOutsideGageH = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxInsideGageH = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // textBoxSiteName
            // 
            resources.ApplyResources(this.textBoxSiteName, "textBoxSiteName");
            this.textBoxSiteName.Name = "textBoxSiteName";
            // 
            // textBoxStationNumber
            // 
            resources.ApplyResources(this.textBoxStationNumber, "textBoxStationNumber");
            this.textBoxStationNumber.Name = "textBoxStationNumber";
            // 
            // textBoxMeasNumber
            // 
            resources.ApplyResources(this.textBoxMeasNumber, "textBoxMeasNumber");
            this.textBoxMeasNumber.Name = "textBoxMeasNumber";
            // 
            // textBoxComments
            // 
            resources.ApplyResources(this.textBoxComments, "textBoxComments");
            this.textBoxComments.Name = "textBoxComments";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSiteName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxMeasNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxStationNumber);
            this.groupBox1.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxDeploymentType);
            this.groupBox2.Controls.Add(this.textBoxMeasLocation);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBoxBoatMotor);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBoxProcessedBy);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxFieldParty);
            this.groupBox2.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // comboBoxDeploymentType
            // 
            this.comboBoxDeploymentType.FormattingEnabled = true;
            this.comboBoxDeploymentType.Items.AddRange(new object[] {
            resources.GetString("comboBoxDeploymentType.Items"),
            resources.GetString("comboBoxDeploymentType.Items1"),
            resources.GetString("comboBoxDeploymentType.Items2"),
            resources.GetString("comboBoxDeploymentType.Items3")});
            resources.ApplyResources(this.comboBoxDeploymentType, "comboBoxDeploymentType");
            this.comboBoxDeploymentType.Name = "comboBoxDeploymentType";
            // 
            // textBoxMeasLocation
            // 
            resources.ApplyResources(this.textBoxMeasLocation, "textBoxMeasLocation");
            this.textBoxMeasLocation.Name = "textBoxMeasLocation";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // textBoxBoatMotor
            // 
            resources.ApplyResources(this.textBoxBoatMotor, "textBoxBoatMotor");
            this.textBoxBoatMotor.Name = "textBoxBoatMotor";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // textBoxProcessedBy
            // 
            resources.ApplyResources(this.textBoxProcessedBy, "textBoxProcessedBy");
            this.textBoxProcessedBy.Name = "textBoxProcessedBy";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // textBoxFieldParty
            // 
            resources.ApplyResources(this.textBoxFieldParty, "textBoxFieldParty");
            this.textBoxFieldParty.Name = "textBoxFieldParty";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxComments);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton2);
            this.groupBox4.Controls.Add(this.radioButton1);
            this.groupBox4.Controls.Add(this.comboBoxMeasurementRating);
            this.groupBox4.Controls.Add(this.comboBoxControlCode2);
            this.groupBox4.Controls.Add(this.comboBoxControlCode3);
            this.groupBox4.Controls.Add(this.comboBoxControlCode1);
            this.groupBox4.Controls.Add(this.comboBoxMagnVariationMethod);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.textBoxWaterTemp);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.textBoxRatingNumber);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.textBoxRatedArea);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBoxIndexV);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.textBoxRatingDischarge);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.textBoxGageHChange);
            this.groupBox4.Controls.Add(this.textBoxOutsideGageH);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.textBoxInsideGageH);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // comboBoxMeasurementRating
            // 
            this.comboBoxMeasurementRating.FormattingEnabled = true;
            this.comboBoxMeasurementRating.Items.AddRange(new object[] {
            resources.GetString("comboBoxMeasurementRating.Items"),
            resources.GetString("comboBoxMeasurementRating.Items1"),
            resources.GetString("comboBoxMeasurementRating.Items2"),
            resources.GetString("comboBoxMeasurementRating.Items3"),
            resources.GetString("comboBoxMeasurementRating.Items4")});
            resources.ApplyResources(this.comboBoxMeasurementRating, "comboBoxMeasurementRating");
            this.comboBoxMeasurementRating.Name = "comboBoxMeasurementRating";
            // 
            // comboBoxControlCode2
            // 
            this.comboBoxControlCode2.FormattingEnabled = true;
            this.comboBoxControlCode2.Items.AddRange(new object[] {
            resources.GetString("comboBoxControlCode2.Items"),
            resources.GetString("comboBoxControlCode2.Items1"),
            resources.GetString("comboBoxControlCode2.Items2"),
            resources.GetString("comboBoxControlCode2.Items3"),
            resources.GetString("comboBoxControlCode2.Items4"),
            resources.GetString("comboBoxControlCode2.Items5"),
            resources.GetString("comboBoxControlCode2.Items6"),
            resources.GetString("comboBoxControlCode2.Items7"),
            resources.GetString("comboBoxControlCode2.Items8"),
            resources.GetString("comboBoxControlCode2.Items9"),
            resources.GetString("comboBoxControlCode2.Items10"),
            resources.GetString("comboBoxControlCode2.Items11"),
            resources.GetString("comboBoxControlCode2.Items12"),
            resources.GetString("comboBoxControlCode2.Items13"),
            resources.GetString("comboBoxControlCode2.Items14")});
            resources.ApplyResources(this.comboBoxControlCode2, "comboBoxControlCode2");
            this.comboBoxControlCode2.Name = "comboBoxControlCode2";
            // 
            // comboBoxControlCode3
            // 
            this.comboBoxControlCode3.FormattingEnabled = true;
            this.comboBoxControlCode3.Items.AddRange(new object[] {
            resources.GetString("comboBoxControlCode3.Items"),
            resources.GetString("comboBoxControlCode3.Items1"),
            resources.GetString("comboBoxControlCode3.Items2"),
            resources.GetString("comboBoxControlCode3.Items3"),
            resources.GetString("comboBoxControlCode3.Items4"),
            resources.GetString("comboBoxControlCode3.Items5"),
            resources.GetString("comboBoxControlCode3.Items6"),
            resources.GetString("comboBoxControlCode3.Items7"),
            resources.GetString("comboBoxControlCode3.Items8"),
            resources.GetString("comboBoxControlCode3.Items9"),
            resources.GetString("comboBoxControlCode3.Items10"),
            resources.GetString("comboBoxControlCode3.Items11"),
            resources.GetString("comboBoxControlCode3.Items12"),
            resources.GetString("comboBoxControlCode3.Items13"),
            resources.GetString("comboBoxControlCode3.Items14")});
            resources.ApplyResources(this.comboBoxControlCode3, "comboBoxControlCode3");
            this.comboBoxControlCode3.Name = "comboBoxControlCode3";
            // 
            // comboBoxControlCode1
            // 
            this.comboBoxControlCode1.FormattingEnabled = true;
            this.comboBoxControlCode1.Items.AddRange(new object[] {
            resources.GetString("comboBoxControlCode1.Items"),
            resources.GetString("comboBoxControlCode1.Items1"),
            resources.GetString("comboBoxControlCode1.Items2"),
            resources.GetString("comboBoxControlCode1.Items3"),
            resources.GetString("comboBoxControlCode1.Items4"),
            resources.GetString("comboBoxControlCode1.Items5"),
            resources.GetString("comboBoxControlCode1.Items6"),
            resources.GetString("comboBoxControlCode1.Items7"),
            resources.GetString("comboBoxControlCode1.Items8"),
            resources.GetString("comboBoxControlCode1.Items9"),
            resources.GetString("comboBoxControlCode1.Items10"),
            resources.GetString("comboBoxControlCode1.Items11"),
            resources.GetString("comboBoxControlCode1.Items12"),
            resources.GetString("comboBoxControlCode1.Items13"),
            resources.GetString("comboBoxControlCode1.Items14")});
            resources.ApplyResources(this.comboBoxControlCode1, "comboBoxControlCode1");
            this.comboBoxControlCode1.Name = "comboBoxControlCode1";
            // 
            // comboBoxMagnVariationMethod
            // 
            this.comboBoxMagnVariationMethod.FormattingEnabled = true;
            this.comboBoxMagnVariationMethod.Items.AddRange(new object[] {
            resources.GetString("comboBoxMagnVariationMethod.Items"),
            resources.GetString("comboBoxMagnVariationMethod.Items1"),
            resources.GetString("comboBoxMagnVariationMethod.Items2"),
            resources.GetString("comboBoxMagnVariationMethod.Items3"),
            resources.GetString("comboBoxMagnVariationMethod.Items4")});
            resources.ApplyResources(this.comboBoxMagnVariationMethod, "comboBoxMagnVariationMethod");
            this.comboBoxMagnVariationMethod.Name = "comboBoxMagnVariationMethod";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // textBoxWaterTemp
            // 
            resources.ApplyResources(this.textBoxWaterTemp, "textBoxWaterTemp");
            this.textBoxWaterTemp.Name = "textBoxWaterTemp";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // textBoxRatingNumber
            // 
            resources.ApplyResources(this.textBoxRatingNumber, "textBoxRatingNumber");
            this.textBoxRatingNumber.Name = "textBoxRatingNumber";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // textBoxRatedArea
            // 
            resources.ApplyResources(this.textBoxRatedArea, "textBoxRatedArea");
            this.textBoxRatedArea.Name = "textBoxRatedArea";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBoxIndexV
            // 
            resources.ApplyResources(this.textBoxIndexV, "textBoxIndexV");
            this.textBoxIndexV.Name = "textBoxIndexV";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // textBoxRatingDischarge
            // 
            resources.ApplyResources(this.textBoxRatingDischarge, "textBoxRatingDischarge");
            this.textBoxRatingDischarge.Name = "textBoxRatingDischarge";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // textBoxGageHChange
            // 
            resources.ApplyResources(this.textBoxGageHChange, "textBoxGageHChange");
            this.textBoxGageHChange.Name = "textBoxGageHChange";
            // 
            // textBoxOutsideGageH
            // 
            resources.ApplyResources(this.textBoxOutsideGageH, "textBoxOutsideGageH");
            this.textBoxOutsideGageH.Name = "textBoxOutsideGageH";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // textBoxInsideGageH
            // 
            resources.ApplyResources(this.textBoxInsideGageH, "textBoxInsideGageH");
            this.textBoxInsideGageH.Name = "textBoxInsideGageH";
            // 
            // FrmSiteInformation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSiteInformation";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textBoxSiteName;
        private System.Windows.Forms.TextBox textBoxStationNumber;
        private System.Windows.Forms.TextBox textBoxMeasNumber;
        private System.Windows.Forms.TextBox textBoxComments;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxMeasLocation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxBoatMotor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxProcessedBy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxFieldParty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxRatedArea;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxIndexV;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxRatingDischarge;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxGageHChange;
        private System.Windows.Forms.TextBox textBoxOutsideGageH;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxInsideGageH;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxWaterTemp;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxRatingNumber;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox comboBoxMeasurementRating;
        private System.Windows.Forms.ComboBox comboBoxControlCode2;
        private System.Windows.Forms.ComboBox comboBoxControlCode3;
        private System.Windows.Forms.ComboBox comboBoxControlCode1;
        private System.Windows.Forms.ComboBox comboBoxMagnVariationMethod;
        private System.Windows.Forms.ComboBox comboBoxDeploymentType;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}