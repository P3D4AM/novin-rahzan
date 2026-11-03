using System.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiOnlineMapGisSettings
{
	Color Color { get; set; }

	double Size { get; set; }

	bool ShowPlacemark { get; set; }
}
