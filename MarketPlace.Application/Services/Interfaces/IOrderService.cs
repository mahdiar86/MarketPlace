using System;
using System.Threading.Tasks;
using MarketPlace.Data.DTOs.Orders;
using MarketPlace.Data.Entities.ProductOrder;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IOrderService : IAsyncDisposable
    {
        Task<long> AddOrderForUser(long userId);
        Task<Order> GetUserLatestOpenOrder(long userId);
        Task AddProductToOpenOrder(AddProductToOrderDTO order, long userId);
        Task<UserOpenOrderDTO> GetUserOpenOrderDetail(long userId);
        Task<long> GetTotalOrderPriceForPayment(long userId);
        Task PayOrderProductPriceToSeller(long userId);
        Task<bool> RemoveOrderDetail(long detailId, long userId);
        Task<ChangeOrderDetailCountResult> ChangeOrderDetailCount(long detailId, int count, long userId);
        Task CloseUserOpenOrderAfterPayment(long userId, long tracingCode);
    }
}