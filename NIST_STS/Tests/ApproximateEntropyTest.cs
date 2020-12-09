using MathNet.Numerics;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /// <summary>
    /// Тест приближенной энтропии.
    /// </summary>
    public class ApproximateEntropyTest : ITest
    {
        private const double alpha = 0.01;
        private readonly int _blockSize;

        public ApproximateEntropyTest(int blockSize = 3)
        {
            this._blockSize = blockSize;
        }

        private double ComputePvalue(BitArray sequence)
        {
            int m = this._blockSize;
            int n = sequence.Length;
            double ApEn = ComputePhi(m, sequence) - ComputePhi(m + 1, sequence);
            double x2stat = 2 * n * (Math.Log(2) - ApEn);
            double Pvalue = SpecialFunctions.GammaUpperRegularized(Math.Pow(2, m - 1), x2stat / 2);
            return Pvalue;
        }

        private static double ComputePhi(int m, BitArray sequence)
        {
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

            double phi = 0;

            for (int i = 0; i < P.Length; i++)
            {
                if (P[i] > 0)
                {
                    phi += P[i] * Math.Log(P[i] / (double)n);
                }
            }

            phi /= n;

            return phi;
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

        public bool Equals([AllowNull] ITest other)
        {
            if (other is ApproximateEntropyTest apTest && this._blockSize == apTest._blockSize)
                return true;
            else return false;
        }

        public override string ToString()
        {
            return this.GetType().Name + " with blockSize = " + this._blockSize;
        }
    }
}
