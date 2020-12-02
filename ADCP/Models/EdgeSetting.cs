using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
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
}
