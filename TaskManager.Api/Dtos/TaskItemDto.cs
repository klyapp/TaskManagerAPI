using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Dtos;

/// <summary>
/// Данные задачи, которые отдаются клиенту.
/// </summary>
public record TaskItemDto(
    int Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    DateTime CreatedAt,
    DateTime? DueDate
);
