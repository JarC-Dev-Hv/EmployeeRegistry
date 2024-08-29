using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Serilog;

namespace EmployeeRegistry.API.Utilities
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                Log.Error(ex, "Validation error: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
            }
            catch (ServiceException ex)
            {
                Log.Error(ex, "Service error: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorDetails = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
