using System;
using System.Collections.Generic;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Tests
{
	public static class BaseValueSegmentHelper
	{
		public static BaseValueSegmentTransactionDto CreateMockTransactionDto()
		{
			return new BaseValueSegmentTransactionDto
			{
				TransactionId = 35411,
				BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
				{
					new BaseValueSegmentOwnerDto
					{
						Id = 353,
						BaseValueSegmentTransactionId = 35411,
						LegalPartyRoleId = 46,
						BeneficialInterestPercent = 100,
						BaseValueSegmentOwnerValueValues = new List<BaseValueSegmentOwnerValueDto>
						{
							new BaseValueSegmentOwnerValueDto
							{
								Id = 113,
								BaseValue = 5444,
								BaseValueSegmentOwnerId = 353,
								BaseValueSegmentValueHeaderId = 767,
								DynCalcStepTrackingId = 0
							},
							new BaseValueSegmentOwnerValueDto
							{
								Id = 114,
								BaseValue = 78444,
								BaseValueSegmentOwnerId = 353,
								BaseValueSegmentValueHeaderId = 767,
								DynCalcStepTrackingId = 0
							}
						}
					}
				},
				BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>
				{
					new BaseValueSegmentValueHeaderDto
					{
						Id = 767,
						BaseYear = 2012,
						BaseValueSegmentOwnerValues = new List<BaseValueSegmentOwnerValueDto>
						{
							new BaseValueSegmentOwnerValueDto
							{
								Id = 113,
								BaseValue = 5444,
								BaseValueSegmentOwnerId = 353,
								BaseValueSegmentValueHeaderId = 767
							},
							new BaseValueSegmentOwnerValueDto
							{
								Id = 114,
								BaseValue = 78444,
								BaseValueSegmentOwnerId = 353,
								BaseValueSegmentValueHeaderId = 767
							}
						},
						BaseValueSegmentValues = new List<BaseValueSegmentValueDto>
						{
							new BaseValueSegmentValueDto
							{
								Id=43,
								BaseValueSegmentValueHeaderId = 767,
								FullValueAmount = 100000,
								PercentComplete = 100,
								SubComponent = 435556,
								ValueAmount = 3598
							}
						}
					}
				}
			};
		}

		public static BaseValueSegmentDto CreateMockDto()
		{
			var baseValueSegment = new BaseValueSegmentDto
			{
				AsOf = new DateTime(2013, 1, 1),
				AssessmentEventTransactionId = 435,
				RevenueObjectId = 54,
				DynCalcInstanceId = 5346,
				SequenceNumber = 1,
				TransactionId = 6457,
				BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>(),
				BaseValueSegmentAssessmentRevisions = new List<AssessmentRevisionBaseValueSegmentDto>()
			};

			baseValueSegment.BaseValueSegmentTransactions.Add(CreateMockTransactionDto());

			baseValueSegment.BaseValueSegmentAssessmentRevisions.Add(new AssessmentRevisionBaseValueSegmentDto
			{
				ReviewMessage = "foo bar",
        BaseValueSegmentStatusType = new BaseValueSegmentStatusTypeDto { Description = "foobar", Name = "foobar" }
			});

			return baseValueSegment;
		}
	}
}
