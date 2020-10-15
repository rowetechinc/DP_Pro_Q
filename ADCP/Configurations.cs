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
            ////工程设置
            //public float MaxWaterVel;//预估最大流速
            //public int EsnNumPerFile;//Ensemble在文件个数

            //public int WaterProfilerRef;//获得水流速方式，仪器、软件
            //public int BoatSpeedRef;//获得船速方式，底跟踪、GPS、无
            //public int TimeRef;    //获得时间方式，仪器、GPS、电脑
            //public int HeadingRef; //获得艏向方式，内置罗盘、GPS罗盘

            //仪器配置
            //public bool ADCPMode;//mode,ADCP/DVL

            //public int EsmbIntervalHH, EsmbIntervalMM;
            //public float EsmbIntervalSShh;//时间平均步长

            //public int BurstInterval_HH, BurstInterval_MM;//Burst Interval
            //public float BurstInterval_SS;
            //public int BurstInterval_n;

            //public bool WaterPingOpen; //水跟踪开关
            //public int WaterProfilerMode; //水跟踪带宽，宽带、窄带、脉冲相干等
            //public float WPLagLengthV; //LagLength
            //public float CWPAP1, CWPAP2, CWPAP3, CWPAP4, CWPAP5;
            //public float WPST_Correlation, WPST_QVelocity, WPST_VVelocity; //Water Profile Screening Thresholds
            //public float WPBlankSize;//盲区
            public float WPBinSize;//单元尺寸
            //public float WPWaterXmt;//脉冲长度
            public int WPBinNum;//单元层数
            //public int WPWaterAvgNum;//呯数
            //public int WaterAvgIntervalHH, WaterAvgIntervalMM;
            //public float WaterAvgIntervalSShh;//平均间隔
            //public float WPTimeBtwnPings;//呯间隔

            //public bool BtmTrkOpen;//底跟踪开关
            //public int BTMode;//底跟踪带宽
            //public float BTPulseLag, BTLongRangeDepth; //pulse to pulse lag，
            //public float BTST_Correlation, BTST_QV, BTST_V;  //LPJ 2012-10-19 add
            //public float BTT_SNRshallow, BTT_Depthshallow2deep, BTT_SNRdeep, BTT_Depthlow2high;  //
            //public float BtmTrkBlank;//盲区
            //public float BtmTrkDepth;//深度
            //public float BtmTrkInterval;//呯间隔

            //public bool WaterTrkOpen;//水参考开关
            //public int WTMode;//水参考带宽
            //public float WTBlankSize;//盲区
            //public float WTBinSize;//单元尺寸
            //public float WTInterval;//呯间隔

            //public float CWSCCwaterTemperature, CWSCCTransducerDepth, CWSCCSalinity, CWSCCSpeedOfSound;//Water Speed Of Sound Control 
            //public float WaterSalinity; //盐度
            //public float WaterTemperature; //水温
            //public float TransducerDepth;//换能器深度
            //public float SoundSpeed;//声速
            //public float HeadingOffset;//艏向偏角
            //public float R232; //波特率
            //public float R485;

            //测流配置
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
