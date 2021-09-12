using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Shopper
{
    public class AddColorToShopperProductViewModel
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

        public bool IsActive { get; set; }
    }
}
