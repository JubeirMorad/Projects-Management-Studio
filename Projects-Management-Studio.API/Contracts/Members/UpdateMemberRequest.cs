
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.API.Contracts.Members
{
    public record UpdateMemberRequest
    (
        Guid UserId,
        ProjectRole Role = ProjectRole.Member
    );
}