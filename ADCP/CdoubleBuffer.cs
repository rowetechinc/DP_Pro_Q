using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADCP
{
    /// <summary>
    /// 双缓冲panel
    /// </summary>
    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                 ControlStyles.OptimizedDoubleBuffer | //双缓冲
                 ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
                 true);
        }
    }

    public class NoPaintBackGroundListView : ListView
    {
        public NoPaintBackGroundListView()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }
        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
    }

    // <summary>
    /// 双缓冲DataGridView,解决闪烁
    /// 使用方法：在DataGridView所在窗体的InitializeComponent方法中更改控件类型实例化语句将
    /// this.dataGridView1 = new System.Windows.Forms.DataGridView();   屏蔽掉，添加下面这句即可
    /// this.dataGridView1 = new DoubleBufferListView();
    /// </summary>
    public class DoubleBufferDataGridView : DataGridView
    {
        public DoubleBufferDataGridView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            //UpdateStatus.Continue;
            UpdateStyles();
        }
    }
}
