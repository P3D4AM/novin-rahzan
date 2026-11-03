using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiWaterfallConnectorLine : ICloneable, IStiJsonReportObject
{
	Color LineColor { get; set; }

	StiPenStyle LineStyle { get; set; }

	float LineWidth { get; set; }

	bool Visible { get; set; }
}
