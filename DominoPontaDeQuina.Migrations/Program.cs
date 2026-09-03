using DominoPontaDeQuina.Migrations.Fluxo;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Interfaces;
using DominoPontaDeQuina.Repository.Repositories;
using DominoPontaDeQuina.Services;
using DominoPontaDeQuina.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// Composicao de dependencias (raiz de composicao da aplicacao).
// Nenhuma classe de aplicacao (DbContext, repository ou service) e instanciada com "new" a partir daqui:
// tudo e registrado no container e resolvido por injecao de construtor.
var servicos = new ServiceCollection();

servicos.AddDbContext<DominoDbContext>(options =>
    options.UseSqlite("Data Source=domino.db"));

servicos.AddScoped<IUsuarioRepository, UsuarioRepository>();
servicos.AddScoped<IJogadorRepository, JogadorRepository>();
servicos.AddScoped<IJogoRepository, JogoRepository>();
servicos.AddScoped<IParticipacaoJogoRepository, ParticipacaoJogoRepository>();

servicos.AddScoped<IUsuarioService, UsuarioService>();
servicos.AddScoped<IJogadorService, JogadorService>();
servicos.AddScoped<IJogoService, JogoService>();

servicos.AddScoped<FluxoPrincipalConsole>();

await using var provedor = servicos.BuildServiceProvider();

using var escopo = provedor.CreateScope();

var contexto = escopo.ServiceProvider.GetRequiredService<DominoDbContext>();
await contexto.Database.EnsureCreatedAsync();

var fluxoPrincipal = escopo.ServiceProvider.GetRequiredService<FluxoPrincipalConsole>();
await fluxoPrincipal.ExecutarAsync();
