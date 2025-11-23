using AutoMapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MindTrack.Test.UserServiceTests;

public class GetAllUsersAsync_Should_ReturnPagedResult
{
 [Fact]
 public async Task GetAllUsersAsync_ReturnsPagedResult()
 {
 var repoMock = new Mock<IUserRepository>();
 var mapperMock = new Mock<IMapper>();
 var users = new List<User> {
 new User { Id =1, Nome = "A" },
 new User { Id =2, Nome = "B" }
 };
 repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

 var responses = new List<UserResponse> { new UserResponse { Id =1, Nome = "A" }, new UserResponse { Id =2, Nome = "B" } };
 mapperMock.Setup(m => m.Map<List<UserResponse>>(It.IsAny<List<User>>())).Returns(responses);

 var service = new UserService(repoMock.Object, mapperMock.Object);
 var result = await service.GetAllUsersAsync();

 Assert.NotNull(result);
 Assert.Equal(2, result.TotalCount);
 Assert.Equal(1, result.Page);
 }
}
