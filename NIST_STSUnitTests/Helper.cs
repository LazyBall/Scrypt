using NIST_STS.Tests;
using System;
using System.Collections;

namespace NIST_STSUnitTests
{
    static class Helper
    {
        private const double testEpsilon = 0.0000005;

        private static BitArray ConvertToBitArray(string sequence)
        {
            BitArray result = new BitArray(sequence.Length);

            for (int i = 0; i < sequence.Length; i++)
            {
                result[i] = (sequence[i]) switch
                {
                    '1' => true,
                    '0' => false,
                    _ => throw new Exception(),
                };
            }

            return result;
        }

        public static bool CheckTest(ITest test, string sequence, params double[] expectedPvalues)
        {
            BitArray bits = ConvertToBitArray(sequence);
            bool success = test.Run(bits, out double[] actualPvalues);
            success = success && (actualPvalues.Length == expectedPvalues.Length);

            for (int i = 0; success && (i < actualPvalues.Length); i++)
            {
                success = Math.Abs(actualPvalues[i] - expectedPvalues[i]) <= testEpsilon;
            }

            return success;
        }
    }
}
