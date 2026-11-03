using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarLineSeries : IStiRadarSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	Color LineColor { get; set; }

	StiPenStyle LineStyle { get; set; }

	bool Lighting { get; set; }

	float LineWidth { get; set; }
}
