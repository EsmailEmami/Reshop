using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class ProductQuestionsForShow
    {
        public int QuestionId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public DateTime QuestionDate { get; set; }
        public string QuestionTitle { get; set; }
        public int AnswersCount { get; set; }
    }
}
