using System;

namespace TAGov.Common.Exceptions
{
    public class LockedException : Exception
    {
	    public LockedException( string message ) : base( message )
	    {
	    }
    }
}
