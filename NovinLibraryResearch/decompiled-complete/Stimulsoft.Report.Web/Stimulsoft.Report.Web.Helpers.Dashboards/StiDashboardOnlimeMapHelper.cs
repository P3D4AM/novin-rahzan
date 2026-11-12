using System;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDashboardOnlimeMapHelper
{
	public static string GetBingMapScript(IStiElement element, bool showTitle)
	{
		return (string)StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard", "Helpers.StiOnlineMapHelper", "GetBingMapScript", new object[2] { element, showTitle }, new Type[2]
		{
			typeof(IStiElement),
			typeof(bool)
		});
	}
}
