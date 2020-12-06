using System;
using ScryptCryptography;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Scrypt:");
            Console.WriteLine(
                BitConverter.ToString(Scrypt.Encode("pleaseletmein", "SodiumChloride", 64, 1, 1, 32))
                );

            Console.WriteLine("Scrypt-OG:");
            Console.WriteLine(
                BitConverter.ToString(Scrypt_OG.Encode("pleaseletmein", "SodiumChloride"))
                );
        }

        static class Scrypt_OG
        {
            public static byte[] Encode(string password, string salt)
            {
                return Scrypt.Encode(password, salt, 64, 1, 1, 32);
            }
        }
    }
}
