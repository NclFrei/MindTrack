using System.Threading.Tasks;
using Xunit;
using Moq;
using MindTrack.Application.Service;
using MindTrack.Domain.Interfaces;
using AutoMapper;

namespace MindTrack.Test.MetaServiceTests;

public class DeleteAsync_Should_ReturnTrueOrFalse
{
 [Fact]
 public async Task DeleteAsync_When_RepositoryReturnsTrue_ReturnsTrue()
 {
 // Arrange
 var repoMock = new Mock<IMetaRepository>();
 var mockMapper = new Mock<IMapper>();
 repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

 var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mockMapper.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.MetaCreateRequest>>());

 // Act
 var result = await service.DeleteAsync(1);

 // Assert
 Assert.True(result);
 }

 [Fact]
 public async Task DeleteAsync_When_RepositoryReturnsFalse_ReturnsFalse()
 {
 // Arrange
 var repoMock = new Mock<IMetaRepository>();
 var mapperMock = new Mock<IMapper>();
        repoMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

 var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.MetaCreateRequest>>());

 // Act
 var result = await service.DeleteAsync(99);

 // Assert
 Assert.False(result);
 }
}
