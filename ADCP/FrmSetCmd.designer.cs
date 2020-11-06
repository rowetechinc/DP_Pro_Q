namespace ADCP
{
    partial class FrmSetCmd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetCmd));
            this.label2 = new System.Windows.Forms.Label();
            this.comboxADCP_Port = new System.Windows.Forms.ComboBox();
            this.btnADCPconnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxADCP_Baudrate = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboxADCP_Port
            // 
            resources.ApplyResources(this.comboxADCP_Port, "comboxADCP_Port");
            this.comboxADCP_Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxADCP_Port.FormattingEnabled = true;
            this.comboxADCP_Port.Name = "comboxADCP_Port";
            this.comboxADCP_Port.Click += new System.EventHandler(this.comboxADCP_Port_Click);
            // 
            // btnADCPconnect
            // 
            resources.ApplyResources(this.btnADCPconnect, "btnADCPconnect");
            this.btnADCPconnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnADCPconnect.Name = "btnADCPconnect";
            this.btnADCPconnect.UseVisualStyleBackColor = true;
            this.btnADCPconnect.Click += new System.EventHandler(this.btnADCPconnect_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // comboBoxADCP_Baudrate
            // 
            resources.ApplyResources(this.comboBoxADCP_Baudrate, "comboBoxADCP_Baudrate");
            this.comboBoxADCP_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxADCP_Baudrate.FormattingEnabled = true;
            this.comboBoxADCP_Baudrate.Items.AddRange(new object[] {
            resources.GetString("comboBoxADCP_Baudrate.Items"),
            resources.GetString("comboBoxADCP_Baudrate.Items1"),
            resources.GetString("comboBoxADCP_Baudrate.Items2"),
            resources.GetString("comboBoxADCP_Baudrate.Items3"),
            resources.GetString("comboBoxADCP_Baudrate.Items4"),
            resources.GetString("comboBoxADCP_Baudrate.Items5"),
            resources.GetString("comboBoxADCP_Baudrate.Items6"),
            resources.GetString("comboBoxADCP_Baudrate.Items7"),
            resources.GetString("comboBoxADCP_Baudrate.Items8")});
            this.comboBoxADCP_Baudrate.Name = "comboBoxADCP_Baudrate";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmSetCmd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.comboBoxADCP_Baudrate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnADCPconnect);
            this.Controls.Add(this.comboxADCP_Port);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetCmd";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboxADCP_Port;
        private System.Windows.Forms.Button btnADCPconnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxADCP_Baudrate;
        private System.Windows.Forms.Button btnCancel;

    }
}