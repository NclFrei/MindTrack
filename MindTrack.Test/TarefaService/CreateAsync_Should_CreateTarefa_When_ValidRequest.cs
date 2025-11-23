using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using AutoMapper;

namespace MindTrack.Test.TarefaServiceTests;

public class CreateAsync_Should_CreateTarefa_When_ValidRequest
{
 [Fact]
 public async Task CreateAsync_ValidRequest_CreatesAndReturnsResponse()
 {
 // Arrange
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();
 var validatorMock = new Mock<IValidator<TarefaCreateRequest>>();

 var request = new TarefaCreateRequest { Titulo = "T1", UserId =1, Prioridade =2 };
 validatorMock
 .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
 .ReturnsAsync(new ValidationResult());

 var tarefa = new Tarefa { Id =5, Titulo = request.Titulo, UserId = request.UserId, Prioridade = request.Prioridade };
 mapperMock.Setup(m => m.Map<Tarefa>(request)).Returns(tarefa);
 repoMock.Setup(r => r.CreateAsync(tarefa)).ReturnsAsync(tarefa);

 var response = new TarefaResponse { Id = tarefa.Id, Titulo = tarefa.Titulo };
 mapperMock.Setup(m => m.Map<TarefaResponse>(tarefa)).Returns(response);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, validatorMock.Object);

 // Act
 var result = await service.CreateAsync(request);

 // Assert
 Assert.NotNull(result);
 Assert.Equal(5, result.Id);
 Assert.Equal("T1", result.Titulo);
 repoMock.Verify(r => r.CreateAsync(tarefa), Times.Once);
 }
}
