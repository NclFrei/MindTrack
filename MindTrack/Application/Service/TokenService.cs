using Microsoft.IdentityModel.Tokens;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MindTrack.Application.Service;

public class TokenService
{
    private readonly IJwtSettingsProvider _jwtSettingsProvider;

    public TokenService(IJwtSettingsProvider jwtSettingsProvider)
    {
        _jwtSettingsProvider = jwtSettingsProvider;
    }

    public string Generate(User usuario)
    {
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

        return handler.WriteToken(token);
    }


    public static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(
            new Claim(ClaimTypes.Name, user.Email));

        return ci;
    }
}