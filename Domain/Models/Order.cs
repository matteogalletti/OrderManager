using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<OrderProduct> Products {get;set;}

        public ICollection<CustomOption> CustomOptions { get; set; }
    }
}
