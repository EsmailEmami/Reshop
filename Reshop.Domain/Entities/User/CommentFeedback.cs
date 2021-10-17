﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class CommentFeedback
    {
        public CommentFeedback()
        {
        }

        [Key]
        public int LikeCommentId { get; set; }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public bool Type { get; set; } // true = like - false = dislike

        #region Relation

        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }

        #endregion
    }
}