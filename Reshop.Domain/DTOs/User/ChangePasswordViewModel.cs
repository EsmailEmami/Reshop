using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Reshop.Domain.DTOs.User
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&*-]).{6,50}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        [Remote("IsPasswordValid", "Account", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        public string Password { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&*-]).{6,50}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Compare("Password", ErrorMessage = "رمز عبور وارد شده مطابقت ندارد.")]
        [RegularExpression(@"^(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&*-]).{6,50}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string ConfirmNewPassword { get; set; }
    }
}