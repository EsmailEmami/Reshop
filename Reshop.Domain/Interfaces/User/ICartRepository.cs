using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.Interfaces.User
{
    public interface ICartRepository
    {
        #region order detail

        Task<OrderDetail> GetOrderDetailAsync(string orderId, int productId, string shopperUserId);
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
        Task<OpenCartViewModel> GetOrderInCartByUserIdForShowCartAsync(string userId);
        Task<Order> GetOrderInCartByUserIdAsync(string userId);

        IEnumerable<ReceivedOrdersViewModel> GetReceivedOrders(string userId);
        IEnumerable<ReceivedOrdersViewModel> GetNotReceivedOrders(string userId);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task AddOrderAsync(Order order);
        void UpdateOrder(Order order);
        Task<bool> IsOrderTrackingCodeExistAsync(string trackingCode);
        void RemoveOrder(Order order);
        Task<Order> GetOrderByOrderDetailAsync(string orderDetailId);

        string GetOpenOrderAddressId(string userId);

        #endregion


        Task SaveChangesAsync();
    }
}