using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarAxis : ICloneable, IStiJsonReportObject
{
	StiRadarAxisCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	IStiRadarArea Area { get; set; }

	bool Visible { get; set; }

	IStiAxisRange Range { get; set; }
}
