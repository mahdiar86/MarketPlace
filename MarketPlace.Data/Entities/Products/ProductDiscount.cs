using MarketPlace.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.Entities.Products
{
    public class ProductDiscount : BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }
        
        [Range(0,100)]
        public int Percentage { get; set; }

        public DateTime ExpireDate { get; set; }

        public int DiscountNumber { get; set; }


        #endregion

        #region Relations 

        public Product Product { get; set; }
        public ICollection<ProductDiscountUsage> ProductDiscountUsages { get; set; }

        #endregion
    }
}
