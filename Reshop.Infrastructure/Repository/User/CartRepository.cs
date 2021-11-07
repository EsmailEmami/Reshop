using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<OrderDetailForShowCartViewModel> GetOrderInCartByUserIdForShowCart(string userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId && !o.IsPayed && !o.IsReceived)
                .SelectMany(c => c.OrderDetails)
                .Select(o => new OrderDetailForShowCartViewModel()
                {
                    ProductsCount = o.Count,
                    ProductPrice = o.ShopperProductColor.Price,
                    ColorName = o.ShopperProductColor.Color.ColorName,
                    Discount = o.ShopperProductColor.Discounts
                        .OrderByDescending(c => c.EndDate)
                        .Select(d => new Tuple<byte, DateTime>(d.DiscountPercent, d.EndDate))
                        .FirstOrDefault(),
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

        public async Task<bool> IsUserOrderAsync(string userId, string orderId) =>
            await _context.Orders.AnyAsync(c => c.OrderId == orderId && c.UserId == userId);

        public async Task<bool> IsOrderExistsAsync(string orderId) =>
            await _context.Orders.AnyAsync(c => c.OrderId == orderId);

        public async Task<bool> IsOpenOrderExistsAsync(string orderId) =>
            await _context.Orders
                .AnyAsync(c =>
                    c.OrderId == orderId &&
                    !c.IsReceived &&
                    !c.IsPayed);

        public async Task<decimal> GetOrderDiscountAsync(string orderId) =>
            await _context.Orders.Where(c => c.OrderId == orderId)
                .Select(c => c.OrderDiscount)
                .SingleOrDefaultAsync();

        public async Task<string> GetUserOpenCartOrderIdAsync(string userId) =>
            await _context.Orders.Where(c => c.UserId == userId && !c.IsPayed && !c.IsReceived)
                .Select(c => c.OrderId).SingleOrDefaultAsync();

        public Task<List<OpenCartDetailViewModel>> GetOrderDetailAsync(string orderId) =>
            _context.Orders.Where(c => c.OrderId == orderId)
                .SelectMany(c => c.OrderDetails)
                .Select(c => new OpenCartDetailViewModel()
                {
                    ProductPrice = c.ShopperProductColor.Price,
                    ProductsCount = c.Count,
                    Discount = c.ShopperProductColor.Discounts
                        .OrderByDescending(e => e.EndDate)
                        .Select(d => new Tuple<byte, DateTime>(d.DiscountPercent, d.EndDate))
                        .FirstOrDefault(),
                })
                .ToListAsync();

        public IEnumerable<OrderForShowViewModel> GetReceivedOrders(string userId) =>
            _context.Orders.Where(c => c.UserId == userId && c.IsReceived && c.IsPayed)
                .Select(c => new OrderForShowViewModel()
                {
                    TrackingCode = c.TrackingCode,
                    PayDate = c.PayDate.Value,
                    Sum = c.Sum,
                    ProPics = c.OrderDetails.Select(p => p.ShopperProductColor.ShopperProduct.Product.ProductGalleries.FirstOrDefault().ImageName)
                });

        public IEnumerable<OrderForShowViewModel> GetNotReceivedOrders(string userId) =>
            _context.Orders.Where(c => c.UserId == userId && !c.IsReceived && c.IsPayed)
                .Select(c => new OrderForShowViewModel()
                {
                    TrackingCode = c.TrackingCode,
                    PayDate = c.PayDate.Value,
                    Sum = c.Sum,
                    ProPics = c.OrderDetails.Select(p => p.ShopperProductColor.ShopperProduct.Product.ProductGalleries.FirstOrDefault().ImageName)
                });

        public async Task<string> GetOrderIdByTrackingCodeAsync(string trackingCode) =>
            await _context.Orders
                .Where(c => c.TrackingCode == trackingCode)
                .Select(c=> c.OrderId)
                .SingleOrDefaultAsync();

        public async Task<string> GetOrderDetailIdByTrackingCodeAsync(string trackingCode) =>
            await _context.OrderDetails
                .Where(c => c.TrackingCode == trackingCode)
                .Select(c => c.OrderDetailId)
                .SingleOrDefaultAsync();

        public async Task<FullOrderForShowViewModel> GetFullOrderForShowAsync(string orderId) =>
            await _context.Orders.Where(c => c.OrderId == orderId && c.IsPayed)
                .Select(c => new FullOrderForShowViewModel()
                {
                    TrackingCode = c.TrackingCode,
                    PayDate = c.PayDate.Value,
                    Sum = c.Sum,
                    ShoppingCost = c.ShippingCost,
                    OrderDiscount = c.OrderDiscount,
                    IsReceived = c.IsReceived,
                    IsPayed = c.IsPayed,
                    Details = c.OrderDetails.Select(a => new OrderDetailForShowViewModel()
                    {
                        OrderDetailId = a.OrderDetailId,
                        ProductsCount = a.Count,
                        ProductPrice = a.Price,
                        ProductTitle = a.ShopperProductColor.ShopperProduct.Product.ProductTitle,
                        ProductImg = a.ShopperProductColor.ShopperProduct.Product.ProductGalleries.OrderBy(i => i.OrderBy).FirstOrDefault().ImageName,
                        Warranty = a.ShopperProductColor.ShopperProduct.Warranty,
                        ShopperId = a.ShopperProductColor.ShopperProduct.ShopperId,
                        ShopperStoreName = a.ShopperProductColor.ShopperProduct.Shopper.StoreName,
                        ColorName = a.ShopperProductColor.Color.ColorName,
                        TrackingCode = a.TrackingCode
                    })
                }).SingleOrDefaultAsync();

        public IEnumerable<OrderForShowInListViewModel> GetUserOrdersForShowInListWithPagination(string userId, string type, string orderBy, int skip, int take)
        {
            IQueryable<Order> orders = _context.Users
                .Where(c => c.UserId == userId)
                .SelectMany(c => c.Orders)
                .Where(c => c.IsPayed);


            orders = type.ToLower() switch
            {
                "all" => orders,
                "received" => orders.Where(c => c.IsReceived),
                "payed" => orders.Where(c => c.IsPayed),
                _ => orders
            };


            orders = orderBy.ToLower() switch
            {
                "news" => orders.OrderByDescending(c => c.CreateDate),
                "last" => orders.OrderBy(c => c.CreateDate),
                "received-news" => orders.OrderByDescending(c => c.IsReceived)
                    .ThenByDescending(c => c.CreateDate),
                "received-last" => orders.OrderByDescending(c => c.IsReceived)
                    .ThenBy(c => c.CreateDate),
                "payed-news" => orders.OrderByDescending(c => c.IsPayed)
                    .ThenByDescending(c => c.CreateDate),
                "payed-last" => orders.OrderByDescending(c => c.IsPayed)
                    .ThenBy(c => c.CreateDate),
                _ => orders.OrderByDescending(c => c.CreateDate)
            };

            orders = orders.Skip(skip).Take(take);

            return orders.Select(c => new OrderForShowInListViewModel()
            {
                OrderId = c.OrderId,
                PayDate = c.PayDate.Value,
                Sum = c.Sum,
                IsReceived = c.IsReceived,
                TrackingCode = c.TrackingCode
            });
        }

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
            _context.Orders.Where(o => o.UserId == userId && !o.IsPayed && !o.IsReceived).Select(c => c.OrderAddressId).SingleOrDefault();

        public async Task<int> GetUserOrdersCount(string userId, string type = "all")
        {
            IQueryable<Order> orders = _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.Orders)
                .Where(c => c.IsPayed);

            return type.ToLower() switch
            {
                "all" => await orders.CountAsync(),
                "payed" => await orders.Where(c => !c.IsReceived).CountAsync(),
                "received" => await orders.Where(c => c.IsReceived).CountAsync(),
                _ => 0
            };
        }

        public async Task AddOrderAddressAsync(OrderAddress orderAddress) =>
            await _context.OrderAddresses.AddAsync(orderAddress);

        public void UpdateOrderAddress(OrderAddress orderAddress) =>
            _context.OrderAddresses.Update(orderAddress);

        public void RemoveOrderAddress(OrderAddress orderAddress) =>
            _context.OrderAddresses.Remove(orderAddress);

        public async Task<OrderAddress> GetOrderAddressByIdAsync(string orderAddressId) =>
            await _context.OrderAddresses.FindAsync(orderAddressId);

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
