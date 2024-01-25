using ClientHandler.Models;
using ClientHandler.Models.Identity;
using ClientHandler.Services.CustomIdentity;
using ClientHandler.Services.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientHandler.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IGeneralService _generalService;
        private readonly IIdentityService _identityService;

        public IdentityController(IGeneralService generalService, IIdentityService identityService)
        {
            _generalService = generalService;
            _identityService = identityService;
        }





        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(loginVMRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _identityService.Login(model);
                if (result != null)
                {
                    return LocalRedirect($"/Home/Main?id={result.Id}");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> LogOut()
        {
            await _identityService.LogOut();
            return LocalRedirect("/");
        }

    }
}
