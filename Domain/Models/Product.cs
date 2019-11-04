using System;

namespace Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

    }

}
