using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Base.Gis;

public sealed class StiGisMapData
{
	public Size Size { get; set; }

	public Color GeometryColor { get; set; }

	public double GeometryLineSize { get; set; }

	public List<string> Commands { get; set; }

	public List<string> Colors { get; set; }

	public List<object> LineSizes { get; set; }

	public List<string> Descriptions { get; set; }

	public bool ShowPlacemark { get; set; }

	public StiLanguageType Language { get; set; }

	public StiFontIcons Icon { get; }

	public Color IconColor { get; }

	public StiGisMapData(StiFontIcons icon, Color iconColor)
	{
		Icon = icon;
		IconColor = iconColor;
	}
}
