using System.Collections.Generic;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Options;

public sealed class StiDashboardElementsLoader
{
	public static List<StiComponent> FetchAll()
	{
		return StiDashboardOptions.Services.Elements;
	}

	public static void Load()
	{
		StiOptions.Services.Components.AddRange(FetchAll());
	}
}
