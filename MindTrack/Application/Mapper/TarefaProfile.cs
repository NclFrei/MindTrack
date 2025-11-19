using AutoMapper;
using MindTrack.Application.Mapper;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Mapper
{
 public class TarefaProfile : Profile
 {
 public TarefaProfile()
 {
 CreateMap<TarefaCreateRequest, Tarefa>();
 CreateMap<Tarefa, TarefaResponse>();
 CreateMap<AtualizarTarefaRequest, Tarefa>();
 }
 }
}