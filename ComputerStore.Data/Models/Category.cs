using System.ComponentModel.DataAnnotations;
using static ComputerStore.Common.EntityConstants.Category;

namespace ComputerStore.Data.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    public virtual ICollection<PcPart> PcParts { get; set; } = new List<PcPart>();
}