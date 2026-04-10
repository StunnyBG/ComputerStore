using System.ComponentModel.DataAnnotations;
using static ComputerStore.Common.EntityConstants.Manufacturer;

namespace ComputerStore.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(CountryMaxLength)]
        public string? Country { get; set; }

        [MaxLength(WebsiteMaxLength)]
        public string? Website { get; set; }

        public virtual ICollection<PcPart> PcParts { get; set; } = new List<PcPart>();
    }
}