using System;
using Contracts.DTO;

namespace Contracts.Services
{
    public interface IOrderService
    {
        string CreateOrder(OrderDTO order, DateTime creationDate, out string errorMessage);
    }
}
