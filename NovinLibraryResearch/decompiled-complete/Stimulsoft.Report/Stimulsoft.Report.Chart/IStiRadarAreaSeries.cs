using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarAreaSeries : IStiRadarLineSeries, IStiRadarSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	StiBrush Brush { get; set; }
}
