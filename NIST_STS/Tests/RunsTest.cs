using MathNet.Numerics;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /// <summary>
    /// Тест "дырок".
    /// </summary>
    public class RunsTest : ITest
    {
        private const double alpha = 0.01;

        private static double ComputePvalue(BitArray sequence)
        {
            int n = sequence.Length;
            double pi = 0;

            foreach (bool bit in sequence)
            {
                if (bit)
                {
                    pi++;
                }
            }

            pi /= n;
            double Pvalue = 0;

            if (Math.Abs(pi - 0.5) <= 2 / Math.Sqrt(n))
            {
                int Vn = 1;

                for (int k = 1; k < n; k++)
                {
                    if (sequence[k] != sequence[k - 1])
                    {
                        Vn++;
                    }
                }

                Pvalue = SpecialFunctions.Erfc(Math.Abs(Vn - 2 * n * (1 - pi) * pi) /
                    (2 * Math.Sqrt(2 * n) * (1 - pi) * pi));
            }

            return Pvalue;
        }

        public bool Equals([AllowNull] ITest other)
        {
            if (other is RunsTest) return true;
            else return false;
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

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
