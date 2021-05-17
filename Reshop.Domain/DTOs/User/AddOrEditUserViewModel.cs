﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.User
{
    public class AddOrEditUserViewModel
    {
        public string UserId { get; set; }


        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; } 

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        [DataType(DataType.PhoneNumber)]
        [Remote("IsPhoneInUse", "Account", HttpMethod = "POST",
            AdditionalFields = "__RequestVerificationToken")]
        public string PhoneNumber { get; set; }

        [Display(Name = "امتیاز")]
        public int Score { get; set; }

        [Display(Name = "موجودی حساب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(10000, 10000000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} تومان باشد")]
        public decimal AccountBalance { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NationalCode { get; set; } = "-";

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PostalCode { get; set; } = "-";

        [Display(Name = "وضعیت شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsPhoneNumberActive { get; set; }

        [Display(Name = "وضعیت مسدودیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsBlocked { get; set; }

        // role
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }

    }
}
