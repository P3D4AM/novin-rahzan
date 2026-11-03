using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisArea : IStiArea, ICloneable, IStiJsonReportObject
{
	StiAxisAreaCoreXF AxisCore { get; }

	IStiInterlacingHor InterlacingHor { get; set; }

	IStiInterlacingVert InterlacingVert { get; set; }

	IStiGridLinesHor GridLinesHor { get; set; }

	IStiGridLinesHor GridLinesHorRight { get; set; }

	IStiGridLinesVert GridLinesVert { get; set; }

	IStiYAxis YAxis { get; set; }

	IStiYAxis YRightAxis { get; set; }

	IStiXAxis XAxis { get; set; }

	IStiXAxis XTopAxis { get; set; }

	bool ReverseHor { get; set; }

	bool ReverseVert { get; set; }
}
