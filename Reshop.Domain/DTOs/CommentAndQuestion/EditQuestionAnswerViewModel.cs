using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class EditQuestionAnswerViewModel
    {
        [Required]
        public int QuestionAnswerId { get; set; }

        [Display(Name = "متن جواب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AnswerText { get; set; }
    }
}
