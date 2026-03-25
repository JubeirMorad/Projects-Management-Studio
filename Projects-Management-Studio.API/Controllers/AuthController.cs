using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var output = await authService.LoginAsync(request.Email, request.Password);
                return Ok(new
                {
                    AccessToken = output.token,
                    RefrshToken = output.refreshToken
                });
            }
            catch (Exception)
            {
                return Ok("Invalid email or password.");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var token = await authService.RefreshTokenAsync(request.RefrshToken);

            return Ok(new {token});
        }

    }
}