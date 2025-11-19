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

        public async Task<List<UserResponse>>GetAllUsersAsync()
        {
            _logger.LogInformation("Obtendo todos os usuários");
            var users = await _userRepository.GetAllAsync();
            _logger.LogInformation("Recuperados {Count} usuários", users?.Count ??0);
            return _mapper.Map<List<UserResponse>>(users);
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
