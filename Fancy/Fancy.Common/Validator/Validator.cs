using Fancy.Common.Exceptions;
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

        public static void ValidateRange(decimal value, decimal min, decimal max, string paramName)
        {
            if (value < min || value > max)
            {
                string message = string.Format(Messages.Messages.ArgumentOutOfRangeMessage, paramName);

                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateNullDatabaseObject(Object obj, string parameterType)
        {
            if (obj == null)
            {
                throw new ObjectNotFoundInDatabaseException(parameterType);
            }
        }
    }
}
