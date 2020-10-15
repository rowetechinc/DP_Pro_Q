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
    public partial class FrmUnit : Form
    {
        public FrmUnit(int iunit)
        {
            InitializeComponent();
            comboBoxUnit.SelectedIndex = iunit;
        }

        public int iUnit;
        private void btnOK_Click(object sender, EventArgs e)
        {
            iUnit = comboBoxUnit.SelectedIndex;
            this.Close();
        }
    }
}
