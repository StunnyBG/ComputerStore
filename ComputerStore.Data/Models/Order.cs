using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComputerStore.Data.Models.Enums;
using static ComputerStore.Common.EntityConstants.Order;

namespace ComputerStore.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        [Column(TypeName = TotalPricePrecision)]
        public decimal TotalPrice { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}