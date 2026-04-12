using System.ComponentModel.DataAnnotations;
using static ComputerStore.Common.EntityConstants.PcPart;

namespace ComputerStore.Data.Dtos
{
    public class PcPartDto
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Range((double)MinPrice, (double)MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [Range(MinStock, int.MaxValue)]
        public int Stock { get; set; }

        [MaxLength(ImagePathMaxLength)]
        public string? ImagePath { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Required]
        public string? ManufacturerName { get; set; }
    }
}
