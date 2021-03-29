using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LatenessManager.Api.Middleware
{
    public static class HandleExceptionsMiddleware
    {
        private static readonly Dictionary<Type, Func<HttpContext, Task>> ExceptionHandlers = new()
        {
            {typeof(ValidationException), HandleValidationException}
        };
        
        public static IApplicationBuilder HandleExceptions(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseExceptionHandler(errorAppBuilder =>
            {
                errorAppBuilder.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    if (ExceptionHandlers.ContainsKey(exception.GetType()))
                    {
                        await ExceptionHandlers[exception.GetType()](context);
                    }
                });
            });
            
            return appBuilder;
        }

        private static async Task HandleValidationException(HttpContext context)
        {
            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = (ValidationException)errorFeature.Error;
            
            var errors = exception.Errors.Select(error => new
            {
                PropertyName = error.Key,
                ErrorCodes = error.Value.Select(a => a.ErrorCode),
                ErrorMessages = error.Value.Select(a => a.ErrorMessage)
            });

            var json = JsonSerializer.Serialize(errors, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json, Encoding.UTF8);
        }
    }
}