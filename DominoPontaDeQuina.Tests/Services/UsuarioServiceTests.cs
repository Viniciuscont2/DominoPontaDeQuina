using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Exceptions;
using DominoPontaDeQuina.Domain.Interfaces;
using DominoPontaDeQuina.Domain.Services;
using FluentAssertions;
using Moq;

namespace DominoPontaDeQuina.Tests.Services;

/// <summary>
/// Testes do UsuarioService com repositório simulado via Moq,
/// validando as decisões de consultar e salvar.
/// </summary>
[Trait("Categoria", "Basico")]
public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _repository = new();
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _service = new UsuarioService(_repository.Object);
    }

    #region RegistrarAsync

    [Fact(DisplayName = "Deve consultar o e-mail e salvar quando o usuário não existir.")]
    public async Task RegistrarAsync_EmailInexistente_DeveConsultarESalvar()
    {
        // Arrange
        _repository
            .Setup(r => r.ObterPorEmailAsync("ana@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var usuario = await _service.RegistrarAsync("Ana", "ana@email.com", "hash123");

        // Assert
        usuario.Nome.Should().Be("Ana");
        usuario.Email.Should().Be("ana@email.com");
        usuario.HashSenha.Should().Be("hash123");

        _repository.Verify(r => r.ObterPorEmailAsync("ana@email.com", It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(r => r.AdicionarAsync(
            It.Is<Usuario>(u => u.Email == "ana@email.com" && u.Nome == "Ana"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve normalizar o e-mail antes de consultar e salvar.")]
    public async Task RegistrarAsync_EmailComEspacosEMaiusculas_DeveNormalizar()
    {
        // Arrange
        _repository
            .Setup(r => r.ObterPorEmailAsync("ana@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var usuario = await _service.RegistrarAsync("  Ana  ", "  Ana@Email.COM ", "hash123");

        // Assert
        usuario.Email.Should().Be("ana@email.com");
        usuario.Nome.Should().Be("Ana");
        _repository.Verify(r => r.ObterPorEmailAsync("ana@email.com", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Não deve salvar e deve lançar exceção quando o e-mail já existir.")]
    public async Task RegistrarAsync_EmailJaCadastrado_DeveLancarExcecaoESemSalvar()
    {
        // Arrange
        _repository
            .Setup(r => r.ObterPorEmailAsync("ana@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Nome = "Outra Ana", Email = "ana@email.com" });

        Func<Task> act = () => _service.RegistrarAsync("Ana", "ana@email.com", "hash123");

        // Act & Assert
        var excecao = await act.Should().ThrowAsync<EmailJaCadastradoException>()
            .WithMessage("*ana@email.com*");
        excecao.Which.Email.Should().Be("ana@email.com");

        _repository.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory(DisplayName = "Dados obrigatórios vazios devem lançar exceção sem tocar no repositório.")]
    [InlineData(null, "ana@email.com", "hash")]
    [InlineData("", "ana@email.com", "hash")]
    [InlineData("   ", "ana@email.com", "hash")]
    [InlineData("Ana", null, "hash")]
    [InlineData("Ana", "", "hash")]
    [InlineData("Ana", "ana@email.com", null)]
    [InlineData("Ana", "ana@email.com", " ")]
    public async Task RegistrarAsync_DadosInvalidos_DeveLancarArgumentException(string? nome, string? email, string? hash)
    {
        // Arrange
        Func<Task> act = () => _service.RegistrarAsync(nome!, email!, hash!);

        // Act & Assert
        await act.Should().ThrowAsync<ArgumentException>();
        _repository.VerifyNoOtherCalls();
    }

    #endregion

    #region ObterPorEmailAsync

    [Fact(DisplayName = "Deve retornar o usuário quando o repositório o encontrar.")]
    public async Task ObterPorEmailAsync_UsuarioExistente_DeveRetornarUsuario()
    {
        // Arrange
        var esperado = new Usuario { Nome = "Bia", Email = "bia@email.com" };
        _repository
            .Setup(r => r.ObterPorEmailAsync("bia@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(esperado);

        // Act
        var resultado = await _service.ObterPorEmailAsync("BIA@email.com");

        // Assert
        resultado.Should().BeSameAs(esperado);
        _repository.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar nulo quando o usuário não existir.")]
    public async Task ObterPorEmailAsync_UsuarioInexistente_DeveRetornarNulo()
    {
        // Arrange
        _repository
            .Setup(r => r.ObterPorEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var resultado = await _service.ObterPorEmailAsync("ninguem@email.com");

        // Assert
        resultado.Should().BeNull();
    }

    [Theory(DisplayName = "E-mail vazio na consulta deve lançar exceção.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void ObterPorEmailAsync_EmailInvalido_DeveLancarArgumentException(string? email)
    {
        // Arrange
        Action act = () => _service.ObterPorEmailAsync(email!);

        // Act & Assert
        act.Should().Throw<ArgumentException>();
        _repository.VerifyNoOtherCalls();
    }

    #endregion
}
