using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2_Simulation.Core
{
    public static class Coordinate
    {
        public static (double R, double ThetaDeg, double ThetaRad) ToPolar(double x, double y)
        {
            double r = Math.Sqrt(x * x + y * y);
            double thetaRad = Math.Atan2(y, x);
            double thetaDeg = thetaRad * 180 / Math.PI;
            return (r, thetaDeg, thetaRad);
        }

        public static (double X, double Y) ToCartesian(double r, double thetaRad)
        {
            double x = r * Math.Cos(thetaRad);
            double y = r * Math.Sin(thetaRad);
            return (x, y);
        }
    }
}
