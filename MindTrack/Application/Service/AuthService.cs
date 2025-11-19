using AutoMapper;
using FluentValidation;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MindTrack.Application.Service;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IValidator<UserCreateRequest> _validator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository usuarioRepository, TokenService tokenService, IMapper mapper, IValidator<UserCreateRequest> validator, ILogger<AuthService>? logger = null)
    {
        _userRepository = usuarioRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _validator = validator;
        _logger = logger ?? NullLogger<AuthService>.Instance;

    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Tentando login para email {Email}", request.Email);

        var usuario = await _userRepository.BuscarPorEmailAsync(request.Email);

        if (usuario == null)
        {
            _logger.LogWarning("Login falhou. Usuário com email {Email} não encontrado", request.Email);
            throw new Exception("Usuário não encontrado.");
        }

        if (!usuario.CheckPassword(request.Password))
        {
            _logger.LogWarning("Senha inválida para usuário id {UserId}", usuario.Id);
            throw new Exception("Senha inválida.");
        }

        var token = _tokenService.Generate(usuario);
        _logger.LogInformation("Usuário id {UserId} logado com sucesso", usuario.Id);

        return new LoginResponse
        {
            Token = token,
        };
    }

    public async Task<UserResponse> CreateUserAsync(UserCreateRequest userRequest)
    {
        _logger.LogInformation("Criando usuário com email {Email}", userRequest.Email);
        var result = await _validator.ValidateAsync(userRequest);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
                .ToList();

            _logger.LogWarning("Validação falhou ao criar usuário: {Errors}", string.Join(";", errors));

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }

        if (await _userRepository.VerificaEmailExisteAsync(userRequest.Email))
        {
            _logger.LogWarning("Email já cadastrado: {Email}", userRequest.Email);
            throw new InvalidOperationException("Email já cadastrado.");
        }

        var usuario = _mapper.Map<User>(userRequest);
        usuario.SetPassword(userRequest.Password);
        var usuarioCriado = await _userRepository.CriarAsync(usuario);
        _logger.LogInformation("Usuário criado com id {UserId}", usuarioCriado.Id);

        return _mapper.Map<UserResponse>(usuarioCriado);
    }
}
