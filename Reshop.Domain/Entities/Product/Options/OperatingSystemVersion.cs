using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product.Options;

public class OperatingSystemVersion
{
    public OperatingSystemVersion()
    {
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string OperatingSystemVersionId { get; set; }

    [Display(Name = "سیستم عامل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string OperatingSystemVersionName { get; set; }

    [ForeignKey("OperatingSystem")]
    public string OperatingSystemId { get; set; }

    #region Relations

    public virtual OperatingSystem OperatingSystem { get; set; }

    #endregion
}