using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagerProjectTracker.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ManagerProjectTracker.Controllers
{
    [EnableCors("AllowAllHeaders")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class managerController : ControllerBase
    {
        private readonly TeamMembersService _teamMembersService;

        public managerController(TeamMembersService teamMembersService)
        {
            _teamMembersService = teamMembersService;
        }
        [HttpGet]
        public List<TeamMembers> list()
        {
            return _teamMembersService.GetAsync();
        }

        [HttpGet("{id:length(6)}")]
        public async Task<ActionResult<TeamMembers>> list(int id)
        {
            var member = await _teamMembersService.GetAsync(id);

            if (member is null)
            {
                return NotFound();
            }

            return member;
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(TeamMembers newTeamMember)
        {
            try
            {
                await _teamMembersService.CreateAsync(newTeamMember);
            }
                catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(list), new { id = newTeamMember.mID }, newTeamMember);
        }


        [HttpPatch("{allocation}")]
        public List<TeamMembers> update(int allocation)
        {
               var updatedTeamMember =  _teamMembersService.UpdateAllocationPatchAsync(allocation);
                return updatedTeamMember;
        }
    }
}
