using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blackfinch.StockTradingPlatform.Data.Contexts;
using Blackfinch.StockTradingPlatform.Data.DTO;
using Blackfinch.StockTradingPlatform.Data.Models;

namespace Blackfinch.StockTradingPlatform.Core.Orders
{
    public interface IOrderService
    {
        public List<OrderDto> GetAllOrders();

        public List<OrderDto> GetProcessingOrders();

        public OrderDto GetOrder(Guid guid);

        public OrderDto PlaceOrder(string symbol, decimal minPriceInPoundSterling, decimal maxPriceInPoundSterling,
            int quantity,
            OrderType type);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderContext _orderContext;

        public OrderService(IOrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public List<OrderDto> GetAllOrders()
        {
            return _orderContext.GetAllOrders();
        }

        public List<OrderDto> GetProcessingOrders()
        {
            return _orderContext.GetProcessingOrders();
        }

        public OrderDto GetOrder(Guid guid)
        {
            return _orderContext.GetOrder(guid);
        }

        public OrderDto PlaceOrder(string symbol, decimal minPriceInPoundSterling, decimal maxPriceInPoundSterling,
            int quantity, OrderType type)
        {
            var orderDto2 = new OrderDto(symbol, minPriceInPoundSterling, maxPriceInPoundSterling, quantity, type);

            var orderDto = _orderContext.AddOrder(orderDto2);
            return orderDto.Type == OrderType.Buy ? HandleBuyOrder(orderDto) : HandleSellOrder(orderDto);
        }

        private OrderDto HandleSellOrder(OrderDto sellOrder)
        {
            Debug.WriteLine("1111");
            var outstandingBuyOrders = _orderContext.GetProcessingBuyOrders()
                .Where(buyOrder => buyOrder.Symbol == sellOrder.Symbol)
                .Where(buyOrder => sellOrder.MinPriceInPoundSterling >= buyOrder.MinPriceInPoundSterling &&
                                   sellOrder.MaxPriceInPoundSterling <= buyOrder.MaxPriceInPoundSterling)
                .OrderBy(buyOrder => buyOrder.OrderPlacedDateTime);
            foreach (var buyOrder in outstandingBuyOrders)
            {
                var numberOfShares = Math.Min(buyOrder.QuantityRemaining, sellOrder.QuantityRemaining);
                buyOrder.QuantityRemaining -= numberOfShares;
                sellOrder.QuantityRemaining -= numberOfShares;

                _orderContext.UpdateSellOrder(sellOrder);
                _orderContext.UpdateBuyOrder(buyOrder);

                if (buyOrder.QuantityRemaining == 0)
                {
                    buyOrder.Status = OrderStatus.Complete;
                    _orderContext.RemoveOrder(buyOrder);
                    _orderContext.AddOrder(buyOrder);
                }

                if (sellOrder.QuantityRemaining != 0) continue;

                sellOrder.Status = OrderStatus.Complete;
                _orderContext.RemoveOrder(sellOrder);
                _orderContext.AddOrder(sellOrder);
                break;
            }

            return GetOrder(sellOrder.Id);
        }

        private OrderDto HandleBuyOrder(OrderDto buyOrder)
        {
            var outstandingSellOrders = _orderContext.GetProcessingSellOrders()
                .Where(sellOrder => buyOrder.Symbol == sellOrder.Symbol)
                .Where(sellOrder => sellOrder.MinPriceInPoundSterling >= buyOrder.MinPriceInPoundSterling &&
                                    sellOrder.MaxPriceInPoundSterling <= buyOrder.MaxPriceInPoundSterling)
                .OrderBy(sellOrder => sellOrder.OrderPlacedDateTime);

            foreach (var sellOrder in outstandingSellOrders)
            {
                var numberOfShares = Math.Min(buyOrder.QuantityRemaining, sellOrder.QuantityRemaining);

                buyOrder.QuantityRemaining -= numberOfShares;
                sellOrder.QuantityRemaining -= numberOfShares;

                _orderContext.UpdateSellOrder(sellOrder);
                _orderContext.UpdateBuyOrder(buyOrder);

                if (buyOrder.QuantityRemaining == 0)
                {
                    buyOrder.Status = OrderStatus.Complete;
                    _orderContext.RemoveOrder(buyOrder);
                    _orderContext.AddOrder(buyOrder);
                    break;
                }

                if (sellOrder.QuantityRemaining != 0) continue;

                sellOrder.Status = OrderStatus.Complete;
                _orderContext.RemoveOrder(sellOrder);
                _orderContext.AddOrder(sellOrder);
            }

            return GetOrder(buyOrder.Id);
        }
    }
}