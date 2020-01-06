using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Implementation;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Mapping;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Tests
{
	public class BaseValueSegmentTransactionDomainTests
	{
		private readonly BaseValueSegmentTransactionDomain _baseValueSegmentTransactionDomain;
		private readonly Mock<IBaseValueSegmentRepository> _mockRepository;
		private readonly Mock<IGrmEventDomain> _mockGrmEventDomain;
		private readonly Mock<IBaseValueSegmentTransactionRepository> _mockTransactionRepository;

		public BaseValueSegmentTransactionDomainTests()
		{
			Mappings.Init();

			_mockRepository = new Mock<IBaseValueSegmentRepository>();
			_mockGrmEventDomain = new Mock<IGrmEventDomain>();
			_mockTransactionRepository = new Mock<IBaseValueSegmentTransactionRepository>();

			_baseValueSegmentTransactionDomain = new BaseValueSegmentTransactionDomain(
				_mockTransactionRepository.Object,
				_mockRepository.Object, _mockGrmEventDomain.Object);
		}

		[Fact]
		public void GetAsyncWithValidIdGetDto()
		{
			_mockTransactionRepository.Setup(x => x.GetAsync(55))
				.ReturnsAsync(new BaseValueSegmentTransaction
				{
					Id = 55,
					BaseValueSegmentId = 23,
					BaseValueSegmentOwners = new List<BaseValueSegmentOwner>
					{
						new BaseValueSegmentOwner
						{
							Id = 524,
							LegalPartyRoleId = 4664,
							BeneficialInterestPercent = 100,
							GRMEventId = 5325,
							DynCalcStepTrackingId = 533,
							BaseValueSegmentTransactionId = 325
						}
					},
					EffectiveStatus = "A",
					TransactionId = 32532,
					BaseValueSegmentTransactionTypeId = 4564
				});

			var item = _baseValueSegmentTransactionDomain.GetAsync(55).Result;

			item.ShouldNotBeNull();
			item.Id.ShouldBe(55);
			item.BaseValueSegmentId.ShouldBe(23);
			item.BaseValueSegmentOwners.Count.ShouldBe(1);
			item.BaseValueSegmentOwners.Single().Id.ShouldBe(524);
			item.BaseValueSegmentOwners.Single().LegalPartyRoleId.ShouldBe(4664);
			item.BaseValueSegmentOwners.Single().GRMEventId.ShouldBe(5325);
			item.BaseValueSegmentOwners.Single().DynCalcStepTrackingId.ShouldBe(533);
			item.BaseValueSegmentOwners.Single().BaseValueSegmentTransactionId.ShouldBe(325);
			item.BaseValueSegmentOwners.Single().BeneficialInterestPercent.ShouldBe(100);
		}

		[Fact]
		public void CreateBaseValueSegmentTransactionNoOwnersBadRequestExceptionIsThrown()
		{
			var baseValueSegmentTransaction = new BaseValueSegmentTransactionDto();

			Should.ThrowAsync<BadRequestException>(() => _baseValueSegmentTransactionDomain.CreateAsync(baseValueSegmentTransaction));
		}

		[Fact]
		public void CreateBaseValueSegmentTransactionNoValueHeadersBadRequestExceptionIsThrown()
		{
			var baseValueSegmentTransaction = new BaseValueSegmentTransactionDto();
			baseValueSegmentTransaction.BaseValueSegmentOwners.Add(new BaseValueSegmentOwnerDto());

			Should.ThrowAsync<BadRequestException>(() => _baseValueSegmentTransactionDomain.CreateAsync(baseValueSegmentTransaction));
		}

		[Fact]
		public void CreatedGrmEventsAreRolledBackWhenCreateBaseValueSegmentTransactionDuringSavingHasException()
		{
			_mockRepository.Setup(x => x.GetUserTransactionType())
				.Returns(new BaseValueSegmentTransactionType { Id = 3, Name = "foo" });

      _mockRepository.Setup(x => x.GetUserDeletedTransactionType())
        .Returns(new BaseValueSegmentTransactionType { Id = 5, Name = "del", Description = "del" });

      var transaction = BaseValueSegmentHelper.CreateMockTransactionDto();

			_mockGrmEventDomain.Setup(x => x.CreateForTransaction(It.Is<BaseValueSegmentTransactionDto>(b => !b.Id.HasValue)))
				.ReturnsAsync(new[] { 43, 63, 41 });

			_mockTransactionRepository.Setup(x => x.CreateAsync(
					It.IsAny<BaseValueSegmentTransaction>(),
					It.IsAny<List<BaseValueSegmentOwnerValue>>()))
				.Throws<Exception>();

			Should.ThrowAsync<Exception>(() => _baseValueSegmentTransactionDomain.CreateAsync(transaction));

			_mockGrmEventDomain.Verify(x => x.Delete(new[] { 43, 63, 41 }), Times.Once);

			_mockTransactionRepository.Verify(x => x.CreateAsync(
					It.IsAny<BaseValueSegmentTransaction>(),
					It.IsAny<List<BaseValueSegmentOwnerValue>>()), Times.Once);
		}

		[Fact]
		public void CreateAsyncCreatesBothGrmEventsAndRelatedBaseValueSegmentTransactionEntities()
		{
			_mockRepository.Setup(x => x.GetUserTransactionType())
				.Returns(new BaseValueSegmentTransactionType { Id = 3, Name = "foo" });

      _mockRepository.Setup(x => x.GetUserDeletedTransactionType())
        .Returns(new BaseValueSegmentTransactionType { Id = 5, Name = "del", Description = "del" });

      var transaction = BaseValueSegmentHelper.CreateMockTransactionDto();

			_mockGrmEventDomain.Setup(x => x.CreateForTransaction(It.Is<BaseValueSegmentTransactionDto>(b => !b.Id.HasValue)))
				.ReturnsAsync(new[] { 43, 63, 41 });

			var result = _baseValueSegmentTransactionDomain.CreateAsync(transaction).Result;

			result.ShouldNotBeNull();
			result.Id.ShouldNotBeNull();

			var owner = result.BaseValueSegmentOwners.Single();
			owner.LegalPartyRoleId.ShouldBe(46);
			owner.BeneficialInterestPercent.ShouldBe(100);
			owner.Id.ShouldNotBeNull();

			_mockTransactionRepository.Verify(x => x.CreateAsync(
				It.IsAny<BaseValueSegmentTransaction>(),
				It.IsAny<List<BaseValueSegmentOwnerValue>>()), Times.Once);

			_mockGrmEventDomain.Verify(x => x.CreateForTransaction(It.Is<BaseValueSegmentTransactionDto>(b => !b.Id.HasValue)), Times.Once);
			_mockGrmEventDomain.Verify(x => x.Delete(It.IsAny<int[]>()), Times.Never);
		}
	}
}
