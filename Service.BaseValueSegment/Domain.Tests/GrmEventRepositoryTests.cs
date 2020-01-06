using System.Linq;
using Moq;
using TAGov.Common;
using TAGov.Common.Http;
using TAGov.Services.Core.BaseValueSegment.Domain.Implementation;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Tests
{
	public class GrmEventRepositoryTests
	{
		private readonly Mock<IUrlService> _mockApplicationSettingsHelper;
		private readonly Mock<IHttpClientWrapper> _mockClientWrapper;
		private readonly GrmEventRepository _grmEventRepository;

		public GrmEventRepositoryTests()
		{
			_mockApplicationSettingsHelper = new Mock<IUrlService>();
			_mockClientWrapper = new Mock<IHttpClientWrapper>();
			_grmEventRepository = new GrmEventRepository(_mockApplicationSettingsHelper.Object, _mockClientWrapper.Object);
		}

		[Fact]
		public void CreateAsync()
		{
			// ReSharper disable once UnusedVariable
			var result = _grmEventRepository.CreateAsync(new GrmEventListCreateDto()).Result;

			_mockClientWrapper.Verify(x => x.Post<GrmEventListCreateDto>(
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<GrmEventListCreateDto>()), Times.Once);

		}

		[Fact]
		public void Delete()
		{
			_grmEventRepository.Delete(new[] { 33, 44 });

			_mockClientWrapper.Verify(x => x.Post<bool>(
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.Is<int[]>(y => y.Length == 2 && y.Contains(33) && y.Contains(44))), Times.Once);

		}

	}
}
