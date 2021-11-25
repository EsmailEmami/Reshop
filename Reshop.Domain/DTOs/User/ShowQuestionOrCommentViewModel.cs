using System;

namespace Reshop.Domain.DTOs.User
{
    public class ShowQuestionOrCommentViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string QuestionOrCommentTitle { get; set; }

        public DateTime SentDate { get; set; }
    }
}
