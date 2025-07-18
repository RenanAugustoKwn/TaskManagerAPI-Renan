﻿using TaskManagementApiProject.Models;

namespace TaskManagementApiProject.DTOs;

public record TaskItemDto(int Id, string Title, string? Details, Models.TaskStatus Status, TaskPriority Priority, int ProjectId);