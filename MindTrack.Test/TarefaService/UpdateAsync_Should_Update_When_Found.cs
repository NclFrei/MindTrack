using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.TarefaServiceTests;

public class UpdateAsync_Should_Update_When_Found
{
 [Fact]
 public async Task UpdateAsync_Found_UpdatesAndReturnsResponse()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();

 var existing = new Tarefa { Id =7, Titulo = "Old", Descricao = "OldDesc" };
 repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);

 var updateRequest = new AtualizarTarefaRequest { Titulo = "New", Descricao = "NewDesc" };
 mapperMock.Setup(m => m.Map(updateRequest, existing)).Returns(existing);

 var updated = new Tarefa { Id =7, Titulo = "New", Descricao = "NewDesc" };
 repoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);

 var response = new TarefaResponse { Id =7, Titulo = "New", Descricao = "NewDesc" };
 mapperMock.Setup(m => m.Map<TarefaResponse>(updated)).Returns(response);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.UpdateAsync(7, updateRequest);

 Assert.NotNull(result);
 Assert.Equal("New", result!.Titulo);
 repoMock.Verify(r => r.UpdateAsync(existing), Moq.Times.Once);
 }
}
