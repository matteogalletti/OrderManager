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

        public string CreateOrder(OrderDTO order, out string errorMessage)
        {
            if (!ValidateCreateOrderRequest(order, out errorMessage))
            {
                return null;
            }

            try
            {
                var toSave = ConvertToNewOrder(order);
                _unitOfWork.OrderRepository.Create(toSave);
                _unitOfWork.Save();
                return toSave.Id.ToString();
            }
            catch (Exception)
            {
                errorMessage = "Error saving new order";
                return null;
            }
        }

        //Returns a list of all orders
        //Only id and creationDate are returned for each order
        public OrderDTO[] GetOrders()
        {
            var orders = _unitOfWork.OrderRepository.FindAll().ToArray();
            return orders.Select(o => ConvertToDTO(o)).ToArray();
        }
        
        public OrderDTO GetOrder(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return null;

            var order = _unitOfWork.OrderRepository.Find(o => o.Id == guid)
                .FirstOrDefault();
            if (order == null) return null;


            return ConvertToDTO(order, convertProducts: true);
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
                return false;
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

        private Order ConvertToNewOrder(OrderDTO dto)
        {
            var orderId = Guid.NewGuid();
            var result = new Order
            {
                Id = orderId,
                CreationDate = DateTime.Now,
                Products = new List<OrderProduct>()
            };
            var customOptions = new List<CustomOption>();

            foreach (var product in dto.Products)
            {
                var productId = Guid.Parse(product.Id);
                result.Products.Add(new OrderProduct
                {
                    ProductId = productId,
                    OrderId = orderId
                });
                if (product.Options != null && product.Options.Count > 0)
                {
                    customOptions.AddRange(product.Options.Select(o => new CustomOption
                    {
                        ProductId = productId,
                        OrderId = orderId,
                        OptionId = Guid.Parse(o.Id),
                        Value = o.Value
                    }));
                }
            }
            result.CustomOptions = customOptions;
            return result;
        }

        private OrderDTO ConvertToDTO(Order order, bool convertProducts = false)
        {
            var result = new OrderDTO
            {
                Id = order.Id.ToString(),
                CreationDate = order.CreationDate
            };

            if (convertProducts)
            {
                var productIds = order.Products.Select(op => op.ProductId)
                .ToArray();
                var products = _unitOfWork.ProductRepository.Find(p => productIds.Contains(p.Id))
                    .ToArray();
                var orderProducts = new List<ProductDTO>();
                foreach (var product in products)
                {
                    var op = new ProductDTO
                    {
                        Id = product.Id.ToString(),
                        Name = product.Name,
                        Category = product.Category.Id.ToString(),
                        Options = GetOptions(product, order.CustomOptions)
                    };
                    orderProducts.Add(op);
                }
                result.Products = orderProducts;
            }

            return result;
        }

        private ProductOptionDTO[] GetOptions(Product product, ICollection<CustomOption> customOptions)
        {
            if(product.BaseOptions == null || product.BaseOptions.Count == 0)
                return new ProductOptionDTO[0];

            var hasCustomOptions = customOptions != null && customOptions.Count > 0;
            var result = new List<ProductOptionDTO>();
            foreach (var opt in product.BaseOptions)
            {
                var option = new ProductOptionDTO
                {
                    Id = opt.Id.ToString(),
                    Name = opt.Name,
                };
                var optionValue = opt.Value;

                if (hasCustomOptions)
                {
                    var relatedOption = customOptions.FirstOrDefault(co => co.ProductId == product.Id && co.OptionId == opt.Id);
                    if (relatedOption != null)
                        optionValue = relatedOption.Value;
                }
                option.Value = optionValue;
                result.Add(option);
            }

            return result.ToArray();
        }
    }
}
