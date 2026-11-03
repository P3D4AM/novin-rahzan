using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarArea : IStiArea, ICloneable, IStiJsonReportObject
{
	StiRadarStyle RadarStyle { get; set; }

	IStiXRadarAxis XAxis { get; set; }

	IStiYRadarAxis YAxis { get; set; }

	IStiInterlacingHor InterlacingHor { get; set; }

	IStiInterlacingVert InterlacingVert { get; set; }

	IStiRadarGridLinesHor GridLinesHor { get; set; }

	IStiRadarGridLinesVert GridLinesVert { get; set; }
}
