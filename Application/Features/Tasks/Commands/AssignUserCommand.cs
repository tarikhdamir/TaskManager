using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Tasks.Commands;

public class AssignUserCommand : IRequest<bool>
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
}

public class AssignUserHandler : IRequestHandler<AssignUserCommand, bool>
{
    private readonly AppDbContext _context;

    public AssignUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
        if (task == null)
            return false; 

        task.AssignedUserId = request.UserId;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
