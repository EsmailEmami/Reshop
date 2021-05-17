using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.User
{
    public class ShopperInformationViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }

        public bool Condition { get; set; }
    }
}
