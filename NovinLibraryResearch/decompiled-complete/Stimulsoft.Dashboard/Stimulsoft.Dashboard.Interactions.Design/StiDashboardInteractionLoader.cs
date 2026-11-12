using System.Linq;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Dashboard.Interactions.Design;

public static class StiDashboardInteractionLoader
{
	internal static StiDashboardInteraction LoadInteractionFromJsonObject(JObject jObject)
	{
		string identName = jObject.Properties().FirstOrDefault((JProperty x) => x.Name == "Ident").Value.ToObject<string>();
		StiDashboardInteraction stiDashboardInteraction = StiDashboardInteractionCreator.New(identName);
		stiDashboardInteraction.LoadFromJsonObject(jObject);
		return stiDashboardInteraction;
	}
}
