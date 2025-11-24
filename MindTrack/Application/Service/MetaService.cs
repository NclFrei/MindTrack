using AutoMapper;
using FluentValidation;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MindTrack.Application.Service;

public class MetaService
{
    private readonly IMetaRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<MetaCreateRequest> _validator;
    private readonly ILogger<MetaService> _logger;


    public MetaService(IMetaRepository repository, IMapper mapper, IValidator<MetaCreateRequest> validator, ILogger<MetaService>? logger = null)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger ?? NullLogger<MetaService>.Instance;

    }

    public async Task<MetaResponse> CreateAsync(MetaCreateRequest request)
    {
        _logger.LogInformation("Criando meta para usuário id {UserId} título {Title}", request.UserId, request.Titulo);
        var result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
                .ToList();

            _logger.LogWarning("Validação falhou ao criar meta: {Errors}", string.Join(";", errors));

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }
        var meta = _mapper.Map<Meta>(request);
        var created = await _repository.CreateAsync(meta);
        _logger.LogInformation("Meta criada com id {MetaId}", created?.Id);
        return _mapper.Map<MetaResponse>(created);
    }

    public async Task<PagedResult<MetaResponse>> GetAllAsync(int page = 1, int pageSize = 10, bool? concluida = null, int? userId = null, DateTime? dataInicio = null, DateTime? dataFim = null)
    {
        _logger.LogInformation("Retornando metas paginação: página {Page} tamanho {PageSize}", page, pageSize);
        var metas = await _repository.GetAllAsync();

        // Aplicar filtros em memória (recomendado mover para repositório para grandes volumes)
        if (concluida.HasValue)
            metas = metas.Where(m => m.Concluida == concluida.Value).ToList();
        if (userId.HasValue)
            metas = metas.Where(m => m.UserId == userId.Value).ToList();
        if (dataInicio.HasValue)
            metas = metas.Where(m => m.DataInicio.HasValue && m.DataInicio.Value.Date >= dataInicio.Value.Date).ToList();
        if (dataFim.HasValue)
            metas = metas.Where(m => m.DataFim.HasValue && m.DataFim.Value.Date <= dataFim.Value.Date).ToList();

        var total = metas.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var items = metas.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new PagedResult<MetaResponse>
        {
            Items = _mapper.Map<List<MetaResponse>>(items),
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = totalPages
        };

        return result;
    }

    public async Task<MetaResponse?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Obtendo meta por id {MetaId}", id);
        var area = await _repository.GetByIdAsync(id);
        if (area == null)
        {
            _logger.LogWarning("Meta com id {MetaId} não encontrada", id);
            return null;
        }
        return _mapper.Map<MetaResponse>(area);
    }

    public async Task<MetaResponse?> UpdateAsync(int id, AtualizarMetaRequest request)
    {
        _logger.LogInformation("Atualizando meta id {MetaId}", id);
        var existingArea = await _repository.GetByIdAsync(id);
        if (existingArea == null)
        {
            _logger.LogWarning("Falha na atualização. Meta id {MetaId} não encontrada", id);
            return null;
        }

        _mapper.Map(request, existingArea);

        var updated = await _repository.UpdateAsync(existingArea);
        _logger.LogInformation("Meta id {MetaId} atualizada", id);
        return _mapper.Map<MetaResponse>(updated);
    }


    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Excluindo meta id {MetaId}", id);
        var result = await _repository.DeleteAsync(id);
        _logger.LogInformation("Resultado da exclusão para meta id {MetaId}: {Result}", id, result);
        return result;
    }

}
