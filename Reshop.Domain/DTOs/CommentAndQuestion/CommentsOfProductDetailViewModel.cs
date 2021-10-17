using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class CommentsOfProductDetailViewModel
    {
        public int AllCommentsCount { get; set; }

        public double CommentsScore { get; set; }

        public int SuggestedBuyersCounts { get; set; }

        public int SuggestedCommentsCounts { get; set; }

        // AVG
        public int ProductSatisfaction { get; set; }

        public int ConstructionQuality { get; set; }

        public int FeaturesAndCapabilities { get; set; }

        public int DesignAndAppearance { get; set; }
    }
}