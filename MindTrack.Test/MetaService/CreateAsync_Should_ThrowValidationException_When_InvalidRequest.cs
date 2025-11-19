using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.Interfaces;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.MetaServiceTests;

public class CreateAsync_Should_ThrowValidationException_When_InvalidRequest
{
 [Fact]
 public async Task CreateAsync_InvalidRequest_ThrowsValidationException()
 {
 // Arrange
 var repoMock = new Mock<IMetaRepository>();
 var mapperMock = new Mock<IMapper>();
        var validatorMock = new Mock<IValidator<MetaCreateRequest>>();

 var request = new MetaCreateRequest { Titulo = "", Descricao = "Desc", UserId =1 };
 var failures = new[] { new ValidationFailure("Titulo", "O titulo da meta é obrigatório.") };
 validatorMock
 .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
 .ReturnsAsync(new ValidationResult(failures));

 var service = new MindTrack.Application.Service.MetaService(repoMock.Object, mapperMock.Object, validatorMock.Object);

 // Act & Assert
 await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(request));
 repoMock.Verify(r => r.CreateAsync(It.IsAny<Domain.Models.Meta>()), Moq.Times.Never);
 }
}
