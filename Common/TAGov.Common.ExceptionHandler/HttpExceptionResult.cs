namespace TAGov.Common
{
    /// <summary>
    /// Used internally to support a generic way of passing ochestration data back to front-end.
    /// </summary>
    /// <remarks>PLEASE DO NOT USE!</remarks>
    public class HttpExceptionResult
    {
        /// <summary>
        /// Gets or sets the Status Code appropriate for the exception.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the POCO class that represents the exception from processing of the API.
        /// </summary>
        public ApiExceptionMessage Body { get; set; }
    }
}
