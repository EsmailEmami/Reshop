using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.User
{
    public class LoginViewModel
    {
        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
