using System;
using System.Collections.Generic;

namespace Contracts.DTO
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<ProductDTO> Products { get; set; }
    }
}
