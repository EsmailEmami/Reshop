using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Api.Controllers.Cart
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        #region Constructor

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        #endregion


        [HttpDelete]
        public async Task<IActionResult> RemoveOrdersAfterDateTime([FromBody] DateTime time)
        {
            if (time >= DateTime.Now)
            {
                return BadRequest();
            }
            var orders = _cartService.GetOrdersAfterDateTime(time);

            if (orders != null)
            {
                await foreach (var order in orders)
                {
                    var orderDetails = _cartService.GetOrderDetailsOfOrder(order.OrderId);

                    await foreach (var orderDetail in orderDetails)
                    {
                        await _cartService.RemoveOrderDetailAsync(orderDetail);
                    }

                    await _cartService.RemoveOrderAsync(order);
                }
            }



            return Ok();
        }
    }
}
