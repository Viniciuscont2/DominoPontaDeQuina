using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Services;
using FluentAssertions;

namespace DominoPontaDeQuina.Tests.Services;

/// <summary>
/// Testes da regra de pontuação: somas múltiplas de 5 geram soma / 5 pontos.
/// </summary>
[Trait("Categoria", "Basico")]
public class PontuacaoServiceTests
{
    private readonly PontuacaoService _service = new();

    [Fact(DisplayName = "Soma 10 deve gerar 2 pontos.")]
    public void CalcularPontos_Soma10_DeveRetornar2()
    {
        // Arrange
        const int soma = 10;

        // Act
        var pontos = _service.CalcularPontos(soma);

        // Assert
        pontos.Should().Be(2);
    }

    [Fact(DisplayName = "Soma 15 deve gerar 3 pontos.")]
    public void CalcularPontos_Soma15_DeveRetornar3()
    {
        // Arrange
        const int soma = 15;

        // Act
        var pontos = _service.CalcularPontos(soma);

        // Assert
        pontos.Should().Be(3);
    }

    [Theory(DisplayName = "Somas múltiplas de 5 devem gerar soma / 5 pontos.")]
    [InlineData(0, 0)]
    [InlineData(5, 1)]
    [InlineData(10, 2)]
    [InlineData(15, 3)]
    [InlineData(20, 4)]
    public void CalcularPontos_SomaMultiplaDeCinco_DeveRetornarSomaDivididaPorCinco(int soma, int esperado)
    {
        // Act
        var pontos = _service.CalcularPontos(soma);

        // Assert
        pontos.Should().Be(esperado);
    }

    [Theory(DisplayName = "Somas que não são múltiplas de 5 não devem pontuar.")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(11)]
    [InlineData(12)]
    public void CalcularPontos_SomaNaoMultiplaDeCinco_DeveRetornarZero(int soma)
    {
        // Act
        var pontos = _service.CalcularPontos(soma);

        // Assert
        pontos.Should().Be(0);
    }

    [Theory(DisplayName = "Soma negativa deve lançar ArgumentOutOfRangeException.")]
    [InlineData(-1)]
    [InlineData(-5)]
    [InlineData(-10)]
    public void CalcularPontos_SomaNegativa_DeveLancarExcecao(int soma)
    {
        // Arrange
        var act = () => _service.CalcularPontos(soma);

        // Act & Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("somaPontas");
    }

    [Fact(DisplayName = "Deve pontuar usando a soma das pontas de um tabuleiro real.")]
    public void CalcularPontos_ComSomaDasPontasDoTabuleiro_DeveRetornarPontosCorretos()
    {
        // Arrange - pontas 5 e 5 => soma 10
        var tabuleiro = new Tabuleiro();
        tabuleiro.Pecas.Add(new Peca(5, 5));

        // Act
        var pontos = _service.CalcularPontos(tabuleiro.SomarPontasExternas());

        // Assert
        pontos.Should().Be(2);
    }

    [Fact(DisplayName = "Tabuleiro vazio (soma 0) não deve gerar pontos.")]
    public void CalcularPontos_TabuleiroVazio_DeveRetornarZero()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act
        var pontos = _service.CalcularPontos(tabuleiro.SomarPontasExternas());

        // Assert
        pontos.Should().Be(0);
    }
}
