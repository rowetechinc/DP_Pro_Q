namespace ADCP
{
    partial class SetFinishBank
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetFinishBank));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDistance = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.numericUpDownFinishBankPara = new System.Windows.Forms.NumericUpDown();
            this.comboBoxFinishBankStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFinishBankPara)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBoxDistance
            // 
            resources.ApplyResources(this.textBoxDistance, "textBoxDistance");
            this.textBoxDistance.Name = "textBoxDistance";
            this.textBoxDistance.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            this.textBoxDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            resources.ApplyResources(this.btnCancle, "btnCancle");
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // numericUpDownFinishBankPara
            // 
            this.numericUpDownFinishBankPara.DecimalPlaces = 2;
            this.numericUpDownFinishBankPara.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            resources.ApplyResources(this.numericUpDownFinishBankPara, "numericUpDownFinishBankPara");
            this.numericUpDownFinishBankPara.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFinishBankPara.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownFinishBankPara.Name = "numericUpDownFinishBankPara";
            this.numericUpDownFinishBankPara.Value = new decimal(new int[] {
            35,
            0,
            0,
            131072});
            this.numericUpDownFinishBankPara.ValueChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // comboBoxFinishBankStyle
            // 
            this.comboBoxFinishBankStyle.FormattingEnabled = true;
            this.comboBoxFinishBankStyle.Items.AddRange(new object[] {
            resources.GetString("comboBoxFinishBankStyle.Items"),
            resources.GetString("comboBoxFinishBankStyle.Items1"),
            resources.GetString("comboBoxFinishBankStyle.Items2")});
            resources.ApplyResources(this.comboBoxFinishBankStyle, "comboBoxFinishBankStyle");
            this.comboBoxFinishBankStyle.Name = "comboBoxFinishBankStyle";
            this.comboBoxFinishBankStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxFinishBankStyle_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDownFinishBankPara);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxFinishBankStyle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxDistance);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // SetFinishBank
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetFinishBank";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFinishBankPara)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDistance;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.NumericUpDown numericUpDownFinishBankPara;
        private System.Windows.Forms.ComboBox comboBoxFinishBankStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
    }
}