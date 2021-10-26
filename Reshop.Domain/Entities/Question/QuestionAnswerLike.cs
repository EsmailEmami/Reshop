using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Question
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
        public virtual User.User User { get; set; }

        #endregion
    }
}
