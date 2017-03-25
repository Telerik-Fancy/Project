using System;

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
