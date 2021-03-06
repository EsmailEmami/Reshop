using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Question
{
    public class ReportQuestionAnswerType
    {
        [Key]
        public int ReportQuestionAnswerTypeId { get; set; }

        [Display(Name = "عنوان ریپورت کامنت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ReportQuestionAnswerTitle { get; set; }
    }
}
