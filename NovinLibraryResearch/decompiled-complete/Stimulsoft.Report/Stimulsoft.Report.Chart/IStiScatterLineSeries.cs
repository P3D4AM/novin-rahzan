using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiScatterLineSeries : IStiScatterSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
}
