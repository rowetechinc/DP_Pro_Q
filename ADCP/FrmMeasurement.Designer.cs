namespace ADCP
{
    partial class FrmMeasurement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMeasurement));
            this.btnEdg1 = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnEdg2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEdg1
            // 
            resources.ApplyResources(this.btnEdg1, "btnEdg1");
            this.btnEdg1.Name = "btnEdg1";
            this.btnEdg1.UseVisualStyleBackColor = true;
            this.btnEdg1.Click += new System.EventHandler(this.btnEdg1_Click);
            // 
            // btnMove
            // 
            resources.ApplyResources(this.btnMove, "btnMove");
            this.btnMove.Name = "btnMove";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnEdg2
            // 
            resources.ApplyResources(this.btnEdg2, "btnEdg2");
            this.btnEdg2.Name = "btnEdg2";
            this.btnEdg2.UseVisualStyleBackColor = true;
            this.btnEdg2.Click += new System.EventHandler(this.btnEdg2_Click);
            // 
            // FrmMeasurement
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnEdg2);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnEdg1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMeasurement";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEdg1;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnEdg2;
    }
}