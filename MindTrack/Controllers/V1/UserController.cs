using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Erros;

namespace MindTrack.Controllers.V1;

/// <summary>
/// Endpoints para gerenciamento de usuários (obter, listar, atualizar e remover).
/// </summary>
[ApiVersion("1.0")]
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

    /// <summary>
    /// Recupera um usuário pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador do usuário</param>
    /// <returns>Informações do usuário</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> ObterUsuario(int id)
    {
        var usuario = await _usuarioService.GetUserByIdAsync(id);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    /// <summary>
    /// Remove um usuário pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do usuário a ser removido</param>
    /// <returns>204 se excluído com sucesso ou404 se não encontrado</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletarUsuario(int id)
    {
        var delete = await _usuarioService.DeleteAsync(id);
        if (!delete)
            return NotFound($"Não foi possível remover: usuário com ID {id} não encontrado.");

        return NoContent();
    }

    /// <summary>
    /// Recupera todos os usuários.
    /// </summary>
    /// <returns>Lista de usuários</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _usuarioService.GetAllUsersAsync(page, pageSize);
        response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = 1, pageSize }), Rel = "first", Method = "GET" });
        response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.TotalPages, pageSize }), Rel = "last", Method = "GET" });
        if (response.Page > 1)
            response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page - 1, pageSize }), Rel = "prev", Method = "GET" });
        if (response.Page < response.TotalPages)
            response.Links.Add(new Link { Href = Url.ActionLink(nameof(GetAll), values: new { page = response.Page + 1, pageSize }), Rel = "next", Method = "GET" });

        return Ok(response);
    }

    /// <summary>
    /// Atualiza parcialmente o perfil do usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário a ser atualizado</param>
    /// <param name="request">Dados para atualização do perfil</param>
    /// <returns>Usuário atualizado</returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> AtualizarPerfil(int id, [FromBody] AtualizarUserRequest request)
    {
        var atualizado = await _usuarioService.AtualizarPerfilAsync(id, request);
        return Ok(atualizado);
    }
}