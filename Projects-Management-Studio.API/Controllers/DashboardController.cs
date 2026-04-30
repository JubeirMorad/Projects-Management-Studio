using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projects_Management_Studio.App.Interfaces.Services;

namespace Projects_Management_Studio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService dashboardService;
        private readonly ICurrrentUserService currrentUserService;
        public DashboardController(IDashboardService dashboardService, ICurrrentUserService currrentUserService)
        {
            this.dashboardService = dashboardService;
            this.currrentUserService = currrentUserService;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDashboard()
        {
            Guid userId = currrentUserService.UserId;

            var result = await dashboardService.GetDashboardAsync(userId);
            return Ok(result);
        }
    }
}