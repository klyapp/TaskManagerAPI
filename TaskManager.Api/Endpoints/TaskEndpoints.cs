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

        // GET /api/tasks?status=InProgress
        group.MapGet("/", async (AppDbContext db, TaskItemStatus? status) =>
        {
            var query = db.Tasks.AsQueryable();

            if (status is not null)
            {
                query = query.Where(t => t.Status == status);
            }

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => ToDto(t))
                .ToListAsync();

            return Results.Ok(tasks);
        })
        .WithName("GetTasks")
        .WithSummary("Получить список задач с опциональной фильтрацией по статусу");

        // GET /api/tasks/{id}
        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);

            return task is null
                ? Results.NotFound(new { message = $"Задача с id {id} не найдена." })
                : Results.Ok(ToDto(task));
        })
        .WithName("GetTaskById")
        .WithSummary("Получить задачу по идентификатору");

        // POST /api/tasks
        group.MapPost("/", async (CreateTaskItemDto dto, AppDbContext db) =>
        {
            var validationResults = Validate(dto);
            if (validationResults.Count > 0)
            {
                return Results.ValidationProblem(ToErrorDictionary(validationResults));
            }

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
        .WithName("CreateTask")
        .WithSummary("Создать новую задачу");

        // PUT /api/tasks/{id}
        group.MapPut("/{id:int}", async (int id, UpdateTaskItemDto dto, AppDbContext db) =>
        {
            var validationResults = Validate(dto);
            if (validationResults.Count > 0)
            {
                return Results.ValidationProblem(ToErrorDictionary(validationResults));
            }

            var task = await db.Tasks.FindAsync(id);
            if (task is null)
            {
                return Results.NotFound(new { message = $"Задача с id {id} не найдена." });
            }

            task.Title = dto.Title.Trim();
            task.Description = dto.Description?.Trim();
            task.Status = dto.Status;
            task.DueDate = dto.DueDate;

            await db.SaveChangesAsync();

            return Results.Ok(ToDto(task));
        })
        .WithName("UpdateTask")
        .WithSummary("Обновить существующую задачу");

        // DELETE /api/tasks/{id}
        group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);
            if (task is null)
            {
                return Results.NotFound(new { message = $"Задача с id {id} не найдена." });
            }

            db.Tasks.Remove(task);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteTask")
        .WithSummary("Удалить задачу");
    }

    private static TaskItemDto ToDto(TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.CreatedAt,
        task.DueDate
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
