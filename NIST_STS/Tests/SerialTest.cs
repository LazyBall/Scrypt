using MathNet.Numerics;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /// <summary>
    /// Тест серий.
    /// </summary>
    public class SerialTest : ITest
    {
        private const double alpha = 0.01;
        private readonly int _blockSize;

        public SerialTest(int blockSize = 3)
        {
            this._blockSize = blockSize;
        }

        public bool Run(BitArray sequence)
        {
            var (Pvalue1, Pvalue2) = ComputePvalues(this._blockSize, sequence);
            return Pvalue1 >= alpha && Pvalue2 >= alpha;
        }

        public bool Run(BitArray sequence, out double[] Pvalues)
        {
            var (Pvalue1, Pvalue2) = ComputePvalues(this._blockSize, sequence);
            Pvalues = new double[] { Pvalue1, Pvalue2 };
            return Pvalue1 >= alpha && Pvalue2 >= alpha;
        }

        private static (double Pvalue1, double Pvalue2) ComputePvalues(int m, BitArray sequence)
        {
            double psi2_m = ComputePsi2(m, sequence);
            double psi2_m1 = ComputePsi2(m - 1, sequence);
            double psi2_m2 = ComputePsi2(m - 2, sequence);
            double delta = psi2_m - psi2_m1;
            double delta2 = psi2_m - 2 * psi2_m1 + psi2_m2;
            double Pvalue1 =
                SpecialFunctions.GammaUpperRegularized(Math.Pow(2, m - 1) / 2, delta / 2);
            double Pvalue2 =
                SpecialFunctions.GammaUpperRegularized(Math.Pow(2, m - 2) / 2, delta2 / 2);
            return (Pvalue1, Pvalue2);
        }

        private static double ComputePsi2(int m, BitArray sequence)
        {
            if (m <= 0) return 0;

            int n = sequence.Length;
            int[] P = new int[1 << m]; //2^m

            for (int i = 0; i < n; i++)
            {
                int bitWord = 0;

                for (int k = 0; k < m; k++)
                {
                    bitWord <<= 1; //001 -> 010
                    if (sequence[(i + k) % n])
                    {
                        bitWord |= 1; //010 -> 011
                    }
                }

                P[bitWord]++;
            }

            double psi2 = 0;

            for (int i = 0; i < P.Length; i++)
            {
                if (P[i] > 0)
                {
                    psi2 += Math.Pow(P[i], 2);
                }
            }

            psi2 = psi2 * Math.Pow(2, m) / n - n;

            return psi2;
        }

        public bool Equals([AllowNull] ITest other)
        {
            if (other is SerialTest sTest && this._blockSize == sTest._blockSize)
                return true;
            else return false;
        }

        public override string ToString()
        {
            return this.GetType().Name + " with blockSize = " + this._blockSize;
        }
    }
}
