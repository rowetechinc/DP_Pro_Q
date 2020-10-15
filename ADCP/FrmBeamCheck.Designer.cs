namespace ADCP
{
    partial class FrmBeamCheck
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
            this.panelBeamCheck = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelBeamCheck
            // 
            this.panelBeamCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBeamCheck.BackColor = System.Drawing.Color.White;
            this.panelBeamCheck.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBeamCheck.Location = new System.Drawing.Point(4, 3);
            this.panelBeamCheck.Name = "panelBeamCheck";
            this.panelBeamCheck.Size = new System.Drawing.Size(716, 555);
            this.panelBeamCheck.TabIndex = 0;
            this.panelBeamCheck.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBeamCheck_Paint);
            // 
            // FrmBeamCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 561);
            this.Controls.Add(this.panelBeamCheck);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBeamCheck";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beam Check";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBeamCheck;
    }
}