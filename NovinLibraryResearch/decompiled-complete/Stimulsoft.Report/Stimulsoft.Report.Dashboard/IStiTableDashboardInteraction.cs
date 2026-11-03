using System;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Dashboard;

public interface IStiTableDashboardInteraction : IStiDashboardInteraction, ICloneable, IStiDefault
{
	bool AllowUserSorting { get; set; }

	bool AllowUserFiltering { get; set; }

	bool DrillDownFiltered { get; set; }

	bool FullRowSelect { get; set; }
}
