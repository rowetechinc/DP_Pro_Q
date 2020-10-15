namespace ADCP
{
    partial class SetStartBank
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetStartBank));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownStartBankPara = new System.Windows.Forms.NumericUpDown();
            this.comboBoxStartBankStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioBtnRightBank = new System.Windows.Forms.RadioButton();
            this.radioBtnLeftBank = new System.Windows.Forms.RadioButton();
            this.textBoxDistance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartBankPara)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDownStartBankPara);
            this.groupBox1.Controls.Add(this.comboBoxStartBankStyle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.radioBtnRightBank);
            this.groupBox1.Controls.Add(this.radioBtnLeftBank);
            this.groupBox1.Controls.Add(this.textBoxDistance);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            //this.label3.ImageKey = global::ADCP.Resource1_en.String86;
            this.label3.Name = "label3";
            // 
            // numericUpDownStartBankPara
            // 
            resources.ApplyResources(this.numericUpDownStartBankPara, "numericUpDownStartBankPara");
            this.numericUpDownStartBankPara.DecimalPlaces = 2;
            this.numericUpDownStartBankPara.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownStartBankPara.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStartBankPara.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownStartBankPara.Name = "numericUpDownStartBankPara";
            this.numericUpDownStartBankPara.Value = new decimal(new int[] {
            35,
            0,
            0,
            131072});
            this.numericUpDownStartBankPara.ValueChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // comboBoxStartBankStyle
            // 
            resources.ApplyResources(this.comboBoxStartBankStyle, "comboBoxStartBankStyle");
            this.comboBoxStartBankStyle.FormattingEnabled = true;
            this.comboBoxStartBankStyle.Items.AddRange(new object[] {
            resources.GetString("comboBoxStartBankStyle.Items"),
            resources.GetString("comboBoxStartBankStyle.Items1"),
            resources.GetString("comboBoxStartBankStyle.Items2")});
            this.comboBoxStartBankStyle.Name = "comboBoxStartBankStyle";
            this.comboBoxStartBankStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxStartBankStyle_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            //this.label2.ImageKey = global::ADCP.Resource1_en.String86;
            this.label2.Name = "label2";
            // 
            // radioBtnRightBank
            // 
            resources.ApplyResources(this.radioBtnRightBank, "radioBtnRightBank");
            //this.radioBtnRightBank.ImageKey = global::ADCP.Resource1_en.String86;
            this.radioBtnRightBank.Name = "radioBtnRightBank";
            this.radioBtnRightBank.UseVisualStyleBackColor = true;
            // 
            // radioBtnLeftBank
            // 
            resources.ApplyResources(this.radioBtnLeftBank, "radioBtnLeftBank");
            this.radioBtnLeftBank.Checked = true;
            //this.radioBtnLeftBank.ImageKey = global::ADCP.Resource1_en.String86;
            this.radioBtnLeftBank.Name = "radioBtnLeftBank";
            this.radioBtnLeftBank.TabStop = true;
            this.radioBtnLeftBank.UseVisualStyleBackColor = true;
            // 
            // textBoxDistance
            // 
            resources.ApplyResources(this.textBoxDistance, "textBoxDistance");
            this.textBoxDistance.Name = "textBoxDistance";
            this.textBoxDistance.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            this.textBoxDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            //this.label1.ImageKey = global::ADCP.Resource1_en.String86;
            this.label1.Name = "label1";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            //this.btnOK.ImageKey = global::ADCP.Resource1_en.String86;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            resources.ApplyResources(this.btnCancle, "btnCancle");
            //this.btnCancle.ImageKey = global::ADCP.Resource1_en.String86;
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // SetStartBank
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetStartBank";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartBankPara)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBtnRightBank;
        private System.Windows.Forms.RadioButton radioBtnLeftBank;
        private System.Windows.Forms.TextBox textBoxDistance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStartBankStyle;
        private System.Windows.Forms.NumericUpDown numericUpDownStartBankPara;
        private System.Windows.Forms.Label label3;
    }
}