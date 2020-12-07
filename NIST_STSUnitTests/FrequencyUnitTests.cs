using Microsoft.VisualStudio.TestTools.UnitTesting;
using NIST_STS.Tests;

namespace NIST_STSUnitTests
{
    [TestClass]
    public class FrequencyUnitTests
    {
        private readonly FrequencyTest _freqTest;
        private readonly (string sequence, double Pvalue)[] _testData;

        public FrequencyUnitTests()
        {
            this._freqTest = new FrequencyTest();
            this._testData = new (string sequence, double Pvalue)[]
            {
                (sequence: "1011010101", Pvalue: 0.527089),
                (sequence: "11001001000011111101101010100010001000010110100011" +
                 "00001000110100110001001100011001100010100010111000", Pvalue: 0.109599)
            };
        }

        private bool RunTest(int testNumber)
        {
            bool result = Helper.CheckTest(this._freqTest, this._testData[testNumber].sequence,
                this._testData[testNumber].Pvalue);
            return result;
        }

        [TestMethod]
        public void TestOnDoc()
        {
            Assert.IsTrue(RunTest(0));
        }

        [TestMethod]
        public void TestOnExample()
        {
            Assert.IsTrue(RunTest(1));
        }
    }
}
