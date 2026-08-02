using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Dtos;
using TaskManager.Api.Models;
using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");
        group.MapGet("/", async (AppDbContext db, TaskItemStatus? status) =>
            {
                var query = db.Tasks.AsQueryable();

                if (status is not null)
                    query = query.Where(t => t.Status == status);

                var tasks = await query
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => ToDto(t))
                    .ToListAsync();

                return Results.Ok(tasks);
            })
            .WithName("GetTasks");
        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);

                return task is null
                    ? Results.NotFound(new { message = $"Задача с id {id} не найдена." })
                    : Results.Ok(ToDto(task));
            })
            .WithName("GetTaskById");
        group.MapPost("/", async (CreateTaskItemDto dto, AppDbContext db) =>
            {
                var validationResults = Validate(dto);
                if (validationResults.Count > 0)
                    return Results.ValidationProblem(ToErrorDictionary(validationResults));

                var task = new TaskItem
                {
                    Title = dto.Title.Trim(),
                    Description = dto.Description?.Trim(),
                    DueDate = dto.DueDate,
                    Status = TaskItemStatus.ToDo,
                    CreatedAt = DateTime.UtcNow
                };

                db.Tasks.Add(task);
                await db.SaveChangesAsync();

                return Results.Created($"/api/tasks/{task.Id}", ToDto(task));
            })
            .WithName("CreateTask");
        group.MapPut("/{id:int}", async (int id, UpdateTaskItemDto dto, AppDbContext db) =>
            {
                var validationResults = Validate(dto);
                if (validationResults.Count > 0)
                    return Results.ValidationProblem(ToErrorDictionary(validationResults));

                var task = await db.Tasks.FindAsync(id);
                if (task is null)
                    return Results.NotFound(new { message = $"Задача с id {id} не найдена." });

                task.Title = dto.Title.Trim();
                task.Description = dto.Description?.Trim();
                task.Status = dto.Status;
                task.DueDate = dto.DueDate;

                await db.SaveChangesAsync();

                return Results.Ok(ToDto(task));
            })
            .WithName("UpdateTask");
        group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
            {
                var task = await db.Tasks.FindAsync(id);
                if (task is null)
                    return Results.NotFound(new { message = $"Задача с id {id} не найдена." });

                db.Tasks.Remove(task);
                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("DeleteTask");
    }
    private static TaskItemDto ToDto(TaskItem task) => new(
        task.Id, task.Title, task.Description, task.Status, task.CreatedAt, task.DueDate
    );

    private static List<ValidationResult> Validate(object dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    private static Dictionary<string, string[]> ToErrorDictionary(List<ValidationResult> results)
    {
        return results
            .SelectMany(r => r.MemberNames.DefaultIfEmpty(string.Empty)
                .Select(member => (Member: member, r.ErrorMessage)))
            .GroupBy(x => x.Member)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage ?? "Некорректное значение.").ToArray()
            );
    }
}