using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Auth : IAuth
    {
        private readonly IConfiguration _configuration;
        public Auth(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(BaseModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.StatusBrother)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

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
