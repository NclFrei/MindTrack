using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Enums;

namespace MindTrack.Controllers.V1;

/// <summary>
/// Endpoints para gerenciamento de tarefas do usuário (criação, leitura, atualização e exclusão).
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class TarefaController : ControllerBase
{
    private readonly TarefaService _service;

    public TarefaController(TarefaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Cria uma nova tarefa para o usuário autenticado.
    /// </summary>
    /// <param name="request">Dados da tarefa a ser criada</param>
    /// <returns>Tarefa criada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] TarefaCreateRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Recupera todas as tarefas do usuário autenticado.
    /// </summary>
    /// <returns>Lista de tarefas</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TarefaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? metaId = null,
        [FromQuery] int? userId = null,
        [FromQuery] int? prioridade = null,
        [FromQuery] DificuldadeEnum? dificuldade = null)
    {
        var result = await _service.GetAllAsync(page, pageSize, metaId, userId, prioridade, dificuldade);
        result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = 1, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "first", Method = "GET" });
        result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = result.TotalPages, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "last", Method = "GET" });
        if (result.Page > 1) result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = result.Page - 1, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "prev", Method = "GET" });
        if (result.Page < result.TotalPages) result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = result.Page + 1, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "next", Method = "GET" });
        return Ok(result);
    }

    /// <summary>
    /// Recupera uma tarefa pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da tarefa</param>
    /// <returns>Tarefa encontrada ou 404 se não existir</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Atualiza completamente uma tarefa existente (PUT).
    /// </summary>
    /// <param name="id">Identificador da tarefa a ser atualizada</param>
    /// <param name="request">Dados para atualização completa</param>
    /// <returns>Tarefa atualizada ou 404 se não existir</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarTarefaRequest request)
    {
        if (request == null) return BadRequest();
        var result = await _service.UpdateAsync(id, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Atualiza parcialmente uma tarefa existente (PATCH).
    /// </summary>
    /// <param name="id">Identificador da tarefa a ser atualizada</param>
    /// <param name="request">Dados parciais para atualização</param>
    /// <returns>Tarefa atualizada ou 404 se não existir</returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Patch(int id, [FromBody] AtualizarTarefaRequest request)
    {
        if (request == null) return BadRequest();
        var result = await _service.UpdateAsync(id, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Remove uma tarefa existente.
    /// </summary>
    /// <param name="id">Identificador da tarefa a ser excluída</param>
    /// <returns>204 se excluído com sucesso ou 404 se não encontrado</returns>
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