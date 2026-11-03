using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiScatterSeries : IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
}
