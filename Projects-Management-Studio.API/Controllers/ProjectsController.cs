using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projects_Management_Studio.API.Contracts.Projects;
using Projects_Management_Studio.App.Interfaces.Services;

namespace Projects_Management_Studio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpPost("New")]
        [Authorize()]
        public async Task<IActionResult> NewProject(AddNewProjectRequest request)
        {
            Guid OwnerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            await projectService.AddNewProjectAsync(request.Name, request.Description, OwnerId);

            return Ok();
        }


        [HttpGet("get-my-projects")]
        [Authorize()]
        public async Task<IActionResult> GetMyProjects()
        {
            Guid OwnerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var projects = await projectService.GetProjectsByOwnerIdAsync(OwnerId);

            return Ok(projects);
        }
    }
}