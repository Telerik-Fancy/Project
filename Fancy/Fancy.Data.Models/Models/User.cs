using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fancy.Data.Models.Models
{
    public class User : IdentityUser
    {
        private ICollection<Order> orders;

        public User()
        {
            this.orders = new HashSet<Order>();
        }

        public virtual ICollection<Order> Orders
        {
            get { return this.orders; }

            set { this.orders = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
