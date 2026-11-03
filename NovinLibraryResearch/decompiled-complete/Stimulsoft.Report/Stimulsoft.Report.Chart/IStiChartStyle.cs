using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiChartStyle : ICloneable, IStiJsonReportObject
{
	StiStyleCoreXF Core { get; set; }
}
