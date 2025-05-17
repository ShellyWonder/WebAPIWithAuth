using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIWithAuth.Data;
using WebAPIWithAuth.Models;
using WebAPIWithAuth.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebAPIWithAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskerItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        //uses embodied expression(=>) to get the user id of the current user
        private string? UserId => _userManager.GetUserId(User);

        #region API GET METHODS
        // GET: api/TaskerItems
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskerItemDTO>>> GetTaskerItems()
        {
            return await _context.TaskerItems
                                 .Where(t => t.UserId == UserId)
                                 .Select(t => t.ToDTO())
                                 .ToListAsync();
        }

        // GET: api/TaskerItems/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskerItemDTO>> GetTaskerItem(int id)
        {
            var taskerItem = await _context.TaskerItems.FirstOrDefaultAsync(t => t.UserId == UserId && t.Id == id);

            if (taskerItem == null) return NotFound();

            return taskerItem.ToDTO();
        } 
        #endregion

        #region API PUT
        // PUT: api/TaskerItems/5
        //Update the task item
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskerItem(int id, TaskerItemDTO taskerItemDTO)
        {
            if (id != taskerItemDTO.Id) return BadRequest();


            var taskerItem = await _context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

            if (taskerItem == null)
            {
                return NotFound();
            }
            else
            {
                taskerItem.Name = taskerItemDTO.Name;
                taskerItem.IsComplete = taskerItemDTO.IsComplete;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskerItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        #endregion

        #region API POST
        // POST: api/TaskerItems  //Create a new task item
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskerItemDTO>> PostTaskerItem(TaskerItemDTO taskerItemDTO)
        {
            var taskerItem = new TaskerItem
            {
                Name = taskerItemDTO.Name,
                IsComplete = taskerItemDTO.IsComplete,
                UserId = UserId
            };
            _context.TaskerItems.Add(taskerItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskerItem", new { id = taskerItem.Id }, taskerItem);
        }
        #endregion

        // DELETE: api/TaskerItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskerItem(int id)
        {
            var taskerItem = await _context.TaskerItems.FirstOrDefaultAsync(taskerItem => taskerItem.Id == id && taskerItem.UserId == UserId);
            if (taskerItem == null)
            {
                return NotFound();
            }

            _context.TaskerItems.Remove(taskerItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize]
        private bool TaskerItemExists(int id)
        {
            return _context.TaskerItems.Any(e => e.Id == id && e.UserId ==UserId);
        }
    }
}
