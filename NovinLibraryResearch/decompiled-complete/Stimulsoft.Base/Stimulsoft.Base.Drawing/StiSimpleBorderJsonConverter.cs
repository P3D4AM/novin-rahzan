using System;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing;

internal class StiSimpleBorderJsonConverter : CustomCreationConverter<StiSimpleBorder>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		JToken jToken = JToken.ReadFrom(reader);
		StiSimpleBorder stiSimpleBorder = new StiSimpleBorder();
		serializer.Populate(jToken.CreateReader(), stiSimpleBorder);
		return stiSimpleBorder;
	}

	public override StiSimpleBorder Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiSimpleBorder;
	}
}
