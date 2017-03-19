using Fancy.Data.Data;
using Fancy.Data.Models.Models;
using Fancy.Services.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Fancy.Common.Enums;

namespace Fancy.Services.Data
{
    public class OrderService : IOrderService
    {
        private IEfFancyData data;

        public OrderService(IEfFancyData data)
        {
            this.data = data;
        }

        public Order GetOrderInBasket(string userId)
        {
            return this.data.Orders.All.FirstOrDefault(o => o.UserId == userId && o.OrderStatus == OrderStatusType.InBasket);
        }

        public void AddItemToBasket(int itemId, string userId)
        {
            var item = this.data.Items.GetById(itemId);
            var user = this.data.Users.GetById(userId);

            var order = this.GetOrderInBasket(userId);
            if(order == null)
            {
                order = new Order();
                order.User = user;
                order.OrderStatus = OrderStatusType.InBasket;

                this.data.Orders.Add(order);
            }

            order.Items.Add(item);

            item.Quantity--;

            this.data.Commit();
        }

        public void RemoveItemFromBasket(int itemId, string userId)
        {
            var item = this.data.Items.GetById(itemId);
            var user = this.data.Users.GetById(userId);

            var order = this.data.Orders.All.FirstOrDefault(o => o.UserId == userId && o.OrderStatus == OrderStatusType.InBasket);

            order.Items.Remove(item);

            item.Quantity++;

            if(order.Items.Count == 0)
            {
                order.OrderStatus = OrderStatusType.Discarded;
            }
            

            this.data.Commit();
        }

        public void ExecuteOrder(int orderId, decimal totalPrice)
        {
            var order = this.data.Orders.GetById(orderId);
            order.OrderStatus = OrderStatusType.Shipped;
            order.TotalPrice = totalPrice;
            order.PurchaseDate = DateTime.Today;

            this.data.Commit();
        }

        public IEnumerable<Order> PreviousOrders(string userId)
        {
            var previousOrders = this.data.Orders.All.Where(o => o.UserId == userId && o.OrderStatus == OrderStatusType.Shipped).ToList();

            return previousOrders;
        }
    }
}
