using CSharpNewAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpNewAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
        bool validateToken(string token);
    }
}
