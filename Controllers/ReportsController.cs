using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApiProject.Data;
using TaskManagementApiProject.Models;
using TaskManagerAPI.DTOs;

namespace TaskManagementApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReportsController(AppDbContext db) => _db = db;

    // GET: api/reports/performance
    [HttpGet("performance")]
    [Authorize(Roles = "gerente")]
    public async Task<ActionResult<IEnumerable<UserPerformanceReportDto>>> GetPerformanceReport()
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        var report = await _db.TaskUpdateHistories
            .Where(h => h.PropertyChanged == "Status"
                        && h.NewValue == Models.TaskStatus.Done.ToString()
                        && h.ChangedAt >= thirtyDaysAgo)
            .GroupBy(h => h.ChangedBy)
            .Select(g => new UserPerformanceReportDto(
                g.Key,
                g.Count(),
                g.Count() / 30.0
            ))
            .ToListAsync();

        return Ok(report);
    }
}
