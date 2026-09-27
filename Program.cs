using LocadoraVeiculos.Data;
using LocadoraVeiculos.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

// Em ambiente de desenvolvimento, cria o banco caso ele ainda não exista.
// Para um projeto acadêmico, a criação por migration continua sendo a forma
// recomendada para versionar a estrutura do banco.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LocadoraContext>();
    context.Database.EnsureCreated();
}

app.Run();
