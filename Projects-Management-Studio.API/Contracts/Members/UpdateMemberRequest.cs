
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.API.Contracts.Members
{
    public record UpdateMemberRequest
    (
        ProjectRole Role = ProjectRole.Member
    );
}