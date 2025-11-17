using AutoMapper;
using FluentValidation;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Service;

public class MetaService
{
    private readonly IMetaRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<MetaCreateRequest> _validator;


    public MetaService(IMetaRepository repository, IMapper mapper, IValidator<MetaCreateRequest> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;

    }

    public async Task<MetaResponse> CreateAsync(MetaCreateRequest request)
    {
        var result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }
        var meta = _mapper.Map<Meta>(request);
        var created = await _repository.CreateAsync(meta);
        return _mapper.Map<MetaResponse>(created);
    }

    public async Task<List<MetaResponse>> GetAllAsync()
    {
        var metas = await _repository.GetAllAsync();
        return _mapper.Map<List<MetaResponse>>(metas);
    }

    public async Task<MetaResponse?> GetByIdAsync(int id)
    {
        var area = await _repository.GetByIdAsync(id);
        return area == null ? null : _mapper.Map<MetaResponse>(area);
    }

    public async Task<MetaResponse?> UpdateAsync(int id, AtualizarMetaRequest request)
    {
        var existingArea = await _repository.GetByIdAsync(id);
        if (existingArea == null) return null;

        _mapper.Map(request, existingArea);

        var updated = await _repository.UpdateAsync(existingArea);
        return _mapper.Map<MetaResponse>(updated);
    }

  
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

}
