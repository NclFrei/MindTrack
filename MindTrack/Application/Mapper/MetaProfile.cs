using AutoMapper;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Mapper;

public class MetaProfile : Profile
{
    public MetaProfile()
    {
        // Requests -> Models
        CreateMap<MetaCreateRequest, Meta>();

        // Models -> Responses
        CreateMap<Meta, MetaResponse>();

        CreateMap<AtualizarMetaRequest, Meta>()
           .ForAllMembers(opt =>
               opt.Condition((src, dest, srcMember) =>
                   srcMember != null &&
                   !(srcMember is string str && string.IsNullOrWhiteSpace(str))
               ));

    }
}


