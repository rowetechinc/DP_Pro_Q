namespace ADCP
{
    partial class FrmGetHeadingOffset
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetHeadingOffset));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.comboBox_RS232 = new System.Windows.Forms.ComboBox();
            this.label_RS232 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.textBoxHeadingOffset = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelEnsNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            // 
            // comboBox_RS232
            // 
            this.comboBox_RS232.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            // 
            // label_RS232
            // 
            resources.ApplyResources(this.label_RS232, "label_RS232");
            this.label_RS232.Name = "label_RS232";
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // textBoxHeadingOffset
            // 
            resources.ApplyResources(this.textBoxHeadingOffset, "textBoxHeadingOffset");
            this.textBoxHeadingOffset.Name = "textBoxHeadingOffset";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnStop
            // 
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.Name = "btnStop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelEnsNumber
            // 
            resources.ApplyResources(this.labelEnsNumber, "labelEnsNumber");
            this.labelEnsNumber.Name = "labelEnsNumber";
            // 
            // FrmGetHeadingOffset
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelEnsNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textBoxHeadingOffset);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.comboBox_RS232);
            this.Controls.Add(this.label_RS232);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetHeadingOffset";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ComboBox comboBox_RS232;
        private System.Windows.Forms.Label label_RS232;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textBoxHeadingOffset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelEnsNumber;
    }
}