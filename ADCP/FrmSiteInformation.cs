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
    public partial class FrmSiteInformation : Form
    {
        public FrmSiteInformation(SiteInformation site)
        {
            InitializeComponent();
            GetReference(site);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            siteInfo.siteName = textBoxSiteName.Text;
            siteInfo.stationNumber = textBoxStationNumber.Text;
            siteInfo.MeasNumber = textBoxMeasNumber.Text;
            siteInfo.comments = textBoxComments.Text;

            siteInfo.FieldParty = textBoxFieldParty.Text;
            siteInfo.ProcessedBy = textBoxProcessedBy.Text;
            siteInfo.DeploymentType = comboBoxDeploymentType.SelectedIndex;

            siteInfo.BoatMotor = textBoxBoatMotor.Text;
            siteInfo.MeasLocation = textBoxMeasLocation.Text;

            siteInfo.InsideGageH = textBoxInsideGageH.Text;
            siteInfo.OutsideGageH = textBoxOutsideGageH.Text;
            siteInfo.GageHChange = textBoxGageHChange.Text;
            siteInfo.RatingDischarge = textBoxRatingDischarge.Text;
            siteInfo.IndexV = textBoxIndexV.Text;
            siteInfo.RatingNumber = textBoxRatingNumber.Text;
            siteInfo.RatedArea = textBoxRatedArea.Text;
            siteInfo.WaterTemp = textBoxWaterTemp.Text;
            siteInfo.MagnVariationMethod = comboBoxMagnVariationMethod.SelectedIndex;
            siteInfo.MeasurementRating = comboBoxMeasurementRating.SelectedIndex;
            //siteInfo.ControlCode1 = comboBoxControlCode1.SelectedIndex;
            //siteInfo.ControlCode2 = comboBoxControlCode2.SelectedIndex;
            //siteInfo.ControlCode3 = comboBoxControlCode3.SelectedIndex;

            this.Close();
        }
        private void GetReference(SiteInformation site)
        {
            textBoxSiteName.Text = site.siteName;
            textBoxStationNumber.Text = site.stationNumber;
            textBoxMeasNumber.Text = site.MeasNumber;
            textBoxComments.Text = site.comments;

            textBoxFieldParty.Text = site.FieldParty;
            textBoxProcessedBy.Text = site.ProcessedBy;
            comboBoxDeploymentType.SelectedIndex = site.DeploymentType;

            textBoxBoatMotor.Text = site.BoatMotor;
            textBoxMeasLocation.Text = site.MeasLocation;

            textBoxInsideGageH.Text = site.InsideGageH;
            textBoxOutsideGageH.Text = site.OutsideGageH;
            textBoxGageHChange.Text = site.GageHChange;
            textBoxRatingDischarge.Text = site.RatingDischarge;
            textBoxIndexV.Text = site.IndexV;
            textBoxRatingNumber.Text = site.RatingNumber;
            textBoxRatedArea.Text = site.RatedArea;
            textBoxWaterTemp.Text = site.WaterTemp;
            comboBoxMagnVariationMethod.SelectedIndex = site.MagnVariationMethod;
            comboBoxMeasurementRating.SelectedIndex = site.MeasurementRating;
            //comboBoxControlCode1.SelectedIndex = site.ControlCode1;
            //comboBoxControlCode2.SelectedIndex = site.ControlCode2;
            //comboBoxControlCode3.SelectedIndex = site.ControlCode3;
        }

        public SiteInformation siteInfo;
        public struct SiteInformation
        {
            public string siteName;
            public string stationNumber;
            public string MeasNumber;
            public string comments;

            public string FieldParty;
            public string ProcessedBy;
            public int DeploymentType;
            public string BoatMotor;
            public string MeasLocation;

            public string InsideGageH;
            public string OutsideGageH;
            public string GageHChange;
            public string RatingDischarge;
            public string IndexV;
            public string RatingNumber;
            public string RatedArea;
            public string WaterTemp;
            public int MagnVariationMethod;
            public int MeasurementRating;
            //public int ControlCode1;
            //public int ControlCode2;
            //public int ControlCode3;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                textBoxInsideGageH.Enabled = true;
            else
                textBoxInsideGageH.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                textBoxOutsideGageH.Enabled = true;
            else
                textBoxOutsideGageH.Enabled = false;
        }

      
    }
}
