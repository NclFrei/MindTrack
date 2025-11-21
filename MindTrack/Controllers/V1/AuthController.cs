using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Erros;

namespace MindTrack.Controllers.V1
{
    /// <summary>
    /// Endpoints responsáveis pela autenticação e criação de usuários.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login com email e senha.
        /// </summary>
        /// <param name="request">Credenciais do usuário</param>
        /// <returns>Token JWT e informações do usuário</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Cadastra um novo usuário na plataforma.
        /// </summary>
        /// <param name="request">Dados do usuário a ser criado</param>
        /// <returns>Usuário criado</returns>
        [HttpPost("cadastro")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponse>> CriarUsuario([FromBody] UserCreateRequest request)
        {
            var usuarioResponse = await _authService.CreateUserAsync(request);
            return CreatedAtAction(null, usuarioResponse);
        }
    }
}
