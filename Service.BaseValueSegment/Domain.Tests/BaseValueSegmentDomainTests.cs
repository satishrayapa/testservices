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
	public class BaseValueSegmentDomainTests
	{
		private readonly Mock<IBaseValueSegmentRepository> _mockRepository;
		private readonly Mock<IGrmEventDomain> _mockGrmEventDomain;
		private readonly BaseValueSegmentDomain _baseValueSegmentDomain;

		public BaseValueSegmentDomainTests()
		{
			_mockRepository = new Mock<IBaseValueSegmentRepository>();
			_mockGrmEventDomain = new Mock<IGrmEventDomain>();
			_baseValueSegmentDomain = new BaseValueSegmentDomain(_mockRepository.Object, _mockGrmEventDomain.Object);

			Mappings.Init();
		}

		[Fact]
		public void CreateBaseValueSegmentWithValidIdGetBadRequestException()
		{
			var baseValueSegment = new BaseValueSegmentDto { Id = 0 };

			Should.ThrowAsync<BadRequestException>(() => _baseValueSegmentDomain.CreateAsync(baseValueSegment));
		}

		[Fact]
		public void CreateBaseValueSegmentNoTransactionBadRequestExceptionIsThrown()
		{
			var baseValueSegment = new BaseValueSegmentDto();

			Should.ThrowAsync<BadRequestException>(() => _baseValueSegmentDomain.CreateAsync(baseValueSegment));
		}

		[Fact]
		public void CreateBaseValueSegmentNoOwnersBadRequestExceptionIsThrown()
		{
			var baseValueSegment = new BaseValueSegmentDto();
			baseValueSegment.BaseValueSegmentTransactions.Add(new BaseValueSegmentTransactionDto());

			Should.ThrowAsync<BadRequestException>(() => _baseValueSegmentDomain.CreateAsync(baseValueSegment));
		}

		[Fact]
		public void CreateBaseValueSegmentNoValueHeaderBadRequestExceptionIsThrown()
		{
			var baseValueSegment = new BaseValueSegmentDto();
			baseValueSegment.BaseValueSegmentTransactions.Add(new BaseValueSegmentTransactionDto
			{
				BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
				{
					new BaseValueSegmentOwnerDto()
				}
			});

			Should.ThrowAsync<BadRequestException>(() => _baseValueSegmentDomain.CreateAsync(baseValueSegment));
		}

		[Fact]
		public void CreatedGrmEventsAreRolledBackWhenCreateBaseValueSegmentDuringSavingHasException()
		{
			var baseValueSegment = BaseValueSegmentHelper.CreateMockDto();

			_mockGrmEventDomain.Setup(x => x.Create(It.Is<BaseValueSegmentDto>(b => !b.Id.HasValue)))
				.ReturnsAsync(new[] { 43, 63, 41 });

			_mockRepository.Setup(x => x.CreateAsync(
					It.IsAny<Repository.Models.V1.BaseValueSegment>(),
					It.IsAny<List<BaseValueSegmentOwnerValue>>()))
				.Throws<Exception>();

			_mockRepository.Setup(x => x.GetUserTransactionType())
				.Returns(new BaseValueSegmentTransactionType { Id = 3, Name = "foo", Description = "bar" });

			_mockRepository.Setup(x => x.GetNewStatusType())
				.Returns(new BaseValueSegmentStatusType { Id = 4, Name = "bar", Description = "bar" });

      _mockRepository.Setup(x => x.GetUserDeletedTransactionType())
                     .Returns(new BaseValueSegmentTransactionType { Id = 5, Name = "del", Description = "del" });

      Should.ThrowAsync<Exception>(() => _baseValueSegmentDomain.CreateAsync(baseValueSegment));

			_mockGrmEventDomain.Verify(x => x.Delete(new[] { 43, 63, 41 }), Times.Once);

			_mockRepository.Verify(x => x.CreateAsync(It.IsAny<Repository.Models.V1.BaseValueSegment>(),
				It.IsAny<List<BaseValueSegmentOwnerValue>>()), Times.Once);
		}

    [Fact]
    public void BaseValueSegmentToEntityDoesNotFail()
    {
      var baseValueSegment = BaseValueSegmentHelper.CreateMockDto();

      Should.NotThrow( () => baseValueSegment.ToEntity()) ;
    }

    [Fact]
		public void CreateAsyncCreatesBothGrmEventsAndRelatedBaseValueSegmentEntities()
		{
			_mockRepository.Setup(x => x.GetUserTransactionType())
				.Returns(new BaseValueSegmentTransactionType { Id = 3, Name = "foo", Description = "foo" });

      _mockRepository.Setup(x => x.GetNewStatusType())
				.Returns(new BaseValueSegmentStatusType { Id = 4, Name = "bar", Description = "bar" });

      _mockRepository.Setup(x => x.GetUserDeletedTransactionType())
                     .Returns(new BaseValueSegmentTransactionType { Id = 5, Name = "del", Description = "del" });

      var baseValueSegment = BaseValueSegmentHelper.CreateMockDto();

			_mockGrmEventDomain.Setup(x => x.Create(It.Is<BaseValueSegmentDto>(b => !b.Id.HasValue)))
				.ReturnsAsync(new[] { 43, 63, 41 });

			var result = _baseValueSegmentDomain.CreateAsync(baseValueSegment).Result;

			result.ShouldNotBeNull();
			result.Id.ShouldNotBeNull();
			result.AsOf.ShouldBe(baseValueSegment.AsOf);
			result.AssessmentEventTransactionId.ShouldNotBe(435);
			result.BaseValueSegmentTransactions.Count.ShouldBe(1);

			var transaction = result.BaseValueSegmentTransactions.Single();
			transaction.BaseValueSegmentId.ShouldNotBeNull();
			transaction.BaseValueSegmentOwners.Count.ShouldBe(1);
			transaction.BaseValueSegmentTransactionType.Name.ShouldBe("foo");
			transaction.BaseValueSegmentTransactionType.Description.ShouldBe("foo");

			var owner = transaction.BaseValueSegmentOwners.Single();
			owner.LegalPartyRoleId.ShouldBe(46);
			owner.BeneficialInterestPercent.ShouldBe(100);
			owner.Id.ShouldNotBeNull();

			result.BaseValueSegmentAssessmentRevisions.Count.ShouldBe(1);
			var assessmentRevision = result.BaseValueSegmentAssessmentRevisions.Single();
			assessmentRevision.ReviewMessage.ShouldBe("foo bar");
			assessmentRevision.BaseValueSegmentStatusType.Name.ShouldBe("bar");
			assessmentRevision.BaseValueSegmentStatusType.Description.ShouldBe("bar");

			_mockRepository.Verify(x => x.CreateAsync(
				It.IsAny<Repository.Models.V1.BaseValueSegment>(),
				It.IsAny<List<BaseValueSegmentOwnerValue>>()), Times.Once);

			_mockGrmEventDomain.Verify(x => x.Create(It.Is<BaseValueSegmentDto>(b => !b.Id.HasValue)), Times.Once);
			_mockGrmEventDomain.Verify(x => x.Delete(It.IsAny<int[]>()), Times.Never);
		}

    [Fact]
    public void CreateAsyncCreatesBothGrmEventsAndRelatedBaseValueSegmentEntitiesAndRetainsUserDeletedTransactionType()
    {
      _mockRepository.Setup(x => x.GetUserTransactionType())
        .Returns(new BaseValueSegmentTransactionType { Id = 3, Name = "foo", Description = "foo" });

      _mockRepository.Setup(x => x.GetNewStatusType())
        .Returns(new BaseValueSegmentStatusType { Id = 4, Name = "bar", Description = "bar" });

      _mockRepository.Setup(x => x.GetUserDeletedTransactionType())
                     .Returns(new BaseValueSegmentTransactionType { Id = 5, Name = "del", Description = "del" });

      var baseValueSegment = BaseValueSegmentHelper.CreateMockDto();

      baseValueSegment.BaseValueSegmentTransactions[0].BaseValueSegmentTransactionType = new BaseValueSegmentTransactionTypeDto { Name = "del", Description = "del" };
      baseValueSegment.BaseValueSegmentTransactions[ 0 ].BaseValueSegmentTransactionTypeId = 5;

      _mockGrmEventDomain.Setup(x => x.Create(It.Is<BaseValueSegmentDto>(b => !b.Id.HasValue)))
        .ReturnsAsync(new[] { 43, 63, 41 });

      var result = _baseValueSegmentDomain.CreateAsync(baseValueSegment).Result;

      result.ShouldNotBeNull();
      result.Id.ShouldNotBeNull();
      result.AsOf.ShouldBe(baseValueSegment.AsOf);
      result.AssessmentEventTransactionId.ShouldNotBe(435);
      result.BaseValueSegmentTransactions.Count.ShouldBe(1);

      var transaction = result.BaseValueSegmentTransactions.Single();
      transaction.BaseValueSegmentId.ShouldNotBeNull();
      transaction.BaseValueSegmentOwners.Count.ShouldBe(1);
      transaction.BaseValueSegmentTransactionType.Name.ShouldBe("del");
      transaction.BaseValueSegmentTransactionType.Description.ShouldBe("del");

      var owner = transaction.BaseValueSegmentOwners.Single();
      owner.LegalPartyRoleId.ShouldBe(46);
      owner.BeneficialInterestPercent.ShouldBe(100);
      owner.Id.ShouldNotBeNull();

      result.BaseValueSegmentAssessmentRevisions.Count.ShouldBe(1);
      var assessmentRevision = result.BaseValueSegmentAssessmentRevisions.Single();
      assessmentRevision.ReviewMessage.ShouldBe("foo bar");
      assessmentRevision.BaseValueSegmentStatusType.Name.ShouldBe("bar");
      assessmentRevision.BaseValueSegmentStatusType.Description.ShouldBe("bar");

      _mockRepository.Verify(x => x.CreateAsync(
        It.IsAny<Repository.Models.V1.BaseValueSegment>(),
        It.IsAny<List<BaseValueSegmentOwnerValue>>()), Times.Once);

      _mockGrmEventDomain.Verify(x => x.Create(It.Is<BaseValueSegmentDto>(b => !b.Id.HasValue)), Times.Once);
      _mockGrmEventDomain.Verify(x => x.Delete(It.IsAny<int[]>()), Times.Never);
    }
		#region Get Tests

		[Fact]
		public void GetBaseValueSegment_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
		{
			var baseValueSgement = new Repository.Models.V1.BaseValueSegment { Id = 999 };

			_mockRepository.Setup(x => x.Get(999)).Returns(baseValueSgement);

			var returnBaseValueSegment = _baseValueSegmentDomain.Get(999);

			returnBaseValueSegment.Id.ShouldBe(baseValueSgement.Id);
		}

		[Fact]
		public void GetBaseValueSegment_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
		{
			var baseValueSegment = new Repository.Models.V1.BaseValueSegment();

			_mockRepository.Setup(x => x.Get(999)).Returns(baseValueSegment);

			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.Get(1));
		}

		[Fact]
		public void GetBaseValueSegment_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
		{
			var baseValueSegment = new Repository.Models.V1.BaseValueSegment();

			_mockRepository.Setup(x => x.Get(999)).Returns(baseValueSegment);

			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.Get(0));
		}

		#endregion

		#region Get By RevenueObjectId and AssessmentEventDate Tests

		[Fact]
		public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
		{
			DateTime assessmentEventDate = new DateTime(1999, 1, 1);
			var baseValueSgement = new Repository.Models.V1.BaseValueSegment { Id = 999, AsOf = assessmentEventDate };

			_mockRepository.Setup(x => x.GetByRevenueObjectIdAndAssessmentEventDate(999, assessmentEventDate)).Returns(baseValueSgement);

			var returnBaseValueSegment = _baseValueSegmentDomain.GetByRevenueObjectIdAndAssessmentEventDate(999, assessmentEventDate);

			returnBaseValueSegment.Id.ShouldBe(baseValueSgement.Id);
		}

		[Fact]
		public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
		{
			DateTime assessmentEventDate = new DateTime(1999, 1, 1);
			var baseValueSegment = new Repository.Models.V1.BaseValueSegment();

			_mockRepository.Setup(x => x.GetByRevenueObjectIdAndAssessmentEventDate(999, assessmentEventDate)).Returns(baseValueSegment);

			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.GetByRevenueObjectIdAndAssessmentEventDate(1, assessmentEventDate));
		}

		[Fact]
		public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
		{
			DateTime assessmentEventDate = new DateTime(1999, 1, 1);
			var baseValueSegment = new Repository.Models.V1.BaseValueSegment();

			_mockRepository.Setup(x => x.GetByRevenueObjectIdAndAssessmentEventDate(999, assessmentEventDate)).Returns(baseValueSegment);

			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.GetByRevenueObjectIdAndAssessmentEventDate(0, assessmentEventDate));
		}

		#endregion

		#region Get BaseValueSegmentEvent By RevenueObjectId Tests

		[Fact]
		public void GetBaseValueSegmentEventByRevenueObjectId_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
		{

			List<BaseValueSegmentEvent> baseValueSegmentEvents = new List<BaseValueSegmentEvent>
																	{
																		new BaseValueSegmentEvent
																		{
																			BvsId = 123,
																			BvsAsOf = new DateTime(2014, 1, 1),
																			RevenueObjectId = 231,
																			SequenceNumber = 1,
																			GRMEventId = 100
																		}
																	};
			_mockRepository.Setup(x => x.GetBvsEventsByRevenueObjectId(999)).Returns(baseValueSegmentEvents);

			var returnBaseValueSegments = _baseValueSegmentDomain.GetBvsEventsByRevenueObjectId(999).ToList();
			returnBaseValueSegments.ShouldNotBeEmpty();
			returnBaseValueSegments[0].BvsId.ShouldBe(baseValueSegmentEvents[0].BvsId);
			returnBaseValueSegments[0].BvsAsOf.ShouldBe(baseValueSegmentEvents[0].BvsAsOf);
			returnBaseValueSegments[0].RevenueObjectId.ShouldBe(baseValueSegmentEvents[0].RevenueObjectId);
			returnBaseValueSegments[0].SequenceNumber.ShouldBe(baseValueSegmentEvents[0].SequenceNumber);
			returnBaseValueSegments[0].GRMEventId.ShouldBe(baseValueSegmentEvents[0].GRMEventId);
		}

		[Fact]
		public void GetBaseValueSegmentEventByRevenueObjectId_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
		{
			_mockRepository.Setup(x => x.GetBvsEventsByRevenueObjectId(999)).Returns(new List<BaseValueSegmentEvent>());

			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.GetBvsEventsByRevenueObjectId(0));
		}

		#endregion

		[Fact]
		public void GetSubComponentDetailsByRevenueObjectIdWithInvalidRevenueObjectIdGetBadRequestException()
		{
			Should.Throw<BadRequestException>(() => _baseValueSegmentDomain.GetSubComponentDetailsByRevenueObjectId(-1, new DateTime(1911, 1, 1)));
		}

		[Fact]
		public void GetSubComponentDetailsByRevenueObjectIdWithValidRevenueObjectIdButNoRecordsGetBadRequestException()
		{
			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.GetSubComponentDetailsByRevenueObjectId(500, new DateTime(1911, 1, 1)));
		}

		[Fact]
		public void GetSubComponentDetailsByRevenueObjectId_CallsRepository_ValidIdIsPassed_RecordIsReturned()
		{
			var subComponentDetails = new List<SubComponentDetail>
										{
											new SubComponentDetail
											{
												SubComponentId = 100,
												SubComponent = "UnitTestSubComponent",
												ComponentTypeId = 200,
												Component =  "UnitTestComponent"
											}
										};

			_mockRepository.Setup(x => x.GetSubComponentDetailsByRevenueObjectId(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(subComponentDetails);

			var testSubComponentDetails = _baseValueSegmentDomain.GetSubComponentDetailsByRevenueObjectId(999, DateTime.Now).Result.ToList();
			testSubComponentDetails.Count.ShouldBe(1);
			testSubComponentDetails[0].SubComponentId.ShouldBe(100);
			testSubComponentDetails[0].SubComponent.ShouldBe("UnitTestSubComponent");
			testSubComponentDetails[0].ComponentTypeId.ShouldBe(200);
			testSubComponentDetails[0].Component.ShouldBe("UnitTestComponent");
		}

		[Fact]
		public void GetBeneficialInterestsByRevenueObjectIdWithInvalidRevenueObjectIdGetBadRequestException()
		{
			Should.Throw<BadRequestException>(() => _baseValueSegmentDomain.GetBeneficialInterestsByRevenueObjectId(-1, new DateTime(1911, 1, 1)));
		}

		[Fact]
		public void GetBeneficialInterestsByRevenueObjectIdWithValidRevenueObjectIdButNoRecordsGetBadRequestException()
		{
			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.GetBeneficialInterestsByRevenueObjectId(500, new DateTime(1911, 1, 1)));
		}

		[Fact]
		public void GetOwnerDetailsByRevenueObjectId_CallsRepository_ValidIdIsPassed_RecordIsReturned()
		{
			var ownerDetails = new List<BeneficialInterestEvent>
			{
				new BeneficialInterestEvent
				{
					GrmEventId = 300,
					EventType = "UnitTestEventType",
					EventDate = new DateTime(1999, 1, 1),
					EffectiveDate = new DateTime(1999, 1, 2),
					DocNumber = "UnitTestDocNumber",
					DocType = "UnitTestDocType",
					DocDate = new DateTime(1999,3,3), Owners = new [] {
						new OwnerDetail
						{
							LegalPartyId = 123,
							LegalPartyRoleId = 431,
							BeneficialInterestPercentage = 12,
							PercentageInterestGain = 25,
							GrmEventId = 323,
							OwnerId = 435,
							OwnerName = "foo"
						}
					}
				}
			};

			_mockRepository.Setup(x => x.GetBeneficialInterestsByRevenueObjectId(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(ownerDetails);

			var testOwnerDetails = _baseValueSegmentDomain.GetBeneficialInterestsByRevenueObjectId(999, DateTime.Now).ToList();

			testOwnerDetails.Count.ShouldBe(1);

			var onlyTestOwnerDetails = testOwnerDetails[0];

			onlyTestOwnerDetails.GrmEventId.ShouldBe(300);
			onlyTestOwnerDetails.EventType.ShouldBe("UnitTestEventType");
			onlyTestOwnerDetails.EventDate.ShouldBe(new DateTime(1999, 1, 1));
			onlyTestOwnerDetails.EffectiveDate.ShouldBe(new DateTime(1999, 1, 2));
			onlyTestOwnerDetails.DocNumber.ShouldBe("UnitTestDocNumber");
			onlyTestOwnerDetails.DocType.ShouldBe("UnitTestDocType");
			onlyTestOwnerDetails.Owners.Length.ShouldBe(1);
			onlyTestOwnerDetails.Owners[0].OwnerId.ShouldBe(435);
			onlyTestOwnerDetails.Owners[0].BeneficialInterestPercentage.ShouldBe(12);
			onlyTestOwnerDetails.Owners[0].PercentageInterestGain.ShouldBe(25);
			onlyTestOwnerDetails.Owners[0].GrmEventId.ShouldBe(323);
			onlyTestOwnerDetails.Owners[0].LegalPartyRoleId.ShouldBe(431);
			onlyTestOwnerDetails.Owners[0].OwnerName.ShouldBe("foo");
		}

		[Fact]
		public void GetBaseValueSegmentConclusionsWithInvalidRevenueObjectIdGetBadRequestException()
		{
			Should.Throw<BadRequestException>(() => _baseValueSegmentDomain.GetBaseValueSegmentConclusions(-1, new DateTime(1911, 1, 1)));
		}

		[Fact]
		public void GetBaseValueSegmentConclusionsWithValidRevenueObjectIdButNoRecordsGetBaseValueSegmentInfoDtos()
		{
			_mockRepository.Setup(x => x.GetBaseValueSegmentConclusions(500, new DateTime(1911, 1, 1)))
				.Returns(new List<BaseValueSegmentConclusion>
				{
					new BaseValueSegmentConclusion
					{
						ConclusionDate = new DateTime(1910,5,1), GrmEventId = 4325, Description = "foo"
					},
					new BaseValueSegmentConclusion
					{
						ConclusionDate = new DateTime(1910,8,1), GrmEventId = 43115, Description = "bar"
					}
				});

			var list = _baseValueSegmentDomain.GetBaseValueSegmentConclusions(500, new DateTime(1911, 1, 1)).ToList();
			list.Count.ShouldBe(2);
			list[0].ConclusionDate.ShouldBe(new DateTime(1910, 5, 1));
			list[0].Description.ShouldBe("foo");
			list[0].GrmEventId.ShouldBe(4325);
			list[1].ConclusionDate.ShouldBe(new DateTime(1910, 8, 1));
			list[1].Description.ShouldBe("bar");
			list[1].GrmEventId.ShouldBe(43115);
		}

		[Fact]
		public void ListWithRevenueObjectIdGetMappedBaseValueSegmentInfoDtos()
		{
			_mockRepository.Setup(x => x.List(34555))
				.Returns(new List<Repository.Models.V1.BaseValueSegment>
				{
					new Repository.Models.V1.BaseValueSegment
					{
						Id=43,
						AsOf = new DateTime(2011,4,4),
						RevenueObjectId = 34555,
						SequenceNumber = 1
					},
					new Repository.Models.V1.BaseValueSegment
					{
						Id=55,
						AsOf = new DateTime(2011,5,4),
						RevenueObjectId = 34555,
						SequenceNumber = 1
					}
				});
			var list = _baseValueSegmentDomain.List(34555).ToList();

			list.Count.ShouldBe(2);

			list[0].AsOf.ShouldBe(new DateTime(2011, 4, 4));
			list[0].Id.ShouldBe(43);
			list[0].RevenueObjectId.ShouldBe(34555);
			list[0].SequenceNumber.ShouldBe(1);

			list[1].AsOf.ShouldBe(new DateTime(2011, 5, 4));
			list[1].Id.ShouldBe(55);
			list[1].RevenueObjectId.ShouldBe(34555);
			list[1].SequenceNumber.ShouldBe(1);
		}

		[Fact]
		public void GetWithRevenueObjectIdAndAsOfAndSequenceNumberGetMappedBaseValueSegmentInfoDto()
		{
			_mockRepository.Setup(x => x.Get(34555, new DateTime(2012, 1, 1), 1))
				.Returns(new Repository.Models.V1.BaseValueSegment
				{
					Id = 43,
					AsOf = new DateTime(2011, 4, 4),
					RevenueObjectId = 34555,
					SequenceNumber = 1
				});

			var item = _baseValueSegmentDomain.Get(34555, new DateTime(2012, 1, 1), 1);

			item.AsOf.ShouldBe(new DateTime(2011, 4, 4));
			item.Id.ShouldBe(43);
			item.RevenueObjectId.ShouldBe(34555);
			item.SequenceNumber.ShouldBe(1);
		}

		[Fact]
		public void GetWithRevenueObjectIdAndAsOfAndSequenceNumberWithNoResultGetNotFoundException()
		{
			Should.Throw<RecordNotFoundException>(() => _baseValueSegmentDomain.Get(34555, new DateTime(2012, 1, 1), 1));

			_mockRepository.Verify(x => x.Get(34555, new DateTime(2012, 1, 1), 1), Times.Once);
		}

		[Fact]
		public void GetBvsHistoryWithValidRevenueObjectIdAndDate()
		{
			_mockRepository.Setup(x => x.GetBaseValueSegmentHistory(54, new DateTime(2013, 1, 1), new DateTime(2016, 1, 1)))
				.Returns(new List<BaseValueSegmentHistory>
				{
					new BaseValueSegmentHistory
					{
						BvsId = 1,
						AsOf = new DateTime(2014, 1, 1),
						BaseYear = 2014,
						BaseValue = 100000,
						BeneficialInterestPercentage = 50,
						BvsTransactionType = "testtrans",
						LegalPartyRoleId = 100,
						OwnerGrmEventId = 200,
						SubComponentId = 300,
						TransactionId = 1000,
						ValueHeaderGrmEventId = 400
					},
					new BaseValueSegmentHistory
					{
						BvsId = 2,
						AsOf = new DateTime(2015, 1, 1),
						BaseYear = 2015,
						BaseValue = 200000,
						BeneficialInterestPercentage = 50,
						BvsTransactionType = "testtrans",
						LegalPartyRoleId = 100,
						OwnerGrmEventId = 200,
						SubComponentId = 300,
						TransactionId = 1000,
						ValueHeaderGrmEventId = 400
					}
				});

			var bvsHistoryList = _baseValueSegmentDomain.GetBaseValueSegmentHistory(54,
				new DateTime(2013, 1, 1), new DateTime(2016, 1, 1)).ToList();

			var firstBvs = bvsHistoryList[0];

			bvsHistoryList.Count().ShouldBe(2);
			firstBvs.BvsId.ShouldBe(1);
			firstBvs.AsOf.ShouldBe(new DateTime(2014, 1, 1));
			firstBvs.BaseYear.ShouldBe(2014);
			firstBvs.BaseValue.ShouldBe(100000);
			firstBvs.BeneficialInterestPercentage.ShouldBe(50);
			firstBvs.BvsTransactionType.ShouldBe("testtrans");
			firstBvs.LegalPartyRoleId.ShouldBe(100);
			firstBvs.OwnerGrmEventId.ShouldBe(200);
			firstBvs.SubComponentId.ShouldBe(300);
			firstBvs.TransactionId.ShouldBe(1000);
			firstBvs.ValueHeaderGrmEventId.ShouldBe(400);
		}

		[Fact]
		public void GetBvsHistoryWithInvalidRevenueObjectIdGetBadRequestException()
		{
			Should.Throw<BadRequestException>(() => _baseValueSegmentDomain.GetBaseValueSegmentHistory(-100, new DateTime(2013, 1, 1), new DateTime(2016, 1, 1)));
		}
	}
}
