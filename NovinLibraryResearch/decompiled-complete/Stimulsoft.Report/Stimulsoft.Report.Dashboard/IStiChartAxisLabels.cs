using System.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiChartAxisLabels
{
	float Angle { get; set; }

	Color Color { get; set; }

	Font Font { get; set; }

	string TextAfter { get; set; }

	string TextBefore { get; set; }

	float Step { get; set; }
}
