using System.Net;
using System.Text.Json;
using FreelancerManager.Domain.Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FreelancerManager.API.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ocorreu um erro não tratado na API.");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		var statusCode = HttpStatusCode.InternalServerError;
		var message = "Ocorreu um erro interno no servidor.";

		switch (exception)
		{
			// 1. Erros de Regra de Negócio do Domínio (Guard Clauses)
			case DomainException domainEx:
				statusCode = HttpStatusCode.BadRequest;
				message = domainEx.Message;
				break;

			// 2. Erros do SQLite (Ex: UNIQUE constraint)
			case DbUpdateException dbEx when dbEx.InnerException is SqliteException sqliteEx:
				if (sqliteEx.Message.Contains("UNIQUE constraint failed"))
				{
					statusCode = HttpStatusCode.Conflict; // 409
					message = "Já existe um registro com este valor único (ex: Número do Recibo já utilizado).";
				}
				else
				{
					statusCode = HttpStatusCode.BadRequest;
					message = "Erro ao tentar salvar os dados no banco.";
				}
				break;
		}

		context.Response.StatusCode = (int)statusCode;

		var responseBody = JsonSerializer.Serialize(new
		{
			Status = (int)statusCode,
			Error = message,
			TraceId = context.TraceIdentifier
		});

		await context.Response.WriteAsync(responseBody);
	}
}