using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Models;

/// <summary>
/// Сущность задачи, хранимая в базе данных.
/// </summary>
public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }
}
