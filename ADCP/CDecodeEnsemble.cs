using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Calcflow;

namespace ADCP
{
    public partial class CDecodeEnsemble
    {
        #region 参数
        public ArrayClass Arr = new ArrayClass();
        private static int HDRLEN = 0;
        private static int MaxArray = 11;
        private int PacketPointer = 0;

        private TestUnion ByteArrayToNumber;

        [StructLayout(LayoutKind.Explicit)]
        private struct TestUnion
        {
            [FieldOffset(0)]
            public byte A;
            [FieldOffset(1)]
            public byte B;
            [FieldOffset(2)]
            public byte C;
            [FieldOffset(3)]
            public byte D;
            [FieldOffset(0)]
            public float Float;
            [FieldOffset(0)]
            public int Int;
        }

        private string VelocityID = "E000001\0";
        private string InstrumentID = "E000002\0";
        private string EarthID = "E000003\0";
        private string AmplitudeID = "E000004\0";
        private string CorrelationID = "E000005\0";
        private string BeamNID = "E000006\0";
        private string XfrmNID = "E000007\0";
        private string EnsembleDataID = "E000008\0";
        private string AncillaryID = "E000009\0";
        private string BottomTrackID = "E000010\0";
        private string NMEAID = "E000011\0";

        private string RiverBTID = "R000001\0";
        private string RiverTimeStampID = "R000002\0";
        private string RiverNMEAID = "R000003\0";
        private string RiverBThump = "R000004\0";
        private string RiverStationID = "R000005\0";
        private string RiverTransectID = "R000006\0";


        #endregion


        public CDecodeEnsemble(byte[] packet, int PacketSize)
        {
            DecodeEnsemble(packet, Arr, PacketSize);
        }

        private byte ByteArrayToByte(byte[] packet)
        {
            return (packet[PacketPointer++]);
        }
        private int ByteArrayToInt(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            return ByteArrayToNumber.Int;
        }
        private string ByteArrayToString(byte[] packet, int len)
        {
            string s = "";
            int i;
            for (i = 0; i < len; i++)
            {
                s += (char)packet[PacketPointer++];
            }
            return s;
        }
        private float ByteArrayToFloat(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            return ByteArrayToNumber.Float;
        }

