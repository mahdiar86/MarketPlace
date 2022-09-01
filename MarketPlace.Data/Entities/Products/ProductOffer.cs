using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.Products
{
    public class ProductOffer : BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        public bool IsActive { get; set; }

        #endregion

        #region Relations

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        #endregion

    }
}
