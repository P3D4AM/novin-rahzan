using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Gauge;

public interface IStiGaugeStyle : ICloneable, IStiJsonReportObject
{
	StiGaugeStyleCoreXF Core { get; set; }
}
