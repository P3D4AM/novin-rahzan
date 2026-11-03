using System;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiOutsideAxisLabels : IStiAxisSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	float LineLength { get; set; }

	Color LineColor { get; set; }
}
