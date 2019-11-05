using System.ComponentModel.DataAnnotations;

namespace Contracts.DTO
{
    public class ProductOptionDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }

    }
}
