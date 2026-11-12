using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Dashboard.Components.Helpers;

public static class StiMeterLoader
{
	public static IEnumerable<StiMeter> LoadFromJson(IEnumerable<JObject> jsons)
	{
		List<StiMeter> meters = new List<StiMeter>();
		jsons.ToList().ForEach(delegate(JObject a)
		{
			meters.Add(LoadFromJson(a));
		});
		return meters.Where((StiMeter m) => m != null);
	}

	public static StiMeter LoadFromJson(JObject json)
	{
		JProperty jProperty = json.Properties().FirstOrDefault((JProperty x) => x.Name == "Ident");
		string identName = jProperty.Value.ToObject<string>();
		StiMeter stiMeter = StiMeterCreator.New(identName);
		stiMeter.LoadFromJsonObject(json);
		return stiMeter;
	}

	public static StiMeter LoadFromJson(string str)
	{
		return LoadFromJson(JObject.Parse(str));
	}
}
