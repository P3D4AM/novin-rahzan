using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiCenterPieLabels : IStiPieSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	bool AutoRotate { get; set; }
}