        private void DecodeEnsemble(byte[] packet, ArrayClass m, int PacketSize)
        {
            int SizeCount = 0;
            int ArrayCount = 0;

            m.VelocityAvailable = false;
            m.InstrumentAvailable = false;
            m.EarthAvailable = false;
            m.AmplitudeAvailable = false;
            m.CorrelationAvailable = false;
            m.BeamNAvailable = false;
            m.XfrmNAvailable = false;
            m.EnsembleDataAvailable = false;
            m.AncillaryAvailable = false;
            m.BottomTrackAvailable = false;
            m.NmeaAvailable = false;
            m.TransectStateAvailable = false;

            int i = 0;
            PacketPointer = HDRLEN;
            for (; i < MaxArray; i++)
            {
                m.Type[i] = ByteArrayToInt(packet);
                m.Bins[i] = ByteArrayToInt(packet);
                m.Beams[i] = ByteArrayToInt(packet);
                m.Imag[i] = ByteArrayToInt(packet);
                m.NameLen[i] = ByteArrayToInt(packet);
                m.Name[i] = ByteArrayToString(packet, 8);

                ArrayCount = m.Bins[i] * m.Beams[i];

                switch (m.Type[i])
                {
                    default:
                        break;
                    case 50:
                        break;
                    case 0:
                        ArrayCount *= 8;
                        break;
                    case 10:
                    case 20:
                        ArrayCount *= 4;
                        break;
                    case 30:
                    case 40:
                        ArrayCount *= 2;
                        break;
                }

                SizeCount = PacketPointer;

                if (VelocityID.Equals(m.Name[i], StringComparison.Ordinal))
                {
                    m.VelocityAvailable = true;
                    for (int beam = 0; beam < m.Beams[i]; beam++)
                    {
                        for (int bin = 0; bin < m.Bins[i]; bin++)
                        {
                            m.Velocity[beam, bin] = ByteArrayToFloat(packet);
                        }
                    }
                }
                else
                {
                    if (InstrumentID.Equals(m.Name[i], StringComparison.Ordinal))
                    {
                        m.InstrumentAvailable = true;

                        for (int beam = 0; beam < m.Beams[i]; beam++)
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                m.Instrument[beam, bin] = ByteArrayToFloat(packet);
                            }
                        }
                    }
                    else
                    {
                        if (EarthID.Equals(m.Name[i], StringComparison.Ordinal))
                        {
                            m.EarthAvailable = true;
                            for (int beam = 0; beam < m.Beams[i]; beam++)
                            {
                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                {
                                    m.Earth[beam, bin] = ByteArrayToFloat(packet);
                                }
                            }
                        }
                        else
                        {
                            if (AmplitudeID.Equals(m.Name[i], StringComparison.Ordinal))
                            {
                                m.AmplitudeAvailable = true;
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    for (int bin = 0; bin < m.Bins[i]; bin++)
                                    {
                                        m.Amplitude[beam, bin] = ByteArrayToFloat(packet);
                                    }
                                }
                            }
                            else
                            {
                                if (CorrelationID.Equals(m.Name[i], StringComparison.Ordinal))
                                {
                                    m.CorrelationAvailable = true;
                                    for (int beam = 0; beam < m.Beams[i]; beam++)
                                    {
                                        for (int bin = 0; bin < m.Bins[i]; bin++)
                                        {
                                            m.Correlation[beam, bin] = ByteArrayToFloat(packet);
                                        }
                                    }
                                }
                                else
                                {
                                    if (BeamNID.Equals(m.Name[i], StringComparison.Ordinal))
                                    {
                                        m.BeamNAvailable = true;
                                        for (int beam = 0; beam < m.Beams[i]; beam++)
                                        {
                                            for (int bin = 0; bin < m.Bins[i]; bin++)
                                            {
                                                m.BeamN[beam, bin] = ByteArrayToInt(packet);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (XfrmNID.Equals(m.Name[i], StringComparison.Ordinal))
                                        {
                                            m.XfrmNAvailable = true;
                                            for (int beam = 0; beam < m.Beams[i]; beam++)
                                            {
                                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                                {
                                                    m.XfrmN[beam, bin] = ByteArrayToInt(packet);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (EnsembleDataID.Equals(m.Name[i], StringComparison.Ordinal))
                                            {
                                                m.EnsembleDataAvailable = true;
                                                m.E_EnsembleNumber = ByteArrayToInt(packet);
                                                m.E_Cells = ByteArrayToInt(packet);
                                                m.E_Beams = ByteArrayToInt(packet);
                                                m.E_PingsInEnsemble = ByteArrayToInt(packet);
                                                m.E_PingCount = ByteArrayToInt(packet);
                                                m.E_Status = ByteArrayToInt(packet);
                                                ////////////////2010-01-14添加的改动////////////////
                                                m.YYYY = ByteArrayToInt(packet);
                                                m.MM = ByteArrayToInt(packet);
                                                m.DD = ByteArrayToInt(packet);
                                                m.HH = ByteArrayToInt(packet);
                                                m.mm = ByteArrayToInt(packet);
                                                m.SS = ByteArrayToInt(packet);
                                                m.hsec = ByteArrayToInt(packet);
                                                ///////////////////////////////////////////////////
                                            }
                                            else
                                            {
                                                if (AncillaryID.Equals(m.Name[i], StringComparison.Ordinal))
                                                {
                                                    m.AncillaryAvailable = true;
                                                    m.A_FirstCellDepth = ByteArrayToFloat(packet);
                                                    m.A_CellSize = ByteArrayToFloat(packet);
                                                    m.A_FirstPingSeconds = ByteArrayToFloat(packet);
                                                    m.A_LastPingSeconds = ByteArrayToFloat(packet);

                                                    m.A_Heading = ByteArrayToFloat(packet);
                                                    m.A_Pitch = ByteArrayToFloat(packet);
                                                    m.A_Roll = ByteArrayToFloat(packet);
                                                    m.A_WaterTemperature = ByteArrayToFloat(packet);
                                                    m.A_BoardTemperature = ByteArrayToFloat(packet);
                                                    m.A_Salinity = ByteArrayToFloat(packet);
                                                    m.A_Pressure = ByteArrayToFloat(packet);
                                                    m.A_Depth = ByteArrayToFloat(packet);
                                                    m.A_SpeedOfSound = ByteArrayToFloat(packet);
                                                }
                                                else
                                                {
                                                    if (BottomTrackID.Equals(m.Name[i], StringComparison.Ordinal))
                                                    {
                                                        m.BottomTrackAvailable = true;
                                                        m.B_FirstPingSeconds = ByteArrayToFloat(packet);
                                                        m.B_LastPingSeconds = ByteArrayToFloat(packet);
                                                        m.B_Heading = ByteArrayToFloat(packet);
                                                        m.B_Pitch = ByteArrayToFloat(packet);
                                                        m.B_Roll = ByteArrayToFloat(packet);
                                                        m.B_WaterTemperature = ByteArrayToFloat(packet);
                                                        m.B_BoardTemperature = ByteArrayToFloat(packet);
                                                        m.B_Salinity = ByteArrayToFloat(packet);
                                                        m.B_Pressure = ByteArrayToFloat(packet);
                                                        m.B_Depth = ByteArrayToFloat(packet);
                                                        m.B_SpeedOfSound = ByteArrayToFloat(packet);
                                                        m.B_Status = ByteArrayToFloat(packet);
                                                        m.B_Beams = ByteArrayToFloat(packet);
                                                        m.B_PingCount = ByteArrayToFloat(packet);

                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Range[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_SNR[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Amplitude[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Correlation[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Velocity[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_BeamN[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Instrument[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_XfrmN[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Earth[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_EarthN[beam] = ByteArrayToFloat(packet);
                                                    }
                                                    else
                                                    {
                                                        if (NMEAID.Equals(m.Name[i], StringComparison.Ordinal))
                                                        {
                                                            m.NmeaAvailable = true;
                                                            int j = 0;
                                                            while (packet[PacketPointer] != 0)
                                                            {
                                                                //m.NMEA_Buffer[j++] = packet[PacketPointer];
                                                                m.NMEA_Buffer[j++] = packet[PacketPointer++]; //LPJ 2013-7-31
                                                                if (j >= 8192)
                                                                    break;
                                                            }
                                                            for (int end = j; end < 8192; end++)
                                                            {
                                                                m.NMEA_Buffer[end] = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (RiverTransectID.Equals(m.Name[i], StringComparison.Ordinal))
                                                            {
                                                                m.TransectStateAvailable = true;
                                                                m.TransectState = ByteArrayToFloat(packet);
                                                                m.TransectNumber = ByteArrayToFloat(packet);
                                                                m.TransectStatus = ByteArrayToFloat(packet);
                                                                m.BottomStatus = ByteArrayToFloat(packet);
                                                                m.ProfileStatus = ByteArrayToFloat(packet);
                                                                m.MovingEnsembles = ByteArrayToFloat(packet);
                                                                m.MovingBTEnsembles = ByteArrayToFloat(packet);
                                                                m.MovingWPEnsembles = ByteArrayToFloat(packet);
                                                                m.CurrentEdge = ByteArrayToFloat(packet);

                                                                m.EdgeType[0] = ByteArrayToFloat(packet);
                                                                m.EdgeDistance[0] = ByteArrayToFloat(packet);
                                                                m.EdgeEnsembles[0] = ByteArrayToFloat(packet);
                                                                m.EdgeStatus[0] = ByteArrayToFloat(packet);

                                                                m.EdgeType[1] = ByteArrayToFloat(packet);
                                                                m.EdgeDistance[1] = ByteArrayToFloat(packet);
                                                                m.EdgeEnsembles[1] = ByteArrayToFloat(packet);
                                                                m.EdgeStatus[1] = ByteArrayToFloat(packet);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                /*
                SizeCount = (PacketPointer - SizeCount) / 4;
                if (SizeCount != ArrayCount)
                {
                    PacketPointer += 4 * (ArrayCount - SizeCount);
                }
                
                if (PacketPointer + 4 >= PacketSize)
                    break;
                */
                //correct for changes in array length
                SizeCount = PacketPointer - SizeCount;

                if (SizeCount != ArrayCount)
                {
                    PacketPointer += (ArrayCount - SizeCount);
                    if (PacketPointer < 0)
                        PacketPointer = 2 * PacketSize;
                }

                if (PacketPointer + 4 >= PacketSize)
                    break;//no more data
            }
            m.nArray = i + 1;
            if (i >= 11)
            {
                m.nArray = 11;
            }
            if (m.E_Cells < 1)
                m.E_Cells = 1;
            PacketPointer = 0;
        }

    }
}
