using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;

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
 public async Task<IActionResult> GetAll()
 {
 var result = await _service.GetAllAsync();
 return Ok(result);
 }

 /// <summary>
 /// Recupera uma tarefa pelo identificador.
 /// </summary>
 /// <param name="id">Identificador da tarefa</param>
 /// <returns>Tarefa encontrada ou404 se não existir</returns>
 [HttpGet("{id}")]
 public async Task<IActionResult> GetById(int id)
 {
 var result = await _service.GetByIdAsync(id);
 if (result == null) return NotFound();
 return Ok(result);
 }

 /// <summary>
 /// Atualiza uma tarefa existente.
 /// </summary>
 /// <param name="id">Identificador da tarefa a ser atualizada</param>
 /// <param name="request">Dados para atualização</param>
 /// <returns>Tarefa atualizada ou404 se não existir</returns>
 [HttpPut("{id}")]
 public async Task<IActionResult> Update(int id, [FromBody] AtualizarTarefaRequest request)
 {
 var result = await _service.UpdateAsync(id, request);
 if (result == null) return NotFound();
 return Ok(result);
 }

 /// <summary>
 /// Remove uma tarefa existente.
 /// </summary>
 /// <param name="id">Identificador da tarefa a ser excluída</param>
 /// <returns>204 se excluído com sucesso ou404 se não encontrado</returns>
 [HttpDelete("{id}")]
 public async Task<IActionResult> Delete(int id)
 {
 var deleted = await _service.DeleteAsync(id);
 if (!deleted) return NotFound();
 return NoContent();
 }
}