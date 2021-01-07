using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ADCP
{
    public class Configurations
    {
        public struct Configuration
        {
            //测流配置
            //Flow measurement configuration
            public int TopMode;
            public int BottomMode;
            public float PowerCoff;
            public bool LeftToRight;
            public int ShoreEnsembleNumber;
            public float DraftInWater;
            public int LeftBankStyle;
            public float LeftBankPara;
            public float LeftDist;
            public int LeftBankPings;
            public int RightBankStyle;
            public float RightBankPara;
            public float RightDist;
            public int RightBankPings;
        }
    }
}
