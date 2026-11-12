using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class RangeListConverter<T> : JsonConverter<RangeList<T>> where T : IComparable<T>
{
	public override void WriteJson(JsonWriter writer, RangeList<T> value, JsonSerializer serializer)
	{
		if (value == null || value.Ranges.Count == 0)
		{
			writer.WriteNull();
		}
		else if (value.Ranges.Count == 1)
		{
			serializer.Serialize(writer, value.Ranges[0]);
		}
		else
		{
			serializer.Serialize(writer, value.Ranges);
		}
	}

	public override RangeList<T> ReadJson(JsonReader reader, Type objectType, RangeList<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType != JsonToken.Null)
		{
			return new RangeList<T>(ReadRanges(reader, serializer));
		}
		return new RangeList<T>();
	}

	private static IEnumerable<Range<T>> ReadRanges(JsonReader reader, JsonSerializer serializer)
	{
		switch (reader.TokenType)
		{
		case JsonToken.String:
		{
			Range<T> range = serializer.Deserialize<Range<T>>(reader);
			if (!range.IsEmpty)
			{
				yield return range;
			}
			break;
		}
		case JsonToken.StartArray:
		{
			Range<T>[] array = serializer.Deserialize<Range<T>[]>(reader);
			for (int i = 0; i < array.Length; i++)
			{
				yield return array[i];
			}
			break;
		}
		default:
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnexpectedToken, reader.TokenType, "RangeList");
		}
	}
}
