using System;
using Microsoft.Extensions.Logging;

namespace TAGov.Common.ExceptionHandler.Web
{
    public class MyHttpExceptionHandler : HttpExceptionHandler
    {
        public MyHttpExceptionHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }

        protected override HttpExceptionResult TransformException(Exception ex)
        {
            if (ex.GetType() == typeof(NullReferenceException))
            { 
                return new HttpExceptionResult { Body = new ApiExceptionMessage("Missing!"), StatusCode = 404 };
            }

            if (ex.GetType() == typeof(ArgumentOutOfRangeException))
            {
                var arg = (ArgumentOutOfRangeException)ex;

                return new HttpExceptionResult { Body = new ApiExceptionMessage(arg.Message), StatusCode = 400 };
            }
            return Handle(ex);
        }
    }
}