using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace OpenDACT.Class_Files
{
    class PolynomialRegression
    {
        private double[] regressionLine;
        private double[] x;
        private double[] y;
        private int order;

        PolynomialRegression(int order)
        {
            this.order = order;
        }

        public void AddPoints(double[] x, double[] y)
        {
            int lengthX = this.x.Length + x.Length;
            int lengthY = this.y.Length + y.Length;

            this.x = this.x.Concat(x).ToArray();
            this.y = this.y.Concat(y).ToArray();
        }

        public double PerformRegression(double desiredY)
        {
            regressionLine = Fit.Polynomial(x, y, order);

            return Evaluate.Polynomial(desiredY, regressionLine);
        }
    }
}
