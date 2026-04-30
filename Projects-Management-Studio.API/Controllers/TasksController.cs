
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
            Guid userId = currrentUser.UserId;

            await taskService.CreateTaskAsync
            (
                userId,
                request.Title,
                request.Description,
                request.ProjectId,
                request.AssignedToUserId
            );

            return Ok();
        }



        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = currrentUser.UserId;

            var tasks = await taskService.GetUserTasksAsync(userId);

            return Ok(tasks);
        }




        [HttpGet("{projectId}/tasks")]
        //Roles = "Admin" later
        public async Task<IActionResult> GetProjectTasks(Guid projectId)
        {
            Guid userId = currrentUser.UserId;

            var tasks = await taskService.GetProjectTasksAsync(userId, projectId);

            return Ok(tasks);
        }



        [HttpPatch("assign")]
        // Role = "Admin" later
        public async Task<IActionResult> AssignTask(AssignTaskRequest request)
        {
            var userId = currrentUser.UserId;

            await taskService.AssignTaskAsync(userId, request.TaskId, request.AssignedToUserId);
            return Ok();
        }



        [HttpPatch("Update/{taskId}")]
        // Role = Project manager later
        public async Task UpdateTask([FromRoute] Guid taskId, [FromBody] UpdateTaskRequest request)
        {
            Guid userId = currrentUser.UserId;
            await taskService.UpdateTaskAsync(userId, taskId, request.Title, request.Description);
        }



        [HttpPatch("Update/{taskId}/Status")]
        // Role = Dev , later
        public async Task UpdateTask(Guid taskId, UpdateStatusRequest request)
        {
            Guid userId = currrentUser.UserId;

            await taskService.UpdateTaskStatusAsync(userId, taskId, request.Status);
        }
    }
}