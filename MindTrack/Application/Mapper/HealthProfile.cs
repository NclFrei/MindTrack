using AutoMapper;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Mapper;

public class HealthProfile : Profile
{
    public HealthProfile()
    {
        CreateMap<HeartMetricRequest, HeartMetric>()
        .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp ?? System.DateTime.UtcNow));

        CreateMap<StressScore, StressScoreResponse>();
    }
}
