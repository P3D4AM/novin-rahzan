using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Dashboard.Components.Helpers;

internal class StiMeterJsonConverter : CustomCreationConverter<StiMeter>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		JToken jToken = JToken.ReadFrom(reader);
		string identName = jToken.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "Ident").Value.ToString();
		StiMeter stiMeter = StiMeterCreator.New(identName);
		if (stiMeter == null)
		{
			return null;
		}
		serializer.Populate(jToken.CreateReader(), stiMeter);
		return stiMeter;
	}

	public override StiMeter Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiMeter;
	}
}
