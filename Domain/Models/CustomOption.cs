using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class CustomOption
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Guid OptionId { get; set; }

        [Required]
        public string Value { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }

        public ProductOption ProductOption { get; set; }
    }
}
