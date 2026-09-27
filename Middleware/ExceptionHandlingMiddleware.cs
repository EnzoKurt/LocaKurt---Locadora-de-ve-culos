using System.Net;
using System.Text.Json;

namespace LocadoraVeiculos.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteError(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception)
        {
            // Não devolvemos detalhes internos da exceção para o cliente.
            // Isso evita expor informações do banco ou da aplicação.
            await WriteError(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno ao processar a solicitação.");
        }
    }

    private static async Task WriteError(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            mensagem = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
