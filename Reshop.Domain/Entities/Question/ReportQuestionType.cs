using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Question
{
    public class ReportQuestionType
    {
        [Key]
        public int ReportQuestionTypeId { get; set; }

        [Display(Name = "عنوان ریپورت کامنت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ReportQuestionTitle { get; set; }
    }
}
