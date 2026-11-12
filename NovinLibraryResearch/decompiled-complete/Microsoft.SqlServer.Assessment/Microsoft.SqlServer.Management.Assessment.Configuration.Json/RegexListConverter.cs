using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class RegexListConverter : JsonConverter<List<Regex>>
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, List<Regex> value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override List<Regex> ReadJson(JsonReader reader, Type objectType, List<Regex> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JsonToken tokenType = reader.TokenType;
		if (tokenType == JsonToken.StartArray)
		{
			return serializer.Deserialize<List<Regex>>(reader);
		}
		List<Regex> list = new List<Regex>();
		list.Add(serializer.Deserialize<Regex>(reader));
		return list;
	}
}
