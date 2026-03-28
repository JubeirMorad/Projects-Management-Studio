using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Projects_Management_Studio.App.Interfaces.Services;

namespace Projects_Management_Studio.API.ApiServices
{
    public class CurrentUserService : ICurrrentUserService
    {

        private readonly IHttpContextAccessor _ctx;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _ctx = httpContextAccessor;
        }
        public Guid UserId => 
                    Guid.Parse(_ctx.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value) ;
    }
}