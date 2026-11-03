using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiClusteredBarSeries : IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
}
