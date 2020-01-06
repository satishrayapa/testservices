using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;

namespace Domain.Tests
{
  public static class BaseValueSegmentDomainBaseExtensions
  {
    public static BaseValueSegmentDto MockData( this Mock<IBaseValueSegmentProvider> baseValueSegmentProvider, MockData mockData )
    {
      var ownerValueDtos = new List<BaseValueSegmentOwnerValueDto>();

      var owner = new BaseValueSegmentOwnerDto
                  {
                    Id = mockData.OwnerId,
                    LegalPartyRoleId = 333,
                    BeneficialInterestPercent = mockData.BeneficialInterestPercentage,
                    BaseValueSegmentOwnerValueValues = ownerValueDtos
                  };

      ownerValueDtos.Add( new BaseValueSegmentOwnerValueDto
                          {
                            BaseValue = 100,
                            BaseValueSegmentOwnerId = mockData.OwnerId,
                            BaseValueSegmentValueHeaderId = 1
                          } );

      var baseValueSegmentDto = new BaseValueSegmentDto
                                {
                                  Id = mockData.BaseValueSegmentId,
                                  AsOf = mockData.EventDate,
                                  SequenceNumber = 1,
                                  BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                                 {
                                                                   new BaseValueSegmentTransactionDto
                                                                   {
                                                                     Id = 31,
                                                                     BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
                                                                                              {
                                                                                                owner
                                                                                              },
                                                                     BaseValueSegmentTransactionType = new BaseValueSegmentTransactionTypeDto
                                                                                                       {
                                                                                                         Description = "BaseValueSegmentTransactionTypeDescription",
                                                                                                         Name = "BaseValueSegmentTransactionTypeName"
                                                                                                       }
                                                                   }
                                                                 }
                                };

      var legalPartyDocuments = new List<LegalPartyDocumentDto>
                                {
                                  new LegalPartyDocumentDto
                                  {
                                    LegalPartyRoleId = 333,
                                    LegalPartyDisplayName = mockData.DisplayName,
                                    DocNumber = mockData.DocumentNumber,
                                    DocType = mockData.DocumentType,
                                    PctGain = mockData.PercentageInterestGain
                                  }
                                };

      mockData.LegalPartyDomain.Setup( x => x.GetLegalPartyRoleDocuments( baseValueSegmentDto ) ).Returns( Task.FromResult<IEnumerable<LegalPartyDocumentDto>>( legalPartyDocuments ) );

      var events = new List<GrmEventInformationDto>
                   {
                     new GrmEventInformationDto
                     {
                       Description = mockData.GrmEventDescription,
                       EventType = mockData.GrmEventType,
                       EventDate = mockData.GrmEventDate
                     }
                   };

      mockData.GrmEventDomain
              .Setup( x => x.GetOwnerGrmEvents( It.Is<BaseValueSegmentDto>( y => y.Id == mockData.BaseValueSegmentId ) ) )
              .Returns( Task.FromResult<IEnumerable<GrmEventInformationDto>>( events ) );

      var tuple = new Tuple<BaseValueSegmentDto, BaseValueSegmentDto>( baseValueSegmentDto, null );

      baseValueSegmentProvider.Setup( x => x.GetCurrentAndPrevious( It.IsAny<int>() ) )
                              .Returns( Task.FromResult( tuple ) );

      return baseValueSegmentDto;
    }

    public static void AssertWith( this BvsOwnerDto bvsOwnerDto, MockData mock )
    {
      bvsOwnerDto.Owners.Length.ShouldBe( 1 );

      var owner = bvsOwnerDto.Owners[ 0 ];
      owner.OwnerId.ShouldBe( mock.OwnerId );
      owner.EventDate.ShouldBeNull();
      owner.EventName.ShouldBe( "Unknown" );
      owner.EventType.ShouldBe( "Unknown" );
      owner.BeneficialInterest.ShouldBe( mock.DisplayName );
      owner.DocNumber.ShouldBe( mock.DocumentNumber );
      owner.DocType.ShouldBe( mock.DocumentType );
      owner.BiPercentage.ShouldBe( mock.BeneficialInterestPercentage );
      owner.PercentageInterestGain.ShouldBe( mock.PercentageInterestGain );
      bvsOwnerDto.EventName.ShouldBe( mock.GrmEventDescription );
      bvsOwnerDto.EventType.ShouldBe( mock.GrmEventType );
      bvsOwnerDto.EventDate.ShouldBe( mock.GrmEventDate );
      bvsOwnerDto.Source.Id.ShouldBe( mock.BaseValueSegmentId );
    }
  }
}
