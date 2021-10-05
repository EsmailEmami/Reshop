using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class CommentsAverageViewModel
    {
        public int ProductSatisfaction { get; set; }

        public int ConstructionQuality { get; set; }

        public int FeaturesAndCapabilities { get; set; }

        public int DesignAndAppearance { get; set; }
    }
}