using Microsoft.VisualStudio.TestTools.UnitTesting;
using NIST_STS.Tests;

namespace NIST_STSUnitTests
{
    [TestClass]
    public class CusumUnitTests
    {
        private readonly (string sequence, bool forwardMode, double Pvalue)[] _testData;

        public CusumUnitTests()
        {
            this._testData = new (string sequence, bool forwardMode, double Pvalue)[]
            {
                (sequence: "1011010111", forwardMode: true, Pvalue: 0.4116588),
                (sequence: "11001001000011111101101010100010001000010110100011" +
                "00001000110100110001001100011001100010100010111000", forwardMode: true,
                Pvalue: 0.219194),
                (sequence: "11001001000011111101101010100010001000010110100011" +
                "00001000110100110001001100011001100010100010111000", forwardMode: false,
                Pvalue: 0.114866)
            };
        }

        private bool RunTest(int testNumber)
        {
            CusumTest cusumTest = new CusumTest(this._testData[testNumber].forwardMode);
            bool result = Helper.CheckTest(cusumTest, this._testData[testNumber].sequence,
                this._testData[testNumber].Pvalue);
            return result;
        }

        [TestMethod]
        public void TestOnDoc()
        {
            Assert.IsTrue(RunTest(0));
        }

        [TestMethod]
        public void TestOnExampleForward()
        {
            Assert.IsTrue(RunTest(1));
        }

        [TestMethod]
        public void TestOnExampleBackward()
        {
            Assert.IsTrue(RunTest(2));
        }
    }
}