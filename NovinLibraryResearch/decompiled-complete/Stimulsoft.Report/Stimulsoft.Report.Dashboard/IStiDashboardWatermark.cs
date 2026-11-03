using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiDashboardWatermark
{
	StiAdvancedWatermark DashboardWatermark { get; set; }

	IStiApp GetApp();
}
