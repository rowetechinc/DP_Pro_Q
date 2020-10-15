
/*该类用于计算GPS安装时与仪器的偏角值
 * 传入的参数为：
 * RTIdata，采集的原始数据
 * fBoatVx_GPS，数据采集时的GPS船速的X坐标分量
 * fBoatVy_GPS，数据采集时的GPS船速的Y坐标分量
 * 
 * 结果值为：
 * dHeadingOffset，GPS安装于仪器的偏角值，
 * dHeadingOffset=底跟踪的直线方向-GPS直线方向的值
 * 
 * 直线方向的计算方法为：
 * Course_MG=arctan(Vx/Vy)
 * Vx=sum(1/2*(Vx[i-1]+Vx[i])*Det_T)
 * Vy=sum(1/2*(Vy[i-1]+Vy[i])*Det_T)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calcflow;

namespace ADCP
{
    public class CGetHeadingOffset
    {
        public CGetHeadingOffset(List<ArrayClass> RTIdata, List<float> fBoatVx_GPS,List<float> fBoatVy_GPS)
        {
            try
            {
                dHeadingOffset = GetHeadingOffset(RTIdata, fBoatVx_GPS,fBoatVy_GPS);
            }
            catch
            {
                dHeadingOffset = 0;
            }
        }
        public double dHeadingOffset;
        public double dCourseMG_Bottom;
        public double dCourseMG_GPS;
        public double dDMG_Bottom;
        public double dDMG_GPS;

        /*计算GPS安装时与仪器的偏角值
         * 传入参数：
         * RTIdata，采集的原始数据
         * fBoatVx_GPS，数据采集时的GPS船速的X坐标分量
         * fBoatVy_GPS，数据采集时的GPS船速的Y坐标分量
         * 
         * 返回：dHeadingOffset，GPS安装时与仪器的偏角，单位为度
         */
        private double GetHeadingOffset(List<ArrayClass> RTIdata, List<float> fBoatVx_GPS, List<float> fBoatVy_GPS)
        {
            double dHeadingOffset = 0;

            //double dCourseMG_Bottom = 0;
            //double dCourseMG_GPS = 0;

            List<Velocity> fBoatV_Bottom=new List<Velocity>();

            for(int i=0;i<RTIdata.Count();i++)
            {
                Velocity BoatV = new Velocity();
                BoatV.VX = RTIdata[i].B_Earth[0];
                BoatV.VY = RTIdata[i].B_Earth[1];

                fBoatV_Bottom.Add(BoatV);
            }

            List<Velocity> fBoatV_GPS = new List<Velocity>();
            for (int i = 0; i < fBoatVx_GPS.Count; i++)
            {
                Velocity BoatV = new Velocity();
                BoatV.VX = fBoatVx_GPS[i];
                BoatV.VY = fBoatVy_GPS[i];

                fBoatV_GPS.Add(BoatV);
            }
 
            //dCourseMG_Bottom = GetCourseMG(RTIdata, fBoatV_Bottom);
            //dCourseMG_GPS = GetCourseMG(RTIdata, fBoatV_GPS);

            GetCourseMG(RTIdata, fBoatV_Bottom, ref dCourseMG_Bottom, ref dDMG_Bottom);
            GetCourseMG(RTIdata, fBoatV_GPS, ref dCourseMG_GPS, ref dDMG_GPS);

            dHeadingOffset = dCourseMG_Bottom - dCourseMG_GPS;

            return dHeadingOffset;
        }

        /*计算直线方向
         * 输入：RTIdata，仪器采集数据
         * fBoatV，船速坐标
         * 
         * 返回：dCourseMGDir，直线方向
         * */
        private void GetCourseMG(List<ArrayClass> RTIdata, List<Velocity> fBoatV, ref double dCourseMG,ref double dDMG)
        {
            //double dCourseMG = 0;
            bool bFirstGoodEnsemble = false;
            int iPrevGoodEnsemble = 0;
            double dLastGoodEnsembleTime = 0;
            float fAccEast = 0, fAccNorth = 0;

            double dCourseMGDir = 0;

            for (int i = 0; i < RTIdata.Count; i++)
            {
                if (i == 0)  //第一个数据
                {
                    Velocity BoatVelocity = new Velocity(); //LPJ 2013-7-9
                    BoatVelocity = (Velocity)(fBoatV[i]); //LPJ 2013-8-16

                    if (BoatVelocity.VX > 20 || BoatVelocity.VY > 20)
                    {
                        bFirstGoodEnsemble = false;
                    }
                    else  //JZH 2012-04-09 当第一个数据组有效时，初始化流量参数
                    {
                        dLastGoodEnsembleTime = RTIdata[i].A_FirstPingSeconds;
                        iPrevGoodEnsemble = i;
                        bFirstGoodEnsemble = true;
                    }
                }
                else
                {
                    Velocity BoatVelocity = new Velocity(); //LPJ 2013-7-9
                    Velocity preBoatVelocity = new Velocity();

                    BoatVelocity = (Velocity)fBoatV[i]; //LPJ 2013-8-16
                    preBoatVelocity = (Velocity)fBoatV[iPrevGoodEnsemble];  //LPJ 2013-8-16
                   

                    if (BoatVelocity.VX < 20 && BoatVelocity.VY < 20)
                    {
                        if (bFirstGoodEnsemble == true)
                        {
                            float fEast = 0.5f * (preBoatVelocity.VX + BoatVelocity.VX) * (float)(RTIdata[i].A_FirstPingSeconds - dLastGoodEnsembleTime);
                            float fNorth = 0.5f * (preBoatVelocity.VY + BoatVelocity.VY) * (float)(RTIdata[i].A_FirstPingSeconds - dLastGoodEnsembleTime);
                            fAccEast += fEast;
                            fAccNorth += fNorth;

                            dCourseMG = (float)Math.Atan2(fAccEast, fAccNorth);
                            dDMG = (float)Math.Sqrt(fAccEast * fAccEast + fAccNorth * fAccNorth);

                            iPrevGoodEnsemble = i;

                            dLastGoodEnsembleTime = RTIdata[i].A_FirstPingSeconds;
                        }
                        else
                        {
                            bFirstGoodEnsemble = true;
                            iPrevGoodEnsemble = i;
                            dLastGoodEnsembleTime = RTIdata[i].A_FirstPingSeconds;
                        }
                    }
                } 
            }

            if (dCourseMG < 0)
            {
                dCourseMGDir = dCourseMG / Math.PI * 180 + 360;
                dCourseMG = dCourseMG / Math.PI * 180 + 360;
            }
            else
            {
                dCourseMGDir = dCourseMG / Math.PI * 180;
                dCourseMG = dCourseMG / Math.PI * 180;
            }

            //return dCourseMGDir;
        }

        private struct Velocity
        {
            public float VX;
            public float VY;
            public float VZ;
        }
    }
}
