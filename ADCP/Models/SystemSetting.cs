using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    public struct SystemSetting
    {
        public bool bEnglishUnit;
        public int iInstrumentTypes;

        //public int iFlowRef;
        public int iSpeedRef;
        public int iHeadingRef;
        public string strRS232;

        public int iMeasurmentMode;
        public int iVerticalBeam;
        public int iAutoBinSize;
        public int iAutoLag;
        public int iWaterTemperatureSource;
        public int iTransducerDepthSource;
        public int iSalinitySource;
        public int iSpeedOfSoundSource;

        public int iBins;

        public double dAveragingInterval;
        public double dMaxMeasurementDepth;
        public double dWpSwitchDepth;
        public double dBtSwitchDepth;
        public double dTransducerDepth;
        public double dBtCorrelationThreshold;
        public double dHeadingOffset;
        public double dMagneticVar;

        public double dBtSNR;

        public double dSalinity;
        public double dWaterTemperature;
        public double dSpeedOfSound;

        public string sAdcpPort;
        public int sAdcpBaud;
        public bool bGPSConnect;
        public string sGpsPort;
        public int sGpsBaud;
        public string firmware;
    }
}
