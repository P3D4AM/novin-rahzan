using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiPieSeriesLabels : IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	bool ShowInPercent { get; set; }
}
