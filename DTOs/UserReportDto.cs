namespace TaskManagerAPI.DTOs;

public record UserPerformanceReportDto(string Username, int TasksCompleted, double AvgPerDay);