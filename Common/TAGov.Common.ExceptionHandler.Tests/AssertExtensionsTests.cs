using System.Collections.Generic;
using Shouldly;
using TAGov.Common.Exceptions;
using Xunit;

namespace TAGov.Common.ExceptionHandler.Tests
{
	public class TestClass
	{
		public int Id { get; set; }
	}

	public class AssertExtensionsTests
	{
		[Fact]
		public void ThrowBadRequestExceptionIfIdIsNegative()
		{
			const int id = -1;
			Should.Throw<BadRequestException>(() => id.ThrowBadRequestExceptionIfInvalid("SomeId"));
		}

		[Fact]
		public void DoesNotThrowBadRequestExceptionIfIdIsPositive()
		{
			const int id = 1;
			Should.NotThrow(() => id.ThrowBadRequestExceptionIfInvalid("SomeId"));
		}

		[Fact]
		public void ForNulllableIdThrowBadRequestExceptionIfIdIsNull()
		{
			int? id = null;
			// ReSharper disable once ExpressionIsAlwaysNull
			Should.Throw<BadRequestException>(() => id.ThrowBadRequestExceptionIfInvalidOnNullable("SomeId"));
		}

		[Fact]
		public void ForNulllableIdThrowBadRequestExceptionIfIdIsNegative()
		{
			int? id = -1;
			Should.Throw<BadRequestException>(() => id.ThrowBadRequestExceptionIfInvalidOnNullable("SomeId"));
		}

		[Fact]
		public void ForNulllableIdDoesNotThrowBadRequestExceptionIfIdIsPositive()
		{
			int? id = 1;
			Should.NotThrow(() => id.ThrowBadRequestExceptionIfInvalidOnNullable("SomeId"));
		}

		[Fact]
		public void ThrowRecordNotFoundExceptionIfItemIsNull()
		{
			TestClass testClass = null;
			// ReSharper disable once ExpressionIsAlwaysNull
			var e = Should.Throw<RecordNotFoundException>(() => testClass.ThrowRecordNotFoundExceptionIfNull(
				 new IdInfo("id1", "5"),
				 new IdInfo("id2", "233")));

			e.RecordId.ShouldContain("id1=5");
			e.RecordId.ShouldContain("id2=233");
		}

		[Fact]
		public void DoesNotThrowRecordNotFoundExceptionIfItemIsNotNull()
		{
			TestClass testClass = new TestClass();
			// ReSharper disable once ExpressionIsAlwaysNull
			Should.NotThrow(() => testClass.ThrowRecordNotFoundExceptionIfNull(
				new IdInfo("foo", "4"),
				new IdInfo("bar", "78")));
		}
	}
}
