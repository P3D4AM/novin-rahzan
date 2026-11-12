using System;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Dashboard.Interactions.Design;

internal class StiDashboardDrillDownParameterJsonConverter : CustomCreationConverter<StiDashboardDrillDownParameter>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		JToken jToken = JToken.ReadFrom(reader);
		StiDashboardDrillDownParameter stiDashboardDrillDownParameter = new StiDashboardDrillDownParameter();
		if (stiDashboardDrillDownParameter == null)
		{
			return null;
		}
		serializer.Populate(jToken.CreateReader(), stiDashboardDrillDownParameter);
		return stiDashboardDrillDownParameter;
	}

	public override StiDashboardDrillDownParameter Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiDashboardDrillDownParameter;
	}
}
