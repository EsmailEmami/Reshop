using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Question
{
    public class ReportQuestionAnswer
    {
        public ReportQuestionAnswer()
        {
        }

        [ForeignKey("QuestionAnswer")]
        public int QuestionAnswerId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("ReportQuestionAnswerType")]
        public int ReportQuestionAnswerTypeId { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        #region Relations

        public virtual QuestionAnswer QuestionAnswer { get; set; }
        public virtual User.User User { get; set; }
        public virtual ReportQuestionAnswerType ReportQuestionAnswerType { get; set; }

        #endregion
    }
}
