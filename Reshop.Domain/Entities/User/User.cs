using Reshop.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class User
    {
        public User()
        {

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string? Email { get; set; }

        [Display(Name = "آواتار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UserAvatar { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد دعوت")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string InviteCode { get; set; }

        [Display(Name = "تعداد دعوت شده ها")]
        public int InviteCount { get; set; }

        [Display(Name = "امتیاز")]
        public int Score { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NationalCode { get; set; }

        [Display(Name = "موجودی حساب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(10000, 10000000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} تومان باشد")]
        public decimal AccountBalance { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime RegisteredDate { get; set; }

        [Display(Name = "وضعیت مسدودیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsBlocked { get; set; }

        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
        public virtual ICollection<UserDiscountCode> UserDiscountCodes { get; set; }
        public virtual ICollection<UserInvite> UserInvites { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
        public virtual ICollection<CommentFeedback> CommentFeedBacks { get; set; }
        public virtual ICollection<QuestionLike> QuestionLikes { get; set; }
        public virtual ICollection<QuestionAnswerLike> QuestionAnswerLikes { get; set; }
        public virtual ICollection<ReportQuestion> QuestionReports { get; set; }
        public virtual ICollection<ReportQuestionAnswer> QuestionAnswerReports { get; set; }

        #endregion
    }
}
