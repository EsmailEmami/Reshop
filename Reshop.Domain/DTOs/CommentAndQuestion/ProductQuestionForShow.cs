using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class ProductQuestionForShow
    {
        public int QuestionId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public DateTime QuestionDate { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }
        public IEnumerable<QuestionAnswersForShow> Answers { get; set; }
    }

    public class QuestionAnswersForShow
    {
        public int QuestionAnswerId { get; set; }
        public string AnswerText { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}