using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DominoPontaDeQuina.Api.Options;
using DominoPontaDeQuina.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DominoPontaDeQuina.Api.Services;

/// <inheritdoc cref="ITokenService"/>
public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private readonly JwtOptions _options = jwtOptions.Value;

    /// <inheritdoc />
    public (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario)
    {
        var expiraEm = DateTime.UtcNow.AddMinutes(_options.ExpiracaoMinutos);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
            new(ClaimTypes.Name, usuario.Nome),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credenciais = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiraEm,
            signingCredentials: credenciais);

        var tokenSerializado = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenSerializado, expiraEm);
    }
}
