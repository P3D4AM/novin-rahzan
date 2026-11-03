using System;
using System.Drawing.Imaging;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Report.Export;

public sealed class StiImageFormatJsonConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		ImageFormat val = (ImageFormat)((value is ImageFormat) ? value : null);
		if (val != null)
		{
			writer.WriteValue(val.Guid.ToString());
		}
	}

	public override object ReadJson(JsonReader reader, Type objectType, object value, JsonSerializer serializer)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (reader.Value != null && reader.ValueType == typeof(string))
		{
			try
			{
				string g = reader.Value as string;
				return (object)new ImageFormat(new Guid(g));
			}
			catch
			{
			}
		}
		return ImageFormat.Jpeg;
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(ImageFormat);
	}
}
