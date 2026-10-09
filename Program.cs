using System.Reflection;
using LocadoraVeiculos.Data;
using LocadoraVeiculos.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuração da conexão com o banco SQL Server Express.
builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços necessários para a documentação e utilização do Swagger.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LocaKurt - API de Locadora de Veículos",
        Version = "v1",
        Description = "API desenvolvida em C# e ASP.NET Core para gerenciamento de uma locadora de veículos."
    });

    // Lê os comentários XML dos Controllers e mostra essas informações
    // diretamente na documentação do Swagger.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Middleware responsável pelo tratamento dos erros da aplicação.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Habilita a interface do Swagger.
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "LocaKurt API v1");
    options.RoutePrefix = "swagger";
});

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

// Cria o banco caso ele ainda não exista.
// A estrutura continua sendo definida pelas classes do Entity Framework.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LocadoraContext>();
    context.Database.EnsureCreated();
}

app.Run();
