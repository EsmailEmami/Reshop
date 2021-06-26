using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class TabletDetail
    {
        [Key]
        public int TabletDetailId { get; set; }

        [Display(Name = "حافظه داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string InternalMemory { get; set; }

        [Display(Name = "مقدار رم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RAMValue { get; set; }

        [Display(Name = "قابلیت مکالمه")]
        [Required]
        public bool IsTalkAbility { get; set; }

        [Display(Name = "اندازه موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Size { get; set; }


        [Display(Name = "شبکه های ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommunicationNetworks { get; set; }

        [Display(Name = "ویژگی ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }

        [Display(Name = "قابلیت پشتیبانی از سیم کارت")]
        [Required]
        public bool IsSIMCardSupporter { get; set; }

        [Display(Name = "تعداد سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public byte QuantitySIMCard { get; set; }

        [Display(Name = "نسخه سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OperatingSystemVersion { get; set; } 

        [Display(Name = "فناوری های ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommunicationTechnologies { get; set; }


        [Display(Name = "درگاه های ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommunicationPorts { get; set; }
    }
}
