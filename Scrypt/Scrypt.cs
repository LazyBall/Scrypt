using System;
using System.Security.Cryptography;
using System.Text;

namespace ScryptCryptography
{
    /// <summary>
    /// The scrypt Algorithm.
    /// </summary>
    /// <remarks>
    /// RFC - <see href="https://tools.ietf.org/html/rfc7914"/>
    /// Article - <see href="http://www.tarsnap.com/scrypt/scrypt.pdf"/>
    /// </remarks>
    public static class Scrypt
    {
        /// <include file='docs.xml' path='docs/members[@name="scrypt"]/EncodeString/*'/>
        public static byte[] Encode(string passphrase, string salt, int N, int r, int p, int dkLen)
        {
            byte[] P = Encoding.UTF8.GetBytes(passphrase);
            byte[] S = Encoding.UTF8.GetBytes(salt);
            return Scrypt.Encode(P, S, N, r, p, dkLen);
        }

        /// <include file='docs.xml' path='docs/members[@name="scrypt"]/EncodeBytes/*'/>
        public static byte[] Encode(byte[] passphrase, byte[] salt, int N, int r, int p, int dkLen)
        {
            if (passphrase is null)
            {
                throw new ArgumentNullException("Passphrase must be not null.", nameof(passphrase));
            }
            if (salt is null)
            {
                throw new ArgumentNullException("Salt must be not null.", nameof(salt));
            }
            Scrypt.CheckParameters(salt, N, r, p, dkLen);
            return Scrypt.MFcrypt(passphrase, salt, N, r, p, dkLen);
        }

        private static void CheckParameters(byte[] salt, int N, int r, int p, int dkLen)
        {
            if (salt.Length < 8)
            {
                throw new ArgumentException("Salt is not at least eight bytes.",
                    nameof(salt));
            }
            if (dkLen < 1)
            {
                throw new ArgumentOutOfRangeException("Intended output length must be " +
                    "a positive integer.", nameof(dkLen));
            }
            if (r < 1)
            {
                throw new ArgumentOutOfRangeException("Block size parameter must be " +
                    "a positive integer.", nameof(r));
            }
            if ((N <= 1) || ((N & (N - 1)) != 0))
            {
                throw new ArgumentOutOfRangeException("CPU / Memory cost parameter must be " +
                    "larger than 1, a power of 2", nameof(N));
            }
            if (r == 1 && N >= (1 << 16))
            {
                throw new ArgumentOutOfRangeException("CPU / Memory cost parameter must be less " +
                    "than 2^(128 * r / 8).", nameof(N));
            }
            if (p < 1 || (ulong)p * (ulong)r >= (1 << 30))
            {
                throw new ArgumentOutOfRangeException("Parallelization parameter must be a positive " +
                    "integer less than or equal to((2 ^ 32 - 1) * hLen) / MFLen " +
                    "where hLen is 32 and MFlen is 128 * r.", nameof(p));
            }
        }

        private class ByteBlocks
        {
            private readonly byte[] _source;
            public int BlockSize { get; private set; }
            public int NumberOfBlocks => this._source.Length / this.BlockSize;

            public ByteBlocks(byte[] sourceArray, int numberOfBlocks)
            {
                this.BlockSize = Math.DivRem(sourceArray.Length, numberOfBlocks, out int remainder);
                if (remainder != 0)
                {
                    throw new Exception(string.Format("Internal error.\n" +
                        "sourceArray.Length is {0}, numberOfBlocks is {1}.",
                        sourceArray.Length, numberOfBlocks));
                }

                this._source = sourceArray;
            }

            public ByteBlocks(int numberOfBlocks, int blockSize)
            {
                this.BlockSize = blockSize;
                this._source = new byte[numberOfBlocks * this.BlockSize];
            }

            public byte[] this[int index]
            {
                get
                {
                    byte[] block = new byte[this.BlockSize];
                    Array.Copy(this._source, this.BlockSize * index, block, 0, this.BlockSize);
                    return block;
                }

                set
                {
                    if (value.Length != this.BlockSize)
                    {
                        throw new Exception(string.Format("Internal error.\n" +
                            "value.Length is {0}, BlockSize is {1}.", value.Length, this.BlockSize));
                    }
                    Array.Copy(value, 0, this._source, this.BlockSize * index, this.BlockSize);
                }
            }

            public static explicit operator byte[](ByteBlocks blocks) => blocks._source;
        }

        /// <summary>
        /// Utility method for Salsa20/8.
        /// </summary>
        private static uint R(uint a, int b)
        {
            return (a << b) | (a >> (32 - b));
        }

