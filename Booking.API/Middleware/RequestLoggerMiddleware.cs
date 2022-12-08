namespace Booking.Middleware
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"The following error happened: {ex.Message}");
                throw;
            }
            finally
            {
                if (context.Response?.StatusCode >= 500)
                {
                    _logger.LogError("Request {method} {url} => {statusCode}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);
                }
                else
                {
                    if(context.Response?.StatusCode >= 400 && context.Response?.StatusCode < 500)
                    {
                        _logger.LogWarning("Request {method} {url} => {statusCode}",
                            context.Request?.Method,
                            context.Request?.Path.Value,
                            context.Response?.StatusCode); 
                    }
                    else
                    {
                        _logger.LogInformation("Request {method} {url} => {statusCode}",
                            context.Request?.Method,
                            context.Request?.Path.Value,
                            context.Response?.StatusCode);
                    }
                }
                Console.WriteLine(DateTime.Now.ToLongTimeString());
            }
        }
    }
}