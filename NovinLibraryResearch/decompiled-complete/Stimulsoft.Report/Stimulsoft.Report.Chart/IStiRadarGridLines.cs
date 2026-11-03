using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarGridLines : ICloneable, IStiJsonReportObject
{
	StiRadarGridLinesCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	Color Color { get; set; }

	StiPenStyle Style { get; set; }

	bool Visible { get; set; }

	IStiArea Area { get; set; }
}
