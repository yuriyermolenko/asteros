using System;
using System.Collections.Generic;
using System.Linq;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.Exceptions;
using HomeTask.Application.Services.Base;
using HomeTask.Application.TypeAdapter;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Contracts.Events.Client;
using HomeTask.Domain.Specifications;
using HomeTask.Infrastructure.Logging.Base;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.Persistence.Repositories;
using MyDeals.DataAccess.UnitOfWork;

namespace HomeTask.Application.Services.ClientAgg
{
    public class ClientService : IClientService, IApplicationService
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLog();

        private readonly IRepository<Client> _clientRepository;
        private readonly ITypeAdapter _typeAdapter;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(
            IRepository<Client> clientRepository,
            ITypeAdapter typeAdapter,
            IEventBus eventBus,
            IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _typeAdapter = typeAdapter;
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
        }

        public int Create(CreateClientRequest request)
        {
            Logger.Debug("LotService: Creating new client " +
                         $"Address : {request.Address} " +
                         $"VIP: {request.VIP}");

            var client = _typeAdapter.Create<CreateClientRequest, Client>(request);

            try
            {
                _clientRepository.Insert(client);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var message = "Failed to create new client " +
                              $"Address : {request.Address} " +
                              $"VIP: {request.VIP}";

                Logger.LogError("LotService: " + message, ex);

                throw new HomeTaskException(message, ex);
            }

            Logger.LogInfo("LotService: New client created " +
                           $"Address : {request.Address} " +
                           $"VIP: {request.VIP}");

            _eventBus.Publish(_typeAdapter.Create<Client, ClientCreated>(client));

            return client.Id;
        }

        public void Delete(int clientId)
        {
            Logger.Debug($"LotService: Deleting client Id {clientId}");

            var client = _clientRepository.FirstOrDefault(ClientSpecifications.ById(clientId));

            if (client == null)
            {
                var message = $"Unable to find client Id {clientId}";

                Logger.LogWarning($"LotService: " + message);

                throw new NotFoundException(message);
            }

            try
            {
                _clientRepository.Delete(client);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var message = $"Failed to delete client Id {clientId}";
                Logger.LogError($"LotService: " + message, ex);

                throw new HomeTaskException(ex.Message, ex);
            }

            Logger.LogInfo($"LotService: Client Id {clientId} deleted");

            _eventBus.Publish(_typeAdapter.Create<Client, ClientDeleted>(client));
        }

        public IEnumerable<ClientDTO> GetAll()
        {
            Logger.Debug("LotService: Retrieving all the clients");

            var result = _clientRepository.Find(ClientSpecifications.Any());

            return result.Select(elem => _typeAdapter.Create<Client, ClientDTO>(elem));
        }
    }
}