using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.User
{
    public interface ICartService
    {
        IEnumerable<OrderDetailForShowCartViewModel> GetUserOpenOrderForShowCart(string userId);
        Task<Order> GetUserOpenOrderAsync(string userId);
        Task EditOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task<FullOrderForShowViewModel> GetFullOrderForShowAsync(string orderId);
        Task<ResultTypes> MakeFinalTheOrder(string orderId);
        Task<ResultTypes> AddToCart(string userId, string shopperProductColorId);
        Task<bool> IsUserOrderAsync(string userId, string orderId);
        Task<bool> IsUserOrderDetailValidationByTrackingCodeAsync(string userId, string trackingCode);
        Task<bool> IsOrderDetailExistAsync(string orderDetailId);
        Task<string> IsUserBoughtProductAsync(string userId, int productId);
        Task<string> GetUserOpenCartOrderIdAsync(string userId);
        Task<string> GetOrderIdByTrackingCodeAsync(string trackingCode);
        Task<string> GetOrderDetailIdByTrackingCodeAsync(string trackingCode);
        Task IncreaseOrderDetailCountAsync(string orderDetailId);
        Task ReduceOrderDetailAsync(string orderDetailId);
        IEnumerable<Order> GetOrdersAfterDateTime(DateTime time);
        IEnumerable<OrderForShowViewModel> GetReceivedOrders(string userId);
        IEnumerable<OrderForShowViewModel> GetNotReceivedOrders(string userId);
        IEnumerable<OrderDetail> GetOrderDetailsOfOrder(string orderId);
        Task RemoveOrderAsync(Order order);
        Task<ResultTypes> RemoveOrderDetailAsync(string orderDetailId);
        string GetOpenOrderAddressId(string userId);


        Task<ResultTypes> AddAddressToOrderAsync(string orderId, string userAddressId);


        //orderId, sum , have any address , tracking code
        Task<Tuple<string, decimal, string>> GetOpenOrderForPaymentAsync(string userId);

        // type = all , received , payed
        // order by = news , last , received-news , received-last , payed-news , payed-last
        Task<Tuple<IEnumerable<OrderForShowInListViewModel>, int, int>> GetUserOrdersForShowInListWithPaginationAsync(string userId, int pageId, int take, string type = "all", string orderBy = "news");
    }
}
