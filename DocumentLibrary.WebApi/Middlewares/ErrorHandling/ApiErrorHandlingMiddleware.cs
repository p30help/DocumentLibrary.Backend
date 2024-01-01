using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.WebApi.ApiModels.Responses;
using Newtonsoft.Json;
using System.Net;

namespace DocumentLibrary.WebApi.Middlewares.ErrorHandling
{
    public class ApiErrorHandlingMiddleware 
    {
        private readonly RequestDelegate _next;

        public ApiErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.StartsWith("/api/"))
            {
                try
                {
                    await _next(context);
                }
                catch (Exception e)
                {
                    await HandleExceptionAsync(context, e);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorResponse = CreateApiError(ex);
            var result = JsonConvert.SerializeObject(errorResponse);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (errorResponse.errorCode == HttpStatusCode.BadRequest.ToString()) ? 400 : 500;
            await context.Response.WriteAsync(result);
        }

        private static ApiError CreateApiError(Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = ex.Message;

            if (ex is ArgumentNullException ||
                ex is ArgumentException ||
                ex is DomainStateException ||
                ex is InvalidValueObjectStateException
                )
            {
                code = HttpStatusCode.BadRequest;
                message = ex.Message;
            }

            return new ApiError(code.ToString(), message);
        }
    }
    
    public static class ApiErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiErrorHandlingMiddleware>();
        }
    }
}
