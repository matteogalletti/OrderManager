using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public Product Product { get; set; }
    }
}
