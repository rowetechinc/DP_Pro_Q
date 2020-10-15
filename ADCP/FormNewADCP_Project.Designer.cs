namespace ADCP
{
    partial class FormNewADCP_Project
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewADCP_Project));
            this.label13 = new System.Windows.Forms.Label();
            this.FileName = new System.Windows.Forms.TextBox();
            this.buttonProjectName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ImageKey = global::ADCP.Resource1.String86;
            this.label13.Name = "label13";
            // 
            // FileName
            // 
            resources.ApplyResources(this.FileName, "FileName");
            this.FileName.Name = "FileName";
            // 
            // buttonProjectName
            // 
            resources.ApplyResources(this.buttonProjectName, "buttonProjectName");
            this.buttonProjectName.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonProjectName.ImageKey = global::ADCP.Resource1.String86;
            this.buttonProjectName.Name = "buttonProjectName";
            this.buttonProjectName.UseVisualStyleBackColor = true;
            this.buttonProjectName.Click += new System.EventHandler(this.buttonProjectName_Click);
            // 
            // FormNewADCP_Project
            // 
            this.AcceptButton = this.buttonProjectName;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonProjectName);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.FileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNewADCP_Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button buttonProjectName;
    }
}