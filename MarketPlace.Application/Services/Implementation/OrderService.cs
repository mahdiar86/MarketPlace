using System;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Orders;
using MarketPlace.Data.Entities.ProductOrder;
using MarketPlace.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        #region Constructor

        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly ISellerWalletService _sellerWalletService;
        private readonly IProductService _productService;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository, ISellerWalletService sellerWalletService, IProductService productService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _sellerWalletService = sellerWalletService;
            _productService = productService;
        }

        public OrderService()
        {
        }

        #endregion

        #region Add Order

        public async Task<long> AddOrderForUser(long userId)
        {
            var order = new Order { UserId = userId, Description = "", TracingCode = "" };

            await _orderRepository.AddEntity(order);
            await _orderRepository.SaveChanges();

            return order.Id;
        }

        #endregion

        #region Get Latest Order Open

        public async Task<Order> GetUserLatestOpenOrder(long userId)
        {
            if (!await _orderRepository.GetQuery().AnyAsync(s => s.UserId == userId && !s.IsPaid))
                await AddOrderForUser(userId);

            var userOpenOrder = await _orderRepository.GetQuery()
                .Include(s => s.OrderDetails)
                .ThenInclude(t => t.Product)
                .ThenInclude(o => o.Seller)
                .SingleOrDefaultAsync(s => s.UserId == userId && !s.IsPaid);

            return userOpenOrder;
        }

        #endregion

        #region Add Product To Open Order

        public async Task AddProductToOpenOrder(AddProductToOrderDTO order, long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            var similarOrder = openOrder.OrderDetails.SingleOrDefault(s =>
                s.ProductId == order.ProductId);

            if (similarOrder == null)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = openOrder.Id,
                    ProductId = order.ProductId,
                    Count = order.Count,
                };

                await _orderDetailRepository.AddEntity(orderDetail);
                await _orderDetailRepository.SaveChanges();
            }
            else
            {
                similarOrder.Count++;

                _orderDetailRepository.EditEntity(similarOrder);
                await _orderDetailRepository.SaveChanges();
            }
        }

        #endregion

        #region Get User Open Order Detail

        public async Task<UserOpenOrderDTO> GetUserOpenOrderDetail(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);

            return new UserOpenOrderDTO
            {
                UserId = userId,
                Description = userOpenOrder.Description,
                Details = userOpenOrder.OrderDetails.Select(s => new UserOpenOrderItemDTO
                {
                    Count = s.Count,
                    ProductId = s.ProductId,
                    ProductPrice = s.ProductPrice,
                    Product = s.Product,
                    DetailId = s.Id
                }).ToList()
            };
        }

        #endregion

        #region Get Total Order Price

        public async Task<long> GetTotalOrderPriceForPayment(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);
            long totalPrice = 0;

            foreach (var detail in userOpenOrder.OrderDetails)
            {
                totalPrice += detail.Count * detail.Product.GetProductPrice();
            }

            return totalPrice;
        }

        #endregion

        #region Pay Order Product Price To Seller

        public async Task PayOrderProductPriceToSeller(long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            foreach (var detail in openOrder.OrderDetails)
            {
                var totalPrice = detail.Product.GetProductPrice() * detail.Count;

                await _sellerWalletService.AddWallet(new Data.Entities.Wallet.SellerWallet
                {
                    SellerId = detail.Product.SellerId,
                    Price = (int)Math.Ceiling(totalPrice * detail.Product.SiteProfit / (double)100),
                    TransactionType = Data.Entities.Wallet.TransactionType.Deposit,
                    Description = $"پرداخت مبلغ {totalPrice.ToString("#,0")} جهت فروش {detail.Product.Title} به تعداد {detail.Count} با {detail.Product.SiteProfit} درصد کمیسیون سایت"
                });

                detail.ProductPrice = totalPrice;
                _orderDetailRepository.EditEntity(detail);
            }

            openOrder.IsPaid = true;
            //todo: set description and tracing code ...
            _orderRepository.EditEntity(openOrder);
            await _orderRepository.SaveChanges();
        }

        #endregion

        #region Remove Order Detail

        public async Task<bool> RemoveOrderDetail(long detailId, long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            var orderDetail = openOrder.OrderDetails.SingleOrDefault(d => d.Id == detailId);

            if (orderDetail == null)
                return false;

            _orderDetailRepository.DeletePermanent(orderDetail);
            await _orderDetailRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Change Order Detail Count

        public async Task<ChangeOrderDetailCountResult> ChangeOrderDetailCount(long detailId, int count, long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            var orderDetail = openOrder.OrderDetails.SingleOrDefault(d => d.Id == detailId);
            var mainProduct = await _productService.GetProductById(orderDetail.ProductId);

            if (orderDetail == null || mainProduct == null || count < 1)
                return ChangeOrderDetailCountResult.Error;

            if (count > mainProduct.QuantityInShop)
                return ChangeOrderDetailCountResult.QuantityIsLowerThanCount;

            // When All Conditions Passed :

            orderDetail.Count = count;

            _orderDetailRepository.EditEntity(orderDetail);
            await _orderDetailRepository.SaveChanges();

            return ChangeOrderDetailCountResult.Success;
        }

        #endregion

        #region Close User Open Order

        public async Task CloseUserOpenOrderAfterPayment(long userId, long tracingCode)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            openOrder.PaymentDate = DateTime.Now;
            openOrder.IsPaid = true;
            openOrder.TracingCode = tracingCode.ToString();

            _orderRepository.EditEntity(openOrder);
            await _orderRepository.SaveChanges();
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            await _orderDetailRepository.DisposeAsync();
            await _orderRepository.DisposeAsync();
        }

        #endregion

    }
}
