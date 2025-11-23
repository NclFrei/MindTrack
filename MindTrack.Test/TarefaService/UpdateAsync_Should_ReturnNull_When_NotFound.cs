using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.TarefaServiceTests;

public class UpdateAsync_Should_ReturnNull_When_NotFound
{
 [Fact]
 public async Task UpdateAsync_NotFound_ReturnsNull()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();
 repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Domain.Models.Tarefa?)null);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.UpdateAsync(42, new AtualizarTarefaRequest { Titulo = "x" });

 Assert.Null(result);
 repoMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Models.Tarefa>()), Moq.Times.Never);
 }
}
