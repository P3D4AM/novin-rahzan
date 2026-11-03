using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiDoughnutSeries : IStiPieSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiAllowApplyBorderColor, IStiAllowApplyBrush, IStiShowZerosSeries
{
	float Width { get; set; }
}
