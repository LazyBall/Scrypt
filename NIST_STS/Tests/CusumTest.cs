using MathNet.Numerics.Distributions;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /*
     * Тест кумулятивных сумм
     * Тест заключается в максимальном отклонении (от нуля) при произвольном обходе, определяемым
     * кумулятивной суммой заданных (-1, +1) цифр в последовательности. Цель данного теста — определить
     * является ли кумулятивная сумма частичных последовательностей, возникающих во входной
     * последовательности, слишком большой или слишком маленькой по сравнению с ожидаемым поведением
     * такой суммы для абсолютно случайной входной последовательности. Таким образом, кумулятивная
     * сумма может рассматриваться как произвольный обход. Для случайной последовательности отклонения
     * от произвольного обхода должны быть вблизи нуля. Для некоторых типов последовательностей, не
     * являющихся в полной мере случайными подобные отклонения от нуля при произвольном обходе будут
     * достаточно существенными. Если вычисленное в ходе теста значение вероятности p < 0,01, то
     * входная двоичная последовательность не является абсолютно случайной. В противном случае она
     * носит случайный характер.
     * 
     * Статистика теста c(S): Максимальное  отклонение  значения  накопленной суммы элементов
     * последовательности от начальной точки отсчета (точка 0).
     * 
     * Выявляемый дефект: Большое  значение  единиц  или  нулей вначале, или в конце двоичной
     * последовательности.
     */

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

        public bool Equals([AllowNull] ITest other)
        {
            if (other is CusumTest cTest && this._forwardMode == cTest._forwardMode)
                return true;
            else return false;
        }

        public override string ToString()
        {
            return this.GetType().Name + " with forwardMode = " + this._forwardMode;
        }
    }
}
