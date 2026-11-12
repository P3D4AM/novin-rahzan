using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report.Design;

internal class StiComponentsByPageTypeFiltrator
{
	internal static List<StiComponent> Filter(StiPage currentPage, List<StiComponent> components)
	{
		if (currentPage.IsDashboard)
		{
			return components.Where((StiComponent c) => c is IStiElement).ToList();
		}
		if (currentPage.IsForm)
		{
			return components.Where((StiComponent c) => c is IStiReportControl).ToList();
		}
		if (currentPage.IsScreen)
		{
			return components.Where((StiComponent c) => c is IStiComponentUI).ToList();
		}
		return components.Where((StiComponent c) => !(c is IStiElement) && !(c is IStiReportControl)).ToList();
	}
}
