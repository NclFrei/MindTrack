using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;

namespace MindTrack.Controllers.V1
{
    [Route("api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MetaResponse>> CreateMetaAsync([FromBody] MetaCreateRequest request)
        {
            var response = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MetaResponse>> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MetaResponse>> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response == null)
                return NotFound($"Meta com ID {id} não encontrada.");

            return Ok(response);
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(MetaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MetaResponse>> PatchArea(int id, [FromBody] AtualizarMetaRequest request)
        {
            if (request == null)
                return BadRequest("Request inválido.");

            var updated = await _service.UpdateAsync(id, request);
            if (updated == null)
                return NotFound($"Meta com ID {id} não encontrada para atualização.");

            return Ok(updated);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Área com ID {id} não encontrada para exclusão.");

            return NoContent();
        }
    }
}
