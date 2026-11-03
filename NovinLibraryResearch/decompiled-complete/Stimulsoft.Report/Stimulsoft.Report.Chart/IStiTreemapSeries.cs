using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiTreemapSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiFontIconsSeries
{
	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }
}
