using System.ComponentModel.DataAnnotations;
using ComputerStore.Data.Models.Enums;
using static ComputerStore.Common.EntityConstants.User;


namespace ComputerStore.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UsernameMaxLength)]
        [MinLength(UsernameMinLength)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(PasswordHashLength)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(EmailMaxLength)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Customer;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}