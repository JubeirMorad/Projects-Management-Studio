using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Projects_Management_Studio.API.Contracts.Members;
using Projects_Management_Studio.App.Interfaces.Services;

namespace Projects_Management_Studio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly ICurrrentUserService _currentUser;

        public MembersController(IMemberService memberService, ICurrrentUserService currrentUserService)
        {
            _memberService = memberService;
            _currentUser = currrentUserService;
        }


        [HttpGet("project/{projectId:Guid}")]
        public async Task<IActionResult> GetByPorject(Guid projectId)
        {
            Guid userId = _currentUser.UserId;

            var result = await _memberService.GetProjectMembersAsync(userId, projectId);

            return Ok(result);
        }



        [HttpGet("get-my-members")]
        public async Task<IActionResult> GetMyMembers()
        {
            Guid userId = _currentUser.UserId;
            var result = await _memberService.GetUserMembersAsync(userId);

            return Ok(result);
        }


        [HttpGet("{memberId:Guid}")]
        //admin only
        public async Task<IActionResult> GetById(Guid memberId)
        {
            var result = await _memberService.GetMemberByIdAsync(memberId);

            return Ok(result);
        }

        [HttpPost("new")]
        //admin only
        public async Task<IActionResult> add(AddMemberRequest request)
        {
            Guid userId = _currentUser.UserId;

            await _memberService.CreateMemberAsync(userId, request.ProjectId, request.UserId);

            return Ok();
        }

        [HttpPut("{memberId:Guid}/update")]
        // admin only
        public async Task<IActionResult> UpdateMember(Guid memberId, UpdateMemberRequest request)
        {
            Guid userId = _currentUser.UserId;

            await _memberService.UpdateMemberAsync(userId, memberId, request.ProjectId, request.UserId, request.Role);

            return Ok();
        }


        
    }
}