        /// <summary>
        /// Apply the salsa20/8 core to the provided block.
        /// </summary>
        private static uint[] Salsa208(uint[] input)
        {
            uint[] x = new uint[16];
            Array.Copy(input, x, input.Length);
            
            for (int i = 0; i < 8; i += 2)
            {
                x[4] ^= R(x[0] + x[12], 7);      x[8] ^= R(x[4] + x[0], 9);
                x[12] ^= R(x[8] + x[4], 13);     x[0] ^= R(x[12] + x[8], 18);
                x[9] ^= R(x[5] + x[1], 7);       x[13] ^= R(x[9] + x[5], 9);
                x[1] ^= R(x[13] + x[9], 13);     x[5] ^= R(x[1] + x[13], 18);
                x[14] ^= R(x[10] + x[6], 7);     x[2] ^= R(x[14] + x[10], 9);
                x[6] ^= R(x[2] + x[14], 13);     x[10] ^= R(x[6] + x[2], 18);
                x[3] ^= R(x[15] + x[11], 7);     x[7] ^= R(x[3] + x[15], 9);
                x[11] ^= R(x[7] + x[3], 13);     x[15] ^= R(x[11] + x[7], 18);
                x[1] ^= R(x[0] + x[3], 7);       x[2] ^= R(x[1] + x[0], 9);
                x[3] ^= R(x[2] + x[1], 13);      x[0] ^= R(x[3] + x[2], 18);
                x[6] ^= R(x[5] + x[4], 7);       x[7] ^= R(x[6] + x[5], 9);
                x[4] ^= R(x[7] + x[6], 13);      x[5] ^= R(x[4] + x[7], 18);
                x[11] ^= R(x[10] + x[9], 7);     x[8] ^= R(x[11] + x[10], 9);
                x[9] ^= R(x[8] + x[11], 13);     x[10] ^= R(x[9] + x[8], 18);
                x[12] ^= R(x[15] + x[14], 7);    x[13] ^= R(x[12] + x[15], 9);
                x[14] ^= R(x[13] + x[12], 13);   x[15] ^= R(x[14] + x[13], 18);
            }

            for (int i = 0; i < x.Length; i++)
            {
                x[i] += input[i];
            }

            return x;
        }

        private static uint[] ConvertToUInts(byte[] array)
        {
            uint[] result = new uint[array.Length / 4];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToUInt32(array, i * 4);
            }

            return result;
        }

        private static byte[] ConvertToBytes(uint[] array)
        {
            byte[] result = new byte[array.Length * 4];

            for (int i = 0; i < array.Length; i++)
            {
                BitConverter.GetBytes(array[i]).CopyTo(result, i * 4);
            }

            return result;
        }

        private static byte[] Xor(byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(array1[i] ^ array2[i]);
            }

            return result;
        }

        private static byte[] BlockMix(int r, byte[] B)
        {
            ByteBlocks blocks = new ByteBlocks(B, 2 * r);
            byte[] X = blocks[2 * r - 1];
            ByteBlocks Y = new ByteBlocks(2 * r, X.Length);

            for (int i = 0; i < 2 * r; i++)
            {
                byte[] T = Xor(X, blocks[i]);
                X = ConvertToBytes(Salsa208(ConvertToUInts(T)));
                Y[i] = X;
            }

            for (int i = 0; i < r; i++)
            {
                blocks[i] = Y[i * 2];
                blocks[i + r] = Y[i * 2 + 1];
            }

            return (byte[])blocks;
        }

        private static int Integerify(byte[] B, int r)
        {
            ByteBlocks blocks = new ByteBlocks(B, 2 * r);
            byte[] requiredBlock = blocks[2 * r - 1];
            return (requiredBlock[0])
                | (requiredBlock[1] << 8)
                | (requiredBlock[2] << 16)
                | (requiredBlock[3] << 24);
        }

        private static byte[] ROMix(int r, byte[] B, int N)
        {
            byte[] X = new byte[B.Length];
            B.CopyTo(X, 0);

            ByteBlocks V = new ByteBlocks(N, X.Length);

            for (int i = 0; i < N; i++)
            {
                V[i] = X;
                X = Scrypt.BlockMix(r, X);
            }

            for (int i = 0; i < N; i++)
            {
                int j = Integerify(X, r) % N;
                if (j < 0)
                {
                    j += N;
                }
                byte[] T = Xor(X, V[j]);
                X = Scrypt.BlockMix(r, T);
            }

            return X;
        }

        private static byte[] MFcrypt(byte[] P, byte[] S, int N, int r, int p, int dkLen)
        {
            ByteBlocks B;
            using (var PBKDF2HMACSHA256 = new Rfc2898DeriveBytes(P, S, 1, HashAlgorithmName.SHA256))
            {
                B = new ByteBlocks(PBKDF2HMACSHA256.GetBytes(p * 128 * r), p);
            }

            for (int i = 0; i < p; i++)
            {
                B[i] = Scrypt.ROMix(r, B[i], N);
            }

            using (var PBKDF2HMACSHA256 = new Rfc2898DeriveBytes(P, (byte[])B, 1,
                HashAlgorithmName.SHA256))
            {
                return PBKDF2HMACSHA256.GetBytes(dkLen);
            }
        }
    }
}
