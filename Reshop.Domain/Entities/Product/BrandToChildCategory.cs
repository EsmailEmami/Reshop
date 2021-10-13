using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Category;

namespace Reshop.Domain.Entities.Product
{
    public class BrandToChildCategory
    {
        [ForeignKey("brand")]
        public int BrandId { get; set; }
        [ForeignKey("ChildCategory")]
        public int ChildCategoryId { get; set; }

        #region Relations

        public virtual Brand Brand { get; set; }
        public virtual ChildCategory ChildCategory { get; set; }

        #endregion
    }
}
