using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiIndicatorIconRange
{
	StiFontIcons Icon { get; set; }

	string StartExpression { get; set; }

	string EndExpression { get; set; }
}
