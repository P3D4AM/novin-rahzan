using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiStackedBaseLineSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	IStiMarker Marker { get; set; }

	IStiLineMarker LineMarker { get; set; }

	bool Lighting { get; set; }

	Color LineColor { get; set; }

	float LineWidth { get; set; }

	StiPenStyle LineStyle { get; set; }
}
