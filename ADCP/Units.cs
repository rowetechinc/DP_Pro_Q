using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADCP
{
    public class Units
    {
        private static double M2FtCoff = (1.0/0.3048);
        private static double Ft2MCoff = 0.3048;

        public double MeterToFeet(double meter,int n)
        {
            double feet = 0;
            feet =meter * Math.Pow( M2FtCoff, n);
            return feet;
        }
        public double FeetToMeter(double feet,int n )
        {
            double meter = 0;
            meter = feet * Math.Pow(Ft2MCoff, n);
            return meter;
        }
    }
}
