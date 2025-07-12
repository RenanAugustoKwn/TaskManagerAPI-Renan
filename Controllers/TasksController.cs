using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApiProject.Data;
using TaskManagementApiProject.DTOs;
using TaskManagementApiProject.Models;

namespace TaskManagementApiProject.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TasksController(AppDbContext db) => _db = db;

    // POST: api/projects/1/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTask(int projectId, CreateTaskItemDto dto)
    {
        var project = await _db.Projects.FindAsync(projectId);
        if (project is null) return NotFound();

        var task = new TaskItem { Title = dto.Title, Details = dto.Details, Status = dto.Status, Priority = dto.Priority, ProjectId = projectId };
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { projectId, id = task.Id },
            new TaskItemDto(task.Id, task.Title, task.Details, task.Status,task.Priority, projectId));
    }

    // GET: api/projects/1/tasks/2
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemDto>> GetTask(int projectId, int id)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == id);
        if (task is null) return NotFound();
        return new TaskItemDto(task.Id, task.Title, task.Details, task.Status, task.Priority, task.ProjectId);
    }

    // PUT: api/projects/1/tasks/2
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int projectId, int id, CreateTaskItemDto dto)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == id);
        if (task is null) return NotFound();

        task.Title = dto.Title;
        task.Details = dto.Details;
        task.Status = dto.Status;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/projects/1/tasks/2
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int projectId, int id)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == id);
        if (task is null) return NotFound();
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}