using MarketPlace.Data.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.Entities.ProductOrder
{
    public class UserOpenOrderItemDTO
    {
        public long DetailId { get; set; }

        public long ProductId { get; set; }

        public int Count { get; set; }

        public long ProductPrice { get; set; }

        public Product Product { get; set; }

        public int? DiscountPercentage { get; set; }
    }
}
