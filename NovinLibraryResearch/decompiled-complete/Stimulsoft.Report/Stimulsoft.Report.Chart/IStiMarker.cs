using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiMarker : ICloneable, IStiFontIconsSeries, IStiJsonReportObject, IStiDefault
{
	StiMarkerCoreXF Core { get; set; }

	bool ShowInLegend { get; set; }

	bool Visible { get; set; }

	StiBrush Brush { get; set; }

	Color BorderColor { get; set; }

	float Size { get; set; }

	float Angle { get; set; }

	StiMarkerType Type { get; set; }
}
