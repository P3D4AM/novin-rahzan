using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiFunnelSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiFontIconsSeries, IStiShowZerosSeries
{
	bool AllowApplyBrush { get; set; }

	bool AllowApplyBorderColor { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }
}
