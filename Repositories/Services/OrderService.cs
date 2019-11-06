using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.DTO;
using Contracts.Services;
using Domain.Models;

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
            if (!ValidateCreateOrderRequest(order, out errorMessage))
            {
                return null;
            }

            //TODO: Create and save order

            return null;
        }

        private bool ValidateCreateOrderRequest(OrderDTO order, out string errorMessage)
        {
            errorMessage = null;

            if (order.Id != null)
            {
                errorMessage = "Id must be null";
                return false;
            }

            if (order.Products == null || order.Products.Count == 0)
            {
                errorMessage = "Empty product list";
                return false;
            }

            var productIds = order.Products.Select(p => {
                if (!Guid.TryParse(p.Id, out Guid guid))
                    return Guid.Empty;
                return guid;
            }).ToArray();
            if (productIds.Any(id => id == Guid.Empty))
            {
                errorMessage = "Order contains products with invalid id";
                return false;
            }

            var existingProducts = _unitOfWork.ProductRepository
                .Find(p => productIds.Contains(p.Id))
                .ToArray();
            if (existingProducts == null || existingProducts.Length != productIds.Length)
            {
                errorMessage = "Order contains not existing products";
                return false;
            }

            var categories = existingProducts.Select(p => p.Category.Id).Distinct().ToArray();
            if (categories.Length != 1)
            {
                errorMessage = "Products do not belong to same category";
            }

            var optionsValidationErrors = new List<string>();
            foreach (var p in order.Products)
            {

                if (!ValidateProductOptions(p, existingProducts))
                    optionsValidationErrors.Add(String.Format("Product {0} contains invalid options", p.Id));
            }
            if (optionsValidationErrors.Any())
            {
                errorMessage = String.Join("\r\n", optionsValidationErrors);
                return false;
            }

            return true;
        }

        private bool ValidateProductOptions(ProductDTO dto, Product[] existingProducts)
        {
            if (dto.Options == null || dto.Options.Count == 0)
                return true;

            if (dto.Options.Any(o => String.IsNullOrWhiteSpace(o.Id)))
                return false;

            var optionIds = dto.Options.Select(o => {
                if (!Guid.TryParse(o.Id, out Guid guid))
                    return Guid.Empty;
                return guid;
            }).ToArray();
            if (optionIds.Any(id => id == Guid.Empty)) return false;

            var relatedProduct = existingProducts.Single(p => p.Id.ToString() == dto.Id);

            if (relatedProduct.BaseOptions == null || relatedProduct.BaseOptions.Count == 0)
                return false;
            var relatedProductOptionIds = relatedProduct.BaseOptions.Select(o => o.Id);

            return optionIds.All(oId => relatedProductOptionIds.Contains(oId));
        }
    }
}
