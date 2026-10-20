namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Calcula a pontuação de uma jogada a partir da soma das pontas externas do tabuleiro.
/// Regra: quando a soma é múltiplo de 5, o time pontua <c>soma / 5</c>; caso contrário, não pontua.
/// </summary>
public class PontuacaoService
{
    /// <summary>
    /// Divisor usado tanto para identificar múltiplos quanto para calcular os pontos.
    /// </summary>
    public const int Divisor = 5;

    /// <summary>
    /// Calcula os pontos gerados pela soma das pontas externas.
    /// </summary>
    /// <param name="somaPontas">A soma das pontas externas do tabuleiro (não pode ser negativa).</param>
    /// <returns>Os pontos obtidos, ou 0 quando a soma não for múltiplo de 5.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Quando <paramref name="somaPontas"/> for negativa.</exception>
    public int CalcularPontos(int somaPontas)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(somaPontas);

        return somaPontas % Divisor == 0 ? somaPontas / Divisor : 0;
    }
}
