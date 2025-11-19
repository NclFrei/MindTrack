using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.MetaServiceTests;

public class UpdateAsync_Should_ReturnNull_When_NotFound
{
 [Fact]
 public async Task UpdateAsync_NotFound_ReturnsNull()
 {
 // Arrange
 var repoMock = new Mock<IMetaRepository>();
 var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Domain.Models.Meta?)null);

 var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mapperMock.Object, Mock.Of<FluentValidation.IValidator<Domain.DTOs.Request.MetaCreateRequest>>());

 // Act
 var result = await service.UpdateAsync(42, new AtualizarMetaRequest { Titulo = "x" });

 // Assert
 Assert.Null(result);
 repoMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Models.Meta>()), Moq.Times.Never);
 }
}
