using EcoEnergia.Api.Data;              // Acesso ao AppDbContext
using Microsoft.EntityFrameworkCore;    // Acesso ao UseOracle

var builder = WebApplication.CreateBuilder(args);

// Registra o AppDbContext usando o Oracle, com a connection string dos User Secrets
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleFIAP")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();