using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;

namespace MindTrack.Controllers.V1;

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

 [HttpPost]
 public async Task<IActionResult> Create([FromBody] TarefaCreateRequest request)
 {
 var result = await _service.CreateAsync(request);
 return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
 }

 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var result = await _service.GetAllAsync();
 return Ok(result);
 }

 [HttpGet("{id}")]
 public async Task<IActionResult> GetById(int id)
 {
 var result = await _service.GetByIdAsync(id);
 if (result == null) return NotFound();
 return Ok(result);
 }

 [HttpPut("{id}")]
 public async Task<IActionResult> Update(int id, [FromBody] AtualizarTarefaRequest request)
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