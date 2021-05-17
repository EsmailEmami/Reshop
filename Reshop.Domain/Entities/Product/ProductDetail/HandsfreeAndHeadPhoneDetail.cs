using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class HandsfreeAndHeadPhoneDetail
    {
        [Key]
        public int HeadPhoneDetailId { get; set; }

        [Display(Name = "نوع اتصال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ConnectionType { get; set; }

        [Display(Name = "نوع گوشی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PhoneType { get; set; }

        [Display(Name = "مناسب برای")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string WorkSuggestion { get; set; }

        [Display(Name = "اتصال دهنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connector { get; set; }

        [Display(Name = "باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportBattery { get; set; }


        [Display(Name = "ویژگی ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }
    }
}
