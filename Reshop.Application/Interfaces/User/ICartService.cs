using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;

namespace Reshop.Application.Interfaces.User
{
    public interface ICartService
    {
        Task<OpenCartViewModel> GetUserOpenOrderForShowCartAsync(string userId);
        Task<Order> GetUserOpenOrderAsync(string userId);
        Task EditOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task<ResultTypes> AddToCart(string userId, int productId,string shopperId);
        Task<bool> IsOrderDetailExistAsync(string orderDetailId);
        Task IncreaseOrderDetailCountAsync(string orderDetailId);
        Task ReduceOrderDetailAsync(string orderDetailId);
        IAsyncEnumerable<Order> GetOrdersAfterDateTime(DateTime time);
        IEnumerable<ReceivedOrdersViewModel> GetReceivedOrders(string userId);
        IEnumerable<ReceivedOrdersViewModel> GetNotReceivedOrders(string userId);
        IAsyncEnumerable<OrderDetail> GetOrderDetailsOfOrder(string orderId);
        Task RemoveOrderAsync(Order order);
        Task<ResultTypes> RemoveOrderDetailAsync(string orderDetailId);
        string GetOpenOrderAddressId(string userId);
        #region Discount

        Task<DiscountUseType> UseDiscountAsync(string orderId, string discountCode);

        #endregion
    }
}
