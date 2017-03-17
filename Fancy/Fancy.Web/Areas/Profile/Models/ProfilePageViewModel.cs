using System.Collections.Generic;

namespace Fancy.Web.Areas.Profile.Models
{
    public class ProfilePageViewModel
    {
        public OrderViewModel OrderInBasket { get; set; }

        public IEnumerable<OrderViewModel> PreviousOrders { get; set; }
    }
}