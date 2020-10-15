using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADCP
{
    public partial class FrmSystemConf : Form
    {
        public FrmSystemConf(SystemConf sysConf)
        {
            InitializeComponent();
            GetReference(sysConf);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           

            this.Close();
        }
        private void GetReference(SystemConf sysConf)
        {
           
        }

        public SystemConf systemConf;
        public struct SystemConf
        {
            public string FirmwareVersion;
            public string SystemNumber;
        }
    }
}
