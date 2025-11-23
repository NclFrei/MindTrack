using AutoMapper;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Service.V2;

public class TaskOrganizerService
{
 private readonly ITarefaRepository _tarefaRepository;
 private readonly IMapper _mapper;

 public TaskOrganizerService(ITarefaRepository tarefaRepository, IMapper mapper)
 {
 _tarefaRepository = tarefaRepository;
 _mapper = mapper;
 }

 public async Task<List<TarefaResponse>> OrganizeAsync(OrganizeTasksRequest request)
 {
 // load user's tasks
 var tasks = await _tarefaRepository.GetAllAsync();
 var userTasks = tasks.Where(t => t.UserId == request.UserId && (!request.MetaId.HasValue || t.MetaId == request.MetaId)).ToList();

 if (!userTasks.Any()) return new List<TarefaResponse>();

 // If no ML model available, use a simple heuristic: score = prioridade - difficultyWeight * dificuldade + stressWeight * stressScore
 var scored = userTasks.Select(t => new { Task = t, Score = HeuristicScore(t, request.StressScore) })
 .OrderByDescending(x => x.Score)
 .Select(x => _mapper.Map<TarefaResponse>(x.Task))
 .ToList();

 return scored;
 }

 private float HeuristicScore(MindTrack.Domain.Models.Tarefa t, float stressScore)
 {
 // convert difficulty enum to numeric
 var difficulty = (float)t.Dificuldade;
 float difficultyWeight =0.5f;
 float stressWeight =0.1f;
 return t.Prioridade *1.0f - difficultyWeight * difficulty + stressWeight * stressScore;
 }
}
