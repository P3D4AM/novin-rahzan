using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal abstract class JsonConverter<T> : JsonConverter
{
	public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		if (!((value != null) ? (value is T) : IsNullable(typeof(T))))
		{
			throw new JsonSerializationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorConverterCannotWriteValue, typeof(T)));
		}
		WriteJson(writer, (T)value, serializer);
	}

	public abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);

	public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		bool flag = existingValue == null;
		if (!flag && !(existingValue is T))
		{
			throw new JsonSerializationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorConverterCannotReadJSONValue, typeof(T)));
		}
		return ReadJson(reader, objectType, flag ? default(T) : ((T)existingValue), !flag, serializer);
	}

	public abstract T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer);

	public sealed override bool CanConvert(Type objectType)
	{
		return typeof(T).IsAssignableFrom(objectType);
	}

	private static bool IsNullable(Type t)
	{
		if (t.IsValueType)
		{
			return IsNullableType(t);
		}
		return true;
	}

	private static bool IsNullableType(Type t)
	{
		return Nullable.GetUnderlyingType(t) != null;
	}
}
