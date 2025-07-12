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

        var user = User?.Identity?.Name ?? "Sistema"; // ou use outro mecanismo para obter o usuário

        var now = DateTime.UtcNow;
        var changes = new List<TaskUpdateHistory>();

        if (dto.Title != task.Title)
            changes.Add(new TaskUpdateHistory
            {
                TaskId = task.Id,
                PropertyChanged = "Title",
                OldValue = task.Title,
                NewValue = dto.Title,
                ChangedAt = now,
                ChangedBy = user
            });

        if (dto.Details != task.Details)
            changes.Add(new TaskUpdateHistory
            {
                TaskId = task.Id,
                PropertyChanged = "Details",
                OldValue = task.Details,
                NewValue = dto.Details,
                ChangedAt = now,
                ChangedBy = user
            });

        if (dto.Status != task.Status)
            changes.Add(new TaskUpdateHistory
            {
                TaskId = task.Id,
                PropertyChanged = "Status",
                OldValue = task.Status.ToString(),
                NewValue = dto.Status.ToString(),
                ChangedAt = now,
                ChangedBy = user
            });

        // Impede a alteração da prioridade (regra de negócio)
        if (dto.Priority != task.Priority)
        {
            return BadRequest("Não é permitido alterar a prioridade da tarefa depois que ela foi criada.");
        }

        // Atualização de campos
        task.Title = dto.Title;
        task.Details = dto.Details;
        task.Status = dto.Status;

        if (changes.Any())
            await _db.TaskUpdateHistories.AddRangeAsync(changes);

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