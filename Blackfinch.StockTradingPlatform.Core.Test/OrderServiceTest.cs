using System;
using Blackfinch.StockTradingPlatform.Core.Orders;
using Blackfinch.StockTradingPlatform.Data.Contexts;
using Blackfinch.StockTradingPlatform.Data.Models;
using NUnit.Framework;

namespace Blackfinch.StockTradingPlatform.Core.Test
{
    public class OrderServiceTest
    {
        private IOrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var orderContext = new OrderContext();

            _orderService = new OrderService(orderContext);
        }

        [Test]
        public void WhenStarted_NoOrdersExist()
        {
            Assert.IsEmpty(_orderService.GetAllOrders());
        }

        [Test]
        public void WhenOrderIsPlaced_CanBeRetrievedById()
        {
            var orderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            Assert.IsNotNull(_orderService.GetOrder(orderDto.Id));
        }

        [Test]
        public void WhenOrderIsRetrievedThatDoesNotExist_ReturnsNull()
        {
            Assert.IsNull(_orderService.GetOrder(Guid.NewGuid()));
        }

        [Test]
        public void WhenOrderIsPlaced_ItIsSetToProcessing()
        {
            var orderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            Assert.AreEqual(OrderStatus.Processing, _orderService.GetOrder(orderDto.Id).Status);
        }

        [Test]
        public void WhenBuyAndSellOrderIsPlaced_ItIsCompleted()
        {
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Sell);
            Assert.AreEqual(OrderStatus.Complete, _orderService.GetOrder(buyOrderDto.Id).Status);
            Assert.AreEqual(OrderStatus.Complete, _orderService.GetOrder(sellOrderDto.Id).Status);
        }

        [Test]
        public void When40BuyAnd20SellOrderIsPlaced_BuyIsProcessingAndSellIsComplete()
        {
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 40, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 20, OrderType.Sell);
            Assert.AreEqual(OrderStatus.Processing, _orderService.GetOrder(buyOrderDto.Id).Status);
            Assert.AreEqual(OrderStatus.Complete, _orderService.GetOrder(sellOrderDto.Id).Status);
        }

        [Test]
        public void When40SellAnd20BuyOrderIsPlaced_BuyIsProcessingAndSellIsComplete()
        {
            var sellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 20, OrderType.Sell);
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 40, OrderType.Buy);
            Assert.AreEqual(OrderStatus.Complete, _orderService.GetOrder(sellOrderDto.Id).Status);
            Assert.AreEqual(OrderStatus.Processing, _orderService.GetOrder(buyOrderDto.Id).Status);
        }

        [Test]
        public void WhenBuyAndSellOrderIsPlaced_RemaingQuantityIsUpdated()
        {
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Sell);
            Assert.AreEqual(0, _orderService.GetOrder(buyOrderDto.Id).QuantityRemaining);
            Assert.AreEqual(0, _orderService.GetOrder(sellOrderDto.Id).QuantityRemaining);
        }

        [Test]
        public void WhenOrdersArePlaced_AllOrdersReturnsAll()
        {
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Sell);
            var extraSellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Sell);
            Assert.AreEqual(3, _orderService.GetAllOrders().Count);
        }

        [Test]
        public void WhenOrdersArePlaced_ProcessingOrdersReturns()
        {
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Sell);
            var extraSellOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Sell);
            Assert.AreEqual(1, _orderService.GetProcessingOrders().Count);
        }

        [Test]
        public void WhenOrdersWithDifferentPricingArePlaced_TheyDontComplete()
        {
            var buyOrderDto = _orderService.PlaceOrder("SEA", 40m, 50m, 100, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 60m, 70m, 100, OrderType.Sell);
            Assert.AreEqual(2, _orderService.GetProcessingOrders().Count);
        }

        [Test]
        public void WhenOrdersArePlaces_OldestIsCompletedFirst()
        {
            var oldestBuyOrderDto = _orderService.PlaceOrder("SEA", 50m, 60m, 100, OrderType.Buy);
            var newestBuyOrderDto = _orderService.PlaceOrder("SEA", 50m, 60m, 100, OrderType.Buy);
            var sellOrderDto = _orderService.PlaceOrder("SEA", 50m, 60m, 100, OrderType.Sell);
            Assert.AreEqual(OrderStatus.Complete, _orderService.GetOrder(oldestBuyOrderDto.Id).Status);
            Assert.AreEqual(OrderStatus.Processing, _orderService.GetOrder(newestBuyOrderDto.Id).Status);
        }
    }
}