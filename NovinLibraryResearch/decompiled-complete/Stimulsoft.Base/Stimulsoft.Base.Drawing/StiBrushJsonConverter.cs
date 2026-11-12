using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing;

internal class StiBrushJsonConverter : CustomCreationConverter<StiBrush>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		JToken jToken = JToken.ReadFrom(reader);
		string identName = jToken.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "Ident").Value.ToString();
		StiBrush stiBrush = StiBrushCreator.New(identName);
		if (stiBrush == null)
		{
			return null;
		}
		serializer.Populate(jToken.CreateReader(), stiBrush);
		return stiBrush;
	}

	public override StiBrush Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiBrush;
	}
}
