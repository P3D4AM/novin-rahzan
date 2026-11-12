using System;
using System.Globalization;
using Stimulsoft.Base.Json.Utilities;

namespace Stimulsoft.Base.Json.Converters;

public class JavaScriptDateTimeConverter : DateTimeConverterBase
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		long value2;
		if (value is DateTime dateTime)
		{
			DateTime dateTime2 = dateTime.ToUniversalTime();
			value2 = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime2);
		}
		else
		{
			if (!(value is DateTimeOffset dateTimeOffset))
			{
				throw new JsonSerializationException("Expected date object value.");
			}
			value2 = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.ToUniversalTime().UtcDateTime);
		}
		writer.WriteStartConstructor("Date");
		writer.WriteValue(value2);
		writer.WriteEndConstructor();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
		if (reader.TokenType == JsonToken.Null)
		{
			if (!ReflectionUtils.IsNullable(objectType))
			{
				throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			return null;
		}
		if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
		{
			throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
		}
		reader.Read();
		if (reader.TokenType != JsonToken.Integer)
		{
			throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}
		long javaScriptTicks = (long)reader.Value;
		DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
		reader.Read();
		if (reader.TokenType != JsonToken.EndConstructor)
		{
			throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected EndConstructor, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}
		if (type == typeof(DateTimeOffset))
		{
			return new DateTimeOffset(dateTime);
		}
		return dateTime;
	}
}
