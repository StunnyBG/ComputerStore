using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ComputerStore.Common.EntityConstants.PcPart;

namespace ComputerStore.Data.Models;

public class PcPart
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = PriceColumnType)]
    [Range((double)MinPrice, (double)MaxPrice)]
    public decimal Price { get; set; }

    [Required]
    [Range(MinStock, int.MaxValue)]
    public int Stock { get; set; }

    [MaxLength(ImagePathMaxLength)]
    public string? ImagePath { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [Required]
    [ForeignKey(nameof(Manufacturer))]
    public int ManufacturerId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}