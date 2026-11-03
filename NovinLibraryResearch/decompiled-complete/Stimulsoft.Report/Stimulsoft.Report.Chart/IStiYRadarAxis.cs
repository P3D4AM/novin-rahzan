using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiYRadarAxis : IStiRadarAxis, ICloneable, IStiJsonReportObject
{
	StiYRadarAxisCoreXF YCore { get; }

	IStiAxisLabels Labels { get; set; }

	IStiAxisTicks Ticks { get; set; }

	StiPenStyle LineStyle { get; set; }

	Color LineColor { get; set; }

	float LineWidth { get; set; }

	StiAxisInfoXF Info { get; set; }
}
