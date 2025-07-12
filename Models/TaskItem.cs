using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApiProject.Models;

public enum TaskStatus { Pending, InProgress, Done }
public enum TaskPriority { Low, Medium, High }

public class TaskItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Details { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;

    [ForeignKey(nameof(Project))]
    public int ProjectId { get; set; }

    public Project? Project { get; set; }
}