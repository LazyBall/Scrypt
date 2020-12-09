using ScryptCryptography;
using System;

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
                BitConverter.ToString(ScryptOG.Encode("pleaseletmein", "SodiumChloride"))
                );
        }
    }
}
