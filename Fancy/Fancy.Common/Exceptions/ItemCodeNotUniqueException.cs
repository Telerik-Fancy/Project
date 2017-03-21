using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fancy.Common.Exceptions
{
    public class ItemCodeNotUniqueException : Exception
    {
        public ItemCodeNotUniqueException(int itemCode)
            : base(string.Format(Messages.Messages.ItemNotUniqueMessage, itemCode))
        {

        }
    }
}
