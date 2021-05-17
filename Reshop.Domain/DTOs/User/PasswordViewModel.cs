using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.User
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Display(Name = "رمز عبور قبلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$", ErrorMessage = "رمز عبور ایمنی رو وارد کنید.")]
        [Remote("IsPasswordValid", "AccountManager", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        public string CurrentPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$", ErrorMessage = "رمز عبور ایمنی رو وارد کنید.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$", ErrorMessage = "رمز عبور ایمنی رو وارد کنید.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "لطفا {0} خود را درست وارد کنید")]
        public string ConfirmNewPassword { get; set; }
    }

    public class RecoveryPasswordViewModel
    {
        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [RegularExpression(@"^(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{11}$", ErrorMessage = "شماره تلفن خود را درست وارد کنید.")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
