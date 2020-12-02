using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ADCP
{
    public partial class SetFinishBank : Form
    {
        public SetFinishBank(EdgeSetting endEdge)
        {
            InitializeComponent();
            GetDefaultConf(endEdge);
        }
        public bool bSetFinishBank; //判断是否采用新输入的岸边参数
        public String strFinishDistance;
        public int iFinishStyle;
        public static decimal dFinishPara;

        private void btnOK_Click(object sender, EventArgs e)
        {
            iFinishStyle = comboBoxFinishBankStyle.SelectedIndex;
            dFinishPara = numericUpDownFinishBankPara.Value;

            strFinishDistance = textBoxDistance.Text;
            bSetFinishBank = true;

            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxFinishBankStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxFinishBankStyle.SelectedIndex)
            {
                case 0:
                    {
                        numericUpDownFinishBankPara.Value = 0.35M;
                        numericUpDownFinishBankPara.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        numericUpDownFinishBankPara.Value = 0.91M;
                        numericUpDownFinishBankPara.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        numericUpDownFinishBankPara.Value = 0.5M;
                        numericUpDownFinishBankPara.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }

        ClassValidateInPut validateInput = new ClassValidateInPut();
        private void TxtBox_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;

            if (!validateInput.ValidateUserInput(textBoxDistance.Text, 12)) 
            {
                btnOK.Enabled = false; 
            }
            if (!validateInput.ValidateUserInput(numericUpDownFinishBankPara.Text, 12)) 
            {
                btnOK.Enabled = false; 
            }
        }

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (validateInput.ValidateCharInput(e.KeyChar,((TextBox)sender).Text))
            {
                e.Handled = true;
            }
        }

        private void GetDefaultConf(EdgeSetting endEdge)
        {
            if (!endEdge.bStartLeft)
            {
                textBoxDistance.Text = endEdge.dLeftDis.ToString();
                numericUpDownFinishBankPara.Value = (decimal)endEdge.dLeftRef;
                comboBoxFinishBankStyle.SelectedIndex = endEdge.iLeftType;
            }
            else
            {
                textBoxDistance.Text = endEdge.dRightDis.ToString();
                numericUpDownFinishBankPara.Value = (decimal)endEdge.dRightRef;
                comboBoxFinishBankStyle.SelectedIndex = endEdge.iRightType;
            }

            if (endEdge.bEnglishUnit)
                label3.Text = "(ft)";
            else
                label3.Text = "(m)";
        }

    }
}
