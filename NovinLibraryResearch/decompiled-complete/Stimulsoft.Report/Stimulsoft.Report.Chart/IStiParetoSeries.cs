using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiParetoSeries : IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries, IStiBaseLineSeries, IStiAllowApplyColorNegative, IStiShowNullsSeries
{
	bool AllowApplyLineColor { get; set; }
}
