using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Tasks.Commands;

public class CreateTaskCommand : IRequest<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public int? AssignedUserId { get; set; }
}

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly AppDbContext _context;

    public CreateTaskHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            Status = string.IsNullOrEmpty(request.Status) ? "Новая" : request.Status, 
            AssignedUserId = request.AssignedUserId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(cancellationToken);
        return task.Id;
    }

}
