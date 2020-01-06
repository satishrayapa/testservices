using System;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class AssessmentEventRepositoryTests
  {
    private readonly AssessmentEventRepository _assessmentEventRepository;

    public AssessmentEventRepositoryTests()
    {
      var applicationSettingsHelperMock = new Mock<IApplicationSettingsHelper>();
      _assessmentEventRepository = new AssessmentEventRepository( applicationSettingsHelperMock.Object, null );
    }

    [Fact]
    public void GetShouldThrowBadRequestExceptionWhenAssessmentEventIdIsInvalid()
    {
      Should.ThrowAsync<BadRequestException>( async () => await _assessmentEventRepository.Get( -1 ) );
    }

    [Fact]
    public void ListAsyncShouldThrowBadRequestExceptionWhenAssessmentEventIdIsInvalid()
    {
      Should.ThrowAsync<BadRequestException>( async () => await _assessmentEventRepository.ListAsync( -1 ) );
    }

    [Fact]
    public void ListAsyncShouldThrowBadRequestExceptionWhenAssessmentEventIdIsInvalidForToday()
    {
      Should.ThrowAsync<BadRequestException>( async () => await _assessmentEventRepository.ListAsync( -1, DateTime.Today ) );
    }
  }
}
