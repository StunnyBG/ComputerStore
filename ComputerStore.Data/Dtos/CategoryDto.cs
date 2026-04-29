using System.ComponentModel.DataAnnotations;
using static ComputerStore.Common.EntityConstants.Category;

namespace ComputerStore.Data.Dtos;

public class CategoryDto
{
    [Required]
    [MaxLength(NameMaxLength)]
    public string? Name { get; set; }

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }
}
