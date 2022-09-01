using MarketPlace.Data.Entities.ProductOrder;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Application.Extensions
{
    public static class PriceExtensions
    {
        public static long GetProductPrice(this Product product)
        {
            if (product.DiscountPercent == 0)
                return product.Price;

            return product.Price - (product.Price * product.DiscountPercent / 100);
        }
        
        public static long GetProductPriceInQuery(this long price, int discount)
        {
            return price * (discount == 0?1:discount) / 100;
        }

        public static long GetOpenOrderDetailWithDiscount(this UserOpenOrderItemDTO detail)
        {
            return detail.Product.GetProductPrice() / detail.Count; 
        }
    }
}
