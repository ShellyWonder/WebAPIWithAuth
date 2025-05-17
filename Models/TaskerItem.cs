
//defines properties of a single task item(object) in the tasker list
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebAPIWithAuth.Models.DTOs;

namespace WebAPIWithAuth.Models
{
    public class TaskerItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; } 

        [Required(ErrorMessage ="Please choose a name. Every task must be named.")]
        public required string Name { get; set; } 
        public bool IsComplete { get; set; } = false;

        /// <summary>
        /// Navigation property to the user who created this task.
        /// defines the relationship between the task and the user--this table's foreign key
        /// </summary>
        public virtual IdentityUser User { get; set; } = null!;

    }
    public static class  TaskerItemExtension
    {
        public static TaskerItemDTO ToDTO(this TaskerItem item) => new TaskerItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            IsComplete = item.IsComplete
        };
    }
}
