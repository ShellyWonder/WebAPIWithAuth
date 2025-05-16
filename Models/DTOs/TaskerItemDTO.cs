using System.ComponentModel.DataAnnotations;

namespace WebAPIWithAuth.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for TaskerItem. 
    /// Used by the front end and API endpoints.
    /// </summary>
    public class TaskerItemDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please choose a name. Every task must be named.")]
        public string? Name { get; set; }
        public bool IsComplete { get; set; } = false;
    }
}
