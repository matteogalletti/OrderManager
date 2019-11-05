using System;
using Contracts;
using DAL.Repositories;
using Domain;
using Domain.Models;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private DomainContext _context;
        public UnitOfWork(DomainContext context)
        {
            _context = context;
        }

        private IBaseRepository<Product> _productRepo;
        public IBaseRepository<Product> ProductRepository
        {
            get
            {
                if(_productRepo == null)
                    _productRepo = new BaseRepository<Product>(_context);
                return _productRepo;
            }
        }

        private IBaseRepository<Order> _orderRepo;
        public IBaseRepository<Order> OrderRepository
        {
            get
            {
                if (_orderRepo == null)
                    _orderRepo = new BaseRepository<Order>(_context);
                return _orderRepo;
            }
        }

        public IBaseRepository<Category> _categoryRepo;
        public IBaseRepository<Category> CategoryRepository
        {
            get
            {
                if (_categoryRepo == null)
                    _categoryRepo = new BaseRepository<Category>(_context);
                return _categoryRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
