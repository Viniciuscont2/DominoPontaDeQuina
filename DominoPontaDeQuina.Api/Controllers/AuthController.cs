using System.Security.Claims;
using DominoPontaDeQuina.Api.Dtos;
using DominoPontaDeQuina.Api.Services;
using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.Api.Controllers;

/// <summary>
/// Expõe os endpoints de registro, login e verificação de usuários autenticados.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IUsuarioRepository usuarioRepository, ITokenService tokenService) : ControllerBase
{
    /// <summary>
    /// Hasher de senhas do ASP.NET Core Identity. É seguro compartilhar a mesma instância
    /// porque a classe não guarda estado entre chamadas.
    /// </summary>
    private static readonly PasswordHasher<Usuario> _hasher = new();

    /// <summary>
    /// Cria uma nova conta de usuário e já retorna um token de acesso.
    /// </summary>
    [HttpPost("registrar")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Registrar(RegistrarRequest request)
    {
        var usuarioExistente = await usuarioRepository.BuscarPorEmailAsync(request.Email);
        if (usuarioExistente is not null)
            return Conflict("Já existe uma conta cadastrada com este e-mail.");

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email
        };

        usuario.HashSenha = _hasher.HashPassword(usuario, request.Senha);

        await usuarioRepository.AdicionarAsync(usuario);
        await usuarioRepository.SalvarAsync();

        var (token, expiraEm) = tokenService.GerarToken(usuario);

        return Ok(new AuthResponse(token, expiraEm, usuario.Id, usuario.Nome, usuario.Email));
    }

    /// <summary>
    /// Autentica um usuário já cadastrado e retorna um token de acesso.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var usuario = await usuarioRepository.BuscarPorEmailAsync(request.Email);
        if (usuario is null)
            return Unauthorized("E-mail ou senha inválidos.");

        var resultado = _hasher.VerifyHashedPassword(usuario, usuario.HashSenha, request.Senha);
        if (resultado == PasswordVerificationResult.Failed)
            return Unauthorized("E-mail ou senha inválidos.");

        var (token, expiraEm) = tokenService.GerarToken(usuario);

        return Ok(new AuthResponse(token, expiraEm, usuario.Id, usuario.Nome, usuario.Email));
    }

    /// <summary>
    /// Endpoint protegido de exemplo. Só responde quando um token JWT válido é enviado
    /// no cabeçalho "Authorization: Bearer {token}".
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var nome = User.FindFirstValue(ClaimTypes.Name);
        var email = User.FindFirstValue(ClaimTypes.Email);

        return Ok(new { usuarioId = id, nome, email });
    }
}
