using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiBoxAndWhiskerSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiAllowApplyBrush
{
	StiBrush Brush { get; set; }

	bool ShowInnerPoints { get; set; }

	bool ShowMeanMarkers { get; set; }
}
