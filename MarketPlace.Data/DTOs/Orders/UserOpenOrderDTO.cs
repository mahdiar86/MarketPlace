using MarketPlace.Data.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Orders
{
    public class UserOpenOrderDTO
    {
        public long UserId { get; set; }

        public string Description { get; set; }

        public List<UserOpenOrderItemDTO> Details { get; set; }
    }

    public enum ChangeOrderDetailCountResult
    {
        Success,
        QuantityIsLowerThanCount,
        Error
    }
}
