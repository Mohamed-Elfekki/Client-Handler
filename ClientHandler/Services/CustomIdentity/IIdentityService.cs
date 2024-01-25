using ClientHandler.Entities;
using ClientHandler.Models.Identity;

namespace ClientHandler.Services.CustomIdentity
{
    public interface IIdentityService
    {
        Task<bool> CreateNewUserAsync(string userName, string password, int roleId);

        Task<int?> GetUserIdAsync(string userName, string password);

        Task<User> Login(loginVMRequest model);

        Task<bool> LogOut();
    }
}
