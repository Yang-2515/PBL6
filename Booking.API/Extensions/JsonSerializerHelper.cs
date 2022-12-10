using System.Text.Json;

namespace Booking.API.Extensions
{
	public static class JsonSerializerHelper
	{
		public static T Deserialize<T>(string jsonString, JsonSerializerOptions options = null)
		{
			return JsonSerializer.Deserialize<T>(jsonString, options ?? AppJsonSerializerOptions.CamelCase);
		}

		public static object Deserialize(string jsonString, Type type)
		{
			return JsonSerializer.Deserialize(jsonString, type, AppJsonSerializerOptions.CamelCase);
		}

		public static string Serialize(object data, Type dataType = default, JsonSerializerOptions options = null)
		{
			dataType = dataType ?? (dataType = data.GetType());
			return JsonSerializer.Serialize(data, dataType, options ?? AppJsonSerializerOptions.CamelCase);
		}

		public static T DefaultDeserialize<T>(string jsonString)
		{
			return JsonSerializer.Deserialize<T>(jsonString);
		}

		public static object DefaultDeserialize(string jsonString, Type type)
		{
			return JsonSerializer.Deserialize(jsonString, type);
		}

		public static string DefaultSerialize(object data, Type dataType = default)
		{
			dataType = dataType ?? (dataType = data.GetType());

			return JsonSerializer.Serialize(data, dataType);
		}
	}
}
