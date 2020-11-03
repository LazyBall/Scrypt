using System;
using System.Security.Cryptography;
using System.Text;

namespace Scrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Scrypt();
            //Console.WriteLine(HMAC("passwd","saltsalt"));
            //Console.WriteLine(Salsa());
            //Console.WriteLine(BlockMix());
            Console.WriteLine(ROMix());
        }

        private static string HMAC(string pass, string salt)
        {
            byte[] P = Encoding.ASCII.GetBytes(pass);
            byte[] S = Encoding.ASCII.GetBytes(salt);
            using (var PBKDF2HMACSHA256 = new Rfc2898DeriveBytes(P, S,
                1, HashAlgorithmName.SHA256))
            {
                return BitConverter.ToString(PBKDF2HMACSHA256.GetBytes(64));
            }
        }

        private static string Salsa()
        {
            string arr = "7e 87 9a 21 4f 3e c9 86 7c a9 40 e6 41 71 8f 26 " +
                         "ba ee 55 5b 8c 61 c1 b5 0d f8 46 11 6d cd 3b 1d " +
                         "ee 24 f3 19 df 9b 3d 85 14 12 1e 4b 5a c5 aa 32 " +
                         "76 02 1d 29 09 c7 48 29 ed eb c6 8d b8 b8 c2 5e ";
            var hexv = arr.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            byte[] val = new byte[hexv.Length];

            for(int i=0; i<hexv.Length; i++)
            {
                val[i] = Convert.ToByte(hexv[i], 16);
            }

            return BitConverter.ToString(Scrypt.ConvertToBytes(
                Scrypt.Salsa208(Scrypt.ConvertToUInts(val))));
        }

        private static string BlockMix()
        {
            string arr = "f7 ce 0b 65 3d 2d 72 a4 10 8c f5 ab e9 12 ff dd "+
           "77 76 16 db bb 27 a7 0e 82 04 f3 ae 2d 0f 6f ad "+
           "89 f6 8f 48 11 d1 e8 7b cc 3b d7 40 0a 9f fd 29 "+
           "09 4f 01 84 63 95 74 f3 9a e5 a1 31 52 17 bc d7 "+
           
           "89 49 91 44 72 13 bb 22 6c 25 b5 4d a8 63 70 fb "+
           "cd 98 43 80 37 46 66 bb 8f fc b5 bf 40 c2 54 b0 "+
           "67 d2 7c 51 ce 4a d5 fe d8 29 c9 0b 50 5a 57 1b "+
           "7f 4d 1c ad 6a 52 3c da 77 0e 67 bc ea af 7e 89 ";
            var hexv = arr.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            byte[] val = new byte[hexv.Length];

            for (int i = 0; i < hexv.Length; i++)
            {
                val[i] = Convert.ToByte(hexv[i], 16);
            }

            return BitConverter.ToString(Scrypt.BlockMix(1,val));
        }

        private static string ROMix()
        {
            string arr = 
                "f7 ce 0b 65 3d 2d 72 a4 10 8c f5 ab e9 12 ff dd " +
                "77 76 16 db bb 27 a7 0e 82 04 f3 ae 2d 0f 6f ad " +
                "89 f6 8f 48 11 d1 e8 7b cc 3b d7 40 0a 9f fd 29 " +
                "09 4f 01 84 63 95 74 f3 9a e5 a1 31 52 17 bc d7 " +
                "89 49 91 44 72 13 bb 22 6c 25 b5 4d a8 63 70 fb " +
                "cd 98 43 80 37 46 66 bb 8f fc b5 bf 40 c2 54 b0 " +
                "67 d2 7c 51 ce 4a d5 fe d8 29 c9 0b 50 5a 57 1b " +
                "7f 4d 1c ad 6a 52 3c da 77 0e 67 bc ea af 7e 89 ";
            var hexv = arr.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            byte[] val = new byte[hexv.Length];

            for (int i = 0; i < hexv.Length; i++)
            {
                val[i] = Convert.ToByte(hexv[i], 16);
            }

            return BitConverter.ToString(Scrypt.ROMix(1, val, 16));
        }
    }
}
