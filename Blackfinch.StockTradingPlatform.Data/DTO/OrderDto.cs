using System;
using Blackfinch.StockTradingPlatform.Data.Models;

namespace Blackfinch.StockTradingPlatform.Data.DTO
{
    public class OrderDto
    {
        public OrderDto(string symbol, decimal minPriceInPoundSterling, decimal maxPriceInPoundSterling, int quantity,
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

        public OrderDto(Order order)
        {
            OrderPlacedDateTime = order.OrderPlacedDateTime;
            Status = order.Status;
            Quantity = order.Quantity;
            QuantityRemaining = order.QuantityRemaining;
            Type = order.Type;
            Symbol = order.Symbol;
            MinPriceInPoundSterling = order.MinPriceInPoundSterling;
            MaxPriceInPoundSterling = order.MaxPriceInPoundSterling;
            Id = order.Id;
        }

        public string Symbol { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal MinPriceInPoundSterling { get; set; }
        public decimal MaxPriceInPoundSterling { get; set; }
        public int Quantity { get; set; }
        public OrderType Type { get; set; }
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderPlacedDateTime { get; }
    }
}