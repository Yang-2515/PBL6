using Booking.API.Extensions;
using Booking.API.ViewModel;
using System.Net;

namespace Booking.Middleware
{
    public class RequestLoggerMiddleware
    {
		private readonly RequestDelegate _next;
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<RequestLoggerMiddleware> _logger;

		public RequestLoggerMiddleware(RequestDelegate next
			, IWebHostEnvironment env
			, ILogger<RequestLoggerMiddleware> logger)
		{
			_env = env;
			_logger = logger;
			_next = next;
		}

        public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			context.Response.ContentType = "application/json";
			if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
				return;

			bool isThrowMsg;
			// Is http exception
			if (typeof(HttpException).IsAssignableFrom(ex.GetType()))
			{
				var httpEx = ex as HttpException;
				context.Response.StatusCode = (int)httpEx.StatusCode;

				if (httpEx.IsClientError())
					_logger.LogWarning(ex.Message);
				isThrowMsg = httpEx.IsClientError();
			}
			// If exception is Domain exception, this is a badrequest response
			else if (typeof(DomainException).IsAssignableFrom(ex.GetType()))
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				isThrowMsg = true;
				_logger.LogWarning(ex.Message);
			}
			// the Internal Server Error
			else
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				isThrowMsg = false;
				_logger.LogError(ex, ex.Message);
			}

			// Prepare error message model to return to client
			var message = !_env.IsProduction() || isThrowMsg ? ex.GetMessage() : "InternalServerError";
			var error = new ErrorViewModel(message, !_env.IsProduction() ? ex.StackTrace : null);

			// Return as json
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(JsonSerializerHelper.Serialize(error));
		}
	}
}