using System.ComponentModel.DataAnnotations;
using TaskManagementApiProject.Models;

namespace TaskManagerAPI.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        public string Role { get; set; } = "colaborador"; // ou "gerente"

        public ICollection<TaskUpdateHistory> Histories { get; set; } = new List<TaskUpdateHistory>();
    }
}
