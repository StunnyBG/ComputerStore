using System.ComponentModel.DataAnnotations;
using ComputerStore.Data.Models.Enums;
using static ComputerStore.Common.EntityConstants.User;

namespace ComputerStore.Data.Dtos;

public class UserDto
{
    [Required]
    [MaxLength(UsernameMaxLength)]
    [MinLength(UsernameMinLength)]
    public string? Username { get; set; }

    [Required]
    [MaxLength(EmailMaxLength)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    public string Role { get; set; } = UserRole.Customer.ToString();
}
