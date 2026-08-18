
using System.Net;
using System.Text.Json;
using backendAPI.Errors;

namespace backendAPI.Middleware;

public class ExceptionMiddleware(RequestDelegate next,
    ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    
    public async Task InvokeAsync(HttpContext context)
    {
        
        try
        {
            await next(context); // go isprakame contextot(http povikot) ponatamu niz pipeline
                                // ako ima exception, odi vo catch blokot
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message); // go fakjame exception prakticno od bilo kade vo backend

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                                          
        
            var response = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace) // ako e development env
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json); 
        }                                                 

    }

}