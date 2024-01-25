using ClientHandler.Entities;
using ClientHandler.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ClientHandler.Services.CustomIdentity
{
    public class IdentityService : IIdentityService
    {
        private readonly ClientHandlerDBContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public IdentityService(ClientHandlerDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private string hashingPassword(string password)
        {

            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        public async Task<bool> CreateNewUserAsync(string nationalId, string password, int roleId)
        {
            var isUserNameExist = await _context.Users.Where(x => x.Username == nationalId).SingleOrDefaultAsync();
            if (isUserNameExist == null)
            {
                // var hashedPassword = hashingPassword(password);
                User user = new User
                {
                    Username = nationalId,
                    HashedPassword = password,
                    RoleId = roleId
                };
                var result = await _context.Users.AddAsync(user);
                if (result != null)
                {
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public async Task<int?> GetUserIdAsync(string userName, string password)
        {
            //var hashedPassword = hashingPassword(password);
            var result = await _context.Users.Where(x => x.Username == userName && x.HashedPassword == password).SingleOrDefaultAsync();
            if (result != null)
            {
                return result.Id;
            }
            return null;
        }

        public async Task<User> Login(loginVMRequest model)
        {
            var user = await _context.Users.Include(x => x.Clients).Include(x => x.Role).Where(x => x.Username == model.UserName && x.HashedPassword == model.Password).SingleOrDefaultAsync();
            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.RoleName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = model.RememberLogin
                });
                return user;
            }
            return null;
        }


        public async Task<bool> LogOut()
        {
            await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }

    }
}
