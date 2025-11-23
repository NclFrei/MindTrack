using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;

namespace MindTrack.Controllers.V1;

/// <summary>
/// Endpoints relacionados a métricas de saúde e pontuações de estresse.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class HealthController : ControllerBase
{
    private readonly HeartMetricService _service;
    private readonly IStressRepository _stressRepository;

    public HealthController(HeartMetricService service, IStressRepository stressRepository)
    {
        _service = service;
        _stressRepository = stressRepository;
    }

    /// <summary>
    /// Recebe métricas cardíacas e calcula/retorna a pontuação de estresse.
    /// </summary>
    /// <param name="request">Métrica cardíaca a ser ingerida</param>
    /// <returns>Pontuação de estresse calculada</returns>
    [HttpPost("metrics")]
    [ProducesResponseType(typeof(StressScoreResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StressScoreResponse>> IngestMetric([FromBody] HeartMetricRequest request)
    {
        if (request == null) return BadRequest("Request inválido.");
        var response = await _service.IngestAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Recupera a pontuação de estresse por identificador.
    /// </summary>
    /// <param name="id">Identificador da pontuação de estresse</param>
    /// <returns>Pontuação de estresse e identificador do usuário</returns>
    [HttpGet("scores/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetById(int id)
    {
        var score = await _stressRepository.GetByIdAsync(id);
        if (score == null) return NotFound();
        return Ok(new { userId = score.UserId, score = score.Score });
    }
}
