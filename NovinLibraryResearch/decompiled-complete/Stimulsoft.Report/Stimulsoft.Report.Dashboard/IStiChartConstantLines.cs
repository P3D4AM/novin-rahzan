using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiChartConstantLines
{
	string Text { get; set; }

	StiPenStyle LineStyle { get; set; }

	Color LineColor { get; set; }

	string AxisValue { get; set; }

	float LineWidth { get; set; }
}
