using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projects_Management_Studio.API.Contracts;
using Projects_Management_Studio.App.Services.Interfaces;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await authService.RegisterAsync(request.Name, request.Email, request.Password);

            return Ok();
        }


    }
}