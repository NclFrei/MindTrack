using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.UserServiceTests;

public class GetUserByIdAsync_Should_ReturnResponse_When_Found
{
 [Fact]
 public async Task GetUserByIdAsync_Found_ReturnsResponse()
 {
 var repoMock = new Mock<IUserRepository>();
 var mapperMock = new Mock<IMapper>();

 var user = new User { Id =5, Nome = "User1", Email = "u@example.com" };
 repoMock.Setup(r => r.BuscarPorIdAsync(5)).ReturnsAsync(user);

 var response = new UserResponse { Id =5, Nome = "User1", Email = "u@example.com" };
 mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(response);

 var service = new UserService(repoMock.Object, mapperMock.Object);

 var result = await service.GetUserByIdAsync(5);

 Assert.NotNull(result);
 Assert.Equal(5, result!.Id);
 }
}
