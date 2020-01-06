using System;

namespace TAGov.Common.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException( string message ) : base( message )
        {
        }
    }
}
