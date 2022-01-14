using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product.Options;

public class Chipset
{
    public Chipset()
    {
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ChipsetId { get; set; }

    [Display(Name = "نام تراشه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string ChipsetName { get; set; }

    #region Relations

    public virtual ICollection<Cpu> Cpus { get; set; }
    public virtual ICollection<Gpu> Gpus { get; set; }

    #endregion
}