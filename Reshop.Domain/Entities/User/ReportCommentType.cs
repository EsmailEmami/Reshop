using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.User
{
    public class ReportCommentType
    {
        [Key]
        public int ReportCommentTypeId { get; set; }

        [Display(Name = "عنوان ریپورت کامنت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ReportCommentTitle { get; set; }
    }
}
