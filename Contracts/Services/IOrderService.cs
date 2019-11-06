using System;
using Contracts.DTO;

namespace Contracts.Services
{
    public interface IOrderService
    {
        string CreateOrder(OrderDTO order, out string errorMessage);
        OrderDTO GetOrder(string id);
        OrderDTO[] GetOrders();
    }
}
