using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiGanttSeries : IStiClusteredBarSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries, IStiRangeSeries
{
}
