namespace ADCP
{
    partial class FrmShowSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShowSummary));
            this.listView_Summary = new ADCP.DVL_Windows.NoPaintBackGroundListView();
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StartDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StartTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Duration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RiverWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Area = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalQ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MeanVelocity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MeanCourse = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DistanceMG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CourseMG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TopQ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BottomQ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LeftQ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RightQ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MiddleQ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnReCal = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_Summary
            // 
            resources.ApplyResources(this.listView_Summary, "listView_Summary");
            this.listView_Summary.CheckBoxes = true;
            this.listView_Summary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName,
            this.StartDate,
            this.StartTime,
            this.Duration,
            this.RiverWidth,
            this.Area,
            this.TotalQ,
            this.MeanVelocity,
            this.MeanCourse,
            this.DistanceMG,
            this.CourseMG,
            this.TotalLength,
            this.TopQ,
            this.BottomQ,
            this.LeftQ,
            this.RightQ,
            this.MiddleQ});
            this.listView_Summary.FullRowSelect = true;
            this.listView_Summary.Name = "listView_Summary";
            this.listView_Summary.ShowItemToolTips = true;
            this.listView_Summary.UseCompatibleStateImageBehavior = false;
            this.listView_Summary.View = System.Windows.Forms.View.Details;
            this.listView_Summary.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_Summary_ItemChecked);
            // 
            // FileName
            // 
            resources.ApplyResources(this.FileName, "FileName");
            // 
            // StartDate
            // 
            resources.ApplyResources(this.StartDate, "StartDate");
            // 
            // StartTime
            // 
            resources.ApplyResources(this.StartTime, "StartTime");
            // 
            // Duration
            // 
            resources.ApplyResources(this.Duration, "Duration");
            // 
            // RiverWidth
            // 
            resources.ApplyResources(this.RiverWidth, "RiverWidth");
            // 
            // Area
            // 
            resources.ApplyResources(this.Area, "Area");
            // 
            // TotalQ
            // 
            resources.ApplyResources(this.TotalQ, "TotalQ");
            // 
            // MeanVelocity
            // 
            resources.ApplyResources(this.MeanVelocity, "MeanVelocity");
            // 
            // MeanCourse
            // 
            resources.ApplyResources(this.MeanCourse, "MeanCourse");
            // 
            // DistanceMG
            // 
            resources.ApplyResources(this.DistanceMG, "DistanceMG");
            // 
            // CourseMG
            // 
            resources.ApplyResources(this.CourseMG, "CourseMG");
            // 
            // TotalLength
            // 
            resources.ApplyResources(this.TotalLength, "TotalLength");
            // 
            // TopQ
            // 
            resources.ApplyResources(this.TopQ, "TopQ");
            // 
            // BottomQ
            // 
            resources.ApplyResources(this.BottomQ, "BottomQ");
            // 
            // LeftQ
            // 
            resources.ApplyResources(this.LeftQ, "LeftQ");
            // 
            // RightQ
            // 
            resources.ApplyResources(this.RightQ, "RightQ");
            // 
            // MiddleQ
            // 
            resources.ApplyResources(this.MiddleQ, "MiddleQ");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnExport,
            this.toolStripBtnClearAll,
            this.toolStripBtnDelete,
            this.toolStripBtnPrint,
            this.toolStripBtnReCal});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // toolStripBtnExport
            // 
            this.toolStripBtnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripBtnExport, "toolStripBtnExport");
            this.toolStripBtnExport.Name = "toolStripBtnExport";
            this.toolStripBtnExport.Click += new System.EventHandler(this.toolStripBtnExport_Click);
            // 
            // toolStripBtnClearAll
            // 
            this.toolStripBtnClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripBtnClearAll, "toolStripBtnClearAll");
            this.toolStripBtnClearAll.Name = "toolStripBtnClearAll";
            this.toolStripBtnClearAll.Click += new System.EventHandler(this.toolStripBtnClearAll_Click);
            // 
            // toolStripBtnDelete
            // 
            this.toolStripBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripBtnDelete, "toolStripBtnDelete");
            this.toolStripBtnDelete.Name = "toolStripBtnDelete";
            this.toolStripBtnDelete.Click += new System.EventHandler(this.toolStripBtnDelete_Click);
            // 
            // toolStripBtnPrint
            // 
            this.toolStripBtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripBtnPrint, "toolStripBtnPrint");
            this.toolStripBtnPrint.Name = "toolStripBtnPrint";
            this.toolStripBtnPrint.Click += new System.EventHandler(this.toolStripBtnPrint_Click);
            // 
            // toolStripBtnReCal
            // 
            this.toolStripBtnReCal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripBtnReCal, "toolStripBtnReCal");
            this.toolStripBtnReCal.Name = "toolStripBtnReCal";
            this.toolStripBtnReCal.Click += new System.EventHandler(this.toolStripBtnReCal_Click);
            // 
            // FrmShowSummary
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView_Summary);
            this.Name = "FrmShowSummary";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Resize += new System.EventHandler(this.FrmShowSummary_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader StartDate;
        private System.Windows.Forms.ColumnHeader StartTime;
        private System.Windows.Forms.ColumnHeader Duration;
        private System.Windows.Forms.ColumnHeader TotalQ;
        private System.Windows.Forms.ColumnHeader RiverWidth;
        private System.Windows.Forms.ColumnHeader Area;
        private System.Windows.Forms.ColumnHeader MeanVelocity;
        private System.Windows.Forms.ColumnHeader MeanCourse;
        private System.Windows.Forms.ColumnHeader DistanceMG;
        private System.Windows.Forms.ColumnHeader CourseMG;
        private System.Windows.Forms.ColumnHeader TotalLength;
        private System.Windows.Forms.ColumnHeader TopQ;
        private System.Windows.Forms.ColumnHeader BottomQ;
        private System.Windows.Forms.ColumnHeader LeftQ;
        private System.Windows.Forms.ColumnHeader RightQ;
        private System.Windows.Forms.ColumnHeader MiddleQ;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripBtnExport;
        private System.Windows.Forms.ToolStripButton toolStripBtnClearAll;
        private System.Windows.Forms.ToolStripButton toolStripBtnDelete;
        private DVL_Windows.NoPaintBackGroundListView listView_Summary;
        private System.Windows.Forms.ToolStripButton toolStripBtnPrint;
        private System.Windows.Forms.ToolStripButton toolStripBtnReCal;
    }
}