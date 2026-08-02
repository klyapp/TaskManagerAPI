using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Dtos;

public class CreateTaskItemDto
{
    [Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}