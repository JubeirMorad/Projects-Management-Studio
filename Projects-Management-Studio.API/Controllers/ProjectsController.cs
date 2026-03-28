
using System.Security.Claims;
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
        private readonly ICurrrentUserService currrentUser;

        public ProjectsController(IProjectService projectService, ICurrrentUserService currrentUser)
        {
            this.projectService = projectService;
            this.currrentUser = currrentUser;
        }


        //
        [HttpPost("New")]
        [Authorize()]
        public async Task<IActionResult> NewProject(AddNewProjectRequest request)
        {
            Guid OwnerId = currrentUser.UserId;
            
            await projectService.AddNewProjectAsync(request.Name, request.Description, OwnerId);

            return Ok();
        }


        //
        [HttpGet("get-my-projects")]
        [Authorize()]
        public async Task<IActionResult> GetMyProjects()
        {
            Guid OwnerId = currrentUser.UserId;

            var projects = await projectService.GetProjectsByOwnerIdAsync(OwnerId);

            return Ok(projects);
        }
    }
}