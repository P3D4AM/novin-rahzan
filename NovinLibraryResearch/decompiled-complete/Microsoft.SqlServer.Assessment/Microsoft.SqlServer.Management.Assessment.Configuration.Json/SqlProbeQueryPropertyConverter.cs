using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class SqlProbeQueryPropertyConverter : JsonConverter
{
	public override bool CanWrite => false;

	public override bool CanConvert(Type objectType)
	{
		if (!(typeof(string) == objectType))
		{
			return typeof(string[]) == objectType;
		}
		return true;
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		return reader.TokenType switch
		{
			JsonToken.String => new string[1] { reader.Value.ToString() }, 
			JsonToken.StartArray => serializer.Deserialize<string[]>(reader), 
			_ => throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnexpectedToken, reader.TokenType, "QueryProperty"), 
		};
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "Probe"));
	}
}
