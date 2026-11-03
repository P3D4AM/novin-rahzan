using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Surface;

namespace Stimulsoft.Report.Dashboard;

public interface IStiDashboard : IStiSurface, IStiPanel, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiBackColor, IStiDashboardElementStyle
{
	StiDashboardContentAlignment ContentAlignment { get; set; }

	List<StiDataFilterRule> GetUserFilters(IStiElement element);
}
