using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.User
{
    public class UserInformationViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }

        public UserInformationViewModel(string userId, string fullName, string phoneNumber)
        {
            UserId = userId;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
    }
}
