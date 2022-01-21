using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Data
{
    public interface IAuth
    {
        void GetSalt(ref byte[] salt);
        string GetHashedPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
    }
}
