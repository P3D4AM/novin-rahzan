using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing;

internal class StiBorderJsonConverter : CustomCreationConverter<StiBorder>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		JToken jToken = JToken.ReadFrom(reader);
		string identName = jToken.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "Ident").Value.ToString();
		StiBorder stiBorder = StiBorderCreator.New(identName);
		if (stiBorder == null)
		{
			return null;
		}
		serializer.Populate(jToken.CreateReader(), stiBorder);
		return stiBorder;
	}

	public override StiBorder Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiBorder;
	}
}
