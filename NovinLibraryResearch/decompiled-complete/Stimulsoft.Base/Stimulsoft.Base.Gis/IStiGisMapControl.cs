using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Base.Gis;

public interface IStiGisMapControl
{
	double Zoom { get; set; }

	bool AllowLocalCache { get; set; }

	void SetProviderType(StiGeoMapProviderType type, Color geometryColor, double geometryLineSize, bool showPlacemark, StiLanguageType language, StiFontIcons icon, Color iconColor);

	void InitGeoCommands(List<string> commands, List<string> colors, List<object> lineSizes, List<string> descriptions);
}
