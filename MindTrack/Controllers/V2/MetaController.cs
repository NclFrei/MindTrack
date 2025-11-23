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

 [HttpPost]
 [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status201Created)]
 public async Task<ActionResult<MetaResponse>> CreateMetaAsync([FromBody] MetaCreateRequest request)
 {
 var response = await _service.CreateAsync(request);
 return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
 }

 [HttpGet]
 [ProducesResponseType(typeof(PagedResult<MetaResponse>), StatusCodes.Status200OK)]
 public async Task<ActionResult<PagedResult<MetaResponse>>> GetAll([FromQuery] int page =1, [FromQuery] int pageSize =10,
 [FromQuery] bool? concluida = null, [FromQuery] int? userId = null, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
 {
 var response = await _service.GetAllAsync(page, pageSize, concluida, userId, dataInicio, dataFim);
 response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page =1, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "first", Method = "GET" });
 response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.TotalPages, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "last", Method = "GET" });
 if (response.Page >1) response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page -1, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "prev", Method = "GET" });
 if (response.Page < response.TotalPages) response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page +1, pageSize, concluida, userId, dataInicio, dataFim }), Rel = "next", Method = "GET" });
 return Ok(response);
 }

 [HttpGet("{id}")]
 [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
 public async Task<ActionResult<MetaResponse>> GetById(int id)
 {
 var response = await _service.GetByIdAsync(id);
 if (response == null) return NotFound();
 return Ok(response);
 }

 [HttpPut("{id}")]
 [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
 public async Task<ActionResult<MetaResponse>> Update(int id, [FromBody] AtualizarMetaRequest request)
 {
 var updated = await _service.UpdateAsync(id, request);
 if (updated == null) return NotFound();
 return Ok(updated);
 }

 [HttpPatch("{id}")]
 [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
 public async Task<ActionResult<MetaResponse>> Patch(int id, [FromBody] AtualizarMetaRequest request)
 {
 var updated = await _service.UpdateAsync(id, request);
 if (updated == null) return NotFound();
 return Ok(updated);
 }

 [HttpDelete("{id}")]
 public async Task<IActionResult> Delete(int id)
 {
 var deleted = await _service.DeleteAsync(id);
 if (!deleted) return NotFound();
 return NoContent();
 }
}
