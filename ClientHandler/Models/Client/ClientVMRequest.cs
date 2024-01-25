﻿using System.ComponentModel.DataAnnotations;

namespace ClientHandler.Models.Client
{
    public class ClientVMRequest
    {
        [Required, Display(Name = ": الإسم")]
        public string Name { get; set; } = null!;
        [Required, RegularExpression(@"^[23]\d{13}$", ErrorMessage = "الرقم القومي يجب أن يتكون من 14 رقم ويبدأ ب 2 أو 3*")]
        [Display(Name = ": الرقم القومي")]
        public string NationalId { get; set; } = null!;
        [Required, RegularExpression(@"^01\d{9}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 11 رقم ويبدأ ب 01*")]
        [Display(Name = ": رقم الهاتف")]
        public string PhoneNumber { get; set; } = null!;
        [Required, Range(5000, 20000, ErrorMessage = "يجب أن يكون المرتب ما بين خمسة ألالاف و عشرون ألف*")]
        [Display(Name = ": المرتب")]
        public decimal Salary { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال المدينة*")]
        public int CityId { get; set; }
        public int VillageId { get; set; }
        [Required(ErrorMessage = "يجب أن يتكون الرقم السري من 7 خانات*"), DataType(DataType.Password), MinLength(7)]
        [Display(Name = ": الرقم السري")]
        public string Password { get; set; } = null!;
    }
}
