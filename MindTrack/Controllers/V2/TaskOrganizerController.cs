using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service.V2;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;

namespace MindTrack.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class TaskOrganizerController : ControllerBase
{
 private readonly TaskOrganizerService _service;

 public TaskOrganizerController(TaskOrganizerService service)
 {
 _service = service;
 }

 /// <summary>
 /// Organiza as tarefas de um usuário de acordo com seu score de estresse usando ML/heurística.
 /// </summary>
 [HttpPost("organize")]
 [ProducesResponseType(typeof(List<TarefaResponse>), StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status400BadRequest)]
 [ProducesResponseType(StatusCodes.Status401Unauthorized)]
 public async Task<IActionResult> Organize([FromBody] OrganizeTasksRequest request)
 {
 if (request == null) return BadRequest();
 var result = await _service.OrganizeAsync(request);
 return Ok(result);
 }
}
