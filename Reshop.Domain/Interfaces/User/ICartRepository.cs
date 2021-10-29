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
        IAsyncEnumerable<OrderDetail> GetOrderDetailsOfOrder(string orderId);
        decimal GetOrderDetailsSumOfOrder(string orderId);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> IsOrderDetailTrackingCodeExistAsync(string trackingCode);
        Task<bool> IsOrderDetailExistAsync(string orderDetailId);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void RemoveOrderDetail(OrderDetail orderDetail);

        #endregion

        #region order

        IAsyncEnumerable<Order> GetOrdersAfterDateTime(DateTime time);
        IEnumerable<OpenCartViewModel> GetOrderInCartByUserIdForShowCart(string userId);
        Task<Order> GetOrderInCartByUserIdAsync(string userId);

        IEnumerable<OrderForShowViewModel> GetReceivedOrders(string userId);
        IEnumerable<OrderForShowViewModel> GetNotReceivedOrders(string userId);

        // type = all , received , payed
        IEnumerable<OrderForShowViewModel> GetUserOrdersWithPagination(string userId, string type, string orderBy, int skip, int take);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task AddOrderAsync(Order order);
        void UpdateOrder(Order order);
        Task<bool> IsOrderTrackingCodeExistAsync(string trackingCode);
        void RemoveOrder(Order order);
        Task<Order> GetOrderByOrderDetailAsync(string orderDetailId);

        string GetOpenOrderAddressId(string userId);

        // type = all , payed , received
        Task<int> GetUserOrdersCount(string userId, string type = "all");

        #endregion

        Task<int> GetSellsCountFromDateAsync(DateTime dateTime);
        Task<int> GetSellCountOfProductColorFromDateAsync(int productId, int colorId, DateTime dateTime);
        Task<int> GetSellCountOfProductColorFromDateAsync(int productId, int colorId);
        Task<string> IsUserBoughtProductAsync(string userId, int productId);


        Task SaveChangesAsync();
    }
}