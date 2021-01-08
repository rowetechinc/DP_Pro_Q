namespace ADCP
{
    partial class FrmSetTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetTime));
            this.dateTimePicker_Date = new System.Windows.Forms.DateTimePicker();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dateTimePicker_Time = new System.Windows.Forms.DateTimePicker();
            this.radioBtnPCTime = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReadTime = new System.Windows.Forms.Button();
            this.radioBtnGMT = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker_Date
            // 
            this.dateTimePicker_Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            resources.ApplyResources(this.dateTimePicker_Date, "dateTimePicker_Date");
            this.dateTimePicker_Date.Name = "dateTimePicker_Date";
            // 
            // btnSetTime
            // 
            resources.ApplyResources(this.btnSetTime, "btnSetTime");
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dateTimePicker_Time
            // 
            this.dateTimePicker_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            resources.ApplyResources(this.dateTimePicker_Time, "dateTimePicker_Time");
            this.dateTimePicker_Time.Name = "dateTimePicker_Time";
            this.dateTimePicker_Time.ShowUpDown = true;
            // 
            // radioBtnPCTime
            // 
            resources.ApplyResources(this.radioBtnPCTime, "radioBtnPCTime");
            this.radioBtnPCTime.Checked = true;
            this.radioBtnPCTime.Name = "radioBtnPCTime";
            this.radioBtnPCTime.TabStop = true;
            this.radioBtnPCTime.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker_Date);
            this.groupBox1.Controls.Add(this.dateTimePicker_Time);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnReadTime
            // 
            resources.ApplyResources(this.btnReadTime, "btnReadTime");
            this.btnReadTime.Name = "btnReadTime";
            this.btnReadTime.UseVisualStyleBackColor = true;
            this.btnReadTime.Click += new System.EventHandler(this.btnReadTime_Click);
            // 
            // radioBtnGMT
            // 
            resources.ApplyResources(this.radioBtnGMT, "radioBtnGMT");
            this.radioBtnGMT.Name = "radioBtnGMT";
            this.radioBtnGMT.TabStop = true;
            this.radioBtnGMT.UseVisualStyleBackColor = true;
            // 
            // FrmSetTime
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioBtnGMT);
            this.Controls.Add(this.btnReadTime);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.radioBtnPCTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSetTime);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetTime";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.FrmSetTime_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker_Date;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Time;
        private System.Windows.Forms.RadioButton radioBtnPCTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReadTime;
        private System.Windows.Forms.RadioButton radioBtnGMT;
    }
}