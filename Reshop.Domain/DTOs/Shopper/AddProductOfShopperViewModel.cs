using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class AddProductOfShopperViewModel
    {
        [Required]
        public string ShopperProductId { get; set; }

        [Required]
        public int ColorId { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int QuantityInStock { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public decimal Price { get; set; }
    }
}
