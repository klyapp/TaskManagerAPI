using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Models.Enums;

namespace TaskManager.Api.Dtos;

public class UpdateTaskItemDto
{
    [Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public TaskItemStatus Status { get; set; }

    public DateTime? DueDate { get; set; }
}