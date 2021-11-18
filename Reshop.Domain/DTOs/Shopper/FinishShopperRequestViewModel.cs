using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Shopper
{
    public class FinishShopperRequestViewModel
    {
        [Required]
        public string RequestId { get; set; }
        public bool IsSuccess { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Reason { get; set; }
    }
}
