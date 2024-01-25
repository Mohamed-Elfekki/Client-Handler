using ClientHandler.Entities;
using ClientHandler.Models;
using ClientHandler.Services.General;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection.Metadata.Ecma335;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using ClientHandler.Models.Client;
using DocumentFormat.OpenXml.InkML;
using System.Security.Claims;
using ClientHandler.Models.Identity;
using ClientHandler.Services.CustomIdentity;

namespace ClientHandler.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IGeneralService _generalService;
        private readonly IXLWorkbook _workbook;
        private readonly ClientHandlerDBContext _context;
        private readonly IIdentityService _identityService;

        public HomeController(IGeneralService generalService, IXLWorkbook workbook, ClientHandlerDBContext context, IIdentityService identityService)
        {
            _generalService = generalService;
            _workbook = workbook;
            _context = context;
            _identityService = identityService;
        }

        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _generalService.GetAllGovernoratesAsync();
            if (result != null)
            {
                GeneralViewModel generalVM = new GeneralViewModel
                {
                    governorates = result
                };
                return View(generalVM);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(GeneralViewModel model)
        {
            var govers = await _generalService.GetAllGovernoratesAsync();
            ClientVMRequest data = new ClientVMRequest
            {
                Name = model.Name,
                NationalId = model.NationalId,
                PhoneNumber = model.PhoneNumber,
                Salary = model.Salary,
                CityId = model.CityId,
                VillageId = model.VillageId,
                Password = model.Password,

            };
            if (ModelState.IsValid)
            {
                var result = await _generalService.CreateNewClientAsync(data);
                if (result != null)
                {

                    TempData["Success"] = "تمت إضافة بياناتك بنجاح";

                    loginVMRequest loginVM = new loginVMRequest
                    {
                        UserName = result.NationalId,
                        Password = model.Password
                    };

                    var user = await _identityService.Login(loginVM);
                    if (user != null)
                    {
                        return RedirectToAction("Main");
                    }
                }
            }
            TempData["Error"] = "لم تمت إضافة البيانات بنجاح، حاول مجددا";
            model.governorates = govers;
            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> GetAllCities(int govId)
        {
            var result = await _generalService.GetAllCitysAsync(govId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllVillages(int cityId)
        {
            var result = await _generalService.GetAllVillagesAsync(cityId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Main()
        {
            var result = await _generalService.GetClientByIdAsync(int.Parse(UserId));
            if (result != null)
            {
                return View(result);
            }
            TempData["Error"] = "لا توجد بيانات للعرض";
            return RedirectToAction("Login", "Identity");
        }


        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<IActionResult> Update(string nationalId)
        {
            var result = await _generalService.GetClientByIdAsync(nationalId);
            var govers = await _generalService.GetAllGovernoratesAsync();
            if (result != null || govers.Count() > 0)
            {
                UpdateVMRequest updateVMRequest = new UpdateVMRequest
                {
                    Name = result.Name,
                    Salary = result.Salary,
                    PhoneNumber = result.PhoneNumber,
                    NationalId = result.NationalId,
                    governorates = govers
                };

                return View(updateVMRequest);
            }
            TempData["Failed"] = "فشلت عملية تحديث البيانات ";
            return RedirectToAction("Main", "Home");
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<IActionResult> Update(string nationalId, UpdateVMRequest model)
        {
            var govers = await _generalService.GetAllGovernoratesAsync();
            if (ModelState.IsValid)
            {
                var result = await _generalService.UpdateClientByIdAsync(nationalId, model);
                if (result != null)
                {
                    TempData["Success"] = "تم تحديث البيانات بنجاح";
                    return RedirectToAction("Main", "Home", result);
                }
            }
            TempData["Error"] = "فشلت عملية التحديث";
            model.governorates = govers;
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string nationalId)
        {
            var result = await _generalService.DeleteClientByIdAsync(nationalId);
            if (result is true)
            {
                TempData["Success"] = "تم حذف العميل بنجاح";
                return RedirectToAction("Main", "Home");
            }
            TempData["Failed"] = "فشلت عملية حذف العميل";
            return RedirectToAction("Main", "Home");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportToExcel()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet";
            string fileName = "Users.xlsx";

            var result = await _generalService.GetAllClientsAsync();
            var worksheet = _workbook.Worksheets.Add("All Users");

            // Define a style for the header
            var headerStyle = _workbook.Style;
            headerStyle.Font.Bold = true;
            headerStyle.Fill.BackgroundColor = XLColor.LightBlue;
            headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Apply the style to the header cells
            for (int col = 1; col <= 8; col++)
            {
                worksheet.Cell(1, col).Style = headerStyle;
            }

            // Populate the header cells
            worksheet.Cell(1, 1).Value = "الإسم";
            worksheet.Cell(1, 2).Value = "الرقم القومي";
            worksheet.Cell(1, 3).Value = "الهاتف";
            worksheet.Cell(1, 4).Value = "المرتب";
            worksheet.Cell(1, 5).Value = "المحافظة";
            worksheet.Cell(1, 6).Value = "المدينة";
            worksheet.Cell(1, 7).Value = "القرية أو القسم";

            for (int i = 1; i <= result.Count; i++)
            {
                var row = i + 1;
                worksheet.Cell(row, 1).Value = result[i - 1].Name;
                worksheet.Cell(row, 2).Value = result[i - 1].NationalId;
                worksheet.Cell(row, 3).Value = result[i - 1].PhoneNumber;
                worksheet.Cell(row, 4).Value = result[i - 1].Salary;
                worksheet.Cell(row, 5).Value = result[i - 1].Governorate;
                worksheet.Cell(row, 6).Value = result[i - 1].City;
                worksheet.Cell(row, 7).Value = result[i - 1].Village;
            }

            var total = worksheet.Cell(1, 8);
            var totalVal = worksheet.Cell(2, 8);

            total.Value = "الإجمالي";
            totalVal.Value = result.Count;

            using (var ms = new MemoryStream())
            {
                _workbook.SaveAs(ms);
                return File(ms.ToArray(), contentType, fileName);
            }
        }


        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<IActionResult> Search(string search)
        {
            var result = await _generalService.searchAsync(search);
            if (result != null)
            {
                return View("Main", result);
            }
            TempData["Error"] = "لا توجد نتائج للمدخلات المعطاة";
            return View("Main");
        }

    }
}