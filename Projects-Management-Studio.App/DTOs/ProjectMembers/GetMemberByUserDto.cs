
namespace Projects_Management_Studio.App.DTOs.ProjectMembers
{
    public record GetMemberByUserDto
    (
        Guid ProjectId,
        string ProjectName,
        string? ProjectDescription,
        int TasksCount,
        string Role,
        Guid OwnerId,
        string OwnerName
    );
}