using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calcflow;
using EstimatingFlowMode;

namespace ADCP
{
    class CSection_SectionDischarge
    {
        //说明：x为起点距，v为垂线处流速，d为垂线处水深，beta为岸边系数，gamma为流速改正因子

        #region 计算第i（从2到n-1）垂线的流量
        /// <summary>
        /// 计算第i（从2到n-1）垂线的流量
        /// </summary>
        /// <param name="xn1">第n1处的地点距</param>
        /// <param name="dn1">第n1处的水深</param>
        /// <param name="vn1">第n1处的流速</param>
        /// <param name="xn2"></param>
        /// <param name="dn2"></param>
        /// <param name="vn2"></param>
        /// <returns></returns>
        public double CalculateMean_Section(float xn1, float dn1, float vn1, float xn2, float dn2, float vn2)
        {
            double dDischarge = 0;

            double dWidth = xn2 - xn1;
            double dMean_V = (vn1 + vn2) / 2;
            double dMean_Depth = (dn1 + dn2) / 2;
            dDischarge = dWidth * dMean_Depth * dMean_V;

            return dDischarge;
        }

        /// <summary>
        /// 计算第i（从2到n-1）垂线的流量
        /// </summary>
        /// <param name="xn1"></param>
        /// <param name="dn2"></param>
        /// <param name="vn2"></param>
        /// <param name="xn3"></param>
        /// <returns></returns>
        public double CalculateMid_Section(float xn1, float dn2, float vn2, float xn3)
        {
            double dDischarge = 0;

            double dWidth = (xn3 - xn1) / 2;
            dDischarge = dWidth * dn2 * vn2;

            return dDischarge;
        }
        #endregion

        #region 计算第1个垂线的流量
        /// <summary>
        /// 计算第1个垂线的流量
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="v2"></param>
        /// <param name="beta1"></param>
        /// <returns></returns>
        public double CalculateMean_Section_First(float x1, float x2, float d1, float d2, float v2, float beta1)
        {
            double dDischarge = 0;

            double dWidth = x2 - x1;
            double dMean_V = v2 * beta1;
            double dMean_Depth = (d1 + d2) / 2;

            dDischarge = dWidth * dMean_Depth * dMean_V;

            return dDischarge;
        }

        /// <summary>
        /// 计算第1个垂线的流量
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="d1"></param>
        /// <param name="v2"></param>
        /// <param name="gamma1"></param>
        /// <returns></returns>
        public double CalculateMid_Section_First(float x1, float x2, float d1, float v2, float gamma1)
        {
            double dDischarge = 0;

            double dWidth = (x2 - x1) / 2;
            dDischarge = dWidth * d1 * v2 * gamma1;

            return dDischarge;
        }
        #endregion

        #region 计算第n个垂线的流量
        /// <summary>
        /// 计算第n个垂线的流量
        /// </summary>
        /// <param name="xn1"></param>
        /// <param name="xn"></param>
        /// <param name="dn1"></param>
        /// <param name="dn"></param>
        /// <param name="vn1"></param>
        /// <param name="betan"></param>
        /// <returns></returns>
        public double CalculateMean_Section_Last(float xn1, float xn, float dn1, float dn, float vn1, float betan)
        {
            double dDischarge = 0;

            double dWidth = xn - xn1;
            double dMean_V = vn1 * betan;
            double dMean_Depth = (dn + dn1) / 2;
            dDischarge = dWidth * dMean_Depth * dMean_V;

            return dDischarge;
        }

        /// <summary>
        /// 计算第n个垂线的流量
        /// </summary>
        /// <param name="xn1"></param>
        /// <param name="xn"></param>
        /// <param name="dn"></param>
        /// <param name="vn1"></param>
        /// <param name="gamman"></param>
        /// <returns></returns>
        public double CalculateMid_Section_Last(float xn1, float xn, float dn, float vn1, float gamman)
        {
            double dDischarge = 0;

            double dWidth = (xn - xn1) / 2;
            dDischarge = dWidth * dn * vn1 * gamman;

            return dDischarge;
        }

        #endregion

