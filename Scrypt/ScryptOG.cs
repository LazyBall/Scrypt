

namespace ScryptCryptography
{
    public static class ScryptOG
    {
        private const int N = 64, r = 1, p = 1;

        public static byte[] Encode(string password, string salt, int dkLen = 32)
        {
            return Scrypt.Encode(password, salt, N, r, p, dkLen);
        }

        public static byte[] Encode(byte[] password, byte[] salt, int dkLen = 32)
        {
            return Scrypt.Encode(password, salt, N, r, p, dkLen);
        }
    }
}
