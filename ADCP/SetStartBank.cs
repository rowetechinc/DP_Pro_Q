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
    public partial class SetStartBank : Form
    {
        public SetStartBank(FrmEdgeSetting.EdgeSetting startEdge)
        {
            InitializeComponent();
            GetDefaultConf(startEdge);
        }

        public bool bSetStartBank; //判断是否采用新输入的岸边参数
        public bool bStartLeftBank;
        public String strStartDistance;
        public int iStartStyle;  //起始岸类型
        public static decimal dStartPara;   //起始岸参数


        private void btnOK_Click(object sender, EventArgs e)
        {
            //LPJ 2013-2-21 当点击“确定”按钮，则岸边流量计算参数采用该对话框中输入的起始岸和距离
            if (radioBtnLeftBank.Checked)
            {
                bStartLeftBank = true;
            }
            else if (radioBtnRightBank.Checked)
            {
                bStartLeftBank = false;
            }

            iStartStyle = comboBoxStartBankStyle.SelectedIndex;
            dStartPara = numericUpDownStartBankPara.Value;
            strStartDistance = textBoxDistance.Text;
            bSetStartBank = true;

            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
           //LPJ 2013-2-21 当点击“取消”，则不采用该输入的数值
            this.Close();
        }

        private void comboBoxStartBankStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxStartBankStyle.SelectedIndex)
            {
                case 0:
                    {
                        numericUpDownStartBankPara.Value = 0.35M;
                        numericUpDownStartBankPara.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        numericUpDownStartBankPara.Value = 0.91M;
                        numericUpDownStartBankPara.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        numericUpDownStartBankPara.Value = 0.5M;
                        numericUpDownStartBankPara.Enabled = true;
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
            if (!validateInput.ValidateUserInput(numericUpDownStartBankPara.Text, 12))
            {
                btnOK.Enabled = false;
            }
        }

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (validateInput.ValidateCharInput(e.KeyChar, ((TextBox)sender).Text))
            {
                e.Handled = true;
            }
        }

        private void GetDefaultConf(FrmEdgeSetting.EdgeSetting startEdge)
        {
            if (startEdge.bStartLeft)
            {
                radioBtnLeftBank.Checked = true;
                textBoxDistance.Text = startEdge.dLeftDis.ToString();
                numericUpDownStartBankPara.Value = (decimal)startEdge.dLeftRef;
                comboBoxStartBankStyle.SelectedIndex = startEdge.iLeftType;
            }
            else
            {
                radioBtnRightBank.Checked = true;
                textBoxDistance.Text = startEdge.dRightDis.ToString();
                numericUpDownStartBankPara.Value = (decimal)startEdge.dRightRef;
                comboBoxStartBankStyle.SelectedIndex = startEdge.iRightType;
            }
            if (startEdge.bEnglishUnit)
                label3.Text = "(ft)";
            else
                label3.Text = "(m)";
        }
    }
}
