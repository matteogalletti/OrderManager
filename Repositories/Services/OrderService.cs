using System;
using System.Linq;
using Contracts;
using Contracts.DTO;
using Contracts.Services;

namespace DAL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string CreateOrder(OrderDTO order, DateTime creationDate, out string errorMessage)
        {
            errorMessage = null;

            if (order.Id != null)
            {
                errorMessage = "Id must be null";
                return null;
            }

            if (order.Products == null || order.Products.Count == 0)
            {
                errorMessage = "Empty product list";
                return null;
            }

            var productIds = order.Products.Select(p => {
                if (!Guid.TryParse(p.Id, out Guid guid))
                    return Guid.Empty;
                return guid;
            }).ToArray();
            if (productIds.Any(id => id == Guid.Empty))
            {
                errorMessage = "Order contains products with invalid id";
                return null;
            }

            var products = _unitOfWork.ProductRepository
                .Find(p => productIds.Contains(p.Id))
                .ToArray();
            if (products == null || products.Length != productIds.Length)
            {
                errorMessage = "Order contains not existing products";
                return null;
            }

            var categories = products.Select(p => p.Category.Id).Distinct().ToArray();
            if (categories.Length != 1)
            {
                errorMessage = "Products do not belong to same category";
            }
                

            return null;
        }
    }
}
