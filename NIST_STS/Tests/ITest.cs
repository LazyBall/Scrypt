using System.Collections;

namespace NIST_STS.Tests
{
    public interface ITest
    {
        /// <summary>
        /// Метод запуска теста.
        /// </summary>
        /// <param name="sequence">Тестируемая последовательность бит.</param>
        /// <returns>Результат прохождения теста.
        /// <list type="table">
        /// <item>
        /// <c>true</c> - тест пройден и последовательность признана случайной.
        /// </item>
        /// <item>
        /// <c>false</c> - тест не пройден и последовательность признана неслучайной.
        /// </item>
        /// </list>
        /// </returns>
        public bool Run(BitArray sequence);

        /// <summary>
        /// Метод запуска теста.
        /// </summary>
        /// <param name="sequence">Тестируемая последовательность бит.</param>
        /// <param name="Pvalues">Массив значений Pvalue, полученных тестом.</param>
        /// <returns>Результат прохождения теста.
        /// <list type="table">
        /// <item>
        /// <c>true</c> - тест пройден и последовательность признана случайной.
        /// </item>
        /// <item>
        /// <c>false</c> - тест не пройден и последовательность признана неслучайной.
        /// </item>
        /// </list>
        /// </returns>
        public bool Run(BitArray sequence, out double[] Pvalues);
    }
}
