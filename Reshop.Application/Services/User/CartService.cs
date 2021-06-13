using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Application.Calculate;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.Order;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Domain.Interfaces.User;

namespace Reshop.Application.Services.User
{
    public class CartService : ICartService
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShopperRepository _shopperRepository;

        public CartService(IProductRepository productRepository, ICartRepository cartRepository, IUserRepository userRepository, IShopperRepository shopperRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _shopperRepository = shopperRepository;
        }

        #endregion

        public async Task<Order> GetUserOpenOrderAsync(string userId)
        {
            return await _cartRepository.GetOrderInCartByUserIdAsync(userId);
        }

        public async Task EditOrderAsync(Order order)
        {
            _cartRepository.UpdateOrder(order);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId) =>
            await _cartRepository.GetOrderByIdAsync(orderId);

        public async Task<ResultTypes> AddToCart(string userId, int productId, string shopperUserId)
        {
            try
            {
                if (!await _userRepository.IsUserExistAsync(userId))
                    return ResultTypes.Failed;

                var product = await _productRepository.GetProductByIdAsync(productId);
                if (product is null)
                    return ResultTypes.Failed;

                var shopperProduct = await _shopperRepository.GetShopperProductAsync(shopperUserId, product.ProductId);
                if (shopperProduct is null)
                    return ResultTypes.Failed;


                var order = await _cartRepository.GetOrderInCartByUserIdAsync(userId);
                if (order is not null)
                {
                    var orderDetail = await _cartRepository.GetOrderDetailAsync(order.OrderId, product.ProductId);

                    if (orderDetail is not null)
                    {
                        orderDetail.Count += 1;
                        orderDetail.Sum = CartCalculator.CalculateOrderDetail(orderDetail.Count, orderDetail.Price);

                        _cartRepository.UpdateOrderDetail(orderDetail);
                        await _cartRepository.SaveChangesAsync();

                        order.Sum = _cartRepository.GetOrderDetailsSumOfOrder(orderDetail.OrderId);
                        _cartRepository.UpdateOrder(order);
                        await _cartRepository.SaveChangesAsync();
                    }
                    else
                    {
                        var orderDetail_new = new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = product.ProductId,
                            Price = product.Price,
                            Count = 1,
                            Sum = product.Price,
                            CreateDate = DateTime.Now,
                            TrackingCode = "RSD" + NameGenerator.GenerateNumber(),
                            ShopperUserId = shopperUserId
                        };

                        while (await _cartRepository.IsOrderDetailTrackingCodeExistAsync(orderDetail_new.TrackingCode))
                        {
                            orderDetail_new.TrackingCode = "RSD" + NameGenerator.GenerateNumber();
                        }

                        await _cartRepository.AddOrderDetailAsync(orderDetail_new);
                        await _cartRepository.SaveChangesAsync();

                        // sum all of order details
                        order.Sum = _cartRepository.GetOrderDetailsSumOfOrder(order.OrderId);
                        _cartRepository.UpdateOrder(order);
                        await _cartRepository.SaveChangesAsync();
                    }
                }
                else
                {
                    var order_new = new Order()
                    {
                        UserId = userId,
                        TrackingCode = "RSO" + NameGenerator.GenerateNumber(),
                        CreateDate = DateTime.Now,
                        Sum = 0,
                        IsPayed = false,
                        IsReceived = false,
                    };
                    while (await _cartRepository.IsOrderTrackingCodeExistAsync(order_new.TrackingCode))
                    {
                        order_new.TrackingCode = "RSO" + NameGenerator.GenerateNumber();
                    }

                    await _cartRepository.AddOrderAsync(order_new);
                    await _cartRepository.SaveChangesAsync();


                    var orderDetail_new = new OrderDetail()
                    {
                        OrderId = order_new.OrderId,
                        ProductId = product.ProductId,
                        Price = product.Price,
                        Count = 1,
                        Sum = product.Price,
                        CreateDate = DateTime.Now,
                        TrackingCode = "RSD" + NameGenerator.GenerateNumber(),
                        ShopperUserId = shopperUserId
                    };

                    while (await _cartRepository.IsOrderDetailTrackingCodeExistAsync(orderDetail_new.TrackingCode))
                    {
                        orderDetail_new.TrackingCode = "RSD" + NameGenerator.GenerateNumber();
                    }

                    await _cartRepository.AddOrderDetailAsync(orderDetail_new);
                    await _cartRepository.SaveChangesAsync();

                    order_new.Sum = _cartRepository.GetOrderDetailsSumOfOrder(order_new.OrderId);
                    _cartRepository.UpdateOrder(order_new);
                    await _cartRepository.SaveChangesAsync();
                }


                shopperProduct.SaleCount += 1;
                _shopperRepository.UpdateShopperProduct(shopperProduct);
                await _cartRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }

            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsOrderDetailExistAsync(string orderDetailId)
        {
            return await _cartRepository.IsOrderDetailExistAsync(orderDetailId);
        }

