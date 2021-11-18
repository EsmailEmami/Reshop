using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Shopper
{
    public class EditShopperProductViewModel
    {
        [Required]
        public string ShopperId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Display(Name = "گارانتی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Warranty { get; set; }

        public bool IsActive { get; set; }
    }
}
