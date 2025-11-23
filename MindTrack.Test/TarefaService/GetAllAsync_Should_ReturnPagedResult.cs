using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MindTrack.Domain.Enums;

namespace MindTrack.Test.TarefaServiceTests;

public class GetAllAsync_Should_ReturnPagedResult
{
 [Fact]
 public async Task GetAllAsync_ReturnsPagedResult()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();
 var tarefas = new List<Tarefa> {
 new Tarefa { Id =1, Titulo = "A", Prioridade =1, Dificuldade = DificuldadeEnum.facil },
 new Tarefa { Id =2, Titulo = "B", Prioridade =2, Dificuldade = DificuldadeEnum.media }
 };
 repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tarefas);

 var responses = new List<TarefaResponse> { new TarefaResponse { Id =1, Titulo = "A" }, new TarefaResponse { Id =2, Titulo = "B" } };
 mapperMock.Setup(m => m.Map<List<TarefaResponse>>(It.IsAny<List<Tarefa>>())).Returns(responses);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.GetAllAsync();

 Assert.NotNull(result);
 Assert.Equal(2, result.TotalCount);
 Assert.Equal(1, result.Page);
 }
}
