using System;

namespace Fancy.Common.Exceptions
{
    public class ObjectNotFoundInDatabaseException : Exception
    {
        public ObjectNotFoundInDatabaseException(string parameterType) 
            : base(string.Format(Messages.Messages.ObjectNotFoundInDatabaseMessage, parameterType))
        {

        }
    }
}
