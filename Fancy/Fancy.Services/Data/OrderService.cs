using Fancy.Data.Data;
using Fancy.Data.Models.Models;
using Fancy.Services.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Fancy.Common.Enums;
using Fancy.Common.Validator;
using Fancy.Common.Constants;

namespace Fancy.Services.Data
{
    public class OrderService : IOrderService
    {
        private IEfFancyData data;

        public OrderService(IEfFancyData data)
        {
            Validator.ValidateNullArgument(data, "data");

            this.data = data;
        }

        public Order GetOrderInBasket(string userId)
        {
            Validator.ValidateNullArgument(userId, "userId");

            var user = this.data.Users.GetById(userId);
            
            Validator.ValidateNullDatabaseObject(user, "User");

            var order = this.data.Orders.GetSingleOrDefault(o => o.UserId == userId && o.OrderStatus == OrderStatusType.InBasket);

            if (order == null)
            {
                order = new Order();
                order.User = user;
                order.OrderStatus = OrderStatusType.InBasket;

                this.data.Orders.Add(order);

                this.data.Commit();
            }

            return order;
        }

        public void AddItemToBasket(int itemId, string userId)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");
            Validator.ValidateNullArgument(userId, "userId");

            var item = this.data.Items.GetById(itemId);
            var user = this.data.Users.GetById(userId);
            var order = this.GetOrderInBasket(userId);

            Validator.ValidateNullDatabaseObject(item, "Item");
            Validator.ValidateNullDatabaseObject(user, "User");

            order.Items.Add(item);

            item.Quantity--;

            this.data.Commit();
        }

        public void RemoveItemFromBasket(int itemId, string userId)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");
            Validator.ValidateNullArgument(userId, "userId");

            var item = this.data.Items.GetById(itemId);
            var user = this.data.Users.GetById(userId);
            var order = this.data.Orders.GetSingleOrDefault(o => o.UserId == userId && o.OrderStatus == OrderStatusType.InBasket);

            Validator.ValidateNullDatabaseObject(item, "Item");
            Validator.ValidateNullDatabaseObject(user, "User");
            Validator.ValidateNullDatabaseObject(order, "Order");

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
            Validator.ValidateRange(orderId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "orderId");
            Validator.ValidateRange(totalPrice, 0, decimal.MaxValue, "totalPrice");

            var order = this.data.Orders.GetById(orderId);

            Validator.ValidateNullDatabaseObject(order, "Order");

            order.OrderStatus = OrderStatusType.Shipped;
            order.TotalPrice = totalPrice;
            order.PurchaseDate = DateTime.Today;

            this.data.Commit();
        }

        public IEnumerable<Order> PreviousOrders(string userId)
        {
            Validator.ValidateNullArgument(userId, "userId");

            var previousOrders = this.data.Orders.GetAll(o => o.UserId == userId && o.OrderStatus == OrderStatusType.Shipped).ToList();

            return previousOrders;
        }
    }
}
