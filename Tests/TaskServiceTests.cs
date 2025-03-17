using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Application.Features.Tasks.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;
using Xunit;

public class TaskServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TaskManagerTestDb")
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    [Fact]
    public async Task CreateTask_ShouldAddTaskToDatabase()
    {
        var dbContext = GetDbContext();
        var handler = new CreateTaskHandler(dbContext);

        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "This is a test task.",
            Status = "Новая",
            AssignedUserId = 1
        };

        var taskId = await handler.Handle(command, CancellationToken.None);
        var createdTask = await dbContext.Tasks.FindAsync(taskId);

        Assert.NotNull(createdTask);
        Assert.Equal("Test Task", createdTask.Title);
        Assert.Equal("Новая", createdTask.Status);
    }

    [Fact]
    public async Task UpdateTaskStatus_ShouldChangeStatus()
    {
        var dbContext = GetDbContext();
        var task = new TaskEntity { Title = "Test Task", Status = "Новая" };
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateTaskStatusHandler(dbContext);
        var command = new UpdateTaskStatusCommand { TaskId = task.Id, Status = "В работе" };

        var result = await handler.Handle(command, CancellationToken.None);
        var updatedTask = await dbContext.Tasks.FindAsync(task.Id);

        Assert.True(result);
        Assert.NotNull(updatedTask);
        Assert.Equal("В работе", updatedTask.Status);
    }

    [Fact]
    public async Task SoftDeleteTask_ShouldMarkTaskAsDeleted()
    {
        var dbContext = GetDbContext();
        var task = new TaskEntity { Title = "Test Task", Status = "Новая" };
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        var handler = new SoftDeleteTaskHandler(dbContext);
        var command = new SoftDeleteTaskCommand { TaskId = task.Id };

        var result = await handler.Handle(command, CancellationToken.None);
        var deletedTask = await dbContext.Tasks.FindAsync(task.Id);

        Assert.True(result);
        Assert.NotNull(deletedTask);
        Assert.NotNull(deletedTask.DeleteDate); 
    }
}
