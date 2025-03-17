using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Features.Tasks.Commands;
using TaskManager.Application.Features.Tasks.Queries;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTask(CreateTaskCommand command)
    {
        var taskId = await _mediator.Send(command);
        return Ok(taskId);
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskEntity>>> GetTasks(
        [FromQuery] string? status,
        [FromQuery] int? assignedUserId,
        [FromQuery] DateTime? createdAfter)
    {
        var query = new GetTasksQuery
        {
            Status = status,
            AssignedUserId = assignedUserId,
            CreatedAfter = createdAfter
        };

        var tasks = await _mediator.Send(query);
        return Ok(tasks);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, UpdateTaskCommand command)
    {
        if (taskId != command.TaskId)
            return BadRequest("Task ID in URL does not match request body.");

        var result = await _mediator.Send(command);
        if (!result)
            return NotFound("Task not found.");

        return NoContent(); 
    }

    [HttpPatch("{taskId}/assign")]
    public async Task<IActionResult> AssignUserToTask(int taskId, [FromBody] AssignUserCommand command)
    {
        if (taskId != command.TaskId)
            return BadRequest("Task ID in URL does not match request body.");

        var result = await _mediator.Send(command);
        if (!result)
            return NotFound("Task not found.");

        return NoContent(); 
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> SoftDeleteTask(int taskId)
    {
        var result = await _mediator.Send(new SoftDeleteTaskCommand { TaskId = taskId });
        if (!result)
            return NotFound("Task not found.");

        return NoContent(); 
    }

    [HttpPatch("{taskId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusCommand command)
    {
        if (taskId != command.TaskId)
            return BadRequest("Task ID in URL does not match request body.");

        var result = await _mediator.Send(command);
        if (!result)
            return NotFound("Task not found.");

        return NoContent(); 
    }


}
