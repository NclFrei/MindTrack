using AutoMapper;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MindTrack.Application.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService>? logger = null)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger ?? NullLogger<UserService>.Instance;
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Obtendo usuário por id {UserId}", id);
            var usuario = await _userRepository.BuscarPorIdAsync(id);

            if (usuario == null)
            {
                _logger.LogWarning("Usuário com id {UserId} não encontrado", id);
                return null;
            }

            _logger.LogInformation("Usuário com id {UserId} encontrado", id);
            return _mapper.Map<UserResponse>(usuario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Excluindo usuário com id {UserId}", id);
            var usuario = await _userRepository.BuscarPorIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Falha ao excluir. Usuário com id {UserId} não encontrado", id);
                return false;
            }

            var result = await _userRepository.DeleteAsync(usuario);
            _logger.LogInformation("Resultado da exclusão para usuário id {UserId}: {Result}", id, result);
            return result;
        }

        public async Task<PagedResult<UserResponse>> GetAllUsersAsync(int page = 1, int pageSize = 10, DateTime? createdFrom = null, DateTime? createdTo = null, string? nomeContains = null)
        {
            _logger.LogInformation("Obtendo usuários paginação: página {Page} tamanho {PageSize}", page, pageSize);
            var users = await _userRepository.GetAllAsync();

            if (createdFrom.HasValue)
                users = users.Where(u => u.DataCriacao >= createdFrom.Value).ToList();
            if (createdTo.HasValue)
                users = users.Where(u => u.DataCriacao <= createdTo.Value).ToList();
            if (!string.IsNullOrWhiteSpace(nomeContains))
                users = users.Where(u => u.Nome != null && u.Nome.Contains(nomeContains, StringComparison.InvariantCultureIgnoreCase)).ToList();

            var total = users.Count;
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new PagedResult<UserResponse>
            {
                Items = _mapper.Map<List<UserResponse>>(items),
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = totalPages
            };

            return result;
        }

        public async Task<UserResponse> AtualizarPerfilAsync(int id, AtualizarUserRequest request)
        {
            _logger.LogInformation("Atualizando perfil do usuário id {UserId}", id);
            var usuario = await _userRepository.BuscarPorIdAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Falha na atualização. Usuário com id {UserId} não encontrado", id);
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            _mapper.Map(request, usuario);

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                usuario.SetPassword(request.Password);
                _logger.LogInformation("Senha atualizada para usuário id {UserId}", id);
            }

            await _userRepository.AtualizarAsync(usuario);


            _logger.LogInformation("Usuário id {UserId} atualizado com sucesso", id);
            return _mapper.Map<UserResponse>(usuario);
        }

    }
}
