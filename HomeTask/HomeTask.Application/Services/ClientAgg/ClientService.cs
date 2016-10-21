using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.Exceptions;
using HomeTask.Application.TypeAdapter;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Contracts.Events.Client;
using HomeTask.Domain.Specifications;
using HomeTask.Infrastructure.Logging.Base;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.Persistence.UnitOfWork;

namespace HomeTask.Application.Services.ClientAgg
{
    public class ClientService : IClientService
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLog();

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ITypeAdapter _typeAdapter;
        private readonly IEventBus _eventBus;

        public ClientService(
            IUnitOfWorkFactory unitOfWorkFactory,
            ITypeAdapter typeAdapter,
            IEventBus eventBus)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _typeAdapter = typeAdapter;
            _eventBus = eventBus;
        }

        public int Create(CreateClientRequest request)
        {
            Logger.Debug("LotService: Creating new client " +
                         $"Address : {request.Address} " +
                         $"VIP: {request.VIP}");

            var client = _typeAdapter.Create<CreateClientRequest, Client>(request);

            IUnitOfWork unitOfWork = null;

            try
            {
                unitOfWork = _unitOfWorkFactory.CreateScope();

                unitOfWork.ClientRepository.Insert(client);
                unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                var message = "Failed to create new client " +
                              $"Address : {request.Address} " +
                              $"VIP: {request.VIP}";

                Logger.LogError("LotService: " + message, ex);

                throw new HomeTaskException(message, ex);
            }
            finally
            {
                unitOfWork?.Dispose();
            }

            InformCreated(request, client);

            return client.Id;
        }

        // fake-async
        public async Task<int> CreateAsync(CreateClientRequest request)
        {
            Logger.Debug("LotService: Creating new client async" +
                         $"Address : {request.Address} " +
                         $"VIP: {request.VIP}");

            var client = _typeAdapter.Create<CreateClientRequest, Client>(request);

            IUnitOfWork unitOfWork = null;

            try
            {
                unitOfWork = _unitOfWorkFactory.CreateScope();

                unitOfWork.ClientRepository.Insert(client);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                var message = "Failed to create new client async" +
                              $"Address : {request.Address} " +
                              $"VIP: {request.VIP}";

                Logger.LogError("LotService: " + message, ex);

                throw new HomeTaskException(message, ex);
            }
            finally
            {
                unitOfWork?.Dispose();
            }

            InformCreated(request, client);

            return client.Id;
        }

        public void Delete(int clientId)
        {
            Logger.Debug($"LotService: Deleting client Id {clientId}");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var client = EnsureClientExistence(clientId, unitOfWork);

                try
                {
                    unitOfWork.ClientRepository.Delete(client);
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    var message = $"Failed to delete client Id {clientId}";
                    Logger.LogError($"LotService: " + message, ex);

                    throw new HomeTaskException(ex.Message, ex);
                }

                InformDeleted(clientId, client);
            }
        }

        public async Task DeleteAsync(int clientId)
        {
            Logger.Debug($"LotService: Deleting client Id {clientId} async");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var client = EnsureClientExistence(clientId, unitOfWork);

                try
                {
                    unitOfWork.ClientRepository.Delete(client);
                    await unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    var message = $"Failed to delete client Id {clientId} async";
                    Logger.LogError($"LotService: " + message, ex);

                    throw new HomeTaskException(ex.Message, ex);
                }

                InformDeleted(clientId, client);
            }
        }

        public IEnumerable<ClientDTO> GetAll()
        {
            Logger.Debug("LotService: Retrieving all the clients");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var result = unitOfWork.ClientRepository.Find(ClientSpecifications.Any());

                return result.Select(elem => _typeAdapter.Create<Client, ClientDTO>(elem));
            }
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            Logger.Debug("LotService: Retrieving all the clients async");

            using (var unitOfWork = _unitOfWorkFactory.CreateScope())
            {
                var result = await unitOfWork.ClientRepository.FindAsync(ClientSpecifications.Any());

                return result.Select(elem => _typeAdapter.Create<Client, ClientDTO>(elem));
            }
        }

        private static Client EnsureClientExistence(int clientId, IUnitOfWork unitOfWork)
        {
            var client = unitOfWork.ClientRepository.FirstOrDefault(ClientSpecifications.ById(clientId));

            if (client == null)
            {
                var message = $"Unable to find client Id {clientId}";

                Logger.LogWarning($"LotService: " + message);

                throw new NotFoundException(message);
            }

            return client;
        }

        private void InformCreated(CreateClientRequest request, Client client)
        {
            Logger.LogInfo("LotService: New client created " +
                           $"Address : {request.Address} " +
                           $"VIP: {request.VIP}");

            _eventBus.Publish(_typeAdapter.Create<Client, ClientCreated>(client));
        }

        private void InformDeleted(int clientId, Client client)
        {
            Logger.LogInfo($"LotService: Client Id {clientId} deleted");

            _eventBus.Publish(_typeAdapter.Create<Client, ClientDeleted>(client));
        }
    }
}