using System;

namespace TAGov.Common
{
    /// <summary>
    /// Internal interface to ochestrate exception handling. 
    /// </summary>
    public interface IHttpExceptionHandler
    {
        /// <summary>
        /// Handles an exception and returns the ochestration data back to front-end.
        /// </summary>
        /// <param name="ex">ex.</param>
        /// <returns>HttpExceptionResult.</returns>
        HttpExceptionResult Handle(Exception ex);
    }
}
