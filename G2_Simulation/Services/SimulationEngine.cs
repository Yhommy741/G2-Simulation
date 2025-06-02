using G2_Simulation.Models;
using System;
using System.Windows;
using System.Windows.Media;

namespace G2_Simulation.Services
{
    public class SimulationEngine
    {
        public double CalculateAngle(Parameters p)
        {
            const double g = 9.81;
            double uY = -Math.Sqrt(2 * g * (p.HeightDrop - p.HeightCollision));
            double wI = (Math.Sqrt(p.KSpring * p.NSpring * 0.075 * 0.075 / p.MomentOfInertia))* 2.07;

            double A = p.MomentOfInertia * (1 + p.CoefficientOfRestitution);
            double B = p.CoefficientOfRestitution * p.MomentOfInertia / p.CollisionRadius - p.BallMass * p.CollisionRadius;
            double C = p.MomentOfInertia / p.CollisionRadius - p.BallMass * p.CollisionRadius;

            Func<double, double> equation = thetaDeg =>
                CalculateRange(thetaDeg, A, B, C, uY, wI, p.HeightCollision, p.R) - p.R / 100;

            double lower = 0.0, upper = 90.0, solution = double.NaN;
            double x0 = 30.0, x1 = 45.0;
            for (int i = 0; i < 100; i++)
            {
                double fx0 = equation(x0);
                double fx1 = equation(x1);
                if (Math.Abs(fx1) < 1e-6)
                {
                    solution = x1;
                    break;
                }
                double xNext = x1 - fx1 * (x1 - x0) / (fx1 - fx0);
                xNext = Math.Max(lower, Math.Min(upper, xNext));
                x0 = x1;
                x1 = xNext;
            }
            if (double.IsNaN(solution))
            {
                double minError = double.MaxValue;
                for (double theta = lower; theta <= upper; theta += 0.1)
                {
                    double error = Math.Abs(equation(theta));
                    if (error < minError)
                    {
                        minError = error;
                        solution = theta;
                    }
                }
            }
            return 90 - solution;
        }

        private double CalculateRange(double thetaDeg, double A, double B, double C, double uY, double wI, double heightCollision, double R)
        {
            double thetaRad = thetaDeg * Math.PI / 180;
            double term1 = (A * wI - B * uY * Math.Cos(thetaRad)) / C;
            double term2 = Math.Abs(uY) * Math.Sin(thetaRad);
            double vX = term1 * Math.Sin(thetaRad) + term2 * Math.Cos(thetaRad);
            double vY = term1 * Math.Cos(thetaRad) - term2 * Math.Sin(thetaRad);
            double discriminant = vY * vY + 2 * 9.81 * heightCollision;
            if (discriminant < 0) return double.NaN;
            double flightTime = (vY + Math.Sqrt(discriminant)) / 9.81;
            return (vX * flightTime);
        }

        public double CalculateDelayTime(Parameters p)
        {
            return -16*p.HeightDrop*p.HeightDrop + 68*p.HeightDrop -32;
        }

        public double CalculateYaw(Parameters p)
        {
            return -(90 - p.ThetaDeg);
        }

        public PointCollection CalculateTrajectory(Parameters p, double thetaBDeg)
        {
            var newPoints = new PointCollection();
            double thetaRad = thetaBDeg * Math.PI / 180;
            const double g = 9.81;
            double uY = -Math.Sqrt(2 * g * (p.HeightDrop - p.HeightCollision));
            double wI = (Math.Sqrt(p.KSpring * p.NSpring * 0.075 * 0.075 / p.MomentOfInertia)) * 2.07;
            double A = p.MomentOfInertia * (1 + p.CoefficientOfRestitution);
            double B = p.CoefficientOfRestitution * p.MomentOfInertia / p.CollisionRadius - p.BallMass * p.CollisionRadius;
            double C = p.MomentOfInertia / p.CollisionRadius - p.BallMass * p.CollisionRadius;
            double term1 = (A * wI - B * uY * Math.Cos(thetaRad)) / C;
            double term2 = Math.Abs(uY) * Math.Sin(thetaRad);
            double vX = term1 * Math.Sin(thetaRad) + term2 * Math.Cos(thetaRad);
            double vY = term1 * Math.Cos(thetaRad) - term2 * Math.Sin(thetaRad);
            double tFlight = (vY + Math.Sqrt(vY * vY + 2 * g * p.HeightCollision)) / g;
            const double xScale = 130, yScale = 130, xOffset = 0, yBase = 300;
            for (int i = 0; i <= 100; i++)
            {
                double t = tFlight * i / 100;
                double x = (vX * t);
                double y = p.HeightCollision + vY * t - 0.5 * g * t * t;
                newPoints.Add(new Point(x * xScale + xOffset, yBase - y * yScale));
            }
            return newPoints;
        }
    }
}
