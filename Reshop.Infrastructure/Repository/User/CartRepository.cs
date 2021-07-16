﻿using System;
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

        public async Task<OrderDetail> GetOrderDetailAsync(string orderId, string shopperId)
        {
            return await _context.OrderDetails
                .SingleOrDefaultAsync(c => c.OrderId == orderId && c.ShopperId == shopperId);
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
                    ProductPrice = _context.ShopperProducts.Single(s => s.ShopperId == o.ShopperId && s.ProductId == o.ProductId).Price,
                    ProductDiscountPercent = _context.ShopperProducts.Single(s => s.ShopperId == o.ShopperId && s.ProductId == o.ProductId).IsInDiscount ? _context.ShopperProducts.Single(s => s.ShopperId == o.ShopperId && s.ProductId == o.ProductId).Discounts.LastOrDefault().DiscountPercent : Convert.ToByte(0),
                    ProductTitle = o.Product.ProductTitle,
                    ProductImg = o.Product.ProductGalleries.FirstOrDefault().ImageName,
                    Warranty = _context.ShopperProducts.Single(a => a.ShopperId == o.ShopperId && a.ProductId == o.ProductId).Warranty,
                    ShopperStoreName = _context.Shoppers.Single(s => s.ShopperId == o.ShopperId).StoreName
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
                    PayDate = c.PayDate,
                    Sum = c.Sum,
                    ProPics = c.OrderDetails.Select(p => p.Product.ProductGalleries.FirstOrDefault().ImageName)
                });

        public IEnumerable<ReceivedOrdersViewModel> GetNotReceivedOrders(string userId) =>
            _context.Orders.Where(c => c.UserId == userId && !c.IsReceived && c.IsPayed)
                .Select(c => new ReceivedOrdersViewModel()
                {
                    OrderId = c.OrderId,
                    TrackingCode = c.TrackingCode,
                    PayDate = c.PayDate,
                    Sum = c.Sum,
                    ProPics = c.OrderDetails.Select(p => p.Product.ProductGalleries.FirstOrDefault().ImageName)
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
