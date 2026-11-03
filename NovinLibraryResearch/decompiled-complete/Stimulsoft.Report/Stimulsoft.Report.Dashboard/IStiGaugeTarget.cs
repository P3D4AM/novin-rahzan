using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Report.Dashboard;

public interface IStiGaugeTarget
{
	bool ShowLabel { get; set; }

	StiPlacement Placement { get; set; }
}
