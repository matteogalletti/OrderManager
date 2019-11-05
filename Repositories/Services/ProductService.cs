using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.DTO;
using Contracts.Services;
using Domain.Models;

namespace DAL.Services
{
    public class ProductService : IProductService 
    {
        private IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductDTO FindProduct(string id)
        {
            if (!Guid.TryParse(id, out Guid productId))
                return null;
            var product = _unitOfWork.ProductRepository.Find(p => p.Id == productId)
                .FirstOrDefault();

            return ConvertToDTO(product);
        }

        public string SaveProduct(ProductDTO product)
        {
            if (!Guid.TryParse(product.Category, out Guid categoryId))
                return null;
            var existingCategory = _unitOfWork.CategoryRepository
                .Find(c => c.Id == categoryId).FirstOrDefault();
            if (existingCategory == null)
                return null;

            var toSave = ConvertToProduct(product, existingCategory);
            _unitOfWork.ProductRepository.Create(toSave);
            _unitOfWork.Save();

            return toSave.Id.ToString();
        }

        private Product ConvertToProduct(ProductDTO dto, Category category)
        {
            var result = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Category = category,
                BaseOptions = new List<ProductOption>()
            };

            if (dto.Options != null)
            {
                foreach (var opt in dto.Options)
                {
                    result.BaseOptions.Add(new ProductOption
                    {
                        Id = Guid.NewGuid(),
                        Name = opt.Name,
                        Value = opt.Value
                    });
                }
            }

            return result;
        }

        private ProductDTO ConvertToDTO(Product product)
        {
            if (product == null) return null;

            var result = new ProductDTO
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Category = product.Category.Id.ToString(),
            };
            if (product.BaseOptions != null)
            {
                result.Options = product.BaseOptions.Select(o => new ProductOptionDTO
                {
                    Id = o.Id.ToString(),
                    Name = o.Name,
                    Value = o.Value
                }).ToArray();
            }

            return result;
        }
    }
}
