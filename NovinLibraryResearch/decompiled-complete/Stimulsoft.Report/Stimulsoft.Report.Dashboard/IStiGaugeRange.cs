using System.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiGaugeRange
{
	Color Color { get; set; }

	double Start { get; set; }

	double End { get; set; }
}
