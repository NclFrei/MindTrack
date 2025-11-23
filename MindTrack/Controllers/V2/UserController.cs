using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Erros;

namespace MindTrack.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
 private readonly UserService _usuarioService;

 public UserController(UserService usuarioService)
 {
 _usuarioService = usuarioService;
 }

 [HttpGet("{id}")]
 [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
 public async Task<ActionResult<UserResponse>> ObterUsuario(int id)
 {
 var usuario = await _usuarioService.GetUserByIdAsync(id);
 if (usuario == null) return NotFound();
 return Ok(usuario);
 }

 [HttpDelete("{id}")]
 [ProducesResponseType(StatusCodes.Status204NoContent)]
 public async Task<IActionResult> DeletarUsuario(int id)
 {
 var delete = await _usuarioService.DeleteAsync(id);
 if (!delete) return NotFound();
 return NoContent();
 }

 [HttpGet]
 [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
 public async Task<ActionResult<PagedResult<UserResponse>>> GetAll([FromQuery] int page =1, [FromQuery] int pageSize =10, [FromQuery] DateTime? createdFrom = null, [FromQuery] DateTime? createdTo = null)
 {
 var response = await _usuarioService.GetAllUsersAsync(page, pageSize, createdFrom, createdTo, null);
 response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page =1, pageSize, createdFrom, createdTo }), Rel = "first", Method = "GET" });
 response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.TotalPages, pageSize, createdFrom, createdTo }), Rel = "last", Method = "GET" });
 if (response.Page >1) response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page -1, pageSize, createdFrom, createdTo }), Rel = "prev", Method = "GET" });
 if (response.Page < response.TotalPages) response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page +1, pageSize, createdFrom, createdTo }), Rel = "next", Method = "GET" });
 return Ok(response);
 }

 [HttpPut("{id}")]
 [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
 public async Task<IActionResult> Put(int id, [FromBody] AtualizarUserRequest request)
 {
 var atualizado = await _usuarioService.AtualizarPerfilAsync(id, request);
 return Ok(atualizado);
 }

 [HttpPatch("{id}")]
 [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
 public async Task<IActionResult> Patch(int id, [FromBody] AtualizarUserRequest request)
 {
 var atualizado = await _usuarioService.AtualizarPerfilAsync(id, request);
 return Ok(atualizado);
 }
}
