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
    public partial class FrmMeasurement : Form
    {
        public FrmMeasurement(int flag)
        {
            InitializeComponent();

            btnEdg1.Enabled = false;
            btnEdg2.Enabled = false;
            btnMove.Enabled = false;

            InitButtons(flag);
        }

        private void InitButtons(int flag)
        {
            switch (flag)
            {
                case 1:
                    {
                        btnEdg1.Enabled = true;
                        break;
                    }
                case 2:
                    {
                        btnMove.Enabled = true;
                        break;
                    }
                case 3:
                    {
                        btnEdg2.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }

        //当点击开始测量时，弹出该Form，且起始岸按钮可用
        private void btnEdg1_Click(object sender, EventArgs e)
        {
            //当点击起始岸时，弹出起始岸设置参数，且move按钮可用
            bStartEdge = true;

            //btnEdg1.Enabled = false;
            //btnMove.Enabled = true;
            this.Close();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            //当点击走航时，开始走航测量，且终止岸按钮可以
            bMoving = true;

            //btnMove.Enabled = false;
            //btnEdg2.Enabled = true;
            this.Close();
        }

        private void btnEdg2_Click(object sender, EventArgs e)
        {
            //当点击终止岸时，弹出终止岸设置参数，且该form关闭
            bEndEdge = true;

            this.Close(); //
        }

        public bool bStartEdge = false;
        public bool bMoving = false;
        public bool bEndEdge = false;
    }
}
