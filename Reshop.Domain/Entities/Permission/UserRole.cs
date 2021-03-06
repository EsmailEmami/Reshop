namespace Reshop.Domain.Entities.Permission
{
    public class UserRole
    {
        public UserRole()
        {

        }

        public string UserId { get; set; }

        public string RoleId { get; set; }

        public UserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        #region Relations

        public virtual User.User User { get; set; }
        public virtual Role Role { get; set; }

        #endregion
    }
}
