using ClientHandler.Models.Country;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ClientHandler.Models.Client
{
    public class UpdateVMRequest
    {
        [Required, Display(Name = ": الإسم")]
        public string Name { get; set; } = null!;

        public string NationalId { get; set; } = null!;

        [Required, Phone, RegularExpression(@"^01\d{9}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 11 رقم ويبدأ ب 01*")]
        [Display(Name = ": رقم الهاتف")]
        public string PhoneNumber { get; set; } = null!;
        [Required, Range(5000, 20000, ErrorMessage = "يجب أن يكون المرتب ما بين خمسة ألالاف و عشرون ألف*")]
        [Display(Name = ": المرتب")]
        public decimal Salary { get; set; }

        [Display(Name = ": المحافظة")]
        [Required(ErrorMessage = "الرجاء إدخال المحافظة")]
        public int GovernorateId { get; set; }

        public IEnumerable<GovernorateVMResponse>? governorates { get; set; }

        [Display(Name = ": المدينة")]
        [Required(ErrorMessage = "الرجاء إدخال المدينة*")]
        public int CityId { get; set; }

        public IEnumerable<CityVMResponse>? citys { get; set; } = new List<CityVMResponse>();

        [Display(Name = ": الحي، القسم أو القرية")]
        [Required(ErrorMessage = "الرجاء إدخال الحى، القسم أو القرية*")]
        public int VillageId { get; set; }

        public IEnumerable<VillageVMResponse>? villages { get; set; } = new List<VillageVMResponse>();
    }
}
