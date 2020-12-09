using MathNet.Numerics;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /// <summary>
    /// Частотный тест.
    /// </summary>
    /// <remarks>
    /// Рекомендуемая длина для тестируемой последовательности - не менее 100 бит.
    /// </remarks>
    public class FrequencyTest : ITest
    {
        private const double alpha = 0.01;

        private static double ComputePvalue(BitArray sequence)
        {
            int Sn = 0;

            foreach (bool bit in sequence)
            {
                Sn += bit ? 1 : (-1);
            }

            double Sobs = Math.Abs(Sn) / Math.Sqrt(sequence.Length);
            double Pvalue = SpecialFunctions.Erfc(Sobs / Math.Sqrt(2));
            return Pvalue;
        }

        public bool Equals([AllowNull] ITest other)
        {
            if (other is FrequencyTest) return true;
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
