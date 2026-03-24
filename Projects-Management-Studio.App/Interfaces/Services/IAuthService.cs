using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.App.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(string userName, string email, string password);

        Task<(string token, string refreshToken)> LoginAsync(string email, string password); 
    }
}