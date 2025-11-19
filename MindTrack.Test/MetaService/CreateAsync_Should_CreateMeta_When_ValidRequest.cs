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

namespace MindTrack.Test.MetaServiceTests;

public class CreateAsync_Should_CreateMeta_When_ValidRequest
{
 [Fact]
 public async Task CreateAsync_ValidRequest_CreatesAndReturnsResponse()
 {
 // Arrange
 var repoMock = new Mock<IMetaRepository>();
 var mockMapper = new Mock<IMapper>();
 var validatorMock = new Mock<IValidator<MetaCreateRequest>>();

 var request = new MetaCreateRequest { Titulo = "Titulo", Descricao = "Desc", UserId =1 };
 validatorMock
 .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
 .ReturnsAsync(new ValidationResult());

 var meta = new Meta { Id =10, Titulo = request.Titulo, Descricao = request.Descricao, UserId = request.UserId };
        mockMapper.Setup(m => m.Map<Meta>(request)).Returns(meta);
 repoMock.Setup(r => r.CreateAsync(meta)).ReturnsAsync(meta);

 var response = new MetaResponse { Id = meta.Id, Titulo = meta.Titulo, Descricao = meta.Descricao };
        mockMapper.Setup(m => m.Map<MetaResponse>(meta)).Returns(response);

 var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mockMapper.Object, validatorMock.Object);

 // Act
 var result = await service.CreateAsync(request);

 // Assert
 Assert.NotNull(result);
 Assert.Equal(10, result.Id);
 Assert.Equal("Titulo", result.Titulo);
 repoMock.Verify(r => r.CreateAsync(meta), Moq.Times.Once);
 }
}
