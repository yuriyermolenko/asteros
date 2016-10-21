using System.Collections.Generic;
using HomeTask.Application.DTO.Order;
using HomeTask.Application.Services.Base;

namespace HomeTask.Application.Services.OrderAgg
{
    public interface IOrderService
    {
        int Create(CreateOrderRequest request);
        void Update(UpdateOrderRequest request);
        void Delete(int orderId);
        IEnumerable<OrderDTO> GetForClient(int clientId);
    }
}