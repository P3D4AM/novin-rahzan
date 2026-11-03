using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisSeriesLabels : IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	bool ShowInPercent { get; set; }
}
