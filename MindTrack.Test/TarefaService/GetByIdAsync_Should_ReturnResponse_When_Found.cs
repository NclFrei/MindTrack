using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.TarefaServiceTests;

public class GetByIdAsync_Should_ReturnResponse_When_Found
{
 [Fact]
 public async Task GetByIdAsync_Found_ReturnsResponse()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();

 var tarefa = new Tarefa { Id =3, Titulo = "X" };
 repoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(tarefa);

 var response = new TarefaResponse { Id =3, Titulo = "X" };
 mapperMock.Setup(m => m.Map<TarefaResponse>(tarefa)).Returns(response);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.GetByIdAsync(3);

 Assert.NotNull(result);
 Assert.Equal(3, result!.Id);
 }
}
