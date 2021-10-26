using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Question
{
    public class QuestionLike
    {
        public QuestionLike()
        {
        }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        #region Relation

        public virtual Question Question { get; set; }
        public virtual User.User User { get; set; }

        #endregion
    }
}