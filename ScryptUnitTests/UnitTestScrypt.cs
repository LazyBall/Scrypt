using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScryptCryptography;
using System;

namespace ScryptUnitTests
{
    [TestClass]
    public class UnitTestScrypt
    {
        [TestMethod]
        public void TestEncodeStrWithNormalParams()
        {
            string actualHash =
                 BitConverter.ToString(Scrypt.Encode("pleaseletmein", "SodiumChloride", 16384, 8, 1, 64));
            actualHash = actualHash.Replace("-", " ");
            string expectedHash =
                "70 23 bd cb 3a fd 73 48 46 1c 06 cd 81 fd 38 eb " +
                "fd a8 fb ba 90 4f 8e 3e a9 b5 43 f6 54 5d a1 f2 " +
                "d5 43 29 55 61 3f 0f cf 62 d4 97 05 24 2a 9a f9 " +
                "e6 1e 85 dc 0d 65 1e 40 df cf 01 7b 45 57 58 87";
            Assert.AreEqual(expectedHash, actualHash, ignoreCase: true);
        }

        [TestMethod]
        public void TestEncodeStrWithBigN()
        {
            string actualHash =
                 BitConverter.ToString(Scrypt.Encode("pleaseletmein", "SodiumChloride",
                 1048576, 8, 1, 64));
            actualHash = actualHash.Replace("-", " ");
            string expectedHash =
                "21 01 cb 9b 6a 51 1a ae ad db be 09 cf 70 f8 81 " +
                "ec 56 8d 57 4a 2f fd 4d ab e5 ee 98 20 ad aa 47 " +
                "8e 56 fd 8f 4b a5 d0 9f fa 1c 6d 92 7c 40 f4 c3 " +
                "37 30 40 49 e8 a9 52 fb cb f4 5c 6f a7 7a 41 a4";
            Assert.AreEqual(expectedHash, actualHash, ignoreCase: true);
        }

        [TestMethod]
        public void TestEncodeBytesWithNormalParams()
        {
            string actualHash =
                 BitConverter.ToString(Scrypt.Encode(
                     new byte[] { 0x70, 0x6c, 0x65, 0x61, 0x73, 0x65, 0x6c, 0x65, 0x74, 0x6d, 0x65, 0x69, 0x6e },
                     new byte[] { 0x53, 0x6f, 0x64, 0x69, 0x75, 0x6d, 0x43, 0x68, 0x6c, 0x6f, 0x72, 0x69, 0x64, 0x65 },
                     16384, 8, 1, 64));
            actualHash = actualHash.Replace("-", " ");
            string expectedHash =
                "70 23 bd cb 3a fd 73 48 46 1c 06 cd 81 fd 38 eb " +
                "fd a8 fb ba 90 4f 8e 3e a9 b5 43 f6 54 5d a1 f2 " +
                "d5 43 29 55 61 3f 0f cf 62 d4 97 05 24 2a 9a f9 " +
                "e6 1e 85 dc 0d 65 1e 40 df cf 01 7b 45 57 58 87";
            Assert.AreEqual(expectedHash, actualHash, ignoreCase: true);
        }

        [TestMethod]
        public void TestEncodeBytesWithBigN()
        {
            string actualHash =
                 BitConverter.ToString(Scrypt.Encode(
                     new byte[] { 0x70, 0x6c, 0x65, 0x61, 0x73, 0x65, 0x6c, 0x65, 0x74, 0x6d, 0x65, 0x69, 0x6e },
                     new byte[] { 0x53, 0x6f, 0x64, 0x69, 0x75, 0x6d, 0x43, 0x68, 0x6c, 0x6f, 0x72, 0x69, 0x64, 0x65 },
                     1048576, 8, 1, 64));
            actualHash = actualHash.Replace("-", " ");
            string expectedHash =
                "21 01 cb 9b 6a 51 1a ae ad db be 09 cf 70 f8 81 " +
                "ec 56 8d 57 4a 2f fd 4d ab e5 ee 98 20 ad aa 47 " +
                "8e 56 fd 8f 4b a5 d0 9f fa 1c 6d 92 7c 40 f4 c3 " +
                "37 30 40 49 e8 a9 52 fb cb f4 5c 6f a7 7a 41 a4";
            Assert.AreEqual(expectedHash, actualHash, ignoreCase: true);
        }
    }
}
