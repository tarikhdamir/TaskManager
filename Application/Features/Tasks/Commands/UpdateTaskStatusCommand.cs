using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Tasks.Commands;

public class UpdateTaskStatusCommand : IRequest<bool>
{
    public int TaskId { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
{
    private readonly AppDbContext _context;
    private static readonly HashSet<string> AllowedStatuses = new() { "Новая", "В работе", "Выполнена" };

    public UpdateTaskStatusHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        if (!AllowedStatuses.Contains(request.Status))
            throw new ArgumentException($"Недопустимый статус задачи: {request.Status}. Разрешены: Новая, В работе, Выполнена.");

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
        if (task == null)
            return false; 

        task.Status = request.Status;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
