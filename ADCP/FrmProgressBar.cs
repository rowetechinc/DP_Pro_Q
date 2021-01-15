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
    public partial class FrmProgressBar : Form
    {
        public FrmProgressBar(int Minimum,int Maximum)
        {
            InitializeComponent();
            prcBar.Maximum = Maximum;
        }
        /// < summary>  
        /// Increase process bar  
        /// < /summary>  
        /// < param name="nValue">the value increased< /param>  
        /// < returns>< /returns>  
        public void setPos( int nValue )  
        {
            if (nValue < prcBar.Maximum)
            {
                prcBar.Value = nValue;
            }
            else
            {
                this.Close();
            }
            //Application.DoEvents();
        }

        private void FrmProgressBar_Load(object sender, EventArgs e)
        {
         //   this.Owner.Enabled = false;
        } 

    }
}
