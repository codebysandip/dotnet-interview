
using System.Text;
using System.Text.Json;
using ReviseDotnet.Exceptions;
using ReviseDotnet.Models;

namespace ApiRezolveHotel.Middlewares;

/// <summary>
/// Custom Response Middleware will intercept every request and will convert
/// response to format of ApiResponse class
/// </summary>
public class CustomResponseMiddleware {
    private RequestDelegate _next;
    public CustomResponseMiddleware(RequestDelegate next) {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        if (context.Request.Path.Value.StartsWith("/api")) {
            context.Request.EnableBuffering();

            Stream responseBody = context.Response.Body;


            using (var newResponseBody = new MemoryStream()) {
                context.Response.Body = newResponseBody;
                var apiResponse = new ApiResponse<Object> {
                    Status = 200,
                    Messages = new List<string>(),
                    Data = new {},
                    ErrorCode = -1
                };
                try {
                    await _next(context);
                } catch(Exception e) {
                    // check different type of instance here to send proper error message and error code
                    // [TODO] Handle Bad request for form validation message
                    apiResponse.Status = 500;
                    if (e is BadRequestException) {
                        apiResponse.ErrorCode = (e as BadRequestException).ErrorCode;
                        apiResponse.Status = 400;
                        apiResponse.Messages.Add(e.Message);
                    } else if (e is InternalServerException) {
                        apiResponse.ErrorCode = (e as InternalServerException).ErrorCode;
                    } else {
                        apiResponse.Messages.Add("Something went wrong. Please try after some time");
                        // [TODO] Log Unknown Error in database
                    }
                    Console.WriteLine($"expection!! {e.Message}");
                    Console.WriteLine($"stack trace!! {e.StackTrace}");

                    await ReturnBody(context, apiResponse, responseBody);
                    return;
                }

                newResponseBody.Seek(0, SeekOrigin.Begin);

                var response = new StreamReader(newResponseBody).ReadToEnd();
                newResponseBody.Seek(0, SeekOrigin.Begin);
                Object obj;
                // check if controller sent null or empty
                if (!String.IsNullOrEmpty(response)) {
                    obj = JsonSerializer.Deserialize<Object>(response);
                } else {
                    obj = null;
                }
                apiResponse.Data = obj;
                // Always send 200 status. Response Body status will be actual status
                context.Response.StatusCode = 200;
                await ReturnBody(context, apiResponse, responseBody);
            }
        } else {
            await _next(context);
        }
    }

    private async Task ReturnBody(HttpContext context, ApiResponse<Object> apiResponse, Stream originBody) {
        // create string content response
        var requestContent = new StringContent(JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions(JsonSerializerDefaults.Web)), Encoding.UTF8, "application/json");
        context.Response.Body = await requestContent.ReadAsStreamAsync();//modified stream
                                                                         // set new content length of body
        context.Response.ContentLength = context.Response.Body.Length;

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        await context.Response.Body.CopyToAsync(originBody);
        context.Response.Body = originBody;
    }
}

public static class CustomResponseMiddlewareExtensions {
    public static IApplicationBuilder UseCustomResponse(
        this IApplicationBuilder builder) {
        return builder.UseMiddleware<CustomResponseMiddleware>();
    }
}