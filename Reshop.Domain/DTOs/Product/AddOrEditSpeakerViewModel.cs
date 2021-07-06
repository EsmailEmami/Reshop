﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditSpeakerViewModel
    {
        [Required]
        public string ShopperUserId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Description { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1000, 1000000000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} تومان باشد")]
        [RegularExpression("([0-9]+)", ErrorMessage = "لطفا عدد وارد کنید")]
        public decimal Price { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "لطفا عدد وارد کنید")]
        public int QuantityInStock { get; set; }

        [Display(Name = "برند محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductBrand { get; set; }

        [Display(Name = "نام محصول برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandProduct { get; set; }

        [Display(Name = "نوع اتصال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ConnectionType { get; set; }

        [Display(Name = "اتصال دهنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connector { get; set; }

        [Display(Name = "نسخه بلوتوٍث")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double BluetoothVersion { get; set; }

        [Display(Name = "ورودی کارت حافظه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsMemoryCardInput { get; set; }

        [Display(Name = "باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportBattery { get; set; }

        [Display(Name = "پورت USB")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportUSBPort { get; set; }

        [Display(Name = "میکروفون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportMicrophone { get; set; }

        [Display(Name = "رادیو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportRadio { get; set; }
        public IEnumerable<IFormFile> SelectedImages { get; set; }
    }
}
