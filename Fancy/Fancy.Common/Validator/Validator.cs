using System;

namespace Fancy.Common.Validator
{
    public class Validator
    {
        public static void ValidateNullArgument(Object obj, string paramName)
        {
            if(obj == null)
            {
                string message = string.Format(Messages.Messages.ArgumentNullMessage, paramName);
                throw new ArgumentNullException(message);
            }
        }
    }
}
