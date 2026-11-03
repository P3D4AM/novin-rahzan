using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiElementScale
{
	public static double Factor(IStiElement element)
	{
		if (element?.Page == null)
		{
			return 1.0;
		}
		return element.Page.Zoom;
	}

	public static double Factor(IStiComponentUI comp)
	{
		if (comp?.Page == null)
		{
			return 1.0;
		}
		return comp.Page.Zoom;
	}

	public static double Factor(StiComponent component)
	{
		if (component?.Page == null)
		{
			return 1.0;
		}
		return component.Page.Zoom;
	}
}
