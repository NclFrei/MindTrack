using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.TarefaServiceTests;

public class GetByIdAsync_Should_ReturnNull_When_NotFound
{
 [Fact]
 public async Task GetByIdAsync_NotFound_ReturnsNull()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();
 repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Domain.Models.Tarefa?)null);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.GetByIdAsync(99);

 Assert.Null(result);
 }
}
