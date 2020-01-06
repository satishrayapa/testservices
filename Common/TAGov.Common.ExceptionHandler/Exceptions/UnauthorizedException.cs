using System;

namespace TAGov.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
	    public UnauthorizedException( string message ) : base( message )
	    {
	    }
    }
}
