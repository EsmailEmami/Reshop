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


        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(8, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\d]{6,20}$", ErrorMessage = "نام کاربری وارد شده باید شامل حروف و اعداد باشد.")]
        public string UserName { get; set; }


        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        [Phone]
        [Remote("IsPhoneInUse", "Account", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        public string PhoneNumber { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$", ErrorMessage = "رمز عبور ایمنی رو وارد کنید.")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "لطفا {0} خود را درست وارد کنید")]
        public string ConfirmPassword { get; set; }
    }
}
