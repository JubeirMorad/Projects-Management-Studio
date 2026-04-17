
namespace Projects_Management_Studio.API.Contracts.Members
{
    public record UpdateMemberRequest
    (
        Guid UserId,
        string Role
    );
}