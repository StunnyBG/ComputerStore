using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ComputerStore.Common.EntityConstants.OrderItem;

namespace ComputerStore.Data.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(MinQuantity, MaxQuantity)]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = UnitPricePrecision)]
    public decimal UnitPrice { get; set; } 

    [Required]
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }

    [Required]
    [ForeignKey(nameof(PcPart))]
    public int PcPartId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PcPart PcPart { get; set; } = null!;
}