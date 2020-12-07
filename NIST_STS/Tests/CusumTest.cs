using System;
using System.Collections;
using MathNet.Numerics.Distributions;

namespace NIST_STS.Tests
{
    /// <summary>
    /// Тест кумулятивных сумм.
    /// </summary>
    public class CusumTest : ITest
    {
        private const double alpha = 0.01;
        private readonly bool _forwardMode;

        /// <param name="forwardMode">Режим работы теста.
        /// <list type="table">
        /// <item><c>true</c> - суммы будут считаться с начала последовательности.</item>
        /// <item><c>false</c> - суммы будут считаться с конца последовательности.</item>
        /// </list></param>
        public CusumTest(bool forwardMode = true)
        {
            this._forwardMode = forwardMode;
        }

        private int ComputeZ(BitArray sequence)
        {
            int k, end, step;
            if (this._forwardMode)
            {
                k = 0;
                end = sequence.Length;
                step = 1;
            }
            else
            {
                k = sequence.Length - 1;
                end = -1;
                step = -1;
            }

            int S, sup, inf;
            S = sup = inf = 0;

            while (k != end)
            {
                if (sequence[k])
                {
                    S++;
                    if (S > sup) sup++;
                }
                else
                {
                    S--;
                    if (S < inf) inf--;
                }
                k += step;
            }

            return (sup > -inf) ? sup : -inf;
        }

        private double ComputePvalue(BitArray sequence)
        {
            int n = sequence.Length;
            int z = ComputeZ(sequence);

            double Pvalue = 1;
            Normal distribution = new Normal();

            for (int k = (-n / z + 1) / 4; k <= (n / z - 1) / 4; k++)
            {
                Pvalue -= distribution.CumulativeDistribution((4 * k + 1) * z / Math.Sqrt(n));
                Pvalue += distribution.CumulativeDistribution((4 * k - 1) * z / Math.Sqrt(n));
            }

            for (int k = (-n / z - 3) / 4; k <= (n / z - 1) / 4; k++)
            {
                Pvalue += distribution.CumulativeDistribution((4 * k + 3) * z / Math.Sqrt(n));
                Pvalue -= distribution.CumulativeDistribution((4 * k + 1) * z / Math.Sqrt(n));
            }

            return Pvalue;
        }

        public bool Run(BitArray sequence)
        {
            return ComputePvalue(sequence) >= alpha;
        }

        public bool Run(BitArray sequence, out double[] Pvalues)
        {
            Pvalues = new double[] { ComputePvalue(sequence) };
            return Pvalues[0] >= alpha;
        }
    }
}
