using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.MetaServiceTests;

public class GetByIdAsync_Should_ReturnNull_When_NotFound
{
 [Fact]
 public async Task GetByIdAsync_NotFound_ReturnsNull()
 {
 // Arrange
 var repoMock = new Mock<IMetaRepository>();
 var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Domain.Models.Meta?)null);

 var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.MetaCreateRequest>>());

 // Act
 var result = await service.GetByIdAsync(99);

 // Assert
 Assert.Null(result);
 }
}
