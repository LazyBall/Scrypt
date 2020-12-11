using MathNet.Numerics;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NIST_STS.Tests
{
    /*
     * Частотный побитовый(монобитный) тест
     * Суть данного теста заключается в определении соотношения между нулями и единицами во всей
     * двоичной последовательности. Цель — выяснить, действительно ли число нулей и единиц в
     * последовательности приблизительно одинаковы, как это можно было бы предположить в случае
     * истинно случайной бинарной последовательности. Тест оценивает, насколько близка доля единиц
     * к 0,5. Таким образом, число нулей и единиц должно быть примерно одинаковым. Если вычисленное
     * в ходе теста значение вероятности p < 0,01, то данная двоичная последовательность не является
     * истинно случайной. В противном случае последовательность носит случайный характер.

     * Статистика теста c(S) : Нормализованная  абсолютная  сумма  значений
     * элементов последовательности.
     * Выявляемый дефект: Слишком  много  нулей  или  единиц  в последовательности.
     * 
     * Если набор случайных величин подчиняется нормальному распределению со стандартным отклонением,
     * то вероятность, что величина отклонится от среднего не более чем на A, равна erfc A/sqrt(2).
     */

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
