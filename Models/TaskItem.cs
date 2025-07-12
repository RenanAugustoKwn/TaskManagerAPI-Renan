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

    public ICollection<TaskUpdateHistory> UpdateHistories { get; set; } = new List<TaskUpdateHistory>();

    [ForeignKey(nameof(Project))]
    public int ProjectId { get; set; }

    public Project? Project { get; set; }
}
public class TaskUpdateHistory
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string PropertyChanged { get; set; } = null!;
    public string OldValue { get; set; } = null!;
    public string NewValue { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; } = null!;

    public TaskItem Task { get; set; } = null!;
}