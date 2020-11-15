using System;
using Blackfinch.StockTradingPlatform.Data.DTO;

namespace Blackfinch.StockTradingPlatform.Data.Models
{
    public class Order
    {
        public Order(string symbol, decimal minPriceInPoundSterling, decimal maxPriceInPoundSterling, int quantity,
            OrderType type)
        {
            OrderPlacedDateTime = DateTime.UtcNow;
            Status = OrderStatus.Processing;
            Quantity = quantity;
            QuantityRemaining = quantity;
            Type = type;
            Symbol = symbol;
            MinPriceInPoundSterling = minPriceInPoundSterling;
            MaxPriceInPoundSterling = maxPriceInPoundSterling;
            Id = Guid.NewGuid();
        }

        public Order(OrderDto orderDto)
        {
            OrderPlacedDateTime = orderDto.OrderPlacedDateTime;
            Status = orderDto.Status;
            Quantity = orderDto.Quantity;
            QuantityRemaining = orderDto.QuantityRemaining;
            Type = orderDto.Type;
            Symbol = orderDto.Symbol;
            MinPriceInPoundSterling = orderDto.MinPriceInPoundSterling;
            MaxPriceInPoundSterling = orderDto.MaxPriceInPoundSterling;
            Id = orderDto.Id;
        }

        public string Symbol { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal MinPriceInPoundSterling { get; set; }
        public decimal MaxPriceInPoundSterling { get; set; }
        public int Quantity { get; set; }
        public OrderType Type { get; set; }
        public Guid Id { get; }
        public OrderStatus Status { get; set; }
        public DateTime OrderPlacedDateTime { get; }
    }
}