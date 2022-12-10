namespace Booking.API.Extensions
{
	public static class ExceptionExtensions
	{
		public static string GetMessage(this Exception ex)
		{
			while (ex?.InnerException != null)
			{
				ex = ex.InnerException;
			}
			return ex?.Message;
		}
	}
}
