using System;
using System.Collections.Generic;
using System.Linq;
using HomeTask.Application.DTO.Order;
using HomeTask.Application.Exceptions;
using HomeTask.Application.TypeAdapter;
using HomeTask.Domain.Aggregates.OrderAgg;
using HomeTask.Domain.Contracts.Events.Order;
using HomeTask.Domain.Specifications;
using HomeTask.Infrastructure.Logging.Base;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.Persistence.Repositories;
using MyDeals.DataAccess.UnitOfWork;

namespace HomeTask.Application.Services.OrderAgg
{
    public class OrderService : IOrderService
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLog();

        private readonly IRepository<Order> _orderRepository;
        private readonly ITypeAdapter _typeAdapter;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IRepository<Order> orderRepository,
            ITypeAdapter typeAdapter,
            IEventBus eventBus,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _typeAdapter = typeAdapter;
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
        }

        public int Create(CreateOrderRequest request)
        {
            Logger.Debug("OrderService: Creating new order for " +
                         $"Client Id : {request.ClientId} " +
                         $"Description: {request.Description}");

            var order = _typeAdapter.Create<CreateOrderRequest, Order>(request);

            try
            {
                _orderRepository.Insert(order);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var message = "Failed to create new order for " +
                              $"Client Id : {request.ClientId} " +
                              $"Description: {request.Description}";

                Logger.LogError("OrderService: " + message, ex);

                throw new HomeTaskException(message, ex);
            }

            Logger.LogInfo("OrderService: New order created for" +
                           $"Client Id : {request.ClientId} " +
                           $"Description: {request.Description}");

            _eventBus.Publish(_typeAdapter.Create<Order, OrderCreated>(order));

            return order.Id;
        }

        public void Update(UpdateOrderRequest request)
        {
            Logger.Debug($"OrderService: Updating order id {request.Id} " +
                 $"Client Id : {request.ClientId} " +
                 $"Description: {request.Description}");

            var order = _orderRepository.FirstOrDefault(OrderSpecifications.ById(request.Id));

            if (order == null)
            {
                var message = $"Unable to find order Id {request.Id}";

                Logger.LogWarning($"OrderService: " + message);

                throw new NotFoundException(message);
            }

            try
            {
                _typeAdapter.Update(request, order);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var message = $"Failed to update order Id {request.Id}" +
                 $"Client Id : {request.ClientId} " +
                 $"Description: {request.Description}";

                Logger.LogError($"OrderService: " + message, ex);

                throw new HomeTaskException(ex.Message, ex);
            }

            Logger.LogInfo($"OrderService: Order id {request.Id} updated" +
                $"Client Id : {request.ClientId} " +
                $"Description: {request.Description}");

            _eventBus.Publish(_typeAdapter.Create<Order, OrderUpdated>(order));
        }

        public void Delete(int orderId)
        {
            Logger.Debug($"OrderService: Deleting order Id {orderId}");

            var order = _orderRepository.FirstOrDefault(OrderSpecifications.ById(orderId));

            if (order == null)
            {
                var message = $"Unable to find order Id {orderId}";

                Logger.LogWarning($"OrderService: " + message);

                throw new NotFoundException(message);
            }

            try
            {
                _orderRepository.Delete(order);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var message = $"Failed to delete order Id {orderId}";
                Logger.LogError($"OrderService: " + message, ex);

                throw new HomeTaskException(ex.Message, ex);
            }

            Logger.LogInfo($"OrderService: Order Id {orderId} deleted");

            _eventBus.Publish(_typeAdapter.Create<Order, OrderDeleted>(order));
        }

        public IEnumerable<OrderDTO> GetForClient(int clientId)
        {
            Logger.Debug($"LotService: Retrieving orders for {clientId}");

            var result = _orderRepository.Find(OrderSpecifications.ByClientId(clientId));

            return result.Select(elem => _typeAdapter.Create<Order, OrderDTO>(elem));
        }
    }
}