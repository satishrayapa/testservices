using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using TAGov.Common.Exceptions;

namespace TAGov.Common
{
	/// <summary>
	/// Concrete implementation for handling the exception(s) fired from the application.
	/// </summary>
	public class HttpExceptionHandler : IHttpExceptionHandler
	{
		private readonly ILogger _logger;
		private readonly EventId _eventId = new EventId(-1);

		public HttpExceptionHandler(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger("HttpExceptionHandler");
		}

		public HttpExceptionResult Handle(Exception ex)
		{
			if (ex.GetType() == typeof(AggregateException))
			{
				var aggregateException = (AggregateException)ex;

				// If there is only 1 exception, we should just handle that one because that
				// should be the root cause of the exception. Otherwise, let it fall naturally
				// to a unknown, unhandled exception.
				if (aggregateException.InnerExceptions.Count == 1)
					ex = aggregateException.InnerExceptions.First();
			}

			// Handled exceptions will have user-friendly messages.
			if (IsApplicationHandledException(ex))
			{
				LogHandledException(ex);

				return TransformException(ex);
			}

			// All other exceptions will land here. We need to add a error Id as a reference.
			var errorId = GetErrorId();
			_logger.LogError(_eventId, ex, "Unhandled exception - Error Id {0}", errorId);

			return new HttpExceptionResult
			{
				Body = new ApiExceptionMessage("An unhandled exception has occured. Please contact our technical support team for additional assistance.") { ErrorId = errorId },
				StatusCode = (int)HttpStatusCode.InternalServerError
			};
		}

		private void LogHandledException(Exception ex)
		{
			if (ex.GetType() == typeof(RecordNotFoundException))
			{
				var recordNotFoundException = (RecordNotFoundException)ex;
				_logger.LogInformation(_eventId, ex, "Handled exception with Record Id: {0} of Type: {1}",
					recordNotFoundException.RecordId,
					recordNotFoundException.Type);
			}
			else
			{
				_logger.LogInformation(_eventId, ex, "Handled exception");
			}
		}

		private string GetErrorId()
		{
			return Guid.NewGuid().ToString("N");
		}

		/// <summary>
		/// Override if you need to determine what exceptions consitutes an application exception.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		protected virtual bool IsApplicationHandledException(Exception ex)
		{
			return ex.GetType() == typeof(ApplicationHandledException) ||
				ex.GetType() == typeof(BadRequestException) ||
				ex.GetType() == typeof(NotFoundException) ||
				ex.GetType() == typeof(RecordNotFoundException) ||
				ex.GetType() == typeof(InternalServerErrorException) ||
                ex.GetType() == typeof(DuplicateRecordException) ||
				ex.GetType() == typeof(LockedException) ||
				ex.GetType() == typeof(ForbiddenException) ||
				ex.GetType() == typeof(UnauthorizedException);
        }

        /// <summary>
        /// Transform exception into an internal exception ochestration data so that we can return the appropriate web-api structure.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual HttpExceptionResult TransformException(Exception ex)
		{
			var statusCode = (int)HttpStatusCode.BadRequest;

			if (ex.GetType() == typeof(NotFoundException) || ex.GetType() == typeof(RecordNotFoundException))
			{
				statusCode = (int)HttpStatusCode.NotFound;
			}

			if (ex.GetType() == typeof(InternalServerErrorException))
			{
				statusCode = (int)HttpStatusCode.InternalServerError;
			}

            if (ex.GetType() == typeof(DuplicateRecordException))
            {
                statusCode = (int)HttpStatusCode.Conflict;
            }

			if ( ex.GetType() == typeof( LockedException ) )
			{
				//423 is HTTP status code for locked but it is not defined in the HttpStatusCode enum
				statusCode = 423;
			}

			if ( ex.GetType() == typeof( ForbiddenException ) )
			{
				statusCode = ( int ) HttpStatusCode.Forbidden;
			}

			if ( ex.GetType() == typeof( UnauthorizedException ) )
			{
				statusCode = ( int ) HttpStatusCode.Unauthorized;
			}

            return new HttpExceptionResult
			{
				Body = new ApiExceptionMessage(ex.Message),
				StatusCode = statusCode
			};
		}
	}
}
