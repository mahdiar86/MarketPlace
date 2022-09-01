using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.Entities.Products
{
    public class ProductDiscountUsage : BaseEntity
    {
        #region Properties

        public long ProductDiscountId { get; set; }

        public long UserId { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public ProductDiscount productDiscount { get; set; }

        #endregion
    }
}
