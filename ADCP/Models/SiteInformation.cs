using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    /// <summary>
    /// Site information values.
    /// Used to define the location.
    /// </summary>
    public class SiteInformation
    {
        public string siteName { get; set; }
        public string stationNumber { get; set; }
        public string MeasNumber { get; set; }
        public string comments { get; set; }

        public string FieldParty { get; set; }
        public string ProcessedBy { get; set; }
        public int DeploymentType { get; set; }
        public string BoatMotor { get; set; }
        public string MeasLocation { get; set; }

        public string InsideGageH { get; set; }
        public string OutsideGageH { get; set; }
        public string GageHChange { get; set; }
        public string RatingDischarge { get; set; }
        public string IndexV { get; set; }
        public string RatingNumber { get; set; }
        public string RatedArea { get; set; }
        public string WaterTemp { get; set; }
        public int MagnVariationMethod { get; set; }
        public int MeasurementRating { get; set; }
        //public int ControlCode1;
        //public int ControlCode2;
        //public int ControlCode3;

        /// <summary>
        /// Initialize the values.
        /// </summary>
        public SiteInformation()
        {
            siteName = "";
            stationNumber = "";
            MeasNumber = "";
            comments = "";
            FieldParty = "";
            ProcessedBy = "";
            DeploymentType = 0;
            BoatMotor = "";
            MeasLocation = "";
            InsideGageH = "";
            OutsideGageH = "";
            GageHChange = "";
            RatingDischarge = "";
            IndexV = "";
            RatingNumber = "";
            RatedArea = "";
            WaterTemp = "";
            MagnVariationMethod = 0;
            MeasurementRating = 0;
        }
    }
}
