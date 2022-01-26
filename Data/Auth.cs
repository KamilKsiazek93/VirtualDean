using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace VirtualDean.Data
{
    public class Auth : IAuth
    {
        public string GetHashedPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        public void GetSalt(ref byte[] salt)
        {
            using var rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetNonZeroBytes(salt);
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return GetHashedPassword(actualPassword) == hashedPassword;
        }
    }
}
