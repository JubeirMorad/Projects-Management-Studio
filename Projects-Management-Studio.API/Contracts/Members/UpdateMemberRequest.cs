
namespace Projects_Management_Studio.API.Contracts.Members
{
    public record UpdateMemberRequest
    (
        Guid ProjectId,
        Guid UserId,
        string Role
    );
}