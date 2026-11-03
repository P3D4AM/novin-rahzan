using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiStackedBarSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiFontIconsSeries, IStiAllowApplyBrushNegative, IStiShowZerosSeries
{
	float Width { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }
}
