using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiLineSeries : IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
}
