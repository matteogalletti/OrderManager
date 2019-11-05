using System;
using Contracts.DTO;

namespace Contracts.Services
{
    public interface IProductService
    {
        ProductDTO[] GetProducts();
        ProductDTO FindProduct(string id);
        string SaveProduct(ProductDTO product);
    }
}
