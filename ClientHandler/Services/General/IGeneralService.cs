using ClientHandler.Models.Client;
using ClientHandler.Models.Country;

namespace ClientHandler.Services.General
{
    public interface IGeneralService
    {
        Task<ClientVMResponse> CreateNewClientAsync(ClientVMRequest model);
        Task<IList<ClientVMResponse>> GetAllClientsAsync();
        Task<IList<ClientVMResponse>> GetClientByIdAsync(int id);
        Task<ClientVMResponse> GetClientByIdAsync(string id);
        Task<bool> DeleteClientByIdAsync(string nationalId);
        Task<ClientVMResponse> UpdateClientByIdAsync(string nationalId, UpdateVMRequest model);

        Task<IEnumerable<GovernorateVMResponse>> GetAllGovernoratesAsync();
        Task<IEnumerable<CityVMResponse>> GetAllCitysAsync(int goverenorateId);
        Task<IEnumerable<VillageVMResponse>> GetAllVillagesAsync(int cityId);

        Task<IList<ClientVMResponse>> searchAsync(string search);
    }
}