        /// <summary>
        /// 计算垂线流速,流向
        /// </summary>
        /// <param name="vn">有效层的north方向流速</param>
        /// <param name="ve">有效层的earth方向流速</param>
        /// <returns></returns>
        public void GetMeanVelocity(float[] vn, float[] ve, out float fMean_V, out float fMean_Dir)
        {
            fMean_Dir = 0; fMean_V = 0;

            int iCount = vn.Count();
            float fSum_ve = 0, fSum_vn = 0;
            int iSumCount = 0;

            for (int i = 0; i < iCount; i++)
            {
                if (vn[i] < 20 && ve[i] < 20)
                {
                    fSum_ve += ve[i];
                    fSum_vn += vn[i];
                    iSumCount++;
                }
            }

            if (iSumCount > 0)
            {
                fSum_vn = fSum_vn / iSumCount;
                fSum_ve = fSum_ve / iSumCount;
            }

            fMean_V = (float)Math.Sqrt(Math.Pow(fSum_vn, 2) + Math.Pow(fSum_ve, 2));
            fMean_Dir = (float)Math.Atan2(fSum_ve, fSum_vn);

            if (fMean_Dir < 0)
                fMean_Dir = (float)(fMean_Dir / Math.PI * 180 + 360);
            else
                fMean_Dir = (float)(fMean_Dir / Math.PI * 180);
        }

