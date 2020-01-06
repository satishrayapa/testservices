using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Implementation;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Tests
{
	public class BaseValueSegmentFlagDomainTests
	{
		private readonly Mock<IBaseValueSegmentFlagRepository> _baseValueSegmentFlagRepository;

		private readonly IBaseValueSegmentFlagDomain _baseValueSegmentFlagDomain;
		public BaseValueSegmentFlagDomainTests()
		{
			_baseValueSegmentFlagRepository = new Mock<IBaseValueSegmentFlagRepository>();

			_baseValueSegmentFlagDomain = new BaseValueSegmentFlagDomain(_baseValueSegmentFlagRepository.Object);
		}

		[Fact]
		public void ListAsyncNoRecordsReturnedGetRecordNotFoundException()
		{
			_baseValueSegmentFlagRepository.Setup(x => x.ListAsync(12345))
				.ReturnsAsync(Enumerable.Empty<BaseValueSegmentFlag>());

			Should.ThrowAsync<RecordNotFoundException>(() => _baseValueSegmentFlagDomain.ListAsync(12345));
		}

		[Fact]
		public void ListAsyncRecordsReturnedGetFlags()
		{
			_baseValueSegmentFlagRepository.Setup(x => x.ListAsync(12345))
				.ReturnsAsync(new List<BaseValueSegmentFlag>
				{
					new BaseValueSegmentFlag{ Id = 1, Description = "foo", RevenueObjectId = 12345},
					new BaseValueSegmentFlag{ Id = 2, Description = "bar", RevenueObjectId = 12345}
				});

			var results = _baseValueSegmentFlagDomain.ListAsync(12345).Result.ToList();

			results.Count.ShouldBe(2);
			var foo = results.Single(x => x.Id == 1);
			foo.Id.ShouldBe(1);
			foo.Description.ShouldBe("foo");
			foo.RevenueObjectId.ShouldBe(12345);

			var bar = results.Single(x => x.Id == 2);
			bar.Id.ShouldBe(2);
			bar.Description.ShouldBe("bar");
			bar.RevenueObjectId.ShouldBe(12345);
		}
	}
}
