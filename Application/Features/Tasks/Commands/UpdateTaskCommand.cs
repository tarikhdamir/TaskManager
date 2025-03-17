using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Tasks.Commands;

public class UpdateTaskCommand : IRequest<bool>
{
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public int? AssignedUserId { get; set; }
}

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateTaskHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task == null)
            return false; 

        if (!string.IsNullOrEmpty(request.Title))
            task.Title = request.Title;

        if (!string.IsNullOrEmpty(request.Description))
            task.Description = request.Description;

        if (!string.IsNullOrEmpty(request.Status))
            task.Status = request.Status;

        if (request.AssignedUserId.HasValue)
            task.AssignedUserId = request.AssignedUserId.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