        #region 计算顶部和底部流量
        /// <summary>
        /// 计算顶部底部流量
        /// </summary>
        /// <param name="src"></param>
        /// <param name="fEastVelocity"></param>
        /// <param name="fNorthVelocity"></param>
        /// <param name="dTime"></param>
        /// <param name="dBottomFlow"></param>
        /// <param name="dTopFlow"></param>
        public void CalculateTop_BottomDischarge(ArrayClass src, float fEastVelocity, float fNorthVelocity, double dTime, out double dBottomFlow, out double dTopFlow)
        {
            dTopFlow = 0;
            dBottomFlow = 0;

            //单元大小
            double Da = src.A_CellSize;
            if (Da < 1e-6) return;
            //底跟踪四个波束中的最小深度
            double Dmin = double.NaN;
            //计算四个波束的平均深度
            int num = 0;
            double depthSum = 0;
            foreach (double d in src.B_Range)
            {
                if (d < 1e-6) continue;

                if (double.IsNaN(Dmin))
                {
                    Dmin = d;
                }
                else
                {
                    if (d < Dmin)
                    {
                        Dmin = d;
                    }
                }

                num++;
                depthSum += d;
            }
            if (num == 0) return;
            double Davg = depthSum / num;

            //有效单元的可能最大深度
            double DLGmax = Dmin * Math.Cos(beamAngle / 180.0 * Math.PI) + draft - Math.Max((pulseLength + pulseLag) / 2.0, Da / 2.0);

            //水面到水底的距离
            double Dtotal = Davg + draft; //draft 为入水深度

            //第一个有效单元
            MeasuredBinInfo firstValid = new MeasuredBinInfo();
            firstValid.Status = BinFlowStatus.Bad;
            //最后一个有效单元
            MeasuredBinInfo lastValid = new MeasuredBinInfo();
            //单元深度
            double binDepth = src.A_FirstCellDepth + draft;
            //实测累计流量
            double SumMeasuredFlow = 0;
            
            List<MeasuredBinInfo> binsInfoArray = new List<MeasuredBinInfo>();

            // 保存有效单元的索引号
            List<int> validBinsIndex = new List<int>();
            for (int i = 0; binDepth < DLGmax && i < src.E_Cells && i * 4 < src.Earth.Length; i++, binDepth += Da)
            {
                //获取矢量交叉乘积
                double xi = CrossProduct(src.Earth[0, i], src.Earth[1, i], fEastVelocity, fNorthVelocity);

                MeasuredBinInfo binInfo = new MeasuredBinInfo();
                binInfo.Depth = binDepth;
                binInfo.Flow = xi * dTime * Da;
                binInfo.Status = (IsGoodBin(src, i) ? BinFlowStatus.Good : BinFlowStatus.Bad);
                binsInfoArray.Add(binInfo);

                if (binInfo.Status == BinFlowStatus.Good)
                {
                    if (firstValid.Status == BinFlowStatus.Bad)
                    {
                        firstValid = binInfo;
                    }
                    lastValid = binInfo;
                    SumMeasuredFlow += binInfo.Flow;

                    //保存一个有效单元索引
                    validBinsIndex.Add(i);
                }
            }
            //ensembleFlow.BinsInfo = binsInfoArray.ToArray();
            if (firstValid.Status == BinFlowStatus.Bad)
            {
                return;
            }

            //第一个有效单元的深度
            double Dtop = firstValid.Depth;
            //最后一个有效单元的深度
            double Dlg = lastValid.Depth;

            double Z3 = Dtotal;
            double Z2 = Dtotal - Dtop + Da / 2.0;
            double Z1 = Dtotal - Dlg - Da / 2.0;
            //顶部流量
            switch (topMode)
            {
                case TopFlowMode.Constants:

                    dTopFlow = firstValid.Flow / Da * (Z3 - Z2);
                    break;
                case TopFlowMode.PowerFunction:
                    {
                        double a = exponent + 1;
                        dTopFlow = SumMeasuredFlow * (Math.Pow(Z3, a) - Math.Pow(Z2, a)) / (Math.Pow(Z2, a) - Math.Pow(Z1, a));
                    }
                    break;
                case TopFlowMode.Slope:
                    {
                        //用于外延计算的 validbins 数量不足，则降为常数法计算
                        if (validBinsIndex.Count < 6)
                        {
                            dTopFlow = firstValid.Flow / Da * (Z3 - Z2);
                            break;
                        }
                        else
                        {
                            // validbin 的信息，三个列表是索引对应同一个 validbin
                            // 每个 validbin 的深度信息
                            List<double> binsDepth = new List<double>();
                            // 每个 validbin 的东向速度
                            List<double> binsEastVelocity = new List<double>();
                            // 每个 validbin 的北向速度
                            List<double> binsNorthVelocity = new List<double>();

                            for (int i = 0; i < 3; i++)
                            {
                                // validbin 索引号
                                int bin_index = validBinsIndex[i];
                                // validbin 深度（从水面起）
                                double bin_depth = src.A_FirstCellDepth + draft + bin_index * Da;
                                // 东向和北向速度
                                double bin_east_vel = src.Earth[0, bin_index];
                                double bin_north_vel = src.Earth[1, bin_index];

                                //加入列表
                                binsDepth.Add(bin_depth);
                                binsEastVelocity.Add(bin_east_vel);
                                binsNorthVelocity.Add(bin_north_vel);
                            }

                            // 外延计算
                            // 外延的深度位置（从水面起算）
                            double dep = (Z3 - Z2) / 2;
                            // 外延计算东向速度
                            Slope east_slope = new Slope();
                            east_slope.setXYs(binsDepth.ToArray(), binsEastVelocity.ToArray());

                            double east_vel;
                            try
                            {
                                east_vel = east_slope.calY(dep);
                            } // 外延失败，斜角为90度，转而采用常数法
                            catch (DivideByZeroException)
                            {
                                dTopFlow = firstValid.Flow / Da * (Z3 - Z2);
                                break;
                            }


                            //外延计算北向速度
                            Slope north_slope = new Slope();
                            north_slope.setXYs(binsDepth.ToArray(), binsNorthVelocity.ToArray());

                            double north_vel;
                            try
                            {
                                north_vel = north_slope.calY(dep);
                            }// 外延失败，斜角为90度，转而采用常数法
                            catch (DivideByZeroException)
                            {
                                dTopFlow = firstValid.Flow / Da * (Z3 - Z2);
                                break;
                            }
                            // 计算流量
                            dTopFlow = dTime * (Z3 - Z2) * CrossProduct(east_vel, north_vel,
                                                     fEastVelocity, fNorthVelocity);
                            break;

                        }

                    }

                default:
                    break;
            }

            //底部流量
            switch (bottomMode)
            {
                case BottomFlowMode.Constants:
                    dBottomFlow = lastValid.Flow / Da * Z1;
                    break;
                case BottomFlowMode.PowerFunction:
                    {
                        double a = exponent + 1;
                        dBottomFlow = SumMeasuredFlow * Math.Pow(Z1, a) / (Math.Pow(Z2, a) - Math.Pow(Z1, a));
                    }
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 计算矢量交叉积
        /// </summary>
        /// <param name="Ve">水流相对于船的东向流速</param>
        /// <param name="Vn">水流相对于船的北向流速</param>
        /// <param name="Vbe">船的东向速度</param>
        /// <param name="Vbn">船的北向速度</param>
        /// <returns></returns>
        protected double CrossProduct(double Ve, double Vn, double Vbe, double Vbn)
        {
            return (Vn + Vbn) * Vbe - (Ve + Vbe) * Vbn;
        }

        /// <summary>
        /// 判断是否为一个GoodBin
        /// </summary>
        /// <param name="src">参与计算数据</param>
        /// <param name="binIndex">bin的索引号</param>
        /// <returns>是否为GoodBin</returns>
        protected bool IsGoodBin(ArrayClass src, int binIndex)
        {
            try
            {
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (src.Amplitude[i, binIndex] < 0)
                        count++;
                }

                if (count > 1) return false;

                count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (src.Correlation[i, binIndex] < 0)
                        count++;
                }

                if (count > 1) return false;

                //JZH 2012-02-06 JZH   取消旧判断方法
                //if (src.BeamN[0, binIndex] < minNG3
                //    || src.BeamN[1, binIndex] < minNG3
                //    || src.BeamN[2, binIndex] < minNG3
                //    || src.BeamN[3, binIndex] < minNG4)
                //    return false;
                //if (src.Earth[0, binIndex] > 88
                //    || src.Earth[1, binIndex] > 88
                //    || src.Earth[2, binIndex] > 88
                //    || src.Earth[3, binIndex] > 88
                //    )
                //    return false;

                //JZH 2012-02-06 新判断方法
                if (src.Earth[0, binIndex] > 88
                   || src.Earth[1, binIndex] > 88
                   || src.Earth[2, binIndex] > 88
                    //    || src.Earth[3, binIndex] > 88
                   )
                    return false;
            }
            catch
            {
                return false;
            }


            return true;

        }

