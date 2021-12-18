using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Reshop.Domain.DTOs.User
{
    public class RegisterViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FullName { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        [Remote("IsPhoneNumberValid", "Account", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        public string PhoneNumber { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&*-]).{6,50}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string Password { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Compare("Password",ErrorMessage = "رمز عبور وارد شده مطابقت ندارد.")]
        [RegularExpression(@"^(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&*-]).{6,50}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string ConfirmPassword { get; set; }
    }
}
