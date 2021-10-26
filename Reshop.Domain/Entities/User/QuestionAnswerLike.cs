using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class QuestionAnswerLike
    {
        public QuestionAnswerLike()
        {
        }

        [ForeignKey("QuestionAnswer")]
        public int QuestionAnswerId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        #region Relation

        public virtual QuestionAnswer QuestionAnswer { get; set; }
        public virtual User User { get; set; }

        #endregion
    }
}
