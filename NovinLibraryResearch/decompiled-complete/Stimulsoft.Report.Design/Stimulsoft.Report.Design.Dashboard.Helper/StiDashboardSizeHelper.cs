using System;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.Dashboard.Helper;

public class StiDashboardSizeHelper
{
	public static void Place(IStiDashboard dashboard, double prevWidth, double prevHeight)
	{
		try
		{
			if (StiDashboardAssembly.Assembly == null)
			{
				return;
			}
			Type type = StiDashboardAssembly.Assembly.GetType("Stimulsoft.Dashboard.Helpers.StiDashboardSizeHelper");
			if (!(type == null))
			{
				MethodInfo method = type.GetMethod("Place", BindingFlags.Static | BindingFlags.Public, null, new Type[3]
				{
					dashboard.GetType(),
					prevWidth.GetType(),
					prevHeight.GetType()
				}, null);
				if (method != null)
				{
					method.Invoke(null, new object[3] { dashboard, prevWidth, prevHeight });
				}
			}
		}
		catch
		{
		}
	}
}
