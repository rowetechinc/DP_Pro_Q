using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Calcflow;

namespace ADCP
{
    public class CalDischarge
    {
        //public DischargeSummary.Report CalFlow(List<ArrayClass> RTIData, int BinDataEnsembleNum,Configurations.Configuration conf,int LeftEns,int RightEns,int _BoatSpeedRef,ArrayList _GPS_VTG,int _HeadingRef,ArrayList _GPS_HDT,double fHeadingOffset)
        public DischargeSummary.Report CalFlow(List<ArrayClass> RTIData, int BinDataEnsembleNum, Configurations.Configuration conf, int LeftEns, int RightEns, int _BoatSpeedRef, ArrayList _BoatV_GPS,ArrayList _BoatV_GPGGA, int _HeadingRef, ArrayList _GPS_HDT, double fHeadingOffset)
        {
            DischargeSummary.Report rep = new DischargeSummary.Report();

            List<ArrayClass> left = new List<ArrayClass>();
            List<ArrayClass> right = new List<ArrayClass>();
         
            double topFlow = 0;
            double measuredFlow = 0;
            double bottomFlow = 0;
            double rightFlow = 0;
            double leftFlow = 0;
            double dShoreVelDir = 0;  //JZH 2012-04-06 岸边流速方向 用来判断岸边流量正负
            double dShoreCoff = 1.0;  //JZH 2012-04-06 岸边流量系数 用来判断岸边流量正负
            double lastSecond = double.NaN;

            float fAccEast = 0; //JZH 2012-04-08  底跟踪东向累积量
            float fAccNorth = 0; //JZH 2012-04-08 底跟踪北向累积量
            float fAccLength = 0; //JZH 2012-04-08 航迹累积长度
            bool bGetFirstGoodEnsemble = true; //JZH 2012-04-08 采集到第一个有效单元
            int iPrevGoodEnsembleNoOffset = 0;  //JZH 2012-04-08  底跟踪前一个有效单元的偏移
            int iPrevGoodEnsemblePos = 0;
            float fAccMG = 0;   //JZH 2012-04-08 直线距离
            double dCourseMG = 0; //JZH 2012-04-08 直线方向角
            float fRiverWidth = 0;  //JZH 2012-04-15 河宽

            ArrayList lEastLength = new ArrayList();   //JZH 2012-04-15
            ArrayList lNorthLength = new ArrayList();  //JZH 2012-04-15
            ArrayList lBottomDepth = new ArrayList();  //JZH 2012-04-15

            //JZH 2012-04-17 计算平均流向参数
            float fAccVx = 0;
            float fAccVy = 0;
            float fAveDepth = 0;
            float fArea = 0;
            double dRightShoreArea = 0.0;
            double dLeftShoreArea = 0.0;
            double fMeanFlowDir = 0;
            rep.MaxDepth = fAveDepth;

            List<float> MaxVelocity = new List<float>(); //LPJ 2012-7-23 该对象用于存储测量的所有有效水流速
            List<float> EnsMaxVel;  //LPJ 2012-7-23 该对象用于存储每个Ensemble的有效水流速

            List<float> XVelocity = new List<float>();  //LPJ 2012-12-26
            List<float> YVelocity = new List<float>(); //LPJ 2012-12-26

            List<int> EnsValue = new List<int>(); //该值用于标记每个Ensemble中的有效层数  LPJ 2012-12-27

            BoatSpeedRef = _BoatSpeedRef; //LPJ 2013-5-31
            HeadingRef = _HeadingRef;     //LPJ 2013-5-31
            //GPS_VTG = _GPS_VTG;           //LPJ 2013-5-31
            BoatV_GPS = _BoatV_GPS;   //LPJ 2013-11-18
            BoatV_GPGGA = _BoatV_GPGGA; //LPJ 2016-8-15
            GPS_Heading = _GPS_HDT;       //LPJ 2013-5-31

            //FrmProgressBar frmProgressBar = new FrmProgressBar(0, BinDataEnsembleNum); //LPJ 2013-7-1
            //frmProgressBar.Show();//LPJ 2013-7-1

            for (int i = 0; i < BinDataEnsembleNum; i++)
            {
                //frmProgressBar.setPos(i); //LPJ 2013-7-1

                EnsMaxVel = new List<float>(); //LPJ 2012-7-23 初始化  单个ensemble的水流速集合

                List<float> Ensfx = new List<float>(); //LPJ 2012-12-27 
                List<float> Ensfy = new List<float>(); //LPJ 2012-12-27

                //2012-04-08 计算导航信息
                #region 计算导航信息
                {
                    DP300_Windows.Velocity BoatSpeed = new DP300_Windows.Velocity(); //LPJ 2013-11-21
                    BoatSpeed.VX = RTIData[i].B_Earth[0];
                    BoatSpeed.VY = RTIData[i].B_Earth[1];
                    if (1 == BoatSpeedRef) //LPJ 2013-11-21
                        BoatSpeed = (DP300_Windows.Velocity)BoatV_GPS[i];
                    else if (2 == BoatSpeedRef)
                        BoatSpeed = (DP300_Windows.Velocity)BoatV_GPGGA[i];
                    else if (3 == BoatSpeedRef)
                    {
                        BoatSpeed.VX = 0;
                        BoatSpeed.VY = 0;
                    }

                    if (i == 0)
                    {
                        if (Math.Abs(BoatSpeed.VX) > 20 ||Math.Abs(BoatSpeed.VY) > 20)
                        {
                            //FirstGoodEnsembleNoOffset++; //起始数据组不是有效底跟踪数据
                            bGetFirstGoodEnsemble = false;
                        }
                        else  //JZH 2012-04-09 初始化流量计算参数
                        {
                            bGetFirstGoodEnsemble = true;
                            lastSecond = RTIData[0].A_FirstPingSeconds;
                            iPrevGoodEnsemblePos = 0;
                        }
                    }
                    else
                    {
                        if (Math.Abs( BoatSpeed.VX)< 20 &&Math.Abs(BoatSpeed.VY )< 20)
                        {
                            if (bGetFirstGoodEnsemble)
                            {
                                DP300_Windows.Velocity PreBoatSpeed=new DP300_Windows.Velocity(); //LPJ 2013-11-21
                                PreBoatSpeed.VX = RTIData[iPrevGoodEnsemblePos].B_Earth[0];
                                PreBoatSpeed.VY = RTIData[iPrevGoodEnsemblePos].B_Earth[1];
                                if (1 == BoatSpeedRef) //LPJ 2013-11-21
                                    PreBoatSpeed = (DP300_Windows.Velocity)BoatV_GPS[iPrevGoodEnsemblePos];
                                else if (2 == BoatSpeedRef) //LPJ 2016-8-15
                                    PreBoatSpeed = (DP300_Windows.Velocity)BoatV_GPGGA[iPrevGoodEnsemblePos];
                                else if (3 == BoatSpeedRef)
                                {
                                    PreBoatSpeed.VX = 0;
                                    PreBoatSpeed.VY = 0;
                                }

                                //float LEast = (1.0f) * 0.5f * (float)(RTIData[iPrevGoodEnsemblePos].B_Earth[0] + RTIData[i].B_Earth[0]) * (float)(RTIData[i].A_FirstPingSeconds - lastSecond);
                                //float LNorth = (1.0f) * 0.5f * (float)(RTIData[iPrevGoodEnsemblePos].B_Earth[1] + RTIData[i].B_Earth[1]) * (float)(RTIData[i].A_FirstPingSeconds - lastSecond);

                                float LEast = (1.0f) * 0.5f * (float)(PreBoatSpeed.VX + BoatSpeed.VX) * (float)(RTIData[i].A_FirstPingSeconds - lastSecond);
                                float LNorth = (1.0f) * 0.5f * (float)(PreBoatSpeed.VY + BoatSpeed.VY) * (float)(RTIData[i].A_FirstPingSeconds - lastSecond);

                                fAccEast = fAccEast + LEast;
                                fAccNorth = fAccNorth + LNorth;
                                fAccLength += (float)System.Math.Sqrt(System.Math.Pow(LEast, 2) + System.Math.Pow(LNorth, 2));
                                fAccMG = (float)System.Math.Sqrt(System.Math.Pow(fAccEast, 2) + System.Math.Pow(fAccNorth, 2));
                                dCourseMG = System.Math.Atan2(fAccEast, fAccNorth);

                                iPrevGoodEnsembleNoOffset = 0;
                                iPrevGoodEnsemblePos = i;

                                //JZH 2012-04-17 计算平均流向
                                float fVx = 0;
                                float fVy = 0;
                                CalculateAverageWaterSpeed(RTIData[i], i,conf, ref fVx, ref fVy, ref fAveDepth, ref EnsMaxVel, ref Ensfx, ref Ensfy);//LPJ 2012-12-27
                                
                                fAccVx += fVx;
                                fAccVy += fVy;
                                //boat speed playback
                                fAccVx -= PreBoatSpeed.VX;
                                fAccVy -= PreBoatSpeed.VY;

                                //LPJ 2012-5-31 最大水深
                                if (fAveDepth > rep.MaxDepth)
                                {
                                    rep.MaxDepth = fAveDepth;
                                }
                               
                                fMeanFlowDir = System.Math.Atan2(fAccVx, fAccVy);

                                double dProjectionDir = 0; //投影角度
                                if (!conf.LeftToRight) //LPJ 2012-7-9
                                 
                                    dProjectionDir = fMeanFlowDir - Math.PI / 2.0;
                                else
                                    dProjectionDir = fMeanFlowDir + Math.PI / 2.0;

                                //JZH 2012-04-15 计算投影长度和面积
                                lEastLength.Add(LEast);
                                lNorthLength.Add(LNorth);
                                 lBottomDepth.Add(getAverageB_depth(RTIData[i]));
                                 fRiverWidth = 0;
                                 fArea = 0;
                                 for (int k = 0; k < lEastLength.Count; k++)
                                 {
                                     fArea += ((float)lEastLength[k] * (float)Math.Sin(dProjectionDir) + (float)lNorthLength[k] * (float)Math.Cos(dProjectionDir)) * ((float)lBottomDepth[k] + (float)conf.DraftInWater); //LPJ 2012-7-9
                                     fRiverWidth += (float)lEastLength[k] * (float)Math.Sin(dProjectionDir) + (float)lNorthLength[k] * (float)Math.Cos(dProjectionDir);
                                 }

                                 //fArea += (LEast * (float)Math.Sin(dProjectionDir) + LNorth * (float)Math.Cos(dProjectionDir)) * ((float)getAverageB_depth(RTIData[i]) + (float)conf.DraftInWater); //LPJ 2012-7-9
                                 //fRiverWidth += LEast * (float)Math.Sin(dProjectionDir) + LNorth * (float)Math.Cos(dProjectionDir);
                                

                                //JZH 2012-04-09 计算中部流量
                                #region 中部流量
                                {
                                    CalculateEnsembleFlowParam param = new CalculateEnsembleFlowParam();
                                    param.RiverDischarge_dTime = RTIData[i].A_FirstPingSeconds - lastSecond;
                                    param.RiverDischargeBottomMode = conf.BottomMode == 0 ? BottomFlowMode.PowerFunction : BottomFlowMode.Constants; //LPJ 2012-7-9

                                    // switch (comboBoxTopMode)
                                    switch (conf.TopMode)  //LPJ 2012-7-9 修改
                                    {
                                        case 0:
                                            {
                                                param.RiverDischargeTopMode = TopFlowMode.PowerFunction;
                                                break;
                                            }
                                        case 1:
                                            {
                                                param.RiverDischargeTopMode = TopFlowMode.Constants;
                                                break;
                                            }
                                        case 2:
                                            {
                                                param.RiverDischargeTopMode = TopFlowMode.Slope;
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    param.RiverDischargeDraft = conf.DraftInWater;   //ADCP吃水 LPJ 2012-7-9
                                    param.RiverDischargeExponent = conf.PowerCoff;    //指数 LPJ 2012-7-9
                                    param.RiverDischargeConditions.RiverDischargeMinNG4 = 1;
                                    param.RiverDischargeInstrument.RiverDischargeBeamAngle = 20;
                                    param.RiverDischargeOrgData = RTIData[i];

                                    //helpme
                                    param.RiverDischargeInstrument.RiverDischargePulseLag = RTIData[i].WP_Lag;
                                    param.RiverDischargeInstrument.RiverDischargePulseLength = RTIData[i].A_CellSize + RTIData[i].WP_Lag;

                                    float ve, vn; //LPJ 2013-7-31
                                    ve = RTIData[i].B_Earth[0]; //LPJ 2013-7-31
                                    vn = RTIData[i].B_Earth[1]; //LPJ 2013-7-31
                                    double dHeadingOffset = 0; //LPJ 2013-9-13

                                    if (1 == BoatSpeedRef)
                                    {
                                        //Velocity boatV_GPS = new Velocity();
                                        //boatV_GPS = (Velocity)(EnsemblesInfoToStore.BoatV_GPS[i]);
                                        //string GPS_VTGBuffer;
                                        DP300_Windows.Velocity boatV_GPS = new DP300_Windows.Velocity();
                                        boatV_GPS = (DP300_Windows.Velocity)BoatV_GPS[i];
                                        if (BoatV_GPS.Count > 0)
                                        {
                                            //GPS_VTGBuffer = GPS_VTG[i].ToString();
                                            //getGPSBoatSpeed(GPS_VTGBuffer, ref ve, ref vn);
                                            ve = boatV_GPS.VX;
                                            vn = boatV_GPS.VY;
                                        }
                                        //ve = boatV_GPS.VX;
                                        //vn = boatV_GPS.VY;

                                        //dHeadingOffset = fHeadingOffset / 180 * Math.PI;  //LPJ 2017-5-15
                                    }
                                    else if (2 == BoatSpeedRef)
                                    {
                                        DP300_Windows.Velocity boatV_GPS = new DP300_Windows.Velocity();
                                        boatV_GPS = (DP300_Windows.Velocity)BoatV_GPGGA[i];
                                        if (BoatV_GPS.Count > 0)
                                        {
                                            ve = boatV_GPS.VX;
                                            vn = boatV_GPS.VY;
                                        }

                                        //dHeadingOffset = fHeadingOffset / 180 * Math.PI;   //LPJ 2017-5-15
                                    }
                                    else if (3 == BoatSpeedRef)
                                    {
                                        ve = 0;
                                        vn = 0;
                                    }

                                    EnsembleFlowInfo flow = Calcflow.RiverDischargeCalculate.CalculateEnsembleFlow(param, ve, vn, dHeadingOffset); //LPJ  2013-7-31
                                    
                                  //  EnsembleFlowInfo flow = Calcflow.RiverDischargeCalculate.CalculateEnsembleFlow(param);
                                    
                                    if (flow.Valid)
                                    {//有效的数据
                                        //JZH 2012-02-05  根据左右岸，判定流量正负
                                        if (conf.LeftToRight)  //LPJ 2012-7-9
                                        {
                                            topFlow += flow.TopFlow;
                                            measuredFlow += flow.MeasuredFlow;
                                            bottomFlow += flow.BottomFlow;
                                        }
                                        else
                                        {
                                            topFlow += flow.TopFlow * (-1.0);
                                            measuredFlow += flow.MeasuredFlow * (-1.0);
                                            bottomFlow += flow.BottomFlow * (-1.0);
                                        }
                                        lastSecond = RTIData[i].A_FirstPingSeconds;
                                        //添加判断左右岸

                                        if (left.Count < LeftEns)
                                        {
                                            left.Add(RTIData[i]);
                                        }
                                        else
                                        {
                                            if (!conf.LeftToRight)   //LPJ 2012-7-9
                                           
                                            {
                                                if (left.Count > 0)
                                                    left.RemoveAt(0);
                                                left.Add(RTIData[i]);
                                            }
                                        }

                                        if (right.Count < RightEns)
                                        {
                                            right.Add(RTIData[i]);
                                        }
                                        else
                                        {
                                            if (conf.LeftToRight) //LPJ 2012-7-9
                                            {
                                                if (right.Count > 0)
                                                    right.RemoveAt(0);
                                                right.Add(RTIData[i]);
                                            }
                                        }
                                    }
                                    lastSecond = RTIData[i].A_FirstPingSeconds;
                                }
                                #endregion
                            }
                            else
                            {
                               
                                bGetFirstGoodEnsemble = true;
                                iPrevGoodEnsemblePos = i;
                                //JZH 2012-04-09 
                                lastSecond = RTIData[i].A_FirstPingSeconds;
                            }
                        }
                        else
                        {
                            iPrevGoodEnsembleNoOffset++;
                        }
                    }
                }
                #endregion

                XVelocity.AddRange(Ensfx);  //LPJ 2012-12-26
                YVelocity.AddRange(Ensfy);  //LPJ 2012-12-26
                if (Ensfx.Count() > 0)
                {
                    EnsValue.Add(Ensfx.Count());  //LPJ 2012-12-27 记录每个Ensemble中的有效层数
                }
            }

          
           //计算最大流速 
            rep.MaxVel=GetMaxVelocity(EnsValue, XVelocity, YVelocity); 

            if (dCourseMG < 0)
                dCourseMG = dCourseMG / Math.PI * 180 + 360;
            else
                dCourseMG = dCourseMG / Math.PI * 180;
           
            #region 计算左岸流量
            try
            {
                CalculateShoreFlowParam param = new CalculateShoreFlowParam();
                //岸边系数
                param.RiverDischarge_A = (double)conf.LeftBankPara;  //LPJ 2012-7-9
                //ADCP吃水
                param.RiverDischargeDraft = conf.DraftInWater; //LPJ 2012-7-9
                //岸边距离
                param.RiverDischargeDistance = conf.LeftDist; //LPJ 2012-7-9
                param.RiverDischargeInstrument.RiverDischargeBeamAngle = 20;        //ADCP参数
                param.RiverDischargeConditions.RiverDischargeMinNG4 = 1;  //判断GOODBIn条件
                //helpme
                param.RiverDischargeInstrument.RiverDischargePulseLag = RTIData[BinDataEnsembleNum - 1].WP_Lag;
                param.RiverDischargeInstrument.RiverDischargePulseLength = RTIData[BinDataEnsembleNum - 1].A_CellSize + RTIData[BinDataEnsembleNum - 1].WP_Lag;

                param.RiverDischargeOrgData = left.ToArray();
                leftFlow = Calcflow.RiverDischargeCalculate.CalculateShoreFlow(param);
                //JZH 2012-04-08 添加岸边流速方向角计算，用于判断岸边流量的正负值                
                dShoreVelDir = Calcflow.RiverDischargeCalculate.CalculateShoreVelocity(param);  //JZH 2012-04-06 获取岸边平均流速的方向
                if (dCourseMG >= 0 && dCourseMG <= 180)
                {
                    if (dShoreVelDir - dCourseMG >= 0 && dShoreVelDir - dCourseMG <= 180)
                    {
                        dShoreCoff = -1.0;
                    }
                    else
                    {
                        dShoreCoff = 1.0;
                    }
                }
                else
                {
                    if (dShoreVelDir - dCourseMG <= 0 && dShoreVelDir - dCourseMG >= -180)
                    {
                        dShoreCoff = 1.0;
                    }
                    else
                    {
                        dShoreCoff = -1.0;
                    }
                }

                //JZH 2012-02-05 采用简易判定方法，确定左右岸流量正负
                if (!conf.LeftToRight)
                {
                    leftFlow = Math.Abs(leftFlow) * dShoreCoff * (-1.0);
                }
                else
                {
                    leftFlow = Math.Abs(leftFlow) * dShoreCoff;
                }

                //JZH 2012-04-16 计算岸边平均水深
                double dLeftShoreAvgDepth = 0.0;
                dLeftShoreAvgDepth = CalculateAvgShoreDepth(left) + conf.DraftInWater;  //LPJ 2012-7-9

                //JZH 2012-04-16 计算岸边面积
                double dLeftShoreWidth = conf.LeftDist; //LPJ 2012-7-9
                double dLeftShoreCoff = conf.LeftBankPara;  //LPJ 2012-7-9
                dLeftShoreArea = CalculateShoreArea(dLeftShoreAvgDepth, dLeftShoreWidth, dLeftShoreCoff);

            }
            catch //(System.Exception ex)
            {

            }

            #endregion

            #region 计算右岸流量
            try
            {
                CalculateShoreFlowParam param = new CalculateShoreFlowParam();
                param.RiverDischarge_A = conf.RightBankPara;  //LPJ 2012-7-9
                param.RiverDischargeDraft = conf.DraftInWater; //LPJ 2012-7-9

                param.RiverDischargeDistance = conf.RightDist; //LPJ 2012-7-9
                param.RiverDischargeInstrument.RiverDischargeBeamAngle = 20;
                param.RiverDischargeConditions.RiverDischargeMinNG4 = 1;

                //helpme
                param.RiverDischargeInstrument.RiverDischargePulseLag = RTIData[BinDataEnsembleNum - 1].WP_Lag;
                param.RiverDischargeInstrument.RiverDischargePulseLength = RTIData[BinDataEnsembleNum - 1].A_CellSize + RTIData[BinDataEnsembleNum - 1].WP_Lag;

                param.RiverDischargeOrgData = right.ToArray();
                rightFlow = Calcflow.RiverDischargeCalculate.CalculateShoreFlow(param);

                //JZH 2012-04-08 添加岸边流速方向角计算，用于判断岸边流量的正负值                
                dShoreVelDir = Calcflow.RiverDischargeCalculate.CalculateShoreVelocity(param);  //JZH 2012-04-06 获取岸边平均流速的方向
                if (dCourseMG >= 0 && dCourseMG <= 180)
                {
                    if (dShoreVelDir - dCourseMG >= 0 && dShoreVelDir - dCourseMG <= 180)
                    {
                        dShoreCoff = -1.0;
                    }
                    else
                    {
                        dShoreCoff = 1.0;
                    }
                }
                else
                {
                    if (dShoreVelDir - dCourseMG <= 0 && dShoreVelDir - dCourseMG >= -180)
                    {
                        dShoreCoff = 1.0;
                    }
                    else
                    {
                        dShoreCoff = -1.0;
                    }
                }
                //JZH 2012-02-05 采用简易判定方法，确定左右岸流量正负
                if (!conf.LeftToRight) //LPJ 2012-7-23 判断左右岸符号反了
                {
                    rightFlow = Math.Abs(rightFlow) * dShoreCoff * (-1.0);
                }
                else
                {
                    rightFlow = Math.Abs(rightFlow) * dShoreCoff;
                }


                //JZH 2012-04-16 计算岸边平均水深
                double dRightShoreAvgDepth = 0.0;
                dRightShoreAvgDepth = CalculateAvgShoreDepth(right) + conf.DraftInWater;  //JZH 2012-04-17  LPJ 2012-7-9

                //JZH 2012-04-16 计算岸边面积
                double dRightShoreWidth = conf.RightDist; //LPJ 2012-7-9  //LPJ 2013-4-12  之前将左岸的距离用作计算右岸宽度上了，修改
                double dRightShoreCoff = conf.RightBankPara; //LPJ 2012-7-9 //LPJ 2013-4-12
                dRightShoreArea = CalculateShoreArea(dRightShoreAvgDepth, dRightShoreWidth, dRightShoreCoff);
            }
            catch //(System.Exception ex)
            {

            }
            #endregion


            #region 流量信息和导航信息汇总
         
            //导航
            float fTotalWidth = Math.Abs(fRiverWidth) + (float)conf.RightDist + (float)conf.LeftDist;  //LPJ 2012-7-9 //河宽
            double dTotalArea = 0;
            dTotalArea = Math.Abs(fArea) + dLeftShoreArea + dRightShoreArea; //总面积
         
            double dMeanTemp = 0;    //平均流向
            if (fMeanFlowDir < 0)
                dMeanTemp = fMeanFlowDir / Math.PI * 180 + 360;
            else
                dMeanTemp = fMeanFlowDir / Math.PI * 180;
           
            double dMeanFlowAverage = 0; //平均流速
            if (dTotalArea == 0)
                dMeanFlowAverage = 0;
            else
                dMeanFlowAverage = Math.Abs(leftFlow + rightFlow + topFlow + measuredFlow + bottomFlow) / dTotalArea;
            
            rep.TotalQ = leftFlow + rightFlow + topFlow + measuredFlow + bottomFlow;
            rep.WaterWidth = Math.Abs(fTotalWidth);
            rep.Area = dTotalArea;
            rep.AverageVel = dMeanFlowAverage;
            rep.AverageDepth = (float)(rep.Area / rep.WaterWidth);
            rep.RightToLeft = !conf.LeftToRight;
            rep.LDis = conf.LeftDist;
            rep.RDis = conf.RightDist;
            rep.dUpDownCoff = conf.PowerCoff;

            //LPJ 2013-6-19 增加ListViewSummary数据 --start
            rep.startDate = RTIData[0].YYYY.ToString("0000") + "/" + RTIData[0].MM.ToString("00") + "/" + RTIData[0].DD.ToString("00");
            rep.startTime = RTIData[0].HH.ToString("0000") + ":" + RTIData[0].mm.ToString("00") + ":" + RTIData[0].SS.ToString("00");

            double duration = (Cal_JulianDay((int)RTIData[BinDataEnsembleNum - 1].YYYY, (int)RTIData[BinDataEnsembleNum - 1].MM, (int)RTIData[BinDataEnsembleNum - 1].DD,
                (int)RTIData[BinDataEnsembleNum - 1].HH, (int)RTIData[BinDataEnsembleNum - 1].mm, (int)RTIData[BinDataEnsembleNum - 1].SS) -
                Cal_JulianDay((int)RTIData[0].YYYY, (int)RTIData[0].MM, (int)RTIData[0].DD, (int)RTIData[0].HH, (int)RTIData[0].mm, (int)RTIData[0].SS)) * 24 * 3600;

            int hh = (int)(duration / 3600.0);
            int mm = (int)((duration - hh * 60) / 60);
            double ss = duration - hh * 3600 - mm * 60;
            rep.Duration = hh.ToString("00") + ":" + mm.ToString("00") + ":" + ss.ToString("00.0");
           
            rep.TopQ = topFlow;
            rep.BottomQ = bottomFlow;
            rep.LeftQ = leftFlow;
            rep.RightQ = rightFlow;
            rep.MiddleQ = measuredFlow;
            rep.MeanCourse = dMeanTemp;
            rep.Length = fAccLength;
            rep.CourseMG = dCourseMG;
            rep.DistanceMG = fAccMG;
            //LPJ 2013-6-19 增加ListViewSummary数据 --end

            #endregion

            //frmProgressBar.Close(); //LPJ 2013-7-1
            return rep;
            
        }

        private void CalculateAverageWaterSpeed(ArrayClass RTIData,int iNum,Configurations.Configuration conf, ref float fAverageVX, ref float fAverageVY, ref float fAverageDepth, ref List<float> ensMaxVel, ref List<float> ensFx, ref List<float> ensFy) //LPJ 2012-12-27
        {
            float VXvalue;
            float VYvalue;
            int iGoodBinNum;    //数据组中深度有效单元数
            fAverageVX = 0;
            fAverageVY = 0;

            float RangeOfMaxDepth;     //最大有效数据单元深度
            float BinSize = RTIData.A_CellSize;
            
            fAverageDepth = getAverageB_depth(RTIData);
            RangeOfMaxDepth = (float)(fAverageDepth * (1 - 0.06) - BinSize - ((float)RTIData.A_FirstCellDepth - BinSize / 2.0f));

            iGoodBinNum = (int)(RangeOfMaxDepth / BinSize);
            
            int iBinCount = GetVelocityBins(RTIData, 0);

            if (iGoodBinNum > iBinCount)
                iGoodBinNum = iBinCount;

            int icountValid = 0;   //有效的单元数
            DP300_Windows.Velocity[] vel = new DP300_Windows.Velocity[iBinCount];
            for (int icount = 0; icount < iGoodBinNum; icount++)
            {
                float maxvel;
                getWaterVelocity(RTIData, iNum, vel); //LPJ 2013-11-21 计算绝对流速
                VYvalue = vel[icount].VY;
                VXvalue = vel[icount].VX;
                
                if (Math.Abs(VXvalue )> 20.0f || Math.Abs(VYvalue) > 20.0f)
                {
                    continue;
                }

                ensFx.Add(VXvalue); //LPJ 2012-12-27
                ensFy.Add(VYvalue); //LPJ 2012-12-27

                icountValid++;
                fAverageVX += VXvalue;
                fAverageVY += VYvalue;
                maxvel = (float)Math.Sqrt(Math.Pow(VXvalue, 2) + Math.Pow(VYvalue, 2)); //2012-6-19 在判断是否为有效单元层数后，计算有效水流速
                ensMaxVel.Add(maxvel); //LPJ 2012-7-23
            }

            if (icountValid == 0)
            {
                fAverageVX = 0;
                fAverageVY = 0;
            }
            else
            {
                fAverageVX = fAverageVX / icountValid;
                fAverageVY = fAverageVY / icountValid;
            }
        }

        //JZH 2012-04-16 计算岸边平均深度
        private double CalculateAvgShoreDepth(List<ArrayClass> array)
        {
            List<double> all_range = new List<double>();
            for (int i = 0; i < array.Count; i++)
            {
                List<double> ensemble_range = new List<double>();
                for (int j = 0; j < 4; j++)
                {
                    if (array[i].B_Range[j] != 0.0)
                    {
                        ensemble_range.Add(array[i].B_Range[j]);
                    }
                }
                if (ensemble_range.Count == 0)
                    continue;
                double ensemble_avg = 0.0;
                foreach (double _range in ensemble_range)
                {
                    ensemble_avg += _range;
                }
                ensemble_avg /= ensemble_range.Count;

                all_range.Add(ensemble_avg);
            }
            double avg_range = 0.0;
            foreach (double e_range in all_range)
            {
                avg_range += e_range;
            }
            avg_range /= all_range.Count;
            return avg_range;
        }

        //JZH 2012-04-17 计算岸边面积
        private double CalculateShoreArea(double dDepth, double dShoreWidth, double coff)
        {
            if (coff >= 0.91)
                return dDepth * dShoreWidth;
            else
                return dDepth * dShoreWidth * ((coff - 0.35) / (0.91 - 0.35) * 0.5 + 0.5);
        }

        private float getAverageB_depth(ArrayClass a)
        {
            float dep = 0;
            float lim = 0.00001f;
            int j = 0;
            for (int i = 0; i < 4; i++)
            {
                if (a.B_Range[i] >= lim)
                {
                    dep += a.B_Range[i];
                    j++;
                }
            }
            //LPJ 2012-6-20 增加对每个波束的底跟踪数据的判断，当平均值与各个波束的差值中最大的那个值大于5时，认为该波束的底跟踪数据不正确，则剔除该数值，
            //然后重新平均其他数值，在这里仅仅考虑存在一个异常值的情况，若有多个则要迭代计算，需要再判断，如果不大于，则该平均值有效
            float avedep = 0;
            if (j != 0)
                avedep = dep / j;
            else
                return 0f;

            dep = 0;
            j = 0;
            float maxave = 0;
            int flag = 0;  //标记差值最大的那个数值的位置
            for (int i = 0; i < 4; i++)
            {
                if (a.B_Range[i] >= lim)
                {
                    if (Math.Abs(a.B_Range[i] - avedep) > maxave)
                    {
                        maxave = Math.Abs(a.B_Range[i] - avedep);
                        flag = i;
                    }
                }
            }

            dep = 0;
            j = 0;
            for (int i = 0; i < 4; i++)
            {
                if (a.B_Range[i] >= lim)
                {
                    if (flag == i && Math.Abs(a.B_Range[i] - avedep) > 5) //判断最大差值是否大于5，若大于则剔除
                    {
                        continue;
                    }
                    dep += a.B_Range[i];
                    j++;
                }
            }
            return (j == 0) ? 0f : (dep / j);
        }

        private static int MaxArray = 11;
        private int GetVelocityBins(ArrayClass Arr, int flag)
        {
            int i;
            string VelocityID = "E000001\0";
            for (i = 0; i < MaxArray; i++)
            {
                if (VelocityID.Equals(Arr.Name[i], StringComparison.Ordinal))
                {
                    return Arr.Bins[i];
                }
            }
            return 0;
        }

        private int BoatSpeedRef, HeadingRef; //LPJ 2013-5-31
        private ArrayList BoatV_GPS = new ArrayList();      //LPJ 2013-5-31
        private ArrayList BoatV_GPGGA = new ArrayList(); //LPJ 2016-8-15
        private ArrayList GPS_Heading = new ArrayList();  //LPJ 2013-5-31

        private void getWaterVelocity(ArrayClass Arr, int iNum, DP300_Windows.Velocity[] Vel)
        {
            int n = GetVelocityBins(Arr, 0);          //得到水单元层数           
            DP300_Windows.Velocity[] waterVel = new DP300_Windows.Velocity[n];

            DP300_Windows.Velocity Bv = new DP300_Windows.Velocity();
            Bv.VX = Arr.B_Earth[0];
            Bv.VY = Arr.B_Earth[1];
            Bv.VZ = Arr.B_Earth[2];

            if (1 == BoatSpeedRef) //LPJ 2013-11-21
            {
                Bv.VX = ((DP300_Windows.Velocity)BoatV_GPS[iNum]).VX;
                Bv.VY = ((DP300_Windows.Velocity)BoatV_GPS[iNum]).VY;
            }
            else if (2 == BoatSpeedRef) //LPJ 2013-11-21
            {
                Bv.VX = ((DP300_Windows.Velocity)BoatV_GPGGA[iNum]).VX;
                Bv.VY = ((DP300_Windows.Velocity)BoatV_GPGGA[iNum]).VY;
            }
            else if (3 == BoatSpeedRef)
            {
                Bv.VX = 0;
                Bv.VY = 0;
            }

            if (Bv.VX >= 80.0 || Bv.VY >= 80.0 || Bv.VZ >= 80.0)
            {
                Bv.VX = 0;
                Bv.VY = 0;
                Bv.VZ = 0;
            }

            //LPJ 2013-5-29 当选择无船速时 --start
            //if (2 ==  BoatSpeedRef)
            //{
            //    Bv.VX = 0;
            //    Bv.VY = 0;
            //    Bv.VZ = 0;
            //}
            ////当选择GPS计算船速时
            //else if (1 ==  BoatSpeedRef)
            //{
            //    string GPS_VTGBuffer;
            //    if (GPS_VTG.Count > 0)
            //    {
            //        GPS_VTGBuffer = GPS_VTG[iNum].ToString();
            //        getGPSBoatSpeed(GPS_VTGBuffer, ref Bv.VX, ref Bv.VY);
            //        Bv.VZ = 0;
            //    }
            //    else
            //    {
            //        Bv.VX = 0;
            //        Bv.VY = 0;
            //        Bv.VZ = 0;
            //    }
            //}
            //LPJ 2013-2-22 当用户勾选GPS计算船速时，并采用GPS数据计算绝对流速--end

            for (int i = 0; i < n; i++)
            {
                waterVel[i].VX = Arr.Earth[0, i];
                waterVel[i].VY = Arr.Earth[1, i];
                waterVel[i].VZ = Arr.Earth[2, i];  //注意，以后改写Velocity结构，加入VQ JZH 2012-03-21
                //JZH 2012-03-21 绝对速度
                if (waterVel[i].VX <= 80.0 && waterVel[i].VY <= 80.0 && waterVel[i].VZ <= 80.0)
                {
                    waterVel[i].VX += Bv.VX;  //JZH 2012-03-21 获取绝对速度
                    waterVel[i].VY += Bv.VY;
                    waterVel[i].VZ += Bv.VZ;
                }
            }

            //LPJ 2013-5-29 艏向采用外部罗经的heading时 --start
            //if (1 == HeadingRef)
            //{
            //    Velocity Bv_instrument;
            //    Bv_instrument.VX = Arr.B_Instrument[0];
            //    Bv_instrument.VY = Arr.B_Instrument[1];
            //    Bv_instrument.VZ = Arr.B_Instrument[2];

            //    string GPS_HDT = GPS_HDTdecode(GPS_Heading[iNum].ToString());
            //    Bv = CalXYZ2ENU(Bv_instrument, float.Parse(GPS_HDT), Arr.B_Pitch, Arr.B_Roll);

            //    for (int i = 0; i < n; i++) //艏向采用仪器heading
            //    {
            //        Velocity Vinstrument;
            //        Vinstrument.VX = Arr.Instrument[0, i];
            //        Vinstrument.VY = Arr.Instrument[1, i];
            //        Vinstrument.VZ = Arr.Instrument[2, i];
            //        waterVel[i] = CalXYZ2ENU(Vinstrument, float.Parse(GPS_HDT), Arr.A_Pitch, Arr.A_Roll);

            //        //JZH 2012-03-21 绝对速度
            //        if (waterVel[i].VX <= 80.0 && waterVel[i].VY <= 80.0 && waterVel[i].VZ <= 80.0)
            //        {
            //            waterVel[i].VX += Bv.VX;  //JZH 2012-03-21 获取绝对速度
            //            waterVel[i].VY += Bv.VY;
            //            waterVel[i].VZ += Bv.VZ;
            //        }
            //    }
            //}

            for (int i = 0; i < n; i++)
            {
                Vel[i].VX = waterVel[i].VX;
                Vel[i].VY = waterVel[i].VY;
                Vel[i].VZ = waterVel[i].VZ;
            }
            //LPJ 2013-5-29 艏向采用GPS的heading时 --end
        }

        private string GPS_HDTdecode(string data1)
        {
            string GPS_HDT;
            try
            {
                string[] split3 = data1.Split('$');    //Modified 2011-7-26
                string HDT_str = split3[1];            //Modified 2011-7-26
                string[] HDT1 = HDT_str.Split(',');     //Modified 2011-7-26
                GPS_HDT = HDT1[1];
               
            }
            catch
            {
                GPS_HDT = "0.0";
            }
            return GPS_HDT;
        }

        //LPJ 2013-5-29 提取在X、Y方向分量上的GPS中的船速，--start
        private void getGPSBoatSpeed(string data1, ref float gpsVx, ref float gpsVy)
        {
            double gpsAngle;
            float gpsShipSpeed;
            try
            {
                string[] split2 = data1.Split('$');
                string VTG_str = split2[1];
                string[] vtg1 = VTG_str.Split(',');
                gpsAngle = double.Parse(vtg1[1]) * Math.PI / 180.0f;                     //提取运动角度，真北方向,单位:度,转为弧度
                gpsShipSpeed = float.Parse(vtg1[5]) * 1.852f * 1000.0f / 3600.0f;                 // 提取水平运动速度数据，单位：节，转为:m/s

                gpsVx = (float)(gpsShipSpeed * Math.Sin(gpsAngle));
                gpsVy = (float)(gpsShipSpeed * Math.Cos(gpsAngle));
            }
            catch
            {
                gpsVx = 0;
                gpsVy = 0;
            }
        }//LPJ 2013-5-29 提取在X、Y方向分量上的GPS中的船速，--end

        private DP300_Windows.Velocity CalXYZ2ENU(DP300_Windows.Velocity V, float Heading, float Pitch, float Roll) //LPJ 2013-5-29
        {
            DP300_Windows.Velocity Ve = new DP300_Windows.Velocity();

            //将角度转为弧度
            float BeamAngle = (float)(Math.PI / 180.0) * 20;
            float X, Y, Z;
            X = V.VX;
            Y = V.VY;
            Z = V.VZ;

            Heading = (float)(Math.PI / 180.0) * Heading;
            Pitch = (float)(Math.PI / 180.0) * Pitch;
            Roll = (float)(Math.PI / 180.0) * Roll;

            float SH = (float)Math.Sin(Heading);
            float CH = (float)Math.Cos(Heading);
            float SP = (float)Math.Sin(Pitch);
            float CP = (float)Math.Cos(Pitch);
            float SR = (float)Math.Sin(Roll);
            float CR = (float)Math.Cos(Roll);
            float SBA = (float)Math.Sin(BeamAngle);
            float CBA = (float)Math.Cos(BeamAngle);

            Ve.VX = X * (SH * CP) - Y * (CH * CR + SH * SR * SP) + Z * (CH * SR - SH * CR * SP);
            Ve.VY = X * (CH * CP) + Y * (SH * CR - CH * SR * SP) - Z * (SH * SR + CH * SP * CR);
            Ve.VZ = X * (SP) + Y * (SR * CP) + Z * (CP * CR);

            return Ve;
        }

        //public DischargeSummary.Report GetReportData()
        //{
        //    DischargeSummary.Report rep = new DischargeSummary.Report();

        //    int x = PathStr.LastIndexOf("\\");
        //    rep.filepath = PathStr.Substring(0, x); //文件路径  ....//PlaybackData
        //    x = rep.filepath.LastIndexOf("\\");
        //    rep.fileName = rep.filepath.Substring(x + 1, rep.filepath.Length - x - 1);

        //    rep.LDis = (double)numericUpDownLeftDist.Value; //左岸距离
        //    rep.RDis = (double)numericUpDownRightDist.Value; //右岸距离
        //    if (radioButtonLeftToRight.Checked)
        //        rep.RightToLeft = false;
        //    else
        //        rep.RightToLeft = true;
        //    rep.TotalQ = dLeftFlow + dRightFlow + dTopFlow + dMeasuredFlow + dBottomFlow;
        //    rep.WaterWidth = (float)(Math.Abs(fMeasRiverWidth) + rep.LDis + rep.RDis); //河宽
        //    rep.Area = Math.Abs(fMeasArea) + dGLeftShoreArea + dGRightShoreArea; //总面积

        //    rep.AverageVel = rep.TotalQ / rep.Area; //
        //    rep.AverageDepth = (float)(rep.Area / rep.WaterWidth);
        //    rep.dUpDownCoff = (double)numericUpDownDraft.Value;

        //    rep.MaxDepth = 0;

        //    List<float> ensVx = new List<float>();
        //    List<float> ensVy = new List<float>();
        //    List<int> ensGoodBin = new List<int>();
        //    //循环计算最大水深和最大流速
        //    for (int i = 0; i < EnsemblesInfoToStore.bottomDepth.Count; i++)
        //    {
        //        if (rep.MaxDepth < (float)EnsemblesInfoToStore.bottomDepth[i])
        //            rep.MaxDepth = (float)EnsemblesInfoToStore.bottomDepth[i];

        //        for (int j = 0; j < (int)EnsemblesInfoToStore.iGoodBin[i]; j++)
        //        {
        //            ensVx.Add((float)((Velocity[])EnsemblesInfoToStore.WaterVelocity[i])[j].VX);
        //            ensVy.Add((float)((Velocity[])EnsemblesInfoToStore.WaterVelocity[i])[j].VY);
        //        }
        //        ensGoodBin.Add((int)EnsemblesInfoToStore.iGoodBin[i]);
        //    }
        //    rep.MaxVel = GetMaxVelocity(ensGoodBin, ensVx, ensVy);

        //    return rep;
        //}

        private float GetMaxVelocity(List<int> EnsGoodBin, List<float> XVelocity, List<float> YVelocity) //LPJ 2013-5-29 汇总时计算最大流速，每30个Ensemble平均获得
        {
            //EnsGoodBin用于记录每个Ensemble中的有效层数
            //XVelocity、YVelocity用于记录所有有效层数的X、Y方向流速

            float avevelocity = 0;      //平均流速
            float sumXvelocity = 0;   //X方向流速总和
            float sumYveloctiy = 0;   //Y方向流速总和
            float aveXvelocity = 0;   //X方向流速平均
            float aveYvelocity = 0;   //Y方向流速平均

            float MaxVel = 0;
            int flag = 0;  //记录个数
            int flagNum = 0;
            for (int i = 0; i < EnsGoodBin.Count; i++)
            {
                if (i >= 30)
                {
                    if (flag > 0) //计算30个Ensemble的流速平均值
                    {
                        aveXvelocity = sumXvelocity / flag;
                        aveYvelocity = sumYveloctiy / flag;
                        avevelocity = (float)Math.Sqrt(Math.Pow(aveXvelocity, 2) + Math.Pow(aveYvelocity, 2));

                        if (avevelocity > MaxVel)
                            MaxVel = avevelocity;

                        for (int ik = 0; ik < EnsGoodBin[i - 30]; ik++)
                        {
                            sumXvelocity = sumXvelocity - XVelocity[ik + flagNum - flag];
                            sumYveloctiy = sumYveloctiy - YVelocity[ik + flagNum - flag];
                        }

                        flag = flag - EnsGoodBin[i - 30];
                    }
                }
                for (int j = 0; j < EnsGoodBin[i]; j++)
                {
                    sumXvelocity += XVelocity[flagNum];
                    sumYveloctiy += YVelocity[flagNum++];
                    flag++;
                }
            }

            if (EnsGoodBin.Count < 30 && EnsGoodBin.Count > 0)
            {
                aveXvelocity = sumXvelocity / flag;
                aveYvelocity = sumYveloctiy / flag;
                MaxVel = (float)Math.Sqrt(Math.Pow(aveXvelocity, 2) + Math.Pow(aveYvelocity, 2));
            }
            return MaxVel;
        }
    
        //private struct Velocity //LPJ 2013-11-18 
        //{
        //    public float VX;
        //    public float VY;
        //    public float VZ;
        //}

        //将公历时刻转换为儒略日
        public double Cal_JulianDay(int year, int month, int day, int hour, int min, int sec)
        {
            int f = 0, g = 0, mid1, mid2;
            double J, JulianDay, A;
            if (month >= 3)
            {
                f = year;
                g = month;
            }
            if (month == 1 || month == 2)
            {
                f = year - 1;
                g = month = 12;
            }
            mid1 = (int)(365.25 * f);
            mid2 = (int)(30.6001 * (g + 1));
            A = 2 - (int)(f / 100.0) + (int)(f / 400.0);
            J = mid1 + mid2 + day + A + 1720994.5;
            JulianDay = J + hour / 24.0 + min / 1440.0 + sec / 86400.0;

            return JulianDay;
        }

    }
}
