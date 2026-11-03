using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiSplineSeries : IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	float Tension { get; set; }
}
