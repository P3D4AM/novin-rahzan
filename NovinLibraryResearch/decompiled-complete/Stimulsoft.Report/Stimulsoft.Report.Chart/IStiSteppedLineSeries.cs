using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiSteppedLineSeries : IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	bool PointAtCenter { get; set; }
}
