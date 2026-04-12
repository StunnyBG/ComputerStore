using System.ComponentModel.DataAnnotations;
using static ComputerStore.Common.EntityConstants.Manufacturer;

namespace ComputerStore.Data.Dtos
{
    public class ManufacturerDto
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(CountryMaxLength)]
        public string? Country { get; set; }

        [MaxLength(WebsiteMaxLength)]
        public string? Website { get; set; }
    }
}
