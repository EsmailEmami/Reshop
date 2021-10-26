using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
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
        public virtual User User { get; set; }
        public virtual ReportQuestionAnswerType ReportQuestionAnswerType { get; set; }

        #endregion
    }
}
