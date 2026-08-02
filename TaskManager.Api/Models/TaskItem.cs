using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Models;

// сущность как она лежит в БД, не путать с DTO ниже — в DTO решаем, что из этого отдавать наружу
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
}