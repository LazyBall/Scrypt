using MathNet.Numerics;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /*
     * Частотный блочный тест
     * Суть теста — определение доли единиц внутри блока длиной m бит. Цель — выяснить действительно
     * ли частота повторения единиц в блоке длиной m бит приблизительно равна m/2, как можно было бы
     * предположить в случае абсолютно случайной последовательности. Вычисленное в ходе теста значение
     * вероятности p должно быть не меньше 0,01. В противном случае (p < 0,01) двоичная
     * последовательность не носит истинно случайный характер. Если принять m = 1, данный тест
     * переходит в частотный побитовый тест. 
     * 
     * Статистика теста c(S): Мера  согласования  наблюдаемого  количества  единиц внутри блока
     * с теоретически ожидаемыми.
     * 
     * Выявляемый дефект: Локализованные  отклонения  частоты появления единиц в блоке от
     * идеального значения Ѕ.
     * 
     * When s is an integer, Q ( s , λ ) (igamc) is the cumulative distribution function 
     * (плотность распределения) for Poisson random variables.
     */

    /// <summary>
    /// Частотный блочный тест.
    /// </summary>
    public class BlockFrequencyTest : ITest
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
            return SpecialFunctions.GammaUpperRegularized(N / 2.0, x2stat / 2);
        }

        public bool Equals([AllowNull] ITest other)
        {
            if (other is BlockFrequencyTest blTest && this._blockSize == blTest._blockSize)
                return true;
            else return false;
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

        public override string ToString()
        {
            return this.GetType().Name + " with blockSize = " + this._blockSize;
        }
    }
}
