using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Report.Dashboard;

public interface IStiGaugeLabels
{
	bool Visible { get; set; }

	StiLabelPlacement Placement { get; set; }

	StiLabelRotationMode RotationMode { get; set; }
}
