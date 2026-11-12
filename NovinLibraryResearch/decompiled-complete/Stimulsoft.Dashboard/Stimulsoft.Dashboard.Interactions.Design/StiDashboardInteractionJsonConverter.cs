using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Dashboard.Interactions.Design;

internal class StiDashboardInteractionJsonConverter : CustomCreationConverter<StiDashboardInteraction>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		JToken jToken = JToken.ReadFrom(reader);
		string identName = jToken.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "Ident").Value.ToString();
		StiDashboardInteraction stiDashboardInteraction = StiDashboardInteractionCreator.New(identName);
		if (stiDashboardInteraction == null)
		{
			return null;
		}
		serializer.Populate(jToken.CreateReader(), stiDashboardInteraction);
		return stiDashboardInteraction;
	}

	public override StiDashboardInteraction Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiDashboardInteraction;
	}
}
