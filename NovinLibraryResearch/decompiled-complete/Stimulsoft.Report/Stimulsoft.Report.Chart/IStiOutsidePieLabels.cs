using System;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiOutsidePieLabels : IStiCenterPieLabels, IStiPieSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	bool ShowValue { get; set; }

	float LineLength { get; set; }

	Color LineColor { get; set; }
}
