

namespace Projects_Management_Studio.App.DTOs.Tasks
{
    public record GetTaskByProjectDto
    (
        Guid Id,
        string Title,
        string? Description,

        Guid? AssignedToUserId,
        string? AssignedToUserName,

        string Status
    );
}