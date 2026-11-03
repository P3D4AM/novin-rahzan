using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiStackedSplineSeries : IStiStackedBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries, IStiAllowApplyColorNegative
{
	float Tension { get; set; }
}
