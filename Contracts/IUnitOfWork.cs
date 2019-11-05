using System;
using Domain.Models;

namespace Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<Product> ProductRepository { get; }
        IBaseRepository<Order> OrderRepository { get; }
        IBaseRepository<Category> CategoryRepository { get; }
        void Save();
    }
}
