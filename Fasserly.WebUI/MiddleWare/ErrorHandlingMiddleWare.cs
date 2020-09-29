﻿using Fasserly.Infrastructure.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fasserly.WebUI.MiddleWare
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleWare> _logger;

        public ErrorHandlingMiddleWare(RequestDelegate next, ILogger<ErrorHandlingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsyn(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsyn(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleWare> logger)
        {
            object errors = null;

            switch (ex)
            {
                case RestException re:
                    logger.LogError(ex, "REST ERROR");
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                case Exception e:
                    logger.LogError(ex, "SERVER ERRROR");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Errror" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                var result = JsonSerializer.Serialize(new { errors });
                await context.Response.WriteAsync(result);
            }
        }

    }
}
