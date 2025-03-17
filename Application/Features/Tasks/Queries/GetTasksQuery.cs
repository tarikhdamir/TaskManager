using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.Application.Features.Tasks.Queries;

public class GetTasksQuery : IRequest<List<TaskEntity>>
{
    public string? Status { get; set; } 
    public int? AssignedUserId { get; set; } 
    public DateTime? CreatedAfter { get; set; } 
}

public class GetTasksHandler : IRequestHandler<GetTasksQuery, List<TaskEntity>>
{
    private readonly AppDbContext _context;

    public GetTasksHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskEntity>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(t => t.Status == request.Status);
        }

        if (request.AssignedUserId.HasValue)
        {
            query = query.Where(t => t.AssignedUserId == request.AssignedUserId);
        }

        if (request.CreatedAfter.HasValue)
        {
            query = query.Where(t => t.CreatedAt >= request.CreatedAfter.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
