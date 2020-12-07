using Microsoft.VisualStudio.TestTools.UnitTesting;
using NIST_STS.Tests;

namespace NIST_STSUnitTests
{
    [TestClass]
    public class BlockFrequencyUnitTests
    {
        private readonly (string sequence, int blockSize, double Pvalue)[] _testData;

        public BlockFrequencyUnitTests()
        {
            this._testData = new (string sequence, int blockSize, double Pvalue)[]
            {
                (sequence: "0110011010", blockSize: 3, Pvalue: 0.801252),
                (sequence: "11001001000011111101101010100010001000010110100011" +
                 "00001000110100110001001100011001100010100010111000", blockSize: 10,
                 Pvalue: 0.706438)
            };
        }

        private bool RunTest(int testNumber)
        {
            BlockFrequencyTest blockFrTest = new 
                BlockFrequencyTest(this._testData[testNumber].blockSize);
            bool result = Helper.CheckTest(blockFrTest, this._testData[testNumber].sequence,
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
