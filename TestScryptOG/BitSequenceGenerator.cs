﻿using ScryptCryptography;
using System;
using System.Collections;
using System.Text;

namespace TestScryptOG
{
    interface IBitSequenceGenerator
    {
        public BitArray Generate(int length);
    }

    class GeneratorOnZeroPassAndSalt : IBitSequenceGenerator
    {
        public BitArray Generate(int length)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            byte[] pass = new byte[rand.Next(0, 256)];
            byte[] salt = new byte[rand.Next(8, 256)];
            int bytesCount = Math.DivRem(length, 8, out int remainder);
            if (remainder > 0) bytesCount++;
            return new BitArray(ScryptOG.Encode(pass, salt, bytesCount))
            {
                Length = length
            };
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }

    class GeneratorOnRandomPassAndSalt : IBitSequenceGenerator
    {
        public BitArray Generate(int length)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            byte[] pass = new byte[rand.Next(0, 256)];
            byte[] salt = new byte[rand.Next(8, 256)];
            rand.NextBytes(pass);
            rand.NextBytes(salt);
            int bytesCount = Math.DivRem(length, 8, out int remainder);
            if (remainder > 0) bytesCount++;
            return new BitArray(ScryptOG.Encode(pass, salt, bytesCount))
            {
                Length = length
            };
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }

    class GeneratorWherePassAndSaltFromConsole : IBitSequenceGenerator
    {
        public BitArray Generate(int length)
        {
            Console.Write("Enter password for ScryptOG: ");
            byte[] pass = Encoding.UTF8.GetBytes(Console.ReadLine());
            Console.Write("Enter salt for ScryptOG (min 8 symbols): ");
            byte[] salt = Encoding.UTF8.GetBytes(Console.ReadLine());
            int bytesCount = Math.DivRem(length, 8, out int remainder);
            if (remainder > 0) bytesCount++;
            return new BitArray(ScryptOG.Encode(pass, salt, bytesCount))
            {
                Length = length
            };
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
