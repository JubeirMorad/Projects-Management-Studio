using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projects_Management_Studio.API.Contracts.Tasks;
using Projects_Management_Studio.App.Interfaces.Services;

namespace Projects_Management_Studio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly ICurrrentUserService currrentUser;

        public TasksController(ITaskService taskService, ICurrrentUserService currrentUserService)
        {
            this.taskService = taskService;
            this.currrentUser = currrentUserService;
        }


        //
        [HttpPost("New")]
        public async Task<IActionResult> NewTask(AddNewTaskRequest request)
        {
            Guid OwnerId = currrentUser.UserId;
            
            await taskService.CreateTaskAsync
            (
                OwnerId,
                request.Title,
                request.Description,
                request.ProjectId,
                request.AssignedToUserId
            );

            return Ok();
        }

        [HttpGet("get-my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = currrentUser.UserId;

            var tasks = await taskService.GetTasksUserAsync(userId);

            return Ok(tasks);
        }

        [HttpGet("get-project-tasks")]
         //Roles = "Admin" later
        public async Task<IActionResult> GetProjectTasks(GetProjectTasksRequest request)
        {

            var tasks = await taskService.GetTasksProjectAsync(request.ProjectId);

            return Ok(tasks);
        } 

    }
}