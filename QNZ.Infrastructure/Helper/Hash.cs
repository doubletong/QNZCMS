using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIG.Infrastructure.Helper
{
    public class Hash
    {
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;

            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        public static string HashPasswordWithSalt(string password, byte[] salt)
        {

            // byte[] toBeHashed = Convert.FromBase64String(password);
            byte[] toBeHashed = Encoding.UTF8.GetBytes(password);
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Combine(toBeHashed, salt));
                return Convert.ToBase64String(hash);
            }
        }


    }
}