using System;
using Domain.Models;

namespace Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<Product> ProductRepository { get; set; }
        IBaseRepository<Order> OrderRepository { get; set; }
        IBaseRepository<Category> CategoryRepository { get; set; }
        void Save();
    }
}
