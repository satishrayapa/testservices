using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using Xunit;

namespace TAGov.Common.ExceptionHandler.Tests
{
	public class HttpExceptionHandlerTests
	{
		[Fact]
		public void WhenHandlingUnhandledArgumentException_ReturnMessageAnd500StatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new ArgumentException("test"));

			result.StatusCode.ShouldBe(500);
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingApplicationHandledException_ReturnMessageAndBadRequestStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new ApplicationHandledException("test"));

			result.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingNotFoundException_ReturnMessageAndNotFoundStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new NotFoundException("test"));

			result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingInternalServerErrorException_ReturnMessageAndInternalServerErrorStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new InternalServerErrorException("test"));

			result.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingRecordNotFoundException_ReturnMessageAndNotFoundStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new RecordNotFoundException("Some Id", typeof(HttpExceptionHandlerTests), "test"));

			result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
			result.Body.Message.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingAggregateExceptionUseInnerNotFoundException_ReturnMessageAndNotFoundStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();

			var aggregateException = new AggregateException(new RecordNotFoundException("Some Id", typeof(HttpExceptionHandlerTests), "test"));
			var result = test.Handle(aggregateException);

			result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
			result.Body.Message.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingDuplicateRecordException_ReturnMessageAndConfictStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new DuplicateRecordException("DuplicateTest"));

			result.StatusCode.ShouldBe((int)HttpStatusCode.Conflict);
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingLockedException_ReturnMessageAndLockedStatusCode()
		{
			var test = GetHttpExceptionHandlerWithMockLoggerFactory();
			var result = test.Handle(new LockedException("LockedTest"));

			result.StatusCode.ShouldBe(423);
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingForbiddenException_ReturnMesasageAndForbiddenStatusCode()
		{
			HttpExceptionHandler test = GetHttpExceptionHandlerWithMockLoggerFactory();
			HttpExceptionResult result = test.Handle( new ForbiddenException( "ForbiddenTest" ) );

			result.StatusCode.ShouldBe( ( int ) HttpStatusCode.Forbidden );
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		[Fact]
		public void WhenHandlingUnauthorizedException_ReturnMessageAndUnauthorizedStatusCode()
		{
			HttpExceptionHandler test = GetHttpExceptionHandlerWithMockLoggerFactory();
			HttpExceptionResult result = test.Handle( new UnauthorizedException( "UnauthorizedTest" ) );

			result.StatusCode.ShouldBe( ( int ) HttpStatusCode.Unauthorized );
			result.Body.Message.ShouldNotBeNullOrEmpty();
			result.Body.ErrorId.ShouldBeNullOrEmpty();
		}

		private HttpExceptionHandler GetHttpExceptionHandlerWithMockLoggerFactory()
		{
			var logger = new Mock<ILogger>();

			var loggerFactory = new Mock<ILoggerFactory>();
			loggerFactory.Setup(x => x.CreateLogger("HttpExceptionHandler")).Returns(logger.Object);

			return new HttpExceptionHandler(loggerFactory.Object);
		}
	}
}
