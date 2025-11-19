using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.MetaServiceTests;

public class GetByIdAsync_Should_ReturnResponse_When_Found
{
    [Fact]
    public async Task GetByIdAsync_Found_ReturnsResponse()
    {
        // Arrange
        var repoMock = new Mock<IMetaRepository>();
        var mapperMock = new Mock<IMapper>();

        var meta = new Meta { Id = 5, Titulo = "X" };
        repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(meta);

        var response = new MetaResponse { Id = 5, Titulo = "X" };
        mapperMock.Setup(m => m.Map<MetaResponse>(meta)).Returns(response);

        var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.MetaCreateRequest>>());

        // Act
        var result = await service.GetByIdAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result!.Id);
        Assert.Equal("X", result.Titulo);
    }
}
