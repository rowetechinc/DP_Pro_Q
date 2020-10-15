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
            this.comboBoxStandardMode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_RS232 = new System.Windows.Forms.ComboBox();
            this.comboBox_RS485 = new System.Windows.Forms.ComboBox();
            this.label_RS232 = new System.Windows.Forms.Label();
            this.label_RS485 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelCheckAdvanced = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxSalinity = new System.Windows.Forms.TextBox();
            this.textBoxHeadingOffset = new System.Windows.Forms.TextBox();
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
            this.comboBoxVesselSpeedRef.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.comboBoxHeadingRef.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHeadingRef.FormattingEnabled = true;
            this.comboBoxHeadingRef.Items.AddRange(new object[] {
            resources.GetString("comboBoxHeadingRef.Items"),
            resources.GetString("comboBoxHeadingRef.Items1")});
            resources.ApplyResources(this.comboBoxHeadingRef, "comboBoxHeadingRef");
            this.comboBoxHeadingRef.Name = "comboBoxHeadingRef";
            this.comboBoxHeadingRef.SelectedIndexChanged += new System.EventHandler(this.comboBoxHeadingRef_SelectedIndexChanged);
            // 
            // comboBoxMeasMode
            // 
            this.comboBoxMeasMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMeasMode.FormattingEnabled = true;
            this.comboBoxMeasMode.Items.AddRange(new object[] {
            resources.GetString("comboBoxMeasMode.Items"),
            resources.GetString("comboBoxMeasMode.Items1")});
            resources.ApplyResources(this.comboBoxMeasMode, "comboBoxMeasMode");
            this.comboBoxMeasMode.Name = "comboBoxMeasMode";
            this.comboBoxMeasMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMeasMode_SelectedIndexChanged);
            // 
            // textBoxTransducerDepth
            // 
            resources.ApplyResources(this.textBoxTransducerDepth, "textBoxTransducerDepth");
            this.textBoxTransducerDepth.Name = "textBoxTransducerDepth";
            this.textBoxTransducerDepth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxTransducerDepth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
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
            // comboBoxStandardMode
            // 
            this.comboBoxStandardMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStandardMode.FormattingEnabled = true;
            this.comboBoxStandardMode.Items.AddRange(new object[] {
            resources.GetString("comboBoxStandardMode.Items"),
            resources.GetString("comboBoxStandardMode.Items1"),
            resources.GetString("comboBoxStandardMode.Items2"),
            resources.GetString("comboBoxStandardMode.Items3")});
            resources.ApplyResources(this.comboBoxStandardMode, "comboBoxStandardMode");
            this.comboBoxStandardMode.Name = "comboBoxStandardMode";
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
            this.comboBox_RS232.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBox_RS232, "comboBox_RS232");
            this.comboBox_RS232.FormattingEnabled = true;
            this.comboBox_RS232.Items.AddRange(new object[] {
            resources.GetString("comboBox_RS232.Items"),
            resources.GetString("comboBox_RS232.Items1"),
            resources.GetString("comboBox_RS232.Items2"),
            resources.GetString("comboBox_RS232.Items3"),
            resources.GetString("comboBox_RS232.Items4"),
            resources.GetString("comboBox_RS232.Items5")});
            this.comboBox_RS232.Name = "comboBox_RS232";
            // 
            // comboBox_RS485
            // 
            this.comboBox_RS485.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBox_RS485, "comboBox_RS485");
            this.comboBox_RS485.FormattingEnabled = true;
            this.comboBox_RS485.Items.AddRange(new object[] {
            resources.GetString("comboBox_RS485.Items"),
            resources.GetString("comboBox_RS485.Items1"),
            resources.GetString("comboBox_RS485.Items2"),
            resources.GetString("comboBox_RS485.Items3"),
            resources.GetString("comboBox_RS485.Items4"),
            resources.GetString("comboBox_RS485.Items5")});
            this.comboBox_RS485.Name = "comboBox_RS485";
            // 
            // label_RS232
            // 
            resources.ApplyResources(this.label_RS232, "label_RS232");
            this.label_RS232.Name = "label_RS232";
            // 
            // label_RS485
            // 
            resources.ApplyResources(this.label_RS485, "label_RS485");
            this.label_RS485.Name = "label_RS485";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // linkLabelCheckAdvanced
            // 
            resources.ApplyResources(this.linkLabelCheckAdvanced, "linkLabelCheckAdvanced");
            this.linkLabelCheckAdvanced.Name = "linkLabelCheckAdvanced";
            this.linkLabelCheckAdvanced.TabStop = true;
            this.linkLabelCheckAdvanced.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCheckAdvanced_LinkClicked);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // textBoxSalinity
            // 
            resources.ApplyResources(this.textBoxSalinity, "textBoxSalinity");
            this.textBoxSalinity.Name = "textBoxSalinity";
            this.textBoxSalinity.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxSalinity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // textBoxHeadingOffset
            // 
            resources.ApplyResources(this.textBoxHeadingOffset, "textBoxHeadingOffset");
            this.textBoxHeadingOffset.Name = "textBoxHeadingOffset";
            this.textBoxHeadingOffset.TextChanged += new System.EventHandler(this.textBoxHeadingOffset_TextChanged);
            this.textBoxHeadingOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxHeadingOffset_KeyPress);
            // 
            // FrmSystemSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxHeadingOffset);
            this.Controls.Add(this.textBoxSalinity);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.linkLabelCheckAdvanced);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_RS232);
            this.Controls.Add(this.comboBox_RS485);
            this.Controls.Add(this.label_RS232);
            this.Controls.Add(this.label_RS485);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxStandardMode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBoxTransducerDepth);
            this.Controls.Add(this.comboBoxMeasMode);
            this.Controls.Add(this.comboBoxHeadingRef);
            this.Controls.Add(this.comboBoxVesselSpeedRef);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSystemSetting";
            this.ShowIcon = false;
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
        private System.Windows.Forms.ComboBox comboBoxStandardMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_RS232;
        private System.Windows.Forms.ComboBox comboBox_RS485;
        private System.Windows.Forms.Label label_RS232;
        private System.Windows.Forms.Label label_RS485;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelCheckAdvanced;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxSalinity;
        private System.Windows.Forms.TextBox textBoxHeadingOffset;
    }
}