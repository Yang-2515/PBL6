using System.Net;
using System.Runtime.Serialization;

namespace Booking.API.Extensions
{
	public class BadRequestException : HttpException
	{

		public BadRequestException(string message)
			: base(HttpStatusCode.BadRequest, message)
		{
		}

		public BadRequestException(string message, Exception innerException)
			: base(HttpStatusCode.BadRequest, message, innerException)
		{
		}

		protected BadRequestException(SerializationInfo info, StreamingContext context)
			: base(HttpStatusCode.BadRequest, info, context)
		{
		}
	}
}
