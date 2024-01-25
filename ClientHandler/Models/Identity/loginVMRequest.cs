using System.ComponentModel.DataAnnotations;

namespace ClientHandler.Models.Identity
{
    public class loginVMRequest
    {
        [Required(ErrorMessage = "الرجاء إدخال الرقم القومي"), Display(Name = "الرقم القومي")]
        public string UserName { get; set; } = null!;

        [Display(Name = ": الرقم السري")]
        [Required(ErrorMessage = "يجب أن يتكون الرقم السري من 7 خانات"), DataType(DataType.Password), MinLength(7, ErrorMessage = "يجب أن يتكون الرقم السري من 7 خانات")]
        public string Password { get; set; } = null!;

        public bool RememberLogin { get; set; } = true;
    }
}
