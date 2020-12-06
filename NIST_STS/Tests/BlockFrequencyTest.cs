using System;
using System.Collections;
using MathNet.Numerics;

namespace NIST_STS.Tests
{
    /// <summary>
    /// Частотный блочный тест.
    /// </summary>
    class BlockFrequencyTest : ITest
    {
        private const double alpha = 0.01;
        private readonly int _blockSize;

        public BlockFrequencyTest(int blockSize = 10)
        {
            this._blockSize = blockSize;
        }

        private static double ComputePvalue(int M, BitArray sequence)
        {
            int n = sequence.Length;
            int N = n / M;
            double[] pi = new double[N];

            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < M; k++)
                {
                    if (sequence[i * M + k])
                    {
                        pi[i]++;
                    }
                }

                pi[i] /= M;
            }

            double x2stat = 0;

            for (int i = 0; i < N; i++)
            {
                x2stat += Math.Pow(pi[i] - 0.5, 2);
            }

            x2stat *= 4 * M;
            return SpecialFunctions.GammaLowerRegularized(N / 2, x2stat / 2);
        }

        public bool Run(BitArray sequence)
        {
            return ComputePvalue(this._blockSize, sequence) >= alpha;
        }

        public bool Run(BitArray sequence, out double[] Pvalues)
        {
            Pvalues = new double[] { ComputePvalue(this._blockSize, sequence) };
            return Pvalues[0] >= alpha;
        }
    }
}
