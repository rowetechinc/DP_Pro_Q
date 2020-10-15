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
    public partial class FrmSystemTest : Form
    {
        public FrmSystemTest()
        {
            InitializeComponent();
            //label1.Text = "Press start to begin system test";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
