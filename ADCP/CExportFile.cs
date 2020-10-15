using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calcflow;

namespace ADCP
{
    public class CExportFile
    {
        public string ArrayClassWriteToFile(ArrayClass m)
        {
            string MatString = string.Empty;
            for (int i = 0; i < m.nArray; i++)
            {
                MatString += m.Name[i];

                MatString += "\r\n";
                switch (m.Name[i])
                {
                    case "E000001\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.Velocity[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }
                    case "E000002\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.Instrument[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }
                    case "E000003\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.Earth[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }
                    case "E000004\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.Amplitude[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }
                    case "E000005\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.Correlation[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }
                    case "E000006\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.BeamN[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }

                    case "E000007\0":
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    MatString += m.XfrmN[beam, bin].ToString("0.0000").PadLeft(12, ' ');
                                }
                                MatString += "\r\n";
                            }
                            break;
                        }
                    case "E000008\0":
                        {
                            MatString += m.E_EnsembleNumber.ToString().PadLeft(12, ' ');
                            MatString += m.E_Cells.ToString().PadLeft(12, ' ');
                            MatString += m.E_Beams.ToString().PadLeft(12, ' ');
                            MatString += m.E_PingsInEnsemble.ToString().PadLeft(12, ' ') + "\r\n";
                            MatString += m.E_PingCount.ToString().PadLeft(12, ' ');
                            MatString += m.E_Status.ToString().PadLeft(12, ' ');
                            //////////////////2010-01-14添加的改动//////////////////
                            //if (radioButtonInsTime.Checked)  //LPJ 2013-6-21 待修改？？？？？？？？？？
                            //{
                            //    MatString += m.YYYY.ToString().PadLeft(12, ' ');
                            //    MatString += m.MM.ToString().PadLeft(12, ' ') + "\r\n";
                            //    MatString += m.DD.ToString().PadLeft(12, ' ');
                            //    MatString += m.HH.ToString().PadLeft(12, ' ');
                            //    MatString += m.mm.ToString().PadLeft(12, ' ');
                            //    MatString += m.SS.ToString().PadLeft(12, ' ') + "\r\n";
                            //    MatString += m.hsec.ToString().PadLeft(12, ' ') + "\r\n";
                            //}
                            //else if (radioButtonGPSTime.Checked)
                            //{
                            //    MatString += m.YYYY.ToString().PadLeft(12, ' ');
                            //    MatString += m.MM.ToString().PadLeft(12, ' ') + "\r\n";
                            //    MatString += m.DD.ToString().PadLeft(12, ' ');
                            //    MatString += gpsTime.Substring(0, 2).PadLeft(12, ' ');
                            //    MatString += gpsTime.Substring(2, 2).PadLeft(12, ' ');
                            //    MatString += gpsTime.Substring(4, 2).PadLeft(12, ' ') + "\r\n";
                            //    MatString += m.hsec.ToString().PadLeft(12, ' ') + "\r\n";
                            //}
                            //else
                            //if (bUsePCTime) //LPJ 2013-6-25 当用户设置使用PC时间
                            //{
                            //    DateTime DT = System.DateTime.Now;

                            //    MatString += DT.Year.ToString().PadLeft(12, ' ');
                            //    MatString += DT.Month.ToString().PadLeft(12, ' ') + "\r\n";
                            //    MatString += DT.Day.ToString().PadLeft(12, ' ');
                            //    MatString += DT.Hour.ToString().PadLeft(12, ' ');
                            //    MatString += DT.Minute.ToString().PadLeft(12, ' ');
                            //    MatString += DT.Second.ToString().PadLeft(12, ' ') + "\r\n";
                            //    MatString += (DT.Millisecond / 10).ToString().PadLeft(12, ' ') + "\r\n";
                            //}
                            //else //LPJ 2013-6-25
                            {
                                MatString += m.YYYY.ToString().PadLeft(12, ' ');
                                MatString += m.MM.ToString().PadLeft(12, ' ') + "\r\n";
                                MatString += m.DD.ToString().PadLeft(12, ' ');
                                MatString += m.HH.ToString().PadLeft(12, ' ');
                                MatString += m.mm.ToString().PadLeft(12, ' ');
                                MatString += m.SS.ToString().PadLeft(12, ' ') + "\r\n";
                                MatString += m.hsec.ToString().PadLeft(12, ' ') + "\r\n";
                            }
                            ///////////////////////////////////////////////////////
                            break;
                        }
                    case "E000009\0":
                        {
                            MatString += m.A_FirstCellDepth.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_CellSize.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_FirstPingSeconds.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_LastPingSeconds.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            MatString += m.A_Heading.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_Pitch.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_Roll.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_WaterTemperature.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            MatString += m.A_BoardTemperature.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_Salinity.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_Pressure.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.A_Depth.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            MatString += m.A_SpeedOfSound.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            break;
                        }
                    case "E000010\0":
                        {
                            MatString += m.B_FirstPingSeconds.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_LastPingSeconds.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_Heading.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_Pitch.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            MatString += m.B_Roll.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_WaterTemperature.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_BoardTemperature.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_Salinity.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            MatString += m.B_Pressure.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_Depth.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_SpeedOfSound.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_Status.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            MatString += m.B_Beams.ToString("0.0000").PadLeft(12, ' ');
                            MatString += m.B_PingCount.ToString("0.0000").PadLeft(12, ' ') + "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_Range[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_SNR[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_Amplitude[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_Correlation[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_Velocity[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_BeamN[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_Instrument[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_XfrmN[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_Earth[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            for (int j = 0; j < m.B_Beams; j++)
                            {
                                MatString += m.B_EarthN[j].ToString("0.0000").PadLeft(12, ' ');
                            }
                            MatString += "\r\n";
                            break;
                        }
                    case "E000011\0": //LPJ 2014-4-23
                        {
                            MatString += System.Text.Encoding.Default.GetString(m.NMEA_Buffer) + "\r\n";
                            break;
                        }
                }
            }
            return MatString;
        }

    }
}
