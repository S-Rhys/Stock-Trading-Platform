using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blackfinch.StockTradingPlatform.Core.Companies;
using Blackfinch.StockTradingPlatform.Core.Orders;
using Blackfinch.StockTradingPlatform.Data.DTO;
using Blackfinch.StockTradingPlatform.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blackfinch.StockTradingPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ICompanyService companyService)
        {
            _orderService = orderService;
            _companyService = companyService;
        }

        [HttpGet("all")]
        public List<OrderDto> GetOrders()
        {
            return _orderService.GetAllOrders();
        }

        [HttpGet("processing")]
        public List<OrderDto> GetProcessingOrders()
        {
            return _orderService.GetProcessingOrders();
        }

        [HttpGet("{orderId:Guid}")]
        public ActionResult<OrderDto> GetOrder(Guid orderId)
        {
            var orderDto = _orderService.GetOrder(orderId);
            return orderDto != null ? (ActionResult<OrderDto>) Ok(orderDto) : NotFound("Order not found");
        }

        [HttpPost("place")]
        public ActionResult<OrderDto> PlaceOrder([Required] decimal minPriceInPoundSterling,
            [Required] decimal maxPriceInPoundSterling,
            [Required] int quantity, [Required] string symbol, [Required] OrderType orderType)
        {
            if (_companyService.GetCompanyBySymbol(symbol) == null)
                return NotFound($"Company not found for symbol: '{symbol}'");
            if (minPriceInPoundSterling < 0) return StatusCode(400, "minPriceInPoundSterling cannot be negative");
            if (maxPriceInPoundSterling < 0) return StatusCode(400, "maxPriceInPoundSterling cannot be negative");
            if (minPriceInPoundSterling > maxPriceInPoundSterling)
                return StatusCode(400, "minPriceInPoundSterling cannot be greater than maxPriceInPoundSterling");
            if (quantity <= 0) return StatusCode(400, "Quantity cannot be 0 or negative");
            var orderDto = _orderService.PlaceOrder(symbol, minPriceInPoundSterling, maxPriceInPoundSterling, quantity,
                orderType);
            return orderDto != null ? (ActionResult<OrderDto>) Ok(orderDto) : StatusCode(500, "Failure to place order");
        }
    }
}