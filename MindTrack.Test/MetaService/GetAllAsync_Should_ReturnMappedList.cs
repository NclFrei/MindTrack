using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.MetaServiceTests;

public class GetAllAsync_Should_ReturnMappedList
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedResponses()
    {
        // Arrange
        var repoMock = new Mock<IMetaRepository>();
        var mapperMock = new Mock<IMapper>();

        var metas = new List<Meta> { new Meta { Id = 1, Titulo = "A" }, new Meta { Id = 2, Titulo = "B" } };
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(metas);

        var responses = new List<MetaResponse> { new MetaResponse { Id = 1, Titulo = "A" }, new MetaResponse { Id = 2, Titulo = "B" } };
        // When mapping a list, service maps the paged items; set up to return responses when mapping list
        mapperMock.Setup(m => m.Map<List<MetaResponse>>(It.IsAny<List<Meta>>())).Returns(responses);

        var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.MetaCreateRequest>>());

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal("A", result.Items[0].Titulo);
    }
}
