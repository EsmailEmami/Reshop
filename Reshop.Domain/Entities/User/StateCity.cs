using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class StateCity
    {
        public StateCity()
        {
        }
        [ForeignKey("State")]
        public int StateId { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        #region Relations

        public virtual State State { get; set; }
        public virtual City City { get; set; }

        #endregion
    }
}
