using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.App.DTOs.Dashboard;

namespace Projects_Management_Studio.App.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync(Guid userId);
    }
}