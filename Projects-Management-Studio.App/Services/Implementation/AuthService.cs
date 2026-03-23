using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.App.Services.Interfaces;

namespace Projects_Management_Studio.App.Services.Implementation
{
    public class AuthService : IAuthService
    {
        public Task RegisterAsync(string userName, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}