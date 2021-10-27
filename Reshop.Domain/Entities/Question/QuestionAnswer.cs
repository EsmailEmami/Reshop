using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Question
{
    public class QuestionAnswer
    {
        public QuestionAnswer()
        {

        }

        [Key]
        public int QuestionAnswerId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [Display(Name = "متن جواب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AnswerText { get; set; }

        [Display(Name = "تاریخ ثبت نظر")]
        public DateTime QuestionAnswerDate { get; set; }

        public bool IsDelete { get; set; } = false;

        [Display(Name = "دلیل حذف")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DeleteDescription { get; set; }

        #region Relations

        public virtual Question Question { get; set; }
        public virtual User.User User { get; set; }
        public virtual ICollection<QuestionAnswerLike> QuestionAnswerLikes { get; set; }
        public virtual ICollection<ReportQuestionAnswer> Reports { get; set; }

        #endregion
    }
}
