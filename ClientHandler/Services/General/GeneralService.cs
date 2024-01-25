using ClientHandler.Entities;
using ClientHandler.Models.Client;
using ClientHandler.Models.Country;
using ClientHandler.Services.CustomIdentity;
using Microsoft.EntityFrameworkCore;

namespace ClientHandler.Services.General
{
    public class GeneralService : IGeneralService
    {
        private readonly IIdentityService _identity;
        private readonly ClientHandlerDBContext _context;

        public GeneralService(IIdentityService identity, ClientHandlerDBContext context)
        {
            _identity = identity;
            _context = context;
        }
        public async Task<ClientVMResponse> CreateNewClientAsync(ClientVMRequest model)
        {
            var newUser = await _identity.CreateNewUserAsync(model.NationalId, model.Password, 1);
            if (newUser is true)
            {
                var userId = (int)await _identity.GetUserIdAsync(model.NationalId, model.Password);

                Client client = new Client
                {
                    Name = model.Name,
                    NationalId = model.NationalId,
                    PhoneNumber = model.PhoneNumber,
                    Salary = model.Salary,
                    UserId = userId,
                    CityId = model.CityId,
                    VillageId = model.VillageId
                };
                var newClient = await _context.Clients.AddAsync(client);
                if (newClient != null)
                {
                    var goverId = await _context.Cities.Where(x => x.Id == client.CityId).Select(x => x.GovernorateId).SingleOrDefaultAsync();
                    var gover = await _context.Governorates.Where(x => x.Id == goverId).Select(x => x.GovernorateName).SingleOrDefaultAsync();
                    var city = await _context.Cities.Where(x => x.Id == client.CityId).Select(x => x.CityName).SingleOrDefaultAsync();
                    var village = await _context.Villages.Where(x => x.Id == client.VillageId).Select(x => x.VillageName).SingleOrDefaultAsync();
                    ClientVMResponse clientVM = new ClientVMResponse
                    {
                        Name = client.Name,
                        NationalId = client.NationalId,
                        PhoneNumber = client.PhoneNumber,
                        Salary = client.Salary,
                        Governorate = gover,
                        City = city,
                        Village = village
                    };
                    _context.SaveChanges();
                    return clientVM;
                }
            }
            return null;
        }

        public async Task<IList<ClientVMResponse>> GetAllClientsAsync()
        {
            var result = await _context.Clients.ToListAsync(); /*_context.Users.ToListAsync();*/
            if (result.Count() > 0)
            {
                IList<ClientVMResponse> clients = new List<ClientVMResponse>();
                foreach (var item in result)
                {
                    var city = await _context.Cities.Where(x => x.Id == item.CityId).SingleOrDefaultAsync();
                    var governorate = await _context.Governorates.Where(x => x.Id == city.GovernorateId).Select(x => x.GovernorateName).SingleOrDefaultAsync();
                    string village = null;
                    if (item.VillageId != null)
                    {
                        village = await _context.Villages.Where(x => x.Id == item.VillageId).Select(x => x.VillageName).SingleOrDefaultAsync();
                    }
                    ClientVMResponse clientVMResponse = new ClientVMResponse
                    {
                        Name = item.Name,
                        NationalId = item.NationalId,
                        PhoneNumber = item.PhoneNumber,
                        Salary = item.Salary,
                        Governorate = governorate,
                        City = city.CityName,
                        Village = village
                    };
                    clients.Add(clientVMResponse);
                }
                return clients;
            }
            return null;
        }

        public async Task<IList<ClientVMResponse>> GetClientByIdAsync(int id)
        {
            var user = await _context.Users.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (user != null)
            {
                if (user.RoleId != 1)
                {
                    var result = await _context.Clients.Include(x => x.City).ToListAsync();
                    IList<ClientVMResponse> clientVMResponses = new List<ClientVMResponse>();
                    foreach (var client in result)
                    {
                        var governorate = await _context.Governorates.Where(x => x.Id == client.City.GovernorateId).SingleOrDefaultAsync();
                        Village village = new Village();
                        if (client.VillageId != null)
                        {
                            village = await _context.Villages.Where(x => x.Id == client.VillageId).SingleOrDefaultAsync();
                        }
                        ClientVMResponse clientVM = new ClientVMResponse
                        {
                            Name = client.Name,
                            NationalId = client.NationalId,
                            PhoneNumber = client.PhoneNumber,
                            Salary = client.Salary,
                            Governorate = governorate.GovernorateName,
                            City = client.City.CityName,
                            Village = village.VillageName
                        };
                        clientVMResponses.Add(clientVM);
                    }
                    return clientVMResponses;
                }
                else
                {
                    var result = await _context.Clients.Include(x => x.City).Include(x => x.Village).Where(x => x.UserId == id).SingleOrDefaultAsync();
                    //var city = await _context.Cities.Where(x => x.Id == result.CityId).SingleOrDefaultAsync();
                    var governorate = await _context.Governorates.Where(x => x.Id == result.City.GovernorateId).SingleOrDefaultAsync();
                    Village village = new Village();
                    if (result.VillageId != null)
                    {
                        village = await _context.Villages.Where(x => x.Id == result.VillageId).SingleOrDefaultAsync();
                    }
                    IList<ClientVMResponse> clientVMResponses = new List<ClientVMResponse>();
                    ClientVMResponse client = new ClientVMResponse
                    {
                        Name = result.Name,
                        NationalId = result.NationalId,
                        PhoneNumber = result.PhoneNumber,
                        Salary = result.Salary,
                        Governorate = governorate.GovernorateName,
                        City = result.City.CityName,
                        Village = village.VillageName
                    };
                    clientVMResponses.Add(client);
                    return clientVMResponses;
                }
            }
            return null;
        }

