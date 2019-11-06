using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DTO
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public ICollection<ProductOptionDTO> Options { get; set; }
    }
}
