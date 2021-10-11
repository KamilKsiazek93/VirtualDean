using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Brothers : IBrothers
    {
        private readonly string _connectionString;
        public Brothers(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public void AddBrother(Brother brother)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Brother> GetBrothers()
        {
            throw new NotImplementedException();
        }
    }
}
