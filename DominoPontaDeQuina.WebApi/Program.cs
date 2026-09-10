using DominoPontaDeQuina.Application;
using DominoPontaDeQuina.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Camada de apresentacao (Controllers)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuracao das dependencias das camadas internas.
// A WebApi e a "composition root": e aqui que Application e Infrastructure sao conectadas.
builder.Services.AddDominoApplication();
builder.Services.AddDominoInfrastructure(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