        public async Task IncreaseOrderDetailCountAsync(string orderDetailId)
        {
            var orderDetail = await _cartRepository.GetOrderDetailByIdAsync(orderDetailId);

            if (orderDetail != null)
            {
                orderDetail.Count++;

                orderDetail.Sum = CartCalculator.CalculateOrderDetail(orderDetail.Count, orderDetail.Price);

                _cartRepository.UpdateOrderDetail(orderDetail);
                await _cartRepository.SaveChangesAsync();

                // update order sum
                var order = await _cartRepository.GetOrderByIdAsync(orderDetail.OrderId);


                order.Sum = _cartRepository.GetOrderDetailsSumOfOrder(order.OrderId);

                _cartRepository.UpdateOrder(order);

                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task ReduceOrderDetailAsync(string orderDetailId)
        {
            var orderDetail = await _cartRepository.GetOrderDetailByIdAsync(orderDetailId);
            

            if (orderDetail != null)
            {
                var order = await _cartRepository.GetOrderByIdAsync(orderDetail.OrderId);

                if (orderDetail.Count > 1)
                {
                    orderDetail.Count--;

                    orderDetail.Sum = CartCalculator.CalculateOrderDetail(orderDetail.Count, orderDetail.Price);

                    _cartRepository.UpdateOrderDetail(orderDetail);
                }
                else
                {
                    _cartRepository.RemoveOrderDetail(orderDetail);
                }

                await _cartRepository.SaveChangesAsync();

              
               

                if (order.OrderDetails.Any())
                {
                    order.Sum = _cartRepository.GetOrderDetailsSumOfOrder(order.OrderId);

                    _cartRepository.UpdateOrder(order);
                }
                else
                {
                    _cartRepository.RemoveOrder(order);
                }

                await _cartRepository.SaveChangesAsync();
            }
        }

        public IAsyncEnumerable<Order> GetOrdersAfterDateTime(DateTime time)
        {
            return _cartRepository.GetOrdersAfterDateTime(time);
        }

        public async IAsyncEnumerable<ReceivedOrdersViewModel> GetReceivedOrders(string userId)
        {
            var orders = _cartRepository.GetReceivedOrders(userId);

            await foreach (var order in orders)
            {
                var model = new ReceivedOrdersViewModel()
                {
                    OrderId = order.OrderId,
                    Sum = order.Sum,
                    TrackingCode = order.TrackingCode,
                    CreateDate = order.CreateDate,
                };

                var listPics = new List<string>();

                model.ProPics = listPics;

                foreach (var orderDetail in order.OrderDetails)
                {
                    var proPics = await _productRepository.GetProductFirstPictureName(orderDetail.ProductId);
                    listPics.Add(proPics);
                }
                yield return model;
            }
        }


        public IAsyncEnumerable<OrderDetail> GetOrderDetailsOfOrder(string orderId)
        {
            return _cartRepository.GetOrderDetailsOfOrder(orderId);
        }

        public async Task RemoveOrderAsync(Order order)
        {
            _cartRepository.RemoveOrder(order);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<ResultTypes> RemoveOrderDetailAsync(string orderDetailId)
        {
            var orderDetail = await _cartRepository.GetOrderDetailByIdAsync(orderDetailId);
            if (orderDetail is null) return ResultTypes.Failed;

            try
            {
                var order = await _cartRepository.GetOrderByIdAsync(orderDetail.OrderId);

                _cartRepository.RemoveOrderDetail(orderDetail);
                await _cartRepository.SaveChangesAsync();

                if (order.OrderDetails.Any())
                {
                    order.Sum = _cartRepository.GetOrderDetailsSumOfOrder(order.OrderId);

                    _cartRepository.UpdateOrder(order);
                }
                else
                {
                    _cartRepository.RemoveOrder(order);
                }

                await _cartRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<DiscountUseType> UseDiscountAsync(string orderId, string discountCode)
        {
            var discount = await _userRepository.GetDiscountByCodeAsync(discountCode);

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.StartDate != null && discount.StartDate < DateTime.Now)
                return DiscountUseType.Expired;

            if (discount.EndDate != null && discount.EndDate >= DateTime.Now)
                return DiscountUseType.Expired;

            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = await _cartRepository.GetOrderByIdAsync(orderId);

            if (_userRepository.IsUserDiscountCodeExist(order.UserId, discount.DiscountId))
                return DiscountUseType.UserUsed;

            var percent = (order.Sum * discount.DiscountPercent) / 100;
            order.Sum -= percent;
            _cartRepository.UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _userRepository.UpdateDiscount(discount);


            var userDiscountCode = new UserDiscountCode()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId
            };
            await _userRepository.AddUserDiscountCodeAsync(userDiscountCode);

            await _userRepository.SaveChangesAsync();

            return DiscountUseType.Success;
        }

    }
}
