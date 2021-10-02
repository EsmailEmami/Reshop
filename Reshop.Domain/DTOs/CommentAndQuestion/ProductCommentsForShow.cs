using System;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class ProductCommentsForShow
    {
        public int CommentId { get; set; }
        public string StoreName { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string ColorName { get; set; }
        public string ProductShortKey { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentTitle { get; set; }
        public string CommentText { get; set; }
        public int LikeCount { get; set; }
    }
}