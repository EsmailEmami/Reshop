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

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&*-]).{6,50}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string Password { get; set; }
    }
}
