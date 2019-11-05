using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DTO
{
    public class ProductDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        public ICollection<ProductOptionDTO> Options { get; set; }
    }
}
