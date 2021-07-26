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

        public IEnumerable<OpenCartViewModel> GetUserOpenOrderForShowCart(string userId)
        {
            return _cartRepository.GetOrderInCartByUserIdForShowCart(userId);
        }

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

        public async Task<ResultTypes> MakeFinalTheOrder(Order order)
        {

            if (order.IsPayed || order.IsReceived || string.IsNullOrEmpty(order.AddressId))
            {
                return ResultTypes.Failed;
            }


            try
            {
                // update orderDetails
                var orderDetails = _cartRepository.GetOrderDetailsOfOrder(order.OrderId);

                await foreach (var orderDetail in orderDetails)
                {
                    var shopperProductColor = await _shopperRepository.GetShopperProductColorAsync(orderDetail.ShopperProductColorId);

                    if (shopperProductColor is null)
                        return ResultTypes.Failed;

                    var lastDiscount = await _shopperRepository.GetLastShopperProductDiscountAsync(shopperProductColor.ShopperProductColorId);

                    orderDetail.Price = shopperProductColor.Price;

                    if (lastDiscount.EndDate > DateTime.Now)
                    {
                        order.OrderDiscount = CartCalculator.CalculateDiscount(orderDetail.Price, lastDiscount.DiscountPercent, orderDetail.Count);
                    }

                    orderDetail.Sum = (orderDetail.Count * orderDetail.Price) - orderDetail.ProductDiscountPrice;
                }

                await _cartRepository.SaveChangesAsync();

                // update order
                order.IsPayed = true;
                order.PayDate = DateTime.Now;
                order.Sum = _cartRepository.GetOrderDetailsSumOfOrder(order.OrderId);

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddToCart(string userId, int productId, string shopperProductColorId)
        {
            try
            {
                if (!await _shopperRepository.IsShopperProductColorExistAsync(shopperProductColorId))
                {
                    return ResultTypes.Failed;
                }

                var order = await _cartRepository.GetOrderInCartByUserIdAsync(userId);
                if (order != null)
                {
                    var orderDetail = await _cartRepository.GetOrderDetailAsync(order.OrderId, shopperProductColorId);

                    if (orderDetail != null)
                    {
                        orderDetail.Count++;

                        _cartRepository.UpdateOrderDetail(orderDetail);
                        await _cartRepository.SaveChangesAsync();
                    }
                    else
                    {
                        var orderDetail_new = new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            Price = 0,
                            Count = 1,
                            ProductDiscountPrice = 0,
                            CreateDate = DateTime.Now,
                            TrackingCode = "RSD-" + NameGenerator.GenerateNumber(),
                            Sum = 0
                        };



                        while (await _cartRepository.IsOrderDetailTrackingCodeExistAsync(orderDetail_new.TrackingCode))
                        {
                            orderDetail_new.TrackingCode = "RSD" + NameGenerator.GenerateNumber();
                        }

                        await _cartRepository.AddOrderDetailAsync(orderDetail_new);
                        await _cartRepository.SaveChangesAsync();

                    }
                }
                else
                {
                    var orderNew = new Order()
                    {
                        UserId = userId,
                        TrackingCode = "RSO-" + NameGenerator.GenerateNumber(),
                        CreateDate = DateTime.Now,
                        OrderDiscount = 0,
                        ShippingCost = 0,
                        Sum = 0,
                        IsPayed = false,
                        IsReceived = false,
                    };
                    while (await _cartRepository.IsOrderTrackingCodeExistAsync(orderNew.TrackingCode))
                    {
                        orderNew.TrackingCode = "RSO-" + NameGenerator.GenerateNumber();
                    }

                    await _cartRepository.AddOrderAsync(orderNew);
                    await _cartRepository.SaveChangesAsync();


                    var orderDetailNew = new OrderDetail()
                    {
                        OrderId = orderNew.OrderId,
                        Price = 0,
                        Count = 1,
                        ProductDiscountPrice = 0,
                        CreateDate = DateTime.Now,
                        TrackingCode = "RSD-" + NameGenerator.GenerateNumber(),
                        ShopperProductColorId = shopperProductColorId,
                        Sum = 0
                    };


                    while (await _cartRepository.IsOrderDetailTrackingCodeExistAsync(orderDetailNew.TrackingCode))
                    {
                        orderDetailNew.TrackingCode = "RSD" + NameGenerator.GenerateNumber();
                    }

                    await _cartRepository.AddOrderDetailAsync(orderDetailNew);
                    await _cartRepository.SaveChangesAsync();
                }


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


                _cartRepository.UpdateOrderDetail(orderDetail);
                await _cartRepository.SaveChangesAsync();

            }
        }

        public async Task ReduceOrderDetailAsync(string orderDetailId)
        {
            var orderDetail = await _cartRepository.GetOrderDetailByIdAsync(orderDetailId);


            if (orderDetail != null)
            {
                orderDetail.Count--;

                var order = await _cartRepository.GetOrderByIdAsync(orderDetail.OrderId);

                if (orderDetail.Count > 0)
                {


                    _cartRepository.UpdateOrderDetail(orderDetail);
                }
                else
                {
                    _cartRepository.RemoveOrderDetail(orderDetail);
                }

                await _cartRepository.SaveChangesAsync();

                if (!order.OrderDetails.Any())
                {
                    _cartRepository.RemoveOrder(order);
                    await _cartRepository.SaveChangesAsync();
                }
            }
        }

        public IAsyncEnumerable<Order> GetOrdersAfterDateTime(DateTime time)
        {
            return _cartRepository.GetOrdersAfterDateTime(time);
        }

        public IEnumerable<ReceivedOrdersViewModel> GetReceivedOrders(string userId) =>
            _cartRepository.GetReceivedOrders(userId);

        public IEnumerable<ReceivedOrdersViewModel> GetNotReceivedOrders(string userId) =>
            _cartRepository.GetNotReceivedOrders(userId);


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

        public string GetOpenOrderAddressId(string userId) =>
            _cartRepository.GetOpenOrderAddressId(userId);

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
