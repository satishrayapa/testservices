using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class BaseValueSegmentDomainTests
  {
    private readonly Mock<IBaseValueSegmentRepository> _baseValueSegmentRepository;
    private readonly Mock<IAssessmentEventRepository> _assessmentEventRepository;
    private readonly BaseValueSegmentDomain _baseValueSegmentDomain;

    public BaseValueSegmentDomainTests()
    {
      _baseValueSegmentRepository = new Mock<IBaseValueSegmentRepository>();
      _assessmentEventRepository = new Mock<IAssessmentEventRepository>();
      _baseValueSegmentDomain = new BaseValueSegmentDomain(
        _baseValueSegmentRepository.Object, _assessmentEventRepository.Object );
    }

    [Fact]
    public void ThrowBadRequestExceptionWhenBaseValueSegmentDtoIsNull()
    {
      Should.ThrowAsync<BadRequestException>( () => _baseValueSegmentDomain.SaveAsync( 123, null ) );
    }

    [Fact]
    public void ThrowBadRequestExceptionWhenAssessmenetEventIdIsInvalid()
    {
      Should.ThrowAsync<BadRequestException>( () => _baseValueSegmentDomain.SaveAsync( -1, new BaseValueSegmentDto() ) );
    }

    [Fact]
    public void CreateNewBaseValueSegmentIfNoCorrespondingIndexOfAssessmentEventIsFoundInBaseValueSegments_WhereLastAssessmentIsMissingBaseValueSegment()
    {
      var date = DateTime.Today;
      const int revenueObjectId = 100;
      const int assessmentEventWithMissingBaseValueSegmentId = 53;
      const int sequenceNumberThatIsMissing = 3;

      _assessmentEventRepository.Setup( x => x.ListAsync( revenueObjectId, date ) ).Returns( MockList(
                                                                                               new AssessmentEventDto { Id = 51 },
                                                                                               new AssessmentEventDto { Id = 52 },
                                                                                               new AssessmentEventDto { Id = assessmentEventWithMissingBaseValueSegmentId } ) );

      _baseValueSegmentRepository.Setup( x => x.GetAsync( revenueObjectId, date, sequenceNumberThatIsMissing ) )
                                 .Returns( Task.FromResult<BaseValueSegmentInfoDto>( null ) );

      // ReSharper disable once UnusedVariable
      var result = _baseValueSegmentDomain.SaveAsync( assessmentEventWithMissingBaseValueSegmentId, new BaseValueSegmentDto
                                                                                                    {
                                                                                                      AsOf = date,
                                                                                                      SequenceNumber = 2,
                                                                                                      RevenueObjectId = revenueObjectId
                                                                                                    } ).Result;

      _baseValueSegmentRepository.Verify( x => x.CreateAsync( It.Is<BaseValueSegmentDto>( y =>
                                                                                            y.SequenceNumber == sequenceNumberThatIsMissing &&
                                                                                            y.AsOf == date &&
                                                                                            y.RevenueObjectId == revenueObjectId ) ), Times.Once );
    }

    [Fact]
    public void CreateNewBaseValueSegmentIfNoCorrespondingIndexOfAssessmentEventIsFoundInBaseValueSegments_WhereMiddleAssessmentIsMissingBaseValueSegment()
    {
      var date = DateTime.Today;
      const int revenueObjectId = 100;
      const int assessmentEventWithMissingBaseValueSegmentId = 52;
      const int sequenceNumberThatIsMissing = 2;

      _assessmentEventRepository.Setup( x => x.ListAsync( revenueObjectId, date ) ).Returns( MockList(
                                                                                               new AssessmentEventDto { Id = 51 },
                                                                                               new AssessmentEventDto { Id = assessmentEventWithMissingBaseValueSegmentId },
                                                                                               new AssessmentEventDto { Id = 53 } ) );

      _baseValueSegmentRepository.Setup( x => x.GetAsync( revenueObjectId, date, sequenceNumberThatIsMissing ) )
                                 .Returns( Task.FromResult<BaseValueSegmentInfoDto>( null ) );

      // ReSharper disable once UnusedVariable
      var result = _baseValueSegmentDomain.SaveAsync( assessmentEventWithMissingBaseValueSegmentId, new BaseValueSegmentDto
                                                                                                    {
                                                                                                      AsOf = date,
                                                                                                      SequenceNumber = 1,
                                                                                                      RevenueObjectId = revenueObjectId
                                                                                                    } ).Result;

      _baseValueSegmentRepository.Verify( x => x.CreateAsync( It.Is<BaseValueSegmentDto>( y =>
                                                                                            y.SequenceNumber == sequenceNumberThatIsMissing &&
                                                                                            y.AsOf == date &&
                                                                                            y.RevenueObjectId == revenueObjectId ) ), Times.Once );
    }

    [Fact]
    public void CreateNewBaseValueSegmentIfNoCorrespondingIndexOfAssessmentEventIsFoundInBaseValueSegments_WhereFirstAssessmentIsMissingBaseValueSegment()
    {
      var date = DateTime.Today;
      const int revenueObjectId = 100;
      const int assessmentEventWithMissingBaseValueSegmentId = 51;
      const int sequenceNumberThatIsMissing = 1;

      _assessmentEventRepository.Setup( x => x.ListAsync( revenueObjectId, date ) ).Returns( MockList(
                                                                                               new AssessmentEventDto { Id = assessmentEventWithMissingBaseValueSegmentId },
                                                                                               new AssessmentEventDto { Id = 52 },
                                                                                               new AssessmentEventDto { Id = 53 } ) );

      _baseValueSegmentRepository.Setup( x => x.GetAsync( revenueObjectId, date, sequenceNumberThatIsMissing ) )
                                 .Returns( Task.FromResult<BaseValueSegmentInfoDto>( null ) );

      // ReSharper disable once UnusedVariable
      var result = _baseValueSegmentDomain.SaveAsync( assessmentEventWithMissingBaseValueSegmentId, new BaseValueSegmentDto
                                                                                                    {
                                                                                                      AsOf = date,
                                                                                                      SequenceNumber = 2,
                                                                                                      RevenueObjectId = revenueObjectId
                                                                                                    } ).Result;

      _baseValueSegmentRepository.Verify( x => x.CreateAsync( It.Is<BaseValueSegmentDto>( y =>
                                                                                            y.SequenceNumber == sequenceNumberThatIsMissing &&
                                                                                            y.AsOf == date &&
                                                                                            y.RevenueObjectId == revenueObjectId ) ), Times.Once );
    }

    private Task<IEnumerable<AssessmentEventDto>> MockList( params AssessmentEventDto[] items )
    {
      return Task.FromResult<IEnumerable<AssessmentEventDto>>( items.ToList() );
    }

    [Fact]
    public void UpdateExistingBaseValueSegmentIfCorrespondingIndexOfAssessmentEventIsFoundInBaseValueSegments()
    {
      var date = DateTime.Today;
      const int revenueObjectId = 100;
      const int assessmentEventWithMissingBaseValueSegmentId = 51;
      const int sequenceNumberThatIsPresent = 1;

      _assessmentEventRepository.Setup( x => x.ListAsync( revenueObjectId, date ) ).Returns( MockList(
                                                                                               new AssessmentEventDto { Id = 51 },
                                                                                               new AssessmentEventDto { Id = 52 },
                                                                                               new AssessmentEventDto { Id = 53 } ) );

      _baseValueSegmentRepository.Setup( x => x.GetAsync( revenueObjectId, date, sequenceNumberThatIsPresent ) )
                                 .Returns( Task.FromResult( new BaseValueSegmentInfoDto() ) );

      var result = _baseValueSegmentDomain.SaveAsync( assessmentEventWithMissingBaseValueSegmentId, new BaseValueSegmentDto
                                                                                                    {
                                                                                                      Id = 1234,
                                                                                                      AsOf = date,
                                                                                                      SequenceNumber = sequenceNumberThatIsPresent,
                                                                                                      RevenueObjectId = revenueObjectId,
                                                                                                      BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                                                                                                     {
                                                                                                                                       new BaseValueSegmentTransactionDto { Id = -1, TransactionId = 552 }
                                                                                                                                     }
                                                                                                    } ).Result;

      result.SequenceNumber.ShouldBe( sequenceNumberThatIsPresent );
      result.Id.ShouldBe( 1234 );

      _baseValueSegmentRepository.Verify( x => x.CreateTransactionAsync( It.Is<BaseValueSegmentTransactionDto>( y => y.TransactionId == 552 ) ), Times.Once );
    }
  }
}