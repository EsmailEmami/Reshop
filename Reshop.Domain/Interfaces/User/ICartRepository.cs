using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.User
{
    public interface ICartRepository
    {
        #region order detail

        Task<OrderDetail> GetOrderDetailAsync(string orderId, string shopperProductColorId);
        Task<OrderDetail> GetOrderDetailByIdAsync(string orderDetailId);
        IEnumerable<OrderDetail> GetOrderDetailsOfOrder(string orderId);
        decimal GetOrderDetailsSumOfOrder(string orderId);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> IsOrderDetailTrackingCodeExistAsync(string trackingCode);
        Task<bool> IsOrderDetailExistAsync(string orderDetailId);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void RemoveOrderDetail(OrderDetail orderDetail);

        #endregion

        #region order

        IEnumerable<Order> GetOrdersAfterDateTime(DateTime time);
        IEnumerable<OrderDetailForShowCartViewModel> GetOrderInCartByUserIdForShowCart(string userId);
        Task<Order> GetOrderInCartByUserIdAsync(string userId);

        Task<bool> IsUserOrderAsync(string userId, string orderId);
        Task<bool> IsOrderExistsAsync(string orderId);
        Task<bool> IsOpenOrderExistsAsync(string orderId);
        Task<decimal> GetOrderDiscountAsync(string orderId);

        Task<string> GetUserOpenCartOrderIdAsync(string userId);

        Task<List<OpenCartDetailViewModel>> GetOrderDetailAsync(string orderId);

        IEnumerable<OrderForShowViewModel> GetReceivedOrders(string userId);
        IEnumerable<OrderForShowViewModel> GetNotReceivedOrders(string userId);

        Task<string> GetOrderIdByTrackingCodeAsync(string trackingCode);
        Task<string> GetOrderDetailIdByTrackingCodeAsync(string trackingCode);
        Task<FullOrderForShowViewModel> GetFullOrderForShowAsync(string orderId);

        // type = all , received , payed
        IEnumerable<OrderForShowInListViewModel> GetUserOrdersForShowInListWithPagination(string userId, string type, string orderBy, int skip, int take);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task AddOrderAsync(Order order);
        void UpdateOrder(Order order);
        Task<bool> IsOrderTrackingCodeExistAsync(string trackingCode);
        void RemoveOrder(Order order);
        Task<Order> GetOrderByOrderDetailAsync(string orderDetailId);

        string GetOpenOrderAddressId(string userId);

        // type = all , payed , received
        Task<int> GetUserOrdersCount(string userId, string type = "all");


        Task AddOrderAddressAsync(OrderAddress orderAddress);
        void UpdateOrderAddress(OrderAddress orderAddress);
        void RemoveOrderAddress(OrderAddress orderAddress);
        Task<OrderAddress> GetOrderAddressByIdAsync(string orderAddressId);

        #endregion

        Task<int> GetSellsCountFromDateAsync(DateTime dateTime);
        Task<int> GetSellCountOfProductColorFromDateAsync(int productId, int colorId, DateTime dateTime);
        Task<int> GetSellCountOfProductColorFromDateAsync(int productId, int colorId);
        Task<string> IsUserBoughtProductAsync(string userId, int productId);


        Task SaveChangesAsync();
    }
}