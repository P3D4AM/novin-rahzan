using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiScatterSplineSeries : IStiScatterLineSeries, IStiScatterSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	float Tension { get; set; }
}
