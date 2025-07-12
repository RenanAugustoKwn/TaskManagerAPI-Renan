using TaskManagementApiProject.Models;

namespace TaskManagementApiProject.DTOs;

public record CreateTaskItemDto(string Title, string? Details, Models.TaskStatus Status, TaskPriority Priority);