        #region 顶部和底部流量属性
        private double draft = 0;
        /// <summary>
        /// 换能器吃水深度(m)
        /// </summary>
        public double Draft
        {
            get { return draft; }
            set { draft = value; }
        }

        private TopFlowMode topMode = TopFlowMode.PowerFunction;
        /// <summary>
        /// 顶部流量计算方案
        /// </summary>
        public TopFlowMode TopMode
        {
            get { return topMode; }
            set { topMode = value; }
        }

        private BottomFlowMode bottomMode = BottomFlowMode.PowerFunction;
        /// <summary>
        /// 底部流量计算方案
        /// </summary>
        public BottomFlowMode BottomMode
        {
            get { return bottomMode; }
            set { bottomMode = value; }
        }

          private double exponent = 1 / 6.0;
        /// <summary>
        /// 幂函数指数
        /// </summary>
        public double Exponent
        {
            get { return exponent; }
            set { exponent = value; }
        }
        #endregion

        #region  设备属性
        protected double beamAngle = 20;
        /// <summary>
        /// 波束角(度)
        /// </summary>
        public double BeamAngle
        {
            get { return beamAngle; }
            set { beamAngle = value; }
        }

        protected double pulseLength = 0;
        /// <summary>
        /// 脉冲长度(m)
        /// </summary>
        public double PulseLength
        {
            get { return pulseLength; }
            set { pulseLength = value; }
        }
        protected double pulseLag = 0;
        /// <summary>
        /// 脉冲间隔(m)
        /// </summary>
        public double PulseLag
        {
            get { return pulseLag; }
            set { pulseLag = value; }
        }
        #endregion

        #endregion
    }
}
