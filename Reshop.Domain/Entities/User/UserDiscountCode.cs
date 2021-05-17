namespace Reshop.Domain.Entities.User
{
    public class UserDiscountCode
    {
        public UserDiscountCode()
        {
            
        }

        public string UserId { get; set; }
        public string DiscountId { get; set; }

        #region relations

        public virtual User User { get; set; }
        public virtual Discount Discount { get; set; }

        #endregion
    }
}
