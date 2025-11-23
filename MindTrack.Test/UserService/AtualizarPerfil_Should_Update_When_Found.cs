using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.UserServiceTests;

public class AtualizarPerfil_Should_Update_When_Found
{
 [Fact]
 public async Task AtualizarPerfil_Found_UpdatesAndReturnsResponse()
 {
 var repoMock = new Mock<IUserRepository>();
 var mapperMock = new Mock<IMapper>();

 var existing = new User { Id =7, Nome = "Old", Email = "old@example.com" };
 repoMock.Setup(r => r.BuscarPorIdAsync(7)).ReturnsAsync(existing);

 var updateRequest = new AtualizarUserRequest { Nome = "New", Email = "new@example.com" };
 mapperMock.Setup(m => m.Map(updateRequest, existing)).Returns(existing);

 repoMock.Setup(r => r.AtualizarAsync(existing)).ReturnsAsync(existing);

 var response = new UserResponse { Id =7, Nome = "New", Email = "new@example.com" };
 mapperMock.Setup(m => m.Map<UserResponse>(existing)).Returns(response);

 var service = new UserService(repoMock.Object, mapperMock.Object);

 var result = await service.AtualizarPerfilAsync(7, updateRequest);

 Assert.NotNull(result);
 Assert.Equal("New", result.Nome);
 }
}
