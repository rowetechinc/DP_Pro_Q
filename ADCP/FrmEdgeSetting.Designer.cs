namespace ADCP
{
    partial class FrmEdgeSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEdgeSetting));
            this.radioBtnLeft = new System.Windows.Forms.RadioButton();
            this.radioBtnRight = new System.Windows.Forms.RadioButton();
            this.numericUpDownLeftRef = new System.Windows.Forms.NumericUpDown();
            this.comboBoxLeftType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLeftDis = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxRightDis = new System.Windows.Forms.TextBox();
            this.comboBoxRightType = new System.Windows.Forms.ComboBox();
            this.numericUpDownRightRef = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxTopEstimate = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPowerCurveCoeff = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxBottomEstimate = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftRef)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRightRef)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPowerCurveCoeff)).BeginInit();
            this.SuspendLayout();
            // 
            // radioBtnLeft
            // 
            resources.ApplyResources(this.radioBtnLeft, "radioBtnLeft");
            this.radioBtnLeft.Checked = true;
            this.radioBtnLeft.Name = "radioBtnLeft";
            this.radioBtnLeft.TabStop = true;
            this.radioBtnLeft.UseVisualStyleBackColor = true;
            // 
            // radioBtnRight
            // 
            resources.ApplyResources(this.radioBtnRight, "radioBtnRight");
            this.radioBtnRight.Name = "radioBtnRight";
            this.radioBtnRight.UseVisualStyleBackColor = true;
            // 
            // numericUpDownLeftRef
            // 
            this.numericUpDownLeftRef.DecimalPlaces = 2;
            this.numericUpDownLeftRef.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            resources.ApplyResources(this.numericUpDownLeftRef, "numericUpDownLeftRef");
            this.numericUpDownLeftRef.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLeftRef.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownLeftRef.Name = "numericUpDownLeftRef";
            this.numericUpDownLeftRef.Value = new decimal(new int[] {
            35,
            0,
            0,
            131072});
            this.numericUpDownLeftRef.ValueChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // comboBoxLeftType
            // 
            this.comboBoxLeftType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLeftType.FormattingEnabled = true;
            this.comboBoxLeftType.Items.AddRange(new object[] {
            resources.GetString("comboBoxLeftType.Items"),
            resources.GetString("comboBoxLeftType.Items1"),
            resources.GetString("comboBoxLeftType.Items2")});
            resources.ApplyResources(this.comboBoxLeftType, "comboBoxLeftType");
            this.comboBoxLeftType.Name = "comboBoxLeftType";
            this.comboBoxLeftType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLeftType_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxLeftDis
            // 
            resources.ApplyResources(this.textBoxLeftDis, "textBoxLeftDis");
            this.textBoxLeftDis.Name = "textBoxLeftDis";
            this.textBoxLeftDis.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxLeftDis.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBtnLeft);
            this.groupBox1.Controls.Add(this.radioBtnRight);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxLeftDis);
            this.groupBox2.Controls.Add(this.comboBoxLeftType);
            this.groupBox2.Controls.Add(this.numericUpDownLeftRef);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBoxRightDis);
            this.groupBox3.Controls.Add(this.comboBoxRightType);
            this.groupBox3.Controls.Add(this.numericUpDownRightRef);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBoxRightDis
            // 
            resources.ApplyResources(this.textBoxRightDis, "textBoxRightDis");
            this.textBoxRightDis.Name = "textBoxRightDis";
            this.textBoxRightDis.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxRightDis.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // comboBoxRightType
            // 
            this.comboBoxRightType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRightType.FormattingEnabled = true;
            this.comboBoxRightType.Items.AddRange(new object[] {
            resources.GetString("comboBoxRightType.Items"),
            resources.GetString("comboBoxRightType.Items1"),
            resources.GetString("comboBoxRightType.Items2")});
            resources.ApplyResources(this.comboBoxRightType, "comboBoxRightType");
            this.comboBoxRightType.Name = "comboBoxRightType";
            this.comboBoxRightType.SelectedIndexChanged += new System.EventHandler(this.comboBoxRightType_SelectedIndexChanged);
            // 
            // numericUpDownRightRef
            // 
            this.numericUpDownRightRef.DecimalPlaces = 2;
            this.numericUpDownRightRef.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            resources.ApplyResources(this.numericUpDownRightRef, "numericUpDownRightRef");
            this.numericUpDownRightRef.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRightRef.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownRightRef.Name = "numericUpDownRightRef";
            this.numericUpDownRightRef.Value = new decimal(new int[] {
            35,
            0,
            0,
            131072});
            this.numericUpDownRightRef.ValueChanged += new System.EventHandler(this.textBox_TextChanged);
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // comboBoxTopEstimate
            // 
            this.comboBoxTopEstimate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTopEstimate.FormattingEnabled = true;
            this.comboBoxTopEstimate.Items.AddRange(new object[] {
            resources.GetString("comboBoxTopEstimate.Items"),
            resources.GetString("comboBoxTopEstimate.Items1"),
            resources.GetString("comboBoxTopEstimate.Items2")});
            resources.ApplyResources(this.comboBoxTopEstimate, "comboBoxTopEstimate");
            this.comboBoxTopEstimate.Name = "comboBoxTopEstimate";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numericUpDownPowerCurveCoeff);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.comboBoxBottomEstimate);
            this.groupBox4.Controls.Add(this.comboBoxTopEstimate);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // numericUpDownPowerCurveCoeff
            // 
            this.numericUpDownPowerCurveCoeff.DecimalPlaces = 4;
            this.numericUpDownPowerCurveCoeff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            resources.ApplyResources(this.numericUpDownPowerCurveCoeff, "numericUpDownPowerCurveCoeff");
            this.numericUpDownPowerCurveCoeff.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPowerCurveCoeff.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDownPowerCurveCoeff.Name = "numericUpDownPowerCurveCoeff";
            this.numericUpDownPowerCurveCoeff.Value = new decimal(new int[] {
            1667,
            0,
            0,
            262144});
            this.numericUpDownPowerCurveCoeff.ValueChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // comboBoxBottomEstimate
            // 
            this.comboBoxBottomEstimate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBottomEstimate.FormattingEnabled = true;
            this.comboBoxBottomEstimate.Items.AddRange(new object[] {
            resources.GetString("comboBoxBottomEstimate.Items"),
            resources.GetString("comboBoxBottomEstimate.Items1")});
            resources.ApplyResources(this.comboBoxBottomEstimate, "comboBoxBottomEstimate");
            this.comboBoxBottomEstimate.Name = "comboBoxBottomEstimate";
            // 
            // FrmEdgeSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEdgeSetting";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftRef)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRightRef)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPowerCurveCoeff)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioBtnLeft;
        private System.Windows.Forms.RadioButton radioBtnRight;
        private System.Windows.Forms.NumericUpDown numericUpDownLeftRef;
        private System.Windows.Forms.ComboBox comboBoxLeftType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLeftDis;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxRightDis;
        private System.Windows.Forms.ComboBox comboBoxRightType;
        private System.Windows.Forms.NumericUpDown numericUpDownRightRef;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxTopEstimate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown numericUpDownPowerCurveCoeff;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxBottomEstimate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}