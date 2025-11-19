using AutoMapper;
using FluentValidation;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MindTrack.Application.Service;

public class TarefaService
{
 private readonly ITarefaRepository _repository;
 private readonly IMapper _mapper;
 private readonly IValidator<TarefaCreateRequest> _validator;
 private readonly ILogger<TarefaService> _logger;

 public TarefaService(ITarefaRepository repository, IMapper mapper, IValidator<TarefaCreateRequest> validator, ILogger<TarefaService>? logger = null)
 {
 _repository = repository;
 _mapper = mapper;
 _validator = validator;
 _logger = logger ?? NullLogger<TarefaService>.Instance;
 }

 public async Task<TarefaResponse> CreateAsync(TarefaCreateRequest request)
 {
 _logger.LogInformation("Criando tarefa para usuário id {UserId} título {Title}", request.UserId, request.Titulo);
 var result = await _validator.ValidateAsync(request);
 if (!result.IsValid)
 {
 var errors = result.Errors
 .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
 .ToList();

 _logger.LogWarning("Validação falhou ao criar tarefa: {Errors}", string.Join(";", errors));

 throw new ValidationException(string.Join(Environment.NewLine, errors));
 }
 var tarefa = _mapper.Map<Tarefa>(request);
 var created = await _repository.CreateAsync(tarefa);
 _logger.LogInformation("Tarefa criada com id {TarefaId}", created?.Id);
 return _mapper.Map<TarefaResponse>(created);
 }

 public async Task<List<TarefaResponse>> GetAllAsync()
 {
 _logger.LogInformation("Retornando todas as tarefas");
 var tarefas = await _repository.GetAllAsync();
 _logger.LogInformation("Recuperadas {Count} tarefas", tarefas?.Count ??0);
 return _mapper.Map<List<TarefaResponse>>(tarefas);
 }

 public async Task<TarefaResponse?> GetByIdAsync(int id)
 {
 _logger.LogInformation("Obtendo tarefa por id {TarefaId}", id);
 var tarefa = await _repository.GetByIdAsync(id);
 if (tarefa == null)
 {
 _logger.LogWarning("Tarefa com id {TarefaId} não encontrada", id);
 return null;
 }
 return _mapper.Map<TarefaResponse>(tarefa);
 }

 public async Task<TarefaResponse?> UpdateAsync(int id, AtualizarTarefaRequest request)
 {
 _logger.LogInformation("Atualizando tarefa id {TarefaId}", id);
 var existing = await _repository.GetByIdAsync(id);
 if (existing == null)
 {
 _logger.LogWarning("Falha na atualização. Tarefa id {TarefaId} não encontrada", id);
 return null;
 }

 _mapper.Map(request, existing);
 var updated = await _repository.UpdateAsync(existing);
 _logger.LogInformation("Tarefa id {TarefaId} atualizada", id);
 return _mapper.Map<TarefaResponse>(updated);
 }

 public async Task<bool> DeleteAsync(int id)
 {
 _logger.LogInformation("Excluindo tarefa id {TarefaId}", id);
 var result = await _repository.DeleteAsync(id);
 _logger.LogInformation("Resultado da exclusão para tarefa id {TarefaId}: {Result}", id, result);
 return result;
 }
}