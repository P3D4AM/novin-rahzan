using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiXRadarAxis : IStiRadarAxis, ICloneable, IStiJsonReportObject
{
	StiXRadarAxisCoreXF XCore { get; }

	IStiRadarAxisLabels Labels { get; set; }
}
