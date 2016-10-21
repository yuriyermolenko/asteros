using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTask.Application.DTO.Order;
using HomeTask.Application.Services.Base;

namespace HomeTask.Application.Services.OrderAgg
{
    public interface IOrderService : IApplicationService
    {
        int Create(CreateOrderRequest request);
        Task<int> CreateAsync(CreateOrderRequest request);
        void Update(UpdateOrderRequest request);
        Task UpdateAsync(UpdateOrderRequest request);
        void Delete(int orderId);
        Task DeleteAsync(int orderId);
        IEnumerable<OrderDTO> GetForClient(int clientId);
        Task<IEnumerable<OrderDTO>> GetForClientAsync(int clientId);
    }
}