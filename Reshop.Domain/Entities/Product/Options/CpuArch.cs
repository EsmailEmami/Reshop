using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product.Options;

public class CpuArch
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CpuArchId { get; set; }

    [Display(Name = "نام پردازنده مرکزی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CpuArchName { get; set; }
}