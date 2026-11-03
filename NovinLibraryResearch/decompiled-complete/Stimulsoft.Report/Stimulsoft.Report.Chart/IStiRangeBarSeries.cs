using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiRangeBarSeries : IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries, IStiRangeSeries
{
}
