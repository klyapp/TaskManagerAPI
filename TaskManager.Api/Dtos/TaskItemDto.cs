using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Dtos;

public record TaskItemDto(
    int Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    DateTime CreatedAt,
    DateTime? DueDate
);