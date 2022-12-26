using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberProjectTracker.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemberProjectTracker.Controllers
{
    [EnableCors("AllowAllHeaders")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class memberController : ControllerBase
    {
        private readonly TaskService _taskService;

        public memberController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public List<Tasks> Get()
        {
            return _taskService.GetAsync();
        }


        [HttpGet("{id:length(6)}")]
        public List<object> list(int id)
        {
            
            try
            {
               _taskService.GetAsync(id);
            }
            catch(Exception e)
            {
                
            }
            return _taskService.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> assigntask(Tasks task)
        {
            try
            {
                await _taskService.CreateAsyncTasks(task);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Get), new { id = task.mID }, task);
        }
    }
}
