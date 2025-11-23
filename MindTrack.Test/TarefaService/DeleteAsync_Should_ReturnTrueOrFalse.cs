using System.Threading.Tasks;
using Xunit;
using Moq;
using MindTrack.Application.Service;
using MindTrack.Domain.Interfaces;
using AutoMapper;

namespace MindTrack.Test.TarefaServiceTests;

public class DeleteAsync_Should_ReturnTrueOrFalse
{
 [Fact]
 public async Task DeleteAsync_When_RepositoryReturnsTrue_ReturnsTrue()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mockMapper = new Mock<IMapper>();
 repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

 var service = new TarefaService(repoMock.Object, mockMapper.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.DeleteAsync(1);

 Assert.True(result);
 }

 [Fact]
 public async Task DeleteAsync_When_RepositoryReturnsFalse_ReturnsFalse()
 {
 var repoMock = new Mock<ITarefaRepository>();
 var mapperMock = new Mock<IMapper>();
 repoMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

 var service = new TarefaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.TarefaCreateRequest>>());

 var result = await service.DeleteAsync(99);

 Assert.False(result);
 }
}
