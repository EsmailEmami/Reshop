using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product.Options;

public class OperatingSystem
{
    public OperatingSystem()
    {
        
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string OperatingSystemId { get; set; }

    [Display(Name = "سیستم عامل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string OperatingSystemName { get; set; }

    #region Relations

    public virtual ICollection<OperatingSystemVersion> OperatingSystemVersions { get; set; }

    #endregion
}