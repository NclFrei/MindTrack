using Microsoft.IdentityModel.Tokens;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MindTrack.Application.Service
{
    public class TokenService
    {
        private readonly IJwtSettingsProvider _jwtSettingsProvider;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IJwtSettingsProvider jwtSettingsProvider, ILogger<TokenService>? logger = null)
        {
            _jwtSettingsProvider = jwtSettingsProvider;
            _logger = logger ?? NullLogger<TokenService>.Instance;
        }

        public string Generate(User usuario)
        {
            _logger.LogInformation("Gerando JWT para usuário id {UserId} email {Email}", usuario?.Id, usuario?.Email);

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettingsProvider.SecretKey);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(usuario),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var token = handler.CreateToken(tokenDescriptor);
            var written = handler.WriteToken(token);

            _logger.LogInformation("JWT gerado para usuário id {UserId}", usuario?.Id);

            return written;
        }

        public static ClaimsIdentity GenerateClaims(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var ci = new ClaimsIdentity();

            // Claim padrão usado pelo front (nameid)
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            // Email como Name
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));

            // Email explícito
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            // Opcional: nome de exibição
            if (!string.IsNullOrEmpty(user.Nome))
                ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Nome));

            return ci;
        }
    }
}
