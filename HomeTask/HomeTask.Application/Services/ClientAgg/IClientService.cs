using System.Collections.Generic;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.Services.Base;

namespace HomeTask.Application.Services.ClientAgg
{
    public interface IClientService
    {
        int Create(CreateClientRequest request);
        void Delete(int clientId);
        IEnumerable<ClientDTO> GetAll();
    }
}