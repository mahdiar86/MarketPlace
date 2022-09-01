using MarketPlace.Data.Entities.Common;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Data.Entities.ProductOrder
{
    public class OrderDetail : BaseEntity
    {
        #region Properties

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public int Count { get; set; }

        public long ProductPrice { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }
        public Order Order { get; set; }

        #endregion
    }
}
