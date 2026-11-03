using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiSeriesInteraction : ICloneable, IStiJsonReportObject
{
	bool DrillDownEnabled { get; set; }

	bool AllowSeries { get; set; }

	bool AllowSeriesElements { get; set; }
}
