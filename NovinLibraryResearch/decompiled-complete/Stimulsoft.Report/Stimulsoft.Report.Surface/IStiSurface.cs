using System.Collections.Generic;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Surface;

public interface IStiSurface
{
	int DeviceWidth { get; set; }

	bool IsMobileSurfacePresent { get; }

	bool IsMobileSurfaceSelected { get; }

	bool IsDesktopSurfaceSelected { get; }

	double Width { get; set; }

	double Height { get; set; }

	List<StiComponent> GetUnplacedElements();

	void SwitchSurfaceViewMode(StiSurfaceViewMode value);

	void RemoveMobileSurface();
}
