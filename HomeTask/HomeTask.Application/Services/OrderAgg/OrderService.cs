using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTask.Application.DTO.Order;
using HomeTask.Application.Exceptions;
using HomeTask.Application.TypeAdapter;
using HomeTask.Domain.Aggregates.OrderAgg;
using HomeTask.Domain.Contracts.Events.Order;
using HomeTask.Domain.Specifications;
using HomeTask.Infrastructure.Logging.Base;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.Persistence.UnitOfWork;

namespace HomeTask.Application.Services.OrderAgg
{
    public class OrderService : IOrderService
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLog();

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ITypeAdapter _typeAdapter;
        private readonly IEventBus _eventBus;

        public OrderService(
            IUnitOfWorkFactory unitOfWorkFactory,
            ITypeAdapter typeAdapter,
            IEventBus eventBus)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _typeAdapter = typeAdapter;
            _eventBus = eventBus;
        }

        public int Create(CreateOrderRequest request)
        {
            Logger.Debug("OrderService: Creating new order for " +
                         $"Client Id : {request.ClientId} " +
                         $"Description: {request.Description}");

            var order = _typeAdapter.Create<CreateOrderRequest, Order>(request);

            IUnitOfWork unitOfWork = null;

            try
            {
                unitOfWork = _unitOfWorkFactory.CreateScope();

                unitOfWork.OrderRepository.Insert(order);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var message = "Failed to create new order for " +
                              $"Client Id : {request.ClientId} " +
                              $"Description: {request.Description}";

                Logger.LogError("OrderService: " + message, ex);

                throw new HomeTaskException(message, ex);
            }
            finally
            {
                unitOfWork?.Dispose();
            }

            InformCreated(request, order);

            return order.Id;
        }

        public async Task<int> CreateAsync(CreateOrderRequest request)
        {
            Logger.Debug("OrderService: Creating new order async for " +
                         $"Client Id : {request.ClientId} " +
                         $"Description: {request.Description}");

            var order = _typeAdapter.Create<CreateOrderRequest, Order>(request);

            IUnitOfWork unitOfWork = null;

            try
            {
                unitOfWork = _unitOfWorkFactory.CreateScope();

                unitOfWork.OrderRepository.Insert(order);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                var message = "Failed to create new order async for " +
                              $"Client Id : {request.ClientId} " +
                              $"Description: {request.Description}";

                Logger.LogError("OrderService: " + message, ex);

                throw new HomeTaskException(message, ex);
            }
            finally
            {
                unitOfWork?.Dispose();
            }

            InformCreated(request, order);

            return order.Id;
        }

        public void Update(UpdateOrderRequest request)
        {
            Logger.Debug($"OrderService: Updating order id {request.Id} " +
                 $"Client Id : {request.ClientId} " +
                 $"Description: {request.Description}");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var order = EnsureOrderExistence(request.Id, unitOfWork);

                try
                {
                    _typeAdapter.Update(request, order);
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    var message = $"Failed to update order Id {request.Id}" +
                                  $"Client Id : {request.ClientId} " +
                                  $"Description: {request.Description}";

                    Logger.LogError($"OrderService: " + message, ex);

                    throw new HomeTaskException(ex.Message, ex);
                }

                InformUpdated(request, order);
            }
        }

        public async Task UpdateAsync(UpdateOrderRequest request)
        {
            Logger.Debug($"OrderService: Updating order id {request.Id} " +
                 $"Client Id : {request.ClientId} " +
                 $"Description: {request.Description}");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var order = EnsureOrderExistence(request.Id, unitOfWork);

                try
                {
                    _typeAdapter.Update(request, order);
                    await unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    var message = $"Failed to update order Id {request.Id}" +
                                  $"Client Id : {request.ClientId} " +
                                  $"Description: {request.Description}";

                    Logger.LogError($"OrderService: " + message, ex);

                    throw new HomeTaskException(ex.Message, ex);
                }

                InformUpdated(request, order);
            }
        }

        public void Delete(int orderId)
        {
            Logger.Debug($"OrderService: Deleting order Id {orderId}");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var order = EnsureOrderExistence(orderId, unitOfWork);

                try
                {
                    unitOfWork.OrderRepository.Delete(order);
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    var message = $"Failed to delete order Id {orderId}";
                    Logger.LogError($"OrderService: " + message, ex);

                    throw new HomeTaskException(ex.Message, ex);
                }

                InformDeleted(orderId, order);
            }
        }

        public async Task DeleteAsync(int orderId)
        {
            Logger.Debug($"OrderService: Deleting order Id {orderId} async");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var order = EnsureOrderExistence(orderId, unitOfWork);

                try
                {
                    unitOfWork.OrderRepository.Delete(order);
                    await unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    var message = $"Failed to delete order Id {orderId} async";
                    Logger.LogError($"OrderService: " + message, ex);

                    throw new HomeTaskException(ex.Message, ex);
                }

                InformDeleted(orderId, order);
            }
        }

        public IEnumerable<OrderDTO> GetForClient(int clientId)
        {
            Logger.Debug($"LotService: Retrieving orders for {clientId}");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var result = unitOfWork.OrderRepository.Find(OrderSpecifications.ByClientId(clientId));

                return result.Select(elem => _typeAdapter.Create<Order, OrderDTO>(elem));
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetForClientAsync(int clientId)
        {
            Logger.Debug($"LotService: Retrieving orders for {clientId} async");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var result = await unitOfWork.OrderRepository.FindAsync(OrderSpecifications.ByClientId(clientId));

                return result.Select(elem => _typeAdapter.Create<Order, OrderDTO>(elem));
            }
        }

        private static Order EnsureOrderExistence(int orderId, IUnitOfWork unitOfWork)
        {
            var order = unitOfWork.OrderRepository.FirstOrDefault(OrderSpecifications.ById(orderId));

            if (order == null)
            {
                var message = $"Unable to find order Id {orderId}";

                Logger.LogWarning($"OrderService: " + message);

                throw new NotFoundException(message);
            }
            return order;
        }

        private void InformUpdated(UpdateOrderRequest request, Order order)
        {
            Logger.LogInfo($"OrderService: Order id {request.Id} updated" +
                           $"Client Id : {request.ClientId} " +
                           $"Description: {request.Description}");

            _eventBus.Publish(_typeAdapter.Create<Order, OrderUpdated>(order));
        }

        private void InformDeleted(int orderId, Order order)
        {
            Logger.LogInfo($"OrderService: Order Id {orderId} deleted");

            _eventBus.Publish(_typeAdapter.Create<Order, OrderDeleted>(order));
        }

        private void InformCreated(CreateOrderRequest request, Order order)
        {
            Logger.LogInfo("OrderService: New order created for" +
                           $"Client Id : {request.ClientId} " +
                           $"Description: {request.Description}");

            _eventBus.Publish(_typeAdapter.Create<Order, OrderCreated>(order));
        }
    }
}