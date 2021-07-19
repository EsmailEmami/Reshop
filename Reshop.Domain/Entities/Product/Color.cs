using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }

        [Display(Name = "نام رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ColorName { get; set; }
    }
}
