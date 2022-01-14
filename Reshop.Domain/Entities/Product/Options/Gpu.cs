using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product.Options;

public class Gpu
{
    public Gpu()
    {
        
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string GpuId { get; set; }

    [Display(Name = "نام پردازنده مرکزی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string GpuName { get; set; }

    [ForeignKey("Chipset")]
    public string ChipsetId { get; set; }

    #region Relations

    public virtual Chipset Chipset { get; set; }

    #endregion
}