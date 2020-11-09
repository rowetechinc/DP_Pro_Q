using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JDL.UILib;

namespace ADCP
{
    public partial class FrmEdgeSetting : Form
    {
        public FrmEdgeSetting(EdgeSetting edgeSetting)
        {
            InitializeComponent();
            GetReference(edgeSetting);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            edgeSet.iTopEstimate = comboBoxTopEstimate.SelectedIndex;
            edgeSet.iBottomEstimate = comboBoxBottomEstimate.SelectedIndex;
            edgeSet.dPowerCurveCoeff = (double)numericUpDownPowerCurveCoeff.Value;

            if (radioBtnLeft.Checked)
                edgeSet.bStartLeft = true;
            else
                edgeSet.bStartLeft = false;
            edgeSet.iLeftType = comboBoxLeftType.SelectedIndex;
            edgeSet.dLeftDis = double.Parse(textBoxLeftDis.Text);
            edgeSet.iRightType = comboBoxRightType.SelectedIndex;
            edgeSet.dRightDis = double.Parse(textBoxRightDis.Text);
            edgeSet.dLeftRef = (double)numericUpDownLeftRef.Value;
            edgeSet.dRightRef =(double) numericUpDownRightRef.Value;
            this.Close();
        }

        private void GetReference(EdgeSetting edgeSetting)
        {
            comboBoxTopEstimate.SelectedIndex = edgeSetting.iTopEstimate;
            comboBoxBottomEstimate.SelectedIndex = edgeSetting.iBottomEstimate;
            numericUpDownPowerCurveCoeff.Value = (decimal)(edgeSetting.dPowerCurveCoeff);

            if (edgeSetting.bStartLeft)
                radioBtnLeft.Checked = true;
            else
                radioBtnRight.Checked = true;

            comboBoxLeftType.SelectedIndex = edgeSetting.iLeftType;
           
            numericUpDownLeftRef.Text = edgeSetting.dLeftRef.ToString();

            comboBoxRightType.SelectedIndex = edgeSetting.iRightType;
           
            numericUpDownRightRef.Text = edgeSetting.dRightRef.ToString();

            if (edgeSetting.bEnglishUnit)
            {
                label8.Text = "(ft)";
                label9.Text = "(ft)";
            }
            else
            {
                label8.Text = "(m)";
                label9.Text = "(m)";
            }
            textBoxLeftDis.Text = edgeSetting.dLeftDis.ToString();
            textBoxRightDis.Text = edgeSetting.dRightDis.ToString();
        }

        public static EdgeSetting edgeSet;
        public struct EdgeSetting
        {
            public bool bEnglishUnit; //LPJ 2013-7-1
            public int iTopEstimate;
            public int iBottomEstimate;
            public double dPowerCurveCoeff;
            public bool bStartLeft;
            public int iLeftType;
            public double dLeftDis;
            public int iRightType;
            public double dRightDis;
            public double dLeftRef;
            public double dRightRef;
        }

        ClassValidateInPut validateInput = new ClassValidateInPut();
    
        private JDL.UILib.BalloonTip m_tip = new BalloonTip();
        private void ShowTip(string str,Control ctl)
        {
            m_tip.Visible = true;
            BalloonTipIconType icontype = (BalloonTipIconType)3;
            Point pos = ctl.PointToScreen(new Point(10, 2));
            int x = pos.X;
            int y = pos.Y;
            this.m_tip.ShowCloseButton = false;
            this.m_tip.ShowAt(x, y, "", str, icontype);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            try
            {
                if (!validateInput.ValidateUserInput(numericUpDownPowerCurveCoeff.Value.ToString(), 12))
                {
                    ShowTip(Resource1.String76, numericUpDownPowerCurveCoeff);
                    btnOK.Enabled = false;
                }
                if (!validateInput.ValidateUserInput(numericUpDownLeftRef.Value.ToString(), 12))
                {
                    ShowTip(Resource1.String76, numericUpDownLeftRef);
                    btnOK.Enabled = false;
                }
                if (!validateInput.ValidateUserInput(textBoxLeftDis.Text, 12))
                {
                    ShowTip(Resource1.String76, textBoxLeftDis);
                    btnOK.Enabled = false;
                }
                if (!validateInput.ValidateUserInput(numericUpDownRightRef.Value.ToString(), 12))
                {
                    ShowTip(Resource1.String76, numericUpDownRightRef);
                    btnOK.Enabled = false;
                }
                if (!validateInput.ValidateUserInput(textBoxRightDis.Text, 12))
                {
                    ShowTip(Resource1.String76, textBoxRightDis);
                    btnOK.Enabled = false;
                }
            }
            catch//(Exception ee)
            {
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (validateInput.ValidateCharInput(e.KeyChar, ((TextBox)sender).Text))
            {
                e.Handled = true;
            }
           
        }

        private void comboBoxLeftType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxLeftType.SelectedIndex)
            {
                case 0:
                    {
                        numericUpDownLeftRef.Value = (decimal)0.35;
                        numericUpDownLeftRef.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        numericUpDownLeftRef.Value = (decimal)0.91;
                        numericUpDownLeftRef.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        numericUpDownLeftRef.Value = (decimal)0.5;
                        numericUpDownLeftRef.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }

        private void comboBoxRightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxRightType.SelectedIndex)
            {
                case 0:
                    {
                        numericUpDownRightRef.Value = (decimal)0.35;
                        numericUpDownRightRef.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        numericUpDownRightRef.Value = (decimal)0.91;
                        numericUpDownRightRef.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        numericUpDownRightRef.Value = (decimal)0.5;
                        numericUpDownRightRef.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }


       
    }
}
