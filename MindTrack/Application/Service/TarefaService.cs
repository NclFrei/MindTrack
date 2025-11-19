using AutoMapper;
using FluentValidation;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Service;

public class TarefaService
{
 private readonly ITarefaRepository _repository;
 private readonly IMapper _mapper;
 private readonly IValidator<TarefaCreateRequest> _validator;

 public TarefaService(ITarefaRepository repository, IMapper mapper, IValidator<TarefaCreateRequest> validator)
 {
 _repository = repository;
 _mapper = mapper;
 _validator = validator;
 }

 public async Task<TarefaResponse> CreateAsync(TarefaCreateRequest request)
 {
 var result = await _validator.ValidateAsync(request);
 if (!result.IsValid)
 {
 var errors = result.Errors
 .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
 .ToList();

 throw new ValidationException(string.Join(Environment.NewLine, errors));
 }
 var tarefa = _mapper.Map<Tarefa>(request);
 var created = await _repository.CreateAsync(tarefa);
 return _mapper.Map<TarefaResponse>(created);
 }

 public async Task<List<TarefaResponse>> GetAllAsync()
 {
 var tarefas = await _repository.GetAllAsync();
 return _mapper.Map<List<TarefaResponse>>(tarefas);
 }

 public async Task<TarefaResponse?> GetByIdAsync(int id)
 {
 var tarefa = await _repository.GetByIdAsync(id);
 return tarefa == null ? null : _mapper.Map<TarefaResponse>(tarefa);
 }

 public async Task<TarefaResponse?> UpdateAsync(int id, AtualizarTarefaRequest request)
 {
 var existing = await _repository.GetByIdAsync(id);
 if (existing == null) return null;

 _mapper.Map(request, existing);
 var updated = await _repository.UpdateAsync(existing);
 return _mapper.Map<TarefaResponse>(updated);
 }

 public async Task<bool> DeleteAsync(int id)
 {
 return await _repository.DeleteAsync(id);
 }
}