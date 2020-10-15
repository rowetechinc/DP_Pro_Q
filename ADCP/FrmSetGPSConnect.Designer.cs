namespace ADCP
{
    partial class FrmSetGPSConnect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetGPSConnect));
            this.btnGPSconnect = new System.Windows.Forms.Button();
            this.comboBoxGPS_Baudrate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboxGPS_Port = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGPSconnect
            // 
            this.btnGPSconnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnGPSconnect, "btnGPSconnect");
            this.btnGPSconnect.Name = "btnGPSconnect";
            this.btnGPSconnect.UseVisualStyleBackColor = true;
            this.btnGPSconnect.Click += new System.EventHandler(this.btnGPSconnect_Click);
            // 
            // comboBoxGPS_Baudrate
            // 
            this.comboBoxGPS_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGPS_Baudrate.FormattingEnabled = true;
            this.comboBoxGPS_Baudrate.Items.AddRange(new object[] {
            resources.GetString("comboBoxGPS_Baudrate.Items"),
            resources.GetString("comboBoxGPS_Baudrate.Items1"),
            resources.GetString("comboBoxGPS_Baudrate.Items2"),
            resources.GetString("comboBoxGPS_Baudrate.Items3"),
            resources.GetString("comboBoxGPS_Baudrate.Items4"),
            resources.GetString("comboBoxGPS_Baudrate.Items5"),
            resources.GetString("comboBoxGPS_Baudrate.Items6")});
            resources.ApplyResources(this.comboBoxGPS_Baudrate, "comboBoxGPS_Baudrate");
            this.comboBoxGPS_Baudrate.Name = "comboBoxGPS_Baudrate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ImageKey = global::ADCP.Resource1.str_aboutText4;
            this.label5.Name = "label5";
            // 
            // comboxGPS_Port
            // 
            this.comboxGPS_Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxGPS_Port.FormattingEnabled = true;
            resources.ApplyResources(this.comboxGPS_Port, "comboxGPS_Port");
            this.comboxGPS_Port.Name = "comboxGPS_Port";
            this.comboxGPS_Port.SelectedIndexChanged += new System.EventHandler(this.comboxGPS_Port_SelectedIndexChanged);
            this.comboxGPS_Port.Click += new System.EventHandler(this.comboxGPS_Port_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ImageKey = global::ADCP.Resource1.str_aboutText4;
            this.label3.Name = "label3";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmSetGPSConnect
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGPSconnect);
            this.Controls.Add(this.comboBoxGPS_Baudrate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboxGPS_Port);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetGPSConnect";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGPSconnect;
        private System.Windows.Forms.ComboBox comboBoxGPS_Baudrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboxGPS_Port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
    }
}