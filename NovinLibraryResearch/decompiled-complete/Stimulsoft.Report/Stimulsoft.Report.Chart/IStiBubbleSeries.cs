using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiBubbleSeries : IStiScatterSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries, IStiSeriesBorderThickness
{
	double[] Weights { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }
}
