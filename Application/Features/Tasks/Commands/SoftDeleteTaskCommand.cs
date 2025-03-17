using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Tasks.Commands;

public class SoftDeleteTaskCommand : IRequest<bool>
{
    public int TaskId { get; set; }
}

public class SoftDeleteTaskHandler : IRequestHandler<SoftDeleteTaskCommand, bool>
{
    private readonly AppDbContext _context;

    public SoftDeleteTaskHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SoftDeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
        if (task == null)
            return false; 

        task.DeleteDate = DateTime.UtcNow; 
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
