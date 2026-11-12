using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Data.Morphs;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class GrouperConverter : JsonConverter<Grouper>
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, Grouper value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "Grouper"));
	}

	public override Grouper ReadJson(JsonReader reader, Type objectType, Grouper existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		switch (reader.TokenType)
		{
		case JsonToken.StartArray:
			return new MultipleFieldGrouper(serializer.Deserialize<string[]>(reader));
		case JsonToken.String:
			return new MultipleFieldGrouper(new string[1] { reader.Value.Normalize<string>() });
		case JsonToken.Null:
		case JsonToken.Undefined:
			return Grouper.Default;
		default:
			throw SerializationUtil.CreateWrongTokenException(reader, JsonToken.String, JsonToken.Null);
		}
	}
}
