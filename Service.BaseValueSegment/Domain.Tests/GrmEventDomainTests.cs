using System;
using System.Collections.Generic;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Implementation;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Tests
{
	public class GrmEventDomainTests
	{
		private readonly Mock<IGrmEventRepository> _mockRepository;
		private readonly GrmEventDomain _grmEventDomain;
		public GrmEventDomainTests()
		{
			_mockRepository = new Mock<IGrmEventRepository>();

			_grmEventDomain = new GrmEventDomain(_mockRepository.Object);
		}

		[Fact]
		public void CreateGrmEventsFromBaseValueSegmentDtoMappingsOccurAndGetGrmEventIdArray()
		{
			var baseValueSegment = new BaseValueSegmentDto
			{
				BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
				{
					new BaseValueSegmentTransactionDto
					{
						BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
						{
							new BaseValueSegmentOwnerDto
							{
								Id = 5235,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,1,1),
									RevenueObjectId = 24661,
									EventType = 34664
								}
							},
							new BaseValueSegmentOwnerDto
							{
								Id = 5238,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,2,1),
									RevenueObjectId = 68656,
									EventType = 9563463
								}
							}
						}, BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>
						{
							new BaseValueSegmentValueHeaderDto
							{
								Id = 352523,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,1,2),
									RevenueObjectId = 6346436,
									EventType = 312455
								}
							},
							new BaseValueSegmentValueHeaderDto
							{
								Id = 352528,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,1,3),
									RevenueObjectId = 75544,
									EventType = 65344
								}
							}
						}
					}
				}
			};

			var created = new GrmEventListCreateDto();
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 44,
				ParentType = GrmEventParentType.Owner,
				ParentId = 5235
			});
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 45,
				ParentType = GrmEventParentType.Owner,
				ParentId = 5238
			});
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 46,
				ParentType = GrmEventParentType.HeaderValue,
				ParentId = 352523
			});
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 47,
				ParentType = GrmEventParentType.HeaderValue,
				ParentId = 352528
			});

			_mockRepository.Setup(x => x.CreateAsync(It.Is<GrmEventListCreateDto>(y => y.GrmEventList.Count == 4 &&
				y.GrmEventList[0].ParentId == 5235 && y.GrmEventList[0].ParentType == GrmEventParentType.Owner && y.GrmEventList[0].EffectiveDateTime == new DateTime(2012, 1, 1) && y.GrmEventList[0].EventType == 34664 &&
				y.GrmEventList[1].ParentId == 5238 && y.GrmEventList[1].ParentType == GrmEventParentType.Owner && y.GrmEventList[1].EffectiveDateTime == new DateTime(2012, 2, 1) && y.GrmEventList[1].EventType == 9563463 &&
				y.GrmEventList[2].ParentId == 352523 && y.GrmEventList[2].ParentType == GrmEventParentType.HeaderValue && y.GrmEventList[2].EffectiveDateTime == new DateTime(2012, 1, 2) && y.GrmEventList[2].EventType == 312455 &&
				y.GrmEventList[3].ParentId == 352528 && y.GrmEventList[3].ParentType == GrmEventParentType.HeaderValue && y.GrmEventList[3].EffectiveDateTime == new DateTime(2012, 1, 3) && y.GrmEventList[3].EventType == 65344)))
			.ReturnsAsync(created);

			var results = _grmEventDomain.Create(baseValueSegment).Result;

			results.ShouldNotBeNull();
			results[0].ShouldBe(44);
			results[1].ShouldBe(45);
			results[2].ShouldBe(46);
			results[3].ShouldBe(47);
		}

		[Fact]
		public void CreateGrmEventsFromBaseValueSegmentDtoWithoutGrmEventsForHeaderValueGetBadRequestException()
		{
			var baseValueSegment = new BaseValueSegmentDto
			{
				BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
				{
					new BaseValueSegmentTransactionDto
					{
						BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
						{
							new BaseValueSegmentOwnerDto
							{
								Id = 5235,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,1,1),
									RevenueObjectId = 24661,
									EventType = 34664
								}
							},
							new BaseValueSegmentOwnerDto
							{
								Id = 5238,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,2,1),
									RevenueObjectId = 68656,
									EventType = 9563463
								}
							}
						}, BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>
						{
							new BaseValueSegmentValueHeaderDto
							{
								Id = 352523
							},
							new BaseValueSegmentValueHeaderDto
							{
								Id = 352528
							}
						}
					}
				}
			};

			Should.Throw<BadRequestException>(() => _grmEventDomain.Create(baseValueSegment));
		}

		[Fact]
		public void CreateGrmEventsFromBaseValueSegmentDtoWithoutGrmEventsForOwnerGetBadRequestException()
		{
			var baseValueSegment = new BaseValueSegmentDto
			{
				BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
				{
					new BaseValueSegmentTransactionDto
					{
						BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
						{
							new BaseValueSegmentOwnerDto
							{
								Id = 5235
							},
							new BaseValueSegmentOwnerDto
							{
								Id = 5238
							}
						}, BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>
						{
							new BaseValueSegmentValueHeaderDto
							{
								Id = 352523,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,1,2),
									RevenueObjectId = 6346436,
									EventType = 312455
								}
							},
							new BaseValueSegmentValueHeaderDto
							{
								Id = 352528,
								GrmEventInformation = new GrmEventCreateInformation
								{
									EffectiveDateTime   = new DateTime(2012,1,3),
									RevenueObjectId = 75544,
									EventType = 65344
								}
							}
						}
					}
				}
			};

			Should.Throw<BadRequestException>(() => _grmEventDomain.Create(baseValueSegment));
		}

		[Fact]
		public void CreateForTransactionFromBaseValueSegmentTransactionDtoMappingsOccurAndGetGrmEventIdArray()
		{
			var transaction = new BaseValueSegmentTransactionDto
			{
				BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
				{
					new BaseValueSegmentOwnerDto
					{
						Id = 5235,
						GrmEventInformation = new GrmEventCreateInformation
						{
							EffectiveDateTime = new DateTime(2012, 1, 1),
							RevenueObjectId = 24661,
							EventType = 34664
						}
					},
					new BaseValueSegmentOwnerDto
					{
						Id = 5238,
						GrmEventInformation = new GrmEventCreateInformation
						{
							EffectiveDateTime = new DateTime(2012, 2, 1),
							RevenueObjectId = 68656,
							EventType = 9563463
						}
					}
				},
				BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>
				{
					new BaseValueSegmentValueHeaderDto
					{
						Id = 352523,
						GrmEventInformation = new GrmEventCreateInformation
						{
							EffectiveDateTime = new DateTime(2012, 1, 2),
							RevenueObjectId = 6346436,
							EventType = 312455
						}
					},
					new BaseValueSegmentValueHeaderDto
					{
						Id = 352528,
						GrmEventInformation = new GrmEventCreateInformation
						{
							EffectiveDateTime = new DateTime(2012, 1, 3),
							RevenueObjectId = 75544,
							EventType = 65344
						}
					}
				}
			};

			var created = new GrmEventListCreateDto();
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 44,
				ParentType = GrmEventParentType.Owner,
				ParentId = 5235
			});
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 45,
				ParentType = GrmEventParentType.Owner,
				ParentId = 5238
			});
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 46,
				ParentType = GrmEventParentType.HeaderValue,
				ParentId = 352523
			});
			created.GrmEventList.Add(new GrmEventCreateDto
			{
				GrmEventId = 47,
				ParentType = GrmEventParentType.HeaderValue,
				ParentId = 352528
			});

			_mockRepository.Setup(x => x.CreateAsync(It.Is<GrmEventListCreateDto>(y => y.GrmEventList.Count == 4 &&
																					   y.GrmEventList[0].ParentId == 5235 && y.GrmEventList[0].ParentType == GrmEventParentType.Owner && y.GrmEventList[0].EffectiveDateTime == new DateTime(2012, 1, 1) && y.GrmEventList[0].EventType == 34664 &&
																					   y.GrmEventList[1].ParentId == 5238 && y.GrmEventList[1].ParentType == GrmEventParentType.Owner && y.GrmEventList[1].EffectiveDateTime == new DateTime(2012, 2, 1) && y.GrmEventList[1].EventType == 9563463 &&
																					   y.GrmEventList[2].ParentId == 352523 && y.GrmEventList[2].ParentType == GrmEventParentType.HeaderValue && y.GrmEventList[2].EffectiveDateTime == new DateTime(2012, 1, 2) && y.GrmEventList[2].EventType == 312455 &&
																					   y.GrmEventList[3].ParentId == 352528 && y.GrmEventList[3].ParentType == GrmEventParentType.HeaderValue && y.GrmEventList[3].EffectiveDateTime == new DateTime(2012, 1, 3) && y.GrmEventList[3].EventType == 65344)))
				.ReturnsAsync(created);

			var results = _grmEventDomain.CreateForTransaction(transaction).Result;

			results.ShouldNotBeNull();
			results[0].ShouldBe(44);
			results[1].ShouldBe(45);
			results[2].ShouldBe(46);
			results[3].ShouldBe(47);
		}

		[Fact]
		public void DeleteIdListIdListArrayGotPassedIn()
		{
			_grmEventDomain.Delete(new[] { 42, 44 });

			_mockRepository.Verify(x => x.Delete(It.Is<int[]>(y => y[0] == 42 && y[1] == 44)), Times.Once);
		}
	}
}
