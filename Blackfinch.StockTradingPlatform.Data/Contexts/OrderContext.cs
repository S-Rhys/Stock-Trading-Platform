using System;
using System.Collections.Generic;
using System.Linq;
using Blackfinch.StockTradingPlatform.Data.DTO;
using Blackfinch.StockTradingPlatform.Data.Models;

namespace Blackfinch.StockTradingPlatform.Data.Contexts
{
    public interface IOrderContext
    {
        public List<OrderDto> GetAllOrders();

        public List<OrderDto> GetProcessingOrders();

        public OrderDto GetOrder(Guid guid);

        public OrderDto AddOrder(OrderDto orderDto);

        public List<OrderDto> GetProcessingBuyOrders();
        public List<OrderDto> GetProcessingSellOrders();

        public bool RemoveOrder(OrderDto orderDto);

        public void UpdateSellOrder(OrderDto orderDto);
        public void UpdateBuyOrder(OrderDto orderDto);
    }

    public class OrderContext : IOrderContext
    {
        private readonly List<Order> _completeOrderQueue = new List<Order>();
        private readonly List<Order> _processingBuyOrderQueue = new List<Order>();
        private readonly List<Order> _processingSellOrderQueue = new List<Order>();


        public List<OrderDto> GetAllOrders()
        {
            return _processingBuyOrderQueue.Concat(_processingSellOrderQueue).ToList()
                .Concat(_completeOrderQueue)
                .Select(order => new OrderDto(order)).ToList();
        }

        public List<OrderDto> GetProcessingOrders()
        {
            return _processingBuyOrderQueue.Concat(_processingSellOrderQueue)
                .Select(order => new OrderDto(order)).ToList();
        }

        public OrderDto GetOrder(Guid guid)
        {
            return GetAllOrders().Find(orderDto => orderDto.Id == guid);
        }

        public OrderDto AddOrder(OrderDto orderDto)
        {
            if (orderDto.Status == OrderStatus.Processing)
            {
                if (orderDto.Type == OrderType.Buy)
                    _processingBuyOrderQueue.Add(new Order(orderDto));
                else
                    _processingSellOrderQueue.Add(new Order(orderDto));
            }
            else
            {
                _completeOrderQueue.Add(new Order(orderDto));
            }

            return GetOrder(orderDto.Id);
        }

        public List<OrderDto> GetProcessingBuyOrders()
        {
            return _processingBuyOrderQueue.Select(order => new OrderDto(order)).ToList();
        }

        public List<OrderDto> GetProcessingSellOrders()
        {
            return _processingSellOrderQueue.Select(order => new OrderDto(order)).ToList();
        }

        public bool RemoveOrder(OrderDto orderDto)
        {
            if (orderDto.Type == OrderType.Buy)
            {
                var orderToRemove = _processingBuyOrderQueue.Find(order => order.Id == orderDto.Id);
                if (orderToRemove != null) return _processingBuyOrderQueue.Remove(orderToRemove);
            }
            else
            {
                var orderToRemove = _processingSellOrderQueue.Find(order => order.Id == orderDto.Id);
                if (orderToRemove != null) return _processingSellOrderQueue.Remove(orderToRemove);
            }

            return false;
        }

        public void UpdateSellOrder(OrderDto orderDto)
        {
            //TODO: test null
            var order = _processingSellOrderQueue.Find(orderItem => orderItem.Id == orderDto.Id);
            if (order == null) return;
            order.Status = orderDto.Status;
            order.QuantityRemaining = orderDto.QuantityRemaining;
        }

        public void UpdateBuyOrder(OrderDto orderDto)
        {
            var order = _processingBuyOrderQueue.Find(orderItem => orderItem.Id == orderDto.Id);
            if (order == null) return;
            order.Status = orderDto.Status;
            order.QuantityRemaining = orderDto.QuantityRemaining;
        }
    }
}