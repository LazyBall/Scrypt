using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ScryptUnitTests
{
    //the site - https://neurotechnics.com/tools/pbkdf2-test was used for testing
    [TestClass]
    public class UnitTestPBRDF2
    {
        [TestMethod]
        public void Test1000Iter()
        {
            byte[] P = Encoding.ASCII.GetBytes("VeryStrongPassword1");
            byte[] S = Encoding.ASCII.GetBytes("VeryRandomSalt1");
            string actualHash;

            using (var PBKDF2HMACSHA256 = new Rfc2898DeriveBytes(P, S,
                1000, HashAlgorithmName.SHA256))
            {
                actualHash = BitConverter.ToString(PBKDF2HMACSHA256.GetBytes(64));
            }
            actualHash = actualHash.Replace("-", "");
            string expextedHash = "7226dc88f09680aa8c61aab58cc7328ec4f18be57cc7ab5cb80dcbeeca321a8eed2fb757c6a33bf133ee87e880e3c916a5edfabc9af590ea6c0958528ac77303";

            Assert.AreEqual(expextedHash, actualHash, ignoreCase: true);
        }

        [TestMethod]
        public void Test1Iter()
        {
            byte[] P = Encoding.ASCII.GetBytes("VeryStrongPassword2");
            byte[] S = Encoding.ASCII.GetBytes("VeryRandomSalt2");
            string actualHash;

            using (var PBKDF2HMACSHA256 = new Rfc2898DeriveBytes(P, S,
                1, HashAlgorithmName.SHA256))
            {
                actualHash = BitConverter.ToString(PBKDF2HMACSHA256.GetBytes(8));
            }
            actualHash = actualHash.Replace("-", "");
            string expextedHash = "c37171e29911b8ba";

            Assert.AreEqual(expextedHash, actualHash, ignoreCase: true);
        }

        [TestMethod]
        public void Test65000Iter()
        {
            byte[] P = Encoding.ASCII.GetBytes("VeryStrongPassword3");
            byte[] S = Encoding.ASCII.GetBytes("VeryRandomSalt3");
            string actualHash;

            using (var PBKDF2HMACSHA256 = new Rfc2898DeriveBytes(P, S,
                65000, HashAlgorithmName.SHA256))
            {
                actualHash = BitConverter.ToString(PBKDF2HMACSHA256.GetBytes(32));
            }
            actualHash = actualHash.Replace("-", "");
            string expextedHash = "74ff70cb7b2bfd2469ec937c44fe2c25b95d1deccda868ff45742b2351543ab4";

            Assert.AreEqual(expextedHash, actualHash, ignoreCase: true);
        }
    }
}
