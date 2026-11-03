using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.App;

public interface IStiButtonUI : IStiDashboardElementStyle, IStiCornerRadius, IStiSimpleShadow
{
	string Text { get; set; }
}
