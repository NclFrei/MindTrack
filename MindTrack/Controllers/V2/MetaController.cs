using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;

namespace MindTrack.Controllers.V2;

/// <summary>
/// V2 - Endpoints para gerenciamento de metas do usuário (mesma funcionalidade da v1).
/// </summary>
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class MetaController : ControllerBase
{
    private readonly MetaService _service;

    public MetaController(MetaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Cria uma nova meta para o usuário autenticado.
    /// </summary>
    /// <param name="request">Dados da meta a ser criada</param>
    /// <returns>Meta criada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MetaResponse>> CreateMetaAsync([FromBody] MetaCreateRequest request)
    {
        var response = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Recupera todas as metas do usuário autenticado com paginação e filtros opcionais.
    /// </summary>
    /// <param name="page">Número da página (padrão1)</param>
    /// <param name="pageSize">Tamanho da página (padrão10)</param>
    /// <param name="concluida">Filtrar por metas concluídas ou não</param>
    /// <param name="userId">Filtrar por usuário</param>
    /// <param name="dataInicio">Filtrar por data de início mínima</param>
    /// <param name="dataFim">Filtrar por data de fim máxima</param>
    /// <returns>Página de metas com links HATEOAS</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MetaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<MetaResponse>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
    [FromQuery] bool? concluida = null, [FromQuery] int? userId = null, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
    {
        var response = await _service.GetAllAsync(page, pageSize, concluida, userId, dataInicio, dataFim);
        response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = 1, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "first", Method = "GET" });
        response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.TotalPages, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "last", Method = "GET" });
        if (response.Page > 1) response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page - 1, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "prev", Method = "GET" });
        if (response.Page < response.TotalPages) response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page + 1, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "next", Method = "GET" });
        return Ok(response);
    }

    /// <summary>
    /// Recupera uma meta específica pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da meta</param>
    /// <returns>Meta encontrada ou404 se não existir</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MetaResponse>> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response == null) return NotFound();
        return Ok(response);
    }

    /// <summary>
    /// Atualiza completamente uma meta existente (PUT).
    /// </summary>
    /// <param name="id">Identificador da meta a ser atualizada</param>
    /// <param name="request">Dados para atualização completa</param>
    /// <returns>Meta atualizada ou404 se não existir</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MetaResponse>> Update(int id, [FromBody] AtualizarMetaRequest request)
    {
        var updated = await _service.UpdateAsync(id, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    /// <summary>
    /// Atualiza parcialmente uma meta existente (PATCH).
    /// </summary>
    /// <param name="id">Identificador da meta a ser atualizada</param>
    /// <param name="request">Dados parciais para atualização</param>
    /// <returns>Meta atualizada ou404 se não existir</returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MetaResponse>> Patch(int id, [FromBody] AtualizarMetaRequest request)
    {
        var updated = await _service.UpdateAsync(id, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    /// <summary>
    /// Remove uma meta existente.
    /// </summary>
    /// <param name="id">Identificador da meta a ser excluída</param>
    /// <returns>204 se excluído com sucesso ou404 se não encontrado</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