        public async Task<ClientVMResponse> GetClientByIdAsync(string nationalId)
        {

            var result = await _context.Clients.Where(x => x.NationalId == nationalId).SingleOrDefaultAsync();
            if (result != null)
            {
                var city = await _context.Cities.Where(x => x.Id == result.CityId).SingleOrDefaultAsync();
                var governorate = await _context.Governorates.Where(x => x.Id == city.GovernorateId).SingleOrDefaultAsync();
                Village village = new Village();
                if (result.VillageId != null)
                {
                    village = await _context.Villages.Where(x => x.Id == result.VillageId).SingleOrDefaultAsync();
                }
                ClientVMResponse client = new ClientVMResponse
                {
                    Name = result.Name,
                    NationalId = result.NationalId,
                    PhoneNumber = result.PhoneNumber,
                    Salary = result.Salary,
                    Governorate = governorate.GovernorateName,
                    City = city.CityName,
                    Village = village.VillageName
                };
                return client;
            }
            return null;
        }

        public async Task<ClientVMResponse> UpdateClientByIdAsync(string nationalId, UpdateVMRequest model)
        {
            var client = await _context.Clients.Where(x => x.NationalId == nationalId).SingleOrDefaultAsync();
            if (client != null)
            {

                client.Name = model.Name;
                client.Salary = model.Salary;
                client.CityId = model.CityId;
                client.VillageId = model.VillageId;
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                var user = await _context.Users.Where(x => x.Id == client.UserId).SingleOrDefaultAsync();
                if (user != null)
                {
                    var city = await _context.Cities.Where(x => x.Id == client.CityId).SingleOrDefaultAsync();
                    var governorate = await _context.Governorates.Where(x => x.Id == city.GovernorateId).SingleOrDefaultAsync();
                    Village village = new Village();
                    if (client.VillageId != null)
                    {
                        village = await _context.Villages.Where(x => x.Id == client.VillageId).SingleOrDefaultAsync();
                    }
                    ClientVMResponse response = new ClientVMResponse()
                    {
                        NationalId = nationalId,
                        Name = model.Name,
                        Salary = model.Salary,
                        PhoneNumber = model.PhoneNumber,
                        Governorate = governorate.GovernorateName,
                        City = city.CityName,
                        Village = village.VillageName
                    };
                    return response;
                }
            }
            return null;
        }

        public async Task<bool> DeleteClientByIdAsync(string nationalId)
        {
            var client = await _context.Clients.Where(x => x.NationalId == nationalId).SingleOrDefaultAsync();

            if (client != null)
            {
                var user = await _context.Users.Where(x => x.Id == client.UserId).SingleOrDefaultAsync();
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<GovernorateVMResponse>> GetAllGovernoratesAsync()
        {
            var result = await _context.Governorates.OrderBy(x => x.GovernorateName).ToListAsync();
            if (result.Count() > 0)
            {
                IList<GovernorateVMResponse> governorates = new List<GovernorateVMResponse>();
                foreach (var item in result)
                {
                    GovernorateVMResponse governorate = new GovernorateVMResponse
                    {
                        GovernorateId = item.Id,
                        GovernorateName = item.GovernorateName
                    };

                    governorates.Add(governorate);
                }
                return governorates;
            }
            return null;
        }

        public async Task<IEnumerable<CityVMResponse>> GetAllCitysAsync(int goverenorateId)
        {
            var result = await _context.Cities.Where(x => x.GovernorateId == goverenorateId).OrderBy(x => x.CityName).ToListAsync();
            if (result.Count() > 0)
            {
                IList<CityVMResponse> cities = new List<CityVMResponse>();
                foreach (var item in result)
                {
                    CityVMResponse city = new CityVMResponse
                    {
                        CityId = item.Id,
                        CityName = item.CityName
                    };

                    cities.Add(city);
                }
                return cities;
            }
            return null;
        }

        public async Task<IEnumerable<VillageVMResponse>> GetAllVillagesAsync(int cityId)
        {
            var result = await _context.Villages.Where(x => x.CityId == cityId).ToListAsync();
            if (result.Count() > 0)
            {
                IList<VillageVMResponse> villages = new List<VillageVMResponse>();
                foreach (var item in result)
                {
                    VillageVMResponse village = new VillageVMResponse
                    {
                        VillageId = item.Id,
                        VillageName = item.VillageName
                    };

                    villages.Add(village);
                }
                return villages;
            }
            return null;
        }

        public async Task<IList<ClientVMResponse>> searchAsync(string search)
        {
            var result = await _context.Clients.Include(x => x.City).Where(x => x.Name.Contains(search)).ToListAsync();
            IList<ClientVMResponse> clientVMResponses = new List<ClientVMResponse>();
            foreach (var client in result)
            {
                var governorate = await _context.Governorates.Where(x => x.Id == client.City.GovernorateId).SingleOrDefaultAsync();
                Village village = new Village();
                if (client.VillageId != null)
                {
                    village = await _context.Villages.Where(x => x.Id == client.VillageId).SingleOrDefaultAsync();
                }
                ClientVMResponse clientVM = new ClientVMResponse
                {
                    Name = client.Name,
                    NationalId = client.NationalId,
                    PhoneNumber = client.PhoneNumber,
                    Salary = client.Salary,
                    Governorate = governorate.GovernorateName,
                    City = client.City.CityName,
                    Village = village.VillageName
                };
                clientVMResponses.Add(clientVM);
            }
            return clientVMResponses;
        }
    }
}
