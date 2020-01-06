using System;

namespace TAGov.Common.Exceptions
{
    /// <summary>
    /// Application should throw this exception when needing to return a user-friendly message based on the message parameter.
    /// </summary>
    public class ApplicationHandledException : Exception
    {
        public ApplicationHandledException(string message) : base(message)
        {
        }
    }
}
