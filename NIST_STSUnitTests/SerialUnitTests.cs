using Microsoft.VisualStudio.TestTools.UnitTesting;
using NIST_STS.Tests;

namespace NIST_STSUnitTests
{
    [TestClass]
    public class SerialUnitTests
    {
        private readonly (string sequence, int blockSize, double Pvalue1,
            double Pvalue2)[] _testData;

        public SerialUnitTests()
        {
            this._testData = new (string sequence, int blockSize, double Pvalue1, double Pvalue2)[]
            {
                (sequence: "0011011101", blockSize: 3, Pvalue1: 0.808792, Pvalue2: 0.670320)
            };
        }

        private bool RunTest(int testNumber)
        {
            SerialTest serialTest = new SerialTest(this._testData[testNumber].blockSize);
            bool result = Helper.CheckTest(serialTest, this._testData[testNumber].sequence,
                this._testData[testNumber].Pvalue1, this._testData[testNumber].Pvalue2);
            return result;
        }

        [TestMethod]
        public void TestOnDoc()
        {
            Assert.IsTrue(RunTest(0));
        }
    }
}