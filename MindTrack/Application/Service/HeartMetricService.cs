using AutoMapper;
using Microsoft.Extensions.Logging;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Service;

public class HeartMetricService
{
    private readonly IHeartMetricRepository _metricRepository;
    private readonly IStressRepository _stressRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<HeartMetricService> _logger;
    private readonly StressService _stressService;

    public HeartMetricService(IHeartMetricRepository metricRepository, IStressRepository stressRepository, IMapper mapper, StressService stressService, ILogger<HeartMetricService>? logger = null)
    {
        _metricRepository = metricRepository;
        _stressRepository = stressRepository;
        _mapper = mapper;
        _stressService = stressService;
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<HeartMetricService>.Instance;
    }

    public async Task<StressScoreResponse> IngestAsync(HeartMetricRequest request)
    {
        _logger.LogInformation("Recebendo métrica cardíaca para usuário {UserId}", request.UserId);
        var metric = _mapper.Map<HeartMetric>(request);
        var created = await _metricRepository.CreateAsync(metric);

        var result = _stressService.Calculate(created.HeartRate, created.Rmssd ?? 0);

        var scoreEntity = new StressScore
        {
            UserId = created.UserId,
            Score = result.Score,
            Level = result.Level,
            SourceMetricId = created.Id
        };

        var stored = await _stressRepository.CreateAsync(scoreEntity);
        _logger.LogInformation("Stress score calculado {Score} para usuário {UserId}", stored.Score, stored.UserId);

        return _mapper.Map<StressScoreResponse>(stored);
    }
}
