using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace ADCP
{
    public partial class LoadPlayBackFile_Windows : Form
    {
        public LoadPlayBackFile_Windows()
        {
            InitializeComponent();
        }
        /// LPJ 2012-4-24 
        System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(LoadPlayBackFile_Windows));

        //////LPJ 2012-4-24 
        /// 应用资源
        /// ApplyResources 的第一个参数为要设置的控件
        ///                  第二个参数为在资源文件中的ID，默认为控件的名称
        public void ApplyResource()
        {
            GetControl(this.Controls);

            //Caption
            res.ApplyResources(this, "$this");
        }

        ////LPJ 2012-4-24 遍历所有控件
        public void GetControl(Control.ControlCollection ctl)
        {
            try
            {
                foreach (Control ctl1 in ctl)
                {
                    res.ApplyResources(ctl1, ctl1.Name);
                    if (ctl1.HasChildren)
                    {
                        GetControl(ctl1.Controls);
                    }
                }
            }
            catch { }
        }

        ////LPJ 2012-4-24 判断语言类型
        public void ChooseLanguage()
        {
            if (Form1.Language_ZH == false)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                ApplyResource();
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh");
                ApplyResource();
            }
        }
    }  
}
