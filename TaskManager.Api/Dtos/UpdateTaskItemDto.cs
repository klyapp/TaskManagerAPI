using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Dtos;

/// <summary>
/// Данные для обновления существующей задачи.
/// </summary>
public class UpdateTaskItemDto
{
    [Required(ErrorMessage = "Поле Title обязательно для заполнения.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title должен содержать от 3 до 200 символов.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description не может превышать 1000 символов.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Поле Status обязательно для заполнения.")]
    public TaskItemStatus Status { get; set; }

    public DateTime? DueDate { get; set; }
}
