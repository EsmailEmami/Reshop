using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class ReportQuestion
    {
        public ReportQuestion()
        {
        }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("ReportQuestionType")]
        public int ReportQuestionTypeId { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        #region Relations

        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
        public virtual ReportQuestionType ReportQuestionType { get; set; }

        #endregion
    }
}
