using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;

namespace Reshop.Infrastructure.Repository.User
{
    public class CartRepository : ICartRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public CartRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<OrderDetail> GetOrderDetailAsync(string orderId, string shopperProductColorId)
        {
            return await _context.OrderDetails
                .SingleOrDefaultAsync(c => c.OrderId == orderId && c.ShopperProductColorId == shopperProductColorId);
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(string orderDetailId)
        {
            return await _context.OrderDetails.FindAsync(orderDetailId);
        }

        public IAsyncEnumerable<OrderDetail> GetOrderDetailsOfOrder(string orderId)
            =>
                _context.OrderDetails.Where(c => c.OrderId == orderId) as IAsyncEnumerable<OrderDetail>;

        public decimal GetOrderDetailsSumOfOrder(string orderId)
        {
            return _context.OrderDetails.Where(o => o.OrderId == orderId).Select(d => d.Sum).Sum();
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        public async Task<bool> IsOrderDetailTrackingCodeExistAsync(string trackingCode)
        {
            return await _context.OrderDetails.AnyAsync(c => c.TrackingCode == trackingCode);
        }

        public async Task<bool> IsOrderDetailExistAsync(string orderDetailId)
        {
            return await _context.OrderDetails.AnyAsync(c => c.OrderDetailId == orderDetailId);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }

        public void RemoveOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
        }

        public IAsyncEnumerable<Order> GetOrdersAfterDateTime(DateTime time)
            =>
                _context.Orders.Where(c => !c.IsPayed && !c.IsReceived && c.CreateDate > time) as IAsyncEnumerable<Order>;

        public IEnumerable<OpenCartViewModel> GetOrderInCartByUserIdForShowCart(string userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId && !o.IsPayed && !o.IsReceived)
                .SelectMany(c => c.OrderDetails)
                .Select(o => new OpenCartViewModel()
                {
                    OrderDetailId = o.OrderDetailId,
                    ProductsCount = o.Count,
                    ProductPrice = o.ShopperProductColor.Price,
                    ColorName = o.ShopperProductColor.Color.ColorName,
                    Discount = o.ShopperProductColor.Discounts.OrderByDescending(c => c.EndDate).Select(d => new Tuple<byte, DateTime>(d.DiscountPercent, d.EndDate)).FirstOrDefault(),
                    ProductTitle = o.ShopperProductColor.ShopperProduct.Product.ProductTitle,
                    ProductImg = o.ShopperProductColor.ShopperProduct.Product.ProductGalleries.FirstOrDefault().ImageName,
                    Warranty = o.ShopperProductColor.ShopperProduct.Warranty,
                    ShopperStoreName = o.ShopperProductColor.ShopperProduct.Shopper.StoreName
                });
        }

        public async Task<Order> GetOrderInCartByUserIdAsync(string userId)
        {
            return await _context.Orders
                .SingleOrDefaultAsync(o => o.UserId == userId && !o.IsPayed && !o.IsReceived);
        }

        public IEnumerable<ReceivedOrdersViewModel> GetReceivedOrders(string userId) =>
            _context.Orders.Where(c => c.UserId == userId && c.IsReceived && c.IsPayed)
                .Select(c => new ReceivedOrdersViewModel()
                {
                    OrderId = c.OrderId,
                    TrackingCode = c.TrackingCode,
                    PayDate = c.PayDate.Value,
                    Sum = c.Sum,
                    ProPics = c.OrderDetails.Select(p => p.ShopperProductColor.ShopperProduct.Product.ProductGalleries.FirstOrDefault().ImageName)
                });

        public IEnumerable<ReceivedOrdersViewModel> GetNotReceivedOrders(string userId) =>
            _context.Orders.Where(c => c.UserId == userId && !c.IsReceived && c.IsPayed)
                .Select(c => new ReceivedOrdersViewModel()
                {
                    OrderId = c.OrderId,
                    TrackingCode = c.TrackingCode,
                    PayDate = c.PayDate.Value,
                    Sum = c.Sum,
                    ProPics = c.OrderDetails.Select(p => p.ShopperProductColor.ShopperProduct.Product.ProductGalleries.FirstOrDefault().ImageName)
                });

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<bool> IsOrderTrackingCodeExistAsync(string trackingCode)
        {
            return await _context.Orders.AnyAsync(c => c.TrackingCode == trackingCode);
        }

        public void RemoveOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<Order> GetOrderByOrderDetailAsync(string orderDetailId)
        {
            return await _context.OrderDetails.Where(c => c.OrderDetailId == orderDetailId)
                .Select(c => c.Order).Include(c => c.OrderDetails).SingleAsync();
        }

        public string GetOpenOrderAddressId(string userId) =>
            _context.Orders.Where(o => o.UserId == userId && !o.IsPayed && !o.IsReceived).Select(c => c.AddressId).SingleOrDefault();

        public async Task<int> GetSellsCountFromDateAsync(DateTime dateTime) =>
            await _context.OrderDetails.Where(c =>
                c.Order.IsPayed &&
                c.Order.PayDate >= dateTime).CountAsync();

        public async Task<int> GetSellCountOfProductColorFromDateAsync(int productId, int colorId, DateTime dateTime) =>
             await _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.ShopperProductColor.ColorId == colorId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= dateTime).CountAsync();

        public async Task<int> GetSellCountOfProductColorFromDateAsync(int productId, int colorId) =>
            await _context.OrderDetails.Where(c =>
                c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                c.ShopperProductColor.ColorId == colorId &&
                c.Order.IsPayed).CountAsync();

        public async Task<string> IsUserBoughtProductAsync(string userId, int productId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.Orders)
                .Where(c => c.IsPayed && c.IsReceived)
                .SelectMany(c => c.OrderDetails)
                .Where(c => c.ShopperProductColor.ShopperProduct.ProductId == productId)
                .Select(c => c.ShopperProductColorId).FirstOrDefaultAsync();

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
