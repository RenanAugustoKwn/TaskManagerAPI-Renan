using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApiProject.Data;
using TaskManagementApiProject.DTOs;
using TaskManagementApiProject.Models;

namespace TaskManagementApiProject.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProjectsController(AppDbContext db) => _db = db;

    // GET: api/projects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var projects = await _db.Projects
            .Select(p => new ProjectDto(p.Id, p.Name, p.Description))
            .ToListAsync();
        return Ok(projects);
    }

    // GET: api/projects/5/tasks
    [HttpGet("{id:int}/tasks")]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasksForProject(int id)
    {
        var exists = await _db.Projects.AnyAsync(p => p.Id == id);
        if (!exists) return NotFound();

        var tasks = await _db.Tasks
            .Where(t => t.ProjectId == id)
            .Select(t => new TaskItemDto(t.Id, t.Title, t.Details, t.Status, t.ProjectId))
            .ToListAsync();
        return Ok(tasks);
    }

    // POST: api/projects
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto dto)
    {
        var project = new Project { Name = dto.Name, Description = dto.Description };
        _db.Projects.Add(project);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, new ProjectDto(project.Id, project.Name, project.Description));
    }
}