using System;
using System.Reflection;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.Dashboard.Helper;

public class StiTitleMeasureHelper
{
	public static SizeD? MeasureTitle(IStiTitle title)
	{
		try
		{
			if (StiDashboardAssembly.Assembly == null)
			{
				return null;
			}
			Type type = StiDashboardAssembly.Assembly.GetType("Stimulsoft.Dashboard.Helpers.StiTitleMeasureHelper");
			if (type == null)
			{
				return null;
			}
			MethodInfo method = type.GetMethod("MeasureTitle", BindingFlags.Static | BindingFlags.Public, null, new Type[1] { title.GetType() }, null);
			if (method != null)
			{
				return (SizeD)method.Invoke(null, new object[1] { title });
			}
		}
		catch
		{
		}
		return null;
	}
}
