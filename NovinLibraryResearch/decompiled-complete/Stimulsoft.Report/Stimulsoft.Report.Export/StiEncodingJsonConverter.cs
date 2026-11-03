using System;
using System.Text;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Report.Export;

public sealed class StiEncodingJsonConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		if (value is Encoding encoding)
		{
			writer.WriteValue(encoding.CodePage);
		}
	}

	public override object ReadJson(JsonReader reader, Type objectType, object value, JsonSerializer serializer)
	{
		if (reader.Value != null && reader.ValueType == typeof(long))
		{
			try
			{
				int codepage = Convert.ToInt32(reader.Value);
				return Encoding.GetEncoding(codepage);
			}
			catch
			{
			}
		}
		return Encoding.Default;
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Encoding);
	}
}
