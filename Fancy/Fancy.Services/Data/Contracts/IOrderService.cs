using Fancy.Data.Models.Models;
using System.Collections.Generic;

namespace Fancy.Services.Data.Contracts
{
    public interface IOrderService
    {
        void AddItemToBasket(int itemId, string userId);

        void RemoveItemFromBasket(int itemId, string userId);

        void ExecuteOrder(int orederId);

        Order GetOrderInBasket(string userId);

        IEnumerable<Order> PreviousOrders(string userId);
    }
}
