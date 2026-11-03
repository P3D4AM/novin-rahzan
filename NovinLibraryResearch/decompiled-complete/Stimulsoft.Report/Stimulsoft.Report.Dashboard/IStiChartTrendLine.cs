using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiChartTrendLine
{
	string KeyValueMeter { get; set; }

	StiChartTrendLineType Type { get; set; }

	StiPenStyle LineStyle { get; set; }

	Color LineColor { get; set; }

	float LineWidth { get; set; }
}
