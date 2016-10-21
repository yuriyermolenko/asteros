using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.Services.Base;

namespace HomeTask.Application.Services.ClientAgg
{
    public interface IClientService : IApplicationService
    {
        int Create(CreateClientRequest request);
        Task<int> CreateAsync(CreateClientRequest request);
        void Delete(int clientId);
        Task DeleteAsync(int clientId);
        IEnumerable<ClientDTO> GetAll();
        Task<IEnumerable<ClientDTO>> GetAllAsync();
    }
}