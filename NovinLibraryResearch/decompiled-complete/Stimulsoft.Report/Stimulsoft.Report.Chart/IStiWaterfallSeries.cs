using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

internal interface IStiWaterfallSeries : IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
	IStiWaterfallConnectorLine ConnectorLine { get; set; }

	IStiWaterfallTotal Total { get; set; }
}
