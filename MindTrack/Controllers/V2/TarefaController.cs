using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Application.Service.V2;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Enums;

namespace MindTrack.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class TarefaController : ControllerBase
{
 private readonly TarefaService _service;
 private readonly TaskOrganizerService _organizer;

 public TarefaController(TarefaService service, TaskOrganizerService organizer)
 {
 _service = service;
 _organizer = organizer;
 }

 [HttpPost]
 public async Task<IActionResult> Create([FromBody] TarefaCreateRequest request)
 {
 var result = await _service.CreateAsync(request);
 return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
 }

 [HttpGet]
 public async Task<IActionResult> GetAll([FromQuery] int page =1, [FromQuery] int pageSize =10, [FromQuery] int? metaId = null, [FromQuery] int? userId = null, [FromQuery] int? prioridade = null, [FromQuery] DificuldadeEnum? dificuldade = null)
 {
 var result = await _service.GetAllAsync(page, pageSize, metaId, userId, prioridade, dificuldade);
 result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page =1, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "first", Method = "GET" });
 result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = result.TotalPages, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "last", Method = "GET" });
 if (result.Page >1) result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = result.Page -1, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "prev", Method = "GET" });
 if (result.Page < result.TotalPages) result.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = result.Page +1, pageSize, metaId, userId, prioridade, dificuldade }), Rel = "next", Method = "GET" });
 return Ok(result);
 }

 [HttpGet("{id}")]
 public async Task<IActionResult> GetById(int id)
 {
 var result = await _service.GetByIdAsync(id);
 if (result == null) return NotFound();
 return Ok(result);
 }

 [HttpGet("by-user/{userId}")]
 public async Task<IActionResult> GetByUser(int userId, [FromQuery] float? stressScore = null)
 {
 var request = new OrganizeTasksRequest { UserId = userId, StressScore = stressScore ??0 };
 var ordered = await _organizer.OrganizeAsync(request);
 return Ok(ordered);
 }

 [HttpPut("{id}")]
 [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
 public async Task<IActionResult> Update(int id, [FromBody] AtualizarTarefaRequest request)
 {
 var result = await _service.UpdateAsync(id, request);
 if (result == null) return NotFound();
 return Ok(result);
 }

 [HttpPatch("{id}")]
 [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
 public async Task<IActionResult> Patch(int id, [FromBody] AtualizarTarefaRequest request)
 {
 var result = await _service.UpdateAsync(id, request);
 if (result == null) return NotFound();
 return Ok(result);
 }

 [HttpDelete("{id}")]
 public async Task<IActionResult> Delete(int id)
 {
 var deleted = await _service.DeleteAsync(id);
 if (!deleted) return NotFound();
 return NoContent();
 }
}
