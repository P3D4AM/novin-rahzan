using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiHistogramSeries : IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
}
