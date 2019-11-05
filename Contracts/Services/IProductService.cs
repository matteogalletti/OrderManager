using System;
using Contracts.DTO;

namespace Contracts.Services
{
    public interface IProductService
    {
        ProductDTO FindProduct(string id);
        string SaveProduct(ProductDTO product);
    }
}
