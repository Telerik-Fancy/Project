using Fancy.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fancy.Data.Contexts.Interfaces
{
    public interface IFancyDbContext
    {
        IDbSet<User> Users { get; }

    }
}
