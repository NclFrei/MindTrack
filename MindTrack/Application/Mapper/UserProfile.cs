using AutoMapper;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Criação de usuário
        CreateMap<UserCreateRequest, User>();

        // Retorno de usuário
        CreateMap<User, UserResponse>();

        CreateMap<AtualizarUserRequest, User>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !(srcMember is string str && string.IsNullOrWhiteSpace(str))
                ));
    }


}