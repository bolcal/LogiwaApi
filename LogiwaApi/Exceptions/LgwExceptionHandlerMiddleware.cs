using System.Net;
using System.Text.Json;

namespace LogiwaApi.Exceptions
{
    public class LgwExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        public LgwExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (EntityNotFoundException notfound)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                await response.WriteAsync(notfound.Message);
            }
            catch(InvalidCategoryException invalidCategory)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                await response.WriteAsync(invalidCategory.Message);
            }
            catch (InvalidProductTitleException invalidTitle)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync(invalidTitle.Message);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    message = ex.Message,
                    statusCode = response.StatusCode
                };
                var errorJson = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(errorJson);
            }
        }
    }
